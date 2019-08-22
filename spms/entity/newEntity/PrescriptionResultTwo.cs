using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity.newEntity
{
    /// <summary>
    /// 设备处方的训练结果
    /// </summary>
    [Serializable]
    [Table("bdl_prescriptionresult")]
    class PrescriptionResultTwo
    {
        // 主键 自增
        [Key]
        public long Pk_pr_id { get; set; }
        // 设备处方ID
        public long Fk_dp_id { get; set; }
        // 关联的单一设备类型ID
        public long Fk_ds_id { get; set; }
        // 用户绑定手环id
        public string Bind_id { get; set; }
        // 用户运动模式 0：计数模式，1：计时模式 废弃
        //public int? Sport_mode { get; set; }
        // 设备训练模式 0康复模式，1主被动模式,2被动模式
        public int? Device_mode { get; set; }
        // 顺向力
        public double? Consequent_force { get; set; }
        // 反向力
        public double? Reverse_force { get; set; }
        // 有氧设备功率
        public double? Power { get; set; }
        // 运动速度等级
        public int? Speed_rank { get; set; }
        // 完成运动个数
        public int? Finish_num { get; set; }
        // 完成运动时间 废弃
        public int? Finish_time { get; set; }
        // 训练总耗能
        public double? Energy { get; set; }
        //心率集合：运动过程实时心率集合，数据之间*分割
        public string Heart_rate_list { get; set; }
        //病人感想，分级选择
        public string pr_userthoughts { get; set; }
        // 创建时间
        public DateTime? Gmt_create { get; set; }
        // 修改时间
        public DateTime? Gmt_modified { get; set; }
    }
}
