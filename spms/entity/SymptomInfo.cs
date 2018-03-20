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
        //训练信息id
        public int Pk_TI_Id { get; set; }
        //高血压（康复前）
        public string SI_Pre_HighPressure { get; set; }
        //低血压（康复前）
        public string SI_Pre_LowPressure { get; set; }
        //心率（康复前）
        public string SI_Pre_HeartRate { get; set; }
        //体温（康复前）
        public string SI_Pre_AnimalHeat { get; set; }
        //脉搏（康复前）
        public int SI_Pre_Pulse { get; set; }
        //高血压（康复后）
        public string SI_Suf_HighPressure { get; set; }
        //低血压（康复后）
        public string SI_Suf_LowPressure { get; set; }
        //心率（康复后）
        public string SI_Suf_HeartRate { get; set; }
        //体温（康复后）
        public string SI_Suf_AnimalHeat { get; set; }
        //脉搏（康复后）
        public int SI_Suf_Pulse { get; set; }

        public override string ToString()
        {
            return $"{nameof(Pk_SI_Id)}: {Pk_SI_Id}, {nameof(Gmt_Create)}: {Gmt_Create}, {nameof(Gmt_Modified)}: {Gmt_Modified}, {nameof(Fk_User_Id)}: {Fk_User_Id}, {nameof(SI_IsJoin)}: {SI_IsJoin}, {nameof(SI_WaterInput)}: {SI_WaterInput}, {nameof(SI_CareInfo)}: {SI_CareInfo}, {nameof(SI_Inquiry)}: {SI_Inquiry}, {nameof(Pk_TI_Id)}: {Pk_TI_Id}, {nameof(SI_Pre_HighPressure)}: {SI_Pre_HighPressure}, {nameof(SI_Pre_LowPressure)}: {SI_Pre_LowPressure}, {nameof(SI_Pre_HeartRate)}: {SI_Pre_HeartRate}, {nameof(SI_Pre_AnimalHeat)}: {SI_Pre_AnimalHeat}, {nameof(SI_Pre_Pulse)}: {SI_Pre_Pulse}, {nameof(SI_Suf_HighPressure)}: {SI_Suf_HighPressure}, {nameof(SI_Suf_LowPressure)}: {SI_Suf_LowPressure}, {nameof(SI_Suf_HeartRate)}: {SI_Suf_HeartRate}, {nameof(SI_Suf_AnimalHeat)}: {SI_Suf_AnimalHeat}, {nameof(SI_Suf_Pulse)}: {SI_Suf_Pulse}";
        }
    }
}
