@{

# Script module or binary module file associated with this manifest.
# RootModule = 'Module.psd1'
ModuleToProcess = 'Erwine.Leonard.T.WindowsForms.psm1'

# Version number of this module.
ModuleVersion = '0.1.0.0'

# ID used to uniquely identify this module
GUID = '8bfcd87a-bb65-48dd-ba84-5dc07bcad763'

# Author of this module
Author = 'Leonard T. Erwine'

# Company or vendor of this module
CompanyName = 'Leonard T. Erwine'

# Copyright statement for this module
Copyright = '(c) 2015 Leonard T. Erwine. All rights reserved.'

# Description of the functionality provided by this module
Description = 'Create and display windows forms and controls.'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '2.0'

# Name of the Windows PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module
DotNetFrameworkVersion = '2.0'

# Minimum version of the common language runtime (CLR) required by this module
CLRVersion = '2.0'

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
FunctionsToExport = @('New-DrawingPoint', 'New-DrawingSize', 'New-DrawingColor', 'Get-GraphicsUnit', 'Get-FontStyle', 'Get-GenericFontFamily', 'New-FontFamily', 
	'New-DrawingFont', 	'New-FormsPadding', 'Show-FormsControl', 'Hide-FormsControl', 'Enable-FormsControl', 'Disable-FormsControl', 'Set-FormControlProperties',
	'Set-ScrollableControlProperties', 'Set-FormButtonProperties', 'Set-FormControlAnchor', 'Set-FormControlBackColor', 'Set-FormControlDock', 'Set-FormControlFont', 
	'Set-FormControlForeColor', 'Set-FormControlLocation', 'Set-FormControlMargin', 'Set-MaximumFormControlSize', 'Set-MinimumFormControlSize', 'Set-FormControlPadding', 
	'Set-FormControlSize', 'Set-FormControlOnClick', 'Set-FormControlOnGotFocus', 'Set-FormControlOnKeyDown', 'Set-FormControlOnKeyPress', 'Set-FormControlOnKeyUp', 
	'Set-FormControlOnLostFocus', 'Set-FormControlOnMouseClick', 'Set-FormControlOnTextChanged', 'Set-ButtonControlOnDoubleClick', 'Set-ButtonControlOnMouseDoubleClick', 
	'New-FormsButton', 'Add-FormControl', 'Set-FormStartPosition', 'Set-FormSize', 'Set-FormLocation', 'Set-MaximumFormSize', 'Set-MinimumFormSize', 'Set-FormAcceptButton', 
	'New-WindowObject', 'Get-ParentWindowsForm', 'New-TableLayoutPanel', 'New-LayoutColumnStyle', 'Get-LayoutPanelColumnStyle', 'Set-LayoutPanelColumnStyle',
	'Set-LayoutPanelColumnWidth', 'Add-LayoutPanelColumnStyle', 'Push-LayoutPanelColumnStyle', 'Remove-LayoutPanelColumnStyle', 'Clear-LayoutPanelColumnStyles',
	'New-LayoutRowStyle', 'Get-LayoutPanelRowStyle', 'Set-LayoutPanelRowStyle', 'Set-LayoutPanelRowHeight', 'Add-LayoutPanelRowStyle', 'Push-LayoutPanelRowStyle', 
	'Remove-LayoutPanelRowStyle', 'Clear-LayoutPanelRowStyles', 'Set-LayoutPanelSizeType', 'Add-TableLayoutControl', 'Remove-TableLayoutControl', 'Clear-TableLayoutControls',
	'New-FormsLabel', 'New-DataGridView', 'Add-DataGridViewTextBoxColumn')

# Cmdlets to export from this module
CmdletsToExport = '*'

# Variables to export from this module
VariablesToExport = '*'

# Aliases to export from this module
AliasesToExport = @('New-FormsMargin')

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

