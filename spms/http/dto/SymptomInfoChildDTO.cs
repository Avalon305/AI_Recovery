using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.http.dto
{
    public class SymptomInfoChildDTO
    {
        public string clientId;
        public string pkSicId;
        public string gmtCreate;
        public string gmtModified;
        public string fkSiId;
        public string status;
        public string sicHighpressure;
        public string sicLowpressure;
        public string sicHeartrate;
        public string sicPulse;
        public string sicAnimalheat;
        public SymptomInfoChildDTO(SymptomInfoChild symptomInfoChild,Setter setter) {
            this.clientId = setter.Set_Unique_Id;
            this.fkSiId = symptomInfoChild.Fk_SI_Id.ToString();
            this.gmtCreate = symptomInfoChild.Gmt_Create.ToString();
            this.gmtModified = symptomInfoChild.Gmt_Modified.ToString();
            this.pkSicId = symptomInfoChild.Pk_SIC_Id.ToString();
            this.sicAnimalheat = symptomInfoChild.SIC_AnimalHeat.ToString();
            this.sicHeartrate = symptomInfoChild.SIC_HeartRate.ToString();
            this.sicHighpressure = symptomInfoChild.SIC_HighPressure.ToString();
            this.sicLowpressure = symptomInfoChild.SIC_LowPressure.ToString();
            this.sicPulse = symptomInfoChild.SIC_Pulse.ToString();
            this.status = symptomInfoChild.Status.ToString();
        }
    }
}
