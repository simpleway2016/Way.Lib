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
using System.Windows.Shapes;

namespace EJClient.Forms.BugCenter
{
    /// <summary>
    /// BugSumbit.xaml 的交互逻辑
    /// </summary>
    public partial class BugSumbit : Window
    {
        BugEditorWindow m_PictureSelector;
        public BugSumbit()
        {
            InitializeComponent();
            word.SetButtons(new string[] { "选择截图..." }, new RoutedEventHandler[] { 选择截图Click });
        }
        protected override void OnClosed(EventArgs e)
        {
            if (m_PictureSelector != null)
            {
                m_PictureSelector.Close();
            }
            base.OnClosed(e);
        }
        void 选择截图Click(object sender, RoutedEventArgs e)
        {
            if (m_PictureSelector == null)
            {
                try
                {
                    m_PictureSelector = new BugEditorWindow();
                   
                }
                catch (Exception ex)
                {
                    Helper.ShowError(ex);
                    return;
                }
                m_PictureSelector.WindowState = System.Windows.WindowState.Maximized;
            }
            m_PictureSelector.Show();
        }
        void submitClick(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text.Trim().Length == 0)
            {
                Helper.ShowMessage(this, "请选择输入标题");
                return;
            }
            if (m_PictureSelector == null && new TextRange(word.richText.Document.ContentStart, word.richText.Document.ContentEnd).Text.Trim().Length == 0)
            {
                Helper.ShowMessage(this ,"请选择截图或者输入文字内容");
                return;
            }
            try
            {
                byte[] textContent = word.Save();
                byte[] picContents = null;
                if (m_PictureSelector != null)
                {
                    picContents = m_PictureSelector.Editor.GetContent();
                }
                this.Cursor = Cursors.Wait;
                Helper.Client.InvokeSync<string>("SubmitBug", txtTitle.Text, textContent, picContents);
                Helper.ShowMessage(this,"成功提交");
                this.Close();
            }
            catch (Exception ex)
            {
                Helper.ShowError(this, ex);
            }
            finally
            {
                this.Cursor = null;
            }
        }
    }
}
