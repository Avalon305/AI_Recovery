using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.entity
{
    [Table("bdl_customdata")]
    public class CustomData
    {
        //主键 自增
        [Key]
        public int Pk_CD_Id { get; set; }
        /// <summary>
        /// 自定义姓名
        /// </summary>
        public string CD_CustomName { get; set; }
        /// <summary>
        /// 自定义类型
        /// </summary>
        public byte? CD_Type { get; set; }
        /// <summary>
        /// 是否删除  默认为o  不删除
        /// </summary>
        public byte? Is_Deleted { get; set; }

        /// <summary>
        /// 构造函数默认是使用中的状态
        /// </summary>
        public CustomData()
        {
            this.Is_Deleted = 0;
        }
        /// <summary>
        /// 自定义类型的枚举类
        /// </summary>
        public enum CustomDataEnum : byte
        {

            /// <summary>
            /// 分组编码：与数据库信息严格对应 
            /// </summary>
            Group = 0,
            /// <summary>
            /// 疾病编码：与数据库信息严格对应  
            /// </summary>
            Disease = 1,
            /// <summary>
            /// 残障编码：与数据库信息严格对应 
            /// </summary>
            Diagiosis = 2
        }

    }
}
