using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
namespace AppLib.Controls.Editor
{
    internal class TableNameSelector : System.Drawing.Design.UITypeEditor
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
                System.Reflection.PropertyInfo table = list.SelectedItem as System.Reflection.PropertyInfo;
                if (this.context.Instance is EntityGridView)
                {
                   
                    if (table != null)
                    {
                        try
                        {
                            Type tableType = table.PropertyType.GetGenericArguments()[0];
                            EntityDB.Attributes.Table tableAtt = tableType.GetCustomAttribute(typeof(EntityDB.Attributes.Table)) as EntityDB.Attributes.Table;
                            if (tableAtt == null)
                                tableAtt = new EntityDB.Attributes.Table("");

                            IComponentChangeService sv = (IComponentChangeService)context.GetService(typeof(IComponentChangeService));
                            sv.OnComponentChanging(this.context.Instance, null);
                            ((EntityGridView)this.context.Instance).IDFieldName = tableAtt.IDField;
                            sv.OnComponentChanged(this.context.Instance, null, null, null);

                            ((EntityGridView)this.context.Instance).MakeColumns(tableType, this.context);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    else
                    {
                        //System.Diagnostics.Debugger.Launch();
                        MethodInfo method = list.SelectedItem as MethodInfo;
                        if (method != null)
                        {
                            Type tableType = method.ReturnType.GetGenericArguments()[0];

                            ((EntityGridView)this.context.Instance).MakeColumns(tableType, this.context);
                            this.context.PropertyDescriptor.SetValue(this.context.Instance, "");
                            return;
                        }
                    }
                }
                else if (this.context.Instance is EntityEditor)
                {
                    Type tableType = table.PropertyType.GetGenericArguments()[0];
                    EntityDB.Attributes.Table tableAtt = tableType.GetCustomAttribute(typeof(EntityDB.Attributes.Table)) as EntityDB.Attributes.Table;
                    if (tableAtt == null)
                        tableAtt = new EntityDB.Attributes.Table("");

                    IComponentChangeService sv = (IComponentChangeService)context.GetService(typeof(IComponentChangeService));
                    sv.OnComponentChanging(this.context.Instance, null);
                    ((EntityEditor)this.context.Instance).IDFieldName = tableAtt.IDField;
                    sv.OnComponentChanged(this.context.Instance, null, null, null);
                    ((EntityEditor)this.context.Instance).MakeColumns(tableType, this.context);
                }

                this.context.PropertyDescriptor.SetValue(this.context.Instance, table.Name);
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
               
                IWebApplication application1 = (IWebApplication)_iGetService.GetService(typeof(IWebApplication));
                System.Configuration.Configuration config = application1.OpenWebConfiguration(true);

                Type linqDBType = null;

                //ITypeDiscoveryService可以发现工程代码里写的所有类型
                ITypeDiscoveryService typeDisconvery = (ITypeDiscoveryService)_iGetService.GetService(typeof(System.ComponentModel.Design.ITypeDiscoveryService));
                System.Collections.ICollection alldbTypes = typeDisconvery.GetTypes(typeof(EntityDB.DBContext), true);
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
                    throw new Exception("请先设置DatabaseConfig属性");
                Type firstParentType = typeof(EntityDB.DBContext);
                Type microsoftType = firstParentType.BaseType;
                try
                {
                   
                    if (list == null)
                    {
                        list = new ListBox();
                        list.Size = new System.Drawing.Size(180 , 260);
                        list.DisplayMember = "Name";
                        list.Click += new EventHandler(list_Click);
                    }
                    else
                        list.Items.Clear();

                    list.BorderStyle = System.Windows.Forms.BorderStyle.None;

                    var properties = linqDBType.GetProperties().OrderBy(m=>m.Name);
                    foreach (var pinfo in properties)
                    {
                        try
                        {
                            if (pinfo.DeclaringType == firstParentType || pinfo.DeclaringType == microsoftType)
                                continue;
                        }
                        catch
                        {
                        }
                        if (pinfo.PropertyType.GetInterface("System.Linq.IQueryable") != null && pinfo.PropertyType.GetGenericArguments().Length > 0)
                        {
                            list.Items.Add(pinfo);
                            if (pinfo.Name == value)
                            {
                                list.SelectedIndex = list.Items.Count - 1;
                            }
                        }
                    }

                 
                   if (this.context.Instance is EntityGridView)
                   {
                       System.Reflection.MethodInfo[] methodInfos = linqDBType.GetMethods( BindingFlags.Public | BindingFlags.Instance ).OrderBy(m=>m.Name).ToArray();
                       foreach (System.Reflection.MethodInfo method in methodInfos)
                       {
                           if (method.ReturnType == null)
                               continue;
                           try
                           {
                               if (method.DeclaringType.BaseType == firstParentType || method.DeclaringType == microsoftType)
                                   continue;
                           }
                           catch
                           {
                           }
                           if (method.ReturnType.GetInterface("System.Linq.IQueryable") != null && method.ReturnType.GetGenericArguments().Length > 0)
                           {
                               list.Items.Add(method);
                               if (method.Name  == value)
                               {
                                   list.SelectedIndex = list.Items.Count - 1;
                               }
                           }
                       }
                   }

                    wSrv.DropDownControl(list);
                }
                catch(Exception ex2)
                {
                    throw (ex2);
                }
                finally
                {
                   // AppDomain.Unload(domain);
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
