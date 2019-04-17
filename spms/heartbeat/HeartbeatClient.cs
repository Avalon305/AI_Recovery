using Com.Bdl.Proto;
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

namespace spms.heartbeat
{
    public class HeartbeatClient
    {
        private volatile static HeartbeatClient instance = new HeartbeatClient();

        public static HeartbeatClient getInstance()
        {
            return instance;
        }
        private HeartbeatClient()
        {
            
        }

        private  IChannel clientChannel;
        private  Bootstrap bootstrap;
        public void initlooper()
        {
            var workerGroup = new MultithreadEventLoopGroup();
            try
            {
                bootstrap = new Bootstrap();
                bootstrap
                    .Group(workerGroup)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.ConnectTimeout, new TimeSpan(3000))
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast("frameDecoder", new ProtobufVarint32FrameDecoder());
                        pipeline.AddLast("decoder", new ProtobufDecoder(BodyStrongMessage.Parser));
                        pipeline.AddLast("frameEncoder", new ProtobufVarint32LengthFieldPrepender());
                        pipeline.AddLast("encoder", new ProtobufEncoder());
                        pipeline.AddLast("tcpHandler", new HeartbeatHandler());

                    }));
            }

            catch (Exception)
            {
                TcpHeartBeatUtils.WriteLogFile("连接宝德龙云平台失败，外部失败...");

            }
        }

        private async void connectAsync()
        {
            try
            {
                this.clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("192.168.1.109"), 60000));

            }
            catch (ConnectTimeoutException) {
                //throw;
                TcpHeartBeatUtils.WriteLogFile("连接宝德龙云平台超时");
            }
            catch (Exception xx)
            {
                //throw xx;
                TcpHeartBeatUtils.WriteLogFile("连接宝德龙云平台超时");
            }

        }

        private void getChannelFutureAsync()
        {
            // 如果管道没有被开启或者被关闭了，那么重连
            if (this.clientChannel == null)
            {
                connectAsync();
            }
            if (!this.clientChannel.Active)
            {
                connectAsync();
            }
            //return this.clientChannel;
        }
        public void sendMsgAsync(BodyStrongMessage msg)
        {
            try
            {
                if (msg != null)
                {

                    getChannelFutureAsync();
                    if (this.clientChannel != null)
                    {
                       clientChannel.WriteAndFlushAsync(msg);
                    }
                   
                }
            }
            catch (Exception)
            {
                TcpHeartBeatUtils.WriteLogFile("连接宝德龙云平台超时");
            }
        }
    }
  
}
