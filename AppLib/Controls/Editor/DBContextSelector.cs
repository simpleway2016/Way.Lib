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
    internal class DBContextSelector : System.Drawing.Design.UITypeEditor
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

                    IReferenceService referenceService = (IReferenceService)_iGetService.GetService(typeof(System.ComponentModel.Design.IReferenceService));
                    object[] types = referenceService.GetReferences(typeof(EntityDB.DBContext));

                    for (int i = 0; i < types.Length; i++)
                    {
                        list.Items.Add(types[i].ToString());
                    }
                    list.SelectedValue = value;

                    wSrv.DropDownControl(list);
                }
                catch(Exception ex2)
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
}
