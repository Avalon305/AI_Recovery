using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}
