

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace WayControls.Windows.Camera
{
    /// <summary>
    /// 一个控制摄像头的类
    /// </summary>
    public class CameraDriver
    {
        private const int WM_USER = 0x400;
        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;
        private const int WM_CAP_START = WM_USER;
        private const int WM_CAP_STOP = WM_CAP_START + 68;
        private const int WM_CAP_DRIVER_CONNECT = WM_CAP_START + 10;
        private const int WM_CAP_DRIVER_DISCONNECT = WM_CAP_START + 11;
        private const int WM_CAP_SAVEDIB = WM_CAP_START + 25;
        private const int WM_CAP_GRAB_FRAME = WM_CAP_START + 60;
        private const int WM_CAP_SEQUENCE = WM_CAP_START + 62;
        private const int WM_CAP_FILE_SET_CAPTURE_FILEA = WM_CAP_START + 20;
        private const int WM_CAP_SEQUENCE_NOFILE = WM_CAP_START + 63;
        private const int WM_CAP_SET_OVERLAY = WM_CAP_START + 51;
        private const int WM_CAP_SET_PREVIEW = WM_CAP_START + 50;
        private const int WM_CAP_SET_CALLBACK_VIDEOSTREAM = WM_CAP_START + 6;
        private const int WM_CAP_SET_CALLBACK_ERROR = WM_CAP_START + 2;
        private const int WM_CAP_SET_CALLBACK_STATUSA = WM_CAP_START + 3;
        private const int WM_CAP_SET_CALLBACK_FRAME = WM_CAP_START + 5;
        private const int WM_CAP_SET_SCALE = WM_CAP_START + 53;
        private const int WM_CAP_SET_PREVIEWRATE = WM_CAP_START + 52;
        private const int WM_CAP_SET_VIDEOFORMAT = WM_USER + 45;
        private IntPtr hWndC;
        private bool bStat = false;

        private IntPtr mControlPtr;
        private int mWidth;
        private int mHeight;
        private int mLeft;
        private int mTop;

        private delegate void FrameEventHandler(IntPtr lwnd, IntPtr lpVHdr);
        public delegate void RecievedFrameEventHandler(byte[] data);
        public event RecievedFrameEventHandler RecievedFrame;

        /// <summary>
        /// 初始化摄像头
        /// </summary>
        /// <param name="handle">控件的句柄</param>
        /// <param name="left">开始显示的左边距</param>
        /// <param name="top">开始显示的上边距</param>
        /// <param name="width">要显示的宽度</param>
        /// <param name="height">要显示的长度</param>
        public CameraDriver(IntPtr handle, int left, int top, int width, int height)
        {
            mControlPtr = handle;
            mWidth = width;
            mHeight = height;
            mLeft = left;
            mTop = top;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFOHEADER
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
        private struct BITMAPINFO
        {
            [MarshalAs(UnmanagedType.Struct, SizeConst = 40)]
            public BITMAPINFOHEADER bmiHeader;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public Int32[] bmiColors;
        }


        [DllImport("User32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, ref  BITMAPINFO lParam);

        [DllImport("avicap32.dll")]
        private static extern IntPtr capCreateCaptureWindowA(byte[] lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);

        [DllImport("avicap32.dll")]
        private static extern bool capGetDriverDescriptionA(short wDriver, byte[] lpszName, int cbName, byte[] lpszVer, int cbVer);

        [DllImport("avicap32.dll")]
        private static extern int capGetVideoFormat(IntPtr hWnd, IntPtr psVideoFormat, int wSize);
        [DllImport("User32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, long lParam);

        [DllImport("User32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, int wMsg, short wParam, FrameEventHandler lParam);

        private bool capSetCallbackOnFrame(IntPtr lwnd, ShowVideo.FrameEventHandler lpProc)
        {
            return ShowVideo.SendMessage(lwnd, ShowVideo.WM_CAP_SET_CALLBACK_FRAME, 0, lpProc);
        }


        private void FrameCallBack(IntPtr lwnd, IntPtr lpVHdr)
        {
            ShowVideo.VIDEOHDR videoHeader = new ShowVideo.VIDEOHDR();
            byte[] VideoData;
            videoHeader = (ShowVideo.VIDEOHDR)ShowVideo.GetStructure(lpVHdr, videoHeader);
            VideoData = new byte[videoHeader.dwBytesUsed];
            ShowVideo.Copy(videoHeader.lpData, VideoData);
            if (this.RecievedFrame != null)
                this.RecievedFrame(VideoData);
        }

        private static int SizeOf(object structure)
        {
            return Marshal.SizeOf(structure);
        }

        /// <summary>
        /// 开始显示图像
        /// </summary>
        public void Start()
        {
            if (bStat)
                return;

            bStat = true;
            byte[] lpszName = new byte[100];
            byte[] lpszVer = new byte[100];

            capGetDriverDescriptionA(0, lpszName, 100, lpszVer, 100);

            hWndC = capCreateCaptureWindowA(lpszName, WS_CHILD | WS_VISIBLE, mLeft, mTop, mWidth, mHeight, mControlPtr, 0);

            if (hWndC.ToInt32() != 0)
            {
                
                //SendMessage(hWndC, WM_CAP_SET_CALLBACK_VIDEOSTREAM, 0, 0);
                //SendMessage(hWndC, WM_CAP_SET_CALLBACK_ERROR, 0, 0);
                //SendMessage(hWndC, WM_CAP_SET_CALLBACK_STATUSA, 0, 0);
                
                SendMessage(hWndC, WM_CAP_DRIVER_CONNECT, 0, 0);
                SendMessage(hWndC, WM_CAP_SET_SCALE, 1, 0);
                SendMessage(hWndC, WM_CAP_SET_PREVIEWRATE, 66, 0);
                SendMessage(hWndC, WM_CAP_SET_OVERLAY, 1, 0);
                SendMessage(hWndC, WM_CAP_SET_PREVIEW, 1, 0);

                BITMAPINFO bitmapinfo = new BITMAPINFO();
                bitmapinfo.bmiHeader.biSize = SizeOf(bitmapinfo.bmiHeader);
                bitmapinfo.bmiHeader.biWidth = mWidth;
                bitmapinfo.bmiHeader.biHeight = mHeight;
                bitmapinfo.bmiHeader.biPlanes = 1;
                bitmapinfo.bmiHeader.biBitCount = 24;
                this.capSetVideoFormat(hWndC, ref  bitmapinfo, SizeOf(bitmapinfo));


                SendMessage(hWndC, ShowVideo.WM_CAP_SET_CALLBACK_FRAME, 0, new FrameEventHandler(FrameCallBack));
            }

            return;




        }

        /// <summary>
        /// 停止显示
        /// </summary>
        public void Stop()
        {
            SendMessage(hWndC, WM_CAP_DRIVER_DISCONNECT, 0, 0);
            bStat = false;
        }


        private bool capSetVideoFormat(IntPtr hCapWnd, ref  BITMAPINFO BmpFormat, int CapFormatSize)
        {
            return SendMessage(hCapWnd, WM_CAP_SET_VIDEOFORMAT, CapFormatSize, ref  BmpFormat);
        }

        /// <summary>
        /// 抓图
        /// </summary>
        /// <param name="path">要保存bmp文件的路径</param>
        public void GrabImage(string path)
        {

            IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
            SendMessage(hWndC, WM_CAP_SAVEDIB, 0, hBmp.ToInt64());

        }

        /// <summary>
        /// 录像
        /// </summary>
        /// <param name="path">要保存avi文件的路径</param>
        public void Kinescope(string path)
        {
            IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
            SendMessage(hWndC, WM_CAP_FILE_SET_CAPTURE_FILEA, 0, hBmp.ToInt64());
            SendMessage(hWndC, WM_CAP_SEQUENCE, 0, 0);
        }

        /// <summary>
        /// 从视频数据放回一个Bmp
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap GetBitmapFromCallBackData(byte[] data , int width , int height)
        {
            Bitmap bit = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bitdata = bit.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {

                byte* p = (byte*)(void*)bitdata.Scan0;
                for (int i = 0; i < bit.Height; i++)
                {
                    for (int k = 0; k < bitdata.Stride; k++)
                    {
                        int index = (bit.Height - (i + 1)) * bitdata.Stride + k;

                        p[0] = data[index];
                        p++;
                    }
                }

            }

            bit.UnlockBits(bitdata);

            return bit;
        }

        /// <summary>
        /// 停止录像
        /// </summary>
        public void StopKinescope()
        {
            SendMessage(hWndC, WM_CAP_STOP, 0, 0);
        }

    }
}

