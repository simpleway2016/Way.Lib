using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace SunRizStudio
{
    static class MyExtensions
    {

        public static string ToJsonString(this object obj)
        {
            if (obj == null)
                return null;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static T ToJsonObject<T>(this string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
    }

    public static class WindowExpendMethod
    {
        class FullScreenWindowInfo
        {
            public FullScreenWindowInfo(Window win)
            {
                window = win;
            }
            public Window window;
            public WindowState windowState;
            public WindowStyle windowStyle;
            public bool windowTopMost;
            public ResizeMode windowResizeMode;
            public Rect windowRect;
        }
        private static List<FullScreenWindowInfo> Infos = new List<FullScreenWindowInfo>();

        /// <summary>
        /// 进入全屏    
        /// </summary>
        /// <param name="window"></param>
        public static void GoFullscreen(this Window window)
        {
            //已经是全屏 
            if (window.IsFullscreen()) return;

            var info = new FullScreenWindowInfo(window);
            Infos.Add(info);
           

            //存储窗体信息       
            info.windowState = window.WindowState;
            info.windowStyle = window.WindowStyle;
            info.windowTopMost = window.Topmost;
            info.windowResizeMode = window.ResizeMode;
            info.windowRect.X = window.Left;
            info.windowRect.Y = window.Top;
            info.windowRect.Width = window.Width;
            info.windowRect.Height = window.Height;

            window.WindowState = WindowState.Maximized;
            return;

            /*
            //变成无边窗体 
            window.WindowState = WindowState.Normal;//假如已经是Maximized，就不能进入全屏，所以这里先调整状态 
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.NoResize;
            window.Topmost = true;//最大化后总是在最上面 

            //获取窗口句柄 
            var handle = new WindowInteropHelper(window).Handle;
            //获取当前显示器屏幕
            Screen screen = Screen.FromHandle(handle);

            //调整窗口最大化,全屏的关键代码就是下面3句 
            window.MaxWidth = screen.Bounds.Width;
            window.MaxHeight = screen.Bounds.Height;
            window.WindowState = WindowState.Maximized;

            //解决切换应用程序的问题
            //window.Activated += new EventHandler(window_Activated);
            //window.Deactivated += new EventHandler(window_Deactivated);
            */
        }

        static void window_Deactivated(object sender, EventArgs e)
        {
            var window = sender as Window;
            //window.Topmost = false;
        }
        static void window_Activated(object sender, EventArgs e)
        {
            var window = sender as Window;
            //window.Topmost = true;
        }

        /// <summary>
        /// 退出全屏
        /// </summary>
        /// <param name="window"></param>
        public static void ExitFullscreen(this Window window)
        {
            //已经不是全屏无操作 
            if (!window.IsFullscreen()) return;

            var info = Infos.FirstOrDefault(m => m.window == window);
            Infos.Remove(info);

            //恢复窗口先前信息，这样就退出了全屏 
            //window.Topmost = _windowTopMost;
            window.WindowStyle = info.windowStyle;
            window.ResizeMode = ResizeMode.CanResize;//设置为可调整窗体大小 
            window.Left = info.windowRect.Left;
            window.Width = info.windowRect.Width;
            window.Top = info.windowRect.Top;
            window.Height = info.windowRect.Height;
            window.WindowState = info.windowState;//恢复窗口状态信息 
            window.ResizeMode = info.windowResizeMode;//恢复窗口可调整信息 
            //移除不需要的事件 
            //window.Activated -= window_Activated;
            //window.Deactivated -= window_Deactivated;
        }

        /// <summary>
        /// 窗体是否在全屏状态
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static bool IsFullscreen(this Window window)
        {
            if (window == null)
                throw new ArgumentNullException("window");
            return Infos.Any(m=>m.window == window);
        }
    }
}
