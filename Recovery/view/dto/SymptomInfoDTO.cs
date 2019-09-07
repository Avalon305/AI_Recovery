using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recovery.entity;
using Recovery.util;

namespace Recovery.view.dto
{
    /// <summary>
    /// 用于绑定界面
    /// </summary>
    class SymptomInfoDTO
    {
        
        public int ID { get; set; }
        public int TI_ID { get; set; }
        //数据创建时间
        public DateTime Create { get; set; }
        //
        public string DateStr { get; set; }
        //血压（康复前）
        public string Pre_Pressure { get; set; }
        //脉搏（康复前）
        public string Pre_Pulse { get; set; }
        //心率（康复前）
        public string Pre_HeartRate { get; set; }
        //体温（康复前）
        public string Pre_AnimalHeat { get; set; }
        //血压（康复后）
        public string Suf_Pressure { get; set; }
        //脉搏（康复后）
        public string Suf_Pulse { get; set; }
        //心率（康复后）
        public string Suf_HeartRate { get; set; }
        //体温（康复后）
        public string Suf_AnimalHeat { get; set; }
        //饮水量
        public string WaterInput { get; set; }
        //问诊票
        public string Inquiry { get; set; }
        //是否参加
        public string Join { get; set; }
        //看护记录
        public string CareInfo { get; set; }

        public List<SymptomInfoDTO> ConvertDtoList(List<SymptomInfo> symptomInfos)
        {
            List<SymptomInfoDTO> symptomInfoDtos = new List<SymptomInfoDTO>();
            foreach (var symptomInfo in symptomInfos)
            {
                symptomInfoDtos.Add(new SymptomInfoDTO(symptomInfo));
            }

            return symptomInfoDtos;
        }

        public SymptomInfoDTO()
        {

        }
       
        public SymptomInfoDTO(SymptomInfo symptomInfo)
        {
            this.TI_ID = symptomInfo.Fk_TI_Id;
            this.ID = symptomInfo.Pk_SI_Id;
            this.Create = symptomInfo.Gmt_Create.Value;
            this.CareInfo = symptomInfo.SI_CareInfo;
            this.Inquiry = symptomInfo.SI_Inquiry;
            this.WaterInput = symptomInfo.SI_WaterInput;
            this.Join = symptomInfo.SI_IsJoin == 0 ? LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Yes") : LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.No");

            this.Pre_Pressure = symptomInfo.SI_Pre_HighPressure + " / " + symptomInfo.SI_Pre_LowPressure;
            this.Pre_AnimalHeat = symptomInfo.SI_Pre_AnimalHeat;
            if (symptomInfo.SI_Pre_Pulse != -1)
            {
                this.Pre_Pulse = symptomInfo.SI_Pre_Pulse == 0 ? LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Regular") : LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Irregular");
            }
            else
            {
                this.Pre_Pulse = "";
            }
            this.Pre_HeartRate = symptomInfo.SI_Pre_HeartRate;

            this.Suf_AnimalHeat = symptomInfo.SI_Suf_AnimalHeat;
            this.Suf_HeartRate = symptomInfo.SI_Suf_HeartRate;
            if(symptomInfo.SI_Suf_Pulse != -1) { 
                this.Suf_Pulse = symptomInfo.SI_Suf_Pulse == 0 ? LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Regular") : LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Irregular");
            }
            else
            {
                this.Suf_Pulse = "";
            }
            this.Suf_Pressure = symptomInfo.SI_Suf_HighPressure + " / " + symptomInfo.SI_Suf_LowPressure;
            List<string> inquiryList = new List<string>();
            this.DateStr = symptomInfo.Gmt_Create.ToString();
            //翻译问卷票
            foreach (string inquiry in symptomInfo.SI_Inquiry.Split(new char[] { ',' }))
            {
                if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Janguidness"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Janguidness"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Diarrhea"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Diarrhea"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Wamble"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Wamble"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.BeingBreathless"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.BeingBreathless"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.CoughAndPhlegm"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.CoughAndPhlegm"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Fever"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Fever"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Stomachache"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Stomachache"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.APoorAppetite"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.APoorAppetite"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Constipation"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Constipation"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Dizziness"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Dizziness"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Headache"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Headache"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Other"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Other"));
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.NotApplicable"))
                {
                    inquiryList.Add(LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.NotApplicable"));
                }
            }
            Inquiry = string.Join(",", inquiryList.ToArray());
        }
    }
}