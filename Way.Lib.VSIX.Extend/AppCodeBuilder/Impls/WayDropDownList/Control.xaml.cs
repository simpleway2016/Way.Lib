using System;
using System.Collections.Generic;
using System.IO;
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

namespace Way.Lib.VSIX.Extend.AppCodeBuilder.Impls.WayDropDownList
{
    /// <summary>
    /// Control.xaml 的交互逻辑
    /// </summary>
    public partial class Control : UserControl
    {
        WayCheckboxListBuilder _builder;
        
    
        public Control(WayCheckboxListBuilder b)
        {
            _builder = b;
            InitializeComponent();
        }

        private void btnMakeCode_Click(object sender, RoutedEventArgs e)
        {
            //valueMember="id" textMember="Name" datasource="Way.Lib.ScriptRemoting.WinTest.MyDB.Databases"
            try
            {
                string templateFolderPath = BuilderForm.GetService<Services.IApplication>().GetTemplatePath();
                if (System.IO.Directory.Exists(templateFolderPath) == false)
                    throw new Exception("缺乏代码模板文件夹，不能生成代码");

                var encode = System.Text.Encoding.GetEncoding("gb2312");
                templateFolderPath += "//"+_builder.Name+"//";

                if (true)
                {




                    #region html代码
                    if (true)
                    {
                        string htmlTemplate;

                        htmlTemplate = File.ReadAllText($"{templateFolderPath}html.txt", encode);



                        htmlTemplate = htmlTemplate.Replace("{%ControlId}", _builder.ControlId);


                        txtHtml.Text = htmlTemplate.Replace("{%Attributes}", _builder.GetAttributes());
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtHtml.Text);
        }
    }
}
