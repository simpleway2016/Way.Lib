using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.EntityDB
{
    public class RepeatValueException : Exception
    {
        /// <summary>
        /// 重复列的中文注释
        /// </summary>
        public string[] ColumnCaptions
        {
            get;
            set;
        }
        /// <summary>
        /// 重复列的字段名
        /// </summary>
        public string[] ColumnNames
        {
            get;
            set;
        }
        public RepeatValueException(string[] columnNames, string[] columnCaptions, string message)
            : base(message)
        {
            this.ColumnNames = columnNames;
            this.ColumnCaptions = columnCaptions;
        }
    }
}
