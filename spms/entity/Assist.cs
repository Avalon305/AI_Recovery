﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity
{

    /// <summary>
    /// 用于辅助的三个实体，不需要写service，直接dao
    /// </summary>

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

        public Assist()
        {

        }
        public Assist(string name)
        {
            this.Gr_Name = name;
            this.Gmt_Create = DateTime.Now;
            this.Gmt_Modified = DateTime.Now;
        }

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

        public Disease() {

        }
        public Disease(string name)
        {
            this.Ds_Name = name;
            this.Gmt_Create = DateTime.Now;
            this.Gmt_Modified = DateTime.Now;
        }
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

        public Diagnosis()
        {

        }
        public Diagnosis(string name)
        {
            this.Dn_Name = name;
            this.Gmt_Create = DateTime.Now;
            this.Gmt_Modified = DateTime.Now;
        }
    }
}
