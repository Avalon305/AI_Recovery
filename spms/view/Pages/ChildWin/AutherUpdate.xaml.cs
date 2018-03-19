﻿using spms.dao;
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
    /// UserUpdate.xaml 的交互逻辑
    /// </summary>
    public partial class AutherUpdate : Window
    {
        public Auther selectedAuther = new Auther();
        AuthDAO authDAO = new AuthDAO();
        public AutherUpdate()
        {
            InitializeComponent();
        }
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //取消按钮，关闭此窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void Button_OK(object sender, RoutedEventArgs e)
        { //获取控件值
            selectedAuther.Auth_UserName = UserName.Text;
            selectedAuther.Gmt_Modified = DateTime.Now;
            selectedAuther.Auth_OfflineTime = DateTime.Now;
            string PassWord = Pass.Password;
            string REPassword = Confirm_Pass.Password;
            if (PassWord.Equals(REPassword))
            {
                selectedAuther.Auth_UserPass = PassWord;
                authDAO.UpdateByPrimaryKey(selectedAuther);


            }
            this.Close();
        }
    }
}