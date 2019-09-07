/// ***********************************************************************
/// 创 建 者    ：张方琛
/// 创建日期    ：2019/8/12 09:56:24
/// 功能描述    ：netty handler
/// ***********************************************************************

using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Newtonsoft.Json;
using NLog;
using Recovery.service;
using Recovery.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.protocol
{
	class ProtocolHandler : ChannelHandlerAdapter //管道处理基类，较常用
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private ParserTCPFrame parserTCPFrame = new ParserTCPFrame();

		//链接建立时出发，主动给客户端发数据
		public override void ChannelActive(IChannelHandlerContext context)
		{



		}


		//	重写基类的方法，当消息到达时触发，这里收到消息后，在控制台输出收到的内容，并原样返回了客户端
		public override void ChannelRead(IChannelHandlerContext context, object message)
		{
			var service = new DeviceCommService();
			var buffer = message as Message;
			logger.Info("收到报文：" + buffer.ToString());

			Message response = new Message();
			response.Sequence = buffer.Sequence > Int32.MaxValue - 1 ? 0 : buffer.Sequence + 1;

			switch (buffer.Type)
			{

				case HeadType.LoginRequest:
					Console.WriteLine("收到报文：-------->" + JsonConvert.SerializeObject(buffer.LoginRequest));

					var loginResp = service.HandleLoginRequest(buffer.LoginRequest);
					response.Type = HeadType.LoginResponse;
				    response.LoginResponse = loginResp;
					break;
				case HeadType.KeepaliveRequest:
					Console.WriteLine("收到报文：-------->" + JsonConvert.SerializeObject(buffer.KeepaliveRequest));

					var kpResp = service.HandleKeepaliveResponse(buffer.KeepaliveRequest);
					response.Type = HeadType.KeepaliveResponse;
					response.KeepaliveResponse = kpResp;
					break;
				case HeadType.PersonalSetRequest:
					Console.WriteLine("收到报文：-------->" + JsonConvert.SerializeObject(buffer.PersonalSetRequest));

					var setResp = service.HandlePersonalSetRequest(buffer.PersonalSetRequest);
					response.Type = HeadType.PersonalSetResponse;
					response.PersonalSetResponse = setResp;
					break;
				case HeadType.UploadRequest:
					Console.WriteLine("收到报文：-------->" + JsonConvert.SerializeObject(buffer.UploadRequest));

					var upResp = service.HandleUploadRequest(buffer.UploadRequest);
					response.Type = HeadType.UploadResponse;
					response.UploadResponse = upResp;
					break;
				case HeadType.MuscleStrengthRequest:
					Console.WriteLine("收到报文：-------->" + JsonConvert.SerializeObject(buffer.MuscleStrengthRequest));

					var muscleResp = service.HandleMuscleStrengthRequest(buffer.MuscleStrengthRequest);
					response.Type = HeadType.MuscleStrengthResponse;
					response.MuscleStrengthResponse = muscleResp;
					break;
				case HeadType.ErrorInfoRequest:
					Console.WriteLine("收到报文：-------->" + JsonConvert.SerializeObject(buffer.ErrorInfoRequest));

					var errorResp = service.HandleErrorInfoRequest(buffer.ErrorInfoRequest);
					response.Type = HeadType.ErrorInfoResponse;
					response.ErrorInfoResponse = errorResp;
					break;

				default:

					break;
			}
			logger.Info("响应报文：" + response.ToString());
			context.WriteAndFlushAsync(response);
			#region
			//var buffer = message as IByteBuffer;
			//if (buffer.ReadableBytes > 0)
			//{
			//    byte[] source = new byte[buffer.ReadableBytes];
			//    buffer.ReadBytes(source);

			//    // logger.Info("收到报文："+ ProtocolUtil.BytesToString(source));
			//    Console.WriteLine("收到报文：" + ProtocolUtil.BytesToString(source));
			//    byte[] response = parserTCPFrame.Parser(source);
			//    //logger.Info("响应报文："+ ProtocolUtil.BytesToString(response));
			//    Console.WriteLine("响应报文：" + ProtocolUtil.BytesToString(response));
			//    context.WriteAndFlushAsync(Unpooled.CopiedBuffer(response));


			//}
			#endregion

		}

		// 输出到客户端，也可以在上面的方法中直接调用WriteAndFlushAsync方法直接输出
		public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

		//捕获 异常，并输出到控制台后断开链接，提示：客户端意外断开链接，也会触发
		public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
		{

			context.CloseAsync();
		}
	}

}
