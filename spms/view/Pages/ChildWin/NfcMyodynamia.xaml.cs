using spms.dao;
using spms.entity;
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
using System.Windows.Shapes;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// NfcMyodynamia.xaml 的交互逻辑
    /// </summary>
    public partial class NfcMyodynamia : Window
    {
        public NfcMyodynamia()
        {
            InitializeComponent();
        }

        private void Button_OK(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Height = SystemParameters.WorkArea.Size.Height;
            //绑定数据
            Load_Data();
        }

        private void Load_Data()
        {
            UserDAO userDAO = new UserDAO();

            UserRelationDao userRelationDao = new UserRelationDao();
            userRelationDao.FindUserRelationByuserID(((User)UsersInfo.SelectedItem).User_Id);
        }
    }
}
