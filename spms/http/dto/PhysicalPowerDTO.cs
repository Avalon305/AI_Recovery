using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.http.dto
{
    public class PhysicalPowerDTO
    {
        public string clientId;
        public string pkPpId;
        public string gmtCreate;
        public string gmtModified;
        public string fkUserId;
        public string ppHigh;
        public string ppWeight;
        public string ppGrip;
        public string ppEyeopenstand;
        public string ppFunctionprotract;
        public string ppSitandreach;
        public string ppTimeupgo;
        public string ppWalk5milegeneral;
        public string ppWalk5milefast;
        public string ppWalk10mile;
        public string ppWalk6minute;
        public string ppStep2minute;
        public string ppLegraise2minute;
        public string ppUsermemo;
        public string ppWorkermemo;
       
       
             public PhysicalPowerDTO(PhysicalPower physicalPower, string mac)
        {
            this.clientId = mac;
            this.fkUserId = physicalPower.FK_user_Id.ToString();
            this.gmtCreate = physicalPower.Gmt_Create.ToString().Replace("/", "-");
            this.gmtModified = physicalPower.Gmt_Modified.ToString().Replace("/", "-");
            this.pkPpId = physicalPower.Pk_PP_Id.ToString();
            this.ppEyeopenstand = physicalPower.PP_EyeOpenStand.ToString();
            this.ppFunctionprotract = physicalPower.PP_FunctionProtract.ToString();
            this.ppGrip = physicalPower.PP_Grip.ToString();
            this.ppHigh = physicalPower.PP_High.ToString();
            this.ppLegraise2minute = physicalPower.PP_LegRaise2Minute.ToString();
            this.ppSitandreach = physicalPower.PP_SitandReach.ToString();
            this.ppStep2minute = physicalPower.PP_Step2Minute.ToString();
            this.ppTimeupgo = physicalPower.PP_TimeUpGo.ToString();
            this.ppUsermemo = physicalPower.PP_UserMemo.ToString();
            this.ppWalk10mile = physicalPower.PP_Walk10Mile.ToString();
            this.ppWalk5milefast = physicalPower.PP_Walk5MileFast.ToString();
            this.ppWalk5milegeneral = physicalPower.PP_Walk5MileGeneral.ToString();
            this.ppWalk6minute = physicalPower.PP_Walk6Minute.ToString();
            this.ppWeight = physicalPower.PP_Weight.ToString();
            this.ppWorkermemo = physicalPower.PP_WorkerMemo.ToString();
        }
    }
}
