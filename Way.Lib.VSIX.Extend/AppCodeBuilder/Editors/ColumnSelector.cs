using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Way.Lib.VSIX.Extend.AppCodeBuilder.Editors;

namespace Way.Lib.VSIX.Extend.AppCodeBuilder
{
    internal class ColumnSelector : System.Drawing.Design.UITypeEditor
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

            ShowDrowDown( (string)value    );

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
            try
            {
                if(this.wSrv == null)
                    this.wSrv = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                IDataControl dataControl = this.context.Instance as IDataControl;
                if (dataControl.DBContext == null)
                    throw new Exception("Please select DBContext first!");
                if (dataControl.Table == null)
                    throw new Exception("Please select Table first!");
                if (list == null)
                {
                    list = new ListBox();
                    list.Height = 300;
                    list.Width = 200;
                    list.Click += new EventHandler(list_Click);
                    list.GotFocus += List_GotFocus;
                }

                list.BorderStyle = System.Windows.Forms.BorderStyle.None;

                var type = ((PropertyInfo)dataControl.Table.Value).PropertyType;
                Type dataType;
                if (type.IsGenericType)
                {
                    dataType = type.GenericTypeArguments[0];
                }
                else if (type.IsArray)
                {
                    dataType = type.GetElementType();
                }
                else if (type.HasElementType)
                {
                    dataType = type.GetElementType();
                }
                else
                {
                    return;
                }

                var properties = dataType.GetProperties( BindingFlags.Public | BindingFlags.Instance).Where(m=>m.GetCustomAttribute<Way.EntityDB.WayLinqColumnAttribute>() != null).Select(m=>m.Name).ToArray();

                list.DataSource = properties;

                _tosetValue = value;
                //if (value != null)
                //    list.SelectedItem = properties.FirstOrDefault(m => m.Name == value.Name);
                wSrv.DropDownControl(list);
               
            }
            catch (Exception ex2)
            {
                throw (ex2);
            }

        }
        string _tosetValue;
        private void List_GotFocus(object sender, EventArgs e)
        {
            list.SelectedItem = _tosetValue;
        }


        #endregion




    }
}
