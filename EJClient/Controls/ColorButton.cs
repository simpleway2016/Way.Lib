using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EJClient.Controls
{
    public class ColorButton : Button
    {
        #region Text
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {

        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string),
            typeof(ColorButton),
            new FrameworkPropertyMetadata("A", OnTextChanged));
        #endregion

        public delegate void ColorClickHandler(object sender , Color c);
        public event ColorClickHandler ColorClick;
        System.Windows.Shapes.Path m_sel;
        TextBlock m_txt;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            try
            {
                m_sel = (System.Windows.Shapes.Path)Template.FindName("m_sel", this);
                m_txt = (TextBlock)Template.FindName("m_txt", this);

                m_txt.MouseDown += m_sel_MouseDown;
                m_sel.MouseDown += m_sel_MouseDown;
            }
            catch
            {
            }
        }

        public void SetColor(SolidColorBrush c)
        {
            m_txt.Foreground = c;
        }

        void m_sel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            Color c = ((SolidColorBrush)m_txt.Foreground).Color;
            System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();
            cd.Color = System.Drawing.Color.FromArgb( c.R,c.G,c.B );
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                c = Color.FromArgb((byte)255,(byte)cd.Color.R, (byte)cd.Color.G, (byte)cd.Color.B);
                m_txt.Foreground = new SolidColorBrush(c);
                if (ColorClick != null)
                    ColorClick(this, c);
            }
        }

    }
}
