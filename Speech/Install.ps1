$Script:ModuleName = 'Erwine.Leonard.T.Speech';
$Script:InstallRoot = $MyInvocation.MyCommand.Definition | Split-Path -Parent;
$Script:SourceManifestPath = $Script:InstallRoot | Join-Path -ChildPath "$Script:ModuleName.psd1";
if (-not $Script:SourceManifestPath | Test-Path -PathType Leaf) {
    Write-Error -Message 'Module manifest file not found' -Category ObjectNotFound -TargetObject $Script:SourceManifestPath;
    return;
}
$Script:NewModuleManifest = Test-ModuleManifest -Path $Script:SourceManifestPath -ErrorAction Continue;
if ($Script:NewModuleManifest -eq $null) {
    Write-Error -Message 'Unable to load module manifest to be installed. Cannot continue.' -Category ReadError -TargetObject $Script:SourceManifestPath;
    return;
}
Function Get-SpecialFolderPath {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Environment+SpecialFolder]$SpecialFolder
    )
    Process {
        $Path = [System.Environment]::GetFolderPath($SpecialFolder);
        if (-not [System.String]::IsNullOrEmpty($Path)) { [System.IO.Path]::GetFullPath($Path) }
    }
}

$AllUsersBasePaths = @(([System.Environment+SpecialFolder]::CommonApplicationData,
    [System.Environment+SpecialFolder]::CommonDesktopDirectory,
    [System.Environment+SpecialFolder]::CommonProgramFiles,
    [System.Environment+SpecialFolder]::CommonProgramFilesX86,
    [System.Environment+SpecialFolder]::CommonPrograms,
    [System.Environment+SpecialFolder]::ProgramFiles,
    [System.Environment+SpecialFolder]::ProgramFilesX86,
    [System.Environment+SpecialFolder]::Programs) | Get-SpecialFolderPath | Select-Object -Unique);
$CurrentUserBasePaths = @(([System.Environment+SpecialFolder]::ApplicationData,
    [System.Environment+SpecialFolder]::LocalApplicationData,
    [System.Environment+SpecialFolder]::MyDocuments,
    [System.Environment+SpecialFolder]::Personal) | Get-SpecialFolderPath | Select-Object -Unique);

$Script:AvailableModules = @(Get-Module -ListAvailable | ForEach-Object {
    $ManifestPath = [System.IO.Path]::GetFullPath($_.Path);
    $InstallLocation = $ManifestPath | Split-Path -Parent;
    New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
        ManifestPath = $ManifestPath;
        InstallLocation = $InstallLocation;
        InstallRoot = $InstallLocation | Split-Path -Parent;
        ModuleInfo = $_;
    };
});

$Script:TargetLocations = @($env:PSModulePath.Split([System.IO.Path]::PathSeparator) | ForEach-Object {
    $Path = [System.IO.Path]::GetFullPath($_);
    $Properties = @{
        Directory = New-Object -TypeName 'System.IO.DirectoryInfo' -ArgumentList $Path;
        IsInModulePath = $true;
    };
    if (($CurrentUserBasePaths | Where-Object { $Path.Length -ge $Path -and [String.Equals]::($Path, $_.Substring(0, $Path.Length), [StringComparison]::InvariantCultureIgnoreCase) }) -ne $null) {
        $Properties['Level'] = 2;
    } else {
        if (($AllUsersBasePaths | Where-Object { $Path.Length -ge $Path -and [String.Equals]::($Path, $_.Substring(0, $Path.Length), [StringComparison]::InvariantCultureIgnoreCase) }) -ne $null) {
            $Properties['Level'] = 1;
        } else {
            $Properties['Level'] = 0;
        }
    }
    $Properties | Write-Output;
});
if (($Script:TargetLocations | Where-Object { $_['Level'] -eq 2 }) -eq $null) {
    $Script:TargetLocations = $Script:TargetLocations + @(@{
        Directory = New-Object -TypeName 'System.IO.DirectoryInfo' -ArgumentList ([System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::ProgramFiles) | Join-Path -ChildPath 'WindowsPowerShell\Modules');
        IsInModulePath = $false;
        Level = 2;
    });
}
if (($Script:TargetLocations | Where-Object { $_['Level'] -eq 1 }) -eq $null) {
    $Script:TargetLocations = $Script:TargetLocations + @(@{
        Directory = New-Object -TypeName 'System.IO.DirectoryInfo' -ArgumentList ([System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Personal) | Join-Path -ChildPath 'WindowsPowerShell\Modules');
        IsInModulePath = $false;
        Level = 1;
    });
}
if (($Script:TargetLocations | Where-Object { $_['Level'] -eq 1 }) -eq $null) {
    $Script:TargetLocations = $Script:TargetLocations + @(@{
        Directory = New-Object -TypeName 'System.IO.DirectoryInfo' -ArgumentList ($PSHOME | Join-Path -ChildPath 'Modules');
        IsInModulePath = $false;
        Level = 0;
    });
}
$Script:TargetLocations | ForEach-Object {
    $Path = $_['Directory'].FullName;
    $ExistingModules = @($Script:AvailableModules | Where-Object { $_.InstallRoot -ieq $Path });
    if ($ExistingModules.Count -eq 0) {
        $_['ExistingModule'] = $null;
    } else {
        $_['ExistingModule'] = $ExistingModules[0];
    }
}



