cls;
if ((Get-Module -Name 'Erwine.Leonard.T.WPF') -ne $null) { Remove-Module -Name 'Erwine.Leonard.T.WPF' }
Import-Module -Name 'Erwine.Leonard.T.WPF';


if ($PSVersionTable.PSVersion.Major -lt 3) { $PSScriptRoot = $MyInvocation.MyCommand.Path | Split-Path -Parent }
$Splat = @{
    TypeName = 'System.CodeDom.Compiler.CompilerParameters';
    ArgumentList = (,@(
		[System.Text.RegularExpressions.Regex].Assembly.Location,
        [System.Xml.XmlDocument].Assembly.Location,
        [System.Management.Automation.ScriptBlock].Assembly.Location,
        (Add-Type -AssemblyName 'PresentationFramework' -ErrorAction Stop -PassThru)[0].Assembly.Location,
        (Add-Type -AssemblyName 'PresentationCore' -ErrorAction Stop -PassThru)[0].Assembly.Location,
        (Add-Type -AssemblyName 'WindowsBase' -ErrorAction Stop -PassThru)[0].Assembly.Location,
        (Get-WpfModuleAssemblyPath)
    ));
    Property = @{
        IncludeDebugInformation = $true;
        CompilerOptions = '/define:DEBUG';
    }
};
if ($PSVersionTable.CLRVersion.Major -lt 3 -or ($PSVersionTable.PSVersion.Major -lt 3 -and $PSVersionTable.CLRVersion.Major -eq 3 -and $PSVersionTable.CLRVersion.Minor -lt 5)) {
    $Splat.Property.CompilerOptions += ';PSLEGACY;PSLEGACY2';
} else {
    if ($PSVersionTable.PSVersion.Major -lt 3 -or $PSVersionTable.CLRVersion.Major -lt 4) {
        $Splat.Property.CompilerOptions += ';PSLEGACY;PSLEGACY3';
    }
}

