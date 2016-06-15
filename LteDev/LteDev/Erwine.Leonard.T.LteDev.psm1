if ((Get-Module -Name 'Erwine.Leonard.T.IOUtility') -eq $null) { Import-Module -Name 'Erwine.Leonard.T.IOUtility' -ErrorAction Stop }
$Script:IndentText = '  ';

Function Get-ClrSimpleName {
    <#
		.SYNOPSIS
			Get simple CLR full name
 
		.DESCRIPTION
			Displays simple CLR full name of a type

		.EXAMPLE
			Get-ClrSimpleFullName -InputType [System.Uri]

		.EXAMPLE
			$MyObject.GetType() | Get-ClrSimpleFullName -NameOnly

		.EXAMPLE
			$MyObject | Get-ClrSimpleFullName -ForceFullName

		.NOTES
			Makes type names simpler, omitting fully qualified assembly names and generic parameter count references.
	#>
	[CmdletBinding(DefaultParameterSetName = 'Type')]
	[OutputType([string])]
	Param(
		# Type of CLR object
		[Parameter(Mandatory = $true, ValueFromPipeline = $true, Position = 0, ParameterSetName = 'Type')]
		[System.Type]$InputType,
        
		# Object for get CRL full name
		[Parameter(Mandatory = $true, ValueFromPipeline = $true, Position = 0, ParameterSetName = 'Object')]
		[object[]]$InputObject,
        
		# Always include namespace
        [Parameter(Mandatory = $false)]
        [switch]$ForceFullName,
        
		# Do not include the namespace
        [Parameter(Mandatory = $false)]
        [switch]$NameOnly
    )

    Process {
        if ($PSBoundParameters.ContainsKey('InputObject')) {
            if ($ForceFullName) {
                $InputObject | ForEach-Object { $_.GetType() } | Get-ClrSimpleName -ForceFullName;
            } else {
                $InputObject | ForEach-Object { $_.GetType() } | Get-ClrSimpleName;
            }
        } else {
            if ($InputType.IsArray) {
                if ($ForceFullName) {
                    '{0}[{1}]' -f ($InputType.GetElementType() | Get-ClrSimpleName -ForceFullName), (New-Object -TypeName 'System.String' -ArgumentList (',', ($InputType.GetArrayRank() - 1)));
                } else {
                    '{0}[{1}]' -f ($InputType.GetElementType() | Get-ClrSimpleName), (New-Object -TypeName 'System.String' -ArgumentList (',', ($InputType.GetArrayRank() - 1)));
                }
            } else {
                if ($InputType.IsGenericParameter) {
                    $InputType.Name;
                } else {
                    $n = $InputType.Namespace;
                    if ($NameOnly -or $n -eq $null) {
                        $n = '';
                    } else {
                        $n += '.';
                    }
                    $n += $InputType.Name;
                    if ($InputType.IsGenericType) {
                        if ($n -match '`\d+$') { $n = $n.SubString(0, $n.Length - $Matches[0].Length) }
                        if ($NameOnly) {
                            $n
                        } else {
                            '{0}[{1}]' -f $n, (($InputType.GetGenericArguments() | Get-ClrSimpleName -ForceFullName) -join ', ');
                        }
                    } else {
                        if ($ForceFullName -or $NameOnly) {
                            $n;
                        } else {
                            switch ($n) {
                                'System.String' { 'string'; break; }
                                'System.Boolean' { 'bool'; break; }
                                'System.Int32' { 'int'; break; }
                                'System.Int64' { 'long'; break; }
                                'System.Single' { 'float'; break; }
                                'System.Double' { 'double'; break; }
                                'System.Byte' { 'byte'; break; }
                                'System.Char' { 'char'; break; }
                                'System.Collections.Hashtable' { 'hashtable'; break; }
                                'Object' { 'object'; break; }
                                'System.Management.Automation.SwitchParameter' { 'switch'; break; }
                                default { $n; break; }
                            }
                        }
                    }
                }
            }
        }
    }
}

