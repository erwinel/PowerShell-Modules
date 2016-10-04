cls;
if ($PSScriptRoot -eq $null) { $PSScriptRoot = $MyInvocation.InvocationName | Split-Path -Parent }
Add-Type -Path (('..\BackgroundPipelineInvocation.cs', '..\BackgroundPipelineParameters.cs', '..\PsWpfInvocation.cs', '..\WindowOwner.cs', '..\XamlWindow.cs') | ForEach-Object { $PSScriptRoot | Join-Path -ChildPath $_ }) `
	-ReferencedAssemblies 'System.Management.Automation', 'PresentationFramework', 'WindowsBase', 'System.Xml';

[Xml]$Xaml = @'
<Window xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        Title="{Binding Path=WindowTitle, Mode=TwoWay}" Width="{Binding Path=WindowWidth, Mode=TwoWay}" Height="{Binding Path=WindowHeight, Mode=TwoWay}">
    <Grid xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto" />
            <ColumnDefinition />
        </Grid.ColumnDefinitions>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>
        <TextBlock x:Name="captionTextBlock" Grid.ColumnSpan="2" TextWrapping="Wrap" />
        <TextBox x:Name="textBox" Grid.ColumnSpan="2" Grid.Row="1" VerticalAlignment="Top"></TextBox>
        <Button x:Name="okButton" Grid.Row="2" Content="OK" IsDefault="true" Height="25" Width="75" HorizontalAlignment="Right" />
        <Button x:Name="cancelButton" Grid.Column="1" Grid.Row="2" Content="Cancel" IsCancel="false" Height="25" Width="75" />
    </Grid>
</Window>
'@;
$XamlWindow = New-Object -TypeName 'ActivityLogger.XamlWindow' -ArgumentList $Xaml;
$XamlWindow.Variables['PSScriptRoot'] = $PSScriptRoot;
$XamlWindow.BeforeWindowCreated = {
    Add-Type -Path ($PSScriptRoot | Join-Path -ChildPath 'ListingWindowVM.cs') -ReferencedAssemblies 'System.Management.Automation', 'PresentationFramework', 'System.Xml';
};
$XamlWindow.Xaml.OuterXml;