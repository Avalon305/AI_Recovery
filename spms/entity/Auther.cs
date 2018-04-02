using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity
{
    //权限人员实体
    [Table("bdl_auth")]
    public class Auther
    {   //权限等级常量
        /// <summary>
        /// 超级管理员
        /// </summary>
        public static  byte? AUTH_LEVEL_ADMIN = 0;
        /// <summary>
        /// 一般管理员
        /// </summary>
        public static byte? AUTH_LEVEL_MANAGER = 1;



        //用户状态常量，0代表正常、解冻，1代表冻结，2代表完全离线
        /// <summary>
        /// 正常/解冻
        /// </summary>
        public static byte? USER_STATUS_GENERAL = 0;
        /// <summary>
        /// 冻结
        /// </summary>
        public static byte? USER_STATUS_FREEZE = 1;
        /// <summary>
        /// 完全离线
        /// </summary>
        public static byte? USER_STATUS_FREE = 2;
        //截止时间常量，完全离线至N年
        public static DateTime? Auth_OFFLINETIMEFREE = DateTime.MaxValue;
        //主键 自增
        [Key]
        public int Pk_Auth_Id { get; set; }
        //登录用户名
        public string Auth_UserName { get; set; }
        //登录密码
        public string Auth_UserPass { get; set; }
        //权限等级  
        public byte? Auth_Level { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //用户状态
        public byte? User_Status { get; set; }
        //使用截止时间
        public DateTime? Auth_OfflineTime { get; set; }
    }
}
