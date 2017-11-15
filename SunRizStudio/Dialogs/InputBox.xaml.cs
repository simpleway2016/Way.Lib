using SunRizStudio.Models;
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

namespace SunRizStudio.Dialogs
{
    /// <summary>
    /// InputBox.xaml 的交互逻辑
    /// </summary>
    public partial class InputBox : Window
    {
        Model _model = new Model();
        public string Value
        {
            get
            {
                if (this.IsPassword)
                    return txtPwd.Password;
                else
                    return _model.Value;
            }
            set
            {
                if (this.IsPassword)
                { }
                else
                    _model.Value = value;
            }
        }
        class Model : Way.Lib.DataModel
        {
            string _Value;
            public string Value
            {
              
                get
                {

                    return _Value;
                }
                set
                {
                    if (_Value != value)
                    {
                        _Value = value;
                        OnPropertyChanged("Value" , null , value);
                    }
                }
            }
            string _Caption;
            public string Caption
            {
                get
                {
                    return _Caption;
                }
                set
                {
                    if (_Caption != value)
                    {
                        _Caption = value;
                        OnPropertyChanged("Caption",null,value);
                    }
                }
            }
        }

        bool _IsPassword;
        public bool IsPassword
        {
            get
            {
                return _IsPassword;
            }
            set
            {
                if (_IsPassword != value)
                {
                    _IsPassword = value;
                    txtPwd.Visibility = _IsPassword ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                    txtContent.Visibility = _IsPassword ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
                }
            }
        }

        public InputBox()
        {
            InitializeComponent();
        }
        public InputBox(string caption, string title)
        {
            InitializeComponent();
            
            _model.Caption = caption;
            this.Title = title;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.DataContext = _model;
            this._btnOK.Click += (s, e) => {
                this.DialogResult = true;
            };
            this._btnCancel.Click += (s, e) => {
                this.DialogResult = false;
            };

            this._btnOK.IsDefault = true;
            this._btnCancel.IsCancel = true;
            txtPwd.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
