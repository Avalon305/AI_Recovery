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
        public string clientId;
        public string pkSiId;
        public string gmtCreate;
        public string gmtModified;
        public string fkUserId;
        public string siIsjoin;
        public string siWaterinput;
        public string siInquiry;
        public string siCareinfo;

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
        {
            this.clientId = mac;
            this.fkUserId = symptomInfo.Fk_User_Id.ToString();

            this.gmtCreate = symptomInfo.Gmt_Create.ToString();
            this.gmtModified = symptomInfo.Gmt_Modified.ToString();
            this.pkSiId = symptomInfo.Pk_SI_Id.ToString();
            this.siCareinfo = symptomInfo.SI_CareInfo.ToString();
            this.siInquiry = symptomInfo.SI_Inquiry.ToString();
            this.siIsjoin = symptomInfo.SI_IsJoin.ToString();
            this.siWaterinput = symptomInfo.SI_WaterInput.ToString();
        }
    }
}
