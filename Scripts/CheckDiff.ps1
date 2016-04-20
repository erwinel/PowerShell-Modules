Import-Module -Name 'Erwine.Leonard.T.IOUtility';

Function Test-FileStreamComparison {
    Param(
        [Parameter(Mandatory = $true)]
        [System.IO.FileStream]$Master,
        [Parameter(Mandatory = $true)]
        [System.IO.FileStream]$Target
    )
    
    $MasterBytes = New-Object -TypeName 'System.Byte[]' -ArgumentList 8192;
    $TargetBytes = New-Object -TypeName 'System.Byte[]' -ArgumentList 8192;
    do {
        $MasterCount = $MasterFileStream.Read($MasterBytes, 0, $MasterBytes.Length);
        $TargetCount = $TargetFileStream.Read($TargetBytes, 0, $MasterBytes.Length);
        if ($MasterCount -ne $TargetCount) {
            return $false
        }
        for ($i = 0; $i -lt $MasterCount; $i++) {
            if ($MasterBytes[$i] -ne $TargetBytes[$i]) {
                return $false
            }
        }
    } while ($TargetCount -gt 0);
    return $true;
}

Function Test-FileInfoComparison {
    Param(
        [Parameter(Mandatory = $true)]
        [System.IO.FileInfo]$Master,
        [Parameter(Mandatory = $true)]
        [System.IO.FileInfo]$Target
    )
    
    if ($Master.Length -ne $Target.Length) {
        return $false
    }
    
    $success = $null;
    $MasterFileStream = $Master.OpenRead();
    try {
        $TargetFileStream = $Target.OpenRead();
        try {
            $success = Test-FileStreamComparison -Master $MasterFileStream -Target $TargetFileStream;
        } catch {
            throw;
        } finally {
            $TargetFileStream.Dispose();
        }
    } catch {
        Write-Warning -Message ('Unable to compare "{0}" to "{1}".' -f $Master.FullName, $Target.FullName);
        throw;
    } finally {
        $MasterFileStream.Dispose();
    }
    
    if ($success -ne $null) { $success | Write-Output }
}

