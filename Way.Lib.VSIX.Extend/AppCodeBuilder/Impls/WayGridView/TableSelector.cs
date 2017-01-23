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
    internal class TableSelector : System.Drawing.Design.UITypeEditor
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

            ShowDrowDown( value == null ? null : (PropertyInfo)((ValueDescription)value).Value  );

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
                this.context.PropertyDescriptor.SetValue(this.context.Instance,new ValueDescription(list.SelectedItem, ()=>
                {
                    return ((System.Reflection.PropertyInfo)list.SelectedItem).Name;
                }));
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        #region ShowDrowDown
        private void ShowDrowDown(PropertyInfo value)
        {
            try
            {
                if(this.wSrv == null)
                    this.wSrv = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                WayGridViewBuilder builder = (WayGridViewBuilder)this.context.Instance;
                if (builder.DBContext == null)
                    throw new Exception("Please select DBContext first!");

                if (list == null)
                {
                    list = new ListBox();
                    list.Height = 300;
                    list.Click += new EventHandler(list_Click);
                    list.GotFocus += List_GotFocus;
                }

                list.BorderStyle = System.Windows.Forms.BorderStyle.None;

                var properties = ((Type)builder.DBContext.Value).GetProperties( BindingFlags.Public | BindingFlags.Instance).OrderBy(m=>m.Name).Where(m=>m.PropertyType.IsGenericType || m.PropertyType.IsArray || m.PropertyType.HasElementType).ToArray();

                list.DisplayMember = "Name";
                list.DataSource = properties;

                _tosetValue = ( value == null ? null : properties.FirstOrDefault(m => m.Name == value.Name));
                //if (value != null)
                //    list.SelectedItem = properties.FirstOrDefault(m => m.Name == value.Name);
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
