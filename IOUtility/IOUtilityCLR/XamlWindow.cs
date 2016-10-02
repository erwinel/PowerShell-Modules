using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Xml;

namespace IOUtilityCLR
{
    /// <summary>
    /// This represents the WPF window to be displayed.
    /// </summary>
    [Serializable()]
    public class XamlWindow
    {
        /// <summary>
        /// Default XML namespace.
        /// </summary>
        public const string XML_XMLNAMESPACE = "http://www.w3.org/2000/xmlns/";

        /// <summary>
        /// XML namespace for XAML markup.
        /// </summary>
        public const string XAML_XMLNAMESPACE = "http://schemas.microsoft.com/winfx/2006/xaml";

        /// <summary>
        /// XML namespace for XAML Presentation node markup.
        /// </summary>
        public const string PRESENTATION_XMLNAMESPACE = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

        /// <summary>
        /// XML namespace for XAML compatibility node markup.
        /// </summary>
        public const string MARKUP_COMPATIBILITY_XMLNAMESPACE = "http://schemas.openxmlformats.org/markup-compatibility/2006";

        /// <summary>
        /// Element name for XAML window markup.
        /// </summary>
        public const string WINDOW_ELEMENTNAME = "Window";

        internal BackgroundPipelineParameters CreateParameters()
        {
            BackgroundPipelineParameters result = _parameters.Clone();
            
            result.PipelineScripts.Add(ScriptBlock.Create(String.Format("Add-Type -AssemblyName '{0}'", (typeof(System.Windows.Window)).Assembly.FullName)));
            result.PipelineScripts.Add(ScriptBlock.Create(String.Format(@"Add-Type -AssemblyName '{0}';
Add-Type -Path ($PSScriptRoot | Join-Path -ChildPath 'DisplayWindowVM.cs') -ReferencedAssemblies 'System.Management.Automation', 'PresentationFramework', 'WindowsBase', 'System.Xml';", 
                (typeof(System.Windows.DependencyObject)).FullName)));
            result.PipelineScripts.Add(BeforeWindowCreated);
            result.PipelineScripts.Add(ScriptBlock.Create(SCRIPTBLOCK_CREATEWINDOW));
            result.PipelineScripts.Add(BeforeShowDialog);
            result.PipelineScripts.Add(ScriptBlock.Create(SCRIPTBLOCK_SHOWDIALOG));
            result.PipelineScripts.Add(AfterDialogClosed);

            if (result.Variables.ContainsKey("Xaml"))
                result.Variables["Xaml"] = _xaml;
            else
                result.Variables.Add("Xaml", _xaml);

            WindowProxy windowProxy = new WindowProxy(this);
            if (result.Variables.ContainsKey("Window"))
                result.Variables["Window"] = windowProxy;
            else
                result.Variables.Add("Window", windowProxy);

            if (result.Variables.ContainsKey("ResultState"))
                result.Variables["ResultState"] = Hashtable.Synchronized(new Hashtable());
            else
                result.Variables.Add("ResultState", Hashtable.Synchronized(new Hashtable()));

            Hashtable windowControls = new Hashtable();
            foreach (string s in _controlNames)
            {
                if (!String.IsNullOrEmpty(s) && !windowControls.ContainsKey(s))
                    windowControls.Add(s, null);
            }

            // TODO: Use Window variable, instead
            if (result.Variables.ContainsKey("WindowControls"))
                result.Variables["WindowControls"] = windowControls;
            else
                result.Variables.Add("WindowControls", windowControls);

            // TODO: Use Window variable, instead
            if (String.IsNullOrWhiteSpace(Title))
            {
                if (result.Variables.ContainsKey("WindowTitle"))
                    result.Variables.Remove("WindowTitle");
            }
            else if (result.Variables.ContainsKey("WindowTitle"))
                result.Variables["WindowTitle"] = Title;
            else
                result.Variables.Add("WindowTitle", Title);

            // TODO: Use Window variable, instead
            if (!Width.HasValue)
            {
                if (result.Variables.ContainsKey("WindowWidth"))
                    result.Variables.Remove("WindowWidth");
            }
            else if (result.Variables.ContainsKey("WindowWidth"))
                result.Variables["WindowWidth"] = Width.Value;
            else
                result.Variables.Add("WindowWidth", Width.Value);

            // TODO: Use Window variable, instead
            if (!Height.HasValue)
            {
                if (result.Variables.ContainsKey("WindowHeight"))
                    result.Variables.Remove("WindowHeight");
            }
            else if (result.Variables.ContainsKey("WindowHeight"))
                result.Variables["WindowHeight"] = Height.Value;
            else
                result.Variables.Add("WindowHeight", Height.Value);

            return result;
        }

