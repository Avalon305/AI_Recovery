using spms.constant;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.protocol
{
    class MakerTCPFrame
    {
        private MakerTCPFrame() { }

        private static MakerTCPFrame instance = new MakerTCPFrame();
        public static MakerTCPFrame GetInstance()
        {
            return instance;
        }

        public byte[] PackData(MsgId msgId,Int16 serialNo, string terminalId, byte[] data)
        {
            int pos = 0;
            int data_len = data.Length;
            byte[] arr = new byte[15 + data_len];

            arr[0] = 0x7E;
            arr[1] = Convert.ToByte(((int)msgId) & 0x00FF);//消息ID高字节
            arr[2] = Convert.ToByte((((int)msgId) & 0xFF00) >> 8);//消息ID，低字节
            // 消息体长度
            arr[3] = Convert.ToByte(data_len & 0x00FF);
            arr[4] = Convert.ToByte((data_len & 0xFF00) >> 8);
            //终端号码
            terminalId.PadLeft(12, '0');
            arr[5] = Convert.ToByte(terminalId.Substring(0 * 2, 2), 16);
            arr[6] = Convert.ToByte(terminalId.Substring(1 * 2, 2), 16);
            arr[7] = Convert.ToByte(terminalId.Substring(2 * 2, 2), 16);
            arr[8] = Convert.ToByte(terminalId.Substring(3 * 2, 2), 16);
            arr[9] = Convert.ToByte(terminalId.Substring(4 * 2, 2), 16);
            arr[10] = Convert.ToByte(terminalId.Substring(5 * 2, 2), 16);
            // 消息流水号
            arr[11] = Convert.ToByte(serialNo & 0x00FF);//流水号高字节
            arr[12] = Convert.ToByte((serialNo & 0xFF00) >> 8);//流水号，低字节
            //数据体填充
            Array.Copy(data, 0, arr, 13, data_len);
            pos += 12 + data_len;
            // 校验码
            arr[++pos] = ProtocolUtil.XorByByte(arr,1, 12 + data_len) ;
            //标识位置
            arr[++pos] = 0x7E;



            return ProtocolUtil.Transfer(arr);
        }

        /// <summary>
        /// 组帧，通用应答
        /// </summary>
        /// <param name="serialNo">流水号</param>
        /// <param name="msgId">回应的请求消息ID</param>
        /// <param name="respType"></param>
        /// <returns></returns>
        public byte[] Make8001Frame(Int16 serialNo, MsgId msgId, CommResponse respType )
        {
            byte[] arr = new byte[5];
            arr[0] = Convert.ToByte(serialNo & 0x00FF);//流水号高字节
            arr[1] = Convert.ToByte((serialNo & 0xFF00) >> 8);//流水号，低字节
            arr[2] = Convert.ToByte(((int)msgId) & 0x00FF);//消息ID高字节
            arr[3] = Convert.ToByte((((int)msgId) & 0xFF00) >> 8);//消息ID，低字节
            arr[4] = (byte)respType;
            return arr;
        }
        /// <summary>
        /// 响应处方
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="msgId"></param>
        /// <param name="respType"></param>
        /// <returns></returns>
        public byte[] Make8008Frame(Int16 serialNo, MsgId msgId, CommResponse respType)
        {  
            //TODO 备忘
            string notes = "别忘了吃药";
            int len = notes.Length * 2;
            byte[] arr = new byte[66 + len] ;

            return arr;
        }
        public byte[] Make0001Frame()
        {
            return PackData(MsgId.X0002, 1, "123456789012", new byte[0]);
        }

    }
}
