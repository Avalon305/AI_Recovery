using spms.constant;
using spms.entity;
using spms.service;
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
        /// <summary>
        /// 响应照片包数
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        public byte[] Make8006Frame(string idcard)
        {
            UserPicService picService = new UserPicService();
            Int16 count = picService.GetPicturePackCount(idcard);

            byte[] arr = new byte[2];
            arr[0] = Convert.ToByte(count & 0x00FF);//照片包数高字节
            arr[1] = Convert.ToByte((count & 0xFF00) >> 8);//照片包数，低字节
            return arr;
        }

        /// <summary>
        /// 响应照片数据
        /// </summary>
        /// <param name="packNum"></param>
        /// <param name="idcard"></param>
        /// <returns></returns>
        public byte[] Make8007Frame(Int16 packNum,string idcard)
        {
            byte[] arr = new byte[514];
            arr[0] = Convert.ToByte(packNum & 0x00FF);//包序号高字节
            arr[1] = Convert.ToByte((packNum & 0xFF00) >> 8);//包序号，低字节

            UserPicService picService = new UserPicService();
            byte[] picData = picService.GetPictureData(idcard, packNum);

            Array.Copy(picData, 0, arr, 2, picData.Length);
            return arr;
        }

        /// <summary>
        /// 组帧，用户信息
        /// </summary>
        /// <returns></returns>
        public byte[] Make800AFrame(string idcard)
        {
            UserService userService = new UserService();
            User u = userService.GetByIdCard(idcard);

            byte[] arr = new byte[730];
            //用户ID32个字节
            byte[] idBytes = Encoding.GetEncoding("GBK").GetBytes(idcard);
            Array.Copy(idBytes, 0, arr, 0, idBytes.Length);
            //姓名20个字节
            byte[] userName = Encoding.GetEncoding("GBK").GetBytes(u.User_Name);
            Array.Copy(userName, 0, arr, 32, userName.Length);
            //姓名拼音32个字节
            byte[] namePinYin = Encoding.GetEncoding("GBK").GetBytes(u.User_Namepinyin);
            Array.Copy(namePinYin, 0, arr, 52, namePinYin.Length);
            //性别，协议里0是男1是女，异或一下
            arr[85] = (byte)(u.User_Sex ^ 1);
            //出生年月 4字节
            string birthStr = String.Format("yyyyMMdd ", u.User_Birth);
            byte[] birth = ProtocolUtil.StringToBcd(birthStr);
            Array.Copy(birth, 0, arr, 86, birth.Length);
            //组 128字节
            byte[] group = Encoding.GetEncoding("GBK").GetBytes(u.User_GroupName);
            Array.Copy(group, 0, arr, 90, group.Length);
            //开始时的要护理程度 128 字节
            byte[] initCare = Encoding.GetEncoding("GBK").GetBytes(u.User_InitCare);
            Array.Copy(initCare, 0, arr, 218, initCare.Length);
            //现在的要护理程度
            byte[] nowCare = Encoding.GetEncoding("GBK").GetBytes(u.User_Nowcare);
            Array.Copy(nowCare, 0, arr, 346, nowCare.Length);
            //诊断名称
            byte[] illnessName = Encoding.GetEncoding("GBK").GetBytes(u.User_IllnessName);
            Array.Copy(illnessName, 0, arr, 474, illnessName.Length);
            //伤残名称
            byte[] disabilitiesName = Encoding.GetEncoding("GBK").GetBytes(u.User_PhysicalDisabilities);
            Array.Copy(disabilitiesName, 0, arr, 602, disabilitiesName.Length);
            return arr;

        }

    }
}
