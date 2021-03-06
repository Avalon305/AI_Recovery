﻿using NLog;
using Recovery.bean;
using Recovery.constant;
using Recovery.entity;
using Recovery.service;
using Recovery.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.protocol
{
    class ParserTCPFrame
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private OnlineDeviceService onlineDeviceService = new OnlineDeviceService();

        public byte[] Parser(byte[] source)
        {
            byte[] response = new byte[0];

            byte[] buffer = ProtocolUtil.UnTransfer(source);
            MsgId msgId = ProtocolUtil.BytesToMsgId(buffer, 0);
            Int16 data_len = BitConverter.ToInt16(buffer, 2);
            data_len = IPAddress.NetworkToHostOrder(data_len);
            string terminalId = ProtocolUtil.BcdToString(buffer, 4, 6);
            Int16 serialNo = BitConverter.ToInt16(buffer, 10);
            serialNo = IPAddress.NetworkToHostOrder(serialNo);
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
            frameBean.DeviceType = (DeviceType)buffer[4];
            if (xor != calcXor)
            {
                logger.Error("校验码不符合预期：" + ProtocolUtil.BytesToString(source));

                frameBean.AppendErrorMsg("校验码不符合预期");
                byte[] d = MakerTCPFrame.GetInstance().Make8001Frame(serialNo, MsgId.X0001, CommResponse.MistakeMsg);
                response = MakerTCPFrame.GetInstance().PackData(MsgId.X8001, frameBean.TerminalId, d);
                return response;
            }

            switch (msgId)
            {
                case MsgId.X0001://设备通用应答
                    break;
                case MsgId.X0002://心跳OK
                    logger.Info("收到心跳：" + ProtocolUtil.BytesToString(source));
                    WriteLogFile("收到心跳：" + ProtocolUtil.BytesToString(source));
                    HandleHeartBeat(ref response, frameBean);
                    break;
                case MsgId.X0008://开始训练
                    logger.Info("收到开始训练请求：" + ProtocolUtil.BytesToString(source));
                    WriteLogFile("收到开始训练请求：" + ProtocolUtil.BytesToString(source));
                    HandleStartPrictice(ref response, frameBean);
                    break;
                case MsgId.X0009://训练结果上报
                    logger.Info("收到训练结果上报：" + ProtocolUtil.BytesToString(source));
                    WriteLogFile("收到训练结果上报：" + ProtocolUtil.BytesToString(source));
                    byte[] dd = MakerTCPFrame.GetInstance().Make8001Frame(serialNo, MsgId.X0001, CommResponse.Success);
                    response = MakerTCPFrame.GetInstance().PackData(MsgId.X8001, frameBean.TerminalId, dd);
                    HandlePricticeResult(ref response, frameBean);
                    break;
                case MsgId.X000A://请求使用者信息
                    logger.Info("收到请求使用者信息：" + ProtocolUtil.BytesToString(source));
                    WriteLogFile("收到请求使用者信息：" + ProtocolUtil.BytesToString(source));

                    HandleRequestUserInfo(ref response, frameBean);
                    break;
                case MsgId.X0007://请求照片数据OK
                    logger.Info("收到请求照片数据OK：" + ProtocolUtil.BytesToString(source));
                    WriteLogFile("收到请求照片数据OK：" + ProtocolUtil.BytesToString(source));
                    HandleRequestImageData(ref response, frameBean);
                    break;
                default:
                    logger.Error("收到未知消息：" + ProtocolUtil.BytesToString(source));
                    WriteLogFile("收到未知消息：" + ProtocolUtil.BytesToString(source));
                    frameBean.AppendErrorMsg("未知的消息ID");
                    byte[] d = MakerTCPFrame.GetInstance().Make8001Frame(serialNo, MsgId.X0001, CommResponse.UnSupport);
                    response = MakerTCPFrame.GetInstance().PackData(MsgId.X8001, frameBean.TerminalId, d);
                    break;
            }
            logger.Info("响应的报文：" + ProtocolUtil.BytesToString(response));
            WriteLogFile("响应的报文：" + ProtocolUtil.BytesToString(response));
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
            string idcard =  ProtocolUtil.GetEndUserIdString(body,0);

            byte[] data = MakerTCPFrame.GetInstance().Make8007Frame(idcard);
         
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X8007, frameBean.TerminalId, data);

        }



        /// <summary>
        /// 处理请求用户信息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="frameBean"></param>
        private void HandleRequestUserInfo(ref byte[] response, TcpFrameBean frameBean)
        {
            byte[] body = frameBean.DataBody;
            string idcard = ProtocolUtil.GetEndUserIdString(body, 0);

            byte[] data = MakerTCPFrame.GetInstance().Make800AFrame(idcard);
        
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X800A, frameBean.TerminalId, data);
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
            string idCard = ProtocolUtil.GetEndUserIdString(body, 0);

            byte[] d = new byte[body.Length - 32];
            Array.Copy(body, 32, d, 0, d.Length);
            //设备类型
            DeviceType deviceType = (DeviceType)d[0];
            logger.Info("收到训练结果上报，设备是：" + deviceType.ToString());
             
            paser.PaserXall(d, idCard, deviceType);
            //数据上报响应通用应答
            byte[] data = MakerTCPFrame.GetInstance().Make8001Frame(frameBean.SerialNo, frameBean.MsgId, CommResponse.Success);
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X8001, frameBean.TerminalId, data);

        }

        /// <summary>
        /// 处理请求处方信息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="frameBean"></param>
        public void HandleStartPrictice(ref byte[] response, TcpFrameBean frameBean)
        {
            var maker = MakerTCPFrame.GetInstance(); ;

            byte[] data = frameBean.DataBody;
            string userId = ProtocolUtil.GetEndUserIdString(data, 0);

            DeviceType deviceType = (DeviceType)data[32];

            byte[] d = maker.Make8008Frame(userId, deviceType);
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X8008, frameBean.TerminalId, d);

      

        }
       
        /// <summary>
        /// 处理心跳
        /// </summary>
        /// <param name="response"></param>
        /// <param name="frameBean"></param>
        public void HandleHeartBeat(ref byte[] response, TcpFrameBean frameBean)
        {
            try
            {
                onlineDeviceService.SavaOrUpdateOnlineInfo(frameBean);
            }catch(Exception ex)
            {
                logger.Error("心跳更新或存储在线时间时发生异常：" + ex.ToString());
            }
            byte[] data = MakerTCPFrame.GetInstance().Make8001Frame(frameBean.SerialNo, frameBean.MsgId, CommResponse.Success);
            response = MakerTCPFrame.GetInstance().PackData(MsgId.X8001, frameBean.TerminalId, data);

        }
        //直接写入文件而不是走日志，测试时候使用，会降低性能，发布时去掉
        private void WriteLogFile(string content) {
            ////string path = Directory.GetCurrentDirectory() + "\\logDebug\\netty.txt";
            //string path3 = System.IO.Directory.GetCurrentDirectory();
            //string path = @"E:\nettyLog.log";
            //if (!File.Exists(path)) {
            //    //System.IO.File.WriteAllText(path, "临时日志文件，可以删除！", Encoding.UTF8);
            //    File.Create(path);
            //}
            //try {
            //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            //    {

            //        //file.Write(line);//直接追加文件末尾，不换行
            //        file.WriteLine(DateTime.Now.ToString() +"--->" +content);// 直接追加文件末尾，换行 
            //    }
            //} catch (System.IO.IOException e) {
            //    if (!File.Exists(path))
            //    {
            //        //System.IO.File.WriteAllText(path, "临时日志文件，可以删除！", Encoding.UTF8);
            //        File.Create(@"D:\nettyLog.log");
            //    }
            //}
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                path = System.IO.Path.Combine(path
                , "ZLogs\\");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                string fileFullName = System.IO.Path.Combine(path
                , string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMdd-HHmm")));


                using (StreamWriter output = System.IO.File.AppendText(fileFullName))
                {
                    output.WriteLine(content);

                    output.Close();
                }
            }
            catch (Exception)
            {

                 
            }
         
        }

        /// <summary>
        /// 内部类，辅助解析各种设备上报
        /// </summary>
        class ParserPricticeResult
        {
            private Logger logger = LogManager.GetCurrentClassLogger();
            /// <summary>
            /// 胸部推举机上报解析
            /// </summary>
            /// <param name="body"></param>
            public void PaserXall(byte[] body, string idCard,DeviceType deviceType)
            {
                //运动强度
                byte strength = ProtocolUtil.BcdToInt(body[1]);
                //运动时间 1/100秒
                Int32 time = BitConverter.ToInt32(body, 2);
                time = IPAddress.NetworkToHostOrder(time);
                //总移动距离 毫米
                Int32 distance = BitConverter.ToInt32(body, 6);
                distance = IPAddress.NetworkToHostOrder(distance);
                //总功 1/100 焦耳
                Int32 energy = BitConverter.ToInt32(body, 10);
                energy = IPAddress.NetworkToHostOrder(energy);

                //消耗热量 1/100 卡路里
                Int32 heat = BitConverter.ToInt32(body, 14);
                heat = IPAddress.NetworkToHostOrder(heat);
                //指数标识 0 正数 1 负数
                //指数值 1/100
                Int32 singer = BitConverter.ToInt32(body, 19);
                singer = IPAddress.NetworkToHostOrder(singer);
                singer = body[18] == 0x00 ? singer : -1 * singer;
                //动作节奏 0没问题 1 有些许问题 2 有问题
                byte rhythem = body[23];
                //使用者感想
                string think = ProtocolUtil.GetEndString(body, 24);

                PrescriptionResult result = new PrescriptionResult();
                //自觉运动强度
                result.PR_SportStrength = (byte)(strength - 5);
                result.PR_Time1 = time / 100.0  ;
                result.PR_Distance = distance;
                result.PR_CountWorkQuantity = energy / 100.0;
                result.PR_Cal = heat / 100.0;
                result.PR_Index = singer / 100.0;
                result.PR_Evaluate = rhythem;
                result.PR_UserThoughts = think;
                result.Gmt_Create = DateTime.Now;
                result.Gmt_Modified = result.Gmt_Create;

                StringBuilder sb = new StringBuilder();
                sb.Append("运动强度：").Append(strength).Append("运动时间：").Append(time).Append("总移动距离：").Append(distance).Append("总功：").Append(energy)
                    .Append("消耗热量").Append(heat).Append("指数值：").Append(singer).Append("动作节奏：").Append(rhythem);
                logger.Info("训练上报的解析结果：" + sb.ToString());
                TrainService trainService = new TrainService();

                // 存数据库
                trainService.AddPrescriptionResult(idCard, result, deviceType);


            }
        }
    }


}
