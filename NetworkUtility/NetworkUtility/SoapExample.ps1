$Script:XmlNamespaces = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
    xml = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'xml.xsd'; Uri = 'http://www.w3.org/XML/1998/namespace'; }
    xsd = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'XmlSchema.xsd'; Uri = 'http://www.w3.org/2001/XMLSchema'; }
    xsi = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'XMLSchema-instance.xsd'; Uri = 'http://www.w3.org/2001/XMLSchema-instance'; }
    TypeLibrary = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'TypeLibrary.xsd'; Uri = 'http://www.w3.org/2001/03/XMLSchema/TypeLibrary'; }
    env = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'soap-envelope.xsd'; Uri = 'http://www.w3.org/2003/05/soap-envelope'; }
    enc = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'soap-encoding.xsd'; Uri = 'http://www.w3.org/2001/06/soap-encoding'; }
    soap = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'soap11envelope.xsd'; Uri = 'http://schemas.xmlsoap.org/soap/envelope/'; }
    soapenc = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'soap11encoding.xsd'; Uri = 'http://schemas.xmlsoap.org/soap/encoding/'; }
    tm = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = ''; Uri = 'http://microsoft.com/wsdl/mime/textMatching/'; }
    wsdl = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'wsdl.xsd'; Uri = 'http://schemas.xmlsoap.org/wsdl/'; }
    http = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'wsdl11html.xsd'; Uri = 'http://schemas.xmlsoap.org/wsdl/http/'; }
    mime = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'wsdl11mime.xsd'; Uri = 'http://schemas.xmlsoap.org/wsdl/mime/'; }
    soap12 = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ File = 'wsdl11soap12.xsd'; Uri = 'http://schemas.xmlsoap.org/wsdl/soap12/'; }
};


Function Add-XmlElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Alias('Xml', 'XmlElement', 'Element', 'Document', 'XmlDocument')]
        [ValidateScript({ $_ -is [System.Xml.XmlElement] -or $_ -is [System.Xml.XmlDocument]})]
        [System.Xml.XmlNode]$Parent,
        
        [ValidateScript({ $Script:XmlNamespaces.($_) -ne $null })]
        [Alias('NamespaceKey', 'Key')]
        [string]$NsKey,
        
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ [System.Xml.XmlConvert]::EncodeLocalName($_) -eq $_ })]
        [Alias('Name')]
        [string]$LocalName,
        
        [Parameter(ParameterSetName = 'Optional')]
        [Parameter(Mandatory = $true, ParameterSetName = 'TextNode')]
        [Parameter(Mandatory = $true, ParameterSetName = 'CData')]
        [AllowEmptyString()]
        [string]$Value,
        
        [System.Xml.XmlLinkedNode]$Replace,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'TextNode')]
        [switch]$ForceTextNode,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'CData')]
        [switch]$ForceCData
    )
    
    Process {
        if ($Parent -is [System.Xml.XmlDocument]) {
            $XmlDocument = $Parent;
        } else {
            $XmlDocument = $Parent.OwnerDocument;
        }
        if ($PSBoundParameters.ContainsKey('NsKey')) {
            $NamespaceUri = $Script:XmlNamespaces.($NsKey).Uri;
            $Prefix = $Parent.GetPrefixOfNamespace($NamespaceUri);
            if ($Prefix -eq $null) {
                if ($Parent.GetNamespaceOfPrefix($NsKey) -eq $null) {
                    $Prefix = $NsKey;
                } else {
                    $i = 0;
                    for ($Prefix = $NsKey + $i.ToString(); $Parent.GetNamespaceOfPrefix($NsKey) -ne $null; $Prefix = $NsKey + $i.ToString()) { $i++ }
                }
            }
            $XmlElement = $XmlDocument.CreateElement($Prefix, $LocalName, $NamespaceUri);
        } else {
            $XmlElement = $XmlDocument.CreateElement($LocalName);
        }
        if ($PSBoundParameters.ContainsKey('Replace')) {
            if ($Parent -is [System.Xml.XmlDocument]) {
                if ($Parent.DocumentElement -eq $null) {
                    $XmlElement = $Parent.AppendChild($XmlElement);
                } else {
                    $XmlElement = $Parent.DocumentElement.ReplaceChild($XmlElement, $Replace);
                }
            } else {
                $XmlElement = $Parent.ReplaceChild($XmlElement, $Replace);
            }
        } else {
            $XmlElement = $Parent.AppendChild($XmlElement);
        }
        if ($PSBoundParameters.ContainsKey('Value')) {
            if ($ForceTextNode) {
                Set-XmlElementText -Element $XmlElement -Value $Value -ForceTextNode;
            } else {
                if ($ForceCData) {
                    Set-XmlElementText -Element $XmlElement -Value $Value -ForceCData;
                } else {
                    Set-XmlElementText -Element $XmlElement -Value $Value;
                }
            }
        } else {
            $XmlElement | Write-Output;
        }
    }
}

