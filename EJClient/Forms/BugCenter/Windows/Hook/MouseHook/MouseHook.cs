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
	public class MouseHook
	{
        /// <summary>
        /// 只要鼠标左键mouseup事件
        /// </summary>
        public bool JustGetLButtonEvent = false;
		#region 事件
		
			
		/// <summary>
		/// 
		/// </summary>
		public event WayMouseEventHandler OnMouseActivity;
        public bool 忽略MouseMove事件 = true;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="button"></param>
		/// <param name="clicks"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="delta"></param>
		/// <param name="ActiveType"></param>
		/// <returns>返回false表示不再把消息传到下个控件</returns>
		public delegate bool WayMouseEventHandler(MouseButtons button , int clicks , int x , int y , int delta , MouseActiveType ActiveType);
		#endregion

		/// <summary>
		/// 鼠标事件类型
		/// </summary>
		public enum MouseActiveType
		{
			/// <summary>
			/// 
			/// </summary>
			MouseDown = 0,
			/// <summary>
			/// 
			/// </summary>
			MouseUp = 1,
			/// <summary>
			/// 
			/// </summary>
			DBLClick = 2,
			/// <summary>
			/// 
			/// </summary>
			MouseMove = 3
		}
		private API.HookProc MouseHookProcedure;

		/// <summary>
		/// 
		/// </summary>
		private int hMouseHook = 0;

		private int threadID = 0;

		/// <summary>
		/// 
		/// </summary>
		public MouseHook()
		{

		}

		/// <summary>
		/// 
		/// </summary>
        ~MouseHook()
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
               hMouseHook = API.SetWindowsHookEx(API.WH_MOUSE_LL, MouseHookProcedure, hIntance, this.threadID);

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

       /// <summary>
        /// 开始安装线程钩子
        /// </summary>
       /// <param name="currentThreadId">线程Id</param>
        public void Start(int currentThreadId)
        {
            //安装鼠标钩子
            if (hMouseHook == 0)
            {
                //生成一个HookProc的实例.
                MouseHookProcedure = new API.HookProc(MouseHookProc);

                hMouseHook = API.SetWindowsHookEx(API.WH_MOUSE, MouseHookProcedure, (IntPtr)0, currentThreadId);

                //如果装置失败停止钩子
                if (hMouseHook == 0)
                {
                    Stop();
                    throw new Exception("安装钩子失败.");
                }
            }
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
		private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
		{
            if (this.JustGetLButtonEvent && wParam != API.WM_LBUTTONUP)
            {
                return API.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
            }


			//如果正常运行并且用户要监听鼠标的消息
			if ((nCode >= 0) && (OnMouseActivity != null))
			{
				MouseActiveType type = MouseActiveType.MouseMove;
				MouseButtons button = MouseButtons.None;
				int clickCount = 0;
                
				switch (wParam)
				{
					case API.WM_LBUTTONDOWN:
						button = MouseButtons.Left;
						clickCount = 1;
						type = MouseActiveType.MouseDown;
						break;
					case API.WM_LBUTTONUP:
						button = MouseButtons.Left;
						clickCount = 1;
						type = MouseActiveType.MouseUp;
						break;
					case API.WM_LBUTTONDBLCLK:
						button = MouseButtons.Left;
						clickCount = 2;
						type = MouseActiveType.DBLClick;
						break;
					case API.WM_RBUTTONDOWN:
						button = MouseButtons.Right;
						clickCount = 1;
						type = MouseActiveType.MouseDown;
						break;
					case API.WM_RBUTTONUP:
						button = MouseButtons.Right;
						clickCount = 1;
						type = MouseActiveType.MouseUp;
						break;
					case API.WM_RBUTTONDBLCLK:
						button = MouseButtons.Right;
						clickCount = 2;
						type = MouseActiveType.DBLClick;
						break;
				}

                if (this.忽略MouseMove事件 && type == MouseActiveType.MouseMove)
                {
                    goto returtag;
                }

				//从回调函数中得到鼠标的信息
				API.MouseHookStruct MyMouseHookStruct = (API.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(API.MouseHookStruct));

                if (_windowhandler > 0)
                {
                    int whandler = API.WindowFromPoint(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
                    if (whandler != _windowhandler)
                    {
                        while (true)
                        {
                            whandler = API.GetParent(new IntPtr( whandler) ).ToInt32();
                            if (whandler == _windowhandler)
                                break;
                            else if (whandler <= 0)
                                goto returtag;
                        }
                    }
                }
                
                bool flag = OnMouseActivity(button, clickCount, MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y, 0 , type);
                if (flag == false)
                {
                    return -1;
                }
            }

            returtag:
			return API.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
		}

		#endregion

	}
}
