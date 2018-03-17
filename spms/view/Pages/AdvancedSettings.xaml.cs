using Dapper;
using spms.dao;
using spms.entity;
using spms.util;
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
            AutherCollection = new ObservableCollection<Auther>(AutherList);
            DeviceSetCollection = new ObservableCollection<DeviceSet>(DeviceSetList);
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
        private void Btn_Insert(object sender, RoutedEventArgs e)
        {

            judge = 1;  //现在为添加状态       
            DataGrid1.CanUserAddRows = true;
            //点击添加后  将CanUserAddRows重新设置为True，这样DataGrid就会自动生成新行，我们就能在新行中输入数据了。  
        }
        private void BTN_Update(object sender, RoutedEventArgs e)
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Execute("update bdl_auth set Auth_UserName=@Auth_UserName,Gmt_Create=@Gmt_Create ,Gmt_Modified=@Gmt_Modified,User_Status=@User_Status,Auth_OfflineTime=@Auth_OfflineTime where Pk_Auth_Id=@Pk_Auth_Id", AutherCollection);
            }
        }
        private void Btn_Confirm(object sender, RoutedEventArgs e)
        {
            if (checkchange == 1)
            {
                MessageBox.Show(DeviceSetCollection[0].DSet_Status.ToString());
                using (var conn = DbUtil.getConn())
                {
                    conn.Execute("update bdl_deviceset set DSet_Status=@DSet_Status where DSet_Name=@DSet_Name", DeviceSetCollection);
                }
            }
            DataGrid1.CanUserAddRows = false;
            auther = DataGrid1.SelectedItem as Auther;//获取该行的记录  
            if (judge == 1)//如果是添加状态就保存该行的值到lstInformation中  这样我们就完成了新行值的获取  
            {
                authDAO.Insert(auther);
            }

        }
    }
}
