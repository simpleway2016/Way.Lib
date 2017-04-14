using EJClient.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Search
{
    class InterfaceModuleResult:ISearchResult
    {
         SearchContent m_data;
         public InterfaceModuleResult(SearchContent data)
        {
            m_data = data;
        }
        public void Show()
        {
            TreeNode.InterfaceItemNode itemNode = MainWindow.instance.FindInterfaceModule(this.m_data.ID.Value);
            if (itemNode != null)
                itemNode.OnDoubleClick(null, null);
        }

        string _Title;
        public string Title
        {
            get {
                if (_Title == null)
                {
                    try
                    {
                        _Title = "[接口模块]、" + Helper.Client.InvokeSync<string>("GetInterfaceModulePath", this.m_data.ID.Value);
                    }
                    catch (Exception ex)
                    {
                        Helper.ShowError(ex);
                    }
                }
                return _Title;
            }
        }
    }
}
