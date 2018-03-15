using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.http.dto
{
    public class DevicePrescriptionDTO
    {
        public string clientId;
        public string pkDpId;
        public string gmtCreate;
        public string gmtModified;
        public string fkTiId;
        public string fkDsId;
        public string dpAttrs;
        public string dpGroupcount;
        public string dpGroupnum;
        public string dpRelaxtime;
        public string dpMoveway;
        public string dpStatus;
        public string dpWeight;
        public string dpMemo;
        public DevicePrescriptionDTO(DevicePrescription devicePrescription,Setter setter) {
            this.clientId = setter.Set_Unique_Id;
            this.dpAttrs = devicePrescription.DP_Attrs;
            this.dpGroupcount = devicePrescription.dp_groupcount.ToString();
            this.dpGroupnum = devicePrescription.dp_groupnum.ToString();
            this.dpMemo = devicePrescription.DP_Memo;
            this.dpMoveway = devicePrescription.dp_moveway.ToString();

            this.dpRelaxtime = devicePrescription.dp_relaxtime.ToString();
            this.dpStatus = devicePrescription.dp_status.ToString();
            this.dpWeight = devicePrescription.dp_weight.ToString();
            this.fkDsId = devicePrescription.Fk_DS_Id.ToString();
            this.fkTiId = devicePrescription.Fk_TI_Id.ToString();
            this.gmtCreate = devicePrescription.Gmt_Create.ToString();
            this.gmtModified = devicePrescription.Gmt_Modified.ToString();
            this.pkDpId = devicePrescription.Pk_DP_Id.ToString();
        }
        public DevicePrescriptionDTO(DevicePrescription devicePrescription, string mac)
        {
            this.clientId = mac;
            this.dpAttrs = devicePrescription.DP_Attrs;
            this.dpGroupcount = devicePrescription.dp_groupcount.ToString();
            this.dpGroupnum = devicePrescription.dp_groupnum.ToString();
            this.dpMemo = devicePrescription.DP_Memo;
            this.dpMoveway = devicePrescription.dp_moveway.ToString();

            this.dpRelaxtime = devicePrescription.dp_relaxtime.ToString();
            this.dpStatus = devicePrescription.dp_status.ToString();
            this.dpWeight = devicePrescription.dp_weight.ToString();
            this.fkDsId = devicePrescription.Fk_DS_Id.ToString();
            this.fkTiId = devicePrescription.Fk_TI_Id.ToString();
            this.gmtCreate = devicePrescription.Gmt_Create.ToString();
            this.gmtModified = devicePrescription.Gmt_Modified.ToString();
            this.pkDpId = devicePrescription.Pk_DP_Id.ToString();
        }
    }
}
