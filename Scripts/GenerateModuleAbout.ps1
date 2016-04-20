$PreviousVerbosePreference = $VerbosePreference;
$ModuleName = Read-Host -Prompt 'Enter name of module';
if ($ModuleName -eq $null -or ($ModuleName = $ModuleName.Trim()) -eq '') {
    Write-Warning -Message 'Aborted.';
    return;
}
$Module = Get-Module -Name $ModuleName;
if ($Module -eq $null) {
    $Module = Get-Module -Name $ModuleName -ListAvailable;
    if ($Module -eq $null) {
        Write-Error -Message ('Module named "{0}" not found.' -f $ModuleName) -Category ObjectNotFound -ErrorId 1 -TargetObject $ModuleName -CategoryTargetName 'ModuleName';
    } else {
        Write-Warning -Message 'Module must be imported so commands can be enumerated.';
    }
    return;
}
$aboutLocation = ($Module.Path | Split-Path -Parent) | Join-Path -ChildPath ('about_{0}.help.txt' -f $Module.Name);
$Sections = @{
    'LONG DESCRIPTION' = @()
}
if ($aboutLocation | Test-Path -PathType Leaf) {
    $lastSection = 'LONG DESCRIPTION';
    $LastLine = $lastSection;
    [System.IO.File]::ReadAllLines($aboutLocation) | ForEach-Object {
        $Line = $_.TrimEnd();
        if ($Line.Length -gt 0) {
            if ([System.Char]::IsWhiteSpace($Line[0])) {
                if ($LastLine -eq '') {
                    $Sections[$lastSection] += @('', $Line);
                } else {
                    if ($Sections[$lastSection].Count -eq 0) {
                        $Sections[$lastSection] += $Line;
                    } else {
                        $Sections[$lastSection][-1] = '{0} {1}' -f $Sections[$lastSection][-1], $Line.TrimStart();
                    }
                }
            } else {
                $lastSection = $Line;
                if (-not $Sections.ContainsKey($lastSection)) { $Sections[$lastSection] = @() }
            }
        }
        $LastLine = $Line;
    }
}
$t = 'about_{0}' -f $Module.Name;
if ($Sections.ContainsKey('TOPIC') -and $Sections['TOPIC'].Count -gt 0) {
    if (@($Sections['TOPIC'] | Where-Object { $_.Trim() -eq $t }).Count -eq 0) {
        $Sections['TOPIC'] = @('    {0}' -f $t) + $Sections['TOPIC'];
    }
} else {
    $Sections['TOPIC'] = @('    {0}' -f $t);
}
if ($Sections.ContainsKey('SHORT DESCRIPTION') -and $Sections['SHORT DESCRIPTION'].Count -gt 0) {
    $VerbosePreference = [System.Management.Automation.ActionPreference]::Continue;
    'Current short description:' | Write-Verbose;
    $Sections['SHORT DESCRIPTION'] | Write-Verbose;
    $VerbosePreference = $PreviousVerbosePreference;
    $ShortDescription = Read-Host -Prompt 'Enter short description or blank for default';
    if ($ShortDescription -eq $null) {
        Write-Warning -Message 'Aborted.';
        return;
    }
    if (($ShortDescription = $ShortDescription.Trim()) -ne '') { $Sections['SHORT DESCRIPTION'] = @($ShortDescription) }
} else {
    $ShortDescription = Read-Host -Prompt 'Enter short description';
    if ($ShortDescription -eq $null -or ($ShortDescription = $ShortDescription.Trim()) -eq '') {
        Write-Warning -Message 'Aborted.';
        return;
    }
    $Sections['SHORT DESCRIPTION'] = @('    {0}' -f $ShortDescription);
}
if ($Sections.ContainsKey('LONG DESCRIPTION') -and $Sections['LONG DESCRIPTION'].Count -gt 0) {
    $VerbosePreference = [System.Management.Automation.ActionPreference]::Continue;
    'Current long description:' | Write-Verbose;
    $Sections['LONG DESCRIPTION'] | Write-Verbose;
    $VerbosePreference = $PreviousVerbosePreference;
    $d = Read-Host -Prompt 'Enter long description, line 1, or blank for default';
    if ($d -eq $null) {
        Write-Warning -Message 'Aborted.';
        return;
    }
    if (($d = $d.Trim()) -ne '') { $Sections['LONG DESCRIPTION'] = @() }
} else {
    $d = Read-Host -Prompt 'Enter long description, line 1';
    if ($d -eq $null -or ($d = $d.Trim()) -eq '') {
        Write-Warning -Message 'Aborted.';
        return;
    }
    $Sections['LONG DESCRIPTION'] = @();
}
if ($Sections['LONG DESCRIPTION'].Count -eq 0) {
    $Sections['LONG DESCRIPTION'] = @($d.Trim());
    $n = 1;
    while ($d -ne $null) {
        $n++;
        if ($Sections['LONG DESCRIPTION'][$Sections['LONG DESCRIPTION'].Count - 1] -eq '') {
            $d = Read-Host -Prompt ('Enter long description, line {0} or enter another blank line to finish' -f $n);
            if ($d -eq $null) {
                Write-Warning -Message 'Aborted.';
                return;
            }
            $d = $d.TrimEnd();
            if ($d -eq '') {
                if ($Sections['LONG DESCRIPTION'].Length -eq 1) { $l = @() } else { $Sections['LONG DESCRIPTION'] = $Sections['LONG DESCRIPTION'][0..($Sections['LONG DESCRIPTION'].Length - 2)] }
                $d = $null;
            } else {
                $Sections['LONG DESCRIPTION'] = $Sections['LONG DESCRIPTION'] + ('    {0}' -f $d).TrimEnd();
            }
        } else {
            $d = Read-Host -Prompt ('Enter long description, line {0} (enter another 2 blank lines to finish)' -f $n);
            if ($d -eq $null) {
                Write-Warning -Message 'Aborted.';
                return;
            }
            $Sections['LONG DESCRIPTION'] = $Sections['LONG DESCRIPTION'] + ('    {0}' -f $d).TrimEnd();
        }
    }
}
if ($Sections['LONG DESCRIPTION'].Count -eq 0) {
    Write-Warning -Message 'Aborted.';
    return;
}

