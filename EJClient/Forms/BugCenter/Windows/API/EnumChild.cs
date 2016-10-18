using System;
using System.Collections.Generic;
using System.Text;

namespace WayControls.Windows
{
    /// <summary>
    /// 
    /// </summary>
    public class EnumChild
    {
        private IntPtr parentHandler;
        /// <summary>
        /// 
        /// </summary>
        public List<IntPtr> ChildHandlers = new List<IntPtr>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentHandler"></param>
        public EnumChild(IntPtr parentHandler)
        {
            this.parentHandler = parentHandler;
            API.EnumChildWindows(parentHandler, new API.EnumChildWindowsCallBackHandler(enumChild) , 0);
        }

        private bool enumChild(IntPtr windowHandler, int lparam)
        {
            ChildHandlers.Add(windowHandler);
            return true;
        }

    }
}