        private object _syncRoot = new object();
        private bool _xmlChanged = false;

        /// <summary>
        /// True if the XAML markup has changed since the last time control names were detected; otherwise, false.
        /// </summary>
        public bool XmlChanged { get { return _xmlChanged; } }
        
        private bool _doNotAutoDetectControlNames = false;

        /// <summary>
        /// True to supress auto-detecting control names; otherwise, false.
        /// </summary>
        public bool DoNotAutoDetectControlNames
        {
            get { return _doNotAutoDetectControlNames; }
            set
            {
                lock (_syncRoot)
                {
                    if (_doNotAutoDetectControlNames == value)
                        return;
                    _doNotAutoDetectControlNames = value;
                    if (_doNotAutoDetectControlNames || !_xmlChanged)
                        return;
                }
                DetectControlNames();
            }
        }
        
        private Collection<string> _controlNames = new Collection<string>();

        /// <summary>
        /// Names of controls represented by the associated XAML markup.
        /// </summary>
        public Collection<string> ControlNames
        {
            get
            {
                lock (_syncRoot)
                {
                    if (!_xmlChanged || _doNotAutoDetectControlNames)
                        return _controlNames;
                }
                return DetectControlNames();
            }
        }

        private BackgroundPipelineParameters _parameters = new BackgroundPipelineParameters(ApartmentState.STA, PSThreadOptions.ReuseThread);

        /// <summary>
        /// Values to be shared between host process and the invoked powershell instance.
        /// </summary>
        public Hashtable SynchronizedData { get { return _parameters.SynchronizedData; } }

        /// <summary>
        /// Variables to be defined in the background PowerShell invocation.
        /// </summary>
        public Dictionary<string, object> Variables { get { return _parameters.Variables; } }

        private ScriptBlock _beforeWindowCreated = null;

        /// <summary>
        /// Script block which is to be invoked before the WPF window is created.
        /// </summary>
        public ScriptBlock BeforeWindowCreated
        {
            get { return _beforeWindowCreated; }
            set
            {
                lock (_syncRoot)
                    _beforeWindowCreated = value;
            }
        }

        private ScriptBlock _beforeShowDialog = null;

        /// <summary>
        /// Script block which is to be invoked after the WPF window has been created, and before it is shown.
        /// </summary>
        public ScriptBlock BeforeShowDialog
        {
            get { return _beforeShowDialog; }
            set
            {
                lock (_syncRoot)
                    _beforeShowDialog = value;
            }
        }

        private ScriptBlock _afterDialogClosed = null;

        /// <summary>
        /// Script block which gets called after the WPF window has been closed.
        /// </summary>
        public ScriptBlock AfterDialogClosed
        {
            get { return _afterDialogClosed; }
            set
            {
                lock (_syncRoot)
                    _afterDialogClosed = value;
            }
        }

        private XmlDocument _xaml;

        /// <summary>
        /// XAML markup which represents the window that is to be displayed.
        /// </summary>
        public XmlDocument Xaml
        {
            get { return _xaml; }
            set
            {
                lock (_syncRoot)
                {
                    XmlDocument xaml = value ?? new XmlDocument();
                    if (_xaml != null && ReferenceEquals(xaml, _xaml))
                        return;
                    if (xaml.DocumentElement == null)
                    {
                        xaml.AppendChild(xaml.CreateElement(WINDOW_ELEMENTNAME, PRESENTATION_XMLNAMESPACE));
                        (xaml.DocumentElement.Attributes.Append(xaml.CreateAttribute("xmlns", "x", XML_XMLNAMESPACE)) as XmlAttribute).Value = XAML_XMLNAMESPACE;
                        (xaml.DocumentElement.Attributes.Append(xaml.CreateAttribute("xmlns", "mc", XML_XMLNAMESPACE)) as XmlAttribute).Value = MARKUP_COMPATIBILITY_XMLNAMESPACE;
                        (xaml.DocumentElement.Attributes.Append(xaml.CreateAttribute("Title")) as XmlAttribute).Value = "{Binding Path=WindowTitle, Mode=TwoWay}";
                        (xaml.DocumentElement.Attributes.Append(xaml.CreateAttribute("Width")) as XmlAttribute).Value = "{Binding Path=WindowWidth, Mode=TwoWay}";
                        (xaml.DocumentElement.Attributes.Append(xaml.CreateAttribute("Height")) as XmlAttribute).Value = "{Binding Path=WindowHeight, Mode=TwoWay}";
                    }
                    else if (xaml.DocumentElement.LocalName != WINDOW_ELEMENTNAME || xaml.DocumentElement.NamespaceURI != PRESENTATION_XMLNAMESPACE)
                        throw new ArgumentException("Only window XAML may be used.", "value");
                    _xmlChanged = true;
                    xaml.NodeInserted += Xaml_DocumentChanged;
                    xaml.NodeRemoved += Xaml_DocumentChanged;
                    xaml.NodeChanged += Xaml_DocumentChanged;
                    if (_xaml != null)
                    {
                        _xaml.NodeInserted -= Xaml_DocumentChanged;
                        _xaml.NodeRemoved -= Xaml_DocumentChanged;
                        _xaml.NodeChanged -= Xaml_DocumentChanged;
                    }
                    _xaml = xaml;
                }
            }
        }