Function Break-Lines {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowEmptyString()]
        [string]$Line
    )
    
    Begin { $Regex = New-Object -TypeName 'System.Text.RegularExpressions.Regex' -ArgumentList '(\s\S+|\s)$' }
    
    Process {
        if ($Line.Length -lt 80) {
            $Line | Write-Output;
        } else {
            $Indent = '';
            for ($c=0; $c -lt $Line.Length -and $c -lt 40; $c++) {
                if (-not [System.Char]::IsWhiteSpace($Line[$c])) { break }
                $Indent += $Line[$c];
            }
            
            while ($Line.Length -gt 79) {
                $M = $Regex.Match($Line.Substring(0, 79));
                if ($M.Success) {
                    $Line.Substring(0, $M.Index).TrimEnd();
                    $Line = $Indent + $Line.Substring($M.Index + 1).TrimStart();
                } else {
                    $Line.Substring(0, 79);
                    $Line = $Indent + $Line.Substring(79);
                }
            }
            if ($Line -ne '') { $Line }
        }
    }
}

$Sections['EXPORTED COMMANDS'] = @('    Following is a list of commands exported by this module:') + @($Module.ExportedCommands.Keys | ForEach-Object {
    $h = Get-Help -Name $_;
    $txt = $h.Synopsis;
    if ($txt -eq $null) { $txt = '' } else { $txt = ($txt | Out-String).TrimEnd() }
    if ($txt -eq '') {
        $txt = $h.description;
        if ($txt -eq $null) { $txt = '' } else { $txt = ($txt | Out-String).TrimEnd() }
    }
    @(
        '',
        ('    ' + $_)
    ) | Write-Output;
    if ($txt -ne '') {
        (($txt -split '\r\n?|\n') | ForEach-Object { '        ' + $_ })
    }
});
$Sections['SEE ALSO'] = @($Module.ExportedCommands.Keys | ForEach-Object { '    ' + $_ | Write-Output });

$SectionKeys = @('TOPIC', 'SHORT DESCRIPTION', 'LONG DESCRIPTION', 'EXPORTED COMMANDS');
$SectionKeys = $SectionKeys + @($Sections.Keys | Where-Object { $_ -ine 'SEE ALSO' -and $SectionKeys -inotcontains $_ });
$SectionKeys += 'SEE ALSO';

$OutputLines = @($SectionKeys | ForEach-Object {
    if ($_ -ne 'TOPIC') { '' | Write-Output }
    $_ | Write-Output;
    $Sections[$_] | Break-Lines;
});

$VerbosePreference = [System.Management.Automation.ActionPreference]::Continue;
$OutputLines | Write-Verbose;
@('', ('Default path is "{0}".' -f $aboutLocation)) | Write-Verbose;
$VerbosePreference = $PreviousVerbosePreference;
$path = Read-Host -Prompt 'Enter enter path, cancel to abort saving, or use a blank line to use default path.';
if ($path -eq $null) { return }
if ($path.Trim().Length -ne 0) { $aboutLocation = $path }
[System.IO.File]::WriteAllLines($aboutLocation, $OutputLines, [System.Text.Encoding]::UTF8);
    