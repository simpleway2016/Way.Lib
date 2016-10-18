using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EJClient.Controls
{
    /// <summary>
    /// WayWord.xaml 的交互逻辑
    /// </summary>
    public partial class WayWord : UserControl
    {
        public WayWord()
        {
            InitializeComponent();
            for (int i = 1; i <= 100; i++)
            {
                cmb_lineheight.Items.Add(i);
            }
            cmb_lineheight.SelectedItem = 6;
            for (int i = 12; i <= 200; i++)
            {
                cmb_fontsize.Items.Add(i);
            }
            cmb_fontsize.SelectedItem = 15;
            InitializeFontFamilyList();

            richText.FontFamily = new FontFamily("宋体");
            richText.FontSize = 15;
            this.Loaded += WayWord_Loaded;
        }

        public byte[] Save()
        {
            return System.Text.Encoding.UTF8.GetBytes( System.Windows.Markup.XamlWriter.Save(richText.Document));
        }

        /// <summary>
        /// 设置自定义按钮
        /// </summary>
        /// <param name="buttonTexts"></param>
        /// <param name="clickHandlers"></param>
        public void SetButtons(string[] buttonTexts,RoutedEventHandler[] clickHandlers)
        {
            ToolBar toolbar = new ToolBar();
            for(int i = 0 ; i < buttonTexts.Length ; i ++)
            {
                Button btn = new Button();
                btn.Content = buttonTexts[i];
                btn.Click += clickHandlers[i];
                toolbar.Items.Add(btn);
            }
            toolbarContainer.Children.Add(toolbar);
        }

        void WayWord_Loaded(object sender, RoutedEventArgs e)
        {
            richText.Focus();
        }

        #region font list
        private void InitializeFontFamilyList()
        {
            List<string> names = new List<string>();
            foreach (FontFamily family in Fonts.SystemFontFamilies)
            {
                LanguageSpecificStringDictionary fontDics = family.FamilyNames;
                if (fontDics.ContainsKey(System.Windows.Markup.XmlLanguage.GetLanguage("zh-cn")))
                {
                    string fontName = null;
                    if (fontDics.TryGetValue(System.Windows.Markup.XmlLanguage.GetLanguage("zh-cn"), out fontName))
                    {
                        names.Add(fontName);
                    }
                }
                else
                {
                    string fontName = null;
                    if (fontDics.TryGetValue(System.Windows.Markup.XmlLanguage.GetLanguage("en-us"), out fontName))
                    {
                        names.Add(fontName);
                    }
                }
            }
            names.Sort();
            foreach (String fontname in names)
            {
                ComboBoxItem item = new ComboBoxItem();

                TextBlock textitem = new TextBlock();
                textitem.Text = fontname;
                textitem.FontSize = 16;
                textitem.FontFamily = new FontFamily(fontname);
                item.Content = textitem;
                cmb_fontfamily.Items.Add(item);

                if (fontname == "宋体")
                {
                    cmb_fontfamily.SelectedIndex = cmb_fontfamily.Items.Count - 1;
                }
            }
        }
        #endregion

      
        private void cmb_lineheight_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                richText.Document.LineHeight = Convert.ToDouble( cmb_lineheight.SelectedItem );
            }
            catch
            {
            }
        }

        private void cmb_fontfamily_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (richText_SelectionChanging)
                return;
            try
            {
                richText.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, ((TextBlock)((ComboBoxItem)cmb_fontfamily.SelectedItem).Content).Text);
            }
            catch
            {
            }
        }

        private void cmb_fontsize_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (richText_SelectionChanging)
                return;
            try
            {
               
                richText.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, cmb_fontsize.SelectedItem.ToString());
            }
            catch
            {
            }
        }


        private void ColorButton_ColorClick_1(object sender, Color c)
        {
            try
            {
                richText.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(c));
            }
            catch (Exception ex)
            {
            }
        }

        private void bgColor_ColorClick_1(object sender, Color c)
        {
            try
            {
                richText.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(c));
            }
            catch (Exception ex)
            {
            }
        }

        bool richText_SelectionChanging = false;
        private void richText_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
            richText_SelectionChanging = true;

            var bg = richText.Selection.GetPropertyValue(TextElement.BackgroundProperty) as SolidColorBrush;
            if (bg != null)
            {
                bgColor.SetColor(bg);
            }
            else
            {
                bgColor.SetColor(Brushes.Black);
            }

            var fontcolor = richText.Selection.GetPropertyValue(TextElement.ForegroundProperty) as SolidColorBrush;
            if (fontcolor != null)
            {
                fontColorButton.SetColor(fontcolor);
            }
            else
            {
                fontColorButton.SetColor(Brushes.Black);
            }

            var index = -1;
            var fontfamily = richText.Selection.GetPropertyValue(TextElement.FontFamilyProperty).ToString();
            for (int i = 0; i < cmb_fontfamily.Items.Count; i++)
            {
                ComboBoxItem item = cmb_fontfamily.Items[i] as ComboBoxItem;
                TextBlock t = item.Content as TextBlock;
                if (t.Text == fontfamily)
                {
                    index = i;
                    break;
                }
            }
            cmb_fontfamily.SelectedIndex = index;

            var fontsize = Convert.ToDouble(richText.Selection.GetPropertyValue(TextElement.FontSizeProperty));
            index = -1;
            for (int i = 0; i < cmb_fontsize.Items.Count; i++)
            {
                if (Convert.ToDouble(cmb_fontsize.Items[i]) == fontsize)
                {
                    index = i;
                    break;
                }
            }
            cmb_fontsize.SelectedIndex = index;

            var fontweight = richText.Selection.GetPropertyValue(TextElement.FontWeightProperty).ToString();
            if (fontweight == "Bold")
                btnB.BorderThickness = new Thickness(1);
            else
                btnB.BorderThickness = new Thickness(0);

            var fontstyle = (System.Windows.FontStyle)richText.Selection.GetPropertyValue(TextElement.FontStyleProperty);
            if (fontstyle == System.Windows.FontStyles.Italic)
            {
                btnI.BorderThickness = new Thickness(1);
            }
            else
            {
                btnI.BorderThickness = new Thickness(0);
            }

            var fontunderline = richText.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            if (fontunderline == TextDecorations.Underline)
            {
                btnU.BorderThickness = new Thickness(1);
            }
            else
            {
                btnU.BorderThickness = new Thickness(0);
            }

            richText_SelectionChanging = false;
        }

        private void btnB_Click_1(object sender, RoutedEventArgs e)
        {
            var fontweight = richText.Selection.GetPropertyValue(TextElement.FontWeightProperty).ToString();
            if (fontweight != "Bold")
            {
                btnB.BorderThickness = new Thickness(1);
                richText.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, System.Windows.FontWeights.Bold);
            }
            else
            {
                richText.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, System.Windows.FontWeights.Normal);
                btnB.BorderThickness = new Thickness(0);
            }
        }

        private void btnI_Click_1(object sender, RoutedEventArgs e)
        {
            var fontstyle = (System.Windows.FontStyle)richText.Selection.GetPropertyValue(TextElement.FontStyleProperty);
            if (fontstyle != System.Windows.FontStyles.Italic)
            {
                btnI.BorderThickness = new Thickness(1);
                richText.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, System.Windows.FontStyles.Italic);
            }
            else
            {
                richText.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, System.Windows.FontStyles.Normal);
                btnI.BorderThickness = new Thickness(0);
            }
        }

        private void btnU_Click_1(object sender, RoutedEventArgs e)
        {
            var fontunderline =  richText.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            if (fontunderline != TextDecorations.Underline)
            {
                richText.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                btnU.BorderThickness = new Thickness(1);
            }
            else
            {
                richText.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                btnU.BorderThickness = new Thickness(0);
            }
        }
    }
}
