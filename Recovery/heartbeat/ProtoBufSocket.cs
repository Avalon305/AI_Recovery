using System;
using System.Threading;
using System.Threading.Tasks;

using DotNetty.Codecs.Protobuf;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace Recovery.heartbeat
{
    public delegate void ReceiveMessageEvent(object message);


    public class ProtoBufSocket
    {
        public event ReceiveMessageEvent ReceiveMessage;

        private AutoResetEvent ChannelInitilizedEvent = new AutoResetEvent(false);
        private Bootstrap SocketBootstrap = new Bootstrap();
        private MultithreadEventLoopGroup WorkGroup = new MultithreadEventLoopGroup();
        private volatile bool Connected = false;
        private IChannel Channel;

        public ProtoBufSocket()
        {
            InitBootstrap();
        }

        private void InitBootstrap()
        {
            SocketBootstrap.Group(WorkGroup)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Option(ChannelOption.SoKeepalive, true)
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


        public void Connect()
        {
            //var thread = new Thread(new ThreadStart(DoConnect().Wait));
            //thread.Start();
            DoConnect().Wait();
        }
        public void Disconnect()
        {
            WorkGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
        }

        public void SendMessage(object message)
        {
            if (!Connected)
                //Connected = ChannelInitilizedEvent.WaitOne();
                Connect();

            Channel.WriteAndFlushAsync(message);
        }

        private async Task DoConnect()
        {
            Connected = false;
            var connected = false;
            do
            {
                try
                {
                    //var clientChannel = await SocketBootstrap.ConnectAsync("222.133.43.210", 29999);
                    var clientChannel = await SocketBootstrap.ConnectAsync("192.168.43.129", 29999);
                    Channel = clientChannel;
                    ChannelInitilizedEvent.Set();
                    connected = true;
                }
                catch (Exception error_net)
                {
                    Console.WriteLine("心跳线程连接失败：" + error_net.StackTrace);
                    Console.WriteLine("Reconnect server after 5 seconds...");
                    Thread.Sleep(5000);
                }
            } while (!connected);
        }

    }
}