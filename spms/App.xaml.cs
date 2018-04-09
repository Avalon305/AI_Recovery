using MySql.Data.MySqlClient;
using NLog;
using spms.bean;
using spms.dao;
using spms.http;
using spms.server;
using spms.service;
using spms.util;
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
using Setter = spms.entity.Setter;

namespace spms
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
          
            base.OnLoadCompleted(e);
        }
        /// <summary>
        /// 应用启动的时候生命周期
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {

            //全局异常处理机制，UI异常
            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            //全局异常处理机制，线程异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //加载语言
            LanguageUtils.SetLanguage();

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
                        MessageBox.Show(LanguageUtils.GetCurrentLanuageStrByKey("App.PortOccupy"));
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
                        
                        MessageBoxResult dr = MessageBox.Show(LanguageUtils.GetCurrentLanuageStrByKey("App.UpdateInfo"), LanguageUtils.GetCurrentLanuageStrByKey("App.Tips"), MessageBoxButton.OKCancel, MessageBoxImage.Question);
                        if (dr == MessageBoxResult.OK)
                        {
                            Process.Start("AutoUpdater.exe", info.GetProcessString());
                            Environment.Exit(0);
                        }
                    }));
                  
                }
                catch(Exception ex)
                {
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
                        if (setterDao.ListAll().Count == 0)
                        {
                            //不激活不开启
                            continue;
                        }
                        if (authDAO.ListAll().Count == 1) {
                            //只有admin，不创建用户，不开启
                            continue;
                        }
                        BigDataOfficer bigDataOfficer = new BigDataOfficer();
                        bigDataOfficer.Run();
                        int heartBeatRate = (int)CommUtil.GetBigDataRate();
                        Thread.Sleep(1000 * 300);

                    }
                }
                catch (Exception ex)
                {

                }

               
            });
            bdth.Start();

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
