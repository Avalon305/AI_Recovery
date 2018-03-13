using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity
{
    //分组实体
    [Table("bdl_group")]
    public class Assist
    {
        //主键 自增
        [Key]
        public int Pk_Gr_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //分组名字
        public string Gr_Name { get; set; }

    }
    //疾病实体
    [Table("bdl_disease")]
    public class Disease
    {
        //主键 自增
        [Key]
        public int Pk_Ds_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //疾病名字
        public string Ds_Name { get; set; }
    }
    //残障名称实体
    [Table("bdl_diagnosis")]
    public class Diagnosis
    {
        //主键 自增
        [Key]
        public int Pk_Dn_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //残疾名字
        public string Dn_Name { get; set; }
    }
}
