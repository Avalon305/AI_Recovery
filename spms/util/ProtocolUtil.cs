using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.util
{
    /// <summary>
    /// 协议解析通用工具类
    /// </summary>
    class ProtocolUtil
    {
        public static string BytesToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2} ", InByte);
            }
            return StringOut;
        }

        /// <summary>
        /// 判断两个数组是否相等
        /// </summary>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <returns></returns>
        public static Boolean ArrayEqual(byte[] arr1,byte[] arr2)
        {
            if (arr1.Length != arr2.Length)
            {
                return false;
            }
            for(int i = 0; i < arr2.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 按字节异或
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte XorByByte(byte[] bytes)
        {
            byte temp = bytes[0];
            for (int i = 1; i < bytes.Length; i++)
            {
                temp ^= bytes[i];
            }
            return temp;
        }
        /// <summary>
        /// 按字节异或
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="beginIndex"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte XorByByte(byte[] bytes, int beginIndex, int len)
        {
            byte temp = bytes[beginIndex];
            for (int i = beginIndex + 1; i < beginIndex + len; i++)
            {
                temp ^= bytes[i];
            }
            return temp;
        }
    }
}