Add-Type -TypeDefinition @'
namespace CheckDiffCLR {
    using System;
    using System.ComponentModel;
    using System.IO;
    public enum ComparisonStatus { Success, MasterNotFound, TargetNotFound, Changed }
    public class ComparisonResult {
        private ComparisonStatus _status = ComparisonStatus.MasterNotFound;
        private string _description = "";
        private string _name = "";
        private string _masterPath = "";
        private string _targetPath = "";
        private bool _isFile = false;
        private string _message = null;
        public ComparisonStatus Status { get { return this._status; } private set { this._status = value; } }
        public string Description { get { return this._description; } private set { this._description = value ?? ""; } }
        public string Name { get { return this._name; } private set { this._name = value ?? ""; } }
        public string MasterPath { get { return this._masterPath; } private set { this._masterPath = value ?? ""; } }
        public string TargetPath { get { return this._targetPath; } private set { this._targetPath = value ?? ""; } }
        public bool IsFile { get { return this._isFile; } private set { this._isFile = value; } }
        public string Message {
            get {
                if (this._message != null)
                    return this._message;
                string message;
                switch (this.Status) {
                    case ComparisonStatus.MasterNotFound: {
                        message = "{0} not found at master location.";
                        break;
                    }
                    case ComparisonStatus.TargetNotFound: {
                        message = "{0} not found at target location.";
                        break;
                    }
                    case ComparisonStatus.Changed: {
                        message = "File changed.";
                        break;
                    }
                    default: {
                        message = "Success";
                        break;
                    }
                }
                this._message = String.Format(message, (this.IsFile) ? "File" : "Folder");
                return this._message;
            }
        }
        public ComparisonResult(string description, string masterPath, string targetPath, ComparisonStatus status) {
            this.Description = description;
            this.MasterPath = masterPath;
            this.TargetPath = targetPath;
            this.Status = status;
            this.IsFile = false;
        }
        public ComparisonResult(string description, string masterPath, string targetPath, ComparisonStatus status, string name, bool isFile) {
            this.Description = description;
            this.MasterPath = masterPath;
            this.TargetPath = targetPath;
            this.Name = name;
            this.Status = status;
            this.IsFile = isFile;
        }
        public static BindingList<ComparisonResult> MakeBindingList(params ComparisonResult[] items) {
            return new BindingList<ComparisonResult>(items);
        }
    }
}
'@ -ErrorAction Stop;
Function Test-DirectoryComparison {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Description,
        [Parameter(Mandatory = $true)]
        [string]$MasterPath,
        [Parameter(Mandatory = $true)]
        [string]$TargetPath,
        [AllowEmptyCollection()]
        [string[]]$MasterInclude,
        [AllowEmptyCollection()]
        [string[]]$MasterExclude,
        [AllowEmptyCollection()]
        [string[]]$TargetInclude,
        [AllowEmptyCollection()]
        [string[]]$TargetExclude
    )
    
    if ($MasterPath | Test-Path -PathType Container) {
        if (-not ($TargetPath | Test-Path -PathType Container)) {
            New-Object -TypeName 'CheckDiffCLR.ComparisonResult' -ArgumentList $Description, $MasterPath, $TargetPath, ([CheckDiffCLR.ComparisonStatus]::TargetNotFound);
            return;
        }
    } else {
            New-Object -TypeName 'CheckDiffCLR.ComparisonResult' -ArgumentList $Description, $MasterPath, $TargetPath, ([CheckDiffCLR.ComparisonStatus]::MasterNotFound);
        return;
    }
    
    $OutputText = @();
    
    if ($MasterInclude.Length -gt 0) {
        if ($MasterExclude.Length -gt 0) {
            $MasterItems = @(Get-ChildItem -Path $MasterPath -Include $MasterInclude -Exclude $MasterExclude);
        } else {
            $MasterItems = @(Get-ChildItem -Path $MasterPath -Include $MasterInclude);
        }
    } else {
        if ($MasterExclude.Length -gt 0) {
            $MasterItems = @(Get-ChildItem -Path $MasterPath -Exclude $MasterExclude);
        } else {
            $MasterItems = @(Get-ChildItem -Path $MasterPath);
        }
    }
    
    if ($TargetInclude.Length -gt 0) {
        if ($TargetExclude.Length -gt 0) {
            $TargetItems = @(Get-ChildItem -Path $TargetPath -Include $TargetInclude -Exclude $TargetExclude);
        } else {
            $TargetItems = @(Get-ChildItem -Path $TargetPath -Include $TargetInclude);
        }
    } else {
        if ($TargetExclude.Length -gt 0) {
            $TargetItems = @(Get-ChildItem -Path $TargetPath -Exclude $TargetExclude);
        } else {
            $TargetItems = @(Get-ChildItem -Path $TargetPath);
        }
    }
    
    $MasterFileArray = @($MasterItems | Where-Object { $_ -is [System.IO.FileInfo] });
    $MasterDirectoryArray = @($MasterItems | Where-Object { $_ -is [System.IO.DirectoryInfo] });
    $TargetFileArray = @($TargetItems | Where-Object { $_ -is [System.IO.FileInfo] });
    $TargetDirectoryArray = @($TargetItems | Where-Object { $_ -is [System.IO.DirectoryInfo] });
    
    $Items = @($TargetDirectoryArray | Where-Object { $Name = $_.Name; @($MasterDirectoryArray | Where-Object { $_.Name -ieq $Name }).Count -eq 0 });
    if ($Items.Count -gt 0) {
        $Items | ForEach-Object {
            New-Object -TypeName 'CheckDiffCLR.ComparisonResult' -ArgumentList $Description, $MasterPath, $TargetPath, ([CheckDiffCLR.ComparisonStatus]::MasterNotFound), $_.Name, $false;
        };
    }
    
    $Items = @($TargetFileArray | Where-Object { $Name = $_.Name; @($MasterFileArray | Where-Object { $_.Name -ieq $Name }).Count -eq 0 });
    if ($Items.Count -gt 0) {
        $Items | ForEach-Object {
            New-Object -TypeName 'CheckDiffCLR.ComparisonResult' -ArgumentList $Description, $MasterPath, $TargetPath, ([CheckDiffCLR.ComparisonStatus]::MasterNotFound), $_.Name, $true;
        };
    }
    
    $Items = @($MasterDirectoryArray | Where-Object { $Name = $_.Name; @($TargetDirectoryArray | Where-Object { $_.Name -ieq $Name }).Count -eq 0 });
    if ($Items.Count -gt 0) {
        $Items | ForEach-Object {
            New-Object -TypeName 'CheckDiffCLR.ComparisonResult' -ArgumentList $Description, $MasterPath, $TargetPath, ([CheckDiffCLR.ComparisonStatus]::TargetNotFound), $_.Name, $false;
        };
    }
    
    $Items = @($MasterFileArray | Where-Object { $Name = $_.Name; @($TargetFileArray | Where-Object { $_.Name -ieq $Name }).Count -eq 0 });
    if ($Items.Count -gt 0) {
        $Items | ForEach-Object {
            New-Object -TypeName 'CheckDiffCLR.ComparisonResult' -ArgumentList $Description, $MasterPath, $TargetPath, ([CheckDiffCLR.ComparisonStatus]::TargetNotFound), $_.Name, $true;
        };
    }
    
    $MasterFileArray | ForEach-Object { $Name = $_.Name; @{ Master = $_; Target = @($TargetFileArray | Where-Object { $_.Name -ieq $Name }) } } | Where-Object { $_.Target.Count -ne 0 } | ForEach-Object {
        if (Test-FileInfoComparison -Master $_.Master -Target $_.Target[0]) {
            New-Object -TypeName 'CheckDiffCLR.ComparisonResult' -ArgumentList $Description, $MasterPath, $TargetPath, ([CheckDiffCLR.ComparisonStatus]::Success), $_.Master.Name, $true;
        } else {
            New-Object -TypeName 'CheckDiffCLR.ComparisonResult' -ArgumentList $Description, $MasterPath, $TargetPath, ([CheckDiffCLR.ComparisonStatus]::Changed), $_.Master.Name, $true;
        };
    }
    
    foreach ($MasterFolder in $MasterDirectoryArray) {
        $TargetFolder = $TargetDirectoryArray | Where-Object { $_.Name -ieq $MasterFolder.Name };
        if ($TargetFolder -ne $null) {
            Test-DirectoryComparison -Description $Description -MasterPath $MasterFolder.FullName -TargetPath $TargetFolder.FullName -MasterInclude $MasterInclude -MasterExclude $MasterExclude -TargetInclude $TargetExclude -TargetExclude $TargetExclude
        }
    }
}

