$Script:ExtensionsToRename = @{
    '.cs' = @{ Description = 'CSharp Source File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Plain; }
    '.ps1' = @{ Description = 'PowerShell Script File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Plain; }
    '.psd1' = @{ Description = 'PowerShell Module Manifest'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Plain; }
    '.psm1' = @{ Description = 'PowerShell Module File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Plain; }
    '.bat' = @{ Description = 'DOS Batch File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Plain; }
    '.cmd' = @{ Description = 'Command-Line Batch File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Plain; }
    '.gitattributes' = @{ Description = 'GIT Settings File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Plain; }
    '.gitignore' = @{ Description = 'GIT Settings File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Plain; }
    '.sln' = @{ Description = 'Visual Studio Solution File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Xml; }
    '.md' = @{ Description = 'GIT markdown documentation file'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Plain; }
    '.config' = @{ Description = 'Configuration file'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Xml; }
    '.pssproj' = @{ Description = 'PowerShell Project File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Xml; }
    '.cd' = @{ Description = 'Class Diagram File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Xml; }
    '.csproj' = @{ Description = 'CSharp Project File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Xml; }
    '.resx' = @{ Description = 'Resource Definition File'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Xml; }
};
$Script:NamesToRename = @{
    'license' = @{ Description = 'Software License'; MediaType = [System.Net.Mime.MediaTypeNames+Text]::Plain; }
}
$Script:MimeTypeMappings = @(
    @{ Value = 'text/plain'; Description = 'Plain Text' },
    @{ Value = 'text/'; Description = 'Text File' }
);

Import-Module 'Erwine.Leonard.T.IOUtility' -ErrorAction Stop;
Import-Module 'Erwine.Leonard.T.XmlUtility' -ErrorAction Stop;
Add-Type -Assembly 'System.Web' -ErrorAction Stop;

$Script:PreviousVerbosePreference = $VerbosePreference;

Function Write-Information {
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [AllowEmptyString()]
        [string[]]$Message
    )
    
    Begin { $VerbosePreference = [System.Management.Automation.ActionPreference]::Continue }
    
    Process { $Message | Write-Verbose }
    
    End { $VerbosePreference = $Script:PreviousVerbosePreference; }
}

Function Convert-SourceFiles {
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [System.IO.DirectoryInfo[]]$Source,
        
        [Parameter(Mandatory = $true, Position = 1)]
        [System.IO.DirectoryInfo]$Target
    )
    
    Begin {
        $MapMethod = ([System.Web.HttpUtility].Assembly.GetTypes() | Where-Object { $_.FullName -eq 'System.Web.MimeMapping' }).GetMethod('GetMimeMapping', ([System.Reflection.BindingFlags]([System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Static)));
    }
    Process {
        foreach ($sourceDirectory in $Source) {
            $targetDirectory = New-Object -TypeName 'System.IO.DirectoryInfo' -ArgumentList ($Target.FullName | Join-Path -ChildPath $sourceDirectory.Name);
            if (-not $targetDirectory.Exists) { $targetDirectory.Create(); $targetDirectory.Refresh(); }
            
            foreach ($fileInfo in $sourceDirectory.GetFiles()) {
                $XmlElement = Add-XmlElement -ParentElement $Script:MappingDocument.DocumentElement -Name 'Mapping';
                try {
                    $mimeType = $MapMethod.Invoke($null, $fileInfo.Name);
                } catch {
                    $mimeType = $null;
                }
                if ([System.String]::IsNullOrEmpty($mimeType)) { $mimeType = [System.Net.Mime.MediaTypeNames+Application]::Octet }
                if ($Script:ExtensionsToRename.Keys -icontains $fileInfo.Extension) {
                    $targetName = '{0}-{1}.txt' -f [System.IO.Path]::GetFileNameWithoutExtension($fileInfo.Name), $fileInfo.Extension.Substring(1);
                    $targetPath = $sourceDirectory.Fullname | Join-Path -ChildPath $targetName;
                    $i = 0;
                    while ([System.IO.File]::Exists($targetPath) -or [System.IO.Directory]::Exists($targetPath)) {
                        $targetName = '{0}-{1}{2}.txt' -f [System.IO.Path]::GetFileNameWithoutExtension($fileInfo.Name), $fileInfo.Extension.Substring(1), $i;
                        $targetPath = $sourceDirectory.Fullname | Join-Path -ChildPath $targetName;
                        $i++;
                    }
                    $isRenamed = $true;
                    $description = $Script:ExtensionsToRename[$fileInfo.Extension.ToLower()].Description;
                    if ($mimeType -eq [System.Net.Mime.MediaTypeNames+Application]::Octet) { $mimeType = $Script:ExtensionsToRename[$fileInfo.Extension.ToLower()].MediaType }
                } else {
                    if ($Script:NamesToRename.Keys -icontains $fileInfo.Name) {
                        if ([System.String]::IsNullOrEmpty($fileInfo.Extension)) {
                            $targetName = '{0}.txt' -f [System.IO.Path]::GetFileNameWithoutExtension($fileInfo.Name);
                        } else {
                            $targetName = '{0}-{1}.txt' -f [System.IO.Path]::GetFileNameWithoutExtension($fileInfo.Name), $fileInfo.Extension.Substring(1);
                        }
                        $targetPath = $sourceDirectory.Fullname | Join-Path -ChildPath $targetName;
                        $i = 0;
                        while ([System.IO.File]::Exists($targetPath) -or [System.IO.Directory]::Exists($targetPath)) {
                            if ([System.String]::IsNullOrEmpty($fileInfo.Extension)) {
                                $targetName = '{0}{1}.txt' -f [System.IO.Path]::GetFileNameWithoutExtension($fileInfo.Name), $i;
                            } else {
                                $targetName = '{0}-{1}{2}.txt' -f [System.IO.Path]::GetFileNameWithoutExtension($fileInfo.Name), $fileInfo.Extension.Substring(1), $i;
                            }
                            $targetPath = $sourceDirectory.Fullname | Join-Path -ChildPath $targetName;
                            $i++;
                        }
                        $isRenamed = $true;
                        $description = $Script:NamesToRename[$fileInfo.Name.ToLower()].Description;
                        if ($mimeType -eq [System.Net.Mime.MediaTypeNames+Application]::Octet) { $mimeType = $Script:NamesToRename[$fileInfo.Name.ToLower()].MediaType }
                    } else {
                        $isRenamed = $false;
                        $targetName = $fileInfo.Name;
                        $description = $null;
                        foreach ($m in $Script:MimeTypeMappings) {
                            if ($mimeType.StartsWith($m.Value)) {
                                $description = $m.Description;
                                break;
                            }
                        }
                        
                        if ($description -eq $null) { $description = 'Unknown type' }
                    }
                };
                Add-XmlAttribute -XmlElement $XmlElement -Value $fileInfo.Name -Name 'Name';
                Add-XmlAttribute -XmlElement $XmlElement -Value $description -Name 'Description';
                Add-XmlAttribute -XmlElement $XmlElement -Value $mimeType -Name 'MimeType';
                Add-XmlAttribute -XmlElement $XmlElement -Value ([System.Xml.XmlConvert]::ToString($isRenamed)) -Name 'IsRenamed';
                $targetPath = $targetDirectory.Fullname | Join-Path -ChildPath $targetName;
                $relativePath = $targetPath.Substring($Script:TargetPath.Length);
                if ($relativePath.StartsWith('\')) { $relativePath = $relativePath.Substring(1) }
                Set-XmlText -XmlElement $XmlElement -InnerText $relativePath;
                $targetBeforeRename = $targetDirectory.Fullname | Join-Path -ChildPath $fileInfo.name;
                $targetPath = $targetDirectory.Fullname | Join-Path -ChildPath $targetName;
                $relativePath = '{0} => {1}' -f $fileInfo.FullName.Substring($Script:SourcePath.Length), $relativePath;
                if ($relativePath.StartsWith('\')) { $relativePath = $relativePath.Substring(1) }
                $relativePath | Write-Information;
                $targetPath = $targetDirectory.Fullname | Join-Path -ChildPath $targetName;
                [System.IO.File]::Copy($fileInfo.FullName, $targetPath);
                #[System.IO.File]::Copy($fileInfo.FullName, $targetDirectory.Fullname);
                #if ($targetBeforeRename -ne $targetPath) {
                #    [System.IO.File]::Move($targetBeforeRename, $targetPath);
                #}
            }
            
            $d = $sourceDirectory.GetDirectories();
            if($d.Count -gt 0) { $d | Convert-SourceFiles -Target $targetDirectory.FullName }
        }
    }
}

'Open folder dialog is open in another window.' | Write-Information;
$Script:SourcePath = Read-FileDialog -SelectedPath ([System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Personal)) -Description 'Select Source' -Folder
'Open folder dialog closed.' | Write-Information;
if ($Script:SourcePath -eq $null) {
    Write-Warning -Message 'Aborted.';
    return;
}
'Open folder dialog is open in another window.' | Write-Information;
$Script:TargetPath = Read-FileDialog -SelectedPath ([System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Personal)) -ShowNewFolderButton $true -Description 'Select Target' -Folder
'Open folder dialog closed.' | Write-Information;
if ($Script:TargetPath -eq $null) {
    Write-Warning -Message 'Aborted.';
    return;
}

[xml]$Script:MappingDocument = '<FileNameMappings />';
Convert-SourceFiles -Source $SourcePath -Target $TargetPath;

$XmlWriterSettings = New-XmlWriterSettings -Indent $true;
$xmlName = 'FileMapping.xml';
$xmlPath = $Script:TargetPath | Join-Path -ChildPath $xmlName;
$i = 0;
while ([System.IO.File]::Exists($xmlPath) -or [System.IO.Directory]::Exists($xmlPath)) {
    $xmlName = 'FileMapping{0}.xml' -f $i;
    $xmlPath = $Script:TargetPath | Join-Path -ChildPath $xmlName;
    $i++;
}
Write-XmlDocument -Document $Script:MappingDocument -OutputFileName $xmlPath -Settings $XmlWriterSettings;
