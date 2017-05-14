using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Permissions;
using Microsoft.Win32;

namespace EJClient.UI.WindowsControl
{

    public class WebBrowserExtendedNavigatingEventArgs : System.ComponentModel.CancelEventArgs
    {
        private string _Url;
        public string Url
        {
            get { return _Url; }
        }

        public object ppDisp;

        private string _Frame;
        public string Frame
        {
            get { return _Frame; }
        }

        public WebBrowserExtendedNavigatingEventArgs(string url, string frame)
            : base()
        {
            _Url = url;
            _Frame = frame;
        }
    }



    public class ExtendedWebBrowser : System.Windows.Forms.WebBrowser
    {
        System.Windows.Forms.AxHost.ConnectionPointCookie cookie;
        WebBrowserExtendedEvents events;

        [System.Runtime.InteropServices.DllImport("wininet.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
        static bool SetedUseIE8 = false;
        public ExtendedWebBrowser()
        {
            if (SetedUseIE8 == false)
            {
                SetedUseIE8 = true;
                //使用IE8作为web browser内核，它默认使用ie7
                string exename = System.IO.Path.GetFileNameWithoutExtension(System.Windows.Forms.Application.ExecutablePath);

                string FEATURE_BROWSER_EMULATION = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
                string FEATURE_DOCUMENT_COMPATIBLE_MODE = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_DOCUMENT_COMPATIBLE_MODE";

                using (RegistryKey regkey1 = Registry.CurrentUser.CreateSubKey(FEATURE_BROWSER_EMULATION))
                using (RegistryKey regkey2 = Registry.CurrentUser.CreateSubKey(FEATURE_DOCUMENT_COMPATIBLE_MODE))
                {
                    regkey1.SetValue(exename + ".exe", 12000, RegistryValueKind.DWord);//8000 ie8  9000 ie9 10000 ie10
                    //我设置12000，大于本机版本，希望用本机IE最高版本执行

                    //regkey2.SetValue(exename + ".exe", 80000, RegistryValueKind.DWord);

                    regkey1.SetValue(exename + ".vshost.exe", 12000, RegistryValueKind.DWord);
                    //regkey2.SetValue(exename + ".vshost.exe", 80000, RegistryValueKind.DWord);
                    regkey1.Close();
                    regkey2.Close();
                }
            }
        }

        #region
        #region Raises the Quit event when the browser window is about to be destroyed

        /// <summary>
        /// Overridden
        /// </summary>
        /// <param name="m">The <see cref="Message"/> send to this procedure</param>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WindowsMessages.WM_PARENTNOTIFY)
            {
                //int lp = m.LParam.ToInt32();
                int wp = m.WParam.ToInt32();

                int X = wp & 0xFFFF;
                //int Y = (wp >> 16) & 0xFFFF;
                if (X == (int)WindowsMessages.WM_DESTROY)
                    this.OnQuit();
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// A list of all the available window messages
        /// </summary>
        enum WindowsMessages
        {
            WM_ACTIVATE = 0x6,
            WM_ACTIVATEAPP = 0x1C,
            WM_AFXFIRST = 0x360,
            WM_AFXLAST = 0x37F,
            WM_APP = 0x8000,
            WM_ASKCBFORMATNAME = 0x30C,
            WM_CANCELJOURNAL = 0x4B,
            WM_CANCELMODE = 0x1F,
            WM_CAPTURECHANGED = 0x215,
            WM_CHANGECBCHAIN = 0x30D,
            WM_CHAR = 0x102,
            WM_CHARTOITEM = 0x2F,
            WM_CHILDACTIVATE = 0x22,
            WM_CLEAR = 0x303,
            WM_CLOSE = 0x10,
            WM_COMMAND = 0x111,
            WM_COMPACTING = 0x41,
            WM_COMPAREITEM = 0x39,
            WM_CONTEXTMENU = 0x7B,
            WM_COPY = 0x301,
            WM_COPYDATA = 0x4A,
            WM_CREATE = 0x1,
            WM_CTLCOLORBTN = 0x135,
            WM_CTLCOLORDLG = 0x136,
            WM_CTLCOLOREDIT = 0x133,
            WM_CTLCOLORLISTBOX = 0x134,
            WM_CTLCOLORMSGBOX = 0x132,
            WM_CTLCOLORSCROLLBAR = 0x137,
            WM_CTLCOLORSTATIC = 0x138,
            WM_CUT = 0x300,
            WM_DEADCHAR = 0x103,
            WM_DELETEITEM = 0x2D,
            WM_DESTROY = 0x2,
            WM_DESTROYCLIPBOARD = 0x307,
            WM_DEVICECHANGE = 0x219,
            WM_DEVMODECHANGE = 0x1B,
            WM_DISPLAYCHANGE = 0x7E,
            WM_DRAWCLIPBOARD = 0x308,
            WM_DRAWITEM = 0x2B,
            WM_DROPFILES = 0x233,
            WM_ENABLE = 0xA,
            WM_ENDSESSION = 0x16,
            WM_ENTERIDLE = 0x121,
            WM_ENTERMENULOOP = 0x211,
            WM_ENTERSIZEMOVE = 0x231,
            WM_ERASEBKGND = 0x14,
            WM_EXITMENULOOP = 0x212,
            WM_EXITSIZEMOVE = 0x232,
            WM_FONTCHANGE = 0x1D,
            WM_GETDLGCODE = 0x87,
            WM_GETFONT = 0x31,
            WM_GETHOTKEY = 0x33,
            WM_GETICON = 0x7F,
            WM_GETMINMAXINFO = 0x24,
            WM_GETOBJECT = 0x3D,
            WM_GETTEXT = 0xD,
            WM_GETTEXTLENGTH = 0xE,
            WM_HANDHELDFIRST = 0x358,
            WM_HANDHELDLAST = 0x35F,
            WM_HELP = 0x53,
            WM_HOTKEY = 0x312,
            WM_HSCROLL = 0x114,
            WM_HSCROLLCLIPBOARD = 0x30E,
            WM_ICONERASEBKGND = 0x27,
            WM_IME_CHAR = 0x286,
            WM_IME_COMPOSITION = 0x10F,
            WM_IME_COMPOSITIONFULL = 0x284,
            WM_IME_CONTROL = 0x283,
            WM_IME_ENDCOMPOSITION = 0x10E,
            WM_IME_KEYDOWN = 0x290,
            WM_IME_KEYLAST = 0x10F,
            WM_IME_KEYUP = 0x291,
            WM_IME_NOTIFY = 0x282,
            WM_IME_REQUEST = 0x288,
            WM_IME_SELECT = 0x285,
            WM_IME_SETCONTEXT = 0x281,
            WM_IME_STARTCOMPOSITION = 0x10D,
            WM_INITDIALOG = 0x110,
            WM_INITMENU = 0x116,
            WM_INITMENUPOPUP = 0x117,
            WM_INPUTLANGCHANGE = 0x51,
            WM_INPUTLANGCHANGEREQUEST = 0x50,
            WM_KEYDOWN = 0x100,
            WM_KEYFIRST = 0x100,
            WM_KEYLAST = 0x108,
            WM_KEYUP = 0x101,
            WM_KILLFOCUS = 0x8,
            WM_LBUTTONDBLCLK = 0x203,
            WM_LBUTTONDOWN = 0x201,
            WM_LBUTTONUP = 0x202,
            WM_MBUTTONDBLCLK = 0x209,
            WM_MBUTTONDOWN = 0x207,
            WM_MBUTTONUP = 0x208,
            WM_MDIACTIVATE = 0x222,
            WM_MDICASCADE = 0x227,
            WM_MDICREATE = 0x220,
            WM_MDIDESTROY = 0x221,
            WM_MDIGETACTIVE = 0x229,
            WM_MDIICONARRANGE = 0x228,
            WM_MDIMAXIMIZE = 0x225,
            WM_MDINEXT = 0x224,
            WM_MDIREFRESHMENU = 0x234,
            WM_MDIRESTORE = 0x223,
            WM_MDISETMENU = 0x230,
            WM_MDITILE = 0x226,
            WM_MEASUREITEM = 0x2C,
            WM_MENUCHAR = 0x120,
            WM_MENUCOMMAND = 0x126,
            WM_MENUDRAG = 0x123,
            WM_MENUGETOBJECT = 0x124,
            WM_MENURBUTTONUP = 0x122,
            WM_MENUSELECT = 0x11F,
            WM_MOUSEACTIVATE = 0x21,
            WM_MOUSEFIRST = 0x200,
            WM_MOUSEHOVER = 0x2A1,
            WM_MOUSELAST = 0x20A,
            WM_MOUSELEAVE = 0x2A3,
            WM_MOUSEMOVE = 0x200,
            WM_MOUSEWHEEL = 0x20A,
            WM_MOVE = 0x3,
            WM_MOVING = 0x216,
            WM_NCACTIVATE = 0x86,
            WM_NCCALCSIZE = 0x83,
            WM_NCCREATE = 0x81,
            WM_NCDESTROY = 0x82,
            WM_NCHITTEST = 0x84,
            WM_NCLBUTTONDBLCLK = 0xA3,
            WM_NCLBUTTONDOWN = 0xA1,
            WM_NCLBUTTONUP = 0xA2,
            WM_NCMBUTTONDBLCLK = 0xA9,
            WM_NCMBUTTONDOWN = 0xA7,
            WM_NCMBUTTONUP = 0xA8,
            WM_NCMOUSEHOVER = 0x2A0,
            WM_NCMOUSELEAVE = 0x2A2,
            WM_NCMOUSEMOVE = 0xA0,
            WM_NCPAINT = 0x85,
            WM_NCRBUTTONDBLCLK = 0xA6,
            WM_NCRBUTTONDOWN = 0xA4,
            WM_NCRBUTTONUP = 0xA5,
            WM_NEXTDLGCTL = 0x28,
            WM_NEXTMENU = 0x213,
            WM_NOTIFY = 0x4E,
            WM_NOTIFYFORMAT = 0x55,
            WM_NULL = 0x0,
            WM_PAINT = 0xF,
            WM_PAINTCLIPBOARD = 0x309,
            WM_PAINTICON = 0x26,
            WM_PALETTECHANGED = 0x311,
            WM_PALETTEISCHANGING = 0x310,
            WM_PARENTNOTIFY = 0x210,
            WM_PASTE = 0x302,
            WM_PENWINFIRST = 0x380,
            WM_PENWINLAST = 0x38F,
            WM_POWER = 0x48,
            WM_PRINT = 0x317,
            WM_PRINTCLIENT = 0x318,
            WM_QUERYDRAGICON = 0x37,
            WM_QUERYENDSESSION = 0x11,
            WM_QUERYNEWPALETTE = 0x30F,
            WM_QUERYOPEN = 0x13,
            WM_QUEUESYNC = 0x23,
            WM_QUIT = 0x12,
            WM_RBUTTONDBLCLK = 0x206,
            WM_RBUTTONDOWN = 0x204,
            WM_RBUTTONUP = 0x205,
            WM_RENDERALLFORMATS = 0x306,
            WM_RENDERFORMAT = 0x305,
            WM_SETCURSOR = 0x20,
            WM_SETFOCUS = 0x7,
            WM_SETFONT = 0x30,
            WM_SETHOTKEY = 0x32,
            WM_SETICON = 0x80,
            WM_SETREDRAW = 0xB,
            WM_SETTEXT = 0xC,
            WM_SETTINGCHANGE = 0x1A,
            WM_SHOWWINDOW = 0x18,
            WM_SIZE = 0x5,
            WM_SIZECLIPBOARD = 0x30B,
            WM_SIZING = 0x214,
            WM_SPOOLERSTATUS = 0x2A,
            WM_STYLECHANGED = 0x7D,
            WM_STYLECHANGING = 0x7C,
            WM_SYNCPAINT = 0x88,
            WM_SYSCHAR = 0x106,
            WM_SYSCOLORCHANGE = 0x15,
            WM_SYSCOMMAND = 0x112,
            WM_SYSDEADCHAR = 0x107,
            WM_SYSKEYDOWN = 0x104,
            WM_SYSKEYUP = 0x105,
            WM_TCARD = 0x52,
            WM_TIMECHANGE = 0x1E,
            WM_TIMER = 0x113,
            WM_UNDO = 0x304,
            WM_UNINITMENUPOPUP = 0x125,
            WM_USER = 0x400,
            WM_USERCHANGED = 0x54,
            WM_VKEYTOITEM = 0x2E,
            WM_VSCROLL = 0x115,
            WM_VSCROLLCLIPBOARD = 0x30A,
            WM_WINDOWPOSCHANGED = 0x47,
            WM_WINDOWPOSCHANGING = 0x46,
            WM_WININICHANGE = 0x1A
        }

        /// <summary>
        /// Raises the <see cref="Quit"/> event
        /// </summary>
        protected void OnQuit()
        {
            EventHandler h = Quit;
            if (null != h)
                h(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raised when the browser application quits
        /// </summary>
        /// <remarks>
        /// Do not confuse this with DWebBrowserEvents2.Quit... That's something else.
        /// </remarks>
        public event EventHandler Quit;


        #endregion
        #endregion

        //This method will be called to give you a chance to create your own event sink
        protected override void CreateSink()
        {
            //MAKE SURE TO CALL THE BASE or the normal events won't fire
            base.CreateSink();
            events = new WebBrowserExtendedEvents(this);
            cookie = new System.Windows.Forms.AxHost.ConnectionPointCookie(this.ActiveXInstance, events, typeof(DWebBrowserEvents2));

        }

        protected override void DetachSink()
        {
            if (null != cookie)
            {
                cookie.Disconnect();
                cookie = null;
            }
            base.DetachSink();
        }

        //This new event will fire when the page is navigating
        public event EventHandler<WebBrowserExtendedNavigatingEventArgs> BeforeNavigate;
        public event EventHandler<WebBrowserExtendedNavigatingEventArgs> BeforeNewWindow;

        protected void OnBeforeNewWindow(string url, out bool cancel, ref object ppDisp)
        {
            EventHandler<WebBrowserExtendedNavigatingEventArgs> h = BeforeNewWindow;
            WebBrowserExtendedNavigatingEventArgs args = new WebBrowserExtendedNavigatingEventArgs(url, null);
            if (null != h)
            {
                h(this, args);
                ppDisp = args.ppDisp;
            }
            cancel = args.Cancel;
        }
        public object GetActiveXApplication()
        {
            return this.ActiveXInstance.GetType().InvokeMember("Application", System.Reflection.BindingFlags.GetProperty, null, this.ActiveXInstance, null);

        }

        protected void OnBeforeNavigate(string url, string frame, out bool cancel)
        {
            EventHandler<WebBrowserExtendedNavigatingEventArgs> h = BeforeNavigate;
            WebBrowserExtendedNavigatingEventArgs args = new WebBrowserExtendedNavigatingEventArgs(url, frame);
            if (null != h)
            {
                h(this, args);
            }
            //Pass the cancellation chosen back out to the events
            cancel = args.Cancel;
            if (url.ToLower().StartsWith("javascript:"))
            {
                string js = url.Substring("javascript:".Length);
                if (js.Length == 0 || js.ToLower() == "void" || js.ToLower().StartsWith("void("))
                {
                    cancel = true;
                    return;
                }
            }
        }
        //This class will capture events from the WebBrowser
        class WebBrowserExtendedEvents : System.Runtime.InteropServices.StandardOleMarshalObject, DWebBrowserEvents2
        {
            ExtendedWebBrowser _Browser;
            public WebBrowserExtendedEvents(ExtendedWebBrowser browser) { _Browser = browser; }

            //Implement whichever events you wish
            public void BeforeNavigate2(object pDisp, ref object URL, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
            {
                _Browser.OnBeforeNavigate((string)URL, (string)targetFrameName, out cancel);
            }

            public void NewWindow3(ref object pDisp, ref bool cancel, ref object flags, ref object URLContext, ref object URL)
            {

                _Browser.OnBeforeNewWindow((string)URL, out cancel, ref pDisp);
            }



            public void NavigateError(object pDisp, ref object URL, ref object frame, ref object statusCode, ref bool cancel)
            {

            }
        }

        #region DWebBrowserEvents2 完整定义
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIDispatch), Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
        public interface ___DWebBrowserEvents2
        {
            [DispId(0x66)]
            void StatusTextChange([MarshalAs(UnmanagedType.BStr)] string Text);
            [DispId(0x6c)]
            void ProgressChange(int Progress, int ProgressMax);
            [DispId(0x69)]
            void CommandStateChange(int Command, [MarshalAs(UnmanagedType.VariantBool)] bool Enable);
            [DispId(0x6a)]
            void DownloadBegin();
            [DispId(0x68)]
            void DownloadComplete();
            [DispId(0x71)]
            void TitleChange([MarshalAs(UnmanagedType.BStr)] string Text);
            [DispId(0x70)]
            void PropertyChange([MarshalAs(UnmanagedType.BStr)] string szProperty);
            [DispId(250)]
            void BeforeNavigate2([MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object Flags, [In] ref object TargetFrameName, [In] ref object PostData, [In] ref object Headers, [In, Out, MarshalAs(UnmanagedType.VariantBool)] ref bool Cancel);
            [DispId(0xfb)]
            void NewWindow2([In, Out, MarshalAs(UnmanagedType.IDispatch)] ref object ppDisp, [In, Out, MarshalAs(UnmanagedType.VariantBool)] ref bool Cancel);
            [DispId(0xfc)]
            void NavigateComplete2([MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);
            [DispId(0x103)]
            void DocumentComplete([MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);
            [DispId(0xfd)]
            void OnQuit();
            [DispId(0xfe)]
            void OnVisible([MarshalAs(UnmanagedType.VariantBool)] bool Visible);
            [DispId(0xff)]
            void OnToolBar([MarshalAs(UnmanagedType.VariantBool)] bool ToolBar);
            [DispId(0x100)]
            void OnMenuBar([MarshalAs(UnmanagedType.VariantBool)] bool MenuBar);
            [DispId(0x101)]
            void OnStatusBar([MarshalAs(UnmanagedType.VariantBool)] bool StatusBar);
            [DispId(0x102)]
            void OnFullScreen([MarshalAs(UnmanagedType.VariantBool)] bool FullScreen);
            [DispId(260)]
            void OnTheaterMode([MarshalAs(UnmanagedType.VariantBool)] bool TheaterMode);
            [DispId(0x106)]
            void WindowSetResizable([MarshalAs(UnmanagedType.VariantBool)] bool Resizable);
            [DispId(0x108)]
            void WindowSetLeft(int Left);
            [DispId(0x109)]
            void WindowSetTop(int Top);
            [DispId(0x10a)]
            void WindowSetWidth(int Width);
            [DispId(0x10b)]
            void WindowSetHeight(int Height);
            [DispId(0x107)]
            void WindowClosing([MarshalAs(UnmanagedType.VariantBool)] bool IsChildWindow, [In, Out, MarshalAs(UnmanagedType.VariantBool)] ref bool Cancel);
            [DispId(0x10c)]
            void ClientToHostWindow([In, Out] ref int CX, [In, Out] ref int CY);
            [DispId(0x10d)]
            void SetSecureLockIcon(int SecureLockIcon);
            [DispId(270)]
            void FileDownload([In, Out, MarshalAs(UnmanagedType.VariantBool)] ref bool Cancel);
            [DispId(0x10f)]
            void NavigateError([MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object Frame, [In] ref object StatusCode, [In, Out, MarshalAs(UnmanagedType.VariantBool)] ref bool Cancel);
            [DispId(0xe1)]
            void PrintTemplateInstantiation([MarshalAs(UnmanagedType.IDispatch)] object pDisp);
            [DispId(0xe2)]
            void PrintTemplateTeardown([MarshalAs(UnmanagedType.IDispatch)] object pDisp);
            [DispId(0xe3)]
            void UpdatePageStatus([MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object nPage, [In] ref object fDone);
            [DispId(0x110)]
            void PrivacyImpactedStateChange([MarshalAs(UnmanagedType.VariantBool)] bool bImpacted);
            [DispId(0x111)]
            void NewWindow3([In, Out, MarshalAs(UnmanagedType.IDispatch)] ref object ppDisp, [In, Out, MarshalAs(UnmanagedType.VariantBool)] ref bool Cancel, uint dwFlags, [MarshalAs(UnmanagedType.BStr)] string bstrUrlContext, [MarshalAs(UnmanagedType.BStr)] string bstrUrl);
        }
        #endregion

        [System.Runtime.InteropServices.ComImport(), System.Runtime.InteropServices.Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D"),
        System.Runtime.InteropServices.InterfaceTypeAttribute(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIDispatch),
        System.Runtime.InteropServices.TypeLibType(System.Runtime.InteropServices.TypeLibTypeFlags.FHidden)]
        public interface DWebBrowserEvents2
        {

            [System.Runtime.InteropServices.DispId(250)]
            void BeforeNavigate2(
                [System.Runtime.InteropServices.In,
                System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)] object pDisp,
                [System.Runtime.InteropServices.In] ref object URL,
                [System.Runtime.InteropServices.In] ref object flags,
                [System.Runtime.InteropServices.In] ref object targetFrameName, [System.Runtime.InteropServices.In] ref object postData,
                [System.Runtime.InteropServices.In] ref object headers,
                [System.Runtime.InteropServices.In,
                System.Runtime.InteropServices.Out] ref bool cancel);

            [DispId(271)]
            void NavigateError(
                [In, MarshalAs(UnmanagedType.IDispatch)] object pDisp,
                [In] ref object URL, [In] ref object frame,
                [In] ref object statusCode, [In, Out] ref bool cancel);


            [System.Runtime.InteropServices.DispId(273)]
            void NewWindow3(
                [In, Out, MarshalAs(UnmanagedType.IDispatch)] ref  object pDisp,
                [System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out] ref bool cancel,
                [System.Runtime.InteropServices.In] ref object flags,
                [System.Runtime.InteropServices.In] ref object URLContext,
                [System.Runtime.InteropServices.In] ref object URL);


        }
    }


    /// <summary>
    /// Based on WebBrowserEx class from On-Screen HTML project: http://osh.codeplex.com/ - Copyright (c) 2007 Ilia Shramko
    /// </summary>

    public class WebBrowserEx : ExtendedWebBrowser
    {


        #region WebBrowserSiteEx Class
        protected class WebBrowserSiteEx : System.Windows.Forms.WebBrowser.WebBrowserSite, IServiceProvider, IInternetSecurityManager
        {
            private static Guid IID_IInternetSecurityManager =
                Marshal.GenerateGuidForType(typeof(IInternetSecurityManager));

            private WebBrowserEx _webBrowser;

            public WebBrowserSiteEx(WebBrowserEx webBrowser)
                : base(webBrowser)
            {
                _webBrowser = webBrowser;
            }

            #region IServiceProvider Members
            public int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
            {
                if (guidService == IID_IInternetSecurityManager &&
                    riid == IID_IInternetSecurityManager)
                {
                    ppvObject = Marshal.GetComInterfaceForObject(this,
                        typeof(IInternetSecurityManager));
                    return Constants.S_OK;
                }
                ppvObject = IntPtr.Zero;
                return Constants.E_NOINTERFACE;
            }
            #endregion IServiceProvider Members

            #region IInternetSecurityManager Members
            public unsafe int SetSecuritySite(void* pSite)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }

            public unsafe int GetSecuritySite(void** ppSite)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }

            public unsafe int MapUrlToZone(string url, int* pdwZone, int dwFlags)
            {
                *pdwZone = 0;//local -> "Local", "Intranet", "Trusted", "Internet", "Restricted"
                return Constants.S_OK;
            }

            public unsafe int GetSecurityId(string url, byte* pbSecurityId, int* pcbSecurityId, int dwReserved)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }

            public unsafe int ProcessUrlAction(string url, int dwAction, byte* pPolicy, int cbPolicy,
                byte* pContext, int cbContext, int dwFlags, int dwReserved)
            {
                *((int*)pPolicy) = (int)Constants.UrlPolicy.URLPOLICY_ALLOW;
                return Constants.S_OK;
            }

            public unsafe int QueryCustomPolicy(string pwszUrl, void* guidKey, byte** ppPolicy, int* pcbPolicy, byte* pContext, int cbContext, int dwReserved)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }

            public int SetZoneMapping(int dwZone, string lpszPattern, int dwFlags)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }

            public unsafe int GetZoneMappings(int dwZone, void** ppenumString, int dwFlags)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }
            #endregion

        }
        #endregion WebBrowserSiteEx Class

        private WebBrowserSiteEx _site;

        public WebBrowserEx()
        {

        }


        public void RegisterAsBrowser()
        {
            this.ActiveXInstance.GetType().InvokeMember("RegisterAsBrowser", System.Reflection.BindingFlags.SetProperty, null, this.ActiveXInstance, new object[] { true });

        }
        protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
        {
            if (_site == null)
                _site = new WebBrowserSiteEx(this);


            return _site;
        }


    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
    public interface IServiceProvider
    {
        [PreserveSig]
        int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b")]
    public interface IInternetSecurityManager
    {
        [PreserveSig]
        unsafe int SetSecuritySite(void* pSite);
        [PreserveSig]
        unsafe int GetSecuritySite(void** ppSite);
        [PreserveSig]
        unsafe int MapUrlToZone([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, int* pdwZone, [In] int dwFlags);
        [PreserveSig]
        unsafe int GetSecurityId([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, byte* pbSecurityId, int* pcbSecurityId, int dwReserved);
        [PreserveSig]
        unsafe int ProcessUrlAction([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, int dwAction, byte* pPolicy, int cbPolicy, byte* pContext, int cbContext, int dwFlags, int dwReserved);
        [PreserveSig]
        unsafe int QueryCustomPolicy([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, void* guidKey, byte** ppPolicy, int* pcbPolicy, byte* pContext, int cbContext, int dwReserved);
        [PreserveSig]
        int SetZoneMapping(int dwZone, [In, MarshalAs(UnmanagedType.LPWStr)] string lpszPattern, int dwFlags);
        [PreserveSig]
        unsafe int GetZoneMappings(int dwZone, void** ppenumString, int dwFlags);
    }

    public static class Constants
    {
        public const int S_OK = 0;
        public const int E_NOINTERFACE = unchecked((int)0x80004002);
        public const int INET_E_DEFAULT_ACTION = unchecked((int)0x800C0011);
        public enum UrlPolicy
        {
            URLPOLICY_ALLOW = 0,
            URLPOLICY_QUERY = 1,
            URLPOLICY_DISALLOW = 3,
        }
    }
}