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

namespace EJClient.UI
{
    /// <summary>
    /// InputBox.xaml 的交互逻辑
    /// </summary>
    public partial class InputBox : Window
    {
        public string Value
        {
            get
            {
                return txtValue.Text;
            }
            set
            {
                txtValue.Text = value;
            }
        }
        public InputBox(string caption, string title)
        {
            InitializeComponent();
            this.Title = title;
            txtTitle.Content = caption;
        }
        bool acceptEmty = false;
        public InputBox(string caption, string title,double textBoxHeight)
        {
            InitializeComponent();
            acceptEmty = true;
            this.Title = title;
            txtValue.Height = textBoxHeight;
            txtValue.TextWrapping = TextWrapping.Wrap;
            txtValue.AcceptsReturn = true;
            txtTitle.Content = caption;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (acceptEmty || this.Value.Length > 0)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtValue.Focus();
        }
        
    }
}
