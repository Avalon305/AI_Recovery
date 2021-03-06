﻿using System;
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
using Recovery.util;

namespace Recovery.view.Pages.ChildWin
{
    /// <summary>
    /// AdvancedSettingPassWord.xaml 的交互逻辑
    /// </summary>
    public partial class AdvancedSettingPassWord : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        public int IsTrue = 0;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        public AdvancedSettingPassWord()
        {
            InitializeComponent();
            this.Width = SystemParameters.WorkArea.Size.Width * 0.33;
            this.Height = this.Width / 2.57;
        }
        private void Confirm(object sender, RoutedEventArgs e)
        {
            string Password = PasswordText.Password;
            if (Password == "111")
            {
                IsTrue = 1;
                this.Close();
            }
            else {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("密码错误，请重新输入", "Password error, please retype"));
            }
            

        }
        private void Cancel(object sender, RoutedEventArgs e)
        {     
            this.Close();
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               Confirm(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }
    }
}
