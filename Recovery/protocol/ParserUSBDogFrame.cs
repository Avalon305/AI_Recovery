using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recovery.constant;
using Recovery.util;

namespace Recovery.protocol
{
    class ParserUSBDogFrame : IParserFrame
    {
        public void Parser(ref object result,byte[] source)
        {
            //---- 解析逻辑
            //解析出数据体来，赋值给result
            //取出数据长度
            string dataLenStr = source[2].ToString("x2") + source[3].ToString("x2");
            Int32 data_len = Convert.ToInt32(dataLenStr, 16);
            byte[] temp = new byte[data_len];
            Array.Copy(source, 4, temp, 0, data_len);
            result = AesUtil.Decrypt(temp, ProtocolConstant.USB_DOG_PASSWORD);

        }
    }
}
