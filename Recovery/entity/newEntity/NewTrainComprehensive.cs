using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.entity.newEntity
{
    public class NewTrainComprehensive
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
        //移乘方式
        public int dp_moveway { get; set; }
        //自觉运动强度,1-10代表轻松->剧烈
        public string PR_SportStrength { get; set; }
        //训练时间
        public double PR_finish_time { get; set; }
        //训练总耗能
        public double PR_Energy { get; set; }
        //完成组数
        public int PR_finish_num { get; set; }
        //病人感想
        public string PR_UserThoughts { get; set; }
    }
}
