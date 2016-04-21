@{

# Script module or binary module file associated with this manifest.
RootModule = 'Erwine.Leonard.T.CertificateCryptography.psm1'

# Version number of this module.
ModuleVersion = '0.1.0.0'

# ID used to uniquely identify this module
GUID = '2f1be0d9-d0b2-42b7-a477-791e8b2a2f17'

# Author of this module
Author = 'Leonard T. Erwine'

# Company or vendor of this module
CompanyName = 'Leonard T. Erwine'

# Copyright statement for this module
Copyright = '(c) 2015 Leonard T. Erwine. All rights reserved.'

# Description of the functionality provided by this module
Description = 'Encrypt and decrypt using PKI technology.'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '4.0'

# Name of the Windows PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module
DotNetFrameworkVersion = '4.5'

# Minimum version of the common language runtime (CLR) required by this module
CLRVersion = '4.5'

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

# Functions to export from this module
FunctionsToExport = @('New-CryptographyOid', 'New-AsnEncodedData', 'New-X509StoreOpenFlags', 'Get-X509Store', 'New-X509KeyUsageFlags', 'Select-X509Certificate',
    'Show-X509Certificate', 'Read-X509Certificate', 'New-AesManaged', 'Protect-WithRSA', 'Protect-WithX509Certificate', 'Protect-WithSymmetricAlgorithm',
    'Unprotect-WithRSA', 'Unprotect-WithX509Certificate', 'Unprotect-WithSymmetricAlgorithm', 'New-X500DistinguishedName', 'New-SelfSignedCertificate')

# Cmdlets to export from this module
CmdletsToExport = '*'

# Variables to export from this module
VariablesToExport = '*'

# Aliases to export from this module
AliasesToExport = @('New-CryptoObjectIdentifier')

# List of all modules packaged with this module
# ModuleList = @()

# List of all files packaged with this module
# FileList = @()

# Private data to pass to the module specified in RootModule/ModuleToProcess
# PrivateData = ''

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''

}

