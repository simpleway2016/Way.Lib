#region Using directives

using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
#endregion

namespace WayControls.Windows
{
	/// <summary>
	/// 
	/// </summary>
	public class API
	{
		/// <summary>
		/// 
		/// </summary>
		public API()
		{

		}

      

		#region 常量定义

        public const int MOUSEEVENTF_LEFTDOWN = 0x2;
        public const int MOUSEEVENTF_LEFTUP = 0x4;



        public const int WM_HOTKEY = 0x312;
        public const int DIB_RGB_COLORS = 0;
        public const int SRCCOPY = 0xCC0020;
        public const int WM_CLOSE = 0x10;
        public const int IMC_CLOSESTATUSWINDOW = 0x21;
        public const int IMC_OPENSTATUSWINDOW = 0x22;
        public const int WM_IME_CONTROL = 0x283;


        #region 各种钩子
        /// <summary>
        /// 
        /// </summary>
        public const int WH_CALLWNDPROC = 4;
        /// <summary>
        /// 
        /// </summary>
        public const int WH_CALLWNDPROCRET = 12;
        /// <summary>
        /// 
        /// </summary>
        public const int WH_CBT = 5;
        /// <summary>
        /// 
        /// </summary>
        public const int WH_DEBUG = 9;
        /// <summary>
        /// 
        /// </summary>
        public const int WH_FOREGROUNDIDLE = 11;
        /// <summary>
        /// 
        /// </summary>
        public const int WH_GETMESSAGE = 3;
        /// <summary>
        /// 
        /// </summary>
        public const int WH_HARDWARE = 8;
        /// <summary>
        /// 
        /// </summary>
        public const int WH_JOURNALPLAYBACK = 1;
        /// <summary>
        /// 
        /// </summary>
        public const int WH_JOURNALRECORD = 0;
        /// <summary>
        /// 线程键盘钩子
        /// </summary>
        public const int WH_KEYBOARD = 2;
        /// <summary>
        /// 线程鼠标钩子
        /// </summary>
        public const int WH_MOUSE = 7;
        /// <summary>
        /// 
        /// </summary>
        public const int WH_MSGFILTER = (-1);
        /// <summary>
        /// 
        /// </summary>
        public const int WH_SHELL = 10;
        /// <summary>
        /// 
        /// </summary>
        public const int WH_SYSMSGFILTER = 6;

        #endregion

        public const int SPI_SETFASTTASKSWITCH = 36;


        public const int WM_GETICON   =   127   ;
  public const int ICON_BIG   =   1   ;
        public const int ICON_SMALL = 0;

        public const int WM_PAINT = 0xF;

        /// <summary>
        /// 
        /// </summary>
        public const int WS_HSCROLL = 0x100000;
        /// <summary>
        /// 
        /// </summary>
        public const int WS_VSCROLL = 0x200000;

        /// <summary>
        /// </summary>
        public const uint SHGFI_DISPLAYNAME = 0x000000200;     // get display name
        /// <summary>
        /// </summary>
        public const uint SHGFI_TYPENAME = 0x000000400;     // get type name
        /// <summary>
        /// </summary>
        public const uint SHGFI_ATTRIBUTES = 0x000000800;     // get attributes
        /// <summary>
        /// </summary>
        public const uint SHGFI_ICONLOCATION = 0x000001000;     // get icon location
        /// <summary>
        /// </summary>
        public const uint SHGFI_EXETYPE = 0x000002000;     // return exe type
        /// <summary>
        /// </summary>
        public const uint SHGFI_SYSICONINDEX = 0x000004000;     // get system icon index
        /// <summary>
        /// </summary>
        public const uint SHGFI_LINKOVERLAY = 0x000008000;     // put a link overlay on icon
        /// <summary>
        /// </summary>
        public const uint SHGFI_SELECTED = 0x000010000;     // show icon in selected state
        /// <summary>
        /// </summary>
        public const uint SHGFI_ATTR_SPECIFIED = 0x000020000;     // get only specified attributes
        /// <summary>
        /// </summary>
        public const uint SHGFI_LARGEICON = 0x000000000;     // get large icon

        /// <summary>
        /// </summary>
        public const uint SHGFI_OPENICON = 0x000000002;     // get open icon
        /// <summary>
        /// </summary>
        public const uint SHGFI_SHELLICONSIZE = 0x000000004;     // get shell size icon
        /// <summary>
        /// </summary>
        public const uint SHGFI_PIDL = 0x000000008;     // pszPath is a pidl
        /// <summary>
        /// </summary>
        public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;     // use passed dwFileAttribute
        /// <summary>
        /// </summary>
        public const uint SHGFI_ADDOVERLAYS = 0x000000020;     // apply the appropriate overlays
        /// <summary>
        /// </summary>
        public const uint SHGFI_OVERLAYINDEX = 0x000000040;     // Get the index of the overlay

        /// <summary>
        /// </summary>
        public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        /// <summary>
        /// </summary>
        public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
        public const int WM_GETTEXT = 0xd;

        public const int WS_EX_TOPMOST = 0x8;
        public const int WS_DISABLED = 0x8000000;
        public const int WS_CLIPSIBLINGS = 0x4000000;
        public const int WS_OVERLAPPED = 0x0;
        public const int WM_CREATE = 0x1;

        /// <summary>
        /// 
        /// </summary>
        public const uint SHGFI_ICON = 0x100;

        /// <summary>
        /// 
        /// </summary>
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

		/// <summary>
		/// 
		/// </summary>
		public const int TOKEN_ADJUST_PRIVILEGES = 0x20;
		/// <summary>
		/// 
		/// </summary>
		public const int TOKEN_QUERY = 0x8;

        /// <summary>
        /// 
        /// </summary>
        public const int EM_FORMATRANGE = WM_USER + 57;

        /// <summary>
        /// 
        /// </summary>
        public const uint WS_POPUP = 0x80000000;

		/// <summary>
		/// 
		/// </summary>
		public const int SE_PRIVILEGE_ENABLED = 0x2;

        internal const int EWX_LOGOFF = 0x00000000;
        internal const int EWX_SHUTDOWN = 0x00000001;
        internal const int EWX_REBOOT = 0x00000002;
        internal const int EWX_FORCE = 0x00000004;
        internal const int EWX_POWEROFF = 0x00000008;
        internal const int EWX_FORCEIFHUNG = 0x00000010;

		/// <summary>
		/// 
		/// </summary>
		public const int SWP_NOSIZE = 0x1;
		/// <summary>
		/// 
		/// </summary>
		public const int SWP_NOZORDER =0x4 ;
		/// <summary>
		/// 
		/// </summary>
		public const int SWP_NOMOVE =0x2 ;
		/// <summary>
		/// 
		/// </summary>
		public const int SWP_DRAWFRAME =0x20 ;
		/// <summary>
		/// 
		/// </summary>
		public const int GWL_STYLE =(-16) ;
		/// <summary>
		/// 
		/// </summary>
		public const int WS_THICKFRAME =0x40000;

		/// <summary>
		/// 
		/// </summary>
		public const int WM_PRINTCLIENT =0x0318;

		/// <summary>
		/// 
		/// </summary>
		public const int WM_PRINT =15;

		/// <summary>
		/// 
		/// </summary>
		public const long PRF_CHECKVISIBLE =0x00000001L;
		/// <summary>
		/// 
		/// </summary>
		public const long PRF_NONCLIENT =0x00000002L;
		/// <summary>
		/// 
		/// </summary>
		public const long PRF_CLIENT =0x00000004L;
		/// <summary>
		/// 
		/// </summary>
		public const long PRF_ERASEBKGND =0x00000008L;
		/// <summary>
		/// 
		/// </summary>
		public const long PRF_CHILDREN =0x00000010L;
		/// <summary>
		/// 
		/// </summary>
		public const long PRF_OWNED =0x00000020L;

		/// <summary>
		/// 
		/// </summary>
		public const int WM_NCLBUTTONDOWN =0xA1;
		/// <summary>
		/// 
		/// </summary>
		public const int HTCAPTION =0x2;
        public const int SPI_SCREENSAVERRUNNING = 97;

		/// <summary>
		/// 
		/// </summary>
		public const int GW_CHILD = 5;
		#region Mouse
		/// <summary>
		/// 全局鼠标钩子
		/// </summary>
		public const int WH_MOUSE_LL =14; 
		/// <summary>
		/// 
		/// </summary>
		public const int WM_MOUSEMOVE =0x200;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_LBUTTONDOWN =0x201;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_RBUTTONDOWN =0x204;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_MBUTTONDOWN =0x207;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_LBUTTONUP =0x202;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_RBUTTONUP =0x205;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_MBUTTONUP =0x208;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_LBUTTONDBLCLK =0x203;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_RBUTTONDBLCLK =0x206;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_MBUTTONDBLCLK =0x209;