Function Get-EventUsage {
	[CmdletBinding()]
	[OutputType([string[]])]
	Param(
		[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
		[System.Reflection.EventInfo]$EventInfo
    )
    
    Process {
        '{0}: {1}' -f $EventInfo.Name, ($EventInfo.EventHandlerType | Get-ClrSimpleName);
        $SimpleName = $EventInfo.ReflectedType | Get-ClrSimpleName -NameOnly;
        '{0}${1}.add_{2}({{' -f $Script:IndentText, $SimpleName, $EventInfo.Name;
        $MethodInfo = $EventInfo.EventHandlerType.GetMethod('Invoke');
        $Parameters = @($MethodInfo.GetParameters());
        if ($Parameters.Count -gt 0) {
            '{0}{0}Param(' -f $Script:IndentText;
            for ($i = 0; $i -lt $Parameters.Length; $i++) {
                '{0}{0}{0}[Parameter(Mandatory = $true, Position = {1})]' -f $Script:IndentText, $i;
                $n = '{0}{0}{0}[{1}]${2}{3}' -f $Script:IndentText, ($Parameters[$i].ParameterType | Get-ClrSimpleName), $Parameters[$i].Name.Substring(0, 1).ToUpper(), $Parameters[$i].Name.Substring(1);
                if ($i -lt ($Parameters.Length - 1)) {
                    $n + ',';
                } else {
                    $n;
                }
            }
            '{0}{0})' -f $Script:IndentText;
        }
        '';
        '{0}}});' -f $Script:IndentText
    }
}

Function Get-FieldUsage {
	[CmdletBinding()]
	[OutputType([string[]])]
	Param(
		[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
		[System.Reflection.FieldInfo]$FieldInfo,
        
        [Parameter(Mandatory = $false)]
        [switch]$IncludeSpecialNamed
    )
    
    Process {
        if ($IncludeSpecialNamed -or -not $FieldInfo.IsSpecialName) {
            '{0}: {1}' -f $FieldInfo.Name, ($FieldInfo.FieldType | Get-ClrSimpleName);
            if ($FieldInfo.IsStatic) {
                $FullName = $FieldInfo.DeclaringType | Get-ClrSimpleName;
                '{0}[{1}]${2} = [{3}]::{4};' -f  $Script:IndentText, ($FieldInfo.FieldType | Get-ClrSimpleName), ($FieldInfo.FieldType | Get-ClrSimpleName -NameOnly), $FullName, $FieldInfo.Name;
                if (-not ($FieldInfo.IsInitOnly -or $FieldInfo.IsLiteral)) {
                    '{0}[{1}]::{2} = ${3};' -f $Script:IndentText, $FullName, $FieldInfo.Name, ($FieldInfo.FieldType | Get-ClrSimpleName -NameOnly);
                }
            } else {
                $SimpleName = $FieldInfo.ReflectedType | Get-ClrSimpleName -NameOnly;
                '{0}[{1}]${2} = ${3}.{4};' -f $Script:IndentText, ($FieldInfo.FieldType | Get-ClrSimpleName), ($FieldInfo.FieldType | Get-ClrSimpleName -NameOnly), $SimpleName, $FieldInfo.Name;
                if (-not ($FieldInfo.IsInitOnly -or $FieldInfo.IsLiteral)) {
                    '{0}${1}.{2} = ${3};' -f $Script:IndentText, $SimpleName, $FieldInfo.Name, ($FieldInfo.FieldType | Get-ClrSimpleName -NameOnly);
                }
            }
        }
    }
}

Function Get-PropertyUsage {
	[CmdletBinding()]
	[OutputType([string[]])]
	Param(
		[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
		[System.Reflection.PropertyInfo]$PropertyInfo,
        
        [Parameter(Mandatory = $false)]
        [switch]$IncludeSpecialNamed
    )
    
    Process {
        if ($IncludeSpecialNamed -or -not $PropertyInfo.IsSpecialName) {
            '{0}: {1}' -f $PropertyInfo.Name, ($PropertyInfo.PropertyType | Get-ClrSimpleName);
            $CanRead = $PropertyInfo.CanRead;
            $CanWrite = $PropertyInfo.CanWrite;
            if ($CanRead -and $PropertyInfo.GetGetMethod($false) -eq $null) { $CanRead = $false }
            if ($CanWrite -and $PropertyInfo.GetSetMethod($false) -eq $null) { $CanWrite = $false }
            if ($PropertyInfo.IsStatic) {
                $FullName = $PropertyInfo.DeclaringType | Get-ClrSimpleName;
                if ($CanRead) {
                    '{0}[{1}]${2} = [{3}]::{4};' -f  $Script:IndentText, ($PropertyInfo.PropertyType | Get-ClrSimpleName), ($PropertyInfo.PropertyType | Get-ClrSimpleName -NameOnly), $FullName, $PropertyInfo.Name;
                } else {
                    '{0}[{1}]${2} = $value;' -f  $Script:IndentText, ($PropertyInfo.PropertyType | Get-ClrSimpleName), ($PropertyInfo.PropertyType | Get-ClrSimpleName -NameOnly);
                }
                if ($CanWrite) {
                    '{0}[{1}]::{2} = ${3};' -f $Script:IndentText, $FullName, $PropertyInfo.Name, ($PropertyInfo.PropertyType | Get-ClrSimpleName -NameOnly);
                }
            } else {
                $SimpleName = $PropertyInfo.ReflectedType | Get-ClrSimpleName -NameOnly;
                if ($CanRead) {
                    '{0}[{1}]${2} = ${3}.{4};' -f $Script:IndentText, ($PropertyInfo.PropertyType | Get-ClrSimpleName), ($PropertyInfo.PropertyType | Get-ClrSimpleName -NameOnly), $SimpleName, $PropertyInfo.Name;
                } else {
                    '{0}[{1}]${2} = $value;' -f  $Script:IndentText, ($PropertyInfo.PropertyType | Get-ClrSimpleName), ($PropertyInfo.PropertyType | Get-ClrSimpleName -NameOnly);
                }
                if ($CanWrite) {
                    '{0}${1}.{2} = ${3};' -f $Script:IndentText, $SimpleName, $PropertyInfo.Name, ($PropertyInfo.PropertyType | Get-ClrSimpleName -NameOnly);
                }
            }
        }
    }
}

Function Get-ParameterUsage {
	[CmdletBinding()]
	[OutputType([string[]])]
	Param(
		[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
		[System.Reflection.MethodBase]$Method,
        
		[Parameter(Mandatory = $false)]
		[switch]$IsInitVars,
        
		[Parameter(Mandatory = $false)]
		[switch]$NonOptional,
        
		[Parameter(Mandatory = $false)]
		[switch]$IncludeType
    )
    Process {
        $Parameters = @($Method.GetParameters());
        if ($NonOptional) {
            $OptionalParameters = @($Parameters | Where-Object { -not $_.IsOptional });
            if ($OptionalParameters.Count -eq $Parameters.Count) { $Parameters = @() }
        }
        if ($Parameters.Count -gt 0) {
            if ($IsInitVars) {
                for ($i = 0; $i -lt $Parameters.Length; $i++) {
                    '[{0}]${1} = $value{2}' -f ($Parameters[$i].ParameterType | Get-ClrSimpleName), $Parameters[$i].Name, $i;
                }
            } else {
                if ($IncludeType) {
                    ($Parameters | ForEach-Object { '[{0}]${1}' -f ($_.ParameterType | Get-ClrSimpleName), $_.Name  }) -join ', ';
                } else {
                    ($Parameters | ForEach-Object { '$' + $_.Name  }) -join ', ';
                }
            }
        } else {
            if (-not $IsInitVars) { '' };
        }
    }
}

Function Get-ConstructorUsage {
	[CmdletBinding()]
	[OutputType([string[]])]
	Param(
		[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
		[System.Reflection.ConstructorInfo]$ConstructorInfo
    )
    
    Process {
        $ArgList = Get-ParameterUsage -Method $ConstructorInfo;
        $FullName = $ConstructorInfo.ReflectedType | Get-ClrSimpleName;
        $SimpleName = $ConstructorInfo.ReflectedType | Get-ClrSimpleName -NameOnly;
        '{0}({1});' -f $SimpleName, ((Get-ParameterUsage -Method $ConstructorInfo -IncludeType) -join ', ');
        Get-ParameterUsage -Method $ConstructorInfo -IsInitVars | Out-IndentedText -IndentText $Script:IndentText;
        if ($ArgList.Length -eq 0) {
            '{0}${1} = New-Object -TypeName ''{2}''' -f $Script:IndentText, $SimpleName, $FullName;
        } else {
            '{0}${1} = New-Object -TypeName ''{2}'' -ArgumentList {3}' -f $Script:IndentText, $SimpleName, $FullName, $ArgList;
            $ArgList = Get-ParameterUsage -Method $ConstructorInfo -NonOptional;
            if ($ArgList.Length -gt 0) {'${0} = New-Object -TypeName ''{1}'' -ArgumentList {2}' -f $SimpleName, $FullName, $ArgList  }
        }
    }
}

Function Get-MethodUsage {
	[CmdletBinding()]
	[OutputType([string[]])]
	Param(
		[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
		[System.Reflection.MethodInfo]$MethodInfo,
        
        [Parameter(Mandatory = $false)]
        [switch]$IncludeSpecialNamed
    )
    
    Begin { $LastName = '' }
    
    Process {
        if ($IncludeSpecialNamed -or -not $MethodInfo.IsSpecialName) {
            $ArgList = Get-ParameterUsage -Method $MethodInfo;
            '{0}: {1} {0}({2});' -f $MethodInfo.Name, ($MethodInfo.ReturnType | Get-ClrSimpleName), ((Get-ParameterUsage -Method $MethodInfo -IncludeType) -join ', ');
            Get-ParameterUsage -Method $MethodInfo -IsInitVars | Out-IndentedText -IndentText $Script:IndentText;
            $Pre = '';
            if ($MethodInfo.ReturnType -ne [System.Void]) { $Pre = '[{0}]${1} = ' -f ($MethodInfo.ReturnType | Get-ClrSimpleName), ($MethodInfo.ReturnType | Get-ClrSimpleName -NameOnly) }
            if ($MethodInfo.IsStatic) {
                '{0}{1}[{2}]::{3}({4})' -f $Script:IndentText, $Pre, ($MethodInfo.DeclaringType | Get-ClrSimpleName), $MethodInfo.Name, $ArgList;
            } else {
                '{0}{1}${2}.{3}({4})' -f $Script:IndentText, $Pre, ($MethodInfo.ReflectedType | Get-ClrSimpleName -NameOnly), $MethodInfo.Name, $ArgList;
            }
            if ($ArgList.Length -gt 0 -and ($ArgList = Get-ParameterUsage -Method $MethodInfo -NonOptional).Length -gt 0) {
                if ($MethodInfo.IsStatic) {
                    '{0}{1}[{2}]::{3}({4})' -f $Script:IndentText, $Pre, ($MethodInfo.DeclaringType | Get-ClrSimpleName), $MethodInfo.Name, $ArgList;
                } else {
                    '{0}{1}${2}.{3}({4})' -f $Script:IndentText, $Pre, ($MethodInfo.ReflectedType | Get-ClrSimpleName -NameOnly), $MethodInfo.Name, $ArgList;
                }
            }
        }
    }
}

Function Get-TypeUsage {
	<#
		.SYNOPSIS
			Get object type sample usage
 
		.DESCRIPTION
			Displays sample code text for an object type, to demonstrate usage

		.EXAMPLE
			Get-TypeUsage -InputType [System.Uri]

		.EXAMPLE
			$MyObject.GetType() | Get-TypeUsage -Methods

		.EXAMPLE
			$MyObject | Get-TypeUsage

		.NOTES
			If none of the optional switches are used, then all information will be returned.
	#>
	[CmdletBinding(DefaultParameterSetName = 'Type')]
	[OutputType([string[]])]
	Param(
		# Type of CLR object
		[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true, ParameterSetName = 'Type')]
		[Type]$InputType,
        
		# Object for which to get usage info
		[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true, ParameterSetName = 'Object')]
		[object[]]$InputObject,
        
		# Return constructors
		[Parameter(Mandatory = $false)]
		[switch]$Constructors,
        
		# Return methods
		[Parameter(Mandatory = $false)]
		[switch]$Methods,
        
		# Return properties
		[Parameter(Mandatory = $false)]
		[switch]$Properties,
        
		# Return fields
		[Parameter(Mandatory = $false)]
		[switch]$Fields,
        
		# Return events
		[Parameter(Mandatory = $false)]
        $Events,
        
		# Return static members
		[Parameter(Mandatory = $false)]
		[switch]$Static,
        
		# Return instance (non-static) members
		[Parameter(Mandatory = $false)]
		[switch]$Instance,
        
		# Include inherited members
		[Parameter(Mandatory = $false)]
		[switch]$Inherited,
        
		# Include base type names
		[Parameter(Mandatory = $false)]
		[switch]$ShowBaseTypes
	)
	
    Begin {
        $Options = @{
            Constructors = $Constructors.IsPresent;
            Methods = $Methods.IsPresent;
            Properties = $Properties.IsPresent;
            Fields = $Fields.IsPresent;
            Events = $Events.IsPresent;
            Static = $Static.IsPresent;
            Instance = $Instance.IsPresent;
            Inherited = $Inherited.IsPresent;
            ShowBaseTypes = $Inheritance.ShowBaseTypes;
        };
        $IsExplicit = $false;
        foreach ($key in $Options.Keys) { if ($Options[$key]) { $IsExplicit = $true; break; } }
        if ($IsExplicit) {
            if (-not ($Constructors -or $Methods -or $Properties -or $Fields -or $Events)) {
                $Options.Constructors = $true;
                $Options.Methods = $true;
                $Options.Properties = $true;
                $Options.Fields = $true;
                $Options.Events = $true;
            }
            if (-not ($Static -or $Instance)) {
                $Options.Static = $true;
                $Options.Instance = $true;
            }
        } else {
            foreach ($key in @($Options.Keys)) { $Options[$key] = $true; }
        }
        
        $BindingFlags = [System.Reflection.BindingFlags]::Public;
        if ($Options.Static) { [System.Reflection.BindingFlags]$BindingFlags = $BindingFlags -bor [System.Reflection.BindingFlags]::Static }
        if ($Options.Instance) { [System.Reflection.BindingFlags]$BindingFlags = $BindingFlags -bor [System.Reflection.BindingFlags]::Instance }
        if (-not $Options.Inherited) { [System.Reflection.BindingFlags]$BindingFlags = $BindingFlags -bor [System.Reflection.BindingFlags]::DeclaredOnly }
    }

	Process {
        if ($PSBoundParameters.ContainsKey('InputObject')) {
            $splat = @{ InputType = $null }
            foreach ($key in $Options) { if ($Options[$key]) { $splat.Add($key, (New-Object -Typename 'System.Management.Automation.SwitchParameter' -ArgumentList $true)) } }
            foreach ($obj in $InputObject) {
                $splat.InputType = $obj.GetType();
                Get-TypeUsage @splat;
            }
            break;
        } else {
            $FullName = $InputType | Get-ClrSimpleName;
            $FullName;
            $SimpleName = $InputType | Get-ClrSimpleName -NameOnly;
            if ($Options.ShowbaseTypes) {
                if ($InputType.BaseType.BaseType -ne $null) {
                    $Script:IndentText + 'Inherits From:';
                    for ($t = $InputType.BaseType; $t.BaseType -ne $null; $t = $t.BaseType) { '{0}{0}$obj -is [{1}]' -f $Script:IndentText, ($t | Get-ClrSimpleName) };
                }
                $InterfaceArray = $InputType.GetInterfaces();
                if ($InterfaceArray.Length -gt 0) {
                    $Script:IndentText + 'Implements:';
                    $InterfaceArray | Get-ClrSimpleName | Foreach-Object { '{0}{0}$obj -is [{1}]' -f $Script:IndentText, $_ } | Out-IndentedText -IndentText $Script:IndentText;
                }
            }
            if ($Options.Events) {
                $EventArray = @($InputType.GetEvents($BindingFlags) | Sort-Object -Property 'Name');
                if ($EventArray.Count -gt 0) {
                    $Script:IndentText + 'Events:';
                    $EventArray | Get-EventUsage | Out-IndentedText -IndentText $Script:IndentText -Level 2;
                }
            }
            if ($Options.Fields) {
                $FieldArray = @($InputType.GetFields($BindingFlags) | Sort-Object -Property 'Name');
                if ($FieldArray.Count -gt 0) {
                    $Script:IndentText + 'Fields:';
                    $FieldArray | Get-FieldUsage | Out-IndentedText -IndentText $Script:IndentText -Level 2;
                }
            }
            if ($Options.Properties) {
                $PropertyArray = @($InputType.GetProperties($BindingFlags) | Sort-Object -Property 'Name');
                if ($PropertyArray.Count -gt 0) {
                    $Script:IndentText + 'Properties:';
                    $PropertyArray | Get-PropertyUsage | Out-IndentedText -IndentText $Script:IndentText -Level 2;
                }
            }
            if ($Options.Constructors -and -not ($InputType.IsAbstract -or $InputType.IsInterface)) {
                $ConstructorArray = $InputType.GetConstructors();
                if ($ConstructorArray.Count -gt 0) {
                    $Script:IndentText + 'Constructors:';
                    $ConstructorArray | Get-ConstructorUsage | Out-IndentedText -IndentText $Script:IndentText -Level 2;
                }
            }
            if ($Options.Methods) {
                $MethodArray = $InputType.GetMethods($BindingFlags) | Where-Object { -not $_.IsSpecialName } | Group-Object -Property 'Name' | Sort-Object -Property 'Name';
                if ($MethodArray.Count -gt 0) {
                    $Script:IndentText + 'Methods:';
                    foreach ($GroupInfo in $MethodArray) {
                        if ($GroupInfo.Group.Count -eq 1) {
                            $GroupInfo.Group[0] | Get-MethodUsage | Out-IndentedText -IndentText $Script:IndentText -Level 2;
                        } else {
                            '{0}{0}{1} {2} Overrides' -f $Script:IndentText, $GroupInfo.Name, $GroupInfo.Group.Count;
                            $GroupInfo.Group | Get-MethodUsage | Out-IndentedText -IndentText $Script:IndentText -Level 3;
                        }
                    }
                }
            }
        }
	}
}

Function New-HelpItemsDocument {
	<#
		.SYNOPSIS
			Create new XML Help document
 
		.DESCRIPTION
			Generates empty MAML help XML document.
	#>
	[CmdletBinding()]
	[OutputType([System.Xml.XmlDocument])]
	Param()
	
	$XmlDocument = New-Object -TypeName 'System.Xml.XmlDocument';
	$XmlDocument.AppendChild($XmlDocument.CreateElement('helpItems', 'http://msh')).Attributes.Append('schema').Value = 'maml';
	$XmlDocument | Write-Output;
}

Function Assert-HelpItemsDocument {
	[CmdletBinding()]
	[OutputType([System.Xml.XmlDocument])]
	Param(
		[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
		[ValidateScript({ $_.DocumentElement -ne $null -and $_.DocumentElement.LocalName -eq 'helpItems' -and $_.DocumentElement.NamespaceURI -eq 'http://msh' })]
		[System.Xml.XmlDocument]$XmlDocument
	)
	
	Process {
		if ($XmlDocument.DocumentElement -ne $null -and $XmlDocument.DocumentElement.LocalName -eq 'helpItems' -and $XmlDocument.
	}
	$XmlDocument = New-Object -TypeName 'System.Xml.XmlDocument';
	$XmlDocument.AppendChild($XmlDocument.CreateElement('helpItems', 'http://msh')).Attributes.Append('schema').Value = 'maml';
	$XmlDocument | Write-Output;
}

Function Add-CommandNode {
	<#
		.SYNOPSIS
			Add command element
 
		.DESCRIPTION
			Adds commmand element to MAML help file
	#>
	[CmdletBinding()]
	[OutputType([System.Xml.XmlElement])]
	Param(
		# Command object used to generate MAML xml
		[Parameter(Position = 0, ValueFromPipeline = $true)]
		[ValidateScript({ $_ -is [System.Management.Automation.CmdletInfo] -or $_ -is [System.Management.Automation.FunctionInfo] })]
		[System.Management.Automation.CommandInfo]$CommandInfo,
		
		# Command object used to generate MAML xml
		[Parameter(Position = 0, ValueFromPipeline = $true)]
		[ValidateScript({ $_.DocumentElement -ne $null -and $_.DocumentElement.LocalName -eq 'helpItems' -and $_.DocumentElement.NamespaceURI -eq 'http://msh' })]
		[System.Xml.XmlDocument]$HelpItems
	)
	
	Begin {
		$XmlDocument = New-Object -TypeName 'System.Xml.XmlDocument';
		$XmlDocument.LoadXml(@'
<helpItems xmlns="http://msh" schema="maml" />
'@);
#<command:command xmlns:maml="http://schemas.microsoft.com/maml/2004/10" xmlns:command="http://schemas.microsoft.com/maml/dev/command/2004/10"
#	xmlns:dev="http://schemas.microsoft.com/maml/dev/2004/10" xmlns:MSHelp="http://msdn.microsoft.com/mshelp" />
	}

	Process {
		$XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('command', 'command', 'http://schemas.microsoft.com/maml/dev/command/2004/10'));
		$XmlElement.Attributes.Append($XmlDocument.CreateAttribute('xmlns', 'maml', 'http://www.w3.org/2000/xmlns/')).Value = 'http://schemas.microsoft.com/maml/2004/10';
		$XmlElement.Attributes.Append($XmlDocument.CreateAttribute('xmlns', 'dev', 'http://www.w3.org/2000/xmlns/')).Value = 'http://schemas.microsoft.com/maml/dev/2004/10';
		$XmlElement.Attributes.Append($XmlDocument.CreateAttribute('xmlns', 'MSHelp', 'http://www.w3.org/2000/xmlns/')).Value = 'http://msdn.microsoft.com/mshelp';
}

Function Convert-CommandToMamlHelp {
	<#
		.SYNOPSIS
			Get MAML help for Cmdlet or Function
 
		.DESCRIPTION
			Generates MAML help XML from a System.Management.Automation.CommandInfo object.
	#>
	[CmdletBinding()]
	[OutputType([System.Xml.XmlDocument])]
	Param(
		# Command object used to generate MAML xml
		[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
		[ValidateScript({ $_ -is [System.Management.Automation.CmdletInfo] -or $_ -is [System.Management.Automation.FunctionInfo] })]
		[System.Management.Automation.CommandInfo]$CommandInfo
	)
	
	Begin {
		$XmlDocument = New-Object -TypeName 'System.Xml.XmlDocument';
		$XmlDocument.LoadXml(@'
<helpItems xmlns="http://msh" schema="maml" />
'@);
#<command:command xmlns:maml="http://schemas.microsoft.com/maml/2004/10" xmlns:command="http://schemas.microsoft.com/maml/dev/command/2004/10"
#	xmlns:dev="http://schemas.microsoft.com/maml/dev/2004/10" xmlns:MSHelp="http://msdn.microsoft.com/mshelp" />
	}

	Process {
		$XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('command', 'command', 'http://schemas.microsoft.com/maml/dev/command/2004/10'));
		$XmlElement.Attributes.Append($XmlDocument.CreateAttribute('xmlns', 'maml', 'http://www.w3.org/2000/xmlns/')).Value = 'http://schemas.microsoft.com/maml/2004/10';
		$XmlElement.Attributes.Append($XmlDocument.CreateAttribute('xmlns', 'dev', 'http://www.w3.org/2000/xmlns/')).Value = 'http://schemas.microsoft.com/maml/dev/2004/10';
		$XmlElement.Attributes.Append($XmlDocument.CreateAttribute('xmlns', 'MSHelp', 'http://www.w3.org/2000/xmlns/')).Value = 'http://msdn.microsoft.com/mshelp';
		if ($CommandInfo -is [System.Management.Automation.FunctionInfo]) {
			# Boolean CmdletBinding
			# System.String Description
			# System.Management.Automation.ScriptBlock ScriptBlock
		} else {
			# System.Type ImplementingType
			# System.Management.Automation.PSSnapInInfo PSSnapIn

		}
		# System.Management.Automation.CommandTypes CommandType
		# System.String DefaultParameterSet
		# System.String Definition
		# System.String HelpFile
		# System.Management.Automation.PSModuleInfo Module
		# System.String ModuleName
		# System.String Name
		# System.String Noun
		# System.Management.Automation.ScopedItemOptions Options
		# System.Collections.ObjectModel.ReadOnlyCollection`1[System.Management.Automation.PSTypeName] OutputType
		# System.Collections.Generic.Dictionary`2[System.String,System.Management.Automation.ParameterMetadata] Parameters
		# System.Collections.ObjectModel.ReadOnlyCollection`1[System.Management.Automation.CommandParameterSetInfo] ParameterSets
		# System.Management.Automation.RemotingCapability RemotingCapability
		# System.String Source
		# System.String Verb
		# System.Version Version
		# System.Management.Automation.SessionStateEntryVisibility Visibility
		<#
    xe ($command + "command") -maml $maml -command $command -dev $dev {
            xe ($command + "details") {
                xe ($command + "name") {"$commandName"}
                xe ($maml + "description") {
                    xe ($maml + "para") {"TODO Add Short description"}
                }
                xe ($maml + "copyright") {
                    xe ($maml + "para") {}
                }
                xe ($command + "verb") {"$(($CommandName -split '-')[0])"}
                xe ($command + "noun") {"$(($commandName -split '-')[1])"}
                xe ($dev + "version") {}
            }
            xe ($maml + "description") {
                xe ($maml + "para") {"TODO Add Long description"}
            }
            xe ($command + "syntax") {
                xe ($command + "syntaxItem") {
                $parameters | foreach { 
                    xe ($command + "name") {"$commandName"}
                        xe ($command + "parameter") -require "false" -variableLength "false" -globbing "false" -pipelineInput "false" -postion "0" {
                            xe ($maml + "name") {"$($_.Name)"}
                            xe ($maml + "description") {
                                xe ($maml + "para") {"TODO Add $($_.Name) Description"}
                            }
                            xe ($command + "parameterValue") -required "false" -variableLength "false" {"$($_.ParameterType.Name)"}
                        }
                    }
                }
            }
            xe ($command + "parameters") {
                $parameters | foreach { 
                xe ($command + "parameter") -required "false" -variableLength "false" -globbing "false" -pipelineInput "false (ByValue)" -position "0" {
                    xe ($maml + "name") {"$($_.Name)"}
		    xe ($maml + "description") {
			xe ($maml + "para") {"TODO Add $($_.Name) Description"}
                    }
		    xe ($command + "parameterValue") -required "true" -variableLength "false" {"$($_.ParameterType.Name)"}
                    xe ($dev + "type") {
                        xe ($maml + "name") {"$($_.ParameterType.Name)"}
			xe ($maml + "uri"){}
                    }
		    xe ($dev + "defaultValue") {}
                }
                }
            }
	    xe ($command + "inputTypes") {
                xe ($command + "inputType") {
                    xe ($dev + "type") {
                        xe ($maml + "name") {"TODO Add $commandName inputType"}
                        xe ($maml + "uri") {}
                        xe ($maml + "description") {
                            xe ($maml + "para") {}
                        }
                    }
			xe ($maml + "description") {}
                }
            }
	    xe ($command + "returnValues") {
		xe ($command + "returnValue") {
		    xe ($dev + "type") {
		        xe ($maml + "name") {"TODO Add $commandName returnType"}
                        xe ($maml + "uri") {}
                        xe ($maml + "description") {
                            xe ($maml + "para") {}
                        }
                    }
		    xe ($maml + "description") {}
		}
	    }
            xe ($command + "terminatingErrors") {}
	    xe ($command + "nonTerminatingErrors") {}
	    xe ($maml + "alertSet") {
		xe ($maml + "title") {}
		xe ($maml + "alert") {
		    xe ($maml + "para") {}
                }
            }
            xe ($command + "examples") {
		xe ($command + "example") {
                    xe ($maml + "title") {"--------------  EXAMPLE 1 --------------"}
                    xe ($maml + "introduction") {
                        xe ($maml + "para") {"C:\PS&gt;"}
                    }
                    xe ($dev + "code") {"TODO Add $commandName Example code"}
                    xe ($dev + "remarks") {
                        xe ($maml + "para") {"TODO Add $commandName Example Comment"}
                        xe ($maml + "para") {}
                        xe ($maml + "para") {}
                        xe ($maml + "para") {}
                        xe ($maml + "para") {}
                    }
                    xe ($command + "commandLines") {
                        xe ($command + "commandLine") {
                            xe ($command + "commandText") {}
                        }
                    }
                }
            }   
            xe ($maml + "relatedLinks") {
                xe ($maml + "navigationLink") {
		    xe ($maml + "linkText") {"$commandName"}
		    xe ($maml + "uri") {}
                }
            }
        }
		#>
	}
}