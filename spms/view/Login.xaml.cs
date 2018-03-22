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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using spms.service;
using spms.view.Pages;
using spms.view.Pages.Frame;

namespace spms.view
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
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
        }
        public Login()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        //登录操作
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ("false" == ConfigurationManager.AppSettings["Debug"])
            {
                //Debug模式直接进系统 方便开发
                MainPage mainpage1 = new MainPage();
                this.Content = mainpage1;
                return;
            }
            //获取用户名
            String name = this.User_Name.Text;
            //获取密码
            String password = this.User_Password.Password;
            
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

            }
            else if (loginResult.Equals("success"))
            {

                //成功登陆，跳转
                MainPage mainpage = new MainPage();
                this.Content = mainpage;
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

            var mi = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            mi.Invoke(bubble, null);

        }

    }
}
