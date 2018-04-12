using NLog;
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
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MakerTCPFrame() { }

        private static MakerTCPFrame instance = new MakerTCPFrame();
        public static MakerTCPFrame GetInstance()
        {
            return instance;
        }

        public byte[] PackData(MsgId msgId, string terminalId, byte[] data)
        {
            int pos = 0;
            int data_len = data.Length;
            byte[] arr = new byte[15 + data_len];

            arr[0] = 0x7E;
            arr[1] = Convert.ToByte((((int)msgId) & 0xFF00) >> 8);//消息ID，低字节
            arr[2] = Convert.ToByte(((int)msgId) & 0x00FF);//消息ID高字节
            // 消息体长度
            arr[3] = Convert.ToByte((data_len & 0xFF00) >> 8);
            arr[4] = Convert.ToByte(data_len & 0x00FF);
            //终端号码
            terminalId.PadLeft(12, '0');
            arr[5] = Convert.ToByte(terminalId.Substring(0 * 2, 2), 16);
            arr[6] = Convert.ToByte(terminalId.Substring(1 * 2, 2), 16);
            arr[7] = Convert.ToByte(terminalId.Substring(2 * 2, 2), 16);
            arr[8] = Convert.ToByte(terminalId.Substring(3 * 2, 2), 16);
            arr[9] = Convert.ToByte(terminalId.Substring(4 * 2, 2), 16);
            arr[10] = Convert.ToByte(terminalId.Substring(5 * 2, 2), 16);
            // 消息流水号
            Int16 serialNo = ProtocolUtil.GetSerialNo();
            arr[11] = Convert.ToByte((serialNo & 0xFF00) >> 8);//流水号，低字节
            arr[12] = Convert.ToByte(serialNo & 0x00FF);//流水号高字节
            //数据体填充
            Array.Copy(data, 0, arr, 13, data_len);
            pos += 12 + data_len;
            // 校验码
            arr[++pos] = ProtocolUtil.XorByByte(arr, 1, 12 + data_len);
            //标识位置
            arr[++pos] = 0x7E;



            return ProtocolUtil.Transfer(arr);
        }

        /// <summary>
        /// 组帧，通用应答
        /// </summary>
        /// <param name="serialNo">流水号，这个流水号是Host字节序</param>
        /// <param name="msgId">回应的请求消息ID</param>
        /// <param name="respType"></param>
        /// <returns></returns>
        public byte[] Make8001Frame(Int16 serialNo, MsgId msgId, CommResponse respType)
        {
            byte[] arr = new byte[5];
            arr[0] = Convert.ToByte((serialNo & 0xFF00) >> 8);//流水号，低字节
            arr[1] = Convert.ToByte(serialNo & 0x00FF);//流水号高字节
            arr[2] = Convert.ToByte((((int)msgId) & 0xFF00) >> 8);//消息ID，低字节
            arr[3] = Convert.ToByte(((int)msgId) & 0x00FF);//消息ID高字节
            arr[4] = (byte)respType;
            return arr;
        }


        /// <summary>
        /// 响应照片数据
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        public byte[] Make8007Frame(string idcard)
        {
            UserPicService picService = new UserPicService();
            try
            {
                byte[] picData = picService.GetPictureData(idcard);
                byte[] arr = new byte[picData.Length];
                Array.Copy(picData, 0, arr, 0, picData.Length);
                 return arr;
            }
            catch(Exception ex)
            {
                logger.Error("请求照片信息时发生异常：" + ex.ToString());
                return new byte[1];
            }
      
        }

        /// <summary>
        /// 组帧，用户信息
        /// </summary>
        /// <returns></returns>
        public byte[] Make800AFrame(string idcard)
        {
            UserService userService = new UserService();
            User u = userService.GetByIdCard(idcard);

            if (u == null)
            {
                logger.Warn("请求用户信息时，用户不存在，用户ID：" + idcard);
                return new byte[1];
            }

            byte[] group = Encoding.GetEncoding("GBK").GetBytes(u.User_GroupName);
            byte[] initCare = Encoding.GetEncoding("GBK").GetBytes(u.User_InitCare);
            byte[] nowCare = Encoding.GetEncoding("GBK").GetBytes(u.User_Nowcare);
            byte[] illnessName = Encoding.GetEncoding("GBK").GetBytes(u.User_IllnessName);
            byte[] disabilitiesName = Encoding.GetEncoding("GBK").GetBytes(u.User_PhysicalDisabilities);
            int pos = 89;

            byte[] arr = new byte[89 + group.Length + initCare.Length + nowCare.Length + illnessName.Length + disabilitiesName.Length + 5];
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
            arr[84] = (byte)(u.User_Sex ^ 1);
            //出生年月 4字节
            string birthStr = String.Format("{0:yyyyMMdd} ", u.User_Birth);

            byte[] birth = ProtocolUtil.StringToBcd(birthStr);
            Array.Copy(birth, 0, arr, 85, birth.Length);
            //组 128字节
            Array.Copy(group, 0, arr, 89, group.Length);
            arr[89 + group.Length] = 0x00;
            pos += group.Length + 1;
            //开始时的要护理程度 128 字节
            Array.Copy(initCare, 0, arr, pos, initCare.Length);
            arr[pos + initCare.Length] = 0x00;
            pos += initCare.Length + 1;
            //现在的要护理程度
            Array.Copy(nowCare, 0, arr, pos, nowCare.Length);
            arr[pos + nowCare.Length] = 0x00;
            pos += nowCare.Length + 1;
            //诊断名称
            Array.Copy(illnessName, 0, arr, pos, illnessName.Length);
            arr[pos + illnessName.Length] = 0x00;
            pos += illnessName.Length + 1;
            //伤残名称
            Array.Copy(disabilitiesName, 0, arr, pos, disabilitiesName.Length);
            arr[pos + disabilitiesName.Length] = 0x00;
            pos += disabilitiesName.Length + 1;
            return arr;

        }


        /// <summary>
        /// 胸部推举机处方组帧应答
        /// </summary>
        /// <param name="idcard"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public byte[] Make8008Frame(string idcard, DeviceType deviceType)
        {
            TrainService trainService =  new TrainService();
            byte[] arr;

            //获取处方信息,已完成的，先判断这个
            var down = trainService.GetDevicePrescriptionByIdCardDeviceType(idcard, deviceType, (byte)DevicePrescription.DOWN);

            if (down != null)
            {
                var undoList = trainService.ListUndoDevicePrescriptionByUserId(idcard);
                arr = new byte[2 + undoList.Count];

                arr[0] = (byte)deviceType;
                arr[1] = 0x02;//已完成该项训练计划
                int pos = 2;
                foreach (var a in undoList)//未完成的顺便返回
                {
                    arr[pos] = (byte)a.Fk_DS_Id;
                    pos++;
                }
                return arr;
            }


            //获取处方信息
            var prescription = trainService.GetDevicePrescriptionByIdCardDeviceType(idcard, deviceType,(byte)DevicePrescription.UNDO);
            if (prescription != null)
            {
               logger.Info("用户ID为："+idcard+";deviceType为:"+deviceType.ToString()+"；的获取到处方信息：" + prescription.ToString());
            }
         
            
            UserService userService = new UserService();
            var u = userService.GetByIdCard(idcard);

            if (u == null)
            {
                arr = new byte[2];
                arr[0] = (byte)deviceType;
                arr[1] = 0x01;//无效用户
                return arr;
            }
            if (prescription == null)
            {
                var undoList = trainService.ListUndoDevicePrescriptionByUserId(idcard);
                arr = new byte[2 + undoList.Count];

                arr[0] = (byte)deviceType;
                arr[1] = 0x03;//无该项训练计划
                int pos = 2;
                foreach(var a in undoList)//未完成的顺便返回
                {
                    arr[pos] = (byte)a.Fk_DS_Id;
                    pos++;
                }
                return arr;
            }
            

            arr = new byte[87];
            arr[0] = (byte)deviceType;
            arr[1] = 0x00;//有训练内容
            // 姓名20个字节
            byte[] userName = Encoding.GetEncoding("GBK").GetBytes(u.User_Name);
            Array.Copy(userName, 0, arr, 2, userName.Length);
            //姓名拼音32个字节
            byte[] namePinYin = Encoding.GetEncoding("GBK").GetBytes(u.User_Namepinyin);
            Array.Copy(namePinYin, 0, arr, 22, namePinYin.Length);
            //移乘方法
            arr[54] = (byte)prescription.dp_moveway;
            //训练组数
            arr[55] = (byte)prescription.dp_groupcount;
            //每组个数
            arr[56] = (byte)prescription.dp_groupnum;
            //休息时间，秒
            arr[57] = (byte)prescription.dp_relaxtime;

            //计数器是否有效，0有效，1无效，数据库没有该字段，暂时无效
            arr[58] = (byte)prescription.dp_timer;
            string[] attrs = prescription.DP_Attrs.Split('*');

            //计数方式。正计时。倒计时
            arr[59] = prescription.dp_timetype;

            //计时时间，分钟
            arr[60] = prescription.dp_timecount;

            //61 备忘 17 字节
            string notes = prescription.DP_Memo;
            if (notes == null)
            {
                notes = "";
            }
            byte[] noteBytes = Encoding.GetEncoding("GBK").GetBytes(notes);
            Array.Copy(noteBytes, 0, arr, 61, noteBytes.Length);

            // 砝码移动距离1/10厘米,存的单位是厘米
            Int16 move = (Int16)(prescription.dp_movedistance * 10);
            arr[78] = Convert.ToByte((move & 0xFF00) >> 8);//低字节
            arr[79] = Convert.ToByte(move & 0x00FF);//高字节

            //砝码重量 单位是kg,发送的是0.1kg
            Int16 weight = (Int16)(prescription.dp_weight * 10);
            arr[80] = Convert.ToByte((weight & 0xFF00) >> 8);//低字节
            arr[81] = Convert.ToByte(weight & 0x00FF);//高字节
                                                      //辅助砝码重量 数据库单位kg，发送的是0.1kg
            Int16 helpWeight = (Int16)(weight % 25);
            arr[82] = Convert.ToByte((helpWeight & 0xFF00) >> 8);//低字节
            arr[83] = Convert.ToByte(helpWeight & 0x00FF);//高字节

            HandleDeviceType(ref arr, deviceType, attrs);
            return arr;
        }

        /// <summary>
        /// 处理各设备类型的个性属性
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="deviceType"></param>
        /// <param name="attrs"></param>
        private void HandleDeviceType(ref byte[] arr, DeviceType deviceType, string[] attrs)
        {
            switch (deviceType)
            {
                case DeviceType.X01:
                    arr[84] = byte.Parse(attrs[0]);//动臂位置 1-5
                    arr[85] = byte.Parse(attrs[1]);//座椅位置1-9
                    arr[86] = 0x00;
                    break;
                case DeviceType.X02:
                    arr[84] = byte.Parse(attrs[0]);//动臂位置 1-8
                    arr[85] = 0x00;
                    arr[86] = 0x00;
                    break;
                case DeviceType.X03:
                    arr[84] = byte.Parse(attrs[0]);//动臂位置1 1-5
                    arr[85] = byte.Parse(attrs[1]);//动臂位置2 1-5
                    arr[86] = byte.Parse(attrs[2]);//座椅位置 1-9
                    break;
                case DeviceType.X04:
                    arr[84] = byte.Parse(attrs[0]);//动臂位置1 1-4
                    arr[85] = byte.Parse(attrs[1]);//动臂位置2 1-4
                    arr[86] = 0x00;
                    break;
                case DeviceType.X05:
                    arr[84] = byte.Parse(attrs[0]);//胸垫位置 1-6
                    arr[85] = byte.Parse(attrs[1]);//座椅位置 1-9
                    arr[86] = 0x00;
                    break;
                case DeviceType.X06:
                    arr[84] = byte.Parse(attrs[0]);//胸垫位置 1-6
                    arr[85] = 0x00;//座椅位置 1-9
                    arr[86] = 0x00;
                    break;
            }
        }


    }
}
