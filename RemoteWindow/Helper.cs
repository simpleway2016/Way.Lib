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

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }

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
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);
        public static Bitmap GetWindowBitmap(IntPtr hwnd, out RECT rect)
        {
            Graphics gSrc = Graphics.FromHwnd(hwnd);
            HandleRef hDcSrc = new HandleRef(null, gSrc.GetHdc());

            GetClientRect(hwnd, out rect);

            const int SRCCOPY = 0xcc0020;     //复制图块的光栅操作码  

            Bitmap bmSave = new Bitmap(rect.Width, rect.Height);     //用于保存图片的位图对象  
            Graphics gSave = Graphics.FromImage(bmSave);     //创建该位图的Graphics对象  
            HandleRef hDcSave = new HandleRef(null, gSave.GetHdc());     //得到句柄  

            BitBlt(hDcSave, 0, 0, rect.Width, rect.Height, hDcSrc, 0, 0, SRCCOPY);

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
        public unsafe static Bitmap CompareBitmap(Bitmap b1, Bitmap b2)
        {
            if (b1 == null)
                return b2;

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
                    }
                    data += 4;
                    data1 += 3;
                    data2 += 3;
                }

                data += offset;
                data1 += offset2;
                data2 += offset2;
            }

            result.UnlockBits(resultData);
            b1.UnlockBits(b1Data);
            b2.UnlockBits(b2Data);

            if(!hasChanged)
            {
                result.Dispose();
                return null;
            }
            return result;
        }
    }
}
