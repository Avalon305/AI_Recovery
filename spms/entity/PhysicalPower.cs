using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity
{
    //体力评价实体
    [Table("bdl_physicalpower")]
    public class PhysicalPower
    {
        //主键 自增
        [Key]
        public int Pk_PP_Id { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }
        //数据更新时间
        public DateTime? Gmt_Modified { get; set; }
        //关联的用户ID
        public int FK_user_Id { get; set; }
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
        //time&up go
        public string PP_TimeUpGo { get; set; }
        //5m步行-通常
        public string PP_Walk5MileGeneral { get; set; }
        //5m步行-最快
        public string PP_Walk5MileFast { get; set; }
        //10m步行，第一个字段是模式
        public string PP_Walk10Mile { get; set; }
        //6分钟步行
        public string PP_Walk6Minute { get; set; }
        //2分钟踏步
        public string PP_Step2Minute { get; set; }
        //2分钟抬腿
        public string PP_LegRaise2Minute { get; set; }
        //使用用者感想
        public string PP_UserMemo { get; set; }
        //工作人员感想
        public string PP_WorkerMemo { get; set; }

        public override string ToString()
        {
            return $"{nameof(Pk_PP_Id)}: {Pk_PP_Id}, {nameof(Gmt_Create)}: {Gmt_Create}, {nameof(Gmt_Modified)}: {Gmt_Modified}, {nameof(FK_user_Id)}: {FK_user_Id}, {nameof(PP_High)}: {PP_High}, {nameof(PP_Weight)}: {PP_Weight}, {nameof(PP_Grip)}: {PP_Grip}, {nameof(PP_EyeOpenStand)}: {PP_EyeOpenStand}, {nameof(PP_FunctionProtract)}: {PP_FunctionProtract}, {nameof(PP_SitandReach)}: {PP_SitandReach}, {nameof(PP_TimeUpGo)}: {PP_TimeUpGo}, {nameof(PP_Walk5MileGeneral)}: {PP_Walk5MileGeneral}, {nameof(PP_Walk5MileFast)}: {PP_Walk5MileFast}, {nameof(PP_Walk10Mile)}: {PP_Walk10Mile}, {nameof(PP_Walk6Minute)}: {PP_Walk6Minute}, {nameof(PP_Step2Minute)}: {PP_Step2Minute}, {nameof(PP_LegRaise2Minute)}: {PP_LegRaise2Minute}, {nameof(PP_UserMemo)}: {PP_UserMemo}, {nameof(PP_WorkerMemo)}: {PP_WorkerMemo}";
        }
    }
}
