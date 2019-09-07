using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recovery.entity;
using Recovery.util;

namespace Recovery.view.dto
{
    class PhysicaleDTO
    {
        public int? ID { get; set; }
        //数据创建时间
        public DateTime? Gmt_Create { get; set; }

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

        /// <summary>
        /// 从字符串中截取第一个参数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string SubstringParams(string value)
        {
            int startIndex = value.IndexOf(",") + 1;
            int endIndex = value.IndexOf(",", startIndex);
            string result = value.Substring(startIndex, endIndex - startIndex);

            if (result.Contains("param"))
            {
                return LanguageUtils.ConvertLanguage("未填写", "Unfilled");
            }
            else
            {
                return result;
            }
        }

        private string isEmpty(string val)
        {
            if (val == null || val.Trim() == "")
            {
                return LanguageUtils.ConvertLanguage("未填写", "Unfilled");
            }
            else
            {
                return val;
            }
        }


        public PhysicaleDTO(PhysicalPower physicalPower)
        {
            Console.WriteLine(physicalPower.PP_High);
            if (physicalPower.PP_High.IndexOf(",") == -1)
            {
                Console.WriteLine(physicalPower.ToString());
                Console.WriteLine("体力报告的数据格式不对，正常的格式应为以','结尾的数据格式");
                return;
            }
            this.ID = physicalPower.Pk_PP_Id;
            this.Gmt_Create = physicalPower.Gmt_Create.Value;
            this.PP_High = SubstringParams(physicalPower.PP_High).ToString();
            this.PP_Weight = SubstringParams(physicalPower.PP_Weight).ToString();
            this.PP_EyeOpenStand = SubstringParams(physicalPower.PP_EyeOpenStand).ToString();
            this.PP_TimeUpGo = SubstringParams(physicalPower.PP_TimeUpGo).ToString();
            this.PP_UserMemo = isEmpty(physicalPower.PP_UserMemo);
            this.PP_WorkerMemo = isEmpty(physicalPower.PP_WorkerMemo);
        }

        public override string ToString()
        {
            return $"{nameof(ID)}: {ID}, {nameof(Gmt_Create)}: {Gmt_Create}, {nameof(PP_High)}: {PP_High}, {nameof(PP_Weight)}: {PP_Weight}, {nameof(PP_EyeOpenStand)}: {PP_EyeOpenStand}, {nameof(PP_TimeUpGo)}: {PP_TimeUpGo}, {nameof(PP_UserMemo)}: {PP_UserMemo}, {nameof(PP_WorkerMemo)}: {PP_WorkerMemo}";
        }
    }
}