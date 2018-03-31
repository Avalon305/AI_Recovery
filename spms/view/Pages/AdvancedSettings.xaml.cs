using Dapper;
using spms.constant;
using spms.dao;
using spms.entity;
using spms.protocol;
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
        //声明List
        List<DeviceSort> DeviceSortList = new List<DeviceSort>();
        List<DeviceSet> DeviceSetList = new List<DeviceSet>();
        //创建DAO实例
        AuthDAO authDAO = new AuthDAO();
        DeviceSortDAO deviceSortDAO = new DeviceSortDAO();
        DeviceSetDAO deviceSetDAO = new DeviceSetDAO();
        SetterDAO SetterDAO = new SetterDAO();
        Auther auther = new Auther();
        DeviceSet deviceSet = new DeviceSet();
        List<entity.Setter> setterList = new List<entity.Setter>();
        SetterDAO setterDao = new SetterDAO();
        int selected = 0;
        byte auth_level = 0x01;
        int Pk_Set_Id;

        public AdvancedSettings()
        {

            InitializeComponent();

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SetterDAO.getSetter().Set_Unique_Id != "" && SetterDAO.getSetter().Set_Unique_Id != null)//判断是否激活
                {
                    Status.Content = "已激活";
                    Color color = Color.FromArgb(255, 2, 200, 5);
                    Status.Foreground = new SolidColorBrush(color);
                    BtnActivite.IsEnabled = false;
                }
            }
            catch (InvalidOperationException ee)
            {
            }
            setterList = setterDao.ListAll();
            try { Pk_Set_Id = setterList[0].Pk_Set_Id; }
            catch (ArgumentOutOfRangeException ee)
            {
            }

            List<Auther> AutherList = new List<Auther>();
            auther = authDAO.GetByAuthLevel(auth_level);
            AutherList.Add(auther);
            DeviceSetList = deviceSetDAO.ListAll();
            ((this.FindName("DataGrid1")) as DataGrid).ItemsSource = AutherList;
            ((this.FindName("ComboBox_Device")) as ComboBox).ItemsSource = DeviceSetList;//系列
            int Dset_Id = (int)ComboBox_Device.SelectedValue;
            DeviceSortList = deviceSortDAO.GetDeviceSortBySet(Dset_Id);
            ((this.FindName("DataGrid2")) as DataGrid).ItemsSource = DeviceSortList;//类型

        }
        //返回上一页
        private void GoBack(object sender, RoutedEventArgs e)
        {
            Window window = (Window)this.Parent;
            window.Content = new DesignPage1();
        }
        //datagrid单击一行事件
        private void Grid_Row_Click(object sender, RoutedEventArgs e)
        {
            selected = 1;
            auther = (Auther)DataGrid1.SelectedItem;
            DataGrid1.DataContext = auther;
        }
        //刷新页面
        private void FlushAuther()
        {
            //添加之后，flush界面
            //致空
            auther = null;
            //刷新界面
            Auther AutherTemp = authDAO.GetByAuthLevel(auth_level);
            List<Auther> AutherList = new List<Auther>();
            AutherList.Add(AutherTemp);
            ((this.FindName("DataGrid1")) as DataGrid).ItemsSource = AutherList;
        }
        //添加按钮的事件
        private void Btn_Insert(object sender, RoutedEventArgs e)
        {
            int count = authDAO.GetAuthCount();
            try
            {
                if (SetterDAO.getSetter().Set_Unique_Id != "" && SetterDAO.getSetter().Set_Unique_Id != null)
                {//判断是否激活
                    if (count < 2)
                    {
                        AutherAdd addAuther = new AutherAdd
                        {
                            Owner = Window.GetWindow(this),
                            ShowActivated = true,
                            ShowInTaskbar = false,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen
                        };
                        addAuther.ShowDialog();
                        FlushAuther();
                    }
                    else
                    {
                        MessageBox.Show("最多只允许存在一个用户");
                    }
                }

            }
            catch (InvalidOperationException ee)
            {
                MessageBox.Show("您没有添加权限请先激活");
            }
        }
        //删除按钮的事件
        private void Btn_Delete(object sender, RoutedEventArgs e) //单击删除按钮触发事件
        {
            if (selected == 1)
            {
                if (MessageBox.Show("你确认要删除所选项吗", "提示：", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    authDAO.DeleteByPrimaryKey(auther);//在数据库中删除
                    FlushAuther();
                    selected = 0;
                }
            }
            else
            {
                MessageBox.Show("请选择删除的一行");
            }

        }
        //更新按钮的事件
        private void BTN_Update(object sender, RoutedEventArgs e)
        {
            if (selected == 1)
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
                FlushAuther();
                selected = 0;
            }
            else
            {
                MessageBox.Show("请选择更新的一行");
            }
        }
        private void Btn_Confirm(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("你确认要保存更改吗", "提示：", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                deviceSortDAO.UpdateDeviceSorts(DeviceSortList);
            }

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
            if (ProtocolConstant.USB_SUCCESS == 0)//u盘成功读取
            {   //获取mac地址
                string strMac = CommUtil.GetMacAddress();
                entity.Setter setter = new entity.Setter();
                //mac地址先变为byte[]再aes加密
                byte[] byteMac = Encoding.GetEncoding("GBK").GetBytes(strMac);
                byte[] AesMac = AesUtil.Encrypt(byteMac, ProtocolConstant.USB_DOG_PASSWORD);
                //存入数据库
                setter.Set_Unique_Id = Encoding.GetEncoding("GBK").GetString(AesMac);
                //setter.Pk_Set_Id = Pk_Set_Id;
                //MessageBox.Show(Pk_Set_Id.ToString());
                //SetterDAO.UpdateOneSet(setter);更新
                SetterDAO.InsertOneMacAdress(setter);
                //注释的部分为添加多个mac地址
                // List<entity.Setter> ListMac = CommUtil.GetMacByWMI();
                // SetterDAO.InsertMacAdress(ListMac);
                Status.Content = "已激活";
                Color color = Color.FromArgb(255, 2, 200, 5);
                Status.Foreground = new SolidColorBrush(color);
                BtnActivite.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("激活失败");
            }



        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Btn_Confirm(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }
    }
}