Function Push-XmlElement {
    [CmdletBinding(DefaultParameterSetName = 'OptionalBefore')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'OptionalBefore')]
        [Parameter(Mandatory = $true, ParameterSetName = 'TextNodeBefore')]
        [Parameter(Mandatory = $true, ParameterSetName = 'CDataBefore')]
        [System.Xml.XmlLinkedNode]$Before,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'ElementAfter')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ValueAfter')]
        [Parameter(Mandatory = $true, ParameterSetName = 'TextNodeAfter')]
        [Parameter(Mandatory = $true, ParameterSetName = 'CDataAfter')]
        [System.Xml.XmlLinkedNode]$After,
        
        [ValidateScript({ $Script:XmlNamespaces.($_) -ne $null })]
        [Alias('NamespaceKey', 'Key')]
        [string]$NsKey,
        
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ [System.Xml.XmlConvert]::EncodeLocalName($_) -eq $_ })]
        [Alias('Name')]
        [string]$LocalName,
        
        [Parameter(ParameterSetName = 'OptionalBefore')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ValueAfter')]
        [Parameter(Mandatory = $true, ParameterSetName = 'TextNodeBefore')]
        [Parameter(Mandatory = $true, ParameterSetName = 'TextNodeAfter')]
        [Parameter(Mandatory = $true, ParameterSetName = 'CDataBefore')]
        [Parameter(Mandatory = $true, ParameterSetName = 'CDataAfter')]
        [AllowEmptyString()]
        [string]$Value,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'TextNodeBefore')]
        [Parameter(Mandatory = $true, ParameterSetName = 'TextNodeAfter')]
        [switch]$ForceTextNode,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'CDataBefore')]
        [Parameter(Mandatory = $true, ParameterSetName = 'CDataAfter')]
        [switch]$ForceCData,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'ElementAfter')]
        [switch]$Novalue
    )
    
    Process {
        if ($PSBoundParameters.ContainsKey('NsKey')) {
            $NamespaceUri = $Script:XmlNamespaces.($NsKey).Uri;
            $Prefix = $Parent.GetPrefixOfNamespace($NamespaceUri);
            if ($Prefix -eq $null) {
                if ($Parent.GetNamespaceOfPrefix($NsKey) -eq $null) {
                    $Prefix = $NsKey;
                } else {
                    $i = 0;
                    for ($Prefix = $NsKey + $i.ToString(); $Parent.GetNamespaceOfPrefix($NsKey) -ne $null; $Prefix = $NsKey + $i.ToString()) { $i++ }
                }
            }
            $XmlElement = $Parent.OwnerDocument.CreateElement($Prefix, $LocalName, $NamespaceUri);
        } else {
            $XmlElement = $Parent.OwnerDocument.CreateElement($LocalName);
        }
        if ($PSBoundParameters.ContainsKey('Before')) {
            $XmlElement = $Parent.InsertBefore($XmlElement, $Before);
        } else {
            $XmlElement = $Parent.InsertAfter($XmlElement, $After);
        }
        if ($PSBoundParameters.ContainsKey('Value')) {
            if ($ForceTextNode) {
                Set-XmlElementText -Element $XmlElement -Value $Value -ForceTextNode;
            } else {
                if ($ForceCData) {
                    Set-XmlElementText -Element $XmlElement -Value $Value -ForceCData;
                } else {
                    Set-XmlElementText -Element $XmlElement -Value $Value;
                }
            }
        } else {
            $XmlElement | Write-Output;
        }
    }
}

