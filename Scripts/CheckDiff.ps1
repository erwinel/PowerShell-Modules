Import-Module -Name 'Erwine.Leonard.T.IOUtility';

$Script:CheckPublish = $true;
$Script:CheckInstall = $true;


$Script:MasterPath = 'C:\Users\leonarde\Downloads\PowerShell-Modules-master';
$Script:SourcePath = 'C:\Users\leonarde\Documents\Source\PowerShell-Modules-master';
$Script:ModulePath = 'C:\Users\leonarde\Documents\WindowsPowerShell\Modules';
$Script:ScriptsPath = 'C:\Users\leonarde\Documents\WindowsPowerShell\Scripts';

Import-Module -Name 'Erwine.Leonard.T.WindowsForms';

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

$ComparisonSets = @(
    @{
        Description = 'CertificateCryptography Install';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'CertificateCryptography';
        MasterExclude = '*.pssproj', 'Install.*', 'Uninstall.*', '*.tests.ps1';
        TargetPath = $Script:ModulePath | Join-Path -ChildPath 'Erwine.Leonard.T.CertificateCryptography';
    },
    @{
        Description = 'CertificateCryptography Custom Types';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'CredentialStorage\CredentialStorageCLR';
        MasterExclude = '*.cd', '*.csproj', 'Properties';
        TargetPath = $Script:SourcePath | Join-Path -ChildPath 'CredentialStorage\CredentialStorage';
        TargetExclude = '*.pssproj', '*.psm1', '*.psd1', '*.bat', '*.ps1', 'CredentialStorage.xsd', '*.help.txt';
    },
    @{
        Description = 'CredentialStorage Install';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'CredentialStorage\CredentialStorage';
        MasterExclude = '*.pssproj', 'Install.*', 'Uninstall.*', '*.tests.ps1';
        TargetPath = $Script:ModulePath | Join-Path -ChildPath 'Erwine.Leonard.T.CredentialStorage';
    },
    @{
        Description = 'CryptographyScripts Install';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'CryptographyScripts';
        MasterExclude = '*.pssproj', 'Install.*', 'Uninstall.*';
        TargetPath = $Script:ScriptsPath | Join-Path -ChildPath 'CryptographyScripts';
    },
    @{
        Description = 'IOUtility Custom Types';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'IOUtility\IOUtilityCLR';
        MasterExclude = '*.cd', '*.csproj', 'Properties';
        TargetPath = $Script:SourcePath | Join-Path -ChildPath 'IOUtility\IOUtility';
        TargetExclude = '*.pssproj', '*.psm1', '*.psd1', '*.bat', '*.ps1', '_setup', '*.help.txt';
    },
    @{
        Description = 'IOUtility Install';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'IOUtility\IOUtility';
        MasterExclude = '*.pssproj', 'Install.*', 'Uninstall.*', '*.tests.ps1', '_setup', 'Setup.*';
        TargetPath = $Script:ModulePath | Join-Path -ChildPath 'Erwine.Leonard.T.IOUtility';
    },
    @{
        Description = 'LteDev Custom Types';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'LteDev\LteDevCLR';
        MasterExclude = '*.cd', '*.csproj', 'Properties';
        TargetPath = $Script:SourcePath | Join-Path -ChildPath 'LteDev\LteDev';
        TargetExclude = '*.pssproj', '*.psm1', '*.psd1', '*.bat', '*.ps1', '*.help.txt';
    },
    @{
        Description = 'LteDev Install';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'LteDev\LteDev';
        MasterExclude = '*.pssproj', 'Install.*', 'Uninstall.*', '*.tests.ps1';
        TargetPath = $Script:ModulePath | Join-Path -ChildPath 'Erwine.Leonard.T.LteDev';
    },
    @{
        Description = 'LteUtils Install';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'LteUtils';
        MasterExclude = '*.pssproj', 'Install.*', 'Uninstall.*', '*.ps1';
        TargetPath = $Script:ModulePath | Join-Path -ChildPath 'Erwine.Leonard.T.LteUtils';
    },
    @{
        Description = 'NetworkUtility Custom Types';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'NetworkUtility\NetworkUtilityCLR';
        MasterExclude = '*.cd', '*.csproj', 'Properties';
        TargetPath = $Script:SourcePath | Join-Path -ChildPath 'NetworkUtility\NetworkUtility';
        TargetExclude = '*.pssproj', '*.psm1', '*.psd1', '*.bat', '*.ps1', '*.help.txt';
    },
    @{
        Description = 'NetworkUtility Install';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'NetworkUtility\NetworkUtility';
        MasterExclude = '*.pssproj', 'Install.*', 'Uninstall.*', '*.tests.ps1';
        TargetPath = $Script:ModulePath | Join-Path -ChildPath 'Erwine.Leonard.T.NetworkUtility';
    },
    @{
        Description = 'Scripts Install';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'Scripts';
        MasterExclude = '*.pssproj';
        TargetPath = $Script:ScriptsPath;
        TargetExclude = 'CryptographyScripts';
    },
    @{
        Description = 'WindowsForms Install';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'WindowsForms';
        MasterExclude = '*.pssproj', 'Install.*', 'Uninstall.*', '*.tests.ps1';
        TargetPath = $Script:ModulePath | Join-Path -ChildPath 'Erwine.Leonard.T.WindowsForms';
    },
    @{
        Description = 'XmlUtility Custom Types';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'XmlUtility\XmlUtilityCLR';
        MasterExclude = '*.cd', '*.csproj', 'Properties';
        TargetPath = $Script:SourcePath | Join-Path -ChildPath 'XmlUtility\XmlUtility';
        TargetExclude = '*.pssproj', '*.psm1', '*.psd1', '*.bat', '*.ps1', '*.help.txt';
    },
    @{
        Description = 'XmlUtility Install';
        MasterPath = $Script:SourcePath | Join-Path -ChildPath 'XmlUtility\XmlUtility';
        MasterExclude = '*.pssproj', 'Install.*', 'Uninstall.*', '*.tests.ps1';
        TargetPath = $Script:ModulePath | Join-Path -ChildPath 'Erwine.Leonard.T.XmlUtility';
    }
);

