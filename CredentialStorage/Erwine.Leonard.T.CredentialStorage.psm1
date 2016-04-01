if ((Get-Module -Name 'Erwine.Leonard.T.IOUtility') -eq $null) { Import-Module -Name 'Erwine.Leonard.T.IOUtility' -ErrorAction Stop }


$Script:CredentialsNamespace = 'urn:Erwine.Leonard.T:PowerShell:CredentialStorage';
$Script:RootElementName = 'CredentialStorage';
$Script:CredentialElementName = 'Credential';

Function New-CredentialDocument {
    [CmdletBinding()]
    [OutputType([System.Xml.XmlDocument])]
    Param()
    
    $CredentialDocument = New-Object -TypeName 'System.Xml.XmlDocument';
    $CredentialDocument.AppendChild($CredentialDocument.CreateElement($Script:RootElementName, $Script:CredentialsNamespace)) | Out-Null;
    $CredentialDocument | Write-Output;
}

Function Test-CredentialParent {
    [CmdletBinding()]
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true, ParameterSetName = 'Element')]
        [Alias('XmlElement', 'CredentialStorage', 'CredentialStorageElement')]
        [System.Xml.XmlElement]$Element
    )
    
    Process { $XmlElement.NamespaceURI -eq $Script:CredentialsNamespace -and $XmlElement.LocalName -eq $Script:RootElementName }
}

Function Test-CredentialDocument {
    [CmdletBinding()]
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true, ParameterSetName = 'Element')]
        [Alias('XmlDocument', 'CredentialStorage', 'CredentialStorageDocument')]
        [System.Xml.XmlDocument]$Document
    )
    
    Process {
        $Document.DocumentElement -ne $null -and $Document.DocumentElement | Test-CredentialParent;
    }
}

Function Test-CredentialElement {
    [CmdletBinding()]
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('XmlElement', 'Credential', 'CredentialElement')]
        [System.Xml.XmlElement]$Element,
        
        [switch]$ValidateChildren
    )
    
    Process {
        if ($XmlElement.NamespaceURI -ne $Script:CredentialsNamespace -or $XmlElement.LocalName -ne $Script:CredentialElementName) {
            $false | Write-Output;
        } else {
            if ($Validate) {
                if ($XmlElement.SelectSingleNode('@Name') -eq $null -or $XmlElement.SelectSingleNode('@UserName') -eq $null -or $XmlElement.SelectSingleNode('@Location') -eq $null -or $XmlElement.SelectSingleNode('@Password') -eq $null) {
                    $false | Write-Output;
                } else {
                    $true | Write-Output;
                }
            } else {
                $true | Write-Output;
            }
        }
    }
}

Function Get-CredentialLookupXPath {
    [CmdletBinding(DefaultParameterSetName = 'Default')]
    [OutputType([string])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Name')]
        [string]$Name,
        
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'UserName')]
        [string]$UserName,
        
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Location')]
        [string]$Location,
        
        [Parameter(Mandatory = $true, Position = 1)]
        [string]$Prefix
    )
    
    Process {
        $XPath = '{0}:{1}' -f $Prefix, $Script:CredentialElementName;
        if ($PSBoundParameters.Count -eq 1) {
            $XPath | Write-Output;
        } else {
            if ($PSBoundParameters.ContainsKey('UserName')) {
                $PredicateNode = '{0}:UserName' -f $Prefix;
                $PredicateValue = $UserName;
            } else {
                if ($PSBoundParameters.ContainsKey('Location')) {
                    $PredicateNode = '{0}:Location' -f $Prefix;
                    $PredicateValue = $Location;
                } else {
                    $PredicateNode = '@Name';
                    $PredicateValue = $Name;
                }
            }
            ('{0}[normalize-whitespace(translate("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", {1}))={2}]' -f $XPath, $PredicateNode, `
                    ($PredicateValue.ToLower() | ConvertTo-WhitespaceNormalizedString | ConvertTo-XPathQuotedString)) | Write-Output;
        }
    }
}