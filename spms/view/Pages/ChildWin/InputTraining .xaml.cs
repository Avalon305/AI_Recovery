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
    /// InputTraining.xaml 的交互逻辑
    /// </summary>
    public partial class InputTraining : Window
    {
        List<string> list = new List<string> { "自理", "照看", "完全失能" };
        public InputTraining()
        {

            InitializeComponent();
            com_01.ItemsSource = Add(0, 700, 2);
            com_02.ItemsSource = Add(0, 30, 2);
            com_03.ItemsSource = Add(1, 5, 2);
            com_04.ItemsSource = Add(1, 9, 2);
            combobox_01.ItemsSource = Add(1, 3, 2);
            combobox_02.ItemsSource = Add(1, 20, 2);
            combobox_03.ItemsSource = Add(1, 60, 2);
            combobox_04.ItemsSource = Add(1.0, 84.0, 2);
            combobox_05.ItemsSource = list;

            com_11.ItemsSource = Add(0, 400, 2);
            com_12.ItemsSource = Add(0, 30, 1);
            com_13.ItemsSource = Add(1, 4, 2);
            com_14.ItemsSource = Add(1, 4, 2);
            combobox_11.ItemsSource = Add(1, 3, 2);
            combobox_12.ItemsSource = Add(1, 20, 2);
            combobox_13.ItemsSource = Add(1, 60, 2);
            combobox_14.ItemsSource = Add(1.0, 32.0, 1);
            combobox_15.ItemsSource = list;

            com_21.ItemsSource = Add(0, 700, 2);
            com_22.ItemsSource = Add(0, 30, 1);
            com_23.ItemsSource = Add(1, 6, 2);
            com_24.ItemsSource = Add(1, 9, 2);
            combobox_21.ItemsSource = Add(1, 3, 2);
            combobox_22.ItemsSource = Add(1, 20, 2);
            combobox_23.ItemsSource = Add(1, 60, 2);
            combobox_24.ItemsSource = Add(0.5, 32.0, 1);
            combobox_25.ItemsSource = list;

            com_31.ItemsSource = Add(0, 260, 1);
            com_32.ItemsSource = Add(0, 30, 1);
            com_33.ItemsSource = Add(1, 5, 2);
            com_34.ItemsSource = Add(1, 5, 2);
            com_35.ItemsSource = Add(1, 9, 2);
            combobox_31.ItemsSource = Add(1, 3, 2);
            combobox_32.ItemsSource = Add(1, 20, 2);
            combobox_33.ItemsSource = Add(1, 60, 2);
            combobox_34.ItemsSource = Add(1.0, 32.0, 1);
            combobox_35.ItemsSource = list;

            com_41.ItemsSource = Add(0, 500, 2);
            com_42.ItemsSource = Add(0, 30, 1);
            com_43.ItemsSource = Add(1, 9, 2);
            combobox_41.ItemsSource = Add(1, 3, 2);
            combobox_42.ItemsSource = Add(1, 20, 2);
            combobox_43.ItemsSource = Add(1, 60, 2);
            combobox_44.ItemsSource = Add(1.0, 32.0, 1);
            combobox_45.ItemsSource = list;

            com_51.ItemsSource = Add(0, 180, 2);
            com_52.ItemsSource = Add(0, 30, 1);
            com_53.ItemsSource = Add(1, 8, 2);
            combobox_51.ItemsSource = Add(1, 3, 2);
            combobox_52.ItemsSource = Add(1, 20, 2);
            combobox_53.ItemsSource = Add(1, 60, 2);
            combobox_54.ItemsSource = Add(1.0, 32.0, 1);
            combobox_55.ItemsSource = list;

            l1.Content = "安典龙";//设置用户姓名
            l2.Content = "13210104659";//用户ID

        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否所有编辑都无效？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr == MessageBoxResult.OK)
            {
                this.Close();
            }
        }
        private List<double> Add(double start, double end, int type)

        {
            List<double> list = new List<double>();
            switch (type)
            {
                case 1:
                    for (double s = start; s <= end; s += 0.5)
                    {
                        list.Add(s);
                    }
                    break;
                case 2:
                    for (double s = start; s <= end; s++)
                    {
                        list.Add(s);
                    }
                    break;

            }
            return list;
        }

        private void checkbox1_Checked(object sender, RoutedEventArgs e)
        {
            combobox_01.IsEnabled = true;
            combobox_02.IsEnabled = true;
            combobox_03.IsEnabled = true;
            combobox_04.IsEnabled = true;
            combobox_05.IsEnabled = true;
            com_01.IsEnabled = true;
            com_04.IsEnabled = true;
            com_02.IsEnabled = true;
            com_03.IsEnabled = true;
        }

        private void checkbox2_Checked(object sender, RoutedEventArgs e)
        {
            combobox_11.IsEnabled = true;
            combobox_12.IsEnabled = true;
            combobox_13.IsEnabled = true;
            combobox_14.IsEnabled = true;
            combobox_15.IsEnabled = true;
            com_11.IsEnabled = true;
            com_12.IsEnabled = true;
            com_13.IsEnabled = true;
            com_14.IsEnabled = true;
        }


        private void checkbox3_Checked(object sender, RoutedEventArgs e)
        {
            combobox_21.IsEnabled = true;
            combobox_22.IsEnabled = true;
            combobox_23.IsEnabled = true;
            combobox_24.IsEnabled = true;
            combobox_25.IsEnabled = true;
            com_21.IsEnabled = true;
            com_24.IsEnabled = true;
            com_22.IsEnabled = true;
            com_23.IsEnabled = true;

        }

        private void checkbox4_Checked(object sender, RoutedEventArgs e)
        {
            combobox_31.IsEnabled = true;
            combobox_32.IsEnabled = true;
            combobox_33.IsEnabled = true;
            combobox_34.IsEnabled = true;
            combobox_35.IsEnabled = true;
            com_31.IsEnabled = true;
            com_34.IsEnabled = true;
            com_32.IsEnabled = true;
            com_33.IsEnabled = true;
            com_35.IsEnabled = true;
        }

        private void checkbox5_Checked(object sender, RoutedEventArgs e)
        {
            combobox_41.IsEnabled = true;
            combobox_42.IsEnabled = true;
            combobox_43.IsEnabled = true;
            combobox_44.IsEnabled = true;
            combobox_45.IsEnabled = true;
            com_41.IsEnabled = true;
            com_42.IsEnabled = true;
            com_43.IsEnabled = true;


        }

        private void checkbox6_Checked(object sender, RoutedEventArgs e)
        {
            combobox_51.IsEnabled = true;
            combobox_52.IsEnabled = true;
            combobox_53.IsEnabled = true;
            combobox_54.IsEnabled = true;
            combobox_55.IsEnabled = true;
            com_51.IsEnabled = true;
            com_52.IsEnabled = true;
            com_53.IsEnabled = true;
        }

        private void checkbox1_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_01.IsEnabled = false;
            combobox_02.IsEnabled = false;
            combobox_03.IsEnabled = false;
            combobox_04.IsEnabled = false;
            combobox_05.IsEnabled = false;
            com_01.IsEnabled = false;
            com_04.IsEnabled = false;
            com_02.IsEnabled = false;
            com_03.IsEnabled = false;
        }

        private void checkbox2_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_11.IsEnabled = false;
            combobox_12.IsEnabled = false;
            combobox_13.IsEnabled = false;
            combobox_14.IsEnabled = false;
            combobox_15.IsEnabled = false;
            com_11.IsEnabled = false;
            com_12.IsEnabled = false;
            com_13.IsEnabled = false;
            com_14.IsEnabled = false;
        }

        private void checkbox3_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_21.IsEnabled = false;
            combobox_22.IsEnabled = false;
            combobox_23.IsEnabled = false;
            combobox_24.IsEnabled = false;
            combobox_25.IsEnabled = false;
            com_21.IsEnabled = false;
            com_24.IsEnabled = false;
            com_22.IsEnabled = false;
            com_23.IsEnabled = false;


        }

        private void checkbox4_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_31.IsEnabled = false;
            combobox_32.IsEnabled = false;
            combobox_33.IsEnabled = false;
            combobox_34.IsEnabled = false;
            combobox_35.IsEnabled = false;
            com_31.IsEnabled = false;
            com_34.IsEnabled = false;
            com_32.IsEnabled = false;
            com_33.IsEnabled = false;
            com_35.IsEnabled = false;
        }

        private void checkbox5_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_41.IsEnabled = false;
            combobox_42.IsEnabled = false;
            combobox_43.IsEnabled = false;
            combobox_44.IsEnabled = false;
            combobox_45.IsEnabled = false;
            com_41.IsEnabled = false;
            com_42.IsEnabled = false;
            com_43.IsEnabled = false;

        }

        private void checkbox6_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_51.IsEnabled = false;
            combobox_52.IsEnabled = false;
            combobox_53.IsEnabled = false;
            combobox_54.IsEnabled = false;
            combobox_55.IsEnabled = false;
            com_51.IsEnabled = false;
            com_52.IsEnabled = false;
            com_53.IsEnabled = false;
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            //一
            string com01 = com_01.Text;
            string com02 = com_02.Text;
            string com03 = com_03.Text;
            string com04 = com_04.Text;
            string combobox01 = combobox_01.Text;
            string combobox02 = combobox_02.Text;
            string combobox03 = combobox_03.Text;
            string combobox04 = combobox_04.Text;
            string combobox05 = combobox_05.Text;
            string s1 = t1.Text;
            List<string> list1 = new List<string> { com01, com02, com03, com04, combobox01, combobox02, combobox03, combobox04, combobox05, s1 };
            //二
            string com11 = com_11.Text;
            string com12 = com_12.Text;
            string com13 = com_13.Text;
            string com14 = com_14.Text;
            string combobox11 = combobox_11.Text;
            string combobox12 = combobox_12.Text;
            string combobox13 = combobox_13.Text;
            string combobox14 = combobox_14.Text;
            string combobox15 = combobox_15.Text;
            string s2 = t2.Text;
            List<string> list2 = new List<string> { com11, com12, com13, com14, combobox11, combobox12, combobox13, combobox14, combobox15, s2 };
            //三
            string com21 = com_21.Text;
            string com22 = com_22.Text;
            string com23 = com_23.Text;
            string com24 = com_24.Text;
            string combobox21 = combobox_21.Text;
            string combobox22 = combobox_22.Text;
            string combobox23 = combobox_23.Text;
            string combobox24 = combobox_24.Text;
            string combobox25 = combobox_25.Text;
            string s3 = t3.Text;
            List<string> list3 = new List<string> { com21, com22, com23, com24, combobox21, combobox22, combobox23, combobox24, combobox25, s3 };
            //四
            string com31 = com_31.Text;
            string com32 = com_32.Text;
            string com33 = com_33.Text;
            string com34 = com_34.Text;
            string com35 = com_35.Text;
            string combobox31 = combobox_31.Text;
            string combobox32 = combobox_32.Text;
            string combobox33 = combobox_33.Text;
            string combobox34 = combobox_34.Text;
            string combobox35 = combobox_35.Text;
            string s4 = t4.Text;
            List<string> list4 = new List<string> { com31, com32, com33, com34, com35, combobox31, combobox32, combobox33, combobox34, combobox35, s4 };

            string com41 = com_41.Text;
            string com42 = com_42.Text;
            string com43 = com_43.Text;
            string combobox41 = combobox_41.Text;
            string combobox42 = combobox_42.Text;
            string combobox43 = combobox_43.Text;
            string combobox44 = combobox_44.Text;
            string combobox45 = combobox_45.Text;
            string s5 = t5.Text;
            List<string> list5 = new List<string> { com41, com42, com43, combobox41, combobox42, combobox43, combobox44, combobox45, s5 };

            string com51 = com_51.Text;
            string com52 = com_52.Text;
            string com53 = com_53.Text;
            string combobox51 = combobox_51.Text;
            string combobox52 = combobox_52.Text;
            string combobox53 = combobox_53.Text;
            string combobox54 = combobox_54.Text;
            string combobox55 = combobox_55.Text;
            string s6 = t6.Text;
            List<string> list6 = new List<string> { com51, com52, com53, combobox51, combobox52, combobox53, combobox54, combobox55, s6 };

        }
    }
}
