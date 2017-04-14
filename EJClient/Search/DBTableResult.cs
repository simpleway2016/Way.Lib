using EJClient.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Search
{
    class DBTableResult:ISearchResult
    {
        SearchContent m_data;
        public DBTableResult(SearchContent data)
        {
            m_data = data;
        }

        public void Show()
        {
            try
            {
                int moduleid = Helper.Client.InvokeSync<int>("GetDBModuleID", this.m_data.ID.Value);
                if (moduleid != 0)
                {
                    TreeNode.DBModuleNode node = MainWindow.instance.FindDBModule(moduleid);
                    if (node != null)
                    {
                        node.ShowTable(this.m_data.ID.Value);
                    }
                }
                else
                {
                    TreeNode.DBTableNode tableNode = MainWindow.instance.FindDBTable(m_data.ID.Value);
                    if (tableNode != null)
                        tableNode.OnDoubleClick(null, null);
                }
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
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
                        _Title = "[数据表]、" + Helper.Client.InvokeSync<string>("GetDBTablePath", this.m_data.ID.Value);
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
