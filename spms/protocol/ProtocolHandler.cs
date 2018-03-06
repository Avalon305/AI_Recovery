using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.protocol
{
    class ProtocolHandler : ChannelHandlerAdapter //管道处理基类，较常用
    {
        //链接建立时出发，主动给客户端发数据
        public override void ChannelActive(IChannelHandlerContext context)
        {
            IByteBuffer initialMessage = Unpooled.Buffer();
            byte[] CMD = { 0xF1, 0xF0 };//命令
            string data_uuid = Guid.NewGuid().ToString("N");//数据
            //Console.WriteLine(data_uuid);
            byte[] uuid = new byte[16];
            int startindex = 0;
            for (int i = 0; i < 16; i++)
            {
                uuid[i] = Convert.ToByte(data_uuid.Substring(startindex, 2), 16);
                startindex += 2;
            }
            byte[] protocal = ProtocolUtil.packData(CMD, uuid);
            string sprotocal = ProtocolUtil.ByteToString(protocal);
            Console.WriteLine(sprotocal);
            initialMessage.WriteBytes(protocal);
            context.WriteAndFlushAsync(initialMessage);

        }
        //	重写基类的方法，当消息到达时触发，这里收到消息后，在控制台输出收到的内容，并原样返回了客户端
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {

            var buffer = message as IByteBuffer;
            byte[] Data = new byte[16];
            if (buffer != null)
            {
                buffer.GetBytes(4, Data);//获取从4开始的16个字节
                string Sdata=ProtocolUtil. ByteToString(Data);
               Console.WriteLine("Received from client: " + Sdata);//打印获得的数据
            }

            context.WriteAsync(message);//写入输出流
        }

        // 输出到客户端，也可以在上面的方法中直接调用WriteAndFlushAsync方法直接输出
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        //捕获 异常，并输出到控制台后断开链接，提示：客户端意外断开链接，也会触发
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }

}
