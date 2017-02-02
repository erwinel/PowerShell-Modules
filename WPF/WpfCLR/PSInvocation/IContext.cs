﻿using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace WpfCLR.PSInvocation
{
    public interface IContext
    {
        PSHost Host { get; }
        string InitialLocation { get; }
        bool? UseLocalScope { get; }
        ApartmentState? ApartmentState { get; }
        PSThreadOptions? ThreadOptions { get; }
        RunspaceConfiguration Configuration { get; }
        Hashtable Variables { get; }
        Hashtable SynchronizedData { get; }
        PSObject This { get; }
    }
}