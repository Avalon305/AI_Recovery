﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NLog;
using Recovery.dao;
using Recovery.entity;
using Recovery.service;
using Recovery.util;
using Recovery.view.dto;
using Recovery.view.Pages.ChildWin;
namespace Recovery.view.Pages.ChildWin
{
    /// <summary>
    /// InputSymptomInformation.xaml 的交互逻辑
    /// </summary>
    public partial class InputSymptomInformation : Window
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        
        private User user;
        //private TrainDTO trainDto;
        public InputSymptomInformation()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;

        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            //获取日期
            //DateTime? da = date.SelectedDate;
            DateTime? da = date.DateTime;
            //康复前血压
            string preLowPressure = bloodlow_1.Text;
            string preHighPressure = bloodhight_1.Text;
            
            //康复前心率
            string preHeartRate = heartRate_1.Text;
            //康复前脉
            int prePulse = -1;
            if (rule_1.IsChecked == true)
            {//规律脉
                prePulse = 0;
            }
            else if (irregular_1.IsChecked == true)
            {//脉律不齐
                prePulse = 1;
            }
            else
            {
                MessageBox.Show(LanguageUtils.ConvertLanguage("请选择康复前脉症状", "Please choose the symptoms of the pre recovery pulse"));
                return;
            }

            //康复前体温
            string preAnimalheat = heat_1.Text;
            
            //康复后血压
            string sufLowPressure = bloodlow_2.Text;
            string sufHighPressure = bloodhight_2.Text;
            if (preLowPressure.Trim() == "" || preHighPressure.Trim() == "" || !(Double.Parse(preHighPressure) > 0 && Double.Parse(preHighPressure) < 300) || !(Double.Parse(preLowPressure) > 0 && Double.Parse(preLowPressure) < 300))
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的血压", "Please enter the right blood pressure"));
                return;
            }
            //康复后心率
            string sufHeartRate = heartRate_2.Text;
            if (preHeartRate.Trim() == "" || !(Int32.Parse(preHeartRate) > 0 && Int32.Parse(preHeartRate) < 200))
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的心率", "Please enter the right heartrate"));
                return;
            }
            int sufPulse = -1;
            if (rule_2.IsChecked == true)
            {//规律脉
                sufPulse = 0;
            }
            else if (irregular_2.IsChecked == true)
            {//脉律不齐
                sufPulse = 1;
            }
            //康复后体温
            string sufAnimalheat = heat_2.Text;
            if ( preAnimalheat.Trim() == "" || !(Double.Parse(preAnimalheat) < 50 && Double.Parse(preAnimalheat) > 30))
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的体温", "Please enter the right temperature"));
                return;
            }

            //问诊票
            List<string> inquiryList = new List<string>();
            foreach (CheckBox chk in this.stackPanel_1.Children.OfType<CheckBox>())
            {
                if (chk.IsChecked == true)
                {
                    inquiryList.Add(chk.Content as string);
                }
            }
            foreach (CheckBox chk in this.stackPanel_2.Children.OfType<CheckBox>())
            {
                if (chk.IsChecked == true)
                {
                    inquiryList.Add(chk.Content as string);
                }
            }
            string inquiryStr = string.Join(",", inquiryList.ToArray());

            //参加不参加
            Byte isJoin = 0;
            if (join_1.IsChecked == true)
            {
                isJoin = 0;
            }
            else if (join_2.IsChecked == true)
            {
                isJoin = 1;
            }
            else
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择是否参加", "Please choose whether or not to participate"));
                return;
            }

            //int tiId = new DevicePrescriptionDAO().GetTIIdByPRId(trainDto.prescriptionResult.Pk_PR_Id);
            //摄取水分量
            string waterInput = amunt.Text;
            if (waterInput.Trim() == "" || Double.Parse(waterInput) < 0)
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的摄水量", "Please enter the correct water intake"));
                return;
            }


            //看护记录
            string careInfo = Record.Text;


            //构建对象
            SymptomInfo symptomInfo = new SymptomInfo();
            
            //症状信息
            symptomInfo.Fk_User_Id = user.Pk_User_Id;
            symptomInfo.Gmt_Create = da;
            symptomInfo.Gmt_Modified = DateTime.Now;
            symptomInfo.SI_CareInfo = careInfo;
            symptomInfo.SI_Inquiry = inquiryStr;
            symptomInfo.SI_IsJoin = isJoin;
            symptomInfo.SI_WaterInput = waterInput;
            if (!string.IsNullOrEmpty(train.Text))
            {
                //如果选择了训练记录
                symptomInfo.Fk_TI_Id = (int) train.SelectedValue;
            }
            //symptomInfo.Fk_TI_Id = tiId;
            //康复前
            symptomInfo.SI_Pre_AnimalHeat = preAnimalheat;
            symptomInfo.SI_Pre_HeartRate = preHeartRate;
            symptomInfo.SI_Pre_HighPressure = preHighPressure;
            symptomInfo.SI_Pre_LowPressure = preLowPressure;
            symptomInfo.SI_Pre_Pulse = prePulse;
            //康复后
            symptomInfo.SI_Suf_AnimalHeat = sufAnimalheat;
            symptomInfo.SI_Suf_HeartRate = sufHeartRate;
            symptomInfo.SI_Suf_HighPressure = sufHighPressure;
            symptomInfo.SI_Suf_LowPressure = sufLowPressure;
            symptomInfo.SI_Suf_Pulse = sufPulse;
            logger.Info("save:" + symptomInfo);
            //存储
            new SymptomService().AddSymptomnInfo(symptomInfo);
            MessageBoxX.Info(LanguageUtils.ConvertLanguage("已存储", "Finished storage"));
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Height = SystemParameters.WorkArea.Size.Height;
            //viewbox.MaxWidth = SystemParameters.WorkArea.Size.Width;
            //去除窗体叉号
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            //
            Dictionary<string, Object> dictionary = (Dictionary<string, Object>)DataContext;
            user = (User)dictionary["user"];
            //trainDto = (TrainDTO) dictionary["trainDto"];
            l1.Content = user.User_Name;
            user_id.Content = user.Pk_User_Id;
            
            List<TrainInfo> trainInfoNoSymp = new TrainInfoDAO().GetTrainInfoNoSymp(user.Pk_User_Id);
            train.ItemsSource = new TrainDTO().ConvertDtoList(trainInfoNoSymp);
        }

        //错误：OnlyInputNumbers
        //设置手机号输入框只能输入数字
        private void OnlyInputNumbers(object sender, TextCompositionEventArgs e)
        {
            inputlimited.InputLimited.OnlyInputNumbers(e);
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               Button_Save(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }

        private void viewbox_load(object sender, RoutedEventArgs e)
        {
            this.Width = viewbox.ActualWidth;
            this.Height = viewbox.ActualHeight;

            Left = (SystemParameters.WorkArea.Size.Width - this.ActualWidth) / 2;
            Top = (SystemParameters.WorkArea.Size.Height - this.ActualHeight) / 2;
        }
    }
}
