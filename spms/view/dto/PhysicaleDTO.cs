using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using spms.entity;

namespace spms.view.dto
{
    class PhysicaleDTO
    {
        //数据创建时间
        public DateTime Gmt_Create { get; set; }

        // 身高
        public string PP_High { get; set; }

        //体重
        public string PP_Weight { get; set; }

        //睁眼是否单脚站立
        public string PP_EyeOpenStand { get; set; }

        //Time Up &amp; go
        public string PP_TimeUpGo { get; set; }

        //使用者感想
        public string PP_UserMemo { get; set; }

        //工作者感想
        public string PP_WorkerMemo { get; set; }


        public PhysicaleDTO(PhysicalPower physicalPower)
        {
            this.Gmt_Create = physicalPower.Gmt_Create.Value;
            this.PP_High = physicalPower.PP_High;
            this.PP_Weight = physicalPower.PP_Weight;
            this.PP_EyeOpenStand = physicalPower.PP_EyeOpenStand;
            this.PP_TimeUpGo = physicalPower.PP_TimeUpGo;
            this.PP_UserMemo = physicalPower.PP_UserMemo;
            this.PP_WorkerMemo = physicalPower.PP_WorkerMemo;
        }
    }
}