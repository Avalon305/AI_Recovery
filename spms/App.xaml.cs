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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Setter = spms.entity.Setter;

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
            var a = ConfigUtil.GetEncrypt("test");
            MessageBox.Show(a);
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
                        MessageBox.Show("通信端口被占用，建议重启计算机后再重试");
                        System.Environment.Exit(0);
                    }));
                }
            });
            th.Start();


            //启动远程更新
            Thread updateTh = new Thread(() =>
            {
                Thread.Sleep(1000 * 3);
                Dictionary<string, string> param = new Dictionary<string, string>();
                var setter = new SetterService().GetSetterDAO().getSetter();
                param.Add("version", setter.Set_Version);
                var result = HttpSender.GET("http://127.0.0.1:8080/pcms/th", param);
                var info = JsonTools.DeserializeJsonToObject<VersionInfo>(result);

                MessageBox.Show(info.downloadUrl + "  " + info.md5);

            });

            updateTh.Start();




            //大数据线程

            Thread bdth = new Thread(() =>
        {
            SetterDAO setterDao = new SetterDAO();
            while (true)
            {
                if (setterDao.ListAll().Count == 0)
                {
                    continue;
                }
                BigDataOfficer bigDataOfficer = new BigDataOfficer();
                bigDataOfficer.Run();
                int heartBeatRate = (int)CommUtil.GetBigDataRate();
                Thread.Sleep(1000 * 300);

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

    }
}
