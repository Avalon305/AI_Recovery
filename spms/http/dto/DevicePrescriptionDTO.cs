using spms.entity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        //计时器增加
        //计时器是否有效
        public string dpTimer { get; set; }
        //计时时间
        public string dpTimeCount { get; set; }
        //计时方式
        public string dpTimeType { get; set; }
        //移动距离
        public string dpMoveDistance { get; set; }

        public DevicePrescriptionDTO(DevicePrescription devicePrescription, string mac)
        {
            this.clientId = mac;
            this.dpAttrs = devicePrescription.DP_Attrs;
            this.dpGroupcount = devicePrescription.dp_groupcount.ToString();
            this.dpGroupnum = devicePrescription.dp_groupnum.ToString();
            this.dpMemo = devicePrescription.DP_Memo;
            this.dpMoveway = devicePrescription.dp_moveway.ToString();

            this.dpRelaxtime = devicePrescription.dp_relaxtime.ToString();
            this.dpStatus = devicePrescription.Dp_status.ToString();
            this.dpWeight = devicePrescription.dp_weight.ToString();
            this.fkDsId = devicePrescription.Fk_DS_Id.ToString();
            this.fkTiId = devicePrescription.Fk_TI_Id.ToString();
            this.gmtCreate = devicePrescription.Gmt_Create.ToString().Replace("/","-");
            this.gmtModified = devicePrescription.Gmt_Modified.ToString().Replace("/", "-");
            this.pkDpId = devicePrescription.Pk_DP_Id.ToString();
            //计时器增加
            this.dpTimer = devicePrescription.dp_timer.ToString();
            this.dpTimeCount = devicePrescription.dp_timecount.ToString();
            this.dpTimeType = devicePrescription.dp_timetype.ToString();
            //移动距离
            this.dpMoveDistance = devicePrescription.dp_movedistance.ToString();
        }
    }
}
