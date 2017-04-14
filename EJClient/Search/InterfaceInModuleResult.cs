using EJClient.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Search
{
    class InterfaceInModuleResult : ISearchResult
    {
        SearchContent m_data;
        public InterfaceInModuleResult(SearchContent data)
        {
            m_data = data;
        }
        public void Show()
        {
            int moduleid = Helper.Client.InvokeSync<int>("GetInterfaceModuleID", m_data.ID.GetValueOrDefault());
            if (moduleid != 0)
            {

                TreeNode.InterfaceItemNode itemNode = MainWindow.instance.FindInterfaceModule(moduleid);
                if (itemNode != null)
                {
                    itemNode.ShowItem(m_data.ID.GetValueOrDefault());
                }
            }
        }
        string _Title;
        public string Title
        {
            get
            {
                if (_Title == null)
                {
                    try
                    {
                        _Title = "[接口说明]、" + Helper.Client.InvokeSync<string>("GetInterfaceInModulePath", this.m_data.ID.Value);
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