$AssemblyLocation = (Add-Type -TypeDefinition @'
namespace PasswordManagerCLR
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Management.Automation;
    using System.Management.Automation.Host;
    using System.Management.Automation.Runspaces;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using System.Threading;
    using System.Xml;
    using WpfCLR;
    
    public class PasswordManagerViewModel : DependencyObject
    {
		private Window _mainWindow;
		
		public PasswordManagerViewModel(Window mainWindow)
		{
            _copyLoginCommand = new RelayCommand(new RelayInvocationHandler<object>(CopyLogin), false, true);
            _copyPasswordCommand = new RelayCommand(new RelayInvocationHandler<object>(CopyPassword), false, true);
            _copyUrlCommand = new RelayCommand(new RelayInvocationHandler<object>(CopyUrl), false, true);
            _moveUpCommand = new RelayCommand(new RelayInvocationHandler<object>(MoveUp), false, true);
            _moveDownCommand = new RelayCommand(new RelayInvocationHandler<object>(MoveDown), false, true);
            _editCommand = new RelayCommand(new RelayInvocationHandler<object>(Edit), false, true);
            _duplicateCommand = new RelayCommand(new RelayInvocationHandler<object>(Duplicate), false, true);
            _newCommand = new RelayCommand(new RelayInvocationHandler<object>(New));
            _deleteCommand = new RelayCommand(new RelayInvocationHandler<object>(Delete), false, true);
            _browseCommand = new RelayCommand(new RelayInvocationHandler<object>(Browse), false, true);
            _exitCommand = new RelayCommand(new RelayInvocationHandler<object>(Exit));
			_mainWindow = mainWindow;
			Items = new ObservableCollection<CredentialItemViewModel>();
		}

		#region CopyLogin Commmand
		
		private RelayCommand _copyLoginCommand;
		
		public RelayCommand CopyLoginCommand { get { return _copyLoginCommand; } }
		
		private void CopyLogin(object parameter)
		{
			
		}
		
		#endregion
		
		#region CopyPassword Commmand
		
		private RelayCommand _copyPasswordCommand;
		
		public RelayCommand CopyPasswordCommand { get { return _copyPasswordCommand; } }
		
		private void CopyPassword(object parameter)
		{
			
		}
		
		#endregion
		
		#region CopyUrl Commmand
		
		private RelayCommand _copyUrlCommand;
		
		public RelayCommand CopyUrlCommand { get { return _copyUrlCommand; } }
		
		private void CopyUrl(object parameter)
		{
			
		}
		
		#endregion
		
		#region MoveUp Commmand
		
		private RelayCommand _moveUpCommand;
		
		public RelayCommand MoveUpCommand { get { return _moveUpCommand; } }
		
		private void MoveUp(object parameter)
		{
			
		}
		
		#endregion
		
		#region MoveDown Commmand
		
		private RelayCommand _moveDownCommand;
		
		public RelayCommand MoveDownCommand { get { return _moveDownCommand; } }
		
		private void MoveDown(object parameter)
		{
			
		}
		
		#endregion
		
		#region Edit Commmand
		
		private RelayCommand _editCommand;
		
		public RelayCommand EditCommand { get { return _editCommand; } }
		
		private void Edit(object parameter)
		{
			
		}
		
		#endregion
		
		#region Duplicate Commmand
		
		private RelayCommand _duplicateCommand;
		
		public RelayCommand DuplicateCommand { get { return _duplicateCommand; } }
		
		private void Duplicate(object parameter)
		{
			
		}
		
		#endregion
		
		#region New Commmand
		
		private RelayCommand _newCommand;
		
		public RelayCommand NewCommand { get { return _newCommand; } }
		
		private void New(object parameter)
		{
			
		}
		
		#endregion
		
		#region Delete Commmand
		
		private RelayCommand _deleteCommand;
		
		public RelayCommand DeleteCommand { get { return _deleteCommand; } }
		
		private void Delete(object parameter)
		{
			
		}
		
		#endregion
		
		#region Browse Commmand
		
		private RelayCommand _browseCommand;
		
		public RelayCommand BrowseCommand { get { return _browseCommand; } }
		
		private void Browse(object parameter)
		{
			
		}
		
		#endregion
		
		#region Exit Commmand
		
		private RelayCommand _exitCommand;
		
		public RelayCommand ExitCommand { get { return _exitCommand; } }
		
		private void Exit(object parameter)
		{
			if (_mainWindow != null)
			{
				_mainWindow.DialogResult = false;
				_mainWindow.Close();
			}
		}
		
		#endregion
		
        #region Caption Dependency Property
        
        public const string PropertyName_Caption = "Caption";
        
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(PropertyName_Caption, typeof(int), typeof(PasswordManagerViewModel),
            new PropertyMetadata("", Caption_ValueChanged, Caption_CoerceValue));
 
		private string _GetCaption() { return (string)(GetValue(CaptionProperty)); }
        private void _SetCaption(string value) { SetValue(CaptionProperty, value ?? ""); }
		
        public string Caption 
        { 
            get
            {
                if (CheckAccess())
                    return _GetCaption();
                return (string)(Dispatcher.Invoke(new GetStringValue(_GetCaption)));
            }
            set
            {
                if (CheckAccess())
                    _SetCaption(value);
                else
                    Dispatcher.Invoke(new SetStringValue(_SetCaption), value);
            }
        }
		
        private static void Caption_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordManagerViewModel vm = (PasswordManagerViewModel)d;
            
        }
        
        private static object Caption_CoerceValue(DependencyObject d, object baseValue) { return (baseValue as string) ?? ""; }
        
        #endregion
        
        #region SelectedIndex Dependency Property
        
        public const string PropertyName_SelectedIndex = "SelectedIndex";
        
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(PropertyName_SelectedIndex, typeof(int), typeof(PasswordManagerViewModel),
            new PropertyMetadata(-1, SelectedIndex_ValueChanged, SelectedIndex_CoerceValue));
 
		private int _GetSelectedIndex() { return (int)(GetValue(SelectedIndexProperty)); }
        private void _SetSelectedIndex(int value) { SetValue(SelectedIndexProperty, value); }
		
        public int SelectedIndex 
        { 
            get
            {
                if (CheckAccess())
                    return _GetSelectedIndex();
                return (int)(Dispatcher.Invoke(new GetIntValue(_GetSelectedIndex)));
            }
            set
            {
                if (CheckAccess())
                    _SetSelectedIndex(value);
                else
                    Dispatcher.Invoke(new SetIntValue(_SetSelectedIndex), value);
            }
        }
        
        private static void SelectedIndex_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordManagerViewModel vm = (PasswordManagerViewModel)d;
            int index = (int)(e.NewValue);
			bool commandEnabled = index >= 0;
            if (commandEnabled)
            {
                CredentialItemViewModel item = vm.Items[index];
                if (vm.SelectedItem == null || !ReferenceEquals(vm.SelectedItem, item))
                    vm.SelectedItem = item;
            }
            else if (vm.SelectedItem != null)
                    vm.SelectedItem = null;
			
			int max = 0;
			if (commandEnabled)
			{
				foreach (CredentialItemViewModel i in vm.Items)
				{
					if (i != null && i.Order > max)
						max = i.Order;
				}
			}
			CredentialItemViewModel selectedItem = vm.SelectedItem;
            
			vm._copyLoginCommand.IsEnabled = commandEnabled;
			vm._copyPasswordCommand.IsEnabled = commandEnabled;
			vm._copyUrlCommand.IsEnabled = commandEnabled;
			vm._moveUpCommand.IsEnabled = commandEnabled && selectedItem != null && selectedItem.Order > 0;
			vm._moveDownCommand.IsEnabled = commandEnabled && selectedItem != null && selectedItem.Order < max;
			vm._editCommand.IsEnabled = commandEnabled;
			vm._duplicateCommand.IsEnabled = commandEnabled;
			vm._deleteCommand.IsEnabled = commandEnabled;
			vm._browseCommand.IsEnabled = commandEnabled;
        }
        
        private static object SelectedIndex_CoerceValue(DependencyObject d, object baseValue)
        {
            PasswordManagerViewModel vm = (PasswordManagerViewModel)d;
            int? value = baseValue as int?;
            if (!value.HasValue || value.Value < -1 || value.Value >= vm.Items.Count)
                return -1;
                
            return value.Value;
        }
        
        #endregion

        #region SelectedItem Dependency Property
        
        public const string PropertyName_SelectedItem = "SelectedItem";
        
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(PropertyName_SelectedItem, typeof(CredentialItemViewModel), typeof(PasswordManagerViewModel),
            new PropertyMetadata(null, SelectedItem_ValueChanged, SelectedItem_CoerceValue));
 
		private CredentialItemViewModel _GetSelectedItem() { return (CredentialItemViewModel)(GetValue(SelectedItemProperty)); }
        private void _SetSelectedItem(CredentialItemViewModel value) { SetValue(SelectedItemProperty, value); }
		
        public CredentialItemViewModel SelectedItem 
        { 
            get
            {
                if (CheckAccess())
                    return _GetSelectedItem();
                return (CredentialItemViewModel)(Dispatcher.Invoke(new GetItemValue(_GetSelectedItem)));
            }
            set
            {
                if (CheckAccess())
                    _SetSelectedItem(value);
                else
                    Dispatcher.Invoke(new SetItemValue(_SetSelectedItem), value);
            }
        }
        
        private static void SelectedItem_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordManagerViewModel vm = (PasswordManagerViewModel)d;
            int index;
            if (e.NewValue == null)
                index = -1;
            else
            {
                index = -1;
                for (int i = 0; i < vm.Items.Count; i++)
                {
                    if (vm.Items[i] != null && ReferenceEquals(vm.Items[i], e.NewValue))
                    {
                        index = i;
                        break;
                    }
                }
            }
            
            if (vm.SelectedIndex != index)
                vm.SelectedIndex = index;
        }
        
        private static object SelectedItem_CoerceValue(DependencyObject d, object baseValue)
        {
            PasswordManagerViewModel vm = (PasswordManagerViewModel)d;
            CredentialItemViewModel item = baseValue as CredentialItemViewModel;
            if (item == null)
                return null;
                
            foreach (CredentialItemViewModel i in vm.Items)
            {
                if (i != null && ReferenceEquals(i, item))
                    return item;
            }
            
            return null;
        }
        
        #endregion

        #region Items Dependency Property
        
        public const string PropertyName_Items = "Items";
        
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(PropertyName_SelectedItem, typeof(ObservableCollection<CredentialItemViewModel>), typeof(PasswordManagerViewModel),
            new PropertyMetadata(null, Items_ValueChanged, Items_CoerceValue));
 
		private ObservableCollection<CredentialItemViewModel> _GetItems() { return (ObservableCollection<CredentialItemViewModel>)(GetValue(SelectedItemProperty)); }
        private void _SetItems(ObservableCollection<CredentialItemViewModel> value) { SetValue(SelectedItemProperty, value ?? new ObservableCollection<CredentialItemViewModel>()); }
		
        public ObservableCollection<CredentialItemViewModel> Items 
        { 
            get
            {
                if (CheckAccess())
                    return _GetItems();
                return (ObservableCollection<CredentialItemViewModel>)(Dispatcher.Invoke(new GetItemCollectionValue(_GetItems)));
            }
            set
            {
                if (CheckAccess())
                    _SetItems(value);
                else
                    Dispatcher.Invoke(new SetItemCollectionValue(_SetItems), value);
            }
        }
        
        private static void Items_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordManagerViewModel vm = (PasswordManagerViewModel)d;
            if (vm.SelectedIndex < 0)
                return;
            
            if (vm.SelectedIndex >= vm.Items.Count)
                vm.SelectedIndex = -1;
            else
            {
                CredentialItemViewModel item = vm.Items[vm.SelectedIndex];
                if (!ReferenceEquals(item, vm.SelectedItem))
                    vm.SelectedItem = item;
            }
        }
        
        private static object Items_CoerceValue(DependencyObject d, object baseValue)
        {
            return (baseValue as ObservableCollection<CredentialItemViewModel>) ?? new ObservableCollection<CredentialItemViewModel>();
        }

        #endregion
    }
    
	delegate string GetStringValue();
	delegate void SetStringValue(string value);
	
	delegate int GetIntValue();
	delegate void SetIntValue(int value);
	
	delegate CredentialItemViewModel GetItemValue();
	delegate void SetItemValue(CredentialItemViewModel value);
	
	delegate ObservableCollection<CredentialItemViewModel> GetItemCollectionValue();
	delegate void SetItemCollectionValue(ObservableCollection<CredentialItemViewModel> value);
		
	public class CredentialItemViewModel : DependencyObject
    {
        public CredentialItemViewModel() { }
		
        public CredentialItemViewModel(string title, string login, string url)
		{
			Title = title;
			Login = login;
			Url = url;
		}
		
        #region Title Dependency Property
        
        public const string PropertyName_Title = "Title";
        
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(PropertyName_Title, typeof(int), typeof(CredentialItemViewModel),
            new PropertyMetadata("", Title_ValueChanged, Title_CoerceValue));
 
		private string _GetTitle() { return (string)(GetValue(TitleProperty)); }
        private void _SetTitle(string value) { SetValue(TitleProperty, value ?? ""); }
		
        public string Title 
        { 
            get
            {
                if (CheckAccess())
                    return _GetTitle();
                return (string)(Dispatcher.Invoke(new GetStringValue(_GetTitle)));
            }
            set
            {
                if (CheckAccess())
                    _SetTitle(value);
                else
                    Dispatcher.Invoke(new SetStringValue(_SetTitle), value);
            }
        }
		
        private static void Title_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CredentialItemViewModel vm = (CredentialItemViewModel)d;
            
        }
        
        private static object Title_CoerceValue(DependencyObject d, object baseValue) { return (baseValue as string) ?? ""; }
        
        #endregion

        #region Login Dependency Property
        
        public const string PropertyName_Login = "Login";
        
        public static readonly DependencyProperty LoginProperty = DependencyProperty.Register(PropertyName_Login, typeof(int), typeof(CredentialItemViewModel),
            new PropertyMetadata("", Login_ValueChanged, Login_CoerceValue));
 
		private string _GetLogin() { return (string)(GetValue(LoginProperty)); }
        private void _SetLogin(string value) { SetValue(LoginProperty, value ?? ""); }
		
        public string Login 
        { 
            get
            {
                if (CheckAccess())
                    return _GetLogin();
                return (string)(Dispatcher.Invoke(new GetStringValue(_GetLogin)));
            }
            set
            {
                if (CheckAccess())
                    _SetLogin(value);
                else
                    Dispatcher.Invoke(new SetStringValue(_SetLogin), value);
            }
        }
        
        private static void Login_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CredentialItemViewModel vm = (CredentialItemViewModel)d;
            
        }
        
        private static object Login_CoerceValue(DependencyObject d, object baseValue) { return (baseValue as string) ?? ""; }
        
        #endregion

        #region Url Dependency Property
        
        public const string PropertyName_Url = "Url";
        
        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(PropertyName_Url, typeof(int), typeof(CredentialItemViewModel),
            new PropertyMetadata("", Url_ValueChanged, Url_CoerceValue));
 
		private string _GetUrl() { return (string)(GetValue(UrlProperty)); }
        private void _SetUrl(string value) { SetValue(UrlProperty, value ?? ""); }
		
        public string Url 
        { 
            get
            {
                if (CheckAccess())
                    return _GetUrl();
                return (string)(Dispatcher.Invoke(new GetStringValue(_GetUrl)));
            }
            set
            {
                if (CheckAccess())
                    _SetUrl(value);
                else
                    Dispatcher.Invoke(new SetStringValue(_SetUrl), value);
            }
        }
        
        private static void Url_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CredentialItemViewModel vm = (CredentialItemViewModel)d;
            
        }
        
        private static object Url_CoerceValue(DependencyObject d, object baseValue) { return (baseValue as string) ?? ""; }
        
        #endregion

        #region Order Dependency Property
        
        public const string PropertyName_Order = "Order";
        
        public static readonly DependencyProperty OrderProperty = DependencyProperty.Register(PropertyName_Order, typeof(int), typeof(CredentialItemViewModel),
            new PropertyMetadata(0));
 
		private int _GetOrder() { return (int)(GetValue(OrderProperty)); }
        private void _SetOrder(int value) { SetValue(OrderProperty, value); }
		
        public int Order 
        { 
            get
            {
                if (CheckAccess())
                    return _GetOrder();
                return (int)(Dispatcher.Invoke(new GetIntValue(_GetOrder)));
            }
            set
            {
                if (CheckAccess())
                    _SetOrder(value);
                else
                    Dispatcher.Invoke(new SetIntValue(_SetOrder), value);
            }
        }
        
        #endregion
	}
}
'@  -CompilerParameters (New-Object @Splat) -Language CSharp -OutputAssembly '' -OutputType ([Microsoft.PowerShell.Commands.OutputAssemblyType]::Library) -PassThru)[0].Assembly.Location;

