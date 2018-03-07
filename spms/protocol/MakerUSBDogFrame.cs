using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.protocol
{
    class MakerUSBDogFrame : IMakerFrame
    {
        /// <summary>
        /// 组装U盘参数
        /// </summary>
        /// <param name="result"></param>
        /// <param name="cmd"></param>
        /// <param name="data"></param>
        public void PackData(ref byte[] result, byte[] cmd, byte[] data)
        {
            int len = data.Length;
            result = new byte[len + 6];
            result[0] = 0xAA;//1.帧首
            result[1] = cmd[0];//2.命令
            string hex = len.ToString("x4");//int转成16进制字符串
            result[2] = Convert.ToByte(hex.Substring(0, 2), 16);//16进制字符串(数字化)转字节 3.长度
            result[3] = Convert.ToByte(hex.Substring(2, 2), 16);
            for (int i = 0; i < data.Length; i++)
            {
                result[4 + i] = data[i];
            }
            //4.异或校检
            byte xor = ProtocolUtil.XorByByte(result, 1, 3 + len);
            result[result.Length - 2] = xor;
            //5.协议尾
            result[result.Length - 1] = 0xCC;
        }
    }
}
