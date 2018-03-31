using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.http.dto
{
    public class UserDTO
    {   //主机标识
        public string clientId;
        //下面的对应entity
        public string pkUserId;
        public string gmtCreate;
        public string gmtModified;
        public string userName;
        public string userNamepinyin;
        public string userSex;
        public string userBirth;
        public string userGroupname;
        public string userInitcare;
        public string userNowcare;
        public string userIllnessname;
        public string userPhysicaldisabilities;
        public string userPhotolocation;
        public string userIdcard;
        public string userPhone;
        public string isDeleted;
        public string userMemo;
        public UserDTO() {

        }
        
        public UserDTO(User user, string mac)
        {
            //设置mac地址
            this.clientId = mac;
            //实体映射
            this.gmtCreate = user.Gmt_Create.ToString().Replace("/", "-");
            this.gmtModified = user.Gmt_Modified.ToString().Replace("/", "-");
            this.isDeleted = user.Is_Deleted.ToString();
            this.pkUserId = user.Pk_User_Id.ToString();
            this.userBirth = user.User_Birth.ToString().Replace("/", "-");
            this.userGroupname = user.User_GroupName;
            this.userIdcard = user.User_IDCard;
            this.userIllnessname = user.User_IllnessName;
            this.userInitcare = user.User_InitCare;
            this.userMemo = user.User_Memo;
            this.userName = user.User_Name;
            this.userNamepinyin = user.User_Namepinyin;
            this.userNowcare = user.User_Nowcare;
            this.userPhone = user.User_Phone;
            this.userPhotolocation = user.User_PhotoLocation;
            //残障名称
            this.userPhysicaldisabilities = user.User_PhysicalDisabilities;
            this.userSex = user.User_Sex.ToString();
        }
    }
}
