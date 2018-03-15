using spms.http.entity;
using spms.util;
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
using spms.service;
using spms.http.dto;
using spms.dao;
using spms.entity;
using spms.protocol;
using spms.constant;
using NLog;

namespace spms
{
    /// <summary>
    /// zeroTest.xaml 的交互逻辑
    /// </summary>
    public partial class zeroTest : Page
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public zeroTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string pingJsonStr = JsonTools.Obj2JSONStrNew(new HttpHeartBeat("ping", "ping"));
            MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string pingJsonStr = JsonTools.Obj2JSONStrNew(new UploadManagementService().ListLimit30());
                 MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AutherDTO autherDTO = new AutherDTO();
            autherDTO.username = "XXX";
            string pingJsonStr = JsonTools.Obj2JSONStrNew(autherDTO);
            MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AuthDAO authDAO = new AuthDAO();
            Auther auther = authDAO.GetByAuthLevel(Auther.AUTH_LEVEL_ADMIN);
            string pingJsonStr = JsonTools.Obj2JSONStrNew(auther);
            MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
 
 
            //已删除，按照冻结处理
            MessageBox.Show("用户被删除，即将退出，请联系宝德龙管理员恢复！");
            Environment.Exit(0);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            AuthDAO authDAO = new AuthDAO();
            Auther auther = authDAO.Login("123", "123");
            string pingJsonStr = JsonTools.Obj2JSONStrNew(auther);
            MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(SystemInfo.GetMacAddress());
 
        }
    }
}