        private void Xaml_DocumentChanged(object sender, XmlNodeChangedEventArgs e) { _xmlChanged = true; }

        /// <summary>
        /// Title for the displayed window.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Initial width of the displayed window.
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// Initial height of the displayed window.
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// Window XAML element.
        /// </summary>
        public XmlElement Content { get { return _xaml.DocumentElement; } }
        
        /// <summary>
        /// Inner XML for the Window XAML.
        /// </summary>
        public string InnerXml
        {
            get
            {
                lock (_syncRoot)
                    return (_xaml.DocumentElement.IsEmpty) ? null : _xaml.DocumentElement.InnerXml;
            }
            set
            {
                lock (_syncRoot)
                {
                    if (value == null)
                    {
                        if (!_xaml.DocumentElement.IsEmpty)
                            _xaml.DocumentElement.IsEmpty = true;
                    } else
                        _xaml.DocumentElement.InnerXml = value;
                }
            }
        }

        /// <summary>
        /// Create new object to represent a WPF window.
        /// </summary>
        public XamlWindow() : this(null as XmlDocument) { }

        /// <summary>
        /// Create new objectd to represent a WPF window with specified markup.
        /// </summary>
        /// <param name="xaml"></param>
        public XamlWindow(XmlDocument xaml)
        {
            try { Xaml = xaml; }
            catch (ArgumentException exception) { throw new ArgumentException(exception.Message, "xaml"); }
        }

        /// <summary>
        /// Create new object to represent a WPF window.
        /// </summary>
        /// <param name="title">Title for window.</param>
        /// <param name="width">Initial width of window.</param>
        /// <param name="height">Initial height of window.</param>
        public XamlWindow(string title, double width, double height)
             : this(null as string)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Create new object to represent a WPF window.
        /// </summary>
        /// <param name="title">Title for window.</param>
        public XamlWindow(string title) : this(null as XmlDocument) { Title = title; }

        /// <summary>
        /// Create new object to represent a WPF window.
        /// </summary>
        /// <param name="width">Initial width of window.</param>
        /// <param name="height">Initial height of window.</param>
        public XamlWindow(double width, double height) : this(null as string, width, height) { }

        /// <summary>
        /// Scan XAML markup for control names.
        /// </summary>
        /// <returns>Collection of control names.</returns>
        public Collection<string> DetectControlNames() { return DetectControlNames(false); }

        /// <summary>
        /// Scan XAML markup for control names.
        /// </summary>
        /// <param name="force">True to force re-scanning of control names; otherwise false to scan only if the document has changed or if it has never been scanned.</param>
        /// <returns>Collection of control names.</returns>
        public Collection<string> DetectControlNames(bool force)
        {
            lock (_syncRoot)
            {
                if (_xmlChanged || force)
                {
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(_xaml.NameTable);
                    nsmgr.AddNamespace("x", XAML_XMLNAMESPACE);
                    _controlNames.Clear();
                    _xmlChanged = false;
                    foreach (XmlAttribute xmlAttribute in _xaml.SelectNodes("//@x:Name"))
                    {
                        string n = xmlAttribute.Value.Trim();
                        if (n.Length > 0 && !_controlNames.Contains(n))
                            _controlNames.Add(n);
                    }
                }
            }
            return _controlNames;
        }
        
        /// <summary>
        /// Script which gets invoked to create the WPF window from the XAML markup.
        /// </summary>
        public const string SCRIPTBLOCK_CREATEWINDOW = @"$XmlNodeReader = (New-Object -TypeName 'System.Xml.XmlNodeReader' -ArgumentList $Xaml);
try { $Window = [Windows.Markup.XamlReader]::Load($XmlNodeReader); } catch { throw } finally { $XmlNodeReader.Dispose() }
if ($Window -ne $null -and $WindowControls.Count -gt 0) { foreach ($name in $WindowControls.Keys) { $WindowControls[$_] = $Window.FindName($name); } }";

