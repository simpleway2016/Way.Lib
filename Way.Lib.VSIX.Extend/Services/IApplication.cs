using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.VSIX.Extend.Services
{
    public interface IApplication
    {
        /// <summary>
        /// 获取代码模板所在文件夹
        /// </summary>
        /// <returns></returns>
        string GetTemplatePath();
    }
}
