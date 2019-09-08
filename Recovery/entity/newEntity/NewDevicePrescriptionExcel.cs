using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.entity.newEntity
{
    public class NewDevicePrescriptionExcel
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
        //训练模式
        public int device_mode { get; set; }
    }
}
