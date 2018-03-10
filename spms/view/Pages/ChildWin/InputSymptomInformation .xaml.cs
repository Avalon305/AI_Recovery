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
            l2.Content = "13210104659";
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            //获取日期
            string da = date.SelectedDate.ToString();

            //康复前数据
            //血压
            string bloodlow1 = bloodlow_1.Text;
            string bloodhight1 = bloodhight_1.Text;
            //心率
            string heartRate1 = heartRate_1.Text;
            //脉
            string heart1 = null;
            if (rule_1.IsChecked == true)
            {
                heart1 = rule_1.Content as string;
                // MessageBoxResult messageBoxResult = MessageBox.Show(heart);
            }
            else
            {
                if (irregular_1.IsChecked == true)
                {
                    heart1 = irregular_1.Content as string;
                }

            }
            //体温
            string heat1 = heat_1.Text;

            //康复后数据
            //血压
            string bloodlow2 = bloodlow_2.Text;
            string bloodhight2 = bloodhight_2.Text;
            //心率
            string heartRate2 = heartRate_2.Text;
            //脉
            string heart2 = null;
            if (rule_2.IsChecked == true)
            {
                heart2 = rule_2.Content as string;

            }
            else
            {
                if (irregular_2.IsChecked == true)
                {
                    heart2 = irregular_2.Content as string;
                }

            }
            //体温
            string heat2 = heat_2.Text;

            //获取问诊票 被选中的将其内容保存在list中
            List<string> list = new List<string>();

            foreach (CheckBox chk in this.stackPanel_1.Children.OfType<CheckBox>())
            {
                if (chk.IsChecked == true)
                {
                    list.Add(chk.Content as string);
                    // MessageBoxResult messageBoxResult = MessageBox.Show(chk.Content as string);
                }

            }
            foreach (CheckBox chk in this.stackPanel_2.Children.OfType<CheckBox>())
            {
                if (chk.IsChecked == true)
                {
                    list.Add(chk.Content as string);
                    // MessageBoxResult messageBoxResult = MessageBox.Show(chk.Content as string);
                }
            }
            //参加不参加
            string join = null;
            if (join_1.IsChecked == true)
            {
                join = join_1.Content as string;
            }
            else if (join_2.IsChecked == true)
            {
                join = join_2.Content as string;
            }
            //摄取水分量
            string am = amunt.Text;

            //看护记录
            string record = Record.Text;

        }
    }
}
