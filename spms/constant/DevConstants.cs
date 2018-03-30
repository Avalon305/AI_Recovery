using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.constant
{
    class DevConstants
    {
        /// <summary>
        /// 计时器有效
        /// </summary>
        public static byte TIMER_VALID = 0x01;
        /// <summary>
        /// 计时器无效
        /// </summary>
        public static byte TIMER_INVALID = 0x02;
        /// <summary>
        /// 正计时
        /// </summary>
        public static byte COUNT = 0x00;
        /// <summary>
        /// 倒计时
        /// </summary>
        public static byte COUNT_DOWN = 0x01;
    }
}
