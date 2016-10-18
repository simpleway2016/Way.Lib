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
	/// 全局键盘钩子
	/// </summary>
	public class KeyBordHook
	{
        public int 被忽略的扫描码 = -999;

		#region 全局的事件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void WayKeyEventHandler(object sender, WayKeyEventArgs e);
        public event WayKeyEventHandler OnKeyDoSomethingEvent;
		/// <summary>
		/// 
		/// </summary>
        public event WayKeyEventHandler OnKeyDownEvent;
		/// <summary>
		/// 
		/// </summary>
        public event WayKeyEventHandler OnKeyUpEvent;
		/// <summary>
		/// 
		/// </summary>
		public event KeyPressEventHandler OnKeyPressEvent;
		#endregion

        private bool isThreadHook = false;//是否是线程钩子
        private int hKeyboardHook = 0; //键盘钩子句柄

		private API.HookProc KeyboardHookProcedure; //声明键盘钩子事件类型.


		/// <summary>
		/// 
		/// </summary>
		public KeyBordHook()
		{

		}
		/// <summary>
		/// 
		/// </summary>
		~KeyBordHook()
		{
			this.Stop();
		}

		#region Start
		/// <summary>
		/// 加载钩子
		/// </summary>
		public void Start()
		{
			//安装键盘钩子 
			if (hKeyboardHook == 0)
			{
				KeyboardHookProcedure = new API.HookProc(KeyboardHookProc);
                Module module = Assembly.GetExecutingAssembly().GetModules()[0];
                IntPtr hIntance =API.GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName);

                hKeyboardHook = API.SetWindowsHookEx(API.WH_KEYBOARD_LL, KeyboardHookProcedure, hIntance/*Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0])*/, 0);

				if (hKeyboardHook == 0)
				{
					Stop();
					throw new Exception("加载钩子失败.");
				}
			}
		}

       /// <summary>
       /// 加载线程钩子
       /// </summary>
       /// <param name="currentThreadId">目标线程id</param>
        public void Start(int currentThreadId)
        {
            //安装键盘钩子 
            if (hKeyboardHook == 0)
            {
                this.isThreadHook = true;
                KeyboardHookProcedure = new API.HookProc(KeyboardHookProc);
                hKeyboardHook = API.SetWindowsHookEx(API.WH_KEYBOARD, KeyboardHookProcedure, (IntPtr)0, currentThreadId);

                
                if (hKeyboardHook == 0)
                {
                    Stop();
                    throw new Exception("加载钩子失败.");
                }
            }
        }
		#endregion

		#region Stop
		/// <summary>
		/// 卸下钩子
		/// </summary>
		public void Stop()
		{
			bool retKeyboard = true;

			if (hKeyboardHook != 0)
			{
				retKeyboard = API.UnhookWindowsHookEx(hKeyboardHook);
				hKeyboardHook = 0;
			}
			//如果卸下钩子失败
			if (!(retKeyboard)) throw new Exception("卸下钩子失败.");
		}
		#endregion

		#region KeyboardHookProc
		private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
		{
            if (!this.isThreadHook)
            {
                #region 全局钩子
                if ((nCode >= 0) && (OnKeyDoSomethingEvent != null || OnKeyDownEvent != null || OnKeyUpEvent != null || OnKeyPressEvent != null))
                {
                    API.KeyboardHookStruct MyKeyboardHookStruct = (API.KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(API.KeyboardHookStruct));
                    //引发OnKeyDownEvent
                    if (OnKeyDoSomethingEvent != null)
                    {
                        Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                        WayKeyEventArgs e = new WayKeyEventArgs(keyData, MyKeyboardHookStruct.scanCode);
                        OnKeyDoSomethingEvent(this, e);
                        if (e.CallNextHookEx == false)
                            return 1;
                    }
                    if (OnKeyDownEvent != null && (wParam == API.WM_KEYDOWN || wParam == API.WM_SYSKEYDOWN))
                    {
                        Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                        WayKeyEventArgs e = new WayKeyEventArgs(keyData , MyKeyboardHookStruct.scanCode);
                        OnKeyDownEvent(this, e);
                        if (e.CallNextHookEx == false)
                            return 1;
                    }

                    //引发OnKeyPressEvent
                    if (OnKeyPressEvent != null && wParam == API.WM_KEYDOWN)
                    {
                        byte[] keyState = new byte[256];
                        API.GetKeyboardState(keyState);

                        byte[] inBuffer = new byte[2];
                        if (API.ToAscii(MyKeyboardHookStruct.vkCode,
                         MyKeyboardHookStruct.scanCode,
                         keyState,
                         inBuffer,
                         MyKeyboardHookStruct.flags) == 1)
                        {
                            KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
                            OnKeyPressEvent(this, e);
                        }
                    }

                    //引发OnKeyUpEvent
                    if (OnKeyUpEvent != null && (wParam == API.WM_KEYUP || wParam == API.WM_SYSKEYUP))
                    {
                        Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                        WayKeyEventArgs e = new WayKeyEventArgs(keyData, MyKeyboardHookStruct.scanCode);
                        OnKeyUpEvent(this, e);
                        if (e.CallNextHookEx == false)
                            return 1;
                    }
                }
                #endregion
            }
            else
            {
                Keys key = (Keys)wParam;
                WayKeyEventArgs e = new WayKeyEventArgs(key,0);
				int lp =  lParam.ToInt32();
				if ( lp > 0)
                {
                    if (OnKeyDownEvent != null)
                    {
						OnKeyDownEvent (null , e);
                    }
                }
                else if (lp < 0)
                {
                    if (OnKeyUpEvent != null)
                    {
                        OnKeyUpEvent(null, e);
                    }
                }

                if (e.CallNextHookEx == false)
                    return 1;
            }
            return API.CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
             
		}
		#endregion


	}
}