Function Get-SpecialFolderPaths
{
  Param(
    [Parameter(Mandatory = $True, ValueFromPipeline = $True)]
    [System.Environment+SpecialFolder]$SpecialFolder
  )
  Process {
    $Path = [System.Environment+SpecialFolder]::GetFolderPath($_);
    if (-not [System.String]::IsNullOrEmpty($Path)) { [System.IO.Path]::GetFullPath($Path) }
  }
}

$AllUsersFolders = @([System.Environment+SpecialFolder]::CommonAdminTools, [System.Environment+SpecialFolder]::CommonApplicationData,
  [System.Environment+SpecialFolder]::CommonProgramFiles, [System.Environment+SpecialFolder]::CommonProgramFilesX86,
  [System.Environment+SpecialFolder]::CommonPrograms, [System.Environment+SpecialFolder]::ProgramFiles,
  [System.Environment+SpecialFolder]::ProgramFilesX86, [System.Environment+SpecialFolder]::Programs) | Get-SpecialFolderPaths);
$CurrentUserFolders = @([System.Environment+SpecialFolder]::MyDocuments, [System.Environment+SpecialFolder]::Personal,
  [System.Environment+SpecialFolder]::UserProfile, [System.Environment+SpecialFolder]::LocalApplicationData,
  [System.Environment+SpecialFolder]::ApplicationData) | Get-SpecialFolderPaths);
  
$InstallLocations = $PSModulePath.Split([System.IO.Path]::DirectorySeparatorChar) | ForEach-Object {
  $Path =  [System.IO.Path]::GetFullPath($_);
  if (($CurrentUserFolders | Where-Object {
    $Path.Length -ge $_.Length -and [System.String]::Equals($_, $Path.Substring(0, $_.Length), [System.StringCompare]::InvariantCultureIgnoreCase)
  }) -ne $null) {
    New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
      Path = $Path;
      Type = 'Current User';
    };
  } else {
    if (($AllUsersFolders | Where-Object {
      $Path.Length -ge $_.Length -and [System.String]::Equals($_, $Path.Substring(0, $_.Length), [System.StringCompare]::InvariantCultureIgnoreCase)
    }) -ne $null) {
      New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
        Path = $Path;
        Type = 'All Users';
      };
    } else {
      New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
        Path = $Path;
        Type = 'Admin Users';
      };
    }
  }
};
if (@($InstallLocations | Where-Object { $_.Type -eq 'Admin Users' }).Count -eq 0)) {
  # TODO: Add Admin Users
  $PSHOME
}
if (@($InstallLocations | Where-Object { $_.Type -eq 'Current User' }).Count -eq 0)) {
  # TODO: Add Current User
  $HOME
}
if (@($InstallLocations | Where-Object { $_.Type -eq 'All Users' }).Count -eq 0)) {
  # TODO: Add All Users
  $AllUsersFolders[0]
}
$Choices = New-Object -TypeName 'System.Collections.ObjectModel.Collection[System.Management.Automation.Host.ChoiceDescription]' -ArgumentList $Label, $Message;