Function Set-XmlElementText {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Alias('Xml', 'XmlElement')]
        [System.Xml.XmlElement]$Element,
        
        [Parameter(ParameterSetName = 'Optional')]
        [Parameter(Mandatory = $true, ParameterSetName = 'TextNode')]
        [Parameter(Mandatory = $true, ParameterSetName = 'CData')]
        [AllowEmptyString()]
        [AllowNull()]
        [string]$Value,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'TextNode')]
        [switch]$ForceTextNode,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'CData')]
        [switch]$ForceCData
    )
    
    Process {
        if ($PSBoundParameters.ContainsKey('Value') -and $Value -ne $null) {
            if ($ForceTextNode -or $Value -eq '') {
                $XmlNode = $Parent.OwnerDocument.CreateTextNode($Value);
            } else {
                if ($ForceCData -or $Value.Trim().Length -eq 0) {
                    $XmlNode = $Parent.OwnerDocument.CreateCDataSection($Value) | Out-Null;
                } else {
                    $XmlNode = $Parent.OwnerDocument.CreateCDataSection($Value);
                    $TextNode = $Parent.OwnerDocument.CreateTextNode($Value);
                    if ($CDataSection.OuterXml.Length -gt $TextNode.OuterXml.Length) { $XmlNode = $TextNode }
                }
            }
            if ($XmlElement.IsEmpty) {
                $XmlElement.AppendChild($XmlNode) | Out-Null;
            } else {
                if ($XmlElement.InnerXml -ne $XmlNode.OuterXml) {
                    $XmlElement.RemoveAll();
                    $XmlElement.AppendChild($XmlNode) | Out-Null;
                }
            }
        } else {
            if (-not $XmlElement.IsEmpty) {
                $XmlElement.RemoveAll();
                $XmlElement.IsEmpty = $true;
            }
        }
    }
}

Function Add-XmlAttribute {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Alias('Xml', 'XmlElement', 'Element')]
        [System.Xml.XmlElement]$Parent,
        
        [ValidateScript({ $Script:XmlNamespaces.($_) -ne $null })]
        [Alias('NamespaceKey')]
        [string]$NsKey,
        
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ [System.Xml.XmlConvert]::EncodeLocalName($_) -eq $_ })]
        [Alias('Name')]
        [string]$LocalName,
        
        [AllowEmptyString()]
        [string]$Value
    )
    
    Process {
        if ($PSBoundParameters.ContainsKey('NsKey')) {
            $NamespaceUri = $Script:XmlNamespaces.($NsKey).Uri;
            $Prefix = Assert-XmlNamespace -Key $NsKey -Element $Parent -ReturnPrefix;
            $XmlAttribute = $Parent.Attributes.Append($Parent.OwnerDocument.CreateAttribute($Prefix, $LocalName, $NamespaceUri));
        } else {
            $XmlAttribute = $Parent.Attributes.Append($Parent.OwnerDocument.CreateAttribute($LocalName));
        }
        if ($PSBoundParameters.ContainsKey('Value')) {
            $XmlAttribute.Value = $Value;
        } else {
            $XmlAttribute | Write-Output;
        }
    }
}

Function Assert-XmlNamespace {
    [CmdletBinding(DefaultParameterSetName = 'KeyedAsAttribute')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'KeyedAsAttribute')]
        [Parameter(Mandatory = $true, ParameterSetName = 'KeyedInManager')]
        [ValidateScript({ $Script:XmlNamespaces.($_) -ne $null })]
        [string]$Key,
        
        [Parameter(ParameterSetName = 'OtherAsAttribute')]
        [Parameter(ParameterSetName = 'OtherInManager')]
        [ValidateScript({ $Script:XmlNamespaces.($_) -ne $null })]
        [string]$DefaultPrefix,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'OtherAsAttribute')]
        [Parameter(Mandatory = $true, ParameterSetName = 'OtherInManager')]
        [ValidateScript({ $Script:XmlNamespaces.($_) -ne $null })]
        [Uri]$NamespaceUri,
        
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'KeyedAsAttribute')]
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'OtherAsAttribute')]
        [Alias('XmlElement')]
        [System.Xml.XmlElement]$Element,
        
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'KeyedInManager')]
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'OtherInManager')]
        [Alias('XmlNamespaceManager')]
        [System.Xml.XmlNamespaceManager]$Nsmgr,
        
        [switch]$ReturnPrefix
    )
    
    Process {
        if ($PSCmdlet.ParameterSetName.StartsWith('Keyed')) {
            $NamespaceUri = New-Object -TypeName 'System.Uri' -ArgumentList $Script:XmlNamespaces.($Key).Uri;
            $DefaultPrefix = $Key;
        }
        if ($PSCmdlet.ParameterSetName.EndsWith('AsAttribute')) {
            $Prefix = $Element.GetPrefixOfNamespace($NamespaceUri);
            if ($Prefix -eq $null) {
                if ($DefaultPrefix -ne '' -and $Element.GetNamespaceOfPrefix($DefaultPrefix) -eq $null) {
                    $Prefix = $DefaultPrefix;
                } else {
                    $DefaultPrefix = 'ns';
                    $i = 0;
                    for ($Prefix = $DefaultPrefix + $i.ToString(); $Element.GetNamespaceOfPrefix($DefaultPrefix) -ne $null; $Prefix = $Key + $i.ToString()) { $i++ }
                }
                $XmlElement.Attributes.Append($XmlElement.OwnerDocument.CreateAttribute('xmlns:' + $Prefix, 'http://www.w3.org/2000/xmlns/')).Value = $NamespaceUri;
            }
        } else {
            $Prefix = $Nsmgr.LookupPrefix($NamespaceUri);
            if ($Prefix -eq $null) {
                if ($DefaultPrefix -ne '' -and $Nsmgr.LookupPrefix($DefaultPrefix) -eq $null) {
                    $Prefix = $DefaultPrefix;
                } else {
                    $DefaultPrefix = 'ns';
                    $i = 0;
                    for ($Prefix = $DefaultPrefix + $i.ToString(); $Nsmgr.LookupPrefix($DefaultPrefix) -ne $null; $Prefix = $DefaultPrefix + $i.ToString()) { $i++ }
                }
                $Nsmgr.AddNamespace($Prefix, $NamespaceUri);
            }
        }
        
        if ($ReturnPrefix) { $Prefix | Write-Output }
    }
}

