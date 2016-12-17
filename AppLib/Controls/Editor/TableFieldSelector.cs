using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
namespace AppLib.Controls.Editor
{
    internal class TableFieldSelector : System.Drawing.Design.UITypeEditor
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
                var pinfo = list.SelectedItem as System.Reflection.PropertyInfo;
                if (pinfo != null)
                {
                   this.context.PropertyDescriptor.SetValue(this.context.Instance, pinfo.Name);
                }
            }
            catch
            {
            }
        }

        #region ShowDrowDown
        private void ShowDrowDown(string value)
        {
           
           
            try
            {
                //System.Diagnostics.Debugger.Launch();
                IKeyObject keyobject = (IKeyObject)this.context.Instance;
                if( string.IsNullOrEmpty( keyobject.KeyTableName) )
                    throw (new Exception("先设置KeyTableName"));

                this.wSrv = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                IServiceProvider _iGetService = this.context.Instance as IServiceProvider;
                if (_iGetService == null)
                {
                    _iGetService = provider;
                }

                try
                {
                    Type linqDBType = null;

                    //ITypeDiscoveryService可以发现工程代码里写的所有类型
                    ITypeDiscoveryService typeDisconvery = (ITypeDiscoveryService)_iGetService.GetService(typeof(System.ComponentModel.Design.ITypeDiscoveryService));
                    System.Collections.ICollection alldbTypes = typeDisconvery.GetTypes(typeof(Way.EntityDB.DBContext), true);
                    string stringType = AppHelper.GetDatabaseLinqType(this.context.Instance);
                    foreach (Type t in alldbTypes)
                    {
                        if (t.FullName == stringType)
                        {
                            linqDBType = t;
                            break;
                        }
                    }
                    if (linqDBType == null)
                        throw new Exception("无法找到" + stringType + "对应的类");

                    if (list == null)
                    {
                        list = new ListBox();
                        list.DisplayMember = "Name";
                        list.Size = new System.Drawing.Size(180,260);
                        list.Click += new EventHandler(list_Click);
                    }
                    else
                        list.Items.Clear();

                    list.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    Type tableType = linqDBType.GetProperty(keyobject.KeyTableName).PropertyType.GetGenericArguments()[0];

                    var properties = tableType.GetProperties();

                    foreach (var pinfo in properties)
                    {
                        if (pinfo.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute), true).Length > 0)
                            continue;
                        list.Items.Add(pinfo);
                        if (pinfo.Name == value)
                            list.SelectedIndex = list.Items.Count - 1;
                    }

                    wSrv.DropDownControl(list);
                }
                catch(Exception ex2)
                {
                    throw (ex2);
                }
                finally
                {
                    //AppDomain.Unload(domain);
                }
               
 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        #endregion




    }
}
