
using DotNetty.Codecs.Protobuf;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Recovery.heartbeat
{
    public class HeartbeatClient
    {

        public static async Task RunClientAsync(BodyStrongMessage msg)
        {        
            var group = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    { 
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast("frameDecoder", new ProtobufVarint32FrameDecoder());
                        pipeline.AddLast("decoder", new ProtobufDecoder(BodyStrongMessage.Parser));
                        pipeline.AddLast("frameEncoder", new ProtobufVarint32LengthFieldPrepender());
                        pipeline.AddLast("encoder", new ProtobufEncoder());
                        pipeline.AddLast("tcpHandler", new HeartbeatHandler());

                    }));

                IChannel bootstrapChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("192.168.43.137"), 60000));

                await bootstrapChannel.WriteAndFlushAsync(msg);
                //Console.ReadLine();

                await bootstrapChannel.CloseAsync();
            }
            finally
            {
                group.ShutdownGracefullyAsync().Wait(1000);
            }
        }
    }
  
}