Function New-SoapEnvelope {
    [CmdletBinding(DefaultParameterSetName = 'env')]
    [OutputType([System.Management.Automation.PSObject])]
    Param(
        [Parameter(ParameterSetName = 'env')]
        [switch]$V12,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'soap')]
        [switch]$V11
    )
    $XmlDocument = New-Object -TypeName 'System.Xml.XmlDocument';
    $XmlElement = Add-XmlElement -Parent $XmlDocument -Key $PSCmdlet.ParameterSetName -LocalName 'Envelope';
    if ($V12) { $EncodingKey = 'enc' } else { $EncodingKey = 'soapEnc' }
    Assert-XmlNamespace -Element $XmlElement -Key 'xsd';
    Assert-XmlNamespace -Element $XmlElement -Key 'xsi';
    Assert-XmlNamespace -Element $XmlElement -Key $EncodingKey;
    Add-XmlElement -Parent $XmlElement -Key $PSCmdlet.ParameterSetName -LocalName 'Body' | Out-Null;
    $SoapEnvelope = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
        XmlDocument = $XmlDocument;
        XmlNamespaceManager = New-Object -TypeName 'System.Xml.XmlNamespaceManager' -ArgumentList $XmlDocument.NameTable;
        EnvelopeKey = $PSCmdlet.ParameterSetName;
        EncodingKey = $EncodingKey;
    };
    $SoapEnvelope.PSTypeNames.Add('SoapExample.SoapEnvelope');
    Assert-XmlNamespace -Nsmgr $SoapEnvelope.XmlNamespaceManager -Key 'xsd';
    Assert-XmlNamespace -Nsmgr $SoapEnvelope.XmlNamespaceManager -Key 'xsi';
    Assert-XmlNamespace -Nsmgr $SoapEnvelope.XmlNamespaceManager -Key $EncodingKey;
    $SoapEnvelope | Write-Output;
}

Function Test-SoapEnvelope {
    [CmdletBinding(DefaultParameterSetName = 'env')]
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('Envelope')]
        [System.Management.Automation.PSObject]$SoapEnvelope,
        
        [Parameter(ParameterSetName = 'env')]
        [switch]$V12,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'soap')]
        [switch]$V11
    )
    
    Process {
        if ($SoapEnvelope.PSTypeNames -notcontains 'SoapExample.SoapEnvelope' -or $SoapEnvelope.XmlDocument -eq $null -or $SoapEnvelope.XmlNamespaceManager -eq $null -or `
                $SoapEnvelope.XmlDocument -isnot [System.Xml.XmlDocument] -or $SoapEnvelope.XmlNamespaceManager -isnot [System.Xml.XmlNamespaceManager] -or `
                $SoapEnvelope.EnvelopeKey -eq $null) {
            $false | Write-Output;
        } else {
            if (($V11 -or $V12) -and $SoapEnvelope.EnvelopeKey -ne $PSCmdlet.ParameterSetName) {
                $false | Write-Output;
            } else {
                $XPath = '/{0}:Envelope' -f (Assert-XmlNamespace -Key $SoapEnvelope.EnvelopeKey -Nsmgr $SoapEnvelope.XmlNamespaceManager -ReturnPrefix);
                if ($SoapEnvelope.XmlDocument.SelectSingleNode($XPath, $SoapEnvelope.XmlNamespaceManager) -eq $null) { $false | Write-Output } else { $true | Write-Output }
            }
        }
    }
}

