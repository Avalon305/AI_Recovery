using Recovery.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Recovery.http.dto
{
    /// <summary>
    /// 用于对Setter的对象上传到云平台数据的DTO，本实体成员都是与Setter实体类成员相对应
    /// </summary>
    public class SetterDTO
    {
        //clientid
        public string clientId;
        //上传数据id
        public string pkSetId;
        //主机唯一标识 MAC地址
        public string setUniqueId;
        //设置语言
        public string setLanguage;
        //机构名称
        public string setOrganizationName;
        //机构分类
        public string setOrganizationSort;
        //照片位置
        public string setPhotoLocation;
        //机构电话
        public string setOrganizationPhone;
        //版本号
        public string setVersion;
        public string backUp;
        public SetterDTO(Setter setter, string mac)
        {
            this.clientId = mac;
            this.pkSetId = setter.Pk_Set_Id.ToString();
            this.setUniqueId = setter.Set_Unique_Id.ToString();
            this.setLanguage = setter.Set_Language.ToString();
            this.setOrganizationName = setter.Set_OrganizationName;
            this.setOrganizationSort = setter.Set_OrganizationSort;
            this.setPhotoLocation = setter.Set_PhotoLocation;
            this.setOrganizationPhone = setter.Set_OrganizationPhone;
            this.setVersion = setter.Set_Version;
            this.backUp = setter.Back_Up;
        }
    }
}
