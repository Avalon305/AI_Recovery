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
        public string siPreHighPressure;
        //低血压（康复前）
        public string siPreLowPressure;
        //心率（康复前）
        public string siPreHeartRate;
        //体温（康复前）
        public string siPreAnimalHeat;
        //脉搏（康复前）
        public string siPrePulse;
        //高血压（康复后）
        public string siSufHighPressure;
        //低血压（康复后）
        public string siSufLowPressure;
        //心率（康复后）
        public string siSufHeartRate;
        //体温（康复后）
        public string siSufAnimalHeat;
        //脉搏（康复后）
        public string siSufPulse; 

        public SymptomInfoDTO(SymptomInfo symptomInfo,Setter setter) {
            this.clientId = setter.Set_Unique_Id;
            this.fkUserId = symptomInfo.Fk_User_Id.ToString();

            this.gmtCreate = symptomInfo.Gmt_Create.ToString();
            this.gmtModified = symptomInfo.Gmt_Modified.ToString();
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
            this.gmtCreate = symptomInfo.Gmt_Create.ToString();
            this.gmtModified = symptomInfo.Gmt_Modified.ToString();
            this.pkSiId = symptomInfo.Pk_SI_Id.ToString();
            this.siCareinfo = symptomInfo.SI_CareInfo.ToString();
            this.siInquiry = symptomInfo.SI_Inquiry.ToString();
            this.siIsjoin = symptomInfo.SI_IsJoin.ToString();
            this.siWaterinput = symptomInfo.SI_WaterInput.ToString();
            //V2表信息
            this.fkTIId = symptomInfo.Fk_TI_Id.ToString();
            this.siPreAnimalHeat= symptomInfo.SI_Pre_AnimalHeat.ToString();
            this.siPreHeartRate= symptomInfo.SI_Pre_HeartRate.ToString();
            this.siPreHighPressure= symptomInfo.SI_Pre_HighPressure.ToString();
            this.siPreLowPressure= symptomInfo.SI_Pre_LowPressure.ToString();
            this.siPrePulse= symptomInfo.SI_Pre_LowPressure.ToString();
            this.siSufAnimalHeat= symptomInfo.SI_Suf_AnimalHeat.ToString();
            this.siSufHeartRate= symptomInfo.SI_Suf_HeartRate.ToString();
            this.siSufHighPressure = symptomInfo.SI_Suf_HighPressure.ToString();
            this.siSufLowPressure= symptomInfo.SI_Suf_LowPressure.ToString();
            this.siSufPulse= symptomInfo.SI_Suf_Pulse.ToString();
        }
    }
}
