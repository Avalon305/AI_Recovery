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
using NLog;
using Recovery.entity;
using Recovery.util;
using Recovery.view.dto;

namespace Recovery.view.Pages.ChildWin
{
    /// <summary>
    /// ViewSymptomInformation.xaml 的交互逻辑
    /// </summary>
    public partial class ViewSymptomInformation : Window
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
        private SymptomInfoDTO symptomInfoDTO;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //viewbox.MaxHeight = SystemParameters.WorkArea.Size.Height;
            //viewbox.MaxWidth = SystemParameters.WorkArea.Size.Width;
            this.Height = SystemParameters.WorkArea.Size.Height;
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            //绑定数据
            Dictionary<string, object> dic = (Dictionary<string, Object>) DataContext;
            user = (User) dic["user"];
            symptomInfoDTO = (SymptomInfoDTO) dic["symptomInfoDto"];
            logger.Info("user:" + user + "; symptomInfoDTO:" + symptomInfoDTO);
            Load_Data();
        }

        private void Load_Data()
        {
            //用户信息
            l1.Content = user.User_Name;
            l2.Content = user.Pk_User_Id;
            //date.DateTime = symptomInfoDTO.Create;
            ;
            //实施日期
            date.Content = symptomInfoDTO.Create.ToString();

            bloodlow_1.Text = symptomInfoDTO.Pre_Pressure.Split(new char[] { '/' })[1].Trim();
            bloodhight_1.Text = symptomInfoDTO.Pre_Pressure.Split(new char[] { '/' })[0].Trim();
            heartRate_1.Text = symptomInfoDTO.Pre_HeartRate;
            
            if (!symptomInfoDTO.Pre_Pulse.Equals(""))
            {
                if (LanguageUtils.EqualsResource(symptomInfoDTO.Pre_Pulse, "VitalInfoView.Regular"))
                {
                    rule_1.IsChecked = true;
                    irregular_1.IsChecked = false;
                }
                else if (LanguageUtils.EqualsResource(symptomInfoDTO.Pre_Pulse, "VitalInfoView.Irregular"))
                {
                    rule_1.IsChecked = false;
                    irregular_1.IsChecked = true;
                }
            } else
            {
                rule_1.IsChecked = false;
                irregular_1.IsChecked = false;
            }

            heat_1.Text = symptomInfoDTO.Pre_AnimalHeat;

            bloodlow_2.Text = symptomInfoDTO.Suf_Pressure.Split(new char[] { '/' })[1].Trim();
            bloodhight_2.Text = symptomInfoDTO.Suf_Pressure.Split(new char[] { '/' })[0].Trim();
            heartRate_2.Text = symptomInfoDTO.Suf_HeartRate;
            if (!symptomInfoDTO.Suf_Pulse.Equals("")) {

                if (LanguageUtils.EqualsResource(symptomInfoDTO.Suf_Pulse, "VitalInfoView.Regular"))
                {
                    rule_2.IsChecked = true;
                    irregular_2.IsChecked = false;
                } else if(LanguageUtils.EqualsResource(symptomInfoDTO.Suf_Pulse, "VitalInfoView.Irregular"))
                {
                    rule_2.IsChecked = false;
                    irregular_2.IsChecked = true;
                }
            }else
            {
                rule_2.IsChecked = false;
                irregular_2.IsChecked = false;

            }

            heat_2.Text = symptomInfoDTO.Suf_AnimalHeat;

            foreach (string inquiry in symptomInfoDTO.Inquiry.Split(new char[]{','}))
            {
                if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Janguidness"))
                {
                    Janguidness.IsChecked = true;
                }
                else if(LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Diarrhea"))
                {
                    Diarrhea.IsChecked = true;
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Wamble"))
                {
                    Wamble.IsChecked = true;
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.BeingBreathless"))
                {
                    BeingBreathless.IsChecked = true;
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.CoughAndPhlegm"))
                {
                    CoughAndPhlegm.IsChecked = true;
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Fever"))
                {
                    Fever.IsChecked = true;
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Stomachache"))
                {
                    Stomachache.IsChecked = true;
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.APoorAppetite"))
                {
                    APoorAppetite.IsChecked = true;
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Constipation"))
                {
                    Constipation.IsChecked = true;
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Dizziness"))
                {
                    Dizziness.IsChecked = true;
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Headache"))
                {
                    Headache.IsChecked = true;
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.Other"))
                {
                    Other.IsChecked = true;
                }
                else if (LanguageUtils.EqualsResource(inquiry, "VitalInfoView.NotApplicable"))
                {
                    NotApplicable.IsChecked = true;
                }
            }
            
            if (LanguageUtils.EqualsResource(symptomInfoDTO.Join, "VitalInfoView.Yes"))
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
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;
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
        private void viewbox_load(object sender, RoutedEventArgs e)
        {
            //this.Visibility = Visibility.Collapsed;
            this.Width = viewbox.ActualWidth;
            this.Height = viewbox.ActualHeight;

            Left = (SystemParameters.WorkArea.Size.Width - this.ActualWidth) / 2;
            Top = (SystemParameters.WorkArea.Size.Height - this.ActualHeight) / 2;
            //this.ShowDialog();
            //this.
            // this.Opacity = 1;
            //  this.Visibility = Visibility.Visible;

        }
    }
}