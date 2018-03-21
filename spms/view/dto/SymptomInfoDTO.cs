using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using spms.entity;

namespace spms.view.dto
{
    /// <summary>
    /// 用于绑定界面
    /// </summary>
    class SymptomInfoDTO
    {
        
        //数据创建时间
        public DateTime? Create { get; set; }
        //饮水量
        public string WaterInput { get; set; }
        //看护记录
        public string CareInfo { get; set; }
        //问诊票
        public string Inquiry { get; set; }
        //心率（康复前）
        public string Pre_HeartRate { get; set; }
        //体温（康复前）
        public string Pre_AnimalHeat { get; set; }
        //脉搏（康复前）
        public string Pre_Pulse { get; set; }
        //血压（康复前）
        public string Pre_Pressure { get; set; }
        //心率（康复后）
        public string Suf_HeartRate { get; set; }
        //体温（康复后）
        public string Suf_AnimalHeat { get; set; }
        //脉搏（康复后）
        public string Suf_Pulse { get; set; }
        //血压（康复后）
        public string Suf_Pressure { get; set; }
        //是否参加
        public string Join { get; set; }

        public SymptomInfoDTO(SymptomInfo symptomInfo)
        {
            this.Create = symptomInfo.Gmt_Create;
            this.CareInfo = symptomInfo.SI_CareInfo;
            this.Inquiry = symptomInfo.SI_Inquiry;
            this.WaterInput = symptomInfo.SI_WaterInput;
            this.Join = symptomInfo.SI_IsJoin == 0 ? "否" : "是";

            this.Pre_Pressure = symptomInfo.SI_Pre_HighPressure + " / " + symptomInfo.SI_Pre_LowPressure;
            this.Pre_AnimalHeat = symptomInfo.SI_Pre_AnimalHeat;
            this.Pre_Pulse = symptomInfo.SI_Pre_Pulse == 0 ? "规律脉" : "脉律不齐";
            this.Pre_HeartRate = symptomInfo.SI_Pre_HeartRate;

            this.Suf_AnimalHeat = symptomInfo.SI_Suf_AnimalHeat;
            this.Suf_HeartRate = symptomInfo.SI_Suf_HeartRate;
            this.Suf_Pulse = symptomInfo.SI_Suf_Pulse == 0 ? "规律脉" : "脉律不齐";
            this.Suf_Pressure = symptomInfo.SI_Suf_HighPressure + " / " + symptomInfo.SI_Suf_LowPressure;
        }
    }
}
