using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace EJClient.Editor
{
    internal class EnumEditor : System.Drawing.Design.UITypeEditor
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            //			IWebFormsDocumentService docService = (IWebFormsDocumentService)provider.GetService(typeof(IWebFormsDocumentService));
            //			throw(new Exception(docService.DocumentUrl));

            
            if (edSvc == null)
                return null;

            EnumEditorForm form = new EnumEditorForm();
            if(value != null)
            form.richTextBox1.Text = Convert.ToString(value);
            if (edSvc.ShowDialog(form) == DialogResult.OK)
            {
                return form.richTextBox1.Text;
            }

            return value;
        }
    }
}
