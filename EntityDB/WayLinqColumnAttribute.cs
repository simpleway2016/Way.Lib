using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace EntityDB
{
    // 摘要:
    //     将类与数据库表中的列相关联。
    public class WayLinqColumnAttribute:Attribute
    {
        // 摘要:
        //     初始化 System.Data.Linq.Mapping.ColumnAttribute 类的新实例。
        public WayLinqColumnAttribute()
        {
        }
        public string Comment { get; set; }
        public string Caption { get; set; }
        public string Storage { get; set; }
        // 摘要:
        //     获取或设置 System.Data.Linq.Mapping.AutoSync 枚举。
        //
        // 返回结果:
        //     System.Data.Linq.Mapping.AutoSync 值。
        public AutoSync AutoSync { get; set; }
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
        //     获取或设置一个值，该值指示列是否为数据库中的计算列。
        //
        // 返回结果:
        //     默认值为空。
        public string Expression { get; set; }
        //
        // 摘要:
        //     获取或设置一个值，该值指示列是否包含数据库自动生成的值。
        //
        // 返回结果:
        //     默认值 = false。
        public bool IsDbGenerated { get; set; }
        //
        // 摘要:
        //     获取或设置一个值，该值指示列是否包含 LINQ to SQL 继承层次结构的鉴别器值。
        //
        // 返回结果:
        //     默认值 = false。
        public bool IsDiscriminator { get; set; }
        //
        // 摘要:
        //     获取或设置一个值，该值指示该类成员是否表示作为表的整个主键或部分主键的列。
        //
        // 返回结果:
        //     默认值 = false。
        public bool IsPrimaryKey { get; set; }
        //
        // 摘要:
        //     获取或设置一个值，该值指示成员的列类型是否为数据库时间戳或版本号。
        //
        // 返回结果:
        //     默认值为 false。
        public bool IsVersion { get; set; }
        //
        // 摘要:
        //     获取或设置一个值，该值指示 LINQ to SQL 如何进行开放式并发冲突的检测。
        //
        // 返回结果:
        //     除非 System.Data.Linq.Mapping.ColumnAttribute.IsVersion 对某个成员为 true，否则默认值为
        //     System.Data.Linq.Mapping.UpdateCheck.Always。其他值为 System.Data.Linq.Mapping.UpdateCheck.Never
        //     和 System.Data.Linq.Mapping.UpdateCheck.WhenChanged。
        public UpdateCheck UpdateCheck { get; set; }
    }
}
