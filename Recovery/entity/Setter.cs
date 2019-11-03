using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.entity
{   //设置者实体
    [Table("bdl_set")]
    public class Setter
    {
        //语言常量
        public static int SET_LANGUAGE_CHINA = 1;
        public static int SET_LANGUAGE_ENGLISH = 2;
        //主键
        [Key]
        public int Pk_Set_Id { get; set; }
        //主机唯一标识 MAC地址
        public string Set_Unique_Id { get; set; }
        //机构名称
        public string Set_OrganizationName { get; set; }
        //照片位置
        public string Set_PhotoLocation { get; set; }
        //机构电话
        public string Set_OrganizationPhone { get; set; }
        //设置语言
        public int Set_Language { get; set; }
        //机构分类
        public string Set_OrganizationSort { get; set; }
        //机构位置
        //public string Set_Organizationaddress { get; set; }
		//版本号
		public string Set_Version { get; set; }
        public string Back_Up { get; set; }


    }
}
