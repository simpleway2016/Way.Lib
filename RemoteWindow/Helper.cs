using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RemoteWindow
{
    class Helper
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            [Newtonsoft.Json.JsonIgnore]
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }
            [Newtonsoft.Json.JsonIgnore]
            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }
            [Newtonsoft.Json.JsonIgnore]
            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);

        enum TernaryRasterOperations : uint
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020,
            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086,
            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6,
            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046,
            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328,
            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008,
            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6,
            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA,
            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226,
            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021,
            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09,
            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049,
            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009,
            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042,
            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062,
            /// <summary>
            /// Capture window as seen on screen.  This includes layered windows 
            /// such as WPF windows with AllowsTransparency="true"
            /// </summary>
            CAPTUREBLT = 0x40000000
        }

        [DllImport("gdi32.dll")]
        static extern bool StretchBlt(HandleRef hdcDest, int nXOriginDest, int nYOriginDest,
    int nWidthDest, int nHeightDest,
    HandleRef hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
    TernaryRasterOperations dwRop);
        [DllImport("gdi32.dll")]
        static extern int SetStretchBltMode(HandleRef hdc, StretchBltMode iStretchMode);
        private enum StretchBltMode : int
        {
            STRETCH_ANDSCANS = 1,
            STRETCH_ORSCANS = 2,
            STRETCH_DELETESCANS = 3,
            STRETCH_HALFTONE = 4,
        }
        public static Bitmap GetWindowBitmap(IntPtr hwnd, out RECT rect,int bitW,int bitH,System.Drawing.Rectangle targetRect)
        {
            Graphics gSrc = Graphics.FromHwnd(hwnd);
            HandleRef hDcSrc = new HandleRef(null, gSrc.GetHdc());

            GetClientRect(hwnd, out rect);

           // const int SRCCOPY = 0xcc0020;     //复制图块的光栅操作码  

            Bitmap bmSave = new Bitmap(bitW, bitH);     //用于保存图片的位图对象  
            Graphics gSave = Graphics.FromImage(bmSave);     //创建该位图的Graphics对象  
            HandleRef hDcSave = new HandleRef(null, gSave.GetHdc());     //得到句柄  

            //BitBlt(hDcSave, 0, 0, rect.Width, rect.Height, hDcSrc, 0, 0, SRCCOPY);
            SetStretchBltMode(hDcSave, StretchBltMode.STRETCH_HALFTONE);
            StretchBlt(hDcSave, targetRect.X, targetRect.Y, targetRect.Width, targetRect.Height, hDcSrc, 0, 0, rect.Width, rect.Height, TernaryRasterOperations.SRCCOPY);

            gSrc.ReleaseHdc();
            gSave.ReleaseHdc();

            gSrc.Dispose();
            gSave.Dispose();

            return bmSave;
        }

        public static byte[] GZip(byte[] byteArray)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress);
                //Compress
                sw.Write(byteArray, 0, byteArray.Length);
                //Close, DO NOT FLUSH cause bytes will go missing...
                sw.Close();
                //Transform byte[] zip data to string
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 找出b2图片和b1图片的不同，返回差异部分图片
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public unsafe static Bitmap CompareBitmap(Bitmap b1, Bitmap b2, out RECT rect)
        {
            if (b1 == null)
            {
                rect = new RECT(new Rectangle(Point.Empty , b2.Size));
                return (Bitmap)b2.Clone();
            }

            int height = b1.Size.Height;
            int width = b1.Size.Width;

            bool hasChanged = false;
            Bitmap result = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            var resultData = result.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var b1Data = b1.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var b2Data = b2.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            var data = (byte*)resultData.Scan0.ToPointer();
            var data1 = (byte*)b1Data.Scan0.ToPointer();
            var data2 = (byte*)b2Data.Scan0.ToPointer();

            var offset = resultData.Stride - 4 * width;
            var offset2 = b1Data.Stride - 3 * width;

            int minX = int.MaxValue, maxX = 0, minY = int.MaxValue, maxY = 0;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (data1[0] != data2[0] || data1[1] != data2[1] || data1[2] != data2[2])
                    {
                        hasChanged = true;
                        data[0] = data2[0];//b
                        data[1] = data2[1];//g
                        data[2] = data2[2];//r
                        data[3] = 255;     //a

                        if (j < minX)
                            minX = j;
                        if (j > maxX)
                            maxX = j;

                        if (i < minY)
                            minY = i;
                        if (i > maxY)
                            maxY = i;
                    }
                    data += 4;
                    data1 += 3;
                    data2 += 3;
                }

                data += offset;
                data1 += offset2;
                data2 += offset2;
            }


            b1.UnlockBits(b1Data);
            b2.UnlockBits(b2Data);

            if (!hasChanged)
            {
                result.UnlockBits(resultData);
                result.Dispose();
                rect = new RECT();
                return null;
            }

            int r_width = maxX - minX + 1;
            int r_height = maxY - minY + 1;
            Bitmap minBitmap = new Bitmap(r_width, r_height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var minData = minBitmap.LockBits(new Rectangle(0, 0, r_width, r_height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            data = (byte*)resultData.Scan0.ToPointer();
            var target = (byte*)minData.Scan0.ToPointer();
            for (int i = 0; i < r_height; i++)
            {
                var source = data + (minY + i) * resultData.Stride + minX * 4;
                var t = target + i * minData.Stride;
                for (int j = 0; j < r_width; j++)
                {
                    t[0] = source[0];
                    t[1] = source[1];
                    t[2] = source[2];
                    t[3] = source[3];

                    t += 4;
                    source += 4;
                }
            }

            result.UnlockBits(resultData);
            result.Dispose();
            minBitmap.UnlockBits(minData);
            rect = new RECT(minX, minY, maxX + 1, maxY + 1);

            return minBitmap;
        }
    }
}
