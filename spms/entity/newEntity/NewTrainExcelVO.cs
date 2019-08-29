using spms.constant;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace spms.entity.newEntity
{
    public class NewTrainExcelVO
    {
        public DateTime Gmt_Create { get; set; }
        //设备名称
        public string DS_name { get; set; }
        //组数
        public int dp_groupcount { get; set; }
        //每组个数
        public int dp_groupnum { get; set; }
        //休息时间
        public int dp_relaxtime { get; set; }
        //移乘方式
        public string dp_moveway { get; set; }
        //自觉运动强度,1-10代表轻松->剧烈
        public string PR_SportStrength { get; set; }
        //第一个时间
        public double PR_Finish_Time { get; set; }
        //热量
        public double PR_Energy { get; set; }
        //完成组数
        public int PR_Finish_Num { get; set; }
        //病人感想
        public string PR_UserThoughts { get; set; }

        public NewTrainExcelVO(NewTrainComprehensive tc)
        {
            this.Gmt_Create = (DateTime)tc.Gmt_Create;
            //坐式划船机
            if (LanguageUtils.EqualsResource(tc.DS_name, "NewDev.Rowing"))
            {
                this.DS_name = LanguageUtils.GetCurrentLanuageStrByKey("NewDev.Rowing");
            }
            //坐式推胸机
            else if (LanguageUtils.EqualsResource(tc.DS_name, "NewDev.ChestPress"))
            {
                this.DS_name = LanguageUtils.GetCurrentLanuageStrByKey("NewDev.ChestPress");
            }
            //腿部推蹬机
            else if (LanguageUtils.EqualsResource(tc.DS_name, "NewDev.HorizontalLegPress"))
            {
                this.DS_name = LanguageUtils.GetCurrentLanuageStrByKey("NewDev.HorizontalLegPress");
            }
            //腹肌训练机
            else if (LanguageUtils.EqualsResource(tc.DS_name, "NewDev.AbdominalMuscleTraining"))
            {
                this.DS_name = LanguageUtils.GetCurrentLanuageStrByKey("NewDev.AbdominalMuscleTraining");
            }
            //三头肌训练机
            else if (LanguageUtils.EqualsResource(tc.DS_name, "NewDev.TricepsTraining"))
            {
                this.DS_name = LanguageUtils.GetCurrentLanuageStrByKey("NewDev.TricepsTraining");
            }
            //腿部外弯机
            else if (LanguageUtils.EqualsResource(tc.DS_name, "NewDev.LegAbduction"))
            {
                this.DS_name = LanguageUtils.GetCurrentLanuageStrByKey("NewDev.LegAbduction");
            }
            //腿部内弯机
            else if (LanguageUtils.EqualsResource(tc.DS_name, "NewDev.LegInturn"))
            {
                this.DS_name = LanguageUtils.GetCurrentLanuageStrByKey("NewDev.LegInturn");
            }
            //蝴蝶机
            else if (LanguageUtils.EqualsResource(tc.DS_name, "NewDev.ButterflyMachine"))
            {
                this.DS_name = LanguageUtils.GetCurrentLanuageStrByKey("NewDev.ButterflyMachine");
            }
            //反向蝴蝶机
            else if (LanguageUtils.EqualsResource(tc.DS_name, "NewDev.ReverseButterflyMachine"))
            {
                this.DS_name = LanguageUtils.GetCurrentLanuageStrByKey("NewDev.ReverseButterflyMachine");
            }
            //坐式背部伸展机
            else if (LanguageUtils.EqualsResource(tc.DS_name, "NewDev.SittingBackExtender"))
            {
                this.DS_name = LanguageUtils.GetCurrentLanuageStrByKey("NewDev.SittingBackExtender");
            }
            this.dp_groupcount = tc.dp_groupcount;
            this.dp_groupnum = tc.dp_groupnum;
            this.dp_relaxtime = tc.dp_relaxtime;
            this.dp_moveway = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, tc.dp_moveway + "");

            if (tc.PR_SportStrength == "10" || tc.PR_SportStrength == "9")
            {
                this.PR_SportStrength = LanguageUtils.ConvertLanguage("困难", "Difficult");
            }
            else if (tc.PR_SportStrength == "8" || tc.PR_SportStrength == "7")
            {
                this.PR_SportStrength = LanguageUtils.ConvertLanguage("有点困难", "A little difficult");
            }
            else if (tc.PR_SportStrength == "6" || tc.PR_SportStrength == "5")
            {
                this.PR_SportStrength = LanguageUtils.ConvertLanguage("轻松", "Relaxed");
            }
            else if (tc.PR_SportStrength == "4" || tc.PR_SportStrength == "3")
            {
                this.PR_SportStrength = LanguageUtils.ConvertLanguage("很轻松", "Easy");
            }
            else if (tc.PR_SportStrength == "2" || tc.PR_SportStrength == "1")
            {
                this.PR_SportStrength = LanguageUtils.ConvertLanguage("非常轻松", "Very easy");
            }
            else
            {
                this.PR_SportStrength = "";
            }

            this.PR_Finish_Time = tc.PR_finish_time;
            this.PR_Energy = tc.PR_Energy;
            this.PR_Finish_Num = tc.PR_finish_num;
            this.PR_UserThoughts = tc.PR_UserThoughts;

        }

        public class SymptomInfoExcelVO
        {
            //数据创建时间
            public DateTime Gmt_Create { get; set; }
            //血压（康复前）
            public string SI_Pre_Pressure { get; set; }
            //心率（康复前）
            public string SI_Pre_HeartRate { get; set; }
            //脉搏（康复前）
            public string SI_Pre_Pulse { get; set; }
            //体温（康复前）
            public string SI_Pre_AnimalHeat { get; set; }
            //高血压（康复后）
            public string SI_Suf_Pressure { get; set; }
            //心率（康复后）
            public string SI_Suf_HeartRate { get; set; }
            //脉搏（康复后）
            public string SI_Suf_Pulse { get; set; }
            //体温（康复后）
            public string SI_Suf_AnimalHeat { get; set; }
            //１．身体倦怠
            public int SI_Tired { get; set; }
            //２．腹泻
            public int SI_Diarrhoea { get; set; }
            //３．摇晃
            public int SI_Shake { get; set; }
            //４．心跳、气喘
            public int SI_Asthma { get; set; }
            //５．咳嗽、有痰
            public int SI_Phlegm { get; set; }
            //６．发烧
            public int SI_Fever { get; set; }
            //７．胸部、肚子痛
            public int SI_Stomach { get; set; }
            //８．没有食欲
            public int SI_NoAppetite { get; set; }
            //９．持续便秘
            public int SI_Constipation { get; set; }
            //１０．感到头晕
            public int SI_Dizzy { get; set; }
            //１１．头痛
            public int SI_Headache { get; set; }
            //１２．其他
            public int SI_Other { get; set; }
            //１３．没有相关症状
            public int SI_NoSymptoms { get; set; }

            //是否参加
            public string SI_IsJoin { get; set; }
            //饮水量
            public string SI_WaterInput { get; set; }
            //看护记录
            public string SI_CareInfo { get; set; }

            public SymptomInfoExcelVO(SymptomInfo si)
            {
                this.Gmt_Create = (DateTime)si.Gmt_Create;
                this.SI_Pre_Pressure = si.SI_Pre_HighPressure + "/" + si.SI_Pre_LowPressure;
                this.SI_Pre_HeartRate = si.SI_Pre_HeartRate;
                this.SI_Pre_Pulse = si.SI_Pre_Pulse == 0 ? LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Regular") : LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Irregular");
                this.SI_Pre_AnimalHeat = si.SI_Pre_AnimalHeat;
                this.SI_Suf_Pressure = si.SI_Suf_HighPressure + "/" + si.SI_Suf_LowPressure;
                this.SI_Suf_HeartRate = si.SI_Suf_HeartRate;
                this.SI_Suf_Pulse = si.SI_Suf_Pulse == 0 ? LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Regular") : LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Irregular");
                this.SI_Suf_AnimalHeat = si.SI_Suf_AnimalHeat;
                string[] inquirys = Regex.Split(si.SI_Inquiry, ",");
                foreach (string str in inquirys)
                {
                    //1. 身体疲倦
                    if (LanguageUtils.EqualsResource(str, "VitalInfoView.Janguidness"))
                    {
                        this.SI_Tired = 1;
                    }
                    //2.腹泻
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.Diarrhea"))
                    {
                        this.SI_Diarrhoea = 1;
                    }
                    //3.摇晃
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.Wamble"))
                    {
                        this.SI_Shake = 1;
                    }
                    //4. 心跳、气喘
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.BeingBreathless"))
                    {
                        this.SI_Asthma = 1;
                    }
                    //5. 咳嗽、有痰
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.CoughAndPhlegm"))
                    {
                        this.SI_Phlegm = 1;
                    }
                    //6. 发烧
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.Fever"))
                    {
                        this.SI_Fever = 1;
                    }
                    //7. 胸部、肚子痛
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.Stomachache"))
                    {
                        this.SI_Stomach = 1;
                    }
                    //8. 没有食欲
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.APoorAppetite"))
                    {
                        this.SI_NoAppetite = 1;
                    }
                    //9. 持续便秘
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.Constipation"))
                    {
                        this.SI_Constipation = 1;
                    }
                    //10. 感到头晕
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.Dizziness"))
                    {
                        this.SI_Dizzy = 1;
                    }
                    //11. 头痛
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.Headache"))
                    {
                        this.SI_Headache = 1;
                    }
                    //12.其他
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.Other"))
                    {
                        this.SI_Other = 1;
                    }
                    //13.没有相关症状
                    else if (LanguageUtils.EqualsResource(str, "VitalInfoView.NotApplicable"))
                    {
                        this.SI_NoSymptoms = 1;
                    }

                }
                this.SI_IsJoin = si.SI_IsJoin == 0 ? LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.No") : LanguageUtils.GetCurrentLanuageStrByKey("VitalInfoView.Yes");
                this.SI_WaterInput = si.SI_WaterInput;
                this.SI_CareInfo = si.SI_CareInfo;
            }
        }

        public class DevicePrescriptionExcel
        {
            //数据创建时间
            public DateTime? Gmt_Create { get; set; }
            //设备名称
            public string DS_name { get; set; }
            //组数
            public int dp_groupcount { get; set; }
            //每组个数
            public int dp_groupnum { get; set; }
            //休息时间
            public int dp_relaxtime { get; set; }
            //移乘方式
            public int dp_moveway { get; set; }
            //砝码
            public double dp_weight { get; set; }
            //注意点，指示
            public int PR_Evaluate { get; set; }

        }

        /// <summary>
        /// 用户Excel普通文档导出体力评价
        /// </summary>
        public class PhysicaleExcelVO
        {
            //1.数据创建时间
            public DateTime Gmt_Create { get; set; }
            // 2.身高
            public string PP_High { get; set; }
            //3.体重
            public string PP_Weight { get; set; }
            //4.握力
            public string PP_Grip { get; set; }
            //5.睁眼是否单脚站立
            public string PP_EyeOpenStand { get; set; }
            //6.功能性前伸
            public string PP_FunctionProtract { get; set; }
            //7.坐姿体前屈
            public string PP_SitandReach { get; set; }
            //8.time&up go
            public string PP_TimeUpGo { get; set; }
            //9.5m步行-通常
            public string PP_Walk5MileGeneral { get; set; }
            //10.5m步行-最快
            public string PP_Walk5MileFast { get; set; }
            //11.10m步行，第一个字段是模式
            public string PP_Walk10Mile { get; set; }
            //12.6分钟步行
            public string PP_Walk6Minute { get; set; }
            //13.2分钟踏步
            public string PP_Step2Minute { get; set; }
            //14.2分钟抬腿
            public string PP_LegRaise2Minute { get; set; }
            //15.使用用者感想
            public string PP_UserMemo { get; set; }
            //16.工作人员感想
            public string PP_WorkerMemo { get; set; }

            /// <summary>
            /// 从字符串中截取第一个参数值
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            private string SubstringParams(string value)
            {
                int startIndex = value.IndexOf(",") + 1;
                int endIndex = value.IndexOf(",", startIndex);
                string result = value.Substring(startIndex, endIndex - startIndex);

                if (result.Contains("param"))
                {
                    return LanguageUtils.ConvertLanguage("未填写", "Unfilled");
                }
                else
                {
                    return result;
                }
            }

            private string isEmpty(string val)
            {
                if (val == null || val.Trim() == "")
                {
                    return LanguageUtils.ConvertLanguage("未填写", "Unfilled");
                }
                else
                {
                    return val;
                }
            }


            public PhysicaleExcelVO(PhysicalPower physicalPower)
            {
                if (physicalPower.PP_High.IndexOf(",") == -1)
                {
                    Console.WriteLine("体力报告的数据格式不对，正常的格式应为以','结尾的数据格式");
                    return;
                }
                this.Gmt_Create = physicalPower.Gmt_Create.Value;
                this.PP_High = SubstringParams(physicalPower.PP_High).ToString();
                this.PP_Weight = SubstringParams(physicalPower.PP_Weight).ToString();
                this.PP_Grip = SubstringParams(physicalPower.PP_Grip).ToString();
                this.PP_EyeOpenStand = SubstringParams(physicalPower.PP_EyeOpenStand).ToString();
                this.PP_FunctionProtract = SubstringParams(physicalPower.PP_FunctionProtract).ToString();
                this.PP_SitandReach = SubstringParams(physicalPower.PP_SitandReach).ToString();
                this.PP_TimeUpGo = SubstringParams(physicalPower.PP_TimeUpGo).ToString();
                this.PP_Walk5MileGeneral = SubstringParams(physicalPower.PP_Walk5MileGeneral).ToString();
                this.PP_Walk5MileFast = SubstringParams(physicalPower.PP_Walk5MileFast).ToString();
                this.PP_Walk10Mile = SubstringParams(physicalPower.PP_Walk10Mile).ToString();
                this.PP_Walk6Minute = SubstringParams(physicalPower.PP_Walk6Minute).ToString();
                this.PP_Step2Minute = SubstringParams(physicalPower.PP_Step2Minute).ToString();
                this.PP_LegRaise2Minute = SubstringParams(physicalPower.PP_LegRaise2Minute).ToString();
                this.PP_UserMemo = isEmpty(physicalPower.PP_UserMemo);
                this.PP_WorkerMemo = isEmpty(physicalPower.PP_WorkerMemo);
            }
        }
    }
}
