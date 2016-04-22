Import-Module 'Erwine.Leonard.T.IOUtility';
Import-Module 'Erwine.Leonard.T.XmlUtility';

$Script:CredentialsPath = 'C:\Users\leonarde\Documents\AppData\Credentials.xml';
$Script:CredentialsXmlDocument = Read-XmlDocument -InputUri $Script:CredentialsPath;

Function Save-CredentialsDocument {
    Param()
    
    $Settings = New-XmlWriterSettings -Indent $true;
    Write-XmlDocument -Document $Script:CredentialsXmlDocument -OutputFileName $Script:CredentialsPath -Settings $Settings;
}

Function Copy-ItemProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$XmlElement
    )
    
    $Choices = New-Object -TypeName 'System.Collections.ObjectModel.Collection[System.Management.Automation.Host.ChoiceDescription]';
    if ($XmlElement.Url -ne $null -and $XmlElement.Url -ne '') {
        $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Url', ('Copy Url ({0})' -f $XmlElement.Url)));
    }
    if ($XmlElement.Login -ne $null -and $XmlElement.Login -ne '') {
        $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Login', ('Copy Login ({0})' -f $XmlElement.Login)));
    }
    if ($XmlElement.Password -ne $null -and $XmlElement.Password -ne '') {
        $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Password', 'Copy Password'));
    }
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Done', 'Done'));
    if ([System.Windows.Forms.Clipboard]::ContainsText()) {
        $TextDataFormat = @(@(
            [System.Windows.TextDataFormat]::Xaml,
            [System.Windows.TextDataFormat]::Rtf,
            [System.Windows.TextDataFormat]::Html,
            [System.Windows.TextDataFormat]::CommaSeparatedValue,
            [System.Windows.TextDataFormat]::UnicodeText,
            [System.Windows.TextDataFormat]::Text
        ) | Where-Object { [System.Windows.Forms.Clipboard]::ContainsText($_) });
        if ($TextDataFormat.Count -gt 0) {
            $ClipboardText = [System.Windows.Forms.Clipboard]::GetText($TextDataFormat[0]);
        } else {
            $ClipboardText = [System.Windows.Forms.Clipboard]::GetText();
        }
    } else {
        $ClipboardText = $null;
    }
    try {
        do {
            $Index = $Host.UI.PromptForChoice('Copy', 'Select value to copy to clipboard', $Choices, 0);
            if ($Index -eq $null -or $Index -lt 0 -or $Index -ge $Choices.Count) {
                $Label = '';
            } else {
                $Label = $Choices[$Index].Label;
            }
            switch ($Label) {
                '_Url' {
                    if ($XmlElement.Url -eq '') {
                        'No Url' | Write-Warning;
                        [System.Windows.Forms.Clipboard]::SetText('');
                    } else {
                        [System.Windows.Forms.Clipboard]::SetText($XmlElement.Url, [System.Windows.TextDataFormat]::Text);
                    }
                    break;
                }
                '_Login' {
                    if ($XmlElement.Login -eq '') {
                        'No Login' | Write-Warning;
                        [System.Windows.Forms.Clipboard]::SetText('');
                    } else {
                        [System.Windows.Forms.Clipboard]::SetText($XmlElement.Login, [System.Windows.TextDataFormat]::Text);
                    }
                    break;
                }
                '_Password' {
                    if ($XmlElement.Password -eq '') {
                        'No password' | Write-Warning;
                        [System.Windows.Forms.Clipboard]::SetText('');
                    } else {
                        $PSCredential = New-Object -TypeName 'System.Management.Automation.PSCredential' -ArgumentList $XmlElement.Login, ($XmlElement.Password | ConvertTo-SecureString);
                        [System.Windows.Forms.Clipboard]::SetText($PSCredential.GetNetworkCredential().Password, [System.Windows.TextDataFormat]::Text);
                        $PSCredential.Dispose();
                    }
                    break;
                }
                default { $Label = $null }
            }
        } while ($Label -ne $null);
    } catch {
        throw;
    } finally {
        if ($ClipboardText -eq $null) {
            [System.Windows.Forms.Clipboard]::SetText('', [System.Windows.TextDataFormat]::Text);
        } else {
            if ($TextDataFormat.Count -gt 0) {
                [System.Windows.Forms.Clipboard]::SetText($ClipboardText, $TextDataFormat[0]);
            } else {
                [System.Windows.Forms.Clipboard]::SetText($ClipboardText);
            }
        }
    }
}

