using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Way.EntityDB
{
    // 摘要:
    //     将类与数据库表中的列相关联。
    public class WayDBColumnAttribute:Attribute
    {
        // 摘要:
        //     初始化 System.Data.Linq.Mapping.ColumnAttribute 类的新实例。
        public WayDBColumnAttribute()
        {
        }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Caption { get; set; }
        public string Storage { get; set; }

        //
        // 摘要:
        //     获取或设置一个值，该值指示列是否可包含 null 值。
        //
        // 返回结果:
        //     默认值 = true。
        public bool CanBeNull { get; set; }
        //
        // 摘要:
        //     获取或设置数据库列的类型。
        //
        // 返回结果:
        //     请参见“备注”。
        public string DbType { get; set; }

        //
        // 摘要:
        //     获取或设置一个值，该值指示列是否包含数据库自动生成的值。
        //
        // 返回结果:
        //     默认值 = false。
        public bool IsDbGenerated { get; set; }

        //
        // 摘要:
        //     获取或设置一个值，该值指示该类成员是否表示作为表的整个主键或部分主键的列。
        //
        // 返回结果:
        //     默认值 = false。
        public bool IsPrimaryKey { get; set; }
      
    }
}