Function Get-SoapBody {
    [CmdletBinding()]
    [OutputType([System.Xml.XmlElement])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('Envelope')]
        [ValidateScript({ $_ | Test-SoapEnvelope })]
        [System.Management.Automation.PSObject]$SoapEnvelope
    )
    
    Process {
        $XPath = '/{0}:Body' -f (Assert-XmlNamespace -Key $SoapEnvelope.EnvelopeKey -Nsmgr $SoapEnvelope.XmlNamespaceManager -ReturnPrefix);
        $XmlElement = $SoapEnvelope.XmlDocument.DocumentElement.SelectSingleNode($XPath, $SoapEnvelope.XmlNamespaceManager);
        if ($XmlElement -eq $null) {
            Add-XmlElement -Parent $SoapEnvelope.XmlDocument.DocumentElement -Key $SoapEnvelope.EnvelopeKey -LocalName 'Body' | Write-Output;
        } else {
            $XmlElement | Write-Output;
        }
    }
}

Function Get-SoapFault {
    [CmdletBinding()]
    [OutputType([System.Xml.XmlElement])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('Envelope')]
        [ValidateScript({ $_ | Test-SoapEnvelope })]
        [System.Management.Automation.PSObject]$SoapEnvelope
    )
    
    Process {
        $XPath = '/{0}:Body/{0}:Fault' -f (Assert-XmlNamespace -Key $SoapEnvelope.EnvelopeKey -Nsmgr $SoapEnvelope.XmlNamespaceManager -ReturnPrefix);
        $XmlElement = $SoapEnvelope.XmlDocument.DocumentElement.SelectSingleNode($XPath, $SoapEnvelope.XmlNamespaceManager);
    }
}

Function Test-SoapFaultExists {
    [CmdletBinding()]
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('Envelope')]
        [ValidateScript({ $_ | Test-SoapEnvelope })]
        [System.Management.Automation.PSObject]$SoapEnvelope
    )
    
    Process {
        if ((Get-SoapFault -SoapEnvelope $SoapEnvelope) -eq $null) {
            $true | Write-Output;
        } else {
            $false | Write-Output;
        }
    }
}


Function Set-SoapFaultDetail {
    [CmdletBinding()]
    [OutputType([System.Xml.XmlElement])]
    Param(
        [Parameter(Mandatory = $true, Position = 0)]
        [Alias('Envelope')]
        [ValidateScript({ $_ | Test-SoapEnvelope })]
        [System.Management.Automation.PSObject]$SoapEnvelope,
        
        [Parameter(Mandatory = $true)]
        [ValidateScript({ [System.Xml.XmlConvert]::EncodeLocalName($_) -eq $_ })]
        [System.Xml.XmlNode[]]$Detail
    )
}

