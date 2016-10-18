using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLib
{
    /// <summary>
    /// 生成验证码
    /// </summary>
    public class VerifyCode
    {
        public string Code
        {
            get;
            private set;
        }

        public Image Image
        {
            get;
            private set;
        }

        static string[] Numbers = null;
        //static FontFamily[] MyFontFamilies = new InstalledFontCollection().Families; 
        public VerifyCode(int width)
        {
            int height = (int)(width * (36 / 90.0f));
            if (Numbers == null)
            {
                Numbers = new string[26];
                //Numbers = new string[36];
                int index = 0;
                for (int i = 0; i < Numbers.Length; i++)
                {
                    Numbers[i] = (index++).ToString();
                    if (index == 10)
                        index = 0;
                }
                //int index = 0;
                //for (int i = (int)'A'; i <= (int)'Z'; i++)
                //{
                //    Numbers[index] = ((char)i).ToString();
                //    index++;
                //}
            }
            Bitmap bitmap = new Bitmap(width , height ,  System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(new SolidBrush(Color.FromArgb(158, 175, 227)), new Rectangle(0,0,width,height));
            
            Random random = new Random();
            StringBuilder verifycode = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                verifycode.Append( Numbers[random.Next(0 , Numbers.Length - 1)] );
            }

            for (int i = 0; i < width*2; i++)
            {
                int x = random.Next( 0 , width*2);
                int y = random.Next(0, height*2);
                int r = random.Next(2, height);
                g.FillEllipse(new SolidBrush(Color.FromArgb(100, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255))), new Rectangle(x - r, y - r, random.Next(1, r), random.Next(1, r)));
            }

            float top = 0;
            float left = 0;
            //int fontSize = (int)(19*(width/100.0f));
            int fontSize = (int)(30 * (width / 100.0f));
            for (int i = 0; i < verifycode.Length; i++)
            {
                //Brush fontBrush = new SolidBrush(Color.FromArgb(180, random.Next(0, 80), random.Next(0, 80), random.Next(0, 80)));
                //Font font = new Font(MyFontFamilies[random.Next(0, MyFontFamilies.Length - 1)], fontSize , FontStyle.Bold);
                SizeF textsize = g.MeasureString(verifycode[i].ToString(), new Font("arial",fontSize*(17/26.0f)));

                g.TranslateTransform(left + textsize.Width / 2, top + textsize.Height/2);
                g.RotateTransform(random.Next(0, 90));
                g.TranslateTransform(-(left + textsize.Width / 2), -(top + textsize.Height / 2));

                GraphicsPath gpath = new GraphicsPath();
                gpath.AddString(verifycode[i].ToString().Replace("1","l"), new FontFamily("arial"), (int)FontStyle.Bold, fontSize, new Point((int)left,(int) top), new StringFormat());
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawPath(new Pen(Color.FromArgb(200, random.Next(0, 80), random.Next(0, 80), random.Next(0, 80)), 1), gpath);

                //g.DrawString(verifycode[i].ToString(), font, fontBrush, left, top);
                g.ResetTransform();
                
                left += textsize.Width-2;
            }

            g.Dispose();
            this.Code = verifycode.ToString();
            this.Image = bitmap;
        }
    }
}
