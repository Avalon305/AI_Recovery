using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity
{
    //症状实体
    [Table("bdl_symptominfo")]
    public class SymptomInfo
    {
        //参加常量，0代表不参加，1参加
        public static byte? NO = 0;
        public static byte? YES = 1;

        //主键 自增  SI为SymptomInfo缩写
        [Key]
        public int Pk_SI_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //症状所属的用户ID
        public int Fk_User_Id { get; set; }
        //是否参加
        public Byte? SI_IsJoin { get; set; }
        //饮水量
        public string SI_WaterInput { get; set; }
        //看护记录
        public string SI_CareInfo { get; set; }
        //问诊票
        public string SI_Inquiry { get; set; }
    }
    //症状信息关联子表
    [Table("bdl_symptominfochild")]
    class SymptomInfoChild
    {
        //状态常量，0代表之前，1代表之后
        public static byte? STATUS_BEFORE = 0;
        public static byte? STATUS_AFTER = 1;
        //主键 自增   
        [Key]
        public int Pk_SIC_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //症状信息所属的用户ID
        public int Fk_SI_Id { get; set; }
        //状态
        public Byte? Status { get; set; }
       
        //高血压
        public string SIC_HighPressure { get; set; }
        //低血压
        public string SIC_LowPressure { get; set; }
        //心率
        public string SIC_HeartRate { get; set; }
        //体温
        public string SIC_AnimalHeat { get; set; }
        //脉搏
        public int SIC_Pulse { get; set; }
        
    }
}
