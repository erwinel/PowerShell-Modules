#
# Module manifest for module 'Erwine.Leonard.T.XmlUtility'
#
# Generated by: Leonard T. Erwine
#
# Generated on: 3/13/2017
#

@{

# Script module or binary module file associated with this manifest.
RootModule = 'Erwine.Leonard.T.XmlUtility.psm1'

# Version number of this module.
ModuleVersion = '1.1'

# Supported PSEditions
# CompatiblePSEditions = @()

# ID used to uniquely identify this module
GUID = 'bd4390dc-a8ad-4bce-8d69-f53ccf8e4163'

# Author of this module
Author = 'Leonard T. Erwine'

# Company or vendor of this module
CompanyName = 'Leonard T. Erwine'

# Copyright statement for this module
Copyright = '(c) 2016 Leonard T. Erwine. All rights reserved.'

# Description of the functionality provided by this module
Description = 'Provides XML functions for advanced manipulation and formatting of XML.'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '4.0'

# Name of the Windows PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
DotNetFrameworkVersion = '4.0'

# Minimum version of the common language runtime (CLR) required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
CLRVersion = '4.0'

# Processor architecture (None, X86, Amd64) required by this module
# ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
# RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
RequiredAssemblies = 'System.Xml'

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
# TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
# FormatsToProcess = @()

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
NestedModules = @('Erwine.Leonard.T.XmlUtilityLib.dll')

# Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
FunctionsToExport = 'New-XmlReaderSettings', 'New-XmlWriterSettings', 'Read-XmlDocument', 'Write-XmlDocument', 'ConvertTo-XmlEncodedName', 
			   'ConvertTo-XmlEncodedNmToken', 'ConvertTo-XmlEncodedLocalName', 'ConvertFrom-XmlEncodedName', 'ConvertTo-XmlString', 
			   'ConvertFrom-XmlString', 'ConvertTo-XmlBinary', 'ConvertFrom-XmlBinary', 'ConvertTo-XmlList', 'ConvertFrom-XmlList', 
			   'Add-XmlAttribute', 'Set-XmlAttribute', 'Add-XmlElement', 'Add-XmlTextElement', 'Set-XmlText'

# Cmdlets to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no cmdlets to export.
CmdletsToExport = '*'

# Variables to export from this module
VariablesToExport = '*'

# Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
AliasesToExport = '*'

# DSC resources to export from this module
# DscResourcesToExport = @()

# List of all modules packaged with this module
# ModuleList = @()

# List of all files packaged with this module
FileList = @('Schemas\base.xsd', 'Schemas\baseConditional.xsd', 'Schemas\block.xsd', 'Schemas\blockCommon.xsd', 'Schemas\blockSoftware.xsd', 'Schemas\command.xsd',
	'Schemas\conditionSet.xsd', 'Schemas\developer.xsd', 'Schemas\developerCommand.rld', 'Schemas\developerCommand.xsd', 'Schemas\developerDscResource.xsd', 
	'Schemas\developerManaged.xsd', 'Schemas\developerManagedClass.xsd', 'Schemas\developerManagedConstructor.xsd', 'Schemas\developerManagedDelegate.xsd', 
	'Schemas\developerManagedEnumeration.xsd', 'Schemas\developerManagedEvent.xsd', 'Schemas\developerManagedField.xsd', 'Schemas\developerManagedInterface.xsd', 
	'Schemas\developerManagedMethod.xsd', 'Schemas\developerManagedNamespace.xsd', 'Schemas\developerManagedOperator.xsd', 'Schemas\developerManagedOverload.xsd',
	'Schemas\developerManagedProperty.xsd', 'Schemas\developerManagedStructure.xsd', 'Schemas\developerReference.xsd', 'Schemas\developerStructure.xsd', 
	'Schemas\developerXaml.xsd', 'Schemas\endUser.xsd', 'Schemas\faq.xsd', 'Schemas\glossary.xsd', 'Schemas\helpItems.xsd', 'Schemas\hierarchy.xsd', 'Schemas\HTMLsymbol.ent', 
	'Schemas\inline.xsd', 'Schemas\inlineCommon.xsd', 'Schemas\inlineSoftware.xsd', 'Schemas\inlineUi.xsd', 'Schemas\ITPro.xsd', 'Schemas\Maml.rld', 'Schemas\Maml.tbr', 
	'Schemas\Maml.xsd', 'Schemas\Maml.xsx', 'Schemas\Maml_HTML.xsl', 'Schemas\Maml_HTML_Style.xsl', 'Schemas\ManagedDeveloper.xsd', 'Schemas\ManagedDeveloperStructure.xsd', 
	'Schemas\ProviderHelp.xsd', 'Schemas\README.md', 'Schemas\shellExecute.xsd', 'Schemas\soap-encoding.xsd', 'Schemas\soap-envelope.xsd', 'Schemas\soap11encoding.xsd', 
	'Schemas\soap11envelope.xsd', 'Schemas\structure.xsd', 'Schemas\structureGlossary.xsd', 'Schemas\structureList.xsd', 'Schemas\structureProcedure.xsd', 
	'Schemas\structureTable.xsd', 'Schemas\structureTaskExecution.xsd', 'Schemas\task.xsd', 'Schemas\troubleshooting.xsd', 'Schemas\TypeLibrary-array.xsd', 
	'Schemas\TypeLibrary-binary.xsd', 'Schemas\TypeLibrary-list.xsd', 'Schemas\TypeLibrary-math.xsd', 'Schemas\TypeLibrary-nn-array.xsd', 'Schemas\TypeLibrary-nn-binary.xsd', 
	'Schemas\TypeLibrary-nn-list.xsd', 'Schemas\TypeLibrary-nn-math.xsd', 'Schemas\TypeLibrary-nn-quantity.xsd', 'Schemas\TypeLibrary-nn-text.xsd', 
	'Schemas\TypeLibrary-quantity.xsd', 'Schemas\TypeLibrary-text.xsd', 'Schemas\TypeLibrary.xsd', 'Schemas\WindowsPhoneSynthesis-core.xsd', 
	'Schemas\WindowsPhoneSynthesis.xsd', 'Schemas\wsdl.xsd', 'Schemas\wsdl11html.xsd', 'Schemas\wsdl11mime.xsd', 'Schemas\wsdl11soap12.xsd', 'Schemas\xhtml-lat1.ent', 
	'Schemas\xhtml-special.ent', 'Schemas\xhtml-symbol.ent', 'Schemas\xml.xsd', 'Schemas\XMLSchema-instance.xsd', 'Schemas\XmlSchema.xsd', 'Schemas\Xslt.xsd')

# Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
PrivateData = @{
	PSData = @{
		# Tags applied to this module. These help with module discovery in online galleries.
		# Tags = @()

		# A URL to the license for this module.
		LicenseUri = 'https://github.com/lerwine/PowerShell-Modules/blob/Work/LICENSE'

		# A URL to the main website for this project.
		ProjectUri = 'https://github.com/lerwine/PowerShell-Modules'

		# A URL to an icon representing this module.
		# IconUri = ''

		# ReleaseNotes of this module
		# ReleaseNotes = ''

	} # End of PSData hashtable
} # End of PrivateData hashtable

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''

}