if ($AssemblyLocation -eq $null) { return }  

$Xaml = @'
<Window xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition />
            <ColumnDefinition Width="Auto" />
            <ColumnDefinition Width="Auto" />
            <ColumnDefinition Width="Auto" />
            <ColumnDefinition Width="Auto" />
            <ColumnDefinition Width="Auto" />
            <ColumnDefinition Width="Auto" />
        </Grid.ColumnDefinitions>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>
        <TextBlock Text="{Binding Caption}" Grid.ColumnSpan="8" TextWrapping="Wrap" />
        <ListView Grid.Row="1" Grid.ColumnSpan="8" ItemsSource="{Binding Items}">
            <ListView.View>
                <GridView>
                    <GridViewColumn Header="Title" DisplayMemberBinding="{Binding Path=Title}" />
                    <GridViewColumn Header="Login" DisplayMemberBinding="{Binding Path=Login}" />
                    <GridViewColumn Header="Url" DisplayMemberBinding="{Binding Path=Url}"/>
                    <GridViewColumn Header="Order" DisplayMemberBinding="{Binding Path=Order}"/>
                </GridView>
            </ListView.View>
        </ListView>
        <TextBlock Text="Copy: " Grid.Row="2" HorizontalAlignment="Right" />
        <Button Content="Login" Grid.Row="2" Height="25" Width="75" Command="{Binding Path=CopyLoginCommand}" />
        <Button Content="Password" Grid.Row="2" Grid.Column="1" Height="25" Width="75" Command="{Binding Path=CopyPasswordCommand}" />
        <Button Content="Url" Grid.Row="2" Grid.Column="2" Height="25" Width="75" Command="{Binding Path=CopyUrlCommand}" />
        <TextBlock Text="Modify: " Grid.Row="3" HorizontalAlignment="Right" />
        <Button Content="Move Up" Grid.Column="1" Grid.Row="3" Height="25" Width="75" Command="{Binding Path=MoveUpCommand}" />
        <Button Content="Move Down" Grid.Column="2" Grid.Row="3" Height="25" Width="75" Command="{Binding Path=MoveDownCommand}" />
        <Button Content="Edit" Grid.Column="3" Grid.Row="3" Height="25" Width="75" Command="{Binding Path=EditCommand}" />
        <Button Content="Duplicate" Grid.Column="4" Grid.Row="3" Height="25" Width="75" Command="{Binding Path=DuplicateCommand}" />
        <Button Content="New" Grid.Column="5" Grid.Row="3" Height="25" Width="75" Command="{Binding Path=NewCommand}" />
        <Button Content="Delete" Grid.Column="6" Grid.Row="3" Height="25" Width="75" Command="{Binding Path=DeleteCommand}" />
        <Button Content="Browse" Grid.Column="5" Grid.Row="4" Height="25" Width="75" Command="{Binding Path=BrowseCommand}" />
        <Button Content="Exit" Grid.Column="6" Grid.Row="4" Height="25" Width="75" Command="{Binding Path=ExitCommand}" />
    </Grid>
</Window>
'@;
#SelectedIndex
#SelectedItem
$WpfWindow = Show-WpfWindow -WindowXaml $Xaml -BeforeWindowCreated {
    Add-Type -AssemblyName $this.SynchronizedData.ModuleAssemblyLocation;
    Add-Type -AssemblyName $this.SynchronizedData.ViewModelAssemblyLocation;
} -BeforeWindowShown {
    $this.MainWindow.DataContext = New-Object -TypeName 'PasswordManagerCLR.PasswordManagerViewModel';
    $this.MainWindow.DataContext.Caption = 'Test caption!!!';
} -SynchronizedData @{
    ViewModelAssemblyLocation = $AssemblyLocation;
    ModuleAssemblyLocation = Get-WpfModuleAssemblyPath;
};