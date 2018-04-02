﻿using spms.dao;
using spms.entity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            //获取文本框的值
            string Name = UserName.Text;
            Auther AutherTemp = authDAO.GetByName(Name);
            if (AutherTemp != null)
            {
                MessageBox.Show("用户名已存在");
            }
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
            if ((bool)No.IsChecked)
            {
                selectedAuther.Auth_OfflineTime = Confirm_Date.SelectedDate;
            }
            else
            {
                DateTime date = (DateTime)Auther.Auth_OFFLINETIMEFREE;
                string sdate = string.Format("{0:yyyy-MM-dd}", date);
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy-MM-dd";
                selectedAuther.Auth_OfflineTime = Convert.ToDateTime(sdate, dtFormat);
            }
            string PassWord = Pass.Password;
            string REPassword = Confirm_Pass.Password;
            if (PassWord.Equals(REPassword))
            {
                selectedAuther.Auth_UserPass = PassWord;
                authDAO.UpdateByPrimaryKey(selectedAuther);


            }
            this.Close();
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_OK(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }
    }
}
