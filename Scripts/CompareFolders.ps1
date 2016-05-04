Remove-Module -Name 'Erwine.Leonard.T.IOUtility'; Import-Module -Name 'Erwine.Leonard.T.IOUtility';

$Script:MasterPath = Read-FileDialog -Description 'Select Master Path' -Folder -ErrorAction Stop;
if ($Script:MasterPath -eq $null) { 'Aborted.' | Write-Warning; return; }

$Script:TargetPath = Read-FileDialog -Description 'Select Target Path' -Folder -ErrorAction Stop;
if ($Script:TargetPath -eq $null) { 'Aborted.' | Write-Warning; return; }

$Script:MasterPath = [System.IO.Path]::GetFullPath($Script:MasterPath);
$Script:TargetPath = [System.IO.Path]::GetFullPath($Script:TargetPath);

$ComparisonResults = @(Compare-FileSystemInfo -ReferencePath $Script:MasterPath -DifferencePath $Script:TargetPath);

if ($ComparisonResults.Count -eq 0) {
    'Both paths are equal';
    return;
}

$ComparisonResults | ForEach-Object {
    New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
        RelativePath = '.' + $_.ReferenceInfo.FullName.Substring($Script:MasterPath.Length)
        Name = $_.ReferenceInfo.Name;
        Message = $_.Message;
    }
} | Out-GridView -Title 'Folder Comparison';