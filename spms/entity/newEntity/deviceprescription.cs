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
    public class DevicePrescription
    {
        //状态常量   1完成  0 未完成
        public static byte? UNDO = 0;
        public static byte? DOWN = 1;

        // 主键 自增
        [Key]
        public int Pk_dp_id { get; set; }
        // 关联的单一训练信息ID
        public int Fk_ti_id { get; set; }
        // 关联的单一设备类型ID
        public int Fk_ds_id { get; set; }
        // 用户运动模式 0：计数模式，1：计时模式
        public int Sport_mode { get; set; }
        // 设备训练模式 0康复模式，1主被动模式,2被动模式
        public int Device_mode { get; set; }
        // 完成状态 1完成,0未完成
        public Byte? Dp_status { get; set; }
        // 医生指示建议
        public string Dp_memo { get; set; }
        // 移乘方式 0自已，1照看，2完全失能
        public int Dp_moveway { get; set; }
        // 目标运动时间
        public int Dp_timecount { get; set; }
        // 目标运动个数
        public int Dp_target_num { get; set; }
        // 座位高度
        public int? Seat_height { get; set; }
        // 靠背距离
        public int? Backrest_distance { get; set; }
        // 踏板位置
        public int? Footboard_distance { get; set; }
        // 杠杆角度
        public double? Lever_angle { get; set; }
        // 额外属性
        public string Extra_setting { get; set; }
        // 前方限制
        public int? Front_limit { get; set; }
        // 后方限制
        public int? Back_limit { get; set; }
        // 顺向力
        public double? Consequent_force { get; set; }
        // 反向力
        public double? Reverse_force { get; set; }
        // 运动速度等级
        public int? Speed_rank { get; set; }
        // 有氧设备功率
        public double? Power { get; set; }
        // 创建时间
        public DateTime? Gmt_create { get; set; }
        // 修改时间
        public DateTime? Gmt_modified { get; set; }
    }
}
