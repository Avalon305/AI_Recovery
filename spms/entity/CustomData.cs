using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity
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
        /// 
        /// </summary>
        public byte？ Is_Deleted { get; set; }

}
}
