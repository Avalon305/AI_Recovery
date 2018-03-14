using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.http.dto
{
    public class TrainInfoDTO
    {
        public string clientId;
        public string pkTiId;
        public string gmtCreate;
        public string gmtModified;
        public string fkUserId;

        public TrainInfoDTO(TrainInfo trainInfo,Setter setter)
        {
            this.clientId = setter.Set_Unique_Id;
            this.fkUserId = trainInfo.FK_User_Id;
            this.gmtCreate = trainInfo.Gmt_Create.ToString();
            this.gmtModified = trainInfo.Gmt_Modified.ToString();
            this.pkTiId = trainInfo.Pk_TI_Id.ToString();
        }
        public TrainInfoDTO(TrainInfo trainInfo, string mac)
        {
            this.clientId = mac;
            this.fkUserId = trainInfo.FK_User_Id;
            this.gmtCreate = trainInfo.Gmt_Create.ToString();
            this.gmtModified = trainInfo.Gmt_Modified.ToString();
            this.pkTiId = trainInfo.Pk_TI_Id.ToString();
        }
    }
}
