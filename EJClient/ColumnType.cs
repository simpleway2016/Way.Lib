using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient
{
    public class ColumnType
    {
        static List<string> _supportTypes = new List<string>(new string[] {
                                            "varchar",
                                            "int",
                                            "image",
                                            "text",
                                            "smallint",
                                            "smalldatetime",
                                            "real",
                                            "datetime",
                                            "float",
                                            "double",
                                            "bit",
                                            "decimal",
                                            "numeric",
                                            "bigint",
                                            "varbinary",
                                            "char",
                                            "timestamp", });
        /// <summary>
        /// 目前支持的数据库字段类型
        /// </summary>
        public static List<string> SupportTypes
        {
            get
            {
                return _supportTypes;
            }
        }
    }
}
