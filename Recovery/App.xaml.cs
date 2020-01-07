
using MySql.Data.MySqlClient;
using NLog;
using Recovery.bean;
using Recovery.dao;
using Recovery.heartbeat;
using Recovery.http;
using Recovery.server;
using Recovery.service;
using Recovery.util;
using Recovery.view.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Setter = Recovery.entity.Setter;

namespace Recovery
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();//实例化　
        SetterDAO setterDao = new SetterDAO();
        AuthDAO authDAO = new AuthDAO();
        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
          
            base.OnLoadCompleted(e);
        }
        /// <summary>
        /// 应用启动的时候生命周期
        /// </summary>
        /// <param name="ex"></param>
        protected override void OnStartup(StartupEventArgs ex)
        {

            //全局异常处理机制，UI异常
            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            //全局异常处理机制，线程异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //加载语言
            LanguageUtils.SetLanguage();
           
            //启动netty,用于与设备端通信
            Thread th = new Thread(() =>
            {
                try
                {
                    logger.Info("线程启动成功");
                    NettyLuncher.getInstance().Start().Wait();

                }
                catch (AggregateException ee)
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        MessageBoxX.Info(LanguageUtils.GetCurrentLanuageStrByKey("App.PortOccupy"));
                        System.Environment.Exit(0);
                    }));
                }
            });
            th.Start();


            //启动远程更新
            Thread updateTh = new Thread(() =>
            {
                try
                {
                    
                    Thread.Sleep(1000 * 8);
                    Dictionary<string, string> param = new Dictionary<string, string>();
                     
                    param.Add("version", CommUtil.GetCurrentVersion());
                     var result = HttpSender.GET(HttpSender.URL_UPDATE, param);
                    if (string.IsNullOrEmpty(result))
                    {
                        return;
                    }
                    var info = JsonTools.DeserializeJsonToObject<VersionInfo>(result);
                    if (info == null || info.update==false)
                    {
                        return;
                    }

                    info.language = LanguageUtils.IsChainese() ? 1 : 0;
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        
                        Boolean dr = MessageBoxX.Question(LanguageUtils.GetCurrentLanuageStrByKey("App.UpdateInfo"));
                        if (dr == true)
                        {
                            Process.Start("AutoUpdater.exe", info.GetProcessString());
                            Environment.Exit(0);
                        }
                    }));
                  
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                    return;
                }

            });
          
            updateTh.Start();

           

            //大数据线程

            Thread bdth = new Thread(() =>
            {

                try
                {
                    SetterDAO setterDao = new SetterDAO();
                    AuthDAO authDAO = new AuthDAO();
                    while (true)
                    {
                        if (setterDao.ListAll().Count != 1)
                        {
                            //不激活不开启
                            Thread.Sleep(1000 * 15);
                            continue;
                        }
                        if (authDAO.ListAll().Count == 1)
                        {
                            //Console.WriteLine("-----------------boom shakalaka--------------");
                            //只有admin，不创建用户，不开启,睡个15s
                            Thread.Sleep(1000 * 15);
                            continue;
                        }
                        try
                        {
                            BigDataOfficer bigDataOfficer = new BigDataOfficer();
                            bigDataOfficer.Run();
                            //int heartBeatRate = (int)CommUtil.GetBigDataRate();
                            Thread.Sleep(1000 * 60);
                            //Console.WriteLine("-----------------boom");
                        }
                        catch(Exception e)
                        {
                            logger.Error("大数据线程失败:" + e.StackTrace);
                            Console.WriteLine("大数据线程失败:" + e.Message);
                           
                        }
                    }
                }
                catch (Exception exct)
                {
                    Console.WriteLine(exct.ToString());
                }
            });
            bdth.Start();

            //心跳线程
            Thread hbth = new Thread(() =>
            {

                ProtoBufSocket socket = null;
                try
                {
                    socket = new ProtoBufSocket();
                    socket.Connect();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("连接失败:" + exception.StackTrace);
                    TcpHeartBeatUtils.WriteLogFile("连接失败:" + exception.StackTrace);
                }

                while (true)
                {
                    try
                    {
                        
                        BodyStrongMessage bodyStrongMessage = new BodyStrongMessage
                        {
                            MessageType = BodyStrongMessage.Types.MessageType.Heaerbeatreq,
                            //可能为null
                            HeartbeatRequest = TcpHeartBeatUtils.GetHeartBeatByCurrent()
                        };
                        
                        socket.SendMessage(bodyStrongMessage);
                        Console.WriteLine("发送msg!!");
                        //Thread.Sleep(5000);
                    }
                    catch (Exception eee)
                    {
                        Console.WriteLine("发送msg失败" + eee.StackTrace);
                        TcpHeartBeatUtils.WriteLogFile("发送msg失败" + eee.StackTrace);
                    }
                    finally
                    {
                        Thread.Sleep(5000);
                    }
                }

            });
            hbth.Start();

            base.OnStartup(ex);
        }



        /// <summary>
        /// 退出的时候清理各种资源，尤其是Netty端口占用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            myTimer.Stop();
            System.Environment.Exit(0);
            base.OnExit(e);
        }

        /// <summary>
        /// UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                logger.Error(  "UI线程全局异常"+e.Exception.ToString());
                if(e.Exception is MySqlException)
                {
                    //MessageBox.Show(LanguageUtils.GetCurrentLanuageStrByKey("App.DbException"));
                }
                e.Handled = true;
            }
            catch (Exception ex)
            {
                logger.Error(  "不可恢复的UI线程全局异常"+ex.ToString());
                
                System.Environment.Exit(0);
            }
        }

        /// <summary>
        /// 非UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    logger.Error(  "非UI线程全局异常"+exception.ToString());

                }
            }
            catch (Exception ex)
            {
                logger.Error(  "不可恢复的非UI线程全局异常"+ex.ToString());
 
                System.Environment.Exit(0);
            }
        }

    }
}
