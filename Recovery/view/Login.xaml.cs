﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Recovery.constant;
using Recovery.dao;
using Recovery.http;
using Recovery.http.entity;
using Recovery.service;
using Recovery.util;
using Recovery.view.Pages;
using Recovery.view.Pages.Frame;

namespace Recovery.view
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {

        //后台心跳更新UI线程
        public System.Timers.Timer timerNotice = null;
        //用到的业务层实例
        UserService userService = new UserService();
        AuthDAO authDao = new AuthDAO();

        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //加载图片
            if (LanguageUtils.IsChainese())
            {
                DesignerHead4.Source = new BitmapImage(new Uri(@"\view\images\6.png", UriKind.Relative));
            }
            else
            {
                //TODO 英文图片
                DesignerHead4.Source = new BitmapImage(new Uri(@"\view\images\6.png", UriKind.Relative));
            }
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            #region 通知公告  

            //SetterDAO setterDao = new SetterDAO();

            ////未激活无心跳
            //if (setterDao.ListAll() != null && setterDao.ListAll().Count == 0)
            //{

            //}
            ////没有一般用户无心跳
            //else if (authDao.ListAll() != null && authDao.ListAll().Count == 1)
            //{

            //}
            //else {
            //    if (timerNotice == null)
            //    {

            //        BindNotice();

            //        timerNotice = new System.Timers.Timer();
            //        timerNotice.Elapsed += new System.Timers.ElapsedEventHandler((o, eea) => { BindNotice(); });

            //        timerNotice.Interval = CommUtil.GetHeartBeatRate();
            //        timerNotice.Start();
            //    }
            //}
            

            #endregion

            //打开时，是否记住密码的勾选，如果是就勾选，并且填充登录名和密码 如果不是就没有操作
            //bool? checkRemind = isRemind.IsChecked;
            String ckeckStr = CommUtil.GetSettingString("isRemind");
            String ckeckStrName = CommUtil.GetSettingString("isRemindName");
            bool? checkRemind = ckeckStr == "true" ? true : false;
            bool? checkRemindName = ckeckStrName == "true" ? true : false;
            if (checkRemind == true) {
                //UI效果-记住密码，一定记住用户名
                isRemind.IsChecked = true;
                isRemindName.IsChecked = true;
                //获取用户名
                String name = ConfigUtil.GetEncrypt("userName", "");
                this.User_Name.Text = name;
                //获取密码
                String password = ConfigUtil.GetEncrypt("password", ""); ;
                this.User_Password.Password = password;
            } else if (checkRemindName==true) {
                //UI效果-只记住用户名就只显示用户名
                isRemindName.IsChecked = true;
                //界面注入
                String name = ConfigUtil.GetEncrypt("userName", "");
                this.User_Name.Text = name;
            }
            
        }
        #region 绑定通知公告
        //private void BindNotice()
        //{
        //    System.Threading.Tasks.Task.Factory.StartNew(() =>
        //    {
        //        try
        //        {

        //            //如果用户没有被上传则return，不允许发心跳，否则就按照不合法冻结了
        //            if (new UploadManagementDAO().CheckExistAuth() != null)
        //            {
        //                return;
        //            }

        //            HeartBeatOffice heartBeatOffice = new HeartBeatOffice();
        //            HttpHeartBeat result = heartBeatOffice.GetHeartBeatByCurrent();
        //            //心跳直接上传   !HttpSender.Ping() ||
        //            if (result == null)
        //            {
        //                //如果没有取到值
        //                return;
        //            }
        //            string jsonStr = HttpSender.POSTByJsonStr("communicationController/analysisJson",
        //                JsonTools.Obj2JSONStrNew<HttpHeartBeat>(result));
        //            HttpHeartBeat webResult = JsonTools.DeserializeJsonToObject<HttpHeartBeat>(jsonStr);
        //            //本地数据更改
        //            if (webResult == null)
        //            {
        //                return;
        //            }
        //            heartBeatOffice.SolveHeartbeat(webResult);
                     
        //        }
        //        catch
        //        {
        //        }
        //    });
        //}

        #endregion
        public Login()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Height = SystemParameters.WorkArea.Size.Height;
            this.Width = SystemParameters.WorkArea.Size.Width;
            this.SizeChanged += new System.Windows.SizeChangedEventHandler(MainWindow_Resize);
        }
        private void MainWindow_Resize(object sender, System.EventArgs e)
        {
            //Console.WriteLine("窗体改变");
            //Console.WriteLine(SystemParameters.WorkArea.Size.Height);
            //Console.WriteLine(SystemParameters.WorkArea.Size.Width);
            if (this.WindowState == WindowState.Maximized)
            {
                //Console.WriteLine("123123");
                this.Height = SystemParameters.WorkArea.Size.Height;
                this.Width = SystemParameters.WorkArea.Size.Width;
            }
            //Console.WriteLine("当前窗体高度："+ this.Height);
            //Console.WriteLine("当前窗体宽度：" + this.Width);
        }
        //登录操作
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*if ("true" == ConfigurationManager.AppSettings["Debug"])
            {
                //Debug模式直接进系统 方便开发
                MainPage mainpage1 = new MainPage();
                this.Content = mainpage1;
                return;
            }*/
            //获取用户名
            String name = this.User_Name.Text;
            //获取密码
            String password = this.User_Password.Password;
            UserConstant.USERNAME = name;
            UserConstant.PASSWORD = password;
            ///登录逻辑
            AuthService authService = new AuthService();
            string loginResult = authService.Login(name, password);
            Console.WriteLine("loginResult:" + loginResult);
           
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password)) {
                loginResult = LanguageUtils.ConvertLanguage("用户名或者密码不能为空", "User name or password cannot be empty");
            }
            //U盾监测，无误后登录
            if (loginResult.Equals("check_U"))
            {
                //admin不能记住密码  很危险
                //验证U盾后跳转
                //暂时不验证U盾
                MainPage mainpage = new MainPage();
                this.Content = mainpage;
                if (timerNotice!=null) {
                    timerNotice.Stop();
                }
                
            }
            else if (loginResult.Equals("success"))
            {
                //普通用户，点击记住密码，登陆成功后，登录用户与密码加入缓存
                bool? checkRemind = isRemind.IsChecked;
                bool? checkRemindName = isRemindName.IsChecked;
                if (checkRemind == true)
                {
                    //加密后存储在config中
                    name = ConfigUtil.Encrypt(name);
                    password = ConfigUtil.Encrypt(password);
                    CommUtil.UpdateSettingString("isRemind", "true");
                    CommUtil.UpdateSettingString("isRemindName", "true");
                    CommUtil.UpdateSettingString("userName", name);
                    CommUtil.UpdateSettingString("password", password);
                } else if (checkRemindName == true) {
                    name = ConfigUtil.Encrypt(name);
                    CommUtil.UpdateSettingString("isRemind", "false");
                    CommUtil.UpdateSettingString("isRemindName", "true");
                    CommUtil.UpdateSettingString("userName", name);
                }
                else {
                    //都不选中就不记住，清空缓存的密码
                    CommUtil.UpdateSettingString("isRemind", "false");
                    CommUtil.UpdateSettingString("isRemindName", "false");
                    CommUtil.UpdateSettingString("userName", "");
                    CommUtil.UpdateSettingString("password", "");
                }

                //成功登陆，跳转
                MainPage mainpage = new MainPage();
                this.Content = mainpage;

                if (timerNotice != null)
                {
                    timerNotice.Stop();
                }
            }
            else {
                //问题登录  在登录提示框内显示信息
                bubble.IsOpen = true;
                //Error_Info.Content = loginResult;
                 
                Error_Info.Content = LanguageUtils.ConvertLanguage(loginResult, "login exist question!");
            }
            
        }
       
        //重置登录表单
        private void Resetting_Button(object sender, RoutedEventArgs e)
        {
            User_Name.Clear();
            User_Password.Clear();
        }

        private void EndBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //解决气泡不随着窗体移动问题
        private void windowmove(object sender, EventArgs e)
        {
            if(bubble.IsOpen == true) { 
            var mi = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            mi.Invoke(bubble, null);
            }
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Button_Click(this, null);
            }
            //else if((e.Key == Key.LWin||e.Key == Key.RWin)&&e.Key == Key.D)
            //{
            //    this.WindowState = WindowState.Minimized;
            //}
            
        }
        private void User_Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!User_Password.Password.Equals(""))
            {

                User_Password.Style = null;
                
                //User_Password.SelectionStart = User_Password.Text.Length;
            }
            else

            {
                Style xxStyle = (Style)this.FindResource("Watermark");
                User_Password.Style = xxStyle;
            }

        }

        private void image_load(object sender, RoutedEventArgs e)
        {
            List<Recovery.entity.Setter> all = new SetterDAO().ListAll();
            if (all != null && all.Count != 0)
            {
                if (all[0].Set_Language == 0)
                {
                    DesignerHead4.Source = new BitmapImage(new Uri("/view/Images/5_6.png", UriKind.RelativeOrAbsolute));
                    DesignerHead4.Margin = new Thickness(99, 23, 0, 0);
                    DesignerHead4.Height = 25;
                    DesignerHead4.Width = 125;
                    DesignerHead3.Height = 65;
                }
                else
                {
                    DesignerHead4.Source = new BitmapImage(new Uri("/view/Images/6.png", UriKind.RelativeOrAbsolute));
                    DesignerHead4.Margin = new Thickness(10, 0, 0, 0);
                    DesignerHead4.Height = 65;
                    DesignerHead4.Width = 225;
                    DesignerHead3.Height = 85;
                }
            }
        }

        private void User_Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!User_Name.Text.Equals(""))
            {
                User_Name.Style = null;
                //User_Name.SelectionStart = User_Name.Text.Length;
            }
            else
            {
                Style xxStyle = (Style)this.FindResource("watermark");
                User_Name.Style = xxStyle;
            }
        }
    }
}
