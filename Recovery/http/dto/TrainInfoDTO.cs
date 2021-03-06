﻿using Recovery.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.http.dto
{
    public class TrainInfoDTO
    {
        public string clientId;
        public string pkTiId;
        public string gmtCreate;
        public string gmtModified;
        public string fkUserId;
        //status
        public string status;

        
        public TrainInfoDTO(TrainInfo trainInfo, string mac)
        {
            this.clientId = mac;
            this.fkUserId = trainInfo.FK_User_Id.ToString();
            this.gmtCreate = trainInfo.Gmt_Create.ToString().Replace("/", "-");
            this.gmtModified = trainInfo.Gmt_Modified.ToString().Replace("/", "-");
            this.pkTiId = trainInfo.Pk_TI_Id.ToString();

            this.status = trainInfo.Status.ToString();
        }
    }
}