$Results = @();
if ($Script:CheckPublish) {
    $Results += @(Test-DirectoryComparison -Description 'Source Publish' -MasterPath $Script:MasterPath -TargetPath $Script:SourcePath);
}

if ($Script:CheckInstall) {
    $ComparisonSets | ForEach-Object {
        $Results += @(Test-DirectoryComparison @_);
    }
}
$Results | Where-Object { $_.Status -ne [CheckDiffCLR.ComparisonStatus]::Success } | Select-Object -Property 'Description', 'Name', 'Message', 'MasterPath', 'TargetPath'

<#
#[CheckDiffCLR.ComparisonResult[]]$List = @($Results | Where-Object { $_.Status -ne [CheckDiffCLR.ComparisonStatus]::Success });

$Form = New-WindowObject -Name 'CheckDiff.MainForm' -Title 'Comparison Results';
try {
    $Form | Set-FormSize -Width 800 -Height 600;
    $TableLayoutPanel = New-TableLayoutPanel -Name 'outerTableLayoutPanel' -Dock Fill
    Add-FormControl -Parent $Form -Child $TableLayoutPanel;
    Add-LayoutPanelColumnStyle -TableLayoutPanel $TableLayoutPanel -Width 100 -Percent
    Add-LayoutPanelColumnStyle -TableLayoutPanel $TableLayoutPanel -Count 1 -AutoSize
    Add-LayoutPanelRowStyle -TableLayoutPanel $TableLayoutPanel -Height 100 -Percent;
    Add-LayoutPanelRowStyle -TableLayoutPanel $TableLayoutPanel -Count 1 -AutoSize;
    $Button = New-FormsButton -Name 'cancelButton' -Text 'Cancel' -DialogResult Cancel -Anchor ([System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right) `
        -Size (New-DrawingSize -Width 75 -Height 25) -TabIndex 2 -TabStop $true -Tag $Form -OnClick {
        Param(
            [Parameter(Mandatory = $true, Position = 0)]
            [System.Windows.Forms.Button]$Sender,
            [Parameter(Mandatory = $true, Position = 1)]
            [System.EventArgs]$Args
        )
        $Sender.Tag.Tag = $null;
        $Sender.Tag.DialogResult = $Sender.DialogResult;
        $Sender.Tag.Close();
    };
    Add-TableLayoutControl -Parent $TableLayoutPanel -Child $Button -Column 0 -Row 1;
    $Button = New-FormsButton -Name 'okButton' -Text 'OK' -DialogResult OK -Anchor ([System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right) `
        -Size (New-DrawingSize -Width 75 -Height 25) -TabIndex 1 -TabStop $true -Disabled -Tag $Form -OnClick {
        Param(
            [Parameter(Mandatory = $true, Position = 0)]
            [System.Windows.Forms.Button]$Sender,
            [Parameter(Mandatory = $true, Position = 1)]
            [System.EventArgs]$Args
        )
        $Sender.Tag.Tag = $Sender.Tag.Tag.SelectedRows;
        $Sender.Tag.DialogResult = $Sender.DialogResult;
        $Sender.Tag.Close();
    };
    Add-TableLayoutControl -Parent $TableLayoutPanel -Child $Button -Column 1 -Row 1;
    $DataGridView = New-DataGridView -Name 'mainDataGridView' -Dock Fill -MultiSelect -ReadOnly -OnSelectionChanged {
        Param(
            [Parameter(Mandatory = $true, Position = 0)]
            [System.Windows.Forms.DataGridView]$Sender,
            [Parameter(Mandatory = $true, Position = 1)]
            [System.EventArgs]$Args
        )
        
        $Sender.Tag.Enabled = ($Sender.SelectedRows.Count -gt 0);
    };
    Add-TableLayoutControl -Parent $TableLayoutPanel -Child $DataGridView -Column 0 -Row 0 -ColumnSpan 2;
    $DataGridView.Tag = $Button;
    $Form.Tag = $DataGridView;
    Add-DataGridViewTextBoxColumn -DataGridView $DataGridView -DataPropertyName 'Description' -HeaderText 'Description' -AutoSizeMode ([System.Windows.Forms.DataGridViewAutoSizeColumnMode]::AllCells) -ReadOnly
    Add-DataGridViewTextBoxColumn -DataGridView $DataGridView -DataPropertyName 'Name' -HeaderText 'Name' -AutoSizeMode ([System.Windows.Forms.DataGridViewAutoSizeColumnMode]::AllCells) -ReadOnly
    Add-DataGridViewTextBoxColumn -DataGridView $DataGridView -DataPropertyName 'Message' -HeaderText 'Message' -AutoSizeMode ([System.Windows.Forms.DataGridViewAutoSizeColumnMode]::Fill) -ReadOnly
    Add-DataGridViewTextBoxColumn -DataGridView $DataGridView -DataPropertyName 'MasterPath' -HeaderText 'Master Location' -AutoSizeMode ([System.Windows.Forms.DataGridViewAutoSizeColumnMode]::AllCells) -ReadOnly
    Add-DataGridViewTextBoxColumn -DataGridView $DataGridView -DataPropertyName 'TargetPath' -HeaderText 'Target Location' -AutoSizeMode ([System.Windows.Forms.DataGridViewAutoSizeColumnMode]::AllCells) -ReadOnly
    $DataGridView.DataSource = [CheckDiffCLR.ComparisonResult]::MakeBindingList($List);
    $Form.ShowDialog();
    $Result = $Form.Tag;
} catch {
    $List
    throw;
} finally {
    $Form.Dispose();
}
#>