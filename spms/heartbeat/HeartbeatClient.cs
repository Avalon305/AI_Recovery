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
      
        private IChannel bootstrapChannel;
        Bootstrap bootstrap;

        private void initlooper() {
            var workerGroup = new MultithreadEventLoopGroup();
          
                bootstrap = new Bootstrap();
                bootstrap
                    .Group(workerGroup)
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
        }
        private void connect()
        {
            try
            {
                this.bootstrapChannel = (IChannel)bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("192.168.1.1"), 6000));
                //logger.debug("远程服务器已经连接, 可以进行数据交换..");
            }
            catch (Exception e)
            {
                
            }

        }

        public IChannel getChannelFuture() 
        {
        // 如果管道没有被开启或者被关闭了，那么重连
        if (this.bootstrapChannel == null) {
            this.connect();
        }
        if (!this.bootstrapChannel.Active) {
            this.connect();
    }
        return this.bootstrapChannel;
    }
    public void sendMsg(BodyStrongMessage msg)
        {
            try
            {
                if (msg != null)
                {
                    this.getChannelFuture().WriteAndFlushAsync(msg);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
  
}
