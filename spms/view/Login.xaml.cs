using System;
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
using spms.constant;
using spms.dao;
using spms.http;
using spms.http.entity;
using spms.service;
using spms.util;
using spms.view.Pages;
using spms.view.Pages.Frame;

namespace spms.view
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
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            #region 通知公告 未激活不心跳

            SetterDAO setterDao = new SetterDAO();
            if (timerNotice == null)
            {

                while (setterDao.ListAll() != null)
                {
                    break;
                }
                BindNotice();

                timerNotice = new System.Timers.Timer();
                timerNotice.Elapsed += new System.Timers.ElapsedEventHandler((o, eea) => { BindNotice(); });

                timerNotice.Interval = CommUtil.GetHeartBeatRate();
                timerNotice.Start();
            }

            #endregion
        }
        #region 绑定通知公告
        private void BindNotice()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {

                    //如果用户没有被上传则return，不允许发心跳，否则就按照不合法冻结了
                    if (new UploadManagementDAO().CheckExistAuth() != null)
                    {
                        return;
                    }

                    HeartBeatOffice heartBeatOffice = new HeartBeatOffice();
                    HttpHeartBeat result = heartBeatOffice.GetHeartBeatByCurrent();
                    //心跳直接上传   !HttpSender.Ping() ||
                    if (result == null)
                    {
                        //如果没有取到值
                        return;
                    }
                    string jsonStr = HttpSender.POSTByJsonStr("communicationController/analysisJson",
                        JsonTools.Obj2JSONStrNew<HttpHeartBeat>(result));
                    HttpHeartBeat webResult = JsonTools.DeserializeJsonToObject<HttpHeartBeat>(jsonStr);
                    //本地数据更改
                    if (webResult == null)
                    {
                        return;
                    }
                    heartBeatOffice.SolveHeartbeat(webResult);
                     
                }
                catch
                {
                }
            });
        }

        #endregion
        public Login()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
                loginResult = "用户名或者密码不能为空";
            }
            //U盾监测，无误后登录
            if (loginResult.Equals("check_U"))
            {
                //验证U盾后跳转
                //暂时不验证U盾
                MainPage mainpage = new MainPage();
                this.Content = mainpage;
                
                timerNotice.Stop();
            }
            else if (loginResult.Equals("success"))
            {

                //成功登陆，跳转
                MainPage mainpage = new MainPage();
                this.Content = mainpage;
                timerNotice.Stop();
            }
            else {
                //问题登录  在登录提示框内显示信息
                bubble.IsOpen = true;
                Error_Info.Content = loginResult;
                
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
            
        }
        private void User_Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!User_Password.Password.Equals(""))
            {

                User_Password.Style = null;
            }
            else

            {
                Style xxStyle = (Style)this.FindResource("Watermark");
                User_Password.Style = xxStyle;
            }

        }
    }
}
