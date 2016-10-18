
using System.Runtime.InteropServices;
using System;
using System.Drawing.Imaging;
using System.Drawing;

namespace WayControls.Windows.Camera
{
    ///  
    ///  avicap  的摘要说明。
    ///  
    public class ShowVideo
    {
        //  ShowVideo  calls
        [DllImport("avicap32.dll")]
        public static extern IntPtr capCreateCaptureWindowA(byte[] lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);
        [DllImport("avicap32.dll")]
        public static extern bool capGetDriverDescriptionA(short wDriver, byte[] lpszName, int cbName, byte[] lpszVer, int cbVer);
        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, short wParam, int lParam);
        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, short wParam, FrameEventHandler lParam);
        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, ref  BITMAPINFO lParam);
        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, ref  API.BITMAPINFO lParam);
        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, ref  CAPSTATUS lParam);
        [DllImport("User32.dll")]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        [DllImport("avicap32.dll")]
        public static extern int capGetVideoFormat(IntPtr hWnd, IntPtr psVideoFormat, int wSize);

        //  constants
        public const int WM_USER = 0x400;
        public const int WS_CHILD = 0x40000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WM_CAP_START = WM_USER;
        public const int WM_CAP_STOP = WM_CAP_START + 68;
        public const int WM_CAP_DLG_VIDEOSOURCE = WM_CAP_START + 42;
        public const int WM_CAP_DLG_VIDEOFORMAT = WM_CAP_START + 41;
        public const int WM_CAP_DLG_VIDEOCOMPRESSION = WM_CAP_START + 46;
        public const int WM_CAP_GET_STATUS = WM_CAP_START + 54;
        public const int WM_CAP_GET_VIDEOFORMAT = WM_CAP_START + 44;
        public const int WM_CAP_DRIVER_CONNECT = WM_CAP_START + 10;
        public const int WM_CAP_DRIVER_DISCONNECT = WM_CAP_START + 11;
        public const int WM_CAP_SAVEDIB = WM_CAP_START + 25;
        public const int WM_CAP_GRAB_FRAME = WM_CAP_START + 60;
        public const int WM_CAP_SEQUENCE = WM_CAP_START + 62;
        public const int WM_CAP_FILE_SET_CAPTURE_FILEA = WM_CAP_START + 20;
        public const int WM_CAP_SEQUENCE_NOFILE = WM_CAP_START + 63;
        public const int WM_CAP_SET_OVERLAY = WM_CAP_START + 51;
        public const int WM_CAP_SET_PREVIEW = WM_CAP_START + 50;
        public const int WM_CAP_SET_CALLBACK_VIDEOSTREAM = WM_CAP_START + 6;
        public const int WM_CAP_SET_CALLBACK_ERROR = WM_CAP_START + 2;
        public const int WM_CAP_SET_CALLBACK_STATUSA = WM_CAP_START + 3;
        public const int WM_CAP_SET_CALLBACK_FRAME = WM_CAP_START + 5;
        public const int WM_CAP_SET_SCALE = WM_CAP_START + 53;
        public const int WM_CAP_SET_PREVIEWRATE = WM_CAP_START + 52;
        public const int WM_CAP_SET_VIDEOFORMAT = WM_USER + 45;


        //  Structures
        [StructLayout(LayoutKind.Sequential)]
        public struct VIDEOHDR
        {
            [MarshalAs(UnmanagedType.I4)]
            public int lpData;
            [MarshalAs(UnmanagedType.I4)]
            public int dwBufferLength;
            [MarshalAs(UnmanagedType.I4)]
            public int dwBytesUsed;
            [MarshalAs(UnmanagedType.I4)]
            public int dwTimeCaptured;
            [MarshalAs(UnmanagedType.I4)]
            public int dwUser;
            [MarshalAs(UnmanagedType.I4)]
            public int dwFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] dwReserved;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct  CAPSTATUS
        {
    public int uiImageWidth ;                  // Width of the image
    public int uiImageHeight ;                 // Height of the image
    public int fLiveWindow ;                 // Now Previewing video?
    public int fOverlayWindow ;              // Now Overlaying video?
    public int fScale ;                 // Scale image to client?
    public API.POINT ptScroll ;                  // Scroll position
    public int fUsingDefaultPalette;            // Using default driver palette?
    public int fAudioHardware ;             // Audio hardware present?
    public int fCapFileExists ;                 // Does capture file exist?
    public int dwCurrentVideoFrame ;          // # of video frames cap'td
   public int  dwCurrentVideoFramesDropped ;     // # of video frames dropped
    public int dwCurrentWaveSamples ;           // # of wave samples cap'td
  public int   dwCurrentTimeElapsedMS ;      // Elapsed capture duration
  public int   hPalCurrent ;                    // Current palette in use
  public int   fCapturingNow ;                // Capture in progress?
  public int   dwReturn ;                      // Error value after any operation
  public int   wNumVideoAllocated ;           // Actual number of video buffers
  public int   wNumAudioAllocated ;      // Actual number of audio buffers
    }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            [MarshalAs(UnmanagedType.I4)]
            public Int32 biSize;
            [MarshalAs(UnmanagedType.I4)]
            public Int32 biWidth;
            [MarshalAs(UnmanagedType.I4)]
            public Int32 biHeight;
            [MarshalAs(UnmanagedType.I2)]
            public short biPlanes;
            [MarshalAs(UnmanagedType.I2)]
            public short biBitCount;
            [MarshalAs(UnmanagedType.I4)]
            public Int32 biCompression;
            [MarshalAs(UnmanagedType.I4)]
            public Int32 biSizeImage;
            [MarshalAs(UnmanagedType.I4)]
            public Int32 biXPelsPerMeter;
            [MarshalAs(UnmanagedType.I4)]
            public Int32 biYPelsPerMeter;
            [MarshalAs(UnmanagedType.I4)]
            public Int32 biClrUsed;
            [MarshalAs(UnmanagedType.I4)]
            public Int32 biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            [MarshalAs(UnmanagedType.Struct, SizeConst = 40)]
            public BITMAPINFOHEADER bmiHeader;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public Int32[] bmiColors;
        }

        public delegate void FrameEventHandler(IntPtr lwnd, IntPtr lpVHdr);

        //  Public  methods
        public static object GetStructure(IntPtr ptr, ValueType structure)
        {
            return Marshal.PtrToStructure(ptr, structure.GetType());
        }

        public static object GetStructure(int ptr, ValueType structure)
        {
            return GetStructure(new IntPtr(ptr), structure);
        }

        public static void Copy(IntPtr ptr, byte[] data)
        {
            Marshal.Copy(ptr, data, 0, data.Length);
        }

        public static void Copy(int ptr, byte[] data)
        {
            Copy(new IntPtr(ptr), data);
        }

        public static int SizeOf(object structure)
        {
            return Marshal.SizeOf(structure);
        }
    }

    //web  camera  class
    public class WebCamera : IDisposable
    {
        //  Constructur
        public WebCamera(IntPtr handle, int width, int height)
        {
            mControlPtr = handle;
            mWidth = width;
            mHeight = height;
        }

        //  delegate  for  frame  callback
        public delegate void RecievedFrameEventHandler(object sender , byte[] data);
        public event RecievedFrameEventHandler RecievedFrame;

        private IntPtr lwndC;  //  Holds  the  unmanaged  handle  of  the  control
        private IntPtr mControlPtr;  //  Holds  the  managed  pointer  of  the  control
        private int mWidth;
        private int mHeight;

        private ShowVideo.FrameEventHandler mFrameEventHandler;  //  Delegate  instance  for  the  frame  callback  -  must  keep  alive!  gc  should  NOT  collect  it

        //  Close  the  web  camera
        public void CloseWebcam()
        {
            this.capDriverDisconnect(this.lwndC);
        }

        /// <summary>
        /// 抓图
        /// </summary>
        /// <param name="path"></param>
        public void CapImage( string path)
        {
            ShowVideo.BITMAPINFO bitmapinfo = new ShowVideo.BITMAPINFO();
            IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
            ShowVideo.SendMessage(this.lwndC, ShowVideo.WM_CAP_SAVEDIB, 0, hBmp.ToInt32());

        }

        /// <summary>
        /// 录像,保存avi文件的路径
        /// </summary>
        /// <param name="lwnd"></param>
        /// <param name="path"></param>
        public void CapScope(IntPtr lwnd, string path)
        {
            IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
            ShowVideo.SendMessage(lwnd, ShowVideo.WM_CAP_FILE_SET_CAPTURE_FILEA, 0, hBmp.ToInt32());
            ShowVideo.SendMessage(lwnd, ShowVideo.WM_CAP_SEQUENCE, 0, 0);
        }

        /// <summary>
        /// 从视频数据放回一个Bmp
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        //public static Bitmap GetBitmapFromCallBackData(byte[] data, int width, int height)
        //{
        //    Bitmap bit = new Bitmap(width, height, PixelFormat.Format24bppRgb);

        //    BitmapData bitdata = bit.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

        //    unsafe
        //    {

        //        byte* p = (byte*)(void*)bitdata.Scan0;
        //        for (int i = 0; i < bit.Height; i++)
        //        {
        //            for (int k = 0; k < bitdata.Stride; k++)
        //            {
        //                int index = (bit.Height - (i + 1)) * bitdata.Stride + k;

        //                p[0] = data[index];
        //                p++;
        //            }
        //        }

        //    }

        //    bit.UnlockBits(bitdata);

        //    return bit;
        //}

        //  start  the  web  camera
        public void StartWebCam()
        {
            byte[] lpszName = new byte[100];
            byte[] lpszVer = new byte[100];

            ShowVideo.capGetDriverDescriptionA(0, lpszName, 100, lpszVer, 100);
            this.lwndC = ShowVideo.capCreateCaptureWindowA(lpszName, ShowVideo.WS_CHILD | ShowVideo.WS_VISIBLE, 0, 0, mWidth, mHeight, this.mControlPtr, 0);

            if (this.capDriverConnect(this.lwndC, 0))
            {
                //有这句，图像才会和mWidth, mHeight相适应
                ShowVideo.SendMessage(this.lwndC, ShowVideo.WM_CAP_SET_SCALE, 1, 0);
                this.capPreviewRate(this.lwndC, 66);
                this.capPreview(this.lwndC, true);

                //ShowVideo.BITMAPINFO bitmapinfo = new ShowVideo.BITMAPINFO();
                //bitmapinfo.bmiHeader.biSize = ShowVideo.SizeOf(bitmapinfo.bmiHeader);
                //bitmapinfo.bmiHeader.biWidth = 353;
                //bitmapinfo.bmiHeader.biHeight = 288;
                //bitmapinfo.bmiHeader.biPlanes = 1;
                //bitmapinfo.bmiHeader.biBitCount = 24;
                
                //设置格式不一定成功，看设备是否支持
                //this.capSetVideoFormat(this.lwndC, ref  bitmapinfo, ShowVideo.SizeOf(bitmapinfo));
                this.mFrameEventHandler = new ShowVideo.FrameEventHandler(FrameCallBack);
                this.capSetCallbackOnFrame(this.lwndC, this.mFrameEventHandler);
                ShowVideo.SetWindowPos(this.lwndC, 0, 0, 0, mWidth, mHeight, 6);

                
                //ShowVideo.SendMessage(this.lwndC, ShowVideo.WM_CAP_GET_VIDEOFORMAT, ShowVideo.SizeOf(bitmapinfo), ref bitmapinfo);
            }
        }

        /// <summary>
        /// 获取摄像头实际图像大小
        /// </summary>
        /// <returns></returns>
        public Size GetVideoSize()
        {
            ShowVideo.CAPSTATUS status = new ShowVideo.CAPSTATUS();
            ShowVideo.SendMessage(this.lwndC, ShowVideo.WM_CAP_GET_STATUS, ShowVideo.SizeOf(status), ref status);
            return new Size(status.uiImageWidth, status.uiImageHeight);
        }

        public void ReSetWindowSize(int width, int height)
        {
            ShowVideo.SetWindowPos(this.lwndC, 0, 0, 0, width, height, 6);
        }

        /// <summary>
        /// 摄像头设置
        /// </summary>
        public void ShowSelectCameraOption()
        {
            ShowVideo.SendMessage(this.lwndC, ShowVideo.WM_CAP_DLG_VIDEOSOURCE, 0, 0);
        }


        //  private  functions
        private bool capDriverConnect(IntPtr lwnd, short i)
        {
            return ShowVideo.SendMessage(lwnd, ShowVideo.WM_CAP_DRIVER_CONNECT, i, 0);
        }

        private bool capDriverDisconnect(IntPtr lwnd)
        {
            return ShowVideo.SendMessage(lwnd, ShowVideo.WM_CAP_DRIVER_DISCONNECT, 0, 0);
        }

        private bool capPreview(IntPtr lwnd, bool f)
        {
            return ShowVideo.SendMessage(lwnd, ShowVideo.WM_CAP_SET_PREVIEW, f, 0);
        }

        private bool capPreviewRate(IntPtr lwnd, short wMS)
        {
            //
            bool s = ShowVideo.SendMessage(lwnd, ShowVideo.WM_CAP_SET_PREVIEWRATE, wMS, 0);
            //ShowVideo.SendMessage(lwnd, ShowVideo.WM_CAP_SET_OVERLAY, 1, 0);
            return s;
        }

        private bool capSetCallbackOnFrame(IntPtr lwnd, ShowVideo.FrameEventHandler lpProc)
        {
            return ShowVideo.SendMessage(lwnd, ShowVideo.WM_CAP_SET_CALLBACK_FRAME, 0, lpProc);
        }

        private bool capSetVideoFormat(IntPtr hCapWnd, ref  ShowVideo.BITMAPINFO BmpFormat, int CapFormatSize)
        {
            return ShowVideo.SendMessage(hCapWnd, ShowVideo.WM_CAP_SET_VIDEOFORMAT, CapFormatSize, ref  BmpFormat);
        }

        /// <summary>
        /// 显示图像格式设置窗口
        /// </summary>
        public void ShowImageFormatSetting()
        {
            ShowVideo.SendMessage(this.lwndC, ShowVideo.WM_CAP_DLG_VIDEOFORMAT, 0, 0);
        }

        /// <summary>
        /// 显示压缩设置
        /// </summary>
        public void ShowCompressionSetting()
        {
            ShowVideo.SendMessage(this.lwndC, ShowVideo.WM_CAP_DLG_VIDEOCOMPRESSION, 0, 0);
        }

        private void FrameCallBack(IntPtr lwnd, IntPtr lpVHdr)
        {
            if (this.RecievedFrame != null)
            {
                

                ShowVideo.VIDEOHDR videoHeader = new ShowVideo.VIDEOHDR();
                byte[] VideoData;
                videoHeader = (ShowVideo.VIDEOHDR)ShowVideo.GetStructure(lpVHdr, videoHeader);
                VideoData = new byte[videoHeader.dwBytesUsed];
                ShowVideo.Copy(videoHeader.lpData, VideoData);



                IntPtr hdc = API.CreateCompatibleDC(0);
                API.BITMAPINFO bitheder = new API.BITMAPINFO();
                ShowVideo.SendMessage(this.lwndC, ShowVideo.WM_CAP_GET_VIDEOFORMAT, ShowVideo.SizeOf(bitheder), ref bitheder);
                API.BITMAPINFOHEADER hhh = bitheder.bmiHeader;
                API.BITMAPINFO bitinfo = new API.BITMAPINFO();

                IntPtr ht = API.CreateDIBSection(IntPtr.Zero, bitheder, API.DIB_RGB_COLORS , videoHeader.lpData , 0 , 0);


                this.RecievedFrame(this , VideoData);
            }
        }

        #region IDisposable 成员
        private bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                if (lwndC != IntPtr.Zero)
                {
                    this.CloseWebcam();
                    lwndC = IntPtr.Zero;
                }
            }
        }

        #endregion
    }

}
