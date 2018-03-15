using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity
{
    //训练信息
    [Table("bdl_traininfo")]
    public class TrainInfo
    {
        //主键 自增
        [Key]
        public int Pk_TI_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //关联的用户ID
        public int FK_User_Id { get; set; }
    }
    //单一设备处方
    [Table("bdl_deviceprescription")]
    public class DevicePrescription
    {
        //状态常量 1做了  0没做
        public static byte? UNDO = 0;
        public static byte? DOWN = 1;
        //主键 自增
        [Key]
        public int Pk_DP_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //关联的单一训练信息ID
        public int Fk_TI_Id { get; set; }
        //关联的单一设备类型ID，通过这个获得设备name
        public int Fk_DS_Id { get; set; }
        //状态
        public Byte? dp_status { get; set; }
        //注意点，指示
        public string DP_Memo { get; set; }

        //设备属性，第一个是设备名，后面可以是多个属性，用*分割，格式：设备名*属性-属性值*[属性-属性值*]
        public string DP_Attrs { get; set; }
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

        public override string ToString()
        {
            return $"{nameof(Pk_DP_Id)}: {Pk_DP_Id}, {nameof(Gmt_Create)}: {Gmt_Create}, {nameof(Gmt_Modified)}: {Gmt_Modified}, {nameof(Fk_TI_Id)}: {Fk_TI_Id}, {nameof(Fk_DS_Id)}: {Fk_DS_Id}, {nameof(dp_status)}: {dp_status}, {nameof(DP_Memo)}: {DP_Memo}, {nameof(DP_Attrs)}: {DP_Attrs}, {nameof(dp_groupcount)}: {dp_groupcount}, {nameof(dp_groupnum)}: {dp_groupnum}, {nameof(dp_relaxtime)}: {dp_relaxtime}, {nameof(dp_moveway)}: {dp_moveway}, {nameof(dp_weight)}: {dp_weight}";
        }
    }
    //设备处方的训练结果  一对一
    [Table("bdl_prescriptionresult")]
    public class PrescriptionResult
    {
        //主键 自增
        [Key]
        public int Pk_PR_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //设备处方ID
        public int Fk_DP_Id { get; set; }
        //自觉运动强度,1-10代表轻松->剧烈
        public int PR_SportStrength { get; set; }
        //第一个时间
        public double PR_Time1 { get; set; }
        //第二个时间
        public double PR_Time2 { get; set; }
        //距离
        public int PR_Distance { get; set; }
        //总工作量
        public int PR_CountWorkQuantity { get; set; }
        //热量
        public double PR_Cal { get; set; }
        //指数
        public double PR_Index { get; set; }
        //完成组数
        public int PR_FinishGroup { get; set; }
        //时机，姿势，评价
        public string PR_Evaluate { get; set; }
        //注意点
        public string PR_AttentionPoint { get; set; }
        //病人感想
        public string PR_UserThoughts { get; set; }
        //备忘
        public string PR_Memo { get; set; }

        public override string ToString()
        {
            return $"{nameof(Pk_PR_Id)}: {Pk_PR_Id}, {nameof(Gmt_Create)}: {Gmt_Create}, {nameof(Gmt_Modified)}: {Gmt_Modified}, {nameof(Fk_DP_Id)}: {Fk_DP_Id}, {nameof(PR_SportStrength)}: {PR_SportStrength}, {nameof(PR_Time1)}: {PR_Time1}, {nameof(PR_Time2)}: {PR_Time2}, {nameof(PR_Distance)}: {PR_Distance}, {nameof(PR_CountWorkQuantity)}: {PR_CountWorkQuantity}, {nameof(PR_Cal)}: {PR_Cal}, {nameof(PR_Index)}: {PR_Index}, {nameof(PR_FinishGroup)}: {PR_FinishGroup}, {nameof(PR_Evaluate)}: {PR_Evaluate}, {nameof(PR_AttentionPoint)}: {PR_AttentionPoint}, {nameof(PR_UserThoughts)}: {PR_UserThoughts}, {nameof(PR_Memo)}: {PR_Memo}";
        }
    }

        //设备系列
        [Table("bdl_deviceset")]
    public class DeviceSet {
        //主键 自增
        [Key]
        public int Pk_DSet_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //系列名称
        public string DSet_Name { get; set; }
    }
    //设备类型
    [Table("bdl_devicesort")]
    public class DeviceSort
    {
        [Key]
        public int Pk_DS_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //设备名称
        public string DS_name { get; set; }
        //所属的设备系列ID
        public int Fk_DSet_Id { get; set; } 
    }

}
