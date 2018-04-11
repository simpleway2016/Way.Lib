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
    /// BugView.xaml 的交互逻辑
    /// </summary>
    public partial class BugView : Window
    {
        public int BugID
        {
            get;
            private set;
        }
        public MyRole MyRole
        {
            get;
            private set;
        }
        public BugView(int bugid,MyRole myrole)
        {
            this.BugID = bugid;
            this.MyRole = myrole;
            InitializeComponent();
            btnFinish.Visibility = System.Windows.Visibility.Collapsed;

            if( myrole == BugCenter.MyRole.Handler )
            word.richText.Document.ContentEnd.InsertTextInRun("修改完毕，再试试看");
            else if (myrole == BugCenter.MyRole.Submitor)
            {
                btnFinish.Visibility = System.Windows.Visibility.Visible;
            }
            else if (myrole == BugCenter.MyRole.Admin)
            {
                mainGrid.RowDefinitions[1].Height = new GridLength(0);
            }
            this.Loaded += BugView_Loaded;
        }

        void BugView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Helper.Client.Invoke<BugHistoryItem[]>("GetBugHistories", (result,error) => {
                //    historiesPanel.Children.Clear();
                //    if (error != null)
                //        Helper.ShowError(error);
                //    else
                //    {
                //        foreach (var data in result)
                //        {
                //            HistoryItem control = new HistoryItem(data, historiesPanel.ActualWidth);

                //            historiesPanel.Children.Add(control);
                //        }

                //    }
                //}, BugID);

                //Helper.Client.Invoke<string>("GetBugPicture", (result, error) => {
                //    if (error != null)
                //        Helper.ShowError(error);
                //    else
                //    {
                //        if(result != null)
                //        bugEditor.Load(Convert.FromBase64String(result));
                //    }
                //}, BugID);
                
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
        }


        private void Finish_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定已经修改完毕吗？" , "" , MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                try
                {
                    Helper.Client.InvokeSync<string>("BugFinish", BugID);
                    
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    Helper.ShowError(ex);
                }
            }
        }

        private void Submit_Click_1(object sender, RoutedEventArgs e)
        {
            byte[] txtContent = null;
            if (  new TextRange(word.richText.Document.ContentStart, word.richText.Document.ContentEnd).Text.Trim().Length > 0)
            {
                txtContent = word.Save();
            }
            
            try
            {
                Helper.Client.InvokeSync<string>("SubmitHistory", BugID, txtContent);
                
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
        }
    }

    public enum MyRole
    {
        Submitor = 0,
        Admin = 1,
        Handler = 2
    }
}
