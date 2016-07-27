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
    [Serializable()]
    public class XamlWindow
    {
        public const string XML_XMLNAMESPACE = "http://www.w3.org/2000/xmlns/";
        public const string XAML_XMLNAMESPACE = "http://schemas.microsoft.com/winfx/2006/xaml";
        public const string PRESENTATION_XMLNAMESPACE = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

        internal BackgroundPipelineParameters CreateParameters()
        {
            BackgroundPipelineParameters result = _parameters.Clone();
            result.PipelineScripts.Add(ScriptBlock.Create("Add-Type -AssemblyName 'PresentationFramework'"));
            result.PipelineScripts.Add(BeforeWindowCreated);
            result.PipelineScripts.Add(ScriptBlock.Create(SCRIPTBLOCK_CREATEWINDOW));
            result.PipelineScripts.Add(BeforeShowDialog);
            result.PipelineScripts.Add(ScriptBlock.Create(SCRIPTBLOCK_SHOWDIALOG));
            result.PipelineScripts.Add(AfterDialogClosed);
            if (result.Variables.ContainsKey("Xaml"))
                result.Variables["Xaml"] = _xaml;
            else
                result.Variables.Add("Xaml", _xaml);
            if (result.Variables.ContainsKey("ControlNames"))
                result.Variables["ControlNames"] = new Collection<string>(_controlNames);
            else
                result.Variables.Add("ControlNames", new Collection<string>(_controlNames));
            return result;
        }

        public const string WINDOW_ELEMENTNAME = "Window";

        private object _syncRoot = new object();
        private bool _xmlChanged = false;
        public bool XmlChanged { get { return _xmlChanged; } }
        
        private bool _doNotAutoDetectControlNames = false;
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
        public Hashtable SynchronizedData { get { return _parameters.SynchronizedData; } }
        public Dictionary<string, object> Variables { get { return _parameters.Variables; } }

        private ScriptBlock _beforeWindowCreated = null;
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
        public string Title
        {
            get
            {
                XmlAttribute xmlAttribute = _xaml.DocumentElement.SelectSingleNode("@Title") as XmlAttribute;
                return (xmlAttribute == null) ? null : xmlAttribute.Value;
            }
            set
            {
                XmlAttribute xmlAttribute = _xaml.DocumentElement.SelectSingleNode("@Title") as XmlAttribute;
                if (value == null)
                {
                    if (xmlAttribute != null)
                        xmlAttribute.ParentNode.Attributes.Remove(xmlAttribute);
                } else if (xmlAttribute == null)
                    (_xaml.DocumentElement.Attributes.Append(_xaml.CreateAttribute("Title")) as XmlAttribute).Value = value;
                else
                    xmlAttribute.Value = value;
            }
        }
        public int? Width
        {
            get
            {
                XmlAttribute xmlAttribute = _xaml.DocumentElement.SelectSingleNode("@Width") as XmlAttribute;
                return (xmlAttribute == null) ? null : XmlConvert.ToInt32(xmlAttribute.Value) as int?;
            }
            set
            {
                XmlAttribute xmlAttribute = _xaml.DocumentElement.SelectSingleNode("@Width") as XmlAttribute;
                if (value.HasValue && value.Value > 0)
                {
                    if (xmlAttribute == null)
                        (_xaml.DocumentElement.Attributes.Append(_xaml.CreateAttribute("Width")) as XmlAttribute).Value = XmlConvert.ToString(value.Value);
                    else
                        xmlAttribute.Value = XmlConvert.ToString(value.Value);
                    
                } else if (xmlAttribute != null)
                    xmlAttribute.ParentNode.Attributes.Remove(xmlAttribute);
            }
        }
        public int? Height
        {
            get
            {
                XmlAttribute xmlAttribute = _xaml.DocumentElement.SelectSingleNode("@Height") as XmlAttribute;
                return (xmlAttribute == null) ? null : XmlConvert.ToInt32(xmlAttribute.Value) as int?;
            }
            set
            {
                XmlAttribute xmlAttribute = _xaml.DocumentElement.SelectSingleNode("@Height") as XmlAttribute;
                if (value.HasValue && value.Value > 0)
                {
                    if (xmlAttribute == null)
                        (_xaml.DocumentElement.Attributes.Append(_xaml.CreateAttribute("Height")) as XmlAttribute).Value = XmlConvert.ToString(value.Value);
                    else
                        xmlAttribute.Value = XmlConvert.ToString(value.Value);
                    
                } else if (xmlAttribute != null)
                    xmlAttribute.ParentNode.Attributes.Remove(xmlAttribute);
            }
        }
        
        public XmlElement Content { get { return _xaml.DocumentElement; } }
        
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
        
        public XamlWindow() : this(null as XmlDocument) { }

        public XamlWindow(XmlDocument xaml)
        {
            try { Xaml = xaml; }
            catch (ArgumentException exception) { throw new ArgumentException(exception.Message, "xaml"); }
        }

        public XamlWindow(string title, int width, int height)
             : this(null as string)
        {
            if (width > 0)
                (_xaml.DocumentElement.Attributes.Append(_xaml.CreateAttribute("Width")) as XmlAttribute).Value = XmlConvert.ToString(width);
            if (height > 0)
                (_xaml.DocumentElement.Attributes.Append(_xaml.CreateAttribute("Height")) as XmlAttribute).Value = XmlConvert.ToString(height);
        }

        public XamlWindow(string title)
             : this(null as XmlDocument)
        {
            if (title != null)
                (_xaml.DocumentElement.Attributes.Append(_xaml.CreateAttribute("Title")) as XmlAttribute).Value = title;
        }

        public XamlWindow(int width, int height) : this(null as string, width, height) { }

        public Collection<string> DetectControlNames() { return DetectControlNames(false); }

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

        public const string SCRIPTBLOCK_CREATEWINDOW = @"$XmlNodeReader = (New-Object -TypeName 'System.Xml.XmlNodeReader' -ArgumentList $Xaml);
try { $Window = [Windows.Markup.XamlReader]::Load($XmlNodeReader); } catch { throw } finally { $XmlNodeReader.Dispose() }
if ($Window -ne $null -and $WindowControls.Count -gt 0) { foreach ($name in $WindowControls.Keys) { $WindowControls[$_] = $Window.FindName($name); } }";

        public const string SCRIPTBLOCK_SHOWDIALOG = @"if ($Window -ne $null) {
    $ResultState.DialogResult = $Window.ShowDialog();
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
}";
    }
}