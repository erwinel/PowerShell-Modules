#
# Module manifest for module 'Erwine.Leonard.T.IOUtility'
#
# Generated by: Leonard T. Erwine
#
# Generated on: 9/4/2024
#

@{

# Script module or binary module file associated with this manifest.
RootModule = 'Erwine.Leonard.T.IOUtility.psm1'

# Version number of this module.
ModuleVersion = '2.0.0'

# Supported PSEditions
# CompatiblePSEditions = @()

# ID used to uniquely identify this module
GUID = '4996a6d0-2d39-4c6c-836b-2842214639de'

# Author of this module
Author = 'Leonard T. Erwine'

# Company or vendor of this module
CompanyName = 'Leonard T. Erwine'

# Copyright statement for this module
Copyright = '(c) 2024 Leonard Thomas Erwine. All rights reserved.'

# Description of the functionality provided by this module
Description = 'Provides utility functions that are useful for advanced input/output operations.'

# Minimum version of the PowerShell engine required by this module
# PowerShellVersion = ''

# Name of the PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
# DotNetFrameworkVersion = ''

# Minimum version of the common language runtime (CLR) required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
# ClrVersion = ''

# Processor architecture (None, X86, Amd64) required by this module
# ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
# RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
# RequiredAssemblies = @()

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
# TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
# FormatsToProcess = @()

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
# NestedModules = @()

# Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
FunctionsToExport = @('Get-TextEncoding', 'Get-StringComparer', 'Get-IndexOfCharacter', 'Get-CharacterClass', 'Test-CharacterClass', 'Optimize-WhiteSpace', 'Remove-ZeroPadding', 'Out-IndentedText', 'Get-IndentLevel', 'Out-UnindentedText',
    'Assert-IsNotNull', 'Assert-IsType', 'Assert-IsString', 'Assert-IsPsEnumerable', 'Invoke-WhenNotNull', 'Invoke-WhenIsType', 'Invoke-WhenIsString', 'Invoke-WhenIsPsEnumerable', 'Read-ShortIntegerFromStream', 'Read-UnsignedShortIntegerFromStream',
    'Read-IntegerFromStream', 'Read-UnsignedIntegerFromStream', 'Read-LongIntegerFromStream', 'Read-UnsignedLongIntegerFromStream', 'Write-ShortIntegerToStream', 'Write-UnsignedShortIntegerToStream', 'Write-IntegerToStream',
    'Write-UnsignedIntegerToStream', 'Write-LongIntegerToStream', 'Write-UnsignedLongIntegerToStream', 'Read-TinyLengthEncodedBytes', 'Read-ShortLengthEncodedBytes', 'Read-LengthEncodedBytes', 'Write-TinyLengthEncodedBytes',
    'Write-ShortLengthEncodedBytes', 'Write-LengthEncodedBytes', 'New-MemoryStream', 'Get-MinBase64BlockSize', 'ConvertTo-Base64String', 'ConvertFrom-Base64String', 'ConvertTo-SafeFileName', 'Use-Location', 'Get-AppDataPath', 'Use-TempFolder',
    'Resolve-OptionalPath', 'Get-PathStringSegments', 'Optimize-PathString', 'Compare-PathStrings', 'Expand-GZip', 'Compress-GZip')

# Cmdlets to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no cmdlets to export.
CmdletsToExport = @()

# Variables to export from this module
VariablesToExport = '*'

# Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
AliasesToExport = @()

# DSC resources to export from this module
# DscResourcesToExport = @()

# List of all modules packaged with this module
# ModuleList = @()

# List of all files packaged with this module
FileList = @('Text-Character-And-String.ps1', 'Type-Assertion.ps1', 'System-IO-Stream.ps1', 'Base64.ps1', 'Filesystem.ps1', 'GZip.ps1', 'README.md', 'about_Erwine.Leonard.T.IOUtility.help.txt', 'IOUtility.tests.ps1',
    'Test-CharacterClass.tests.ps1')

# Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
PrivateData = @{

    PSData = @{

        # Tags applied to this module. These help with module discovery in online galleries.
        # Tags = @()

        # A URL to the license for this module.
		LicenseUri = 'https://github.com/lerwine/PowerShell-Modules/blob/master/LICENSE'

        # A URL to the main website for this project.
		ProjectUri = 'https://github.com/lerwine/PowerShell-Modules/tree/master/IOUtility'

        # A URL to an icon representing this module.
        # IconUri = ''

        # ReleaseNotes of this module
        # ReleaseNotes = ''

        # Prerelease string of this module
        # Prerelease = ''

        # Flag to indicate whether the module requires explicit user acceptance for install/update/save
        # RequireLicenseAcceptance = $false

        # External dependent modules of this module
        # ExternalModuleDependencies = @()

    } # End of PSData hashtable

} # End of PrivateData hashtable

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''

}