		#endregion

		#region 键盘
		/// <summary>
		/// 
		/// </summary>
		public const int WM_KEYDOWN =0x100;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_KEYUP =0x101;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_SYSKEYDOWN =0x104;
		/// <summary>
		/// 
		/// </summary>
		public const int WM_SYSKEYUP =0x105;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_SYSCOMMAND = 0x112;

        /// <summary>
        /// 
        /// </summary>
        public const int SC_RESTORE = 0xF120;
        public const int CS_DROPSHADOW = 0x20000;
        public const int GCL_STYLE = (-26);

        public const int OCR_NORMAL = 32512;
        public const int OCR_IBEAM = 32513;
		/// <summary>
		/// 全局键盘钩子
		/// </summary>
		public const int WH_KEYBOARD_LL =13;
		#endregion

        #region Const

        /// <summary>
        /// 
        /// </summary>
        public const int WM_MOUSEWHEEL = 0x20A;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_CHAR = 0x102;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_SETFOCUS = 0x7;

        /// <summary>
        /// 
        /// </summary>
        public const int KEYEVENTF_KEYUP = 0x2;

        /// <summary>
        /// 
        /// </summary>
        public const int KEYEVENTF_EXTENDEDKEY = 0x1;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_NCMOUSEMOVE = 0xA0;

        /// <summary>
        /// 
        /// </summary>
        public const int GWL_WNDPROC = -4;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_USER = 0x400;

        /// <summary>
        /// 
        /// </summary>
        public const int TRAY_CALLBACK = (WM_USER + 1001);
        /// <summary>
        /// 
        /// </summary>
        public const int WM_SETTEXT = 0xC;


        /// <summary>
        /// 
        /// </summary>
        public const int WM_ERASEBKGND = 0x14;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_NC_HITTEST = 0x84;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_NC_PAINT = 0x85;


        /// <summary>
        /// 
        /// </summary>
        public const int SB_HORZ = 0x0;
        /// <summary>
        /// 
        /// </summary>
        public const int SB_VERT = 0x1;

        /// <summary>
        /// 
        /// </summary>
        public const int WS_CAPTION = 0x00000;
        /// <summary>
        /// 
        /// </summary>
        public const int WS_BORDER = 0x800000;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_VSCROLL = 0x115;
        /// <summary>
        /// 
        /// </summary>
        public const int SB_LINEDOWN = 0x1;

        /// <summary>
        /// 
        /// </summary>
        public const int EM_SETSEL = 0xB1;
        /// <summary>
        /// 
        /// </summary>
        public const int WS_VISIBLE = 0x10000000;

        /// <summary>
        /// 
        /// </summary>
        public const int SM_CXVHSCROLL = 21;
        #endregion


		#endregion

		#region 结构定义

		/// <summary>
		/// 声明一个Point的封送类型
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
            /// <summary>
            /// 
            /// </summary>
            public int x;
            /// <summary>
            /// 
            /// </summary>
			public int y;
		}

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
      public struct FORMATRANGE
        {
            /// <summary>
            /// </summary>
         public IntPtr hdc ;
         /// <summary>
         /// </summary>
         public IntPtr hdcTarget;
         /// <summary>
         /// </summary>
         public Rect rc ;
         /// <summary>
         /// </summary>
         public Rect rcPage ;
         /// <summary>
         /// </summary>
          public CHARRANGE chrg;
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct CHARRANGE
		{
            /// <summary>
            /// 
            /// </summary>
            public int cpMin;
            /// <summary>
            /// 
            /// </summary>
            public int cpMax;
		}

        
        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            /// <summary>
            /// 
            /// </summary>
            public int left;
            /// <summary>
            /// 
            /// </summary>
            public int top;
            /// <summary>
            /// 
            /// </summary>
            public int right;
            /// <summary>
            /// 
            /// </summary>
            public int bottom;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="top"></param>
            /// <param name="right"></param>
            /// <param name="bottom"></param>
            public Rect(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
 

            /// <summary>
            /// 
            /// </summary>
            /// <param name="r"></param>
            public Rect(Rectangle r)
            {
                this.left = r.Left;
                this.top = r.Top;
                this.right = r.Right;
                this.bottom = r.Bottom;
            }

            /// <summary>
            /// 
            /// </summary>
            public Size Size
            {
                get
                {
                    return new Size(this.right - this.left, this.bottom - this.top);
                }
            }
 

        }
 


		/// <summary>
///  声明需要使用的Windows API函数
/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct TokPriv1Luid
		{
            /// <summary>
            /// 
            /// </summary>
			public int Count;
            /// <summary>
            /// 
            /// </summary>
			public long Luid;
            /// <summary>
            /// 
            /// </summary>
			public int Attr;
		}

		/// <summary>
		/// 声明鼠标钩子的封送结构类型
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public class MouseHookStruct
		{
            /// <summary>
            /// 
            /// </summary>
			public POINT pt;
            /// <summary>
            /// 
            /// </summary>
			public int hWnd;
            /// <summary>
            /// 
            /// </summary>
			public int wHitTestCode;
            /// <summary>
            /// 
            /// </summary>
			public int dwExtraInfo;
		}

        [StructLayout(LayoutKind.Sequential)]
        public class MSG
        {
            public IntPtr HWND;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT point;
        }

		/// <summary>
		/// 声明键盘钩子的封送结构类型
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public class KeyboardHookStruct
		{
            /// <summary>
            /// 表示一个在1到254间的虚似键盘码
            /// </summary>
			public int vkCode; //
			/// <summary>
			/// 表示硬件扫描码
			/// </summary>
            public int scanCode; //
            /// <summary>
            /// 
            /// </summary>
			public int flags;
            /// <summary>
            /// 
            /// </summary>
			public int time;
            /// <summary>
            /// 
            /// </summary>
			public int dwExtraInfo;
		}


        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            /// <summary>
            /// </summary>
            public IntPtr hIcon;
            /// <summary>
            /// </summary>
            public IntPtr iIcon;
            /// <summary>
            /// </summary>
            public uint dwAttributes;
            /// <summary>
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            /// <summary>
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
            public const int BI_RGB = 0; 

        }

         [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPFILEHEADER 
                   {
                       public ushort bfType;
                       public int bfSize;
                       public ushort bfReserved1;
                       public ushort bfReserved2;
                       public int bfOffBits; 

                   } 


        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            [MarshalAs(UnmanagedType.Struct, SizeConst = 40)]
            public BITMAPINFOHEADER bmiHeader;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public Int32[] bmiColors;
        }

		#endregion

		#region 委托定义
		/// <summary>
		/// 
		/// </summary>
		/// <param name="nCode"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
		#endregion

