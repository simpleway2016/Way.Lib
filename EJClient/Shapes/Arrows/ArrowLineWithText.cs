using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFArrows.Arrows
{
    /// <summary>
    /// 带文本和箭头的两点之间连线
    /// </summary>
    public class ArrowLineWithText : ArrowLine
    {
        #region Properties

        /// <summary>
        /// 文本的依赖属性
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(ArrowLineWithText),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// 文本对齐的依赖属性
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
            "TextAlignment", typeof(TextAlignment), typeof(ArrowLineWithText),
            new FrameworkPropertyMetadata(TextAlignment.Left, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 文本对齐方式
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// 文本朝上的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsTextUpProperty = DependencyProperty.Register(
            "IsTextUp", typeof(bool), typeof(ArrowLineWithText),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 文本是否朝上
        /// </summary>
        public bool IsTextUp
        {
            get { return (bool) GetValue(IsTextUpProperty); }
            set { SetValue(IsTextUpProperty, value); }
        }

        /// <summary>
        /// 是否显示文本的依赖属性
        /// </summary>
        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
            "ShowText", typeof (bool), typeof (ArrowLineWithText), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 是否显示文本
        /// </summary>
        public bool ShowText
        {
            get { return (bool) GetValue(ShowTextProperty); }
            set { SetValue(ShowTextProperty, value); }
        }

        #endregion Properties

        #region Overrides

        /// <summary>
        /// 重载渲染事件
        /// </summary>
        /// <param name="drawingContext">绘图上下文</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (ShowText&&(Text != null))
            {
                


                Point _startpoint = this.StartPoint;
                Point _endpoint = this.EndPoint;
            __doagain:
                var txt = Text.Trim();
                var startPoint = _startpoint;
                if (!string.IsNullOrEmpty(txt))
                {
                    var vec = _endpoint - _startpoint;
                    var angle = GetAngle(_startpoint, _endpoint);
                    if (angle > 128 || angle < -128)
                    {
                        Point p = _startpoint;
                        _startpoint = _endpoint;
                        _endpoint = p;
                        goto __doagain;
                    }
                    //使用旋转变换,使其与线平等
                    var transform = new RotateTransform(angle) { CenterX = _startpoint.X, CenterY = _startpoint.Y };
                    drawingContext.PushTransform(transform);

                    var defaultTypeface = new Typeface(SystemFonts.StatusFontFamily, SystemFonts.StatusFontStyle,
                        SystemFonts.StatusFontWeight, new FontStretch());
                    var formattedText = new FormattedText(txt, CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        defaultTypeface, SystemFonts.StatusFontSize, Brushes.Black)
                    {
                        //文本最大宽度为线的宽度
                        MaxTextWidth = vec.Length,
                        //设置文本对齐方式
                        TextAlignment = TextAlignment
                    };

                    var offsetY = StrokeThickness;
                    if (IsTextUp)
                    {
                        //计算文本的行数
                        double textLineCount = formattedText.Width/formattedText.MaxTextWidth;
                        if (textLineCount < 1)
                        {
                            //怎么也得有一行
                            textLineCount = 1;
                        }
                        //计算朝上的偏移
                        offsetY = -formattedText.Height*textLineCount -StrokeThickness;
                    }
                    startPoint = startPoint +new Vector(0,offsetY);

                    //System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)formattedText.Width, (int)formattedText.Height);
                    //System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

                    //graphics.DrawString(Text, new System.Drawing.Font("宋体", 10), System.Drawing.Brushes.Black, new System.Drawing.PointF(0, 0));
                    //graphics.Dispose();

                    //ImageSource imageSource = ToBitmapSource(bitmap);

                    //bitmap.Dispose();

                    //drawingContext.DrawImage(imageSource, new Rect(startPoint.X, startPoint.Y, formattedText.Width,formattedText.Height));
                    drawingContext.DrawText(formattedText, startPoint);
                    drawingContext.Pop();
                }
            }
        }
        public static BitmapImage ToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage bit = new BitmapImage();
            bit.BeginInit();
            bit.StreamSource = ms;
            bit.EndInit();

            return bit;
        }
        #endregion Overrides

        #region Private Methods

        /// <summary>
        /// 获取两个点的倾角
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <returns>两个点的倾角</returns>
        private double GetAngle(Point start, Point end)
        {
            var vec = end - start;
            //X轴
            var xAxis = new Vector(1, 0);
            return Vector.AngleBetween(xAxis, vec); 
        }

        #endregion Private Methods

    }
}