Function Set-Soap11Fault {
    [CmdletBinding()]
    [OutputType([System.Xml.XmlElement])]
    Param(
        [Parameter(Mandatory = $true, Position = 0)]
        [Alias('Envelope')]
        [ValidateScript({ $_ | Test-SoapEnvelope -V11 })]
        [System.Management.Automation.PSObject]$SoapEnvelope,
        
        [Parameter(Mandatory = $true)]
        [ValidateScript({ [System.Xml.XmlConvert]::EncodeLocalName($_) -eq $_ })]
        [string]$FaultCodeName,
        
        [Parameter(Mandatory = $true)]
        [Uri]$FaultCodeNs,
        
        [ValidateScript({ [System.Xml.XmlConvert]::EncodeLocalName($_) -eq $_ })]
        [string]$FaultCodePrefix,
        
        [string]$FaultString,
        
        [Uri]$FaultActor
    )
    
    Process {
        $FaultElement = Get-SoapFault -SoapEnvelope $SoapEnvelope;
        if ($FaultElement -eq $null) {
            $FaultElement = Add-XmlElement -Parent (Get-SoapBody -SoapEnvelope $SoapEnvelope) -Key $SoapEnvelope.EnvelopeKey -LocalName 'Fault';
            $FaultCodeElement = $SoapEnvelope.XmlDocument.CreateElement('faultcode', $FaultElement.NamespaceURI);
            $FaultStringElement = $SoapEnvelope.XmlDocument.CreateElement('faultstring', $FaultElement.NamespaceURI);
            $FaultActorElement = $null;
            $DetailElement = $null;
        } else {
            $Prefix = Assert-XmlNamespace -Key $SoapEnvelope.EnvelopeKey -Nsmgr $SoapEnvelope.XmlNamespaceManager -ReturnPrefix;
            $FaultCodeElement = $FaultElement.SelectSingleNode(('{0}:faultcode' -f $Prefix), $SoapEnvelope.XmlNamespaceManager);
            if ($FaultCodeElement -ne $null) {
                $FaultCodeElement = $FaultElement.RemoveChild($FaultCodeElement);
            } else {
                $FaultCodeElement = $SoapEnvelope.XmlDocument.CreateElement('faultcode', $FaultElement.NamespaceURI);
            }
            $FaultStringElement = $FaultElement.SelectSingleNode(('{0}:faultstring' -f $Prefix), $SoapEnvelope.XmlNamespaceManager);
            if ($FaultStringElement -ne $null) {
                $FaultStringElement = $FaultElement.RemoveChild($FaultStringElement);
            } else {
                $FaultStringElement = $SoapEnvelope.XmlDocument.CreateElement('faultstring', $FaultElement.NamespaceURI);
            }
            $FaultActorElement = $FaultElement.SelectSingleNode(('{0}:faultactor' -f $Prefix), $SoapEnvelope.XmlNamespaceManager);
            if ($FaultActorElement -ne $null) {
                $FaultActorElement = $FaultElement.RemoveChild($FaultActorElement);
            }
            $DetailElement = $FaultElement.SelectSingleNode(('{0}:detail' -f $Prefix), $SoapEnvelope.XmlNamespaceManager);
            if ($DetailElement -ne $null) {
                $DetailElement = $FaultElement.RemoveChild($DetailElement);
            }
            if ($FaultElement.ChildNodes.Count -gt 0) { $FaultElement.RemoveAll() }
        }
        
        $FaultCodeElement = $FaultElement.AppendChild($FaultCodeElement);
        if ($PSBoundParameters.ContainsKey('FaultCodePrefix')) {
            Set-XmlElementText -Element $FaultCodeElement -Value ('{0}:{1}' -f (Assert-XmlNamespace -DefaultPrefix $FaultCodePrefix -NamespaceUri $FaultCodeNs -Element $XmlElement -ReturnPrefix), $FaultCodeName);
        } else {
            Set-XmlElementText -Element $FaultCodeElement -Value ('{0}:{1}' -f (Assert-XmlNamespace -NamespaceUri $FaultCodeNs -Element $XmlElement -ReturnPrefix), $FaultCodeName);
        }
        $FaultStringElement = $FaultElement.AppendChild($FaultStringElement);
        Set-XmlElementText -Element $FaultStringElement -Value $FaultString;
        
        if ($PSBoundParameters.ContainsKey('FaultActor')) {
            if ($FaultActorElement -eq $null) {
                Add-XmlElement -Parent $FaultElement -Key $SoapEnvelope.EnvelopeKey -LocalName 'faultactor' -Value $FaultActor.ToString();
            } else {
                $FaultActorElement = $FaultElement.AppendChild($FaultActorElement);
                Set-XmlElementText -Element $FaultActorElement -Value $FaultActor.ToString();
            }
        }
        
        if ($DetailElement -ne $null) { $FaultElement.AppendChild($DetailElement) | Out-Null }
    }
}

