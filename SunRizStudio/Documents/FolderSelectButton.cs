using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SunRizStudio.Documents
{
    /// <summary>
    /// 文件夹选择按钮
    /// </summary>
    public class FolderSelectButton : Button
    {
        public FolderSelectButton()
        {
            this.Click += FolderSelectButton_Click;
        }

        private void FolderSelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog cd = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.Tag = cd.SelectedPath;
                }
            }
        }
    }
}
