using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using spms.view.Pages;
using spms.view.Pages.Frame;

namespace spms
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        //登录操作
        private void Button_Click(object sender, RoutedEventArgs e)
        {   
            //获取用户名
            String name = this.User_Name.Text;
            //获取密码
            String password = this.User_Password.Password;
            Console.WriteLine("name:" + name);
            Console.WriteLine("password:" + password);
            //对用户名密码进行判断
            if(name.Equals("123") && password.Equals("123")) {
                //用户名密码正确进入主页面
            MainPage mainpage = new MainPage();
            this.Content = mainpage;
            }
            else
            {
                //用户名密码错误进行提示
                Name_ErrorInfo.Content = "用户名错误";
                Password_ErrorInfo.Content = "密码错误";
                Console.WriteLine(Name_ErrorInfo.Content);
                Color color = Color.FromArgb(255, 255, 0, 0);
                Name_ErrorInfo.Foreground = new SolidColorBrush(color);
                Password_ErrorInfo.Foreground = new SolidColorBrush(color);

            }
        }
        //打开设置页面
        private void Design(object sender, RoutedEventArgs e)
        {
            DesignPage1 designPage = new DesignPage1();
           this.Content = designPage;
        }
        //重置登录表单
        private void Resetting_Button(object sender, RoutedEventArgs e)
        {
            User_Name.Clear();
            User_Password.Clear();
        }
    }
}
