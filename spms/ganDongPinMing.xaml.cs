﻿using spms.constant;
using spms.dao;
using spms.entity;
using spms.http.dto;
using spms.http.entity;
using spms.service;
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

namespace spms
{
    /// <summary>
    /// ganDongPinMing.xaml 的交互逻辑
    /// </summary>
    public partial class ganDongPinMing : Page
    {
        public ganDongPinMing()
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

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            string pingJsonStr = JsonTools.Obj2JSONStrNew(userService.GetAllUsers());
            MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
           DateTime dateTime = Convert.ToDateTime("2000/02/12");
            MessageBox.Show(dateTime.ToShortDateString());
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            User user = userService.GetByIdCard("438");
            List<User> queryResult = userService.SelectByCondition(user);
            foreach (var i in queryResult) {
                MessageBox.Show(JsonTools.Obj2JSONStrNew(i));
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(DataCodeTypeEnum.Diagiosis.ToString());
        }
    }
}
