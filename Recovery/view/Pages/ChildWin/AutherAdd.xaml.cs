﻿using Recovery.dao;
using Recovery.entity;
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
using Recovery.util;

namespace Recovery.view.Pages.ChildWin
{
    /// <summary>
    /// UserAdd.xaml 的交互逻辑
    /// </summary>
    public partial class AutherAdd : Window
    {
        AuthDAO authDAO = new AuthDAO();
        Auther auther = new Auther();
        public AutherAdd()
        {
            InitializeComponent();
            this.Width = SystemParameters.WorkArea.Size.Width * 0.4;
            this.Height = this.Width / 1.8;
            //this.Height = SystemParameters.WorkArea.Size.Height;

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
        //用户名失去焦点触发事件进行姓名查重,没用到！
        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            //获取文本框的值
            string Name = UserName.Text;
            Auther AutherTemp = authDAO.GetByName(Name);
            if (AutherTemp != null)
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("用户名已存在", "The username has already existed"));
            }
        }

        private void Button_OK(object sender, RoutedEventArgs e)
        {
            string PassWord = "";
            if (UserName.Text != null && UserName.Text != "")
            {
                auther.Auth_UserName = UserName.Text;
            }
            else
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("用户名不能为空", "The username can not be empty"));
            }
            auther.Gmt_Create = DateTime.Now;
            auther.Gmt_Modified = DateTime.Now;
            if ((bool)Yes.IsChecked)
            {

                DateTime date = (DateTime)Auther.Auth_OFFLINETIMEFREE;
                string sdate = string.Format("{0:yyyy-MM-dd}", date);
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy-MM-dd";
                auther.Auth_OfflineTime = Convert.ToDateTime(sdate, dtFormat);
                auther.User_Status = 2;
                this.Close();
            }
            else
            {
                if (Confirm_Date.SelectedDate == null)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("离线时间不能为空", "The offlinetime can not be empty"));

                }
                else
                {
                    auther.Auth_OfflineTime = Confirm_Date.SelectedDate;
                    auther.User_Status = 0;
                    
                }


            }
            PassWord = Pass.Password;
          
            string REPassword = Confirm_Pass.Password;
            auther.Auth_Level = 0x01;
            if (UserName.Text != ""&& auther.User_Status != null && PassWord.Equals(REPassword) && PassWord != "")
            {
                auther.Auth_UserPass = PassWord;
                authDAO.Insert(auther);
                this.Close();
            }
            else if (UserName.Text != ""&&PassWord == "")
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("密码不能为空", "The password can not be empty"));
            }
            else if (!PassWord.Equals(REPassword))
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("密码的两次输入不一致", "Two inconsistencies in the password are inconsistencies"));
            }
           

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
