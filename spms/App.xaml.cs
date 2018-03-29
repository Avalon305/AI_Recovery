using spms.http;
using spms.server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace spms
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 应用启动的时候生命周期
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            //启动netty
            Thread th = new Thread(() =>
            {
                try
                {
                     NettyLuncher.getInstance().Start().Wait();

                }
                catch (AggregateException ee)
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        MessageBox.Show("通信端口被占用，建议重启计算机后再重试");
                        System.Environment.Exit(0);
                    }));
                }
            });
             th.Start();
            try
            {
                //开启大数据上传
                StartBigData();
            }
            catch(Exception ee)
            {
            }
            
            base.OnStartup(e);
        }
        /// <summary>
        /// 退出的时候清理各种资源，尤其是Netty端口占用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            System.Environment.Exit(0);
            base.OnExit(e);
        }

        private void StartBigData()
        {
            //大数据线程，主要上传除心跳之外的所有数据信息
            Thread bigDataThread;
            //启动大数据线程,切换界面记得关闭该线程
            bigDataThread = new Thread(new ThreadStart(UploadDataToWEB));
            //暂时先不启动
            bigDataThread.Start();
        }

        /// <summary>
        /// 上传的方法，参数为秒
        /// </summary>
        public static void UploadDataToWEB()
        {
            //300秒-5分钟一次上传
            BigDataOfficer bigDataOfficer = new BigDataOfficer(300 * 1000);
        }
    }
}
