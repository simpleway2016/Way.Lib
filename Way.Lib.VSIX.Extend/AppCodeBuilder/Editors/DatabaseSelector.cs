using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
namespace Way.Lib.VSIX.Extend.AppCodeBuilder
{
    internal class DatabaseSelector : System.Drawing.Design.UITypeEditor
    {
        private IWindowsFormsEditorService wSrv;
        private ListBox list;
        private IServiceProvider provider;
        private System.ComponentModel.ITypeDescriptorContext context;

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

            ShowDrowDown( value == null ? null : (Type)((ValueDescription)value).Value);

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
                this.context.PropertyDescriptor.SetValue(this.context.Instance,new ValueDescription( list.SelectedItem , ()=>
                {
                    return ((Type)list.SelectedItem).Name;
                }));
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        #region ShowDrowDown
        private void ShowDrowDown(Type value)
        {
            try
            {
                if(this.wSrv == null)
                    this.wSrv = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (list == null)
                {
                    list = new ListBox();
                    list.Height = 300;
                    list.GotFocus += List_GotFocus;
                    list.Click += new EventHandler(list_Click);
                }

                list.BorderStyle = System.Windows.Forms.BorderStyle.None;

                var typeDiscoverer = BuilderForm.GetService<Services.ITypeDiscoverer>();
                var dbtypes = typeDiscoverer.GetTypes(typeof(Way.EntityDB.DBContext));
                if (dbtypes != null)
                {
                    dbtypes = dbtypes.Where(m=>m.GetConstructor(new Type[0])!=null).ToArray();
                    list.DisplayMember = "Name";
                    list.DataSource = dbtypes;
                }


                _tosetValue = value;
                wSrv.DropDownControl(list);
            }
            catch (Exception ex2)
            {
                throw (ex2);
            }

        }
        object _tosetValue;
        private void List_GotFocus(object sender, EventArgs e)
        {
            list.SelectedItem = _tosetValue;
        }
        #endregion




    }
}
