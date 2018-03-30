using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.constant
{
    /// <summary>
    /// 计时器常量类
    /// </summary>
    class DevConstants
    {
        /// <summary>
        /// 计时器有效
        /// </summary>
        public static readonly byte TIMER_VALID = 0x00;
        /// <summary>
        /// 计时器无效
        /// </summary>
        public static readonly byte TIMER_INVALID =0x01;
        /// <summary>
        /// 正计时
        /// </summary>
        public static readonly byte COUNT_FORWARD = 0x00;
        /// <summary>
        /// 倒计时
        /// </summary>
        public static readonly byte COUNT_REVERSE = 0x01;

 
    }
}