Function Set-Soap12Fault {
    [CmdletBinding()]
    [OutputType([System.Xml.XmlElement])]
    Param(
        [Parameter(Mandatory = $true, Position = 0)]
        [Alias('Envelope')]
        [ValidateScript({ $_ | Test-SoapEnvelope -V11 })]
        [System.Management.Automation.PSObject]$SoapEnvelope,
        
        [Parameter(Mandatory = $true)]
        [ValidateSet('DataEncodingUnknown', 'MustUnderstand', 'Receiver', 'Sender', 'VersionMismatch')]
        [string]$Code,
        
        [Parameter(Mandatory = $true)]
        [string[]]$ReasonText,
        
        [Parameter(Mandatory = $true)]
        [ValidatePattern('^[a-zA-Z][a-zA-Z\d]+(-[a-zA-Z\d]+)*$')]
        [string]$ReasonLang = 'en-US',
        
        [Uri]$FaultNode,
        
        [Uri]$Role
    )
    
    Process {
        $FaultElement = Get-SoapFault -SoapEnvelope $SoapEnvelope;
        $Prefix = Assert-XmlNamespace -Key $SoapEnvelope.EnvelopeKey -Nsmgr $SoapEnvelope.XmlNamespaceManager -ReturnPrefix;
        if ($FaultElement -eq $null) {
            $FaultElement = Add-XmlElement -Parent (Get-SoapBody -SoapEnvelope $SoapEnvelope) -Key $SoapEnvelope.EnvelopeKey -LocalName 'Fault';
            $FaultCodeElement = $SoapEnvelope.XmlDocument.CreateElement('Code', $FaultElement.NamespaceURI);
            $FaultReasonElement = $SoapEnvelope.XmlDocument.CreateElement('Reason', $FaultElement.NamespaceURI);
            $FaultNodeElement = $null;
            $RoleElement = $null;
            $DetailElement = $null;
        } else {
            $FaultCodeElement = $FaultElement.SelectSingleNode(('{0}:Code' -f $Prefix), $SoapEnvelope.XmlNamespaceManager);
            if ($FaultCodeElement -ne $null) {
                $FaultCodeElement = $FaultElement.RemoveChild($FaultCodeElement);
            } else {
                $FaultCodeElement = $SoapEnvelope.XmlDocument.CreateElement('Code', $FaultElement.NamespaceURI);
            }
            $FaultReasonElement = $FaultElement.SelectSingleNode(('{0}:Reason' -f $Prefix), $SoapEnvelope.XmlNamespaceManager);
            if ($FaultReasonElement -ne $null) {
                $FaultReasonElement = $FaultElement.RemoveChild($FaultReasonElement);
                $FaultReasonElement.RemoveAll();
            } else {
                $FaultReasonElement = $SoapEnvelope.XmlDocument.CreateElement('Reason', $FaultElement.NamespaceURI);
            }
            $FaultNodeElement = $FaultElement.SelectSingleNode(('{0}:Node' -f $Prefix), $SoapEnvelope.XmlNamespaceManager);
            if ($FaultNodeElement -ne $null) {
                $FaultNodeElement = $FaultElement.RemoveChild($FaultNodeElement);
            }
            $RoleElement = $FaultElement.SelectSingleNode(('{0}:Role' -f $Prefix), $SoapEnvelope.XmlNamespaceManager);
            if ($RoleElement -ne $null) {
                $RoleElement = $FaultElement.RemoveChild($RoleElement);
            }
            $DetailElement = $FaultElement.SelectSingleNode(('{0}:Detail' -f $Prefix), $SoapEnvelope.XmlNamespaceManager);
            if ($DetailElement -ne $null) {
                $DetailElement = $FaultElement.RemoveChild($DetailElement);
            }
            if ($FaultElement.ChildNodes.Count -gt 0) { $FaultElement.RemoveAll() }
        }
        
        $FaultCodeElement = $FaultElement.AppendChild($FaultCodeElement);
        Set-XmlElementText -Element $FaultCodeElement -Value ('{0}:{1}' -f $Prefix, $Code);
        
        $FaultReasonElement = $FaultElement.AppendChild($FaultReasonElement);
        $ReasonText | ForEach-Object {
            $XmlElement = Add-XmlElement -Parent $FaultReasonElement -Key $SoapEnvelope.EnvelopeKey -LocalName 'Text';
            Add-XmlAttribute -Parent $XmlElement -LocalName 'lang' -Value $ReasonLang
            Set-XmlElementText -Element $FaultActorElement -Value $_;
        }
        
        if ($PSBoundParameters.ContainsKey('FaultNode')) {
            if ($FaultNodeElement -eq $null) {
                Add-XmlElement -Parent $FaultElement -Key $SoapEnvelope.EnvelopeKey -LocalName 'Node' -Value $FaultNode.ToString();
            } else {
                $FaultNodeElement = $FaultElement.AppendChild($FaultNodeElement);
                Set-XmlElementText -Element $FaultNodeElement -Value $FaultNode.ToString();
            }
        }
        
        if ($PSBoundParameters.ContainsKey('Role')) {
            if ($RoleElement -eq $null) {
                Add-XmlElement -Parent $FaultElement -Key $SoapEnvelope.EnvelopeKey -LocalName 'Role' -Value $Role.ToString();
            } else {
                $RoleElement = $FaultElement.AppendChild($RoleElement);
                Set-XmlElementText -Element $RoleElement -Value $Role.ToString();
            }
        }
        
        if ($DetailElement -ne $null) { $FaultElement.AppendChild($DetailElement) | Out-Null }
    }
}

