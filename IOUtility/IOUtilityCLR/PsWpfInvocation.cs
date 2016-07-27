using System;
using System.Management.Automation.Host;
using System.Xml;

namespace IOUtilityCLR
{
    public class PsWpfInvocation : BackgroundPipelineInvocation
    {
        public static BackgroundPipelineParameters CreateParameters(XamlWindow xamlWindow)
        {
            if (xamlWindow == null)
                throw new ArgumentNullException("xamlWindow");
            return xamlWindow.CreateParameters();
        }
        public XmlDocument Xaml { get; private set; }
        public PsWpfInvocation(PSHost host, XamlWindow xamlWindow) : this(host, xamlWindow, null) { }
        public PsWpfInvocation(PSHost host, XamlWindow xamlWindow, object state)
            : base(host, CreateParameters(xamlWindow), state) { Xaml = xamlWindow.Xaml; }
    }
}