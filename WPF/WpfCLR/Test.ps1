cls;
if ((Get-Module -Name 'Erwine.Leonard.T.WPF') -ne $null) { Remove-Module -Name 'Erwine.Leonard.T.WPF' }
Import-Module -Name 'Erwine.Leonard.T.WPF';
(New-XamlWindowSource).OuterXml