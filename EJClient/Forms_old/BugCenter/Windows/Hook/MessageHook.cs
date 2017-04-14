#region Using directives

using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using WayControls.Windows;
#endregion

namespace WayControls.Windows.Hook
{
	/// <summary>
	/// 鼠标钩子
	/// </summary>
    public class MessageHook
	{
        
		
		private API.HookProc MouseHookProcedure;

		/// <summary>
		/// 
		/// </summary>
		private int hMouseHook = 0;

		private int threadID = 0;

		/// <summary>
		/// 
		/// </summary>
        public MessageHook()
		{

		}

		/// <summary>
		/// 
		/// </summary>
        ~MessageHook()
		{
			Stop();
		}
        
		#region Start
        private int _windowhandler = -1;
		/// <summary>
		/// 开始安装全局钩子
		/// </summary>
		public void StartForWindow(int windowHandler)
		{
            _windowhandler = windowHandler;
			//安装鼠标钩子
			if (hMouseHook == 0)
			{
				//生成一个HookProc的实例.
				MouseHookProcedure = new API.HookProc(MouseHookProc);
               Module module = Assembly.GetExecutingAssembly().GetModules()[0];
               IntPtr hIntance = API.GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName);
               hMouseHook = API.SetWindowsHookEx(API.WH_GETMESSAGE, MouseHookProcedure, hIntance, WayControls.Windows.API.GetCurrentThreadId());

				//如果装置失败停止钩子
				if (hMouseHook == 0)
				{
                    Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
					Stop();
					throw new Exception("安装钩子失败.");
				}
			}
		}


        public void Start()
        {
            StartForWindow(_windowhandler);
        }


		#endregion

		#region Stop
		/// <summary>
		/// 
		/// </summary>
		public void Stop()
		{
			bool retMouse = true;
			if (hMouseHook != 0)
			{
				retMouse = API.UnhookWindowsHookEx(hMouseHook);
				hMouseHook = 0;
			}

			//如果卸下钩子失败
			if (!(retMouse)) throw new Exception("卸载钩子失败.");
		} 
		#endregion

		#region MouseHookProc
        public delegate bool HookHandler(API.MSG msgObj);
        public event HookHandler Hook;
		private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
		{
            if (Hook != null)
            {
                API.MSG msgObj = (API.MSG)Marshal.PtrToStructure(lParam, typeof(API.MSG));
                if (Hook(msgObj) ==false)
                {
                    return -1;
                }
            }
            return API.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
		}

		#endregion

	}
}
