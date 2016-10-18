using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
namespace AppLib.Controls.Editor
{
    abstract class ControlSelector : System.Drawing.Design.UITypeEditor
    {
        private IWindowsFormsEditorService wSrv;
        private ListBox list;
        private IServiceProvider provider;
        private System.ComponentModel.ITypeDescriptorContext context;
        string xmlfilePath;

        public abstract Type ControlType
        {
            get;
        }

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

            this.context.PropertyDescriptor.SetValue(this.context.Instance, list.SelectedItem.ToString());
        }

        #region ShowDrowDown
        private void ShowDrowDown(string value)
        {
            this.wSrv = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            
            try
            {
                if (list == null)
                {
                    list = new ListBox();
                    list.Click += new EventHandler(list_Click);
                }
                else
                    list.Items.Clear();

                foreach (IComponent component in ((System.Web.UI.Control)this.context.Instance).Site.Container.Components)
                {
                    Type ctrltype = component.GetType();
                    if (ctrltype == ControlType || ctrltype.IsSubclassOf(ControlType))
                    {
                        list.Items.Add(((System.Web.UI.Control)component).ID);
                    }
                }

                //System.Web.UI.Page page = null;
                //System.Web.UI.Control ctrl = context.Instance as System.Web.UI.Control;
                //while (!(ctrl is System.Web.UI.Page))
                //{
                //    ctrl = ctrl.Parent;
                //}
                //page = ctrl as System.Web.UI.Page;
                //List<System.Web.UI.Control> controls = AppHelper.GetControlsByTypes(page, new Type[] { ControlType });
                //foreach (System.Web.UI.Control c in controls)
                //{
                //    list.Items.Add(c.ID);
                //}

                list.SelectedItem = value;
                wSrv.DropDownControl(list);
 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        #endregion




    }
}
