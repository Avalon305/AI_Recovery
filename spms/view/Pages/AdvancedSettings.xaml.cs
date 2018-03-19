using Dapper;
using spms.dao;
using spms.entity;
using spms.util;
using spms.view.Pages.ChildWin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace spms.view.Pages
{
    /// <summary>
    /// AdvancedSettings.xaml 的交互逻辑
    /// </summary>
    public partial class AdvancedSettings : Page
    {
        List<Auther> AutherList = new List<Auther>();
        List<DeviceSet> DeviceSetList = new List<DeviceSet>();
        ObservableCollection<Auther> AutherCollection;
        ObservableCollection<DeviceSet> DeviceSetCollection;
        AuthDAO authDAO = new AuthDAO();
        DeviceSetDAO deviceSetDAO = new DeviceSetDAO();
        Auther auther = new Auther();
        int judge = 0;
        int checkchange = 0;
        public AdvancedSettings()
        {
            InitializeComponent();
            AutherList = authDAO.ListAll();
            DeviceSetList = deviceSetDAO.ListAll();

            DeviceSetCollection = new ObservableCollection<DeviceSet>(DeviceSetList);
            AutherCollection = new ObservableCollection<Auther>(AutherList);
            ((this.FindName("DataGrid1")) as DataGrid).ItemsSource = AutherCollection;
            ((this.FindName("DataGrid2")) as DataGrid).ItemsSource = DeviceSetCollection;
        }
        //返回上一页
        private void GoBack(object sender, RoutedEventArgs e)
        {
            Window window = (Window)this.Parent;
            window.Content = new DesignPage1();
        }
        private void Device_Checked(object sender, RoutedEventArgs e)//单击左侧CheckBox触发事件
        {
            checkchange = 1;


        }
        List<int> selectID = new List<int>();  //保存选中要删除行的FID值  
        private void CheckBox_Click(object sender, RoutedEventArgs e)//单击右侧CheckBox触发事件
        {
            CheckBox checkBox = sender as CheckBox;
            int ID = int.Parse(checkBox.Tag.ToString());   //获取该行的FID  
            var IsChecked = checkBox.IsChecked;
            if (IsChecked == true)
            {
                selectID.Add(ID);         //如果选中就保存FID  
            }
            else
            {
                selectID.Remove(ID);  //如果选中取消就删除里面的FID  
            }
        }
        private void Btn_Delete(object sender, RoutedEventArgs e) //单击删除按钮触发事件
        {
            foreach (int ID in selectID)
            {
                auther.Pk_Auth_Id = ID;
                authDAO.DeleteByPrimaryKey(auther);//在数据库中删除
                for (int i = 0; i < AutherCollection.Count; i++)
                {
                    if (AutherCollection[i].Pk_Auth_Id == ID) AutherCollection.RemoveAt(i);//在collection中删除
                }

            }
        }
        private void Grid_Row_Click(object sender, MouseButtonEventArgs e)
        {
            auther = (Auther)DataGrid2.SelectedItem;
            DataGrid2.DataContext = auther;
        }
        private void Btn_Insert(object sender, RoutedEventArgs e)
        {
            AutherAdd addAuther = new AutherAdd
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            addAuther.ShowDialog();
            //添加之后，flush界面
            //致空
            auther = null;
            //刷新界面
            AutherList = authDAO.ListAll();
            AutherCollection = new ObservableCollection<Auther>(AutherList);
            ((this.FindName("DataGrid1")) as DataGrid).ItemsSource = AutherCollection;
        }
        private void BTN_Update(object sender, RoutedEventArgs e)
        {
            AutherUpdate autherUpdate = new AutherUpdate
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            //自定义窗口更新使用
            Auther auther = (Auther)DataGrid1.SelectedItem;
            autherUpdate.selectedAuther = auther;
            //UI中使用
            autherUpdate.UserName.Text = auther.Auth_UserName;
            autherUpdate.Pass.Password = auther.Auth_UserPass;
            autherUpdate.Confirm_Pass.Password = auther.Auth_UserPass;
            autherUpdate.ShowDialog();
            //致空
            auther = null;
            //刷新界面
            AutherList = authDAO.ListAll();
            AutherCollection = new ObservableCollection<Auther>(AutherList);
            ((this.FindName("DataGrid1")) as DataGrid).ItemsSource = AutherCollection;
        }
        private void Btn_Confirm(object sender, RoutedEventArgs e)
        {


        }

        private void Btn_Activate(object sender, RoutedEventArgs e)
        {
            //InputNonPublicInformationPassword
            InputNonPublicInformationPassword passwordInput = new InputNonPublicInformationPassword
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            passwordInput.ShowDialog();
            Status.Content = "已激活";
            Color color = Color.FromArgb(255, 2, 200, 5);
            Status.Foreground = new SolidColorBrush(color);
        }
    }
}