Function Get-YesOrNo {
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Caption,
        
        [Parameter(Mandatory = $true)]
        [string]$Message,
        
        [bool]$DefaultValue = $false
    )
    $Choices = New-Object -TypeName 'System.Collections.ObjectModel.Collection[System.Management.Automation.Host.ChoiceDescription]';
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Yes'));
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_No'));
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Cancel'));
    if ($DefaultValue) {
        $Index = $Host.UI.PromptForChoice($Caption, $Message, $Choices, 0);
    } else {
        $Index = $Host.UI.PromptForChoice($Caption, $Message, $Choices, 1);
    }
    if ($Index -ne $null) {
        if ($Index -eq 0) {
            $true;
        } else {
            if ($Index -eq 1) { $false }
        }
    }
}

Function Get-ItemCommand {
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$XmlElement
    )
    $Choices = New-Object -TypeName 'System.Collections.ObjectModel.Collection[System.Management.Automation.Host.ChoiceDescription]';
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Open', 'Open to Copy Properties'));
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Edit', 'Edit Properties'));
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Copy', 'Create duplicate copy'));
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Delete', 'Delete credential'));
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Return', 'Return to top-level'));
    do {
        $Index = $Host.UI.PromptForChoice('Command', 'Select item command', $Choices, 0);
        if ($Index -eq $null -or $Index -lt 0 -or $Index -ge $Choices.Count) {
            $Label = '';
        } else {
            $Label = $Choices[$Index].Label;
        }
        switch ($Label) {
            '_Open' {
                Copy-ItemProperties -XmlElement $XmlElement;
                $true;
                break;
            }
            '_Edit' {
                $Name = Read-Host -Prompt ('Enter Name or blank to keep "{0}"' -f $XmlElement.Name);
                if ($Name -eq $null -or $Name.Trim() -eq '') { $Name = $XmlElement.Name }
                if ($XmlElement.Login -eq $null -or $XmlElement.Login -eq '') {
                    if ($XmlElement.Password -eq $null -or $XmlElement.Password -eq '') {
                        $result = Get-YesOrNo -Caption 'Modify Login?' -Message 'Login and password are empty. Would you like to change it?';
                    } else {
                        $result = Get-YesOrNo -Caption 'Modify Login?' -Message 'Login is currently empty, and password is set. Would you like to change it?';
                    }
                } else {
                    if ($XmlElement.Password -eq $null -or $XmlElement.Password -eq '') {
                        $result = Get-YesOrNo -Caption 'Modify Login?' -Message ('Login is currently "{0}", and password is not set. Would you like to change it?' -f $XmlElement.Login);
                    } else {
                        $result = Get-YesOrNo -Caption 'Modify Login?' -Message ('Login is currently "{0}", and password is set. Would you like to change it?' -f $XmlElement.Login);
                    }
                }
                if ($result -ne $null -and $result) {
                    if (($XmlElement.Login -eq $null -or $XmlElement.Login -eq '') -and ($XmlElement.Password -eq $null -or $XmlElement.Password -eq '')) {
                        $PSCredential = Get-Credential;
                    } else {
                        $PSCredential = Get-Credential -Credential (New-Object -TypeName 'System.Management.Automation.PSCredential' -ArgumentList $XmlElement.Login, ($XmlElement.Password | ConvertTo-SecureString));
                    }
                } else {
                    $PSCredential = $null;
                }
                if ($XmlElement.Url -eq $null -or $XmlElement.Url -eq '') {
                    $result = Get-YesOrNo -Caption 'Modify Url?' -Message 'Url is empty. Would you like to change it?';
                } else {
                    $result = Get-YesOrNo -Caption 'Modify Url?' -Message ('Url is currently "{0}", and password is set. Would you like to change it?' -f $XmlElement.Login);
                }
                if ($result -ne $null -and $result) {
                    $Url = Read-Host -Prompt 'Enter URL';
                    if ($Url -ne $null) {
                        $e = $XmlElement.SelectSingleNode('Url');
                        if ($e -eq $null) { $e = $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Name')) }
                        $e.InnerText = $Url;
                    }
                }
                $e = $XmlElement.SelectSingleNode('Url');
                if ($e -eq $null) { $e = $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Name')) }
                $e.InnerText = $Name;
                if ($PSCredential -ne $null) {
                    $e = $XmlElement.SelectSingleNode('Login');
                    if ($e -eq $null) { $e = $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Login')) }
                    if ($PSCredential.UserName.StartsWith('\')) {
                        $e.InnerText = $PSCredential.UserName.Substring(1);
                    } else {
                        $e.InnerText = $PSCredential.UserName;
                    }
                    $e = $XmlElement.SelectSingleNode('Password');
                    if ($e -eq $null) { $e = $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Password')) }
                    if ($PSCredential.Password.Length -eq 0) {
                        $e.InnerText = '';
                    } else {
                        $e.InnerText = $PSCredential.Password | ConvertFrom-SecureString;
                    }
                }
                Save-CredentialsDocument;
                $true;
                break;
            }
            '_Copy' {
                    $Id = 0;
                    while ($Script:CredentialsXmlDocument.SelectSingleNode(('/Credentials/Credential[@ID="{0}"]' -f $Id)) -ne $null) { $Id++ }
                    $XmlElement = $Script:CredentialsXmlDocument.DocumentElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Credential'));
                    $XmlElement.Attributes.Append($Script:CredentialsXmlDocument.CreateAttribute('ID')).Value = $Id.ToString();
                    $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Name')).InnerText = '{0} Copy' -f $XmlElement.Name;
                    $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Login')).InnerText = $XmlElement.Login;
                    $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Password')).InnerText = $XmlElement.Password;
                    $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Url')).InnerText = $XmlElement.Url;
                    $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Notes')).InnerText = $XmlElement.Notes;
                    Save-CredentialsDocument;
                $true;
                break;
            }
            '_Delete' {
                $result = Get-YesOrNo -Caption 'Confirm Delete' -Message 'Are you sure you want to delete this item?';
                if ($result -ne $null) {
                    if ($result) {
                        $XmlElement.Parent.RemoveChild($XmlElement) | Out-Null;
                        Save-CredentialsDocument;
                    }
                    $true;
                } else {
                    $false;
                }
                break;
            }
            default {
                $Label -ne '';
                $Label = $null;
            }
        }
    } while ($Label -ne $null);
}

$Choices = New-Object -TypeName 'System.Collections.ObjectModel.Collection[System.Management.Automation.Host.ChoiceDescription]';
$Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Select', 'Select Credential'));
$Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_New', 'New Credential'));
$Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList 'E_xit', 'Exit'));
do {
    $Index = $Host.UI.PromptForChoice('Password Manager', 'Select command', $Choices, 0);
    if ($Index -eq $null -or $Index -lt 0 -or $Index -ge $Choices.Count) {
        $Label = '';
    } else {
        $Label = $Choices[$Index].Label;
    }
    switch ($Label) {
        '_Select' {
            $XmlNodeList = $Script:CredentialsXmlDocument.SelectNodes('/Credentials/Credential');
            for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
                $XmlElement = $XmlNodeList.Item($i);
                '{0}: {1} = {2}@{3}' -f $XmlElement.ID, $XmlElement.Name, $XmlElement.Login, $XmlElement.Url;
            }
            $XmlElement = $null;
            $n = Read-Host -Prompt 'Enter ID (blank to cancel)';
            if ($n -ne $null -and $n -ne '') {
                $XmlElement = $Script:CredentialsXmlDocument.SelectSingleNode(('/Credentials/Credential[@ID="{0}"]' -f $n.Replace('&', '&amp;').Replace('"', '&quot;')));
                while ($XmlElement -eq $null) {
                    'Not found.' | Write-Waring;
                    $n = Read-Host -Prompt 'Not found. Enter ID (blank to cancel)';
                    if ($n -eq $null -or $n -eq '') { break }
                }
            }
            if ($XmlElement -ne $null) {
                if (-not (Get-ItemCommand -XmlElement $XmlElement)) { $Label = $null }
            }
            break;
        }
        '_New' {
            $Name = Read-Host -Prompt 'Enter Name or blank to cancel';
            if ($Name -ne $null -and $Name.Trim() -ne '') {
                $PSCredential = Get-Credential;
                if ($PSCredential -ne $null) {
                    $Url = Read-Host -Prompt 'Enter Url';
                    if ($Url -eq '') { $Url = '' }
                    $Id = 0;
                    while ($Script:CredentialsXmlDocument.SelectSingleNode(('/Credentials/Credential[@ID="{0}"]' -f $Id)) -ne $null) { $Id++ }
                    $XmlElement = $Script:CredentialsXmlDocument.DocumentElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Credential'));
                    $XmlElement.Attributes.Append($Script:CredentialsXmlDocument.CreateAttribute('ID')).Value = $Id.ToString();
                    $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Name')).InnerText = $Name;
                    if ($PSCredential.UserName.StartsWith('\')) {
                        $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Login')).InnerText = $PSCredential.UserName.Substring(1);
                    } else {
                        $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Login')).InnerText = $PSCredential.UserName;
                    }
                    if ($PSCredential.Password.Length -eq 0) {
                        $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Password')).InnerText = '';
                    } else {
                        $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Password')).InnerText = $PSCredential.Password | ConvertFrom-SecureString;
                    }
                    $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Url')).InnerText = $Url;
                    $XmlElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Notes')) | Out-Null;
                    Save-CredentialsDocument;
                }
            }
            break;
        }
        default { $Label = $null }
    }
} while ($Label -ne $null);