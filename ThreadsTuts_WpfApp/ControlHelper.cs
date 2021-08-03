using System;
using System.Windows.Controls;

namespace ThreadsTuts_WpfApp
{
    public static class ControlHelper
    {
        public static void InvokeEx(this Control control, Action action)
        {
            if (!control.Dispatcher.CheckAccess())
                control.Dispatcher.Invoke(action);
            else
                action();
        }
    }
}