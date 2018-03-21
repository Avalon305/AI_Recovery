﻿using Dapper;
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
        List<Auther> AutherList = new List<Auther>();
        List<DeviceSort> DeviceSortList = new List<DeviceSort>();
        List<DeviceSet> DeviceSetList = new List<DeviceSet>();
        //创建DAO实例
        AuthDAO authDAO = new AuthDAO();
        DeviceSortDAO deviceSortDAO = new DeviceSortDAO();
        DeviceSetDAO deviceSetDAO = new DeviceSetDAO();
        SetterDAO SetterDAO = new SetterDAO();
        Auther auther = new Auther();
        DeviceSet deviceSet = new DeviceSet();
        int selected = 0;
        public AdvancedSettings()
        {
            InitializeComponent();
            byte auth_level = 0x01;
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
            AutherList = authDAO.ListAll();
            ((this.FindName("DataGrid1")) as DataGrid).ItemsSource = AutherList;
        }
        //添加按钮的事件
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
            FlushAuther();
        }
        //删除按钮的事件
        private void Btn_Delete(object sender, RoutedEventArgs e) //单击删除按钮触发事件
        {
            if (selected == 1)
            {
                authDAO.DeleteByPrimaryKey(auther);//在数据库中删除
                FlushAuther();
                selected = 0;
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
            deviceSortDAO.UpdateDeviceSorts(DeviceSortList);

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
            if (true)//是否和发送的数据体相等，相等则鉴权成功，向数据库中写入mac,没实现！！！
            {   //获取mac地址
                string strMac = CommUtil.GetMacAddress();
                entity.Setter setter = new entity.Setter();
                //mac地址先变为byte[]再aes加密
                byte[] byteMac = Encoding.Default.GetBytes(strMac);
                byte[] AesMac = AesUtil.Encrypt(byteMac, ProtocolConstant.USB_DOG_PASSWORD);
                //变为16进制字符串存入数据库
                setter.Set_Unique_Id = ProtocolUtil.BytesToString(AesMac);
                SetterDAO.InsertOneMacAdress(setter);
                //注释的部分为添加多个mac地址
                // List<entity.Setter> ListMac = CommUtil.GetMacByWMI();
                // SetterDAO.InsertMacAdress(ListMac);
            }
            else
            {
                MessageBox.Show("激活失败");
            }
            Status.Content = "已激活";//如果激活成功记录状态，激活按钮变为不能点击 没实现！！！
            Color color = Color.FromArgb(255, 2, 200, 5);
            Status.Foreground = new SolidColorBrush(color);
        }
    }
}
