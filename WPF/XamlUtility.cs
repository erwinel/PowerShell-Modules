using System;
using System.IO;
using System.Management.Automation;
using System.Windows;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Erwine.Leonard.T.WPF
{
    /// <summary>
    /// Utility methods and definitions for processing XAML markup.
    /// </summary>
    public static class XamlUtility
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string XmlNamespaceURI_Presentation = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

        public const string XmlNamespaceURI_Xaml = "http://schemas.microsoft.com/winfx/2006/xaml";

        public const string XmlNamespaceURI_Blend = "http://schemas.microsoft.com/expression/blend/2008";

        public const string XmlNamespaceURI_MarkupCompatibility = "http://schemas.openxmlformats.org/markup-compatibility/2006";
        
        public const string XmlNamespaceURI_Xml = "http://www.w3.org/2000/xmlns/";
        
        public const string Xaml_EmptyWindow = @"<Window xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
</Window>";

        public const string XmlElementName_Window = "Window";
        public const string XmlAttributeName_Width = "Width";
        public const string XmlAttributeName_Height = "Height";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static XmlDocument CreateXamlMarkup(string rootElementName, string xamlName)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (rootElementName == null)
                throw new ArgumentNullException("rootElementName");

            try { return CreateXamlMarkup(new XmlQualifiedName(rootElementName, XmlNamespaceURI_Presentation), xamlName); }
            catch (Exception exception) { throw new ArgumentException("Invalid root element name", "rootElementName", exception); }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static XmlDocument CreateXamlMarkup(XmlQualifiedName rootElementName, string xamlName)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (rootElementName == null)
                throw new ArgumentNullException("rootElementName");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateElement(rootElementName.Name, rootElementName.Namespace));
            if (rootElementName.Namespace != XmlNamespaceURI_Xaml)
                xmlDocument.DocumentElement.Attributes.Append(xmlDocument.CreateAttribute("x", XmlNamespaceURI_Xml)).Value = XmlNamespaceURI_Xaml;
            if (!String.IsNullOrWhiteSpace(xamlName))
                xmlDocument.DocumentElement.Attributes.Append(xmlDocument.CreateAttribute("name", XmlNamespaceURI_Xaml)).Value = xamlName;

            return xmlDocument;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static XmlDocument LoadXmlDocument(XmlReader xmlReader)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (xmlReader == null)
                throw new ArgumentNullException("xmlReader");
            
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);
            return xmlDocument;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static XmlDocument LoadXmlDocument(Stream stream, XmlReaderSettings xmlReaderSettings)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            
            using (XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings))
                return LoadXmlDocument(xmlReader);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static XmlDocument LoadXmlDocument(Stream stream)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);
            return xmlDocument;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static XmlDocument LoadXmlDocument(TextReader textReader, XmlReaderSettings xmlReaderSettings)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (textReader == null)
                throw new ArgumentNullException("textReader");
            
            using (XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings))
                return LoadXmlDocument(xmlReader);
        }

        internal static XmlDocument CreateXamlWindowMarkup()
        {
            XmlDocument windowXaml = new XmlDocument();
            windowXaml.AppendChild(windowXaml.CreateElement(XmlElementName_Window, XmlNamespaceURI_Presentation));
            windowXaml.Attributes.Append(windowXaml.CreateAttribute("xmlns", "x", XmlNamespaceURI_Xml)).Value = XmlNamespaceURI_Xaml;
            windowXaml.Attributes.Append(windowXaml.CreateAttribute("xmlns", "d", XmlNamespaceURI_Xml)).Value = XmlNamespaceURI_Blend;
            windowXaml.Attributes.Append(windowXaml.CreateAttribute("xmlns", "mc", XmlNamespaceURI_Xml)).Value = XmlNamespaceURI_MarkupCompatibility;
            windowXaml.Attributes.Append(windowXaml.CreateAttribute("Ignorable", XmlNamespaceURI_Blend)).Value = "d";
            windowXaml.Attributes.Append(windowXaml.CreateAttribute(XmlAttributeName_Width)).Value = "800";
            windowXaml.Attributes.Append(windowXaml.CreateAttribute(XmlAttributeName_Height)).Value = "600";
            return windowXaml;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static XmlDocument LoadXmlDocument(TextReader textReader)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (textReader == null)
                throw new ArgumentNullException("textReader");
            
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(textReader);
            return xmlDocument;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static XmlDocument ParseXmlDocument(string xmlText, XmlReaderSettings xmlReaderSettings)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (xmlText == null)
                throw new ArgumentNullException("xmlText");
            
            using (StringReader stringReader = new StringReader(xmlText))
                return LoadXmlDocument(stringReader, xmlReaderSettings);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static XmlDocument ParseXmlDocument(string xmlText)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (xmlText == null)
                throw new ArgumentNullException("xmlText");
            
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlText);
            return xmlDocument;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool TryLoadXaml(XmlElement xmlElement, out XamlLoadResult result)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            XmlDocument xmlDocument = null;
            try
            {
                if (xmlElement == null)
                    throw new ArgumentNullException("xmlDocument");

                if (xmlElement.OwnerDocument != null && xmlElement.OwnerDocument.DocumentElement != null && ReferenceEquals(xmlElement, xmlElement.OwnerDocument.DocumentElement))
                    xmlDocument = xmlElement.OwnerDocument;
                else
                {
                    xmlDocument = new XmlDocument();
                    xmlDocument.AppendChild(xmlDocument.ImportNode(xmlElement, true));
                }
                return TryLoadXaml(xmlDocument, out result);
            }
            catch (XamlParseException exception)
            {
                result = new XamlLoadResult(exception, xmlDocument, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlSchemaException exception)
            {
                result = new XamlLoadResult(exception, xmlDocument, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlException exception)
            {
                result = new XamlLoadResult(exception, xmlDocument, exception.LineNumber, exception.LinePosition);
            }
            catch (Exception exception)
            {
                result = new XamlLoadResult(xmlDocument, exception);
            }

            return false;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool TryLoadXaml(XDocument xDocument, out XamlLoadResult result)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                if (xDocument == null || xDocument.Root == null)
                    throw new ArgumentNullException("xDocument");
                return TryLoadXaml(xDocument.Root, out result);
            }
            catch (ArgumentNullException exception)
            {
                result = new XamlLoadResult(null, exception);
            }

            return false;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool TryLoadXaml(XElement xElement, out XamlLoadResult result)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            XmlDocument xmlDocument = null;
            try
            {
                if (xElement == null)
                    throw new ArgumentNullException("xElement");

                xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xElement.ToString(SaveOptions.None));
                return TryLoadXaml(xmlDocument, out result);
            }
            catch (XamlParseException exception)
            {
                result = new XamlLoadResult(exception, xmlDocument, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlSchemaException exception)
            {
                result = new XamlLoadResult(exception, xmlDocument, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlException exception)
            {
                result = new XamlLoadResult(exception, xmlDocument, exception.LineNumber, exception.LinePosition);
            }
            catch (Exception exception)
            {
                result = new XamlLoadResult(xmlDocument, exception);
            }

            return false;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool TryLoadXaml(XmlDocument xmlDocument, out XamlLoadResult result)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                if (xmlDocument == null)
                    throw new ArgumentNullException("xmlDocument");
                
                using (XmlNodeReader xmlNodeReader = new XmlNodeReader(xmlDocument))
                    result = new XamlLoadResult(xmlDocument, XamlReader.Load(xmlNodeReader));
            }
            catch (XamlParseException exception)
            {
                result = new XamlLoadResult(exception, xmlDocument, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlSchemaException exception)
            {
                result = new XamlLoadResult(exception, xmlDocument, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlException exception)
            {
                result = new XamlLoadResult(exception, xmlDocument, exception.LineNumber, exception.LinePosition);
            }
            catch (Exception exception)
            {
                result = new XamlLoadResult(xmlDocument, exception);
            }
            
            return result.Success;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool TryLoadXaml(Stream stream, XmlReaderSettings xmlReaderSettings, out XamlLoadResult result)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                if (stream == null)
                    throw new ArgumentNullException("stream");
                
                TryLoadXaml(LoadXmlDocument(stream, xmlReaderSettings), out result);
            }
            catch (XmlSchemaException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (Exception exception)
            {
                result = new XamlLoadResult(exception, null);
            }
            
            return result.Success;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool TryLoadXaml(Stream stream, out XamlLoadResult result)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                if (stream == null)
                    throw new ArgumentNullException("stream");
                
                TryLoadXaml(LoadXmlDocument(stream), out result);
            }
            catch (XmlSchemaException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (Exception exception)
            {
                result = new XamlLoadResult(exception, null);
            }
            
            return result.Success;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool TryLoadXaml(TextReader textReader, XmlReaderSettings xmlReaderSettings, out XamlLoadResult result)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                if (textReader == null)
                    throw new ArgumentNullException("textReader");
                
                TryLoadXaml(LoadXmlDocument(textReader, xmlReaderSettings), out result);
            }
            catch (XmlSchemaException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (Exception exception)
            {
                result = new XamlLoadResult(exception, null);
            }
            
            return result.Success;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool TryLoadXaml(TextReader textReader, out XamlLoadResult result)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                if (textReader == null)
                    throw new ArgumentNullException("textReader");
                
                TryLoadXaml(LoadXmlDocument(textReader), out result);
            }
            catch (XmlSchemaException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (Exception exception)
            {
                result = new XamlLoadResult(exception, null);
            }
            
            return result.Success;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool TryParseXaml(string xamlText, XmlReaderSettings xmlReaderSettings, out XamlLoadResult result)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                if (xamlText == null)
                    throw new ArgumentNullException("xamlText");
                
                TryLoadXaml(ParseXmlDocument(xamlText, xmlReaderSettings), out result);
            }
            catch (XmlSchemaException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (Exception exception)
            {
                result = new XamlLoadResult(exception, null);
            }
            
            return result.Success;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool TryParseXaml(string xamlText, out XamlLoadResult result)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                if (xamlText == null)
                    throw new ArgumentNullException("xamlText");
                
                TryLoadXaml(ParseXmlDocument(xamlText), out result);
            }
            catch (XmlSchemaException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (Exception exception)
            {
                result = new XamlLoadResult(exception, null);
            }
            
            return result.Success;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool TryLoadXaml(XmlReader xmlReader, out XamlLoadResult result)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                if (xmlReader == null)
                    throw new ArgumentNullException("xmlReader");
                
                TryLoadXaml(LoadXmlDocument(xmlReader), out result);
            }
            catch (XmlSchemaException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (XmlException exception)
            {
                result = new XamlLoadResult(exception, null, exception.LineNumber, exception.LinePosition);
            }
            catch (Exception exception)
            {
                result = new XamlLoadResult(exception, null);
            }
            
            return result.Success;
        }
    }
}