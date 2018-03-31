using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.http.dto
{
    public class SymptomInfoDTO
    {
        //V1表信息
        public string clientId;
        public string pkSiId;
        public string gmtCreate;
        public string gmtModified;
        public string fkUserId;
        public string siIsjoin;
        public string siWaterinput;
        public string siInquiry;
        public string siCareinfo;
        //V2表信息
        //训练信息id
        public string fkTIId;
        //高血压（康复前）
        public string siPreHighpressure;
        //低血压（康复前）
        public string siPreLowpressure;
        //心率（康复前）
        public string siPreHeartrate;
        //体温（康复前）
        public string siPreAnimalheat;
        //脉搏（康复前）
        public string siPrePulse;
        //高血压（康复后）
        public string siSufHighpressure;
        //低血压（康复后）
        public string siSufLowpressure;
        //心率（康复后）
        public string siSufHeartrate;
        //体温（康复后）
        public string siSufAnimalheat;
        //脉搏（康复后）
        public string siSufPulse; 

        public SymptomInfoDTO(SymptomInfo symptomInfo,Setter setter) {
            this.clientId = setter.Set_Unique_Id;
            this.fkUserId = symptomInfo.Fk_User_Id.ToString();

            this.gmtCreate = symptomInfo.Gmt_Create.ToString().Replace("/", "-");
            this.gmtModified = symptomInfo.Gmt_Modified.ToString().Replace("/", "-");
            this.pkSiId = symptomInfo.Pk_SI_Id.ToString();
            this.siCareinfo = symptomInfo.SI_CareInfo.ToString();
            this.siInquiry = symptomInfo.SI_Inquiry.ToString();
            this. siIsjoin= symptomInfo.SI_IsJoin.ToString();
            this.siWaterinput = symptomInfo.SI_WaterInput.ToString();
        }
        public SymptomInfoDTO(SymptomInfo symptomInfo, string mac)
        {   //mac
            this.clientId = mac;
            //V1表信息
            this.fkUserId = symptomInfo.Fk_User_Id.ToString();
            this.gmtCreate = symptomInfo.Gmt_Create.ToString().Replace("/", "-");
            this.gmtModified = symptomInfo.Gmt_Modified.ToString().Replace("/", "-");
            this.pkSiId = symptomInfo.Pk_SI_Id.ToString();
            this.siCareinfo = symptomInfo.SI_CareInfo.ToString();
            this.siInquiry = symptomInfo.SI_Inquiry.ToString();
            this.siIsjoin = symptomInfo.SI_IsJoin.ToString();
            this.siWaterinput = symptomInfo.SI_WaterInput.ToString();
            //V2表信息
            this.fkTIId = symptomInfo.Fk_TI_Id.ToString();
            this.siPreAnimalheat= symptomInfo.SI_Pre_AnimalHeat.ToString();
            this.siPreHeartrate= symptomInfo.SI_Pre_HeartRate.ToString();
            this.siPreHighpressure= symptomInfo.SI_Pre_HighPressure.ToString();
            this.siPreLowpressure= symptomInfo.SI_Pre_LowPressure.ToString();
            this.siPrePulse= symptomInfo.SI_Pre_LowPressure.ToString();
            this.siSufAnimalheat= symptomInfo.SI_Suf_AnimalHeat.ToString();
            this.siSufHeartrate= symptomInfo.SI_Suf_HeartRate.ToString();
            this.siSufHighpressure = symptomInfo.SI_Suf_HighPressure.ToString();
            this.siSufLowpressure= symptomInfo.SI_Suf_LowPressure.ToString();
            this.siSufPulse= symptomInfo.SI_Suf_Pulse.ToString();
        }
    }
}
