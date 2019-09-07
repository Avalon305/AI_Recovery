using Recovery.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.http.dto
{
    /// <summary>
    /// 用于对CustomData的对象上传到云平台数据的DTO，本实体成员都是与CustomData实体类成员相对应
    /// </summary>
    public class CustomDataDTO
    {
        //clientId
        public string clientId;
        public string pkCdId;
        public string cdCustomName;
        public string cdType;
        public string isDeleted;

        /// <summary>
        /// CustomData对象赋值
        /// </summary>
        /// <param name="customData"></param>
        /// <param name="mac"></param>
        public CustomDataDTO(CustomData customData, string mac)
        {
            this.clientId = mac;
            this.pkCdId = customData.Pk_CD_Id.ToString();
            this.cdCustomName = customData.CD_CustomName.ToString();
            this.cdType = customData.CD_Type.ToString();
            this.isDeleted = customData.Is_Deleted.ToString();
        }
    }
    
}
