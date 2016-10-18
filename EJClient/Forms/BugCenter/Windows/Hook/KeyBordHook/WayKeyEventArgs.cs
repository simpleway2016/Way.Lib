using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WayControls.Windows.Hook
{
    /// <summary>
    /// 
    /// </summary>
    public class WayKeyEventArgs : KeyEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public WayKeyEventArgs(Keys key,int scancode):base(key)
        {
            this._s = scancode;
        }

        private int _s = 0;
        public int ScanCode
        {
            get
            {
                return _s;
            }
        }

        private bool _CallNextHookEx = true;
        /// <summary>
        /// 是否传递hook到一下个钩子
        /// </summary>
        public bool CallNextHookEx
        {
            get
            {
                return _CallNextHookEx;
            }
            set
            {
                _CallNextHookEx = value;
            }
        }
    }
}
