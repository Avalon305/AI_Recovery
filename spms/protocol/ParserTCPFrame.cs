using NLog;
using spms.bean;
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
    class ParserTCPFrame
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public byte[] Parser(byte[] source)
        {
            byte[] response = new byte[0];

            byte[] buffer = ProtocolUtil.UnTransfer(source);
            MsgId msgId = ProtocolUtil.BytesToMsgId(buffer, 0);
            Int16 data_len = BitConverter.ToInt16(buffer, 2);
            string terminalId = ProtocolUtil.BcdToString(buffer, 4, 6);
            byte serialNo = buffer[10];

            byte xor = buffer[12 + data_len];
            //数据体
            byte[] data = new byte[data_len];

            Array.Copy(buffer, 12, data, 0, data_len);

            byte calcXor = ProtocolUtil.XorByByte(buffer, 0, 12 + data_len);

            TcpFrameBean frameBean = new TcpFrameBean();
            frameBean.DataBody = data;
            frameBean.SerialNo = serialNo;
            frameBean.TerminalId = terminalId;
            frameBean.MsgId = msgId;
            if (xor != calcXor)
            {
                frameBean.AppendErrorMsg( "校验码不符合预期");
                response = MakerTCPFrame.GetInstance().Make8001Frame((byte)(serialNo + 1), MsgId.X0001, CommResponse.MistakeMsg);
                return response;
            }

            switch (msgId)
            {
                case MsgId.X0001://设备通用应答
                    break;
                case MsgId.X0002://心跳OK
                    HandleHeartBeat(ref response, frameBean);
                    break;
                case MsgId.X0008://开始训练
                    HandleStartPrictice(ref response, frameBean);
                    break;
                case MsgId.X0009://训练结果上报
                    HandlePricticeResult(ref response, frameBean);
                    break;
                case MsgId.X000A://请求使用者信息
                    HandleRequestUserInfo(ref response, frameBean);
                    break;
                case MsgId.X0006://请求照片包数OK
                    HandleRequestImageCount(ref response, frameBean);
                    break;
                case MsgId.X0007://请求照片数据OK
                    HandleRequestImageData(ref response, frameBean);
                    break;
                default:
                    frameBean.AppendErrorMsg("未知的消息ID");
                    response = MakerTCPFrame.GetInstance().Make8001Frame((byte)(serialNo + 1), MsgId.X0001, CommResponse.UnSupport);
                    break;
            }

            return response;
        }
        /// <summary>
        /// 处理请求图片数据
        /// </summary>
        /// <param name="response"></param>
        /// <param name="frameBean"></param>
        private void HandleRequestImageData(ref byte[] response, TcpFrameBean frameBean)
        {
            byte[] body = frameBean.DataBody;
            string idcard = Encoding.GetEncoding("GBK").GetString(body, 0, 18);
            Int16 packNum = BitConverter.ToInt16(body, 32);

            byte[] data = MakerTCPFrame.GetInstance().Make8007Frame(packNum, idcard);
            logger.Error(ProtocolUtil.BytesToString(data));
            byte nextSerialNo = (byte)(frameBean.SerialNo + 1);
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X8007, nextSerialNo, frameBean.TerminalId, data);

        }

       /// <summary>
       /// 处理请求图片包数
       /// </summary>
       /// <param name="response"></param>
       /// <param name="frameBean"></param>
        private void HandleRequestImageCount(ref byte[] response, TcpFrameBean frameBean)
        {
            byte[] idBytes = frameBean.DataBody;
            string idcard = Encoding.GetEncoding("GBK").GetString(idBytes, 0, 18);
         

            byte[] data = MakerTCPFrame.GetInstance().Make8006Frame(idcard);
            Int16 nextSerialNo = (Int16)(frameBean.SerialNo + 1);
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X8006, nextSerialNo, frameBean.TerminalId, data);

        }

        /// <summary>
        /// 处理请求用户信息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="frameBean"></param>
        private void HandleRequestUserInfo(ref byte[] response, TcpFrameBean frameBean)
        {
            byte[] body = frameBean.DataBody;
            string idcard = Encoding.GetEncoding("GBK").GetString(body, 0, 18);
            byte[] data = MakerTCPFrame.GetInstance().Make800AFrame(idcard);
            logger.Error(ProtocolUtil.BytesToString(data));
            Int16 nextSerialNo = (Int16)(frameBean.SerialNo + 1);
            MakerTCPFrame.GetInstance().PackData(MsgId.X800A, nextSerialNo, frameBean.TerminalId, data);
        }
        /// <summary>
        /// 处理训练结果上报
        /// </summary>
        /// <param name="response"></param>
        /// <param name="frameBean"></param>
        private void HandlePricticeResult(ref byte[] response, TcpFrameBean frameBean)
        {
            ParserPricticeResult paser = new ParserPricticeResult();
            byte[] body = frameBean.DataBody;
            //设备类型
            DeviceType deviceType = (DeviceType)body[0];
           
            switch (deviceType)
            {
                case DeviceType.X01://胸部推举机
                    //TODO 设备应该上报用户ID
                    paser.PaserX01(body,"370111111111111115");
                    break;
                case DeviceType.X02:
                    break;
                case DeviceType.X03:
                    break;
                case DeviceType.X04:
                    break;
                case DeviceType.X05:
                    break;
                case DeviceType.X06:
                    break;
            }
            //数据上报响应通用应答
            byte[] data = MakerTCPFrame.GetInstance().Make8001Frame(frameBean.SerialNo, frameBean.MsgId, CommResponse.Success);
            byte nextSerialNo = (byte)(frameBean.SerialNo + 1);
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X8001, nextSerialNo, frameBean.TerminalId, data);

        }

        /// <summary>
        /// 处理请求处方信息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="frameBean"></param>
        public void HandleStartPrictice(ref byte[] response, TcpFrameBean frameBean)
        {
            var maker = new MakerTCPFrame.MakePrescription();

            byte[] data = frameBean.DataBody;
            string userId = Encoding.GetEncoding("GBK").GetString(data, 0, 18);
            DeviceType deviceType = (DeviceType)data[32];
            switch (deviceType)
            {
                case DeviceType.X01://胸部推举机
                    maker.Make8008Frame(userId, deviceType);
                    break;
                case DeviceType.X02:
                    break;
                case DeviceType.X03:
                    break;
                case DeviceType.X04:
                    break;
                case DeviceType.X05:
                    break;
                case DeviceType.X06:
                    break;
            }

        }
        /// <summary>
        /// 处理心跳
        /// </summary>
        /// <param name="response"></param>
        /// <param name="frameBean"></param>
        public void HandleHeartBeat(ref byte[] response,TcpFrameBean frameBean)
        {
            byte[] data = MakerTCPFrame.GetInstance().Make8001Frame(frameBean.SerialNo, frameBean.MsgId, CommResponse.Success);
            byte nextSerialNo = (byte)(frameBean.SerialNo + 1);
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X8001, nextSerialNo, frameBean.TerminalId, data);

        }

        /// <summary>
        /// 内部类，辅助解析各种设备上报
        /// </summary>
        class ParserPricticeResult
        {
            /// <summary>
            /// 胸部推举机上报解析
            /// </summary>
            /// <param name="body"></param>
            public void PaserX01(byte[] body, string idCard)
            {
                //运动强度
                byte strength = ProtocolUtil.BcdToInt(body[1]);
                //运动时间 1/100秒
                Int32 time = BitConverter.ToInt32(body, 2);
                //总移动距离 毫米
                Int32 distance = BitConverter.ToInt32(body, 6);
                //总功 1/100 焦耳
                Int32 energy = BitConverter.ToInt32(body, 10);
                //消耗热量 1/100 卡路里
                Int32 heat = BitConverter.ToInt32(body, 14);
                //指数标识 0 正数 1 负数
                //指数值 1/100
                Int32 singer = BitConverter.ToInt32(body, 19);
                singer = body[18] == 0x00 ? singer : -1 * singer;
                //动作节奏 0没问题 1 有些许问题 2 有问题
                byte rhythem = body[23];
                //使用者感想
                string think = Encoding.GetEncoding("GBK").GetString(body, 24, body.Length - 24);

                PrescriptionResult result = new PrescriptionResult();
                //自觉运动强度
                result.PR_SportStrength = (byte)(strength - 5);
                result.PR_Time1 = time / 100.0;
                result.PR_Distance = distance ;
                result.PR_CountWorkQuantity = energy / 100.0;
                result.PR_Cal = heat / 100.0;
                result.PR_Index = singer / 100.0;
                result.PR_Evaluate = rhythem;
                result.PR_UserThoughts = think;

                TrainService trainService = new TrainService();

                // 存数据库
                trainService.AddPrescriptionResult(idCard, result, DeviceType.X01);


            }
        }
    }

    
}