        /// <summary>
        /// Script which gets invoked to show the WPF window as a dialog window.
        /// </summary>
        public const string SCRIPTBLOCK_SHOWDIALOG = @"if ($Window -ne $null) {
    if ($Window.DataContext -eq $null) {
        $Window.DataContext = New-Object -TypeName 'ActivityLogger.DisplayWindowVM';
    }
    if ($Window.DataContext -is [ActivityLogger.DisplayWindowVM]) {
        if ($WindowTitle -ne $null -and $WindowTitle -is [string] -and $WindowTitle.Length -gt 0) { $Window.DataContext.WindowTitle = $WindowTitle }
        if ($WindowWidth -ne $null -and $WindowWidth -is [double] -and $WindowWidth -gt 0.0) { $Window.DataContext.WindowWidth = $WindowWidth }
        if ($WindowHeight -ne $null -and $WindowHeight -is [double] -and $WindowHeight -gt 0.0) { $Window.DataContext.WindowHeight = $WindowHeight }
    }
    $DialogResult = $Window.ShowDialog();
    if ($WindowControls.Count -gt 0) {
        foreach ($name in $WindowControls.Keys) {
            if ($WindowControls[$_] -eq $null) { continue }
            if ($WindowControls[$_] -is [System.Windows.Controls.DatePicker]) {
                $ResultState[$_] = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
                    SelectedDate = $WindowControls[$_].SelectedDate;
                    SelectedDateFormat = $WindowControls[$_].SelectedDateFormat.ToString();
                    Text = $WindowControls[$_].Text;
                };
                continue;
            }
            if ($WindowControls[$_] -is [System.Windows.Controls.PasswordBox]) {
                $ResultState[$_] = $WindowControls[$_].Password;
                continue;
            }
            if ($WindowControls[$_] -is [System.Windows.Controls.RichTextBox]) {
                $TextRange = New-Object -TypeName 'System.Windows.Documents.TextRange' -ArgumentList $WindowControls[$_].Document.ContentStart, $WindowControls[$_].Document.ContentEnd;
                $Properties = @{ Text = $TextRange.Text }
                $MemoryStream = New-Object -TypeName 'System.IO.MemoryStream';
                try {
                    try {
                        $Range.Save($MemoryStream, [System.Windows.DataFormats]::Rtf);
                        $MemoryStream.Seek(0, [System.IO.SeekOrigin]::Begin) | Out-Null;
                        $StreamReader = New-Object -TypeName 'System.IO.StreamReader' -ArgumentList $MemoryStream, $true;
                        $Properties.RichText = $StreamReader.ReadToEnd();
                    } catch { throw } finally { $StreamReader.Dispose() }
                } catch { throw } finally { $MemoryStream.Dispose() }
                $ResultState[$_] = New-Object -TypeName 'System.Management.Automation.PSObject' -Property $Properties;
                continue;
            }
            if ($WindowControls[$_] -is [System.Windows.Controls.TextBox]) {
                $ResultState[$_] = $WindowControls[$_].Text;
                continue;
            }
            if ($WindowControls[$_] -is [System.Windows.Controls.ComboBox] -or $WindowControls[$_] -is [System.Windows.Controls.TreeView]) {
                $ResultState[$_] = New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
                    SelectedIndex = $WindowControls[$_].SelectedIndex;
                    SelectedValuePath = $WindowControls[$_].SelectedValuePath;
                    Text = $WindowControls[$_].Text;
                };
                continue;
            }
            if ($WindowControls[$_] -is [System.Windows.Controls.ListBox]) {
                $Properties = @{
                    SelectedIndex = $WindowControls[$_].SelectedIndex;
                    SelectedValuePath = $WindowControls[$_].SelectedValuePath;
                    Text = $WindowControls[$_].Text;
                };
                if ($ResultState[$_].SelectionMode -ne [System.Windows.Controls.SelectionMode]::Single) {
                    $Properties.SelectedItems = @();
                    $ResultState[$_].SelectedItems.Count -gt 0) {
                        $CoreAssembly = [int].Assembly;
                        $SystemAssembly = [System.Uri].Assembly;
                        $XmlAssembly = [System.Xml.XmlDocument].Assembly;
                        @($ResultState[$_].SelectedItems) | ForEach-Object {
                            if ($_ -eq $null) { continue }
                            $a = $_.GetType();
                            if ($a.IsPrimitive -or $a.Assembly.Equals($CoreAssembly) -or $a.Assembly.Equals($SystemAssembly) -or $a.Assembly.Equals($XmlAssembly)) {
                                $Properties.SelectedItems = $Properties.SelectedItems + $_;
                            } else {
                                $Properties.SelectedItems = $Properties.SelectedItems + ($_ | Out-String).TrimEnd();
                            }
                        }
                    }
                }
                $ResultState[$_] = New-Object -TypeName 'System.Management.Automation.PSObject' -Property $Properties;
                continue;
            }
        }
    }
    $ResultState.DialogResult = $DialogResult;
}";
    }
}