using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.util
{
    class ProtocolUtil
    {   //拼装u盘协议
        public static byte[] packData(byte[] cmd, byte[] data)
        {   
            int len = data.Length;
            byte[] result = new byte[len + 6];
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
            byte xor = XorByByte(result);
            result[result.Length - 2] = xor;
            //5.协议尾
            result[result.Length - 1] = 0xCC;

            return result;


        }
        //将byte数组变为16进制字符串
        public static string ByteToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2} ", InByte);//0指定参数，x表示16进制  2表示用0填充
            }
            return StringOut;
        }
        //异或校验
        public static byte XorByByte(byte[] bytes)
        {
            byte temp = bytes[1];
            for (int i = 2; i < bytes.Length; i++)
            {
                temp ^= bytes[i];
            }


            return temp;
        }



    }
}