		#region 函数定义
        [DllImport("user32")]
        public static extern int GetMenu(int hwnd);
        [DllImport("kernel32.dll")]
    public static extern
        Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, ref int lpNumberOfBytesWritten);


        [DllImport("kernel32.dll")]
        public static extern
            Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref byte[] lpBuffer, int nSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        public static extern int GetLocaleInfo(int Locale, int LCType, StringBuilder lpLCData, int cchData);


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PORT_INFO_1
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pName;
        }

        [DllImport("winspool.drv", CharSet = CharSet.Auto)]
       public static extern bool EnumPorts(string pName, int level, IntPtr bufptr,
         int cbBuf, out int pcbNeeded, out int pcReturned);

        [DllImport("kernel32", EntryPoint = "GetDiskFreeSpaceExA")]
        public static extern int GetDiskFreeSpaceEx(string lpDirectoryName, ref long lpFreeBytesAvailableToCaller, ref long
             lpTotalNumberOfBytes, ref long lpTotalNumberOfFreeBytes);


        /// <summary>
        /// 真正激活窗口
        /// </summary>
        /// <param name="hander"></param>
        /// <param name="restore"> Restore   the   window   if   it   is   minimized      </param>
        [DllImport("user32")]
        public static extern void SwitchToThisWindow(IntPtr hander , bool restore);
        public enum keyEventCode
        {
            KeyDown = 0,
            KeyUp = 2
        }

        [DllImport("user32")]
        public static extern void keybd_event(System.Windows.Forms.Keys key, int bScan, keyEventCode dwFlags, int dwExtraInfo);

        [DllImport("user32", EntryPoint = "MapVirtualKey")]
        public static extern int MapVirtualKeyA(System.Windows.Forms.Keys wCode, int wMapType);

        [DllImport("user32", EntryPoint = "MapVirtualKey")]
        public static extern int MapVirtualKeyA(int wCode, int wMapType);
        public struct MENUITEMINFO
        {
	public int cbSize ;
	public int fMask ;
	public int fType ;
	public int fState ;
	public int wID ;
public int 	hSubMenu ;
	public int hbmpChecked ;
	public int hbmpUnchecked ;
	public int dwItemData ;
	public string dwTypeData ;
            public int cch;
    }
        public const int MF_BYPOSITION = 0x400;

        [DllImport("user32")]
        public static extern int GetMenuItemCount(int hMenu);
        public const int WM_COMMAND = 0x111;

        [DllImport("user32")]
        public static extern int GetMenuItemID(int hMenu, int nPos);

        [DllImport("user32")]
        public static extern int GetSubMenu(int hMenu, int nPos);

        [DllImport("user32", EntryPoint = "GetMenuItemInfo")]
        public static extern int GetMenuItemInfoA(int hMenu, int un, int b, ref MENUITEMINFO lpMenuItemInfo);
        [DllImport("user32", EntryPoint = "GetMenuString")]
        public static extern int GetMenuStringA(int hMenu, int wIDItem, StringBuilder lpString, int nMaxCount, int wFlag);
        public const int CBM_INIT = 0x4;
        [DllImport("gdi32")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, BITMAPINFO bitInfo, int dwUsage, int lpData, int handle, int dw);

        [DllImport("gdi32")]
        public static extern IntPtr CreateDIBitmap(IntPtr hdc,BITMAPINFOHEADER lpInfoHeader, int dwUsage, object lpInitBits, ref BITMAPINFO lpInitInfo, int wUsage);
        [DllImport("gdi32")]
        public static extern IntPtr CreateCompatibleDC(int hdc);

        [DllImport("gdi32")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private extern static string GetCommandLine();
        [DllImport("user32.dll")]
        public static extern int CreateWindowEx(
                        int dwExStyle,
                        string lpClassName,
                        string lpWindowName,
                        uint dwStyle,
                        int X,  int Y,
                        int nWidth,  int nHeight,
                        IntPtr hWndParent ,
                        int hMenu,
                        int hInstance,
                        int lpParam);


        [DllImport("user32.dll")]
        public static extern int IsWindow(IntPtr hwnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow">
        /// public   const   int   SW_HIDE   =   0   
        ///public   const   int   SW_SHOWNORMAL   =   1   
        ///public   const   int   SW_SHOWMAXIMIZED   =   3   
        ///public   const   int   SW_SHOWNOACTIVATE   =   4   
        ///public   const   int   SW_MINIMIZE   =   6   
        ///public   const   int   SW_SHOWMINNOACTIVE   =   7   
        ///public   const   int   SW_SHOWNA   =   8   
        ///public   const   int   SW_RESTORE   =   9   
        ///SW_HIDE       
        ///隐藏窗口，活动状态给令一个窗口   
        ///SW_MINIMIZE   
        ///最小化窗口，活动状态给令一个窗口   
        ///SW_RESTORE   
        ///用原来的大小和位置显示一个窗口，同时令其进入活动状态   
        ///SW_SHOW   
        ///用当前的大小和位置显示一个窗口，同时令其进入活动状态   
        ///SW_SHOWMAXIMIZED   
        ///最大化窗口，并将其激活   
        ///SW_SHOWMINIMIZED   
        ///最小化窗口，并将其激活   
        ///SW_SHOWMINNOACTIVE   
        ///最小化一个窗口，同时不改变活动窗口   
        ///SW_SHOWNA   
        ///用当前的大小和位置显示一个窗口，不改变活动窗口   
        ///SW_SHOWNOACTIVATE   
        ///用最近的大小和位置显示一个窗口，同时不改变活动窗口   
        ///SW_SHOWNORMAL   
        ///与SW_RESTORE相同 
        /// </param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        public static extern int ShowWindow(
        IntPtr hwnd,
        int nCmdShow
        );

        /// <summary>
        /// 获得前台窗口的句柄。这里的“前台窗口”是指前台应用程序的活动窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32")]
        public static extern int SetSystemCursor(int cursorhandle, int lpCursorName);


        [DllImport("user32", EntryPoint = "LoadCursorA")]
        public static extern int LoadCursor(int hInstance, int lpCursorName);

        [DllImport("user32", EntryPoint = "LoadCursorFromFileA")]
        public static extern IntPtr LoadCursorFromFile(string lpFileName);


        [DllImport("kernel32")]
        public static extern int LoadLibrary(string filename);

        /// <summary>
        /// 真正激活窗口请用SwitchToThisWindow
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int SetForegroundWindow(int hwnd);

        /// <summary>
        /// 获取当前进程的活动窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("kernel32", EntryPoint = "FindResourceA")]
        public static extern int FindResource(int hInstance, string resourcename, IntPtr type);

        [DllImport("kernel32", EntryPoint = "EnumResourceTypesA", CharSet = CharSet.Auto)]
        public static extern int EnumResourceTypes(int hModule, EnumTypeFuncHanlder func, int lparam);

        public delegate bool EnumTypeFuncHanlder(int handler, IntPtr type, int lparam);

        [DllImport("kernel32", EntryPoint = "UpdateResourceA")]
        public static extern int UpdateResource(int hUpdate, IntPtr lpType, string lpname, int wLanguage, byte[] lpdata, int cbData);


        [DllImport("kernel32", EntryPoint = "EndUpdateResourceA")]
        public static extern int EndUpdateResource(int hupdate, int fDiscard);


        [DllImport("kernel32")]
        public static extern int SizeofResource(int hInstance, int hResInfo);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bDeleteExistingResources">说明是否删除PFileName参数指定的现有资源。如果这个参数为TRUE则现有的资源将被删除，而更新可执行文件只包括由UpdateResource函数增加的资源。如果这个参数为FALSE，则更新的可执行文件包括现有的全部资源，除非通过UpdateResource特别说明被删除或是替换的。</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "BeginUpdateResourceA")]
        public extern static int BeginUpdateResource(string filePath, bool bDeleteExistingResources);

        /// <summary>
        /// 通过窗口句柄获得进程
        /// </summary>
        /// <param name="windowHandler"></param>
        /// <param name="processid"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public extern static int GetWindowThreadProcessId(IntPtr windowHandler, ref int processid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public extern static int LockWindowUpdate(IntPtr hwnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BeginUpdateResourceReturnValue"></param>
        /// <param name="lpType"></param>
        /// <param name="lpName"></param>
        /// <param name="wLanaguage">如2052</param>
        /// <param name="data"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "UpdateResourceA")]
        public extern static int UpdateResource(int BeginUpdateResourceReturnValue,
            int lpType, string lpName , int wLanaguage , object data , int dataLength);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="BeginUpdateResourceReturnValue"></param>
        /// <param name="fDiscard">用来说明是否向可执行文件中写入资源更新内容。如果此参数为TRUE，则在可执行文件中无变化；如果此参数为FALSE，则在可执行文件中写入变化。</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "EndUpdateResourceA")]
        public extern static int EndUpdateResource(int BeginUpdateResourceReturnValue, bool fDiscard);
        #region 滚动条
        /// <summary>
        /// 取拉动条最大拉动范围
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nBar">API.SB_VERT表示要取横向还是竖向的拉动条</param>
        /// <param name="lpMinPos"></param>
        /// <param name="lpMaxPos"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetScrollRange(IntPtr hWnd, int nBar, ref int lpMinPos, ref int lpMaxPos);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="bRevert">如设为0，表示恢复原始的系统菜单</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetSystemMenu(IntPtr hWnd, int bRevert);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="nPosition"></param>
        /// <param name="wFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int RemoveMenu(int hMenu, int nPosition , int wFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nBar"></param>
        /// <param name="nMinPos"></param>
        /// <param name="nMaxPos"></param>
        /// <param name="bReDraw"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetScrollRange(IntPtr hWnd, int nBar, int nMinPos, int nMaxPos, bool bReDraw);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
        /// <summary>
        /// 取拉动条当前位置
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nBar">API.SB_VERT表示要取横向还是竖向的拉动条</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pszPath"></param>
        /// <param name="dwFileAttributes"></param>
        /// <param name="psfi"></param>
        /// <param name="cbSizeFileInfo"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpszFile"></param>
        /// <param name="nIconIndex"></param>
        /// <param name="phiconLarge"></param>
        /// <param name="phiconSmall"></param>
        /// <param name="nIcons"></param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, int[] phiconLarge, int[] phiconSmall, uint nIcons);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd">表示当前的程序实例 </param>
        /// <param name="filename">表示包含图标的资源文件名 </param>
        /// <param name="index">表示要取出的图标的序号 
        /// ---- 如 果nIconIndex 为-1, 则 函 数 返 回 包 含 图 标 资 源 的 文 件 的 图 标 个 数. 
        /// ---- 从 文 件 中 取 出 图 标 资 源 前, 应 首 先 调 用 该 函 数 获 得 文 件 中 包 含 的 图 标 资 源 的 个 数. 
        /// ---- 如nIconIndex 为 图 标 资 源 的 序 号， 则 返 回 图 标 句 柄. 
        /// </param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public static extern IntPtr ExtractIcon(IntPtr hwnd, string filename, int index);
        /// <summary>
        /// 设置窗口为最上
        /// </summary>
        /// <param name="handler"></param>
        public static void SetWindowTopMost(IntPtr handler)
        {
            SetWindowPos(handler, -1, 0, 0, 0, 0, 3);
        }

        /// <summary>
        /// gets 桌面句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();



        /// <summary>
        /// 
        /// </summary>
        /// <param name="hModule"></param>
        /// <param name="lpProcName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern IntPtr GetProcAddress(IntPtr handler, string lpProcName);
 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetWindowText(int hwnd, string text);
        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr hwnd, string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int DestroyWindow(int hwnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Wnd"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Repaint"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "MoveWindow")]
        public static extern bool MoveWindow(
            int Wnd,
            int X,
            int Y,
            int Width,
            int Height,
            bool Repaint
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strclassName"></param>
        /// <param name="strWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strclassName, string strWindowName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool GetCursorPos([In, Out] POINT pt);
 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionDescription"></param>
		/// <param name="reservedValue"></param>
		/// <returns></returns>
		[DllImport("wininet.dll")]
		public extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        /// <summary>
        /// 真正激活窗口请用SwitchToThisWindow
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int SetActiveWindow(IntPtr handler);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetFocus")]
        public extern static void SetFocusAPI(IntPtr handler);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpPrevWndFunc"></param>
        /// <param name="HWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "CallWindowProcA")]
        public static extern int CallWindowProc(int lpPrevWndFunc, IntPtr HWnd, int Msg, int wParam, int lParam);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parentHandler"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public extern static IntPtr GetWindow(IntPtr parentHandler , int param);


        [DllImport("user32.dll")]
		public extern static int GetLastActivePopup(IntPtr hwndOwnder);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="output"></param>
		/// <param name="outputLen"></param>
		/// <returns></returns>
		[DllImport("user32", EntryPoint = "GetWindowTextA")]
		public extern static int GetWindowText(IntPtr hwnd , StringBuilder output, int outputMaxLen);

        [DllImport("user32")]
        public extern static int ShowCursor(bool show);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="output"></param>
		/// <param name="outputMaxLen"></param>
		/// <returns></returns>
		[DllImport("user32", EntryPoint = "GetClassNameA")]
		public extern static int GetClassName(IntPtr hwnd, StringBuilder output, int outputMaxLen);
        public static string GetClassName(IntPtr hwnd)
        {
            StringBuilder str = new StringBuilder(500);
            GetClassName(hwnd, str, 500);
            return str.ToString();
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="windowHandler"></param>
		/// <param name="lparam"></param>
		/// <returns></returns>
		public delegate bool EnumChildWindowsCallBackHandler(IntPtr windowHandler , int lparam);

		/// <summary>
		/// 枚举指定父窗口的子窗口
		/// </summary>
		/// <param name="ParentHandler"></param>
		/// <param name="callback"></param>
		/// <param name="lparam"></param>
		/// <returns></returns>
		[DllImport("user32")]
		public extern static int EnumChildWindows(IntPtr ParentHandler , EnumChildWindowsCallBackHandler callback , int lparam );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetDC", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hDC"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ReleaseDC", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);


		/// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="outputStr">用来装在className</param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "GetClassNameA")]
        public static extern int GetClassName(int hwnd, System.Text.StringBuilder outputStr, int maxCount);


        /// <summary>
		/// 
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		[DllImport("USER32.DLL", EntryPoint = "GetWindowLongA")]
		public static extern int GetWindowLong(IntPtr handle , int index );

        /// <summary>
        /// 激活输入法
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL")]
        public static extern int ActivateKeyboardLayout(IntPtr handle, int flags);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWndChild"></param>
        /// <param name="hWndNewParent"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

       /// <summary>
       /// 
       /// </summary>
       /// <param name="hWndChild"></param>
       /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWndChild);
        
        /// <summary>
		/// 
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="index"></param>
		/// <param name="dwNewLong"></param>
		/// <returns></returns>
		[DllImport("USER32.DLL", EntryPoint = "SetWindowLongA")]
		public static extern int SetWindowLong(IntPtr handle, int index , int dwNewLong);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="msg"></param>
        /// <param name="wparam"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        public delegate int MessageCutorHandler(IntPtr handler , int msg , int wparam , int lparam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="MessageCutorFunc"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL", EntryPoint = "SetWindowLongA")]
        public static extern int SetWindowLong(IntPtr handle, int index, MessageCutorHandler MessageCutorFunc);

       /// <summary>
       /// 
       /// </summary>
       /// <param name="handle"></param>
       /// <param name="index"></param>
       /// <param name="MessageCutorFunc"></param>
       /// <returns></returns>
        [DllImport("USER32.DLL", EntryPoint = "SetWindowLongA")]
        public static extern int SetWindowLong(IntPtr handle, long index, long MessageCutorFunc);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hwnd"></param>
        /// <param name="hWndInsertAfter">参数2取-1表示在最顶层显示窗口，取1表示在最底层显示</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="wFlags">若取1，表示窗口大小保持不变，取2表示保持位置不变，因此，取3（=1+2）表示大小和位置均保持不变，取0表示将窗口的大小和位置改变为指定值</param>
		/// <returns></returns>
		[DllImport("USER32.DLL")]
		public static extern int SetWindowPos(IntPtr hwnd , int hWndInsertAfter, int x 
			, int y , int width , int height , int wFlags );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="Msg"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern int SendNotifyMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImportAttribute("user32.dll")]
        public static extern int mouse_event(int dwFlags , int dx , int dy , int cButtons , int dwExtraInfo );



        [DllImportAttribute("user32.dll")]
        public static extern int SendNotifyMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="hRgn"></param>
		/// <param name="bRedraw"></param>
		/// <returns></returns>
		[DllImportAttribute("user32.dll")]
		public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <param name="count"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		[DllImportAttribute("gdi32.dll")]
		public static extern IntPtr CreatePolygonRgn(Point[] point, int count, int type);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <returns></returns>
		[DllImportAttribute("gdi32.dll")]
		public static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="hRgn"></param>
		/// <returns></returns>
		[DllImportAttribute("user32.dll")]
		public static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        [DllImportAttribute("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd,[In , Out] ref Rect rect);

        [DllImport("user32")]
        public static extern int GetClientRect(IntPtr hwnd, [In, Out] ref Rect rect);


        [DllImport("user32")]
        public static extern int ClientToScreen(int hwnd, ref Point lpPoint);
        [DllImport("user32")]
        public static extern int ClientToScreen(IntPtr hwnd, ref Point lpPoint);

		/// <summary>
		/// 获取当前线程一个唯一的线程标识符
		/// </summary>
		/// <returns></returns>
		[DllImport("kernel32")]
		public static extern int GetCurrentThreadId();

		/// <summary>
		/// 创建场景
		/// </summary>
		/// <param name="lpszDriver">驱动名称</param>
		/// <param name="lpszDevice">设备名称</param>
		/// <param name="lpszOutput">无用，可以设定位"NULL" </param>
		/// <param name="lpInitData">任意的打印机数据 </param>
		/// <returns></returns>
		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern IntPtr CreateDC(
		string lpszDriver, //  
			string lpszDevice, //  
			string lpszOutput, // 
			IntPtr lpInitData // 
			);

		
		/// <summary>
		/// 拷贝设备场景
		/// </summary>
		/// <param name="hdcDest">目标设备的句柄</param>
		/// <param name="nXDest">目标对象的左上角的X坐标 </param>
		/// <param name="nYDest">目标对象的左上角的X坐标 </param>
		/// <param name="nWidth">目标对象的矩形的宽度</param>
		/// <param name="nHeight">目标对象的矩形的长度</param>
		/// <param name="hdcSrc">源设备的句柄</param>
		/// <param name="nXSrc">源对象的左上角的X坐标</param>
		/// <param name="nYSrc">源对象的左上角的X坐标 </param>
		/// <param name="dwRop">光栅的操作值 </param>
		/// <returns></returns>
		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern bool BitBlt(
		IntPtr hdcDest, // 
			int nXDest, // 
			int nYDest, // 
			int nWidth, //  
			int nHeight, //  
			IntPtr hdcSrc, //  
			int nXSrc, //  
			int nYSrc, // 
			System.Int32 dwRop // 
			);

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="msg"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[DllImport("USER32.DLL", EntryPoint ="PostMessage")]
		public static extern bool PostMessage(IntPtr hwnd, int msg,
            int wParam, int lParam);

        [DllImport("USER32.DLL", EntryPoint = "PostMessage")]
        public static extern bool PostMessage(int hwnd, int msg,
            int wParam, int lParam);


        [DllImport("USER32.DLL", EntryPoint = "PostMessage")]
        public static extern bool PostMessage(IntPtr hwnd, int msg,
            int wParam, uint lParam);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="msg"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[DllImport("USER32.DLL", EntryPoint ="SendMessage")]
		public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam,
			IntPtr lParam);
        //SendMessage ParentHandle, WM_GETTEXT, Len(strText), ByVal strText 

            [DllImport("USER32.DLL", EntryPoint ="SendMessage")]
		public static extern int SendMessage(IntPtr hwnd, int msg, int wParam,
			StringBuilder text);


        [DllImport("USER32.DLL", EntryPoint = "SendNotifyMessage")]
        public static extern int SendNotifyMessage(IntPtr hwnd, int msg, int wParam,
            StringBuilder text);

        /// <summary>
        /// 用于生成简单的声音
        /// </summary>
        /// <param name="dwFreq">声音频率（从37Hz到32767Hz）。在windows95中忽略</param>
        /// <param name="dwDuration">声音的持续时间，以毫秒为单位。如为-1，表示一直播放声音，直到再次调用该函数为止。在windows95中会被忽略</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern int Beep(int dwFreq , int dwDuration );

		/// <summary>
		/// 脱字符号连续两次闪烁间隔的时间﹐以毫秒为单位
		/// </summary>
		/// <returns></returns>
		[DllImport("USER32.DLL", EntryPoint ="GetCaretBlinkTime")]
		public static extern uint GetCaretBlinkTime();

		/// <summary>
		/// 装置钩子的函数
		/// </summary>
		/// <param name="idHook"></param>
		/// <param name="lpfn"></param>
		/// <param name="hInstance"></param>
		/// <param name="threadId"></param>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet =CharSet.Auto,SetLastError=true, CallingConvention =CallingConvention.StdCall)]
		public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

		/// <summary>
		/// 卸下钩子的函数
		/// </summary>
		/// <param name="idHook"></param>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet =CharSet.Auto, CallingConvention =CallingConvention.StdCall)]
		public static extern bool UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// 根据屏幕坐标获取窗口句柄
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int WindowFromPoint(int x, int y);

        

		/// <summary>
		/// 下一个钩挂的函数
		/// </summary>
		/// <param name="idHook"></param>
		/// <param name="nCode"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet =CharSet.Auto, CallingConvention =CallingConvention.StdCall)]
		public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);


       
        //声明Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cursorHandle"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern IntPtr SetCursor(IntPtr cursorHandle);

        /// <summary>
        /// 设定 Mouse 光标的位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x , int y);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cursorHandle"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern uint DestroyCursor(IntPtr cursorHandle);




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[DllImport("kernel32.dll", ExactSpelling = true)]
		public static extern IntPtr GetCurrentProcess();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="h"></param>
		/// <param name="acc"></param>
		/// <param name="phtok"></param>
		/// <returns></returns>
		[DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="host"></param>
		/// <param name="name"></param>
		/// <param name="pluid"></param>
		/// <returns></returns>
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="htok"></param>
		/// <param name="disall"></param>
		/// <param name="newst"></param>
		/// <param name="len"></param>
		/// <param name="prev"></param>
		/// <param name="relen"></param>
		/// <returns></returns>
		[DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
		internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
			ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="flg"></param>
		/// <param name="rea"></param>
		/// <returns></returns>
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool ExitWindowsEx(int flg, int rea);
        
    [DllImport("user32")] 
    public static extern int EnumWindows(EnumWindowCallBack x, int y); 




		/// <summary>
		/// 
		/// </summary>
		/// <param name="uVirtKey"></param>
		/// <param name="uScanCode"></param>
		/// <param name="lpbKeyState"></param>
		/// <param name="lpwTransKey"></param>
		/// <param name="fuState"></param>
		/// <returns></returns>
		[DllImport("user32")]
		public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pbKeyState"></param>
		/// <returns></returns>
		[DllImport("user32")]
		public static extern int GetKeyboardState(byte[] pbKeyState);
		#endregion

		#region 扩展函数


        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref   RECT param, uint fWinINI);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool SystemParametersInfo(int uiAction, int uiParam, ref int param, int fWinINI);
 
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        const int SPI_SETWORKAREA = 0x002F;

        /// <summary>
        /// 设置屏幕workingArea
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetWorkintArea(int left, int top, int width, int height)
        {
            RECT rect = new RECT();
            rect.left = left;
            rect.top = top;
            rect.right = left + width;
            rect.bottom = top + height;
            SystemParametersInfo(SPI_SETWORKAREA, 0, ref   rect, 0);
        }

        /// <summary>
        /// 设置窗口具有阴影效果
        /// </summary>
        /// <param name="handler"></param>
        public static void SetWindowDropShadow(IntPtr handler)
        {
            SetClassLong(handler, GCL_STYLE, GetClassLong(handler, GCL_STYLE) | CS_DROPSHADOW);
        }


        public const int GW_OWNER = 4;
        public delegate bool EnumWindowCallBack(IntPtr hwnd, int lParam);

        public static IntPtr GetProcessMainWindowHandle(int processid)
        {
            IntPtr mainHandler = IntPtr.Zero;
            EnumWindowCallBack callback = delegate(IntPtr hwnd, int lParam)
            {
                int proid = 0;
                GetWindowThreadProcessId(hwnd, ref proid);
                if (proid == processid)
                {
                    //IntPtr pWnd = GetParent(hwnd);

                    //while (pWnd != IntPtr.Zero)
                    //{
                    //    hwnd = pWnd;
                    //    pWnd = GetParent(pWnd);
                    //}
                    //mainHandler = hwnd;
                    //return false;

                    IntPtr AppWin = GetWindow(hwnd, GW_OWNER);
                    if (AppWin == IntPtr.Zero)
                    {
                        mainHandler = hwnd;
                        return false;
                    }
                    while (AppWin != IntPtr.Zero)
                    {
                        hwnd = AppWin;
                        AppWin = GetWindow(AppWin, GW_OWNER);
                    }
                    mainHandler = hwnd;
                    return false;

                }
                return true;
            };
            EnumWindows(callback, 0);
            return mainHandler;
        }

        public static IntPtr[] GetProcessMainWindowHandles(int processid)
        {
            List<IntPtr> handlers = new List<IntPtr>();

            EnumWindowCallBack callback = delegate(IntPtr hwnd, int lParam)
            {
                int proid = 0;
                GetWindowThreadProcessId(hwnd, ref proid);
                if (proid == processid)
                {
                    IntPtr pWnd = GetParent(hwnd);

                    while (pWnd != IntPtr.Zero)
                    {
                        hwnd = pWnd;
                        pWnd = GetParent(pWnd);
                    }

                    handlers.Add(hwnd);
                }
                return true;
            };
            EnumWindows(callback, 0);
            return handlers.ToArray();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int GetWindowFromPoint(int x, int y)
        {
            return WindowFromPoint(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hThread">GetCurrentThread()</param>
        /// <param name="dwThreadAffinityMask"></param>
        /// <returns>返回上一次使用的cpu</returns>
        [DllImport("kernel32.dll")]
        public static extern int SetThreadAffinityMask(IntPtr hThread,
           int dwThreadAffinityMask);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThread();

        /// <summary>
        /// 更新一个EXE或DLL文件的字符串资源
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="lpType">资源类型</param>
        /// <param name="ResourceName">资源名称</param>
        /// <param name="Value">字符串值</param>
        public static void UpdateFileStringResource(string filePath ,int lpType , string ResourceName , string Value )
        {

            byte[] bs = System.Text.Encoding.Unicode.GetBytes(Value);



            int hUpdate = BeginUpdateResource(filePath, false);

            int c2 = UpdateResource(hUpdate, (IntPtr)lpType, ResourceName, 0, bs, bs.Length);
            c2 = EndUpdateResource(hUpdate, 0);

            if (c2 == 0)
            {
                throw(new Exception("更新失败"));
            }
        }

        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            internal _PROCESSOR_INFO_UNION uProcessorInfo;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort dwProcessorLevel;
            public ushort dwProcessorRevision;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct _PROCESSOR_INFO_UNION
        {
            [FieldOffset(0)]
            internal uint dwOemId;
            [FieldOffset(0)]
            internal ushort wProcessorArchitecture;
            [FieldOffset(2)]
            internal ushort wReserved;
        }

        /// <summary>
        /// 获取窗口文本,包括当前进程的窗口也可以
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static string GetWindowText(IntPtr handler)
        {
            StringBuilder sb = new StringBuilder(1255);

            SendMessage(handler, WM_GETTEXT, 1255, sb);
            return sb.ToString();
        }


        /// <summary>
        /// 用一个字符串去更新多个EXE或DLL文件的字符串资源
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="lpType">资源类型</param>
        /// <param name="ResourceNames">资源名称</param>
        /// <param name="Value">字符串值</param>
        public static void UpdateFileStringResource(string filePath, int lpType, string[] ResourceNames, string Value)
        {

            byte[] bs = System.Text.Encoding.Unicode.GetBytes(Value + "\0");



            int hUpdate = BeginUpdateResource(filePath, false);

            int c2;
            for (int i = 0; i < ResourceNames.Length; i++)
            {
                c2 = UpdateResource(hUpdate, (IntPtr)lpType, ResourceNames[i], 0, bs, bs.Length);


                if (c2 == 0)
                {
                    throw (new Exception("更新" + ResourceNames[i] + "失败"));
                }
            }

            c2 = EndUpdateResource(hUpdate, 0);

            if (c2 == 0)
            {
                throw (new Exception("所有更新失败"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public enum IconSizeType
        {
            /// <summary>
            /// </summary>
            Large = 0,
            /// <summary>
            /// </summary>
            Small = 1,
            /// <summary>
            /// </summary>
            Normal = 2
        }

        /// <summary>
        /// 使窗体关闭失效
        /// </summary>
        /// <param name="handler"></param>
        public static void DisableWindowClose(IntPtr handler)
        {
            int hmenu = GetSystemMenu(handler, 0);
            RemoveMenu(hmenu, 0xF060, 0);
        }

        /// <summary>
        /// 依据扩展名读取图标
        /// </summary>
        /// <param name="Extension">扩展名</param>
        /// <returns></returns>
        public static Icon GetIconByExtension(string Extension, IconSizeType sizeType) 
        {

            SHFILEINFO shinfo = new SHFILEINFO();
            //Use this to get the small Icon
            uint size = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES;
            if (sizeType == IconSizeType.Large)
            {
                size += SHGFI_LARGEICON ;
            }
            else if (sizeType == IconSizeType.Small)
            {
                size += SHGFI_SMALLICON ;
            }

            SHGetFileInfo(Extension, FILE_ATTRIBUTE_NORMAL, ref shinfo, (uint)Marshal.SizeOf(shinfo), size);
            //The icon is returned in the hIcon member of the shinfo struct
            System.Drawing.Icon myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
            return myIcon;
        }


        /// <summary>
        /// 注册ActiveX
        /// </summary>
        /// <param name="DllFileName">dll文件路径</param>
        public static void RegisterDll(string DllFileName)
        {
            IntPtr hwnd = (IntPtr)WayControls.Windows.API.LoadLibrary(DllFileName);
            hwnd = WayControls.Windows.API.GetProcAddress(hwnd, "DllRegisterServer");
            int result = WayControls.Windows.API.CallWindowProc(hwnd.ToInt32(), hwnd, 0, 0, 0);
        }

        /// <summary>
        /// 卸载ActiveX
        /// </summary>
        /// <param name="DllFileName">dll文件路径</param>
        public static void UnRegisterDll(string DllFileName)
        {
            IntPtr hwnd = (IntPtr)WayControls.Windows.API.LoadLibrary(DllFileName);
            hwnd = WayControls.Windows.API.GetProcAddress(hwnd, "DllUnregisterServer");
            int result = WayControls.Windows.API.CallWindowProc(hwnd.ToInt32(), hwnd, 0, 0, 0);
        }

		/// <summary>
		/// 把一个控件或窗体的内容拷贝到bitmap，覆盖在不包括该控件上的其他控件
		/// </summary>
		/// <param name="control"></param>
		/// <param name="bitmap"></param>
		/// <returns></returns>
		public static bool CaptureWindow(System.Windows.Forms.Control control,
										System.Drawing.Bitmap bitmap)
		{
			//This function captures the contents of a window or control

			Graphics g2 =Graphics.FromImage(bitmap);

			//PRF_CHILDREN // PRF_NONCLIENT
			int meint =(int)(PRF_CLIENT | PRF_ERASEBKGND); //| PRF_OWNED ); //  );
			System.IntPtr meptr =new System.IntPtr(meint);

			System.IntPtr hdc =g2.GetHdc();
			SendMessage(control.Handle, WM_PRINT, hdc, meptr);

			g2.ReleaseHdc(hdc);
			g2.Dispose();

			return true;

		}
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern bool BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);

        /// <summary>
        /// 只支持.net窗口
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="bitmap"></param>
        /// <param name="targetBounds"></param>
        public static void DrawToBitmap(IntPtr handler, Bitmap bitmap, Rectangle targetBounds)
        {

            Bitmap image = new Bitmap(targetBounds.Width, targetBounds.Height, bitmap.PixelFormat);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                IntPtr hdc = graphics.GetHdc();
                SendMessage(handler, 0x317, hdc, (IntPtr)30);
                using (Graphics graphics2 = Graphics.FromImage(bitmap))
                {
                    IntPtr handle = graphics2.GetHdc();
                    BitBlt(new HandleRef(graphics2, handle), targetBounds.X, targetBounds.Y, targetBounds.Width, targetBounds.Height, new HandleRef(graphics, hdc), 0, 0, 0xcc0020);
                    graphics2.ReleaseHdcInternal(handle);
                }
                graphics.ReleaseHdcInternal(hdc);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(
         IntPtr hwnd,               // Window to copy,Handle to the window that will be copied. 
         IntPtr hdcBlt,             // HDC to print into,Handle to the device context. 
         UInt32 nFlags              // Optional flags,Specifies the drawing options. It can be one of the following values. 
         );

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(
         IntPtr hdc,        // handle to DC
         int nWidth,     // width of bitmap, in pixels
         int nHeight     // height of bitmap, in pixels
         );
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(
         IntPtr hwnd
         );
        /// <summary>
        /// 获取窗口图像,就算窗口被挡住也可以获取
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static Bitmap GetWindowBmp(IntPtr hWnd)
        {
            IntPtr hscrdc = GetWindowDC(hWnd);
            //Control control = Control.FromHandle(hWnd);
            Rect rect = new Rect();
            GetWindowRect(hWnd, ref rect);
            IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, rect.Size.Width, rect.Size.Height);
            IntPtr hmemdc = CreateCompatibleDC(hscrdc);
            SelectObject(hmemdc, hbitmap);
            PrintWindow(hWnd, hmemdc, 0);
            Bitmap bmp = Bitmap.FromHbitmap(hbitmap);
            DeleteDC(hscrdc);//删除用过的对象
            DeleteDC(hmemdc);//删除用过的对象
            return bmp;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] POINT pt, int cPoints);
 

 

        /// <summary>
        /// 获取窗口图像,就算窗口被挡住也可以获取
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="leftBorderWidth">因为图像包括窗口边缘的部分，这个属性代表返回的左边缘宽</param>
        /// <param name="titleHeight">因为图像包括窗口边缘的部分，这个属性代表返回的窗口标题栏高度</param>
        /// <returns></returns>
        public static Bitmap GetWindowBmp(IntPtr hWnd,out int leftBorderWidth , out int titleHeight)
        {
            Rect rect = new Rect();
            GetWindowRect(hWnd, ref rect);
            POINT p = new POINT();
            
            MapWindowPoints(hWnd, IntPtr.Zero, p, 1);


            leftBorderWidth = p.x - rect.left;
            titleHeight = p.y - rect.top;
            return GetWindowBmp(hWnd);
        }


        public static Bitmap CaptureWindow(IntPtr handler)
        {
            //创建显示器的DC 
            
            Rect rect = new Rect();
            GetWindowRect(handler, ref rect);
            Bitmap MyImage = new Bitmap(rect.Size.Width, rect.Size.Height);

            Graphics g2 = Graphics.FromImage(MyImage);

            IntPtr dc3 = GetDC(handler);

            IntPtr dc2 = g2.GetHdc();
            
            BitBlt(dc2, 0, 0, rect.Size.Width, rect.Size.Height, dc3, 0, 0, 13369376);

            ReleaseDC(handler , dc3);

            g2.ReleaseHdc(dc2);
            return MyImage;

        }

        /// <summary>
        /// 获取屏幕某点的颜色
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Color GetColorFromScreen(int x, int y)
        {
            //  创建显示器的DC  
            IntPtr hdlDisplay = CreateDC("DISPLAY", null, null, IntPtr.Zero);
            //  从指定设备的句柄创建新的  Graphics  对象  
            Graphics gfxDisplay = Graphics.FromHdc(hdlDisplay);
            //  创建只有一个象素大小的  Bitmap  对象  
            Bitmap bmp = new Bitmap(1, 1, gfxDisplay);
            //  从指定  Image  对象创建新的  Graphics  对象  
            Graphics gfxBmp = Graphics.FromImage(bmp);
            //  获得屏幕的句柄  
            IntPtr hdlScreen = gfxDisplay.GetHdc();
            //  获得位图的句柄  
            IntPtr hdlBmp = gfxBmp.GetHdc();
            //  把当前屏幕中鼠标指针所在位置的一个象素拷贝到位图中  
            BitBlt(hdlBmp, 0, 0, 1, 1, hdlScreen, x, y, 13369376);
            //  释放屏幕句柄  
            gfxDisplay.ReleaseHdc(hdlScreen);
            //  释放位图句柄  
            gfxBmp.ReleaseHdc(hdlBmp);
            Color ccc = bmp.GetPixel(0, 0);  //  获取像素的颜色  
           
            bmp.Dispose();  //  释放  bmp  所使用的资源 
            //gfxBmp.ReleaseHdc(hdlBmp);
            //gfxBmp.ReleaseHdc(hdlScreen);
            //gfxBmp.Dispose();
            return ccc;
        }

        public static Color GetColorFromWindow(IntPtr windowhandler , int x, int y)
        {

            //  从指定设备的句柄创建新的  Graphics  对象  
            Graphics gfxDisplay = Graphics.FromHwnd(windowhandler);
            //  创建只有一个象素大小的  Bitmap  对象  
            Bitmap bmp = new Bitmap(1, 1, gfxDisplay);
            //  从指定  Image  对象创建新的  Graphics  对象  
            Graphics gfxBmp = Graphics.FromImage(bmp);
            //  获得屏幕的句柄  
            IntPtr hdlScreen = gfxDisplay.GetHdc();
            //  获得位图的句柄  
            IntPtr hdlBmp = gfxBmp.GetHdc();
            //  把当前屏幕中鼠标指针所在位置的一个象素拷贝到位图中  
            BitBlt(hdlBmp, 0, 0, 1, 1, hdlScreen, x, y, 13369376);
            //  释放屏幕句柄  
            gfxDisplay.ReleaseHdc(hdlScreen);
            //  释放位图句柄  
            gfxBmp.ReleaseHdc(hdlBmp);
            Color ccc = bmp.GetPixel(0, 0);  //  获取像素的颜色  

            bmp.Dispose();  //  释放  bmp  所使用的资源 
            //gfxBmp.ReleaseHdc(hdlBmp);
            //gfxBmp.ReleaseHdc(hdlScreen);
            //gfxBmp.Dispose();
            return ccc;
        }

		#region CopyWindow
		/// <summary>
		/// 把一个控件或窗体的内容拷贝到bitmap，经常用作屏幕拷贝
		/// </summary>
		/// <param name="control"></param>
		/// <param name="bitmap"></param>
		/// <param name="Xsrc">源控件x坐标</param>
		/// <param name="Ysrc">源控件y坐标</param>
		/// <param name="width">宽度</param>
		/// <param name="height">高度</param>
		public static System.Drawing.Bitmap CopyWindow(System.Windows.Forms.Control control,
										int Xsrc, int Ysrc,
			int width , int height)
		{
			System.Drawing.Bitmap bitmap =new Bitmap(width , height);
			Graphics g2 =Graphics.FromImage(bitmap);
			System.IntPtr hdc2 =g2.GetHdc();

            Graphics g = control.CreateGraphics();
			System.IntPtr hdc =g.GetHdc();
			BitBlt(hdc2, 0, 0, width, height, hdc, Xsrc, Ysrc, 13369376);
			g.Dispose();
			g2.Dispose();
			return bitmap;
		}

        public static System.Drawing.Bitmap CopyWindow(IntPtr hwnd)
        {
            Rect rect = new Rect();
            GetWindowRect(hwnd , ref rect);
            Size size = rect.Size;
            System.Drawing.Bitmap bitmap = new Bitmap(size.Width, size.Height);

            Graphics g2 = Graphics.FromImage(bitmap);
            System.IntPtr hdc2 = g2.GetHdc();

            Graphics g = Graphics.FromHwnd(hwnd);
            System.IntPtr hdc = g.GetHdc();
            BitBlt(hdc2, 0, 0, size.Width, size.Height, hdc, 0, 0, 13369376);
            g.Dispose();
            g2.Dispose();
            return bitmap;
        }

		/// <summary>
		/// 把一个控件或窗体的内容拷贝到bitmap，经常用作屏幕拷贝
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
		public static System.Drawing.Bitmap CopyWindow(System.Windows.Forms.Control control)
		{
			return CopyWindow(control , 0 , 0 , control.ClientRectangle.Width,control.ClientRectangle.Height);

		}
		#endregion

		/// <summary>
		/// 互连网网络是否连接
		/// </summary>
		/// <returns></returns>
		public static bool InternetGetConnectedState()
		{
			int pp = 0;
			return InternetGetConnectedState(out pp, 0);
		}

		/// <summary>
		/// 关闭计算机
		/// </summary>
		public static void ShutDownPC()
		{
			shutdown(EWX_FORCE | EWX_SHUTDOWN);
		}

		/// <summary>
		/// 重启计算机
		/// </summary>
		public static void ReBootPC()
		{
            shutdown(EWX_FORCE | EWX_REBOOT);
		}

		/// <summary>
		/// 注销计算机
		/// </summary>
		public static void LOGOFFPC()
		{
			shutdown(EWX_LOGOFF);
		}


        /// <summary>
        /// </summary>
        /// <param name="flag"></param>
		private static void shutdown(int flag)
		{
			bool ok;
			TokPriv1Luid tp;
			IntPtr hproc = GetCurrentProcess();
			IntPtr htok = IntPtr.Zero;
			ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
			tp.Count = 1;
			tp.Luid = 0;
			tp.Attr = SE_PRIVILEGE_ENABLED;
			ok = LookupPrivilegeValue(null, "SeShutdownPrivilege", ref tp.Luid);
			ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
			ok = ExitWindowsEx(flag, 0);
		}

        private const int TOKEN_READ = 0x00020008;
        public const string SE_DEBUG_NAME = "SeDebugPrivilege";


        public const UInt32 SE_PRIVILEGE_ENABLED_BY_DEFAULT = 0x00000001;

        public const UInt32 SE_PRIVILEGE_REMOVED = 0x00000004;
        public const UInt32 SE_PRIVILEGE_USED_FOR_ACCESS = 0x80000000;
        private const Int32 ANYSIZE_ARRAY = 1;
        public struct TOKEN_PRIVILEGES
        {
            public UInt32 PrivilegeCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = ANYSIZE_ARRAY)]
            public LUID_AND_ATTRIBUTES[] Privileges;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct LUID_AND_ATTRIBUTES
        {
            public LUID Luid;
            public UInt32 Attributes;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool LookupPrivilegeValue(string lpSystemName, string lpName,
            out LUID lpLuid);



        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(int hObject);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
           [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges,
           ref TOKEN_PRIVILEGES NewState,
           UInt32 Zero,
           IntPtr Null1,
           IntPtr Null2);
/// <summary>
/// 提升本进程权限
/// </summary>
        public static void EnablePrivilege(bool enabled)
        {
            bool ok;
            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES();
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES, ref htok);
            tp.PrivilegeCount = 1;
            tp.Privileges = new LUID_AND_ATTRIBUTES[1];
            tp.Privileges[0] = new LUID_AND_ATTRIBUTES();
            ok = LookupPrivilegeValue(null, SE_DEBUG_NAME, out tp.Privileges[0].Luid);
            if (!ok)
            {
                CloseHandle(htok);
                throw (new Exception("LookupPrivilegeValue Error:" + Marshal.GetExceptionForHR(Marshal.GetLastWin32Error()).Message));
            }
            tp.Privileges[0].Attributes =(uint)(enabled? SE_PRIVILEGE_ENABLED:0);
            ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);

            CloseHandle(htok);

        }

		/// <summary>
		/// 拷贝屏幕
		/// </summary>
		/// <returns></returns>
		public static System.Drawing.Bitmap CopyScreen()
		{
			IntPtr dc1 =CreateDC("DISPLAY", null, null, (IntPtr)null); 
//创建显示器的DC 
			Graphics g1 =Graphics.FromHdc(dc1); 

			Bitmap MyImage =new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, g1); 

			Graphics g2 =Graphics.FromImage(MyImage); 

			IntPtr dc3 =g1.GetHdc(); 

			IntPtr dc2 =g2.GetHdc(); 

			BitBlt(dc2, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, dc3, 0, 0, 13369376); 

			g1.ReleaseHdc(dc3); 

			g2.ReleaseHdc(dc2);
			return MyImage;

		}
        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr DeleteDC(IntPtr hDC);


        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool SetStretchBltMode(IntPtr hdc , int mode);
        public const int STRETCH_HALFTONE = 4;

        public static void PrintImageToControl(Bitmap memoryImage, Control control)
        {
            

             IntPtr screenDc = GetDC(IntPtr.Zero);
            IntPtr memDc = CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;


            hBitmap = memoryImage.GetHbitmap(Color.FromArgb(0)); // grab a GDI handle from this GDI+ bitmap
                oldBitmap = SelectObject(memDc, hBitmap);//  select the newbitmap,return the oldbitmap


                IntPtr dc1 = GetDC(control.Handle);
                SetStretchBltMode(dc1, STRETCH_HALFTONE);
            StretchBlt(dc1, 0, 0, control.ClientRectangle.Width, control.ClientRectangle.Height, memDc, 0, 0, memoryImage.Width, memoryImage.Height, 13369376);


          ReleaseDC(IntPtr.Zero, screenDc);


            DeleteObject(hBitmap);
            DeleteDC(memDc);
            DeleteObject(oldBitmap);


        }
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long StretchBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int nSrcWidth, int nSrcHeight, int dwRop);
        
        public Bitmap GetControlImage(Control pnl)
        {
            Graphics mygraphics = pnl.CreateGraphics();
            Size s = pnl.Size;
            Bitmap memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            IntPtr dc1 = mygraphics.GetHdc();
            IntPtr dc2 = memoryGraphics.GetHdc();
            //BitBlt(dc2,0 , 0, pnl.ClientRectangle.Width, pnl.ClientRectangle.Height, dc1, 0, 0, 13369376);
            StretchBlt(dc2, 0, 0, pnl.ClientRectangle.Width, pnl.ClientRectangle.Height, dc1, 0, 0, pnl.ClientRectangle.Width, pnl.ClientRectangle.Height, 13369376);
            //BitBlt(dc2, 0, pnl.ClientRectangle.Height, pnl.ClientRectangle.Width, pnl.ClientRectangle.Height, dc1, 0, 0, 13369376);
            mygraphics.ReleaseHdc(dc1);
            memoryGraphics.ReleaseHdc(dc2);

            return memoryImage;
        }

        /// <summary>
        /// 拷贝屏幕,带鼠标指针
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Bitmap CopyScreenWithCursor()
        {
            Bitmap bit = CopyScreen();
            Graphics g = Graphics.FromImage(bit);
            Point p = Cursor.Current.HotSpot;
            Rectangle rect = new Rectangle(new Point(Form.MousePosition.X - p.X, Form.MousePosition.Y - p.Y), Cursor.Current.Size);
            Cursor.Current.Draw(g, rect);
            return bit;
        }

		/// <summary>
		/// 获取高位Int，用反编译参考System.Windows.Forms.NativeMethods.Util
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static int HIWORD(int n)
		{
			return ((n >> 16) & 65535);

		}

        public static int SignedLOWORD(int n)
        {
            return (short)(n & 0xffff);
        }

        public static int SignedHIWORD(int n)
        {
            return (short)((n >> 0x10) & 0xffff);
        }



		/// <summary>
		/// 获取低位Int，用反编译参考System.Windows.Forms.NativeMethods.Util
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static int LOWORD(int n)
		{
			return (n & 65535);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="low"></param>
		/// <param name="high"></param>
		/// <returns></returns>
		public static int MAKELONG(int low, int high)
		{
			return ((high << 16) | (low & 65535));

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="low"></param>
		/// <param name="high"></param>
		/// <returns></returns>
		public static IntPtr MAKEPARAM(int low, int high)
		{
			return new IntPtr((high << 16) | (low & 65535));

		}

		/// <summary>
		/// 是否按下ctrl键盘
		/// </summary>
		/// <returns></returns>
		public static bool ControlKeyActived()
		{
			byte[] bytes = new byte[256];
			GetKeyboardState(bytes);
			return (bytes[0x11] & 128) == 128;
        }

        /// <summary>
        /// 引起鼠标移动控件
        /// </summary>
        /// <param name="handler">控件句柄</param>
        public static void StartControlMove(IntPtr handler)
        {
            ReleaseCapture();
            SendMessage(handler, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        #region PrintRichTextBox

        /// <summary>
        /// 获取RichTextBox内容
        /// </summary>
        /// <param name="rich"></param>
        /// <returns></returns>
        public static Bitmap PrintRichTextBox(RichTextBox rich)
        {
            return PrintRichTextBox(rich, 0, rich.MaxLength);
        }

        /// <summary>
        /// 获取RichTextBox内容
        /// </summary>
        /// <param name="rich"></param>
        /// <param name="charFrom"></param>
        /// <param name="charTo"></param>
        /// <returns></returns>
        public static Bitmap PrintRichTextBox(RichTextBox rich, int charFrom, int charTo)
        {
            WayControls.Windows.API.CHARRANGE cRange = new WayControls.Windows.API.CHARRANGE();
            cRange.cpMin = charFrom;
            cRange.cpMax = charTo;

            int lpMin = 0;
            int lpMax = 0;

            API.GetScrollRange(rich.Handle, API.SB_VERT, ref lpMin, ref lpMax);

            double HeightAnInch = (double)4032 / (double)254;
            double widthAnInch = (double)4305 / (double)285;

            int height = rich.Height;
            if (lpMax > 0)
            {
                height = lpMax;
            }

            int width = rich.Width;

            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);

            WayControls.Windows.API.Rect rectToPrint = new WayControls.Windows.API.Rect();

            rectToPrint.top = 0;
            rectToPrint.bottom = (int)(bitmap.Height * HeightAnInch);
            rectToPrint.left = 0;
            rectToPrint.right = (int)(bitmap.Width * widthAnInch);


            WayControls.Windows.API.Rect rectPage = rectToPrint;

            IntPtr hdc = g.GetHdc();


            WayControls.Windows.API.FORMATRANGE fmtRange = new WayControls.Windows.API.FORMATRANGE();
            fmtRange.chrg = cRange;
            fmtRange.hdc = hdc;
            fmtRange.hdcTarget = hdc;
            fmtRange.rc = rectToPrint;
            fmtRange.rcPage = rectPage;


            IntPtr wparam = IntPtr.Zero;
            wparam = new IntPtr(1);

            //Move the pointer to the FORMATRANGE structure in memory
            IntPtr lparam = IntPtr.Zero;
            lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
            Marshal.StructureToPtr(fmtRange, lparam, false);

            //Send the rendered data for printing
            int res = WayControls.Windows.API.SendMessage(rich.Handle, WayControls.Windows.API.EM_FORMATRANGE, wparam.ToInt32(), lparam.ToInt32());

            //Free the block of memory allocated
            Marshal.FreeCoTaskMem(lparam);

            //Release the device context handle obtained by a previous call
            g.ReleaseHdc(hdc);

            //Return last + 1 character printer
            return bitmap;
        }

        #endregion


        #endregion


    }
}
