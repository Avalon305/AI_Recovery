using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace spms.bean
{
    public class ComprehensiveReportBean
    {
        //用户信息
        public User UserInfo { get; set; }

        public List<TrainingAndSymptomBean> TrainingAndSymptomList { get; set; }
    }

    /// <summary>
    /// 训练信息和症状信息的综合类
    /// </summary>
    public class TrainingAndSymptomBean
    {
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //第一个时间
        public double PR_Time1 { get; set; }
        //第二个时间
        public double PR_Time2 { get; set; }
        //热量
        public double PR_Cal { get; set; }
        //指数
        public double PR_Index { get; set; }
        //高血压（康复前）
        public string SI_Pre_HighPressure { get; set; }
        //低血压（康复前）
        public string SI_Pre_LowPressure { get; set; }
        //高血压（康复后）
        public string SI_Suf_HighPressure { get; set; }
        //低血压（康复后）
        public string SI_Suf_LowPressure { get; set; }
        //饮水量
        public string SI_WaterInput { get; set; }
        //看护记录
        public string SI_CareInfo { get; set; }

        public override string ToString()
        {
            return $"{nameof(PR_Cal)}: {PR_Cal}, {nameof(Gmt_Create)}: {Gmt_Create}, {nameof(PR_Time1)}: {PR_Time1}, {nameof(PR_Time2)}: {PR_Time2}, {nameof(PR_Index)}: {PR_Index}, {nameof(SI_Pre_HighPressure)}: {SI_Pre_HighPressure}, {nameof(SI_Pre_LowPressure)}: {SI_Pre_LowPressure}, {nameof(SI_Suf_HighPressure)}: {SI_Suf_HighPressure}, {nameof(SI_Suf_LowPressure)}: {SI_Suf_LowPressure}, {nameof(SI_WaterInput)}: {SI_WaterInput}, {nameof(SI_CareInfo)}: {SI_CareInfo}";
        }
    }

    /// <summary>
    /// 用户定义委托
    /// </summary>
    class ValueChange
    {
        public bool _flag = false;
        public bool Flag
        {
            get
            {
                return _flag;
            }
            set
            {
                _flag = value;
                OnStringChange();
            }
        }
        public event EventHandler OnStringChangeEvent;
        public void OnStringChange()
        {
            if (OnStringChangeEvent != null)
            {
                OnStringChangeEvent(this, null);
            }
        }
    }

    /// <summary>
    /// 用于Excel的导出数据类
    /// </summary>
    public class PhysicalPowerExcekVO
    {
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //身高
        public string PP_High { get; set; }
        //体重
        public string PP_Weight { get; set; }
        //握力
        public string PP_Grip { get; set; }
        //睁眼单脚站立
        public string PP_EyeOpenStand { get; set; }
        //功能性前伸
        public string PP_FunctionProtract { get; set; }
        //坐姿体前屈
        public string PP_SitandReach { get; set; }
    }

    public class TrainComprehensive
    {
        public DateTime? Gmt_Create { get; set; }
        //设备名称
        public string DS_name { get; set; }
        //组数
        public int dp_groupcount { get; set; }
        //每组个数
        public int dp_groupnum { get; set; }
        //休息时间
        public int dp_relaxtime { get; set; }
        //砝码
        public double dp_weight { get; set; }
        //移乘方式
        public int dp_moveway { get; set; }
        //自觉运动强度,1-10代表轻松->剧烈
        public byte? PR_SportStrength { get; set; }
        //第一个时间
        public double PR_Time1 { get; set; }
        //第二个时间 
        public double PR_Time2 { get; set; }
        //距离
        public int PR_Distance { get; set; }
        //总工作量
        public double PR_CountWorkQuantity { get; set; }
        //热量
        public double PR_Cal { get; set; }
        //指数
        public double PR_Index { get; set; }
        //完成组数
        public int PR_FinishGroup { get; set; }
        //时机，姿势，评价 0没问题 1 有些许问题 2 有问题
        public byte? PR_Evaluate { get; set; }
        //备忘
        public string PR_Memo { get; set; }
        //注意点
        public string PR_AttentionPoint { get; set; }
        //病人感想
        public string PR_UserThoughts { get; set; }
    }

    public class TrainExcelVO
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
        //砝码
        public double dp_weight { get; set; }
        //移乘方式
        public int dp_moveway { get; set; }
        //自觉运动强度,1-10代表轻松->剧烈
        public string PR_SportStrength { get; set; }
        //第一个时间
        public double PR_Time { get; set; }
        //距离
        public int PR_Distance { get; set; }
        //总工作量
        public double PR_CountWorkQuantity { get; set; }
        //热量
        public double PR_Cal { get; set; }
        //指数
        public double PR_Index { get; set; }
        //完成组数
        public int PR_FinishGroup { get; set; }
        //时机，姿势，评价 0没问题 1 有些许问题 2 有问题
        public string PR_Evaluate { get; set; }
        //备忘
        public string PR_Memo { get; set; }
        //注意点
        public string PR_AttentionPoint { get; set; }
        //病人感想
        public string PR_UserThoughts { get; set; }

        public TrainExcelVO(TrainComprehensive tc)
        {
            this.Gmt_Create = (DateTime)tc.Gmt_Create;
            this.DS_name = tc.DS_name;
            this.dp_groupcount = tc.dp_groupcount;
            this.dp_groupnum = tc.dp_groupnum;
            this.dp_relaxtime = tc.dp_relaxtime;
            this.dp_weight = tc.dp_weight;
            this.dp_moveway = tc.dp_moveway;
            if (tc.PR_SportStrength == 10 || tc.PR_SportStrength == 9)
            {
                this.PR_SportStrength = "困难";
            }
            else if (tc.PR_SportStrength == 8 || tc.PR_SportStrength == 7)
            {
                this.PR_SportStrength = "有点困难";
            }
            else if (tc.PR_SportStrength == 6 || tc.PR_SportStrength == 5)
            {
                this.PR_SportStrength = "轻松";
            }
            else if (tc.PR_SportStrength == 4 || tc.PR_SportStrength == 3)
            {
                this.PR_SportStrength = "很轻松";
            }
            else if (tc.PR_SportStrength == 2 || tc.PR_SportStrength == 1)
            {
                this.PR_SportStrength = "非常轻松";
            }

            this.PR_Time = tc.PR_Time2 - tc.PR_Time1;
            this.PR_Distance = tc.PR_Distance;
            this.PR_CountWorkQuantity = tc.PR_CountWorkQuantity;
            this.PR_Cal = tc.PR_Cal;
            this.PR_Index = tc.PR_Index;
            this.PR_FinishGroup = tc.PR_FinishGroup;

            if (tc.PR_Evaluate == 0)
            {
                this.PR_Evaluate = "没问题";
            }
            else if (tc.PR_Evaluate == 1)
            {
                this.PR_Evaluate = "有些许问题";
            }
            else if (tc.PR_Evaluate == 2)
            {
                this.PR_Evaluate = "有问题";
            }


            this.PR_Memo = tc.PR_Memo;
            this.PR_AttentionPoint = tc.PR_AttentionPoint;
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
                this.SI_Pre_Pressure = si.SI_Pre_HighPressure+"/"+ si.SI_Pre_LowPressure;
                this.SI_Pre_HeartRate = si.SI_Pre_HeartRate;
                this.SI_Pre_Pulse = si.SI_Pre_Pulse == 0 ?"规律脉":"脉率不齐";
                this.SI_Pre_AnimalHeat = si.SI_Pre_AnimalHeat;
                this.SI_Suf_Pressure = si.SI_Suf_HighPressure + "/" + si.SI_Suf_LowPressure;
                this.SI_Suf_HeartRate = si.SI_Suf_HeartRate;
                this.SI_Suf_Pulse = si.SI_Suf_Pulse == 0 ? "规律脉" : "脉率不齐";
                this.SI_Suf_AnimalHeat = si.SI_Suf_AnimalHeat;
                string[] inquirys = Regex.Split(si.SI_Inquiry,",");
                foreach (string str in inquirys)
                {
                    if (str == "1. 身体疲倦")
                    {
                        this.SI_Tired = 1;
                    }
                    else if (str == "2. 腹泻")
                    {
                        this.SI_Diarrhoea = 1;
                    }
                    else if (str == "3. 摇晃")
                    {
                        this.SI_Shake = 1;
                    }
                    else if (str == "4. 心跳、气喘")
                    {
                        this.SI_Asthma = 1;
                    }
                    else if (str == "5. 咳嗽、有痰")
                    {
                        this.SI_Phlegm = 1;
                    }
                    else if (str == "6. 发烧")
                    {
                        this.SI_Fever = 1;
                    }
                    else if (str == "7. 胸部、肚子痛")
                    {
                        this.SI_Stomach = 1;
                    }
                    else if (str == "8. 没有食欲")
                    {
                        this.SI_NoAppetite = 1;
                    }
                    else if (str == "9. 持续便秘")
                    {
                        this.SI_Constipation = 1;
                    }
                    else if (str == "10. 感到头晕")
                    {
                        this.SI_Dizzy = 1;
                    }
                    else if (str == "11. 头痛")
                    {
                        this.SI_Headache = 1;
                    }
                    else if (str == "12. 其他")
                    {
                        this.SI_Other = 1;
                    }
                    else if (str == "13. 没有相关症状")
                    {
                        this.SI_NoSymptoms = 1;
                    }
                   
                }
                this.SI_IsJoin = si.SI_IsJoin == 0? "不参加":"参加";
                this.SI_WaterInput = si.SI_WaterInput;
                this.SI_CareInfo = si.SI_CareInfo;
            }
        }
    }
}