Function Remove-SoapFault {
    [CmdletBinding()]
    [OutputType([System.Xml.XmlElement])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('Envelope')]
        [ValidateScript({ $_ | Test-SoapEnvelope })]
        [System.Management.Automation.PSObject]$SoapEnvelope
    )
    
    Process {
        if ((Get-SoapFault -SoapEnvelope $SoapEnvelope) -eq $null) {
            $true | Write-Output;
        } else {
            $false | Write-Output;
        }
    }
}

Function Test-SoapHeaderExists {
    [CmdletBinding()]
    [OutputType([System.Xml.XmlElement])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('Envelope')]
        [ValidateScript({ $_ | Test-SoapEnvelope })]
        [System.Management.Automation.PSObject]$SoapEnvelope
    )
    
    Process {
        if ((Get-SoapHeader -SoapEnvelope $SoapEnvelope) -eq $null) {
            $true | Write-Output;
        } else {
            $false | Write-Output;
        }
    }
}

Function Get-SoapHeader {
    [CmdletBinding()]
    [OutputType([System.Xml.XmlElement])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('Envelope')]
        [ValidateScript({ $_ | Test-SoapEnvelope })]
        [System.Management.Automation.PSObject]$SoapEnvelope,
        
        [switch]$Create
    )
    
    Process {
        $XPath = '/{0}:Header' -f (Assert-XmlNamespace -Key $SoapEnvelope.EnvelopeKey -Nsmgr $SoapEnvelope.XmlNamespaceManager -ReturnPrefix);
        $XmlElement = $SoapEnvelope.XmlDocument.DocumentElement.SelectSingleNode($XPath, $SoapEnvelope.XmlNamespaceManager);
        if ($XmlElement -eq $null) {
            if ($Create) { Add-XmlElement -Parent $SoapEnvelope.XmlDocument.DocumentElement -Key $SoapEnvelope.EnvelopeKey -LocalName 'Header' | Write-Output }
        } else {
            $XmlElement | Write-Output;
        }
    }
}

Function Invoke-SoapRequest {
    [CmdletBinding()]
    [OutputType([System.Management.Automation.PSObject])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('Envelope')]
        [ValidateScript({ $_ | Test-SoapEnvelope })]
        [System.Management.Automation.PSObject]$SoapEnvelope,
        
        [Parameter(Mandatory = $true, Position = 1)]
        [string]$Url,
        
        [Parameter(Position = 2)]
        [Uri]$SOAPAction
    )
    
    Process {
        $Properties = @{ SoapRequest = $Request; Url = $Url }
        try {
            $Properties['WebRequest'] = [System.Net.WebRequest]::Create($Url);
            if ($PSBoundParameters.ContainsKey('SOAPAction')) {
                $Properties['WebRequest'].Headers.Add('SOAPAction', ('"{0}"' -f $SOAPAction.ToString()));
            } else {
                $Properties['WebRequest'].Headers.Add('SOAPAction', '""');
            }
            $Properties['WebRequest'].ContentType = 'text/xml;charset="utf-8"';
            $Properties['WebRequest'].Accept = 'text/xml'
            $Properties['WebRequest'].Method = 'POST';
        } catch {
            $Properties['Error'] = $_;
        }
        if ($Properties['Error'] -ne $null) {
            $Stream = $Properties['WebRequest'].GetRequestStream();
            try {
                $Request.WriteTo($Stream);
                $Stream.Flush();
            } catch {
                $Properties['Error'] = $_;
            } finally {
                $Stream.Close();
                $Stream.Dispose();
            }
        }
        if ($Properties['Error'] -ne $null) {
            try {
                $Properties['WebResponse'] = $WebRequest.GetResponse();
            } catch [System.Net.WebException] {
                $Properties['Error'] = $_;
                if ($_ -is [System.Net.WebException]) {
                    $Properties['WebResponse'] = $_.Response;
                } else {
                    $Properties['WebResponse'] = $_.Exception.Response;
                }
            }
        }
        if ($Properties['Error'] -ne $null) {
            $Stream = $WebResponse.GetResponseStream();
            try {
                $XmlDocument = New-Object -TypeName 'System.Xml.XmlDocument';
                $XmlDocument.Load($Stream);
                $Properties['SoapResponse'] = $XmlDocument;
            } catch [System.Xml.XmlException] {
                $Properties['Error'] = $_;
            } catch {
                $Properties['Error'] = $_;
            } finally {
                $Stream.Close();
                $Stream.Dispose();
            }
        }
        
        $SoapResponse = New-Object -TypeName 'System.Management.Automation.PSObject' -Property $Properties;
        $SoapResponse.PSTypeNames.Add('SoapExample.SoapResponse');
        $SoapResponse | Write-Output;
    }
}

$XmlDocument.OuterXml;