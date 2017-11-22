using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SunRizStudio.Documents
{
    public class BaseDocument : UserControl
    {
        public virtual string Header
        {
            get
            {
                return "";
            }
        }
        /// <summary>
        /// onclose事件
        /// </summary>
        /// <param name="canceled">是否取消</param>
        public virtual void  OnClose(ref bool canceled)
        {
            
        }
    }
}
