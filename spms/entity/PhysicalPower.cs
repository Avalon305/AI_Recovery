using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity
{
    //体力评价实体
    [Table("bdl_physicalpower")]
    class PhysicalPower
    {
        //主键 自增
        [Key]
        public int Pk_PP_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //关联的用户ID
        public int FK_user_Id { get; set; }

        public string pp_high { get; set; }
       
    }
}
