using System;
using System.Collections;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Xml;

namespace IOUtilityCLR
{
    /// <summary>
    /// Rrepresents a WPF Window invocation
    /// </summary>
    public class PsWpfInvocation : BackgroundPipelineInvocation
    {
        /// <summary>
        /// Converts XAML window parameters to invocation parameters
        /// </summary>
        /// <param name="xamlWindow">Object representing XAML window to be displayed.</param>
        /// <returns></returns>
        public static BackgroundPipelineParameters CreateParameters(XamlWindow xamlWindow)
        {
            if (xamlWindow == null)
                throw new ArgumentNullException("xamlWindow");
            return xamlWindow.CreateParameters();
        }

        /// <summary>
        /// XAML markup for window to be displayed.
        /// </summary>
        public XmlDocument Xaml { get; private set; }

        /// <summary>
        /// Contains values which represent the result from the displayed window.
        /// </summary>
        public Hashtable ResultState { get; private set; }

        /// <summary>
        /// Display a XAML window.
        /// </summary>
        /// <param name="host">PowerShell host to use.</param>
        /// <param name="xamlWindow">Object which represents the XAML window to be displayed as well as the functionality.</param>
        public PsWpfInvocation(PSHost host, XamlWindow xamlWindow) : this(host, xamlWindow, null) { }

        /// <summary>
        /// Display a XAML window and track user state.
        /// </summary>
        /// <param name="host">PowerShell host to use.</param>
        /// <param name="xamlWindow">Object which represents the XAML window to be displayed as well as the functionality.</param>
        /// <param name="state">User state to associate with the results.</param>
        public PsWpfInvocation(PSHost host, XamlWindow xamlWindow, object state)
            : base(host, CreateParameters(xamlWindow), state)
        {
            Xaml = xamlWindow.Xaml;
            ResultState = Hashtable.Synchronized(new Hashtable());
        }

        /// <summary>
        /// This gets invoked after the runspace is created and before it is opened.
        /// </summary>
        /// <param name="host">PowerShell host associated with runspace.</param>
        /// <param name="runspace">Runspace about to be opened.</param>
        /// <param name="parameters">Parameters that define the behavior of the invocation</param>
        protected override void BeforeOpenRunspace(PSHost host, Runspace runspace, BackgroundPipelineParameters parameters)
        {
            base.BeforeOpenRunspace(host, runspace, parameters);
            ResultState = parameters.Variables["ResultState"] as Hashtable;
        }
    }
}