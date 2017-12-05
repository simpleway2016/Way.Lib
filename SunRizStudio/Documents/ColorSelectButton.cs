using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SunRizStudio.Documents
{
    public class ColorSelectButton : Button
    {
        public ColorSelectButton()
        {
            this.Click += ColorSelectButton_Click;
        }

        private void ColorSelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog())
            {
                if (this.Tag != null)
                {
                    cd.Color = System.Drawing.ColorTranslator.FromHtml(this.Tag.ToString());
                }
                if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.Tag = System.Drawing.ColorTranslator.ToHtml(cd.Color);
                }
            }
        }
    }
}
