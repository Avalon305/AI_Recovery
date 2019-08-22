using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity.newEntity
{
    /// <summary>
    /// 单一设备处方表
    /// </summary>
    [Serializable]
    [Table("bdl_deviceprescription")]
    public class NewDevicePrescription
    {
        //状态常量   1完成  0 未完成
        public static byte? UNDO = 0;
        public static byte? DOWN = 1;

        // 主键 自增
        [Key]
        public long Pk_dp_id { get; set; }
        // 关联的单一训练信息ID
        public long Fk_ti_id { get; set; }
        // 关联的单一设备类型ID
        public long Fk_ds_id { get; set; }
        // 设备训练模式 0康复模式，1主被动模式,2被动模式
        public int? Device_mode { get; set; }
        // 完成状态 1完成,0未完成
        public int? Dp_status { get; set; }
        // 移乘方式 0自已，1照看，2完全失能
        public int? Dp_moveway { get; set; }
		// 医生指示建议
		public string Dp_memo { get; set; }
		// 运动速度等级
		public int? Speed_rank { get; set; }
		// 顺向力
		public double? Consequent_force { get; set; }
		// 反向力
		public double? Reverse_force { get; set; }
		//组数
		public int? Dp_groupcount { get; set; }
        //每组个数
        public int? Dp_groupnum { get; set; }
        //休息时间
        public int? Dp_relaxtime { get; set; }
        // 创建时间
        public DateTime? Gmt_create { get; set; }
        // 修改时间
        public DateTime? Gmt_modified { get; set; }
    }
}
