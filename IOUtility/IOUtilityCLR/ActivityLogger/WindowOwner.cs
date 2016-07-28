using System;
using System.Windows.Forms;

namespace ActivityLogger
{
    public class WindowOwner : IWin32Window
    {
        private IntPtr _handle;
        public WindowOwner(IntPtr handle) { _handle = handle; }
        public IntPtr Handle { get { return _handle; } }
    }
}