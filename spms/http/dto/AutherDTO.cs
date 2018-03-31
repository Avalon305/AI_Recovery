using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.http.dto
{
    /// <summary>
    /// 桌面平台管理用户DTO，传输setter与auther
    /// 为了与web匹配没有驼峰
    /// </summary>
    public class AutherDTO
    {
        //登录名
        public string username { get; set; }
        //密码
        public string password { get; set; }
        //mac地址
        public string clientId { get; set; }
        //机构类型
        public string organizationSort { get; set; }
        //机构名称
        public string organizationName { get; set; }
        //机构电话
        public string organizationPhone { get; set; }
        //使用截止日期
        public string offlineTime { get; set; }

        public AutherDTO() {

        }
      
        public AutherDTO(Setter setter, Auther auther,string mac)
        {
            this.username = auther.Auth_UserName;
            this.password = auther.Auth_UserPass;
            this.organizationSort = setter.Set_OrganizationSort;
            this.organizationName = setter.Set_OrganizationName;
            this.organizationPhone = setter.Set_OrganizationPhone;
            this.offlineTime = auther.Auth_OfflineTime.ToString();
            this.clientId = mac;
        }

    }
}
