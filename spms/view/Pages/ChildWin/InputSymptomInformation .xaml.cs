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
using spms.dao;
using spms.entity;
using spms.service;
using spms.view.Pages.ChildWin;
namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// InputSymptomInformation.xaml 的交互逻辑
    /// </summary>
    public partial class InputSymptomInformation : Window
    {
        public InputSymptomInformation()
        {
            InitializeComponent();

            l1.Content = "andianl";
            user_id.Content = "13210104659";
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            //获取日期
            string da = date.SelectedDate.ToString();
            
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
            //康复前体温
            string preAnimalheat = heat_1.Text;
            
            //康复后血压
            string sufLowPressure = bloodlow_2.Text;
            string sufHighPressure = bloodhight_2.Text;
            //康复后心率
            string sufHeartRate = heartRate_2.Text;
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
            //康复后体温
            string sufAnimalheat = heat_2.Text;

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

            //摄取水分量
            string waterInput = amunt.Text;

            //看护记录
            string careInfo = Record.Text;

            //构建对象
            SymptomInfo symptomInfo = new SymptomInfo();
            SymptomInfoChild symptomInfoChild = new SymptomInfoChild();
            //症状信息
            symptomInfo.Fk_User_Id = 2;
            symptomInfo.Gmt_Create = DateTime.Now;
            symptomInfo.Gmt_Modified = DateTime.Now;
            symptomInfo.SI_CareInfo = careInfo;
            symptomInfo.SI_Inquiry = inquiryStr;
            symptomInfo.SI_IsJoin = isJoin;
            symptomInfo.SI_WaterInput = waterInput;
            //康复前
            symptomInfoChild.Fk_SI_Id = 2;
            symptomInfoChild.Gmt_Create = DateTime.Now;
            symptomInfoChild.Gmt_Modified = DateTime.Now;
            symptomInfoChild.SIC_AnimalHeat = preAnimalheat;
            symptomInfoChild.SIC_HeartRate = preHeartRate;
            symptomInfoChild.SIC_HighPressure = preHighPressure;
            symptomInfoChild.SIC_LowPressure = preLowPressure;
            symptomInfoChild.SIC_Pulse = prePulse;
            symptomInfoChild.Status = 1;
            //康复后
            symptomInfoChild.Fk_SI_Id = 2;
            symptomInfoChild.Gmt_Create = DateTime.Now;
            symptomInfoChild.Gmt_Modified = DateTime.Now;
            symptomInfoChild.SIC_AnimalHeat = sufAnimalheat;
            symptomInfoChild.SIC_HeartRate = sufHeartRate;
            symptomInfoChild.SIC_HighPressure = sufHighPressure;
            symptomInfoChild.SIC_LowPressure = sufLowPressure;
            symptomInfoChild.SIC_Pulse = sufPulse;
            symptomInfoChild.Status = 2;

            //存储
            int id = new SymptomService().AddSymptomnInfo(symptomInfo);
            MessageBox.Show("完成:" + id);
        }
    }
}
