﻿using Recovery.constant;
using Recovery.dao;
using Recovery.entity;
using Recovery.protocol;
using Recovery.util;
using Recovery.view;
using Recovery.view.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Recovery
{
    /// <summary>
    /// FmyTest.xaml 的交互逻辑
    /// </summary>
    public partial class FmyTest : Window
    {
        public FmyTest()
        {
            InitializeComponent();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] idcard = Encoding.GetEncoding("GBK").GetBytes("370111111111111115");

            byte[] req = MakerTCPFrame.GetInstance().PackData(MsgId.X000A, "123456789012", idcard);
            byte[] request = new byte[req.Length - 2];
            Array.Copy(req, 1, request, 0, req.Length - 2);

            new ParserTCPFrame().Parser(request);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            byte[] body = new byte[34];
            byte[] idcard = Encoding.GetEncoding("GBK").GetBytes("370111111111111115");
            Array.Copy(idcard, 0, body, 0, idcard.Length);
            body[32] = 0x01;
            body[33] = 0x00;

            byte[] req = MakerTCPFrame.GetInstance().PackData(MsgId.X0007, "123456789012", body);
            byte[] request = new byte[req.Length - 2];
            Array.Copy(req, 1, request, 0, req.Length - 2);

            new ParserTCPFrame().Parser(request);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new Login().Show();

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            byte[] content = Encoding.ASCII.GetBytes("abcdrfghijklmnopqrstuvwxyz12345678901");

            byte[] jiamihou = AesUtil.Encrypt(content, ProtocolConstant.USB_DOG_PASSWORD);

            var jiemihou = AesUtil.Decrypt(jiamihou, ProtocolConstant.USB_DOG_PASSWORD);

            var str = Encoding.ASCII.GetString(jiemihou);
            MessageBoxX.Info(str);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var u = new UserDAO().GetByIdCard("370111111111111115");

            MessageBox.Show(u.User_Name);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var u = new UserDAO().Load(1);
            User u1 = new User();
            u1.Pk_User_Id = 1;
            u1.User_IDCard = "23456789";
            new UserDAO().UpdateByPrimaryKey(u1);
        }

        //载入时从数据库加载数据源
        private void TestCombox_Loaded(object sender, RoutedEventArgs e)
        {
            List<DataCode> list = DataCodeCache.GetInstance().GetDateCodeList(DataCodeTypeEnum.MoveWay);

            TestCombox.ItemsSource = list;
        }
        //选中时直接获取编码值
        private void TestCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            view.Pages.MessageBoxX.Info(TestCombox.SelectedValue.ToString());
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            var a = new AuthDAO().GetByAuthLevel(2);
            view.Pages.MessageBoxX.Info(a.User_Status.ToString());
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            //空格分隔
            /*
                当前版本号（0.9.0.0）
                升级的版本号（1.0.0.0）
                更新说明URL（https://github.com/WELL-E）
                更新包文件URL（ http://localhost:9090/UpdateFile.zip）
                更新后文件释放路径（E:\PlatformPath）
                更新包文件MD5码（2b406701f8ad92922feb537fc789561a）
           
             */
            string args = "0.12.2 1.0.0 http://github.com http://localhost:9090/UpdateFile.zip E:\\pppl 2b406701f8ad92922feb537fc789561a";
            Process.Start("AutoUpdater.exe", args);
        }
    }
}
