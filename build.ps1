Param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',

    [string]$Platform = 'Any CPU',

    [ValidateSet('Test', 'Deploy', 'None')]
    [string]$Action = 'None',

    [ValidateSet('Build', 'Resources', 'Compile', 'Rebuild', 'Clean', 'Publish')]
    [string[]]$Targets = @('Build'),

    [ValidateSet('CertificateCryptography', 'CredentialStorage', 'GDIPlus', 'IOUtility', 'LteDev', 'NetworkUtility', 'PSDB', 'XmlUtility')]
    [string]$Project,

    [int]$TermMinWidth = 2048,

    [int]$TermMinHeight = 6000,

    [string]$MsBuildBin = 'C:\Program Files (x86)\MSBuild\14.0\Bin'
)

$old_InformationPreference = $InformationPreference;
$InformationPreference = [System.Management.Automation.ActionPreference]::Ignore;
try {
    $InformationPreference = [System.Management.Automation.ActionPreference]::Continue;
    Write-Information -MessageData "Host: $((Get-Host).Name)"
    $HostRawUI = (Get-Host).UI.RawUI;
    if ($HostRawUI.BufferSize -eq $null) {
        Write-Information -MessageData "Buffer size was null";
        $HostRawUI.BufferSize = New-Object -TypeName ''System.Management.Automation.Host.Size'' -ArgumentList $TermMinWidth, $TermMinHeight;
    } else {
        Write-Information -MessageData "Buffer size was $($HostRawUI.BufferSize.Width), $($HostRawUI.BufferSize.Height)";
        if ($HostRawUI.BufferSize.Width -lt $TermMinWidth) { $HostRawUI.BufferSize.Width = $TermMinWidth }
        if ($HostRawUI.BufferSize.Height -lt $TermMinHeight) { $HostRawUI.BufferSize.Height = $TermMinHeight }
        Write-Information -MessageData "Buffer size is now $($HostRawUI.BufferSize.Width), $($HostRawUI.BufferSize.Height)";
    }
    if ($HostRawUI.MaxWindowSize -ne $null) {
        if ($HostRawUI.MaxWindowSize.Width -lt $HostRawUI.BufferSize.Width) {
            $HostRawUI.MaxWindowSize.Width = $HostRawUI.BufferSize.Width;
        }
        if ($HostRawUI.MaxWindowSize.Height -lt $HostRawUI.BufferSize.Height) {
            $HostRawUI.MaxWindowSize.Height = $HostRawUI.BufferSize.Height;
        }
    }
} finally {
    $InformationPreference = $old_InformationPreference;
}

$Script:SolutionFilePath = $PSScriptRoot | Join-Path -ChildPath $SolutionFile;
$Script:SolutionDirectory = $Script:SolutionFilePath | Split-Path -Parent;
$MSBuildExePath = $MsBuildBin | Join-Path -ChildPath 'MSBuild.exe';
if ($Project -ne $null -and $Project.Trim().Length -gt 0) {
    . $MSBuildExePath "/t:$($Targets -join ';')" "/p:GenerateFullPaths=true" "/p:Configuration=`"$Configuration`"" "/p:Platform=`"$Platform`"" "src/$Project/$Project.csproj";
} else {
    . $MSBuildExePath "/t:$($Targets -join ';')" "/p:GenerateFullPaths=true" "/p:Configuration=`"$Configuration`"" "/p:Platform=`"$Platform`"" "src/PowerShellModules.sln";
}
