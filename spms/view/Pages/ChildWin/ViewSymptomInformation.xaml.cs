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
using spms.entity;
using spms.view.dto;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// ViewSymptomInformation.xaml 的交互逻辑
    /// </summary>
    public partial class ViewSymptomInformation : Window
    {
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private User user;
        private SymptomInfoDTO symptomInfoDTO;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            //绑定数据
            Dictionary<string, object> dic = (Dictionary<string, Object>) DataContext;
            user = (User) dic["user"];
            symptomInfoDTO = (SymptomInfoDTO) dic["symptomInfoDto"];

            Load_Data();
        }

        private void Load_Data()
        {
            //用户信息
            l1.Content = user.User_Name;
            l2.Content = user.Pk_User_Id;

            //实施日期
            date.Text = symptomInfoDTO.Create.ToString();
            bloodlow_1.Text = symptomInfoDTO.Pre_Pressure.Split(new char[] {'/'})[1].Trim();
            bloodhight_1.Text = symptomInfoDTO.Pre_Pressure.Split(new char[] {'/'})[0].Trim();
            heartRate_1.Text = symptomInfoDTO.Pre_HeartRate;
            if (symptomInfoDTO.Pre_Pulse == "规律脉")
            {
                rule_1.IsChecked = true;
                irregular_1.IsChecked = false;
            }
            else
            {
                rule_1.IsChecked = true;
                irregular_1.IsChecked = false;
            }

            heat_1.Text = symptomInfoDTO.Pre_AnimalHeat;

            bloodlow_2.Text = symptomInfoDTO.Suf_Pressure.Split(new char[] {'/'})[1].Trim();
            bloodhight_2.Text = symptomInfoDTO.Suf_Pressure.Split(new char[] {'/'})[0].Trim();
            heartRate_2.Text = symptomInfoDTO.Suf_HeartRate;
            if (symptomInfoDTO.Suf_Pulse == "规律脉")
            {
                rule_2.IsChecked = true;
                irregular_2.IsChecked = false;
            }
            else
            {
                rule_2.IsChecked = false;
                irregular_2.IsChecked = true;
            }

            heat_2.Text = symptomInfoDTO.Suf_AnimalHeat;

            foreach (CheckBox chk in this.stackPanel_1.Children.OfType<CheckBox>())
            {
                if (symptomInfoDTO.Inquiry.Contains(chk.Content.ToString()))
                {
                    chk.IsChecked = true;
                }
            }

            foreach (CheckBox chk in this.stackPanel_2.Children.OfType<CheckBox>())
            {
                if (symptomInfoDTO.Inquiry.Contains(chk.Content.ToString()))
                {
                    chk.IsChecked = true;
                }
            }


            if (symptomInfoDTO.Join == "是")
            {
                join_1.IsChecked = true;
                join_2.IsChecked = false;
            }
            else
            {
                join_1.IsChecked = false;
                join_2.IsChecked = true;
            }

            amunt.Text = symptomInfoDTO.WaterInput;
            Record.Text = symptomInfoDTO.CareInfo;
        }

        public ViewSymptomInformation()
        {
            InitializeComponent();
        }

        //取消操作，关闭窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               Cancel(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }
    }
}