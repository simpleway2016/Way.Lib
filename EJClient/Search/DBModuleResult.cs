using EJClient.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Search
{
    class DBModuleResult:ISearchResult
    {
        SearchContent m_data;
        public DBModuleResult(SearchContent data)
        {
            m_data = data;
        }
        public void Show()
        {
            TreeNode.DBModuleNode node = MainWindow.instance.FindDBModule(m_data.ID.Value);
            if (node != null)
            {
                node.OnDoubleClick(null, null);
            }
        }

        string _Title;
        public string Title
        {
            get {
                if (_Title == null)
                {
                    try
                    {
                        _Title = "[数据模块]、" + Helper.Client.InvokeSync<string>("GetDBModulePath", this.m_data.ID.Value);
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
