using spms.dao;
using spms.entity;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AuthDAO dao = new AuthDAO();
            Auth auth = new Auth();
            auth.Auth_Level = 1;
            auth.Auth_UserName = "haha";
            auth.Auth_UserPass = "444";
            auth.Gmt_Create = null;
            auth.Gmt_Modified = null;
            auth.User_Status = 1;
            auth.Auth_OfflineTime = DateTime.Now;

            Auth a = new Auth();
            a.Auth_UserPass = "333";

            List<Auth> list = new List<Auth>
            {
                auth,a
            };

            var resutl = dao.ListByUserStatus(1);
            resutl.ForEach(delegate (Auth name)
            {
                MessageBox.Show(name.Auth_UserName);
            });

        }
    }
}
