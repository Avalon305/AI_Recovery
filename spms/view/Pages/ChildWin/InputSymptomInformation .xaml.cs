using System;
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
using spms.dao;
using spms.entity;
using spms.service;
using spms.view.dto;
using spms.view.Pages.ChildWin;
namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// InputSymptomInformation.xaml 的交互逻辑
    /// </summary>
    public partial class InputSymptomInformation : Window
    {
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        
        private User user;
        private TrainDTO trainDto;
        public InputSymptomInformation()
        {
            InitializeComponent();
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
            int prePulse = 0;
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
                MessageBox.Show("请选择康复前脉症状");
                return;
            }

            //康复前体温
            string preAnimalheat = heat_1.Text;
            
            //康复后血压
            string sufLowPressure = bloodlow_2.Text;
            string sufHighPressure = bloodhight_2.Text;
            if (sufLowPressure.Trim() == "" || sufHighPressure.Trim() == "" || preLowPressure.Trim() == "" || preHighPressure.Trim() == "" || !(Double.Parse(sufLowPressure) > 0 && Double.Parse(sufLowPressure) < 300) || !(Double.Parse(sufHighPressure) > 0 && Double.Parse(sufHighPressure) < 300) || !(Double.Parse(preHighPressure) > 0 && Double.Parse(preHighPressure) < 300) || !(Double.Parse(preLowPressure) > 0 && Double.Parse(preLowPressure) < 300))
            {
                MessageBox.Show("请输入正确的血压");
                return;
            }
            //康复后心率
            string sufHeartRate = heartRate_2.Text;
            if (sufHeartRate.Trim() == "" || preHeartRate.Trim() == "" || !(Int32.Parse(sufHeartRate) > 0 && Int32.Parse(sufHeartRate) < 200) || !(Int32.Parse(preHeartRate) > 0 && Int32.Parse(preHeartRate) < 200))
            {
                MessageBox.Show("请输入正确的心率");
                return;
            }
            //康复后脉
            int sufPulse = 0;
            if (rule_2.IsChecked == true)
            {//规律脉
                sufPulse = 0;
            }
            else if (irregular_2.IsChecked == true)
            {//脉律不齐
                sufPulse = 1;
            }
            else
            {
                MessageBox.Show("请选择康复后脉症状");
                return;
            }

            //康复后体温
            string sufAnimalheat = heat_2.Text;
            if (sufAnimalheat.Trim() == "" || preAnimalheat.Trim() == "" || !(Double.Parse(sufAnimalheat) < 50 && Double.Parse(sufAnimalheat) > 30) || !(Double.Parse(preAnimalheat) < 50 && Double.Parse(preAnimalheat) > 30))
            {
                MessageBox.Show("请输入正确的体温");
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
                MessageBox.Show("请选择是否参加");
                return;
            }

            int tiId = new DevicePrescriptionDAO().GetTIIdByPRId(trainDto.prescriptionResult.Pk_PR_Id);
            //摄取水分量
            string waterInput = amunt.Text;
            if (waterInput.Trim() == "" || Double.Parse(waterInput) < 0)
            {
                MessageBox.Show("请输入正确的摄水量");
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
            symptomInfo.Fk_TI_Id = tiId;
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

            //存储
            new SymptomService().AddSymptomnInfo(symptomInfo);
            MessageBox.Show("已存储");
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewbox.MaxHeight = SystemParameters.WorkArea.Size.Height;
            viewbox.MaxWidth = SystemParameters.WorkArea.Size.Width;
            //去除窗体叉号
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            //
            Dictionary<string, Object> dictionary = (Dictionary<string, Object>)DataContext;
            user = (User)dictionary["user"];
            trainDto = (TrainDTO) dictionary["trainDto"];
            l1.Content = user.User_Name;
            user_id.Content = user.Pk_User_Id;
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
    }
}
