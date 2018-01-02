try { Import-Module -Name '..\Module\PowerShellModule' -ErrorAction Stop }
catch {
    $Host.UI.WriteErrorLine($_);
    Write-Warning -Message 'Error loading target module.';
    return;
}

$TestScripts = @(Get-ChildItem -Path $PSScriptRoot -Filter 'Test-*.ps1');
$ErrorObj = @();
$MessageObj = @();
$TestScripts | ForEach-Object {
    $CmdErrors = @();
    $CmdWarnings = @();
    $Name = $_.Name;
    Invoke-Expression -Command $_.FullName -ErrorVariable 'CmdErrors' -WarningVariable 'CmdWarnings' -ErrorAction Continue;
    if ($CmdErrors.Count -gt 0) {
        $ErrorObj = $ErrorObj + @(@($CmdErrors) | Select-Object -Property @{
            Label = 'Script'; Expression = { $Name } }, @{
            Label = 'Message'; Expression = { $_.ToString() } }, @{
            Label = 'InnerMessage'; Expression = {
                $m = $_.ToString();
                for ($e = $_.Exception; $e -ne $null; $e = $e.InnerException) {
                    if ($e.Message -ne $m) { return $e.Message }
                }
                return '';
            } }, @{
            Label = 'Position'; Expression = { if ($_.InvocationInfo -eq $null) { return '' } $_.InvocationInfo.PositionMessage } }, @{
            Label = 'Category'; Expression = { if ($_.CategoryInfo -eq $null) { return '' } $_.CategoryInfo.GetMessage() } }, @{
            Label = 'Stack Trace'; Expression = { if ($_.ScriptStackTrace -eq $null) { return '' } $_.ScriptStackTrace } });
    }
    if ($CmdWarnings.Count -gt 0) {
        $MessageObj = $MessageObj + @(@($CmdWarnings) | Select-Object -Property @{ Label = 'Type'; Expression = { 'Warning' } }, @{ Label = 'Script'; Expression = { $Name } }, 'Message');
    } else {
        if ($CmdErrors.Count -eq 0) { $MessageObj = $MessageObj + @([PSCustomObject]@{ Type = 'Success'; Script = $Name; Message = 'No errors or warnings' }) };
    }
}

if ($ErrorObj.Count -gt 0) {
    $ErrorObj | Out-GridView -Title 'Test Errors';
}

if ($WarningObj.Count -gt 0) {
    $WarningObj | Out-GridView -Title 'Test Warnings';
}