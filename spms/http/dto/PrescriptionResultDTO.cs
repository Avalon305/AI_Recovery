using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.http.dto
{
    public class PrescriptionResultDTO
    {
        public string clientId;
        public string pkPrId;
        public string gmtCreate;
        public string gmtModified;
        public string prSportstrength;
        public string fkDpId;
        public string prTime1;
        public string prDistance;
        public string prCountworkquantity;
        public string prCal;
        public string prIndex;
        public string prTime2;
        public string prFinishgroup;
        public string prEvaluate;
        public string prAttentionpoint;
        public string prUserthoughts;
        public string prMemo;
       
        public PrescriptionResultDTO(PrescriptionResult prescriptionResult, string mac)
        {
            this.clientId = mac;
            this.fkDpId = prescriptionResult.Fk_DP_Id.ToString();

            this.gmtCreate = prescriptionResult.Gmt_Create.ToString().Replace("/", "-");
            this.gmtModified = prescriptionResult.Gmt_Modified.ToString().Replace("/", "-");
            this.pkPrId = prescriptionResult.Pk_PR_Id.ToString();
            this.prAttentionpoint = prescriptionResult.PR_AttentionPoint.ToString();
            this.prCal = prescriptionResult.PR_Cal.ToString();
            this.prCountworkquantity = prescriptionResult.PR_CountWorkQuantity.ToString();
            this.prDistance = prescriptionResult.PR_Distance.ToString();
            this.prEvaluate = prescriptionResult.PR_Evaluate.ToString();
            this.prFinishgroup = prescriptionResult.PR_FinishGroup.ToString();
            this.prIndex = prescriptionResult.PR_Index.ToString();
            this.prMemo = prescriptionResult.PR_Memo==null?"": prescriptionResult.PR_Memo.ToString();
            this.prSportstrength = prescriptionResult.PR_SportStrength.ToString();
            this.prTime1 = prescriptionResult.PR_Time1.ToString();
            this.prTime2 = prescriptionResult.PR_Time2.ToString();
            this.prUserthoughts = prescriptionResult.PR_UserThoughts.ToString();
        }
    }
}
