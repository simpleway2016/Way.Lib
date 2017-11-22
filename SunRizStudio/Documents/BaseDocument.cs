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
        internal DocTabItem TabItem;
        public event EventHandler TitleChanged;
        string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if(_title !=value)
                {
                    _title = value;
                    if (TitleChanged != null)
                    {
                        TitleChanged(this, null);
                    }
                }
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
