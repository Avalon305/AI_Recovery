using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity
{
    [Table("bdl_user")]
    class User
    {
        //删除状态常量
        public static byte? DELETED = 1;
        public static byte? NO_DELETED = 0;


        //主键 自增
        [Key]
        public int Pk_User_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //宝德龙设备使用人员姓名
        public string User_Name { get; set; }
        //姓名拼音
        public string User_Namepinyin { get; set; }
        //性别  
        public byte? User_Sex { get; set; }
        //生日
        public DateTime? User_Birth { get; set; }
        //小组名称
        public string User_GroupName { get; set; }
        //初期护理程度
        public string User_InitCare { get; set; }
        //当前护理程度
        public string User_Nowcare { get; set; }
        //疾病名称
        public string User_IllnessName { get; set; }
        //残障名称
        public string User_PhysicalDisabilities { get; set; }
        //备忘
        public string User_Memo { get; set; }
        //照片位置
        public string User_PhotoLocation { get; set; }
        //id卡
        public string User_IDCard { get; set; }
        //电话号码
        public string User_Phone { get; set; }
        //是否删除  
        public byte? Is_Deleted { get; set; }


    }
}
