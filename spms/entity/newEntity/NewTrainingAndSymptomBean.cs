using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity.newEntity
{
    public class NewTrainingAndSymptomBean
    {
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //训练时间
        public double PR_Finish_Time { get; set; }
        //热量
        public double PR_Energy { get; set; }
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
            return $"{nameof(PR_Energy)}: {PR_Energy}, {nameof(Gmt_Create)}: {Gmt_Create}, {nameof(PR_Finish_Time)}: {PR_Finish_Time}, {nameof(SI_Pre_HighPressure)}: {SI_Pre_HighPressure}, {nameof(SI_Pre_LowPressure)}: {SI_Pre_LowPressure}, {nameof(SI_Suf_HighPressure)}: {SI_Suf_HighPressure}, {nameof(SI_Suf_LowPressure)}: {SI_Suf_LowPressure}, {nameof(SI_WaterInput)}: {SI_WaterInput}, {nameof(SI_CareInfo)}: {SI_CareInfo}";
        }
    }
}
