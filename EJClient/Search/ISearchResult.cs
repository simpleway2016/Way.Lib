using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Search
{
    /// <summary>
    /// 
    /// </summary>
    interface ISearchResult
    {
        /// <summary>
        /// 定位到此搜索项
        /// </summary>
        void Show();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string Title
        {
            get;
        }
    }
}
