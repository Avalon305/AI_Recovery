﻿using Dapper;
using Recovery.constant;
using Recovery.dao;
using Recovery.entity;
using Recovery.protocol;
using Recovery.service;
using Recovery.util;
using Recovery.view.Pages.ChildWin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
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

namespace Recovery.view.Pages
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
        AuthService authService = new AuthService();
        DeviceSet deviceSet = new DeviceSet();
        List<entity.Setter> setterList = new List<entity.Setter>();
        SetterDAO setterDao = new SetterDAO();
        int selected = 0;
        byte auth_level = 0x01;
        int Pk_Set_Id;

        public AdvancedSettings()
        {

            InitializeComponent();
            //this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            //this.MaxWidth = SystemParameters.WorkArea.Size.Width;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //加载图片
            if (LanguageUtils.IsChainese())
            {
                title_pic.Source = new BitmapImage(new Uri(@"\view\Images\bdl.PNG", UriKind.Relative));
                title_pic.Height = 198.4;
            }
            else
            {
                //TODO 英文图片
                title_pic.Source = new BitmapImage(new Uri(@"\view\Images\design.png", UriKind.Relative));
                title_pic.Height = 257;
            }
            try
            {
                if (SetterDAO.getSetter() != null)
                {
                    if (SetterDAO.getSetter().Set_Unique_Id != "" && SetterDAO.getSetter().Set_Unique_Id != null)//判断是否激活
                    {
                        Status.Content = LanguageUtils.GetCurrentLanuageStrByKey("SettingsView.Activated");
                        Color color = Color.FromArgb(255, 2, 200, 5);
                        Status.Foreground = new SolidColorBrush(color);
                        BtnActivite.IsEnabled = false;
                    }
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
            try
            {
                List<Auther> AutherList = new List<Auther>();
                auther = authDAO.GetAuther(auth_level);
                AutherList.Add(auther);
                ((this.FindName("DataGrid1")) as DataGrid).ItemsSource = AutherList;
            }
            catch (Exception ee) { }
            try
            {
                DeviceSetList = deviceSetDAO.ListAll();
                ((this.FindName("ComboBox_Device")) as ComboBox).ItemsSource = DeviceSetList;//系列
                int Dset_Id = (int)ComboBox_Device.SelectedValue;
                DeviceSortList = deviceSortDAO.GetDeviceSortBySet(Dset_Id);
                ((this.FindName("DataGrid2")) as DataGrid).ItemsSource = DeviceSortList;//类型
            }
            catch (Exception ee)
            {
            }

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
            try
            { //添加之后，flush界面
              //致空
                auther = null;
                //刷新界面
                Auther AutherTemp = authDAO.GetAuther(auth_level);
                List<Auther> AutherList = new List<Auther>();
                AutherList.Add(AutherTemp);
                ((this.FindName("DataGrid1")) as DataGrid).ItemsSource = AutherList;
            }
            catch (Exception ee)
            {
                List<Auther> AutherList = null;
                ((this.FindName("DataGrid1")) as DataGrid).ItemsSource = AutherList;
            }

        }
        //添加按钮的事件
        private void Btn_Insert(object sender, RoutedEventArgs e)
        {
            int count = authDAO.GetAuthCount();
            if (SetterDAO.ListAll().Count != 0)
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
                    authService.UpLoadAuth();
                }
                else
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("最多只允许存在一个用户", "Only one user is allowed"));
                }
            }
            else
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("您没有添加权限请先激活", "You did not add permission, please activate first"));
            }
        }
        //删除按钮的事件
        private void Btn_Delete(object sender, RoutedEventArgs e) //单击删除按钮触发事件
        {
            if (selected == 1)
            {
                if (MessageBoxX.Question(LanguageUtils.ConvertLanguage("你确认要删除所选项吗", "Do you want to delete the selected item?")))
                {
                    authDAO.DeleteByPrimaryKey(auther);//在数据库中删除
                    FlushAuther();
                    selected = 0;
                }
            }
            else
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择删除的一行", "Please select an delete row"));
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
                if (authDAO.GetAuther(auth_level).User_Status==2)//auther.Auth_OfflineTime.ToString() == "9999-12-31 00:00:00"
                {
                    autherUpdate.Yes.IsChecked = true;
                    autherUpdate.Confirm_Date.SelectedDate = auther.Auth_OfflineTime;
                }
                else {
                    autherUpdate.No.IsChecked = true;
                    autherUpdate.Confirm_Date.SelectedDate = auther.Auth_OfflineTime;
                }
                          
                autherUpdate.ShowDialog();
                FlushAuther();
                selected = 0;
            }
            else
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择更新的一行", "Please select an updated row"));
            }
        }
        private void Btn_Confirm(object sender, RoutedEventArgs e)
        {
            if (MessageBoxX.Question(LanguageUtils.ConvertLanguage("你确认要保存更改吗", "Are you sure you want to save your changes?")))
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
            if (ProtocolConstant.USB_SUCCESS == 0)//u盘成功读取 破解状态为0
            {   //获取mac地址
                StringBuilder stringBuilder = new StringBuilder();
                //string strMac = CommUtil.GetMacAddress();
                // List<string> Macs = CommUtil.GetMacByWMI();
                List<string> Macs = CommUtil.GetMacByIPConfig();
                foreach (string mac in Macs)
                {
                    string prefix = "物理地址. . . . . . . . . . . . . : ";
                    string Mac=mac.Substring(prefix.Length-1);
                    stringBuilder.Append(Mac);
                }
                //Console.WriteLine("==================="+stringBuilder.ToString());
                //MessageBox.Show("===================" + stringBuilder.ToString());
                entity.Setter setter = new entity.Setter();
                //mac地址先变为byte[]再aes加密
                byte[] byteMac = Encoding.GetEncoding("GBK").GetBytes(stringBuilder.ToString());
                byte[] AesMac = AesUtil.Encrypt(byteMac, ProtocolConstant.USB_DOG_PASSWORD);
                //存入数据库
                //setter.Set_Unique_Id = Encoding.GetEncoding("GBK").GetString(AesMac);
                setter.Set_Unique_Id = ProtocolUtil.BytesToString(AesMac);
                /*AES解密
                 * byte[] a = ProtocolUtil.StringToBcd(setter.Set_Unique_Id);
                byte[] b = AesUtil.Decrypt(a, ProtocolConstant.USB_DOG_PASSWORD);
                Console.WriteLine(Encoding.GetEncoding("GBK").GetString(b));*/
                //默认照片路径，激活时获取(路径中不要有汉字)
                string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
                string path = ConfigurationManager.AppSettings["PicPath"];
                setter.Set_PhotoLocation = basePath + path;
                setter.Set_Language = 1;
                //默认备份路径，激活时获取
                setter.Back_Up = basePath +@"BackUp\";
                setter.Set_Language = 1;
                //设置版本号
                setter.Set_Version = CommUtil.GetCurrentVersion();
                if (!Directory.Exists(@setter.Set_PhotoLocation))
                {
                    Directory.CreateDirectory(@setter.Set_PhotoLocation);//不存在就创建目录
                }
                /*if (Directory.Exists(@setter.Set_PhotoLocation)) {  //存在就删除
                    Directory.Delete(@setter.Set_PhotoLocation, true);
                    Directory.CreateDirectory(@setter.Set_PhotoLocation);
                }*/
                SetterDAO.InsertOneMacAdress(setter);
                //注释的部分为添加多个mac地址
                // List<entity.Setter> ListMac = CommUtil.GetMacByWMI();
                // SetterDAO.InsertMacAdress(ListMac);




                Status.Content = LanguageUtils.GetCurrentLanuageStrByKey("SettingsView.Activated");
                Color color = Color.FromArgb(255, 2, 200, 5);
                Status.Foreground = new SolidColorBrush(color);
                BtnActivite.IsEnabled = false;
            }
            else
            {
                MessageBoxX.Error(LanguageUtils.ConvertLanguage("激活失败", "Activation fails"));
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
