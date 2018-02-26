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
        /// 内容是否有修改
        /// </summary>
        /// <returns></returns>
        public virtual bool HasChanged()
        {
            return false;
        }

        /// <summary>
        /// 保存内容
        /// </summary>
        /// <returns>成功返回true</returns>
        public virtual bool Save()
        {
            return true;
        }

        /// <summary>
        /// 撤销
        /// </summary>
        public virtual void Undo()
        {

        }
        public virtual void Redo()
        {

        }
        /// <summary>
        /// 剪切
        /// </summary>
        public virtual void Cut()
        {

        }
        /// <summary>
        /// 复制
        /// </summary>
        public virtual void Copy()
        {

        }
        /// <summary>
        /// 粘贴
        /// </summary>
        public virtual void Paste()
        {

        }
        /// <summary>
        /// 全选
        /// </summary>
        public virtual void SelectAll()
        {

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