$Script:MasterPath = Read-FileDialog -Description 'Select Master Path' -Folder -ErrorAction Stop;
if ($Script:MasterPath -eq $null) { 'Aborted.' | Write-Warning }

$Script:TargetPath = Read-FileDialog -Description 'Select Target Path' -Folder -ErrorAction Stop;
if ($Script:TargetPath -eq $null) { 'Aborted.' | Write-Warning }

$Results = @(Test-DirectoryComparison -Description 'Source Publish' -MasterPath $Script:MasterPath -TargetPath $Script:TargetPath);
$Differences = @($Results | Where-Object { $_.Status -ne [CheckDiffCLR.ComparisonStatus]::Success });

if ($Differences.Count -eq 0) {
    $Results | Select-Object -Property 'Description', 'Name', 'Message', @{Name="Relative Path"; Expression = {
        $p = $_.MasterPath.Substring($Script:MasterPath.Length);
        if ($p.StartsWith('\')) { $p.Substring(1) } else { $p }
    }} | Out-GridView -Title 'Both directories are identical.';
} else {
    $Differences | Select-Object -Property 'Description', 'Name', 'Message', @{Name="Relative Path"; Expression = {
        if ($_.Status -eq [CheckDiffCLR.ComparisonStatus]::MasterNotFound) {
            $p = $_.MasterPath.Substring($Script:MasterPath.Length);
        } else {
            $p = $_.TargetPath.Substring($Script:TargetPath.Length);
        }
        if ($p.StartsWith('\')) { $p.Substring(1) } else { $p }
    }} | Out-GridView -Title ('{0} discrepancies found.' -f $Differences.Count);
}
