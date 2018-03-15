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

            byte calcXor = ProtocolUtil.XorByByte(buffer, 0, 11 + data_len);

            TcpFrameBean frameBean = new TcpFrameBean();
            if (xor != calcXor)
            {
                frameBean.AppendErrorMsg( "校验码不符合预期");
                return response;
            }
            frameBean.DataBody = data;
            frameBean.SerialNo = serialNo;
            frameBean.TerminalId = terminalId;
            frameBean.MsgId = msgId;

            switch (msgId)
            {
                case MsgId.X0001://设备通用应答
                    break;
                case MsgId.X0002://心跳
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
                    break;
            }

            return response;
        }

        private void HandleRequestImageData(ref byte[] response, TcpFrameBean frameBean)
        {
            byte[] body = frameBean.DataBody;
            string idcard = Encoding.GetEncoding("GBK").GetString(body, 0, 18);
            Int16 packNum = BitConverter.ToInt16(body, 32);

            byte[] data = MakerTCPFrame.GetInstance().Make8007Frame(packNum, idcard);
            byte nextSerialNo = (byte)(frameBean.SerialNo + 1);
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X8007, nextSerialNo, frameBean.TerminalId, data);

        }

       
        private void HandleRequestImageCount(ref byte[] response, TcpFrameBean frameBean)
        {
            byte[] idBytes = frameBean.DataBody;
            string idcard = Encoding.GetEncoding("GBK").GetString(idBytes, 0, 18);
         

            byte[] data = MakerTCPFrame.GetInstance().Make8006Frame(idcard);
            Int16 nextSerialNo = (Int16)(frameBean.SerialNo + 1);
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X8006, nextSerialNo, frameBean.TerminalId, data);

        }

        private void HandleRequestUserInfo(ref byte[] response, TcpFrameBean frameBean)
        {
            byte[] body = frameBean.DataBody;
            string idcard = Encoding.GetEncoding("GBK").GetString(body, 0, 18);
        }

        private void HandlePricticeResult(ref byte[] response, TcpFrameBean frameBean)
        {
            throw new NotImplementedException();
        }

        public void HandleStartPrictice(ref byte[] response, TcpFrameBean frameBean)
        {
            byte[] data = frameBean.DataBody;
            string userId = Encoding.GetEncoding("GBK").GetString(data, 0, 18);
            DeviceType deviceType = (DeviceType)data[32];

        }
        public void HandleHeartBeat(ref byte[] response,TcpFrameBean frameBean)
        {
            byte[] data = MakerTCPFrame.GetInstance().Make8001Frame(frameBean.SerialNo, frameBean.MsgId, CommResponse.Success);
            byte nextSerialNo = (byte)(frameBean.SerialNo + 1);
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X8001, nextSerialNo, frameBean.TerminalId, data);

        }
    }
}
