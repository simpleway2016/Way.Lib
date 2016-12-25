using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Way.Lib.VSIX
{
    public partial class Form1 : Form
    {
        internal class DatabaseSelector : System.Drawing.Design.UITypeEditor
        {
            private IWindowsFormsEditorService wSrv;
            private ListBox list;
            private IServiceProvider provider;
            private System.ComponentModel.ITypeDescriptorContext context;
            string xmlfilePath;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <param name="provider"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
            {

                this.provider = provider;
                this.context = context;

                ShowDrowDown(value == null ? "" : value.ToString());

                return value;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
            {
                return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
            }

            private void list_Click(object sender, EventArgs e)
            {
                this.wSrv.CloseDropDown();
                try
                {
                    this.context.PropertyDescriptor.SetValue(this.context.Instance, list.SelectedItem);
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }

            #region ShowDrowDown
            private void ShowDrowDown(string value)
            {
                //System.Diagnostics.Debugger.Launch();
                this.wSrv = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                IServiceProvider _iGetService = this.context.Instance as IServiceProvider;

                if (_iGetService == null)
                {
                    _iGetService = provider;
                }

                try
                {
                    try
                    {


                        if (list == null)
                        {
                            list = new ListBox();
                            list.Click += new EventHandler(list_Click);
                        }
                        else
                            list.Items.Clear();

                        list.BorderStyle = System.Windows.Forms.BorderStyle.None;

                        //ITypeDiscoveryService可以发现工程代码里写的所有类型
                        ITypeDiscoveryService typeDisconvery = (ITypeDiscoveryService)_iGetService.GetService(typeof(System.ComponentModel.Design.ITypeDiscoveryService));
                        System.Collections.ICollection alldbTypes = typeDisconvery.GetTypes(typeof(Form), true);
                       
                        foreach (Type t in alldbTypes)
                        {
                            if (t.BaseType.FullName == "EntityDB.DBContext")
                                continue;
                            list.Items.Add(t.FullName);
                        }

                        list.SelectedValue = value;

                        wSrv.DropDownControl(list);
                    }
                    catch (Exception ex2)
                    {
                        throw (ex2);
                    }


                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        MessageBox.Show(ex.InnerException.ToString());
                    }
                    else
                        MessageBox.Show(ex.ToString());
                }

            }
            #endregion




        }
        class Test
        {
            [System.ComponentModel.Category("系统设定"), System.ComponentModel.Editor(typeof(DatabaseSelector), typeof(System.Drawing.Design.UITypeEditor))]
            public string DatabaseConfig
            {
                get;
                set;
            }
        }
    
        public Form1()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = new Test(); 
        }
    }
}
