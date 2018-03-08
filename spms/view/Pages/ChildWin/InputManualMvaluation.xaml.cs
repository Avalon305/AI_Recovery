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
    /// InputManualMvaluation.xaml 的交互逻辑
    /// </summary>
    public partial class InputManualMvaluation : Window
    {

        private new bool IsEnabled = true;
        private bool UnEnabled = false;
        public InputManualMvaluation()
        {
            InitializeComponent();

            List<String> list = new List<string>
            {
                "T字拐杖",
                "4字拐杖",
                "步行器",
                "其他"
            };
            comBox1.ItemsSource = list;
            comBox3.ItemsSource = list;
        }


        private void c1_Checked(object sender, RoutedEventArgs e)
        {
            t1.IsEnabled = IsEnabled;
        }

        private void c1_Unchecked(object sender, RoutedEventArgs e)
        {
            t1.IsEnabled = UnEnabled;
        }



        private void c4_Checked(object sender, RoutedEventArgs e)
        {
            c3.IsChecked = false;
        }

        private void c3_Checked(object sender, RoutedEventArgs e)
        {
            c4.IsChecked = false;
        }

        private void c5_Checked(object sender, RoutedEventArgs e)
        {
            c6.IsChecked = false;
            c7.IsChecked = false;
        }

        private void c6_Checked(object sender, RoutedEventArgs e)
        {
            c7.IsChecked = false;
            c5.IsChecked = false;
            comBox1.IsEnabled = true;
            comBox2.IsEnabled = true;
        }
        private void c6_Unchecked(object sender, RoutedEventArgs e)
        {
            comBox1.IsEnabled = false;
            comBox2.IsEnabled = false;
        }

        private void c7_Checked(object sender, RoutedEventArgs e)
        {
            c5.IsChecked = false;
            c6.IsChecked = false;

        }

        private void c8_Checked(object sender, RoutedEventArgs e)
        {
            c9.IsChecked = false;
        }

        private void c9_Checked(object sender, RoutedEventArgs e)
        {
            c8.IsChecked = false;
        }

        private void c10_Checked(object sender, RoutedEventArgs e)
        {
            c11.IsChecked = false;
        }

        private void c11_Checked(object sender, RoutedEventArgs e)
        {
            c10.IsChecked = false;
        }

        private void c16_Checked(object sender, RoutedEventArgs e)
        {
            c12.IsChecked = false;
            c13.IsChecked = false;
        }

        private void c12_Checked(object sender, RoutedEventArgs e)
        {
            c13.IsChecked = false;
            c16.IsChecked = false;
        }

        private void c13_Checked(object sender, RoutedEventArgs e)
        {
            c12.IsChecked = false;
            c16.IsChecked = false;
        }

        private void c14_Checked(object sender, RoutedEventArgs e)
        {
            c15.IsChecked = false;
        }

        private void c15_Checked(object sender, RoutedEventArgs e)
        {
            c14.IsChecked = false;
        }

        private void c17_Checked(object sender, RoutedEventArgs e)
        {
            c18.IsChecked = false;
        }

        private void c18_Checked(object sender, RoutedEventArgs e)
        {
            c17.IsChecked = false;
        }

        private void c19_Checked(object sender, RoutedEventArgs e)
        {
            c20.IsChecked = false;
        }

        private void c20_Checked(object sender, RoutedEventArgs e)
        {
            c19.IsChecked = false;
        }

        private void c21_Checked(object sender, RoutedEventArgs e)
        {
            c22.IsChecked = false;
        }

        private void c22_Checked(object sender, RoutedEventArgs e)
        {
            c21.IsChecked = false;
        }

        private void c23_Checked(object sender, RoutedEventArgs e)
        {
            c24.IsChecked = false;
            c25.IsChecked = false;
        }

        private void c24_Checked(object sender, RoutedEventArgs e)
        {
            c23.IsChecked = false;
            c25.IsChecked = false;
            comBox3.IsEnabled = IsEnabled;
        }


        private void c24_Unchecked(object sender, RoutedEventArgs e)
        {
            comBox3.IsEnabled = false;
        }

        private void c25_Checked(object sender, RoutedEventArgs e)
        {
            c24.IsChecked = false;
            c23.IsChecked = false;
            //text1.IsEnabled = IsEnabled;
        }

        //private void c25_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    text1.IsEnabled = UnEnabled;
        //}

        private void c26_Checked(object sender, RoutedEventArgs e)
        {
            c27.IsChecked = false;

        }

        private void c27_Checked(object sender, RoutedEventArgs e)
        {
            c26.IsChecked = false;
        }

        private void c28_Checked(object sender, RoutedEventArgs e)
        {
            c29.IsChecked = false;
            c30.IsChecked = false;
        }

        private void c29_Unchecked(object sender, RoutedEventArgs e)
        {
            comBox4.IsEnabled = false;
        }

        //private void c30_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    text2.IsEnabled = UnEnabled;
        //}

        private void c29_Checked(object sender, RoutedEventArgs e)
        {
            c28.IsChecked = false;
            c30.IsChecked = false;
            comBox4.IsEnabled = IsEnabled;
        }

        private void c30_Checked(object sender, RoutedEventArgs e)
        {
            c29.IsChecked = false;
            c28.IsChecked = false;
            //text2.IsEnabled = IsEnabled;
        }

        private void c31_Checked(object sender, RoutedEventArgs e)
        {
            c32.IsEnabled = false;
        }

        private void c32_Checked(object sender, RoutedEventArgs e)
        {
            c31.IsEnabled = false;
        }

        private void c33_Checked(object sender, RoutedEventArgs e)
        {
            c34.IsChecked = false;
            c35.IsChecked = false;
        }

        private void c34_Checked(object sender, RoutedEventArgs e)
        {
            c33.IsChecked = false;
            c35.IsChecked = false;

            comBox5.IsEnabled = IsEnabled;
        }

        private void c34_Unchecked(object sender, RoutedEventArgs e)
        {
            comBox5.IsEnabled = UnEnabled;
        }

        private void c35_Checked(object sender, RoutedEventArgs e)
        {
            c34.IsChecked = false;
            c33.IsChecked = false;


            //text3.IsEnabled = IsEnabled;
        }

        //private void c35_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    text3.IsEnabled = UnEnabled;
        //}

        private void c36_Checked(object sender, RoutedEventArgs e)
        {
            c37.IsChecked = false;
        }

        private void c37_Checked(object sender, RoutedEventArgs e)
        {
            c36.IsChecked = false;
        }

        private void c38_Checked(object sender, RoutedEventArgs e)
        {
            c39.IsChecked = false;
            c40.IsChecked = false;

        }

        private void c39_Unchecked(object sender, RoutedEventArgs e)
        {
            comBox6.IsEnabled = UnEnabled;
        }

        //private void c40_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    text4.IsEnabled = UnEnabled;
        //}

        private void c39_Checked(object sender, RoutedEventArgs e)
        {
            c38.IsChecked = false;
            c40.IsChecked = false;
            comBox6.IsEnabled = IsEnabled;
        }

        private void c40_Checked(object sender, RoutedEventArgs e)
        {
            c39.IsChecked = false;
            c38.IsChecked = false;
            //text4.IsEnabled = IsEnabled;
        }

        private void c43_Checked(object sender, RoutedEventArgs e)
        {
            c44.IsChecked = false;
            c45.IsChecked = false;
        }

        private void c44_Checked(object sender, RoutedEventArgs e)
        {
            c45.IsChecked = false;
            c43.IsChecked = false;
            comBox7.IsEnabled = IsEnabled;
        }

        private void c44_Unchecked(object sender, RoutedEventArgs e)
        {
            comBox7.IsEnabled = UnEnabled;
        }

        private void c45_Checked(object sender, RoutedEventArgs e)
        {
            c44.IsChecked = false;
            c43.IsChecked = false;
            //text5.IsEnabled = IsEnabled;
        }

        //private void c45_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    text5.IsEnabled = UnEnabled;
        //}

        private void c46_Checked(object sender, RoutedEventArgs e)
        {
            c47.IsChecked = false;
            c48.IsChecked = false;
        }

        private void c47_Checked(object sender, RoutedEventArgs e)
        {
            c46.IsChecked = false;
            c48.IsChecked = false;
            comBox8.IsEnabled = IsEnabled;
        }

        private void c47_Unchecked(object sender, RoutedEventArgs e)
        {
            comBox8.IsEnabled = UnEnabled;
        }

        private void c48_Checked(object sender, RoutedEventArgs e)
        {
            c46.IsChecked = false;
            c47.IsChecked = false;
            //text6.IsEnabled = IsEnabled;
        }

        //private void c48_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    text6.IsEnabled = UnEnabled;
        //}

        private void c49_Checked(object sender, RoutedEventArgs e)
        {
            c50.IsChecked = false;
        }

        private void c50_Checked(object sender, RoutedEventArgs e)
        {
            c49.IsChecked = false;
        }
        //取消操作，关闭窗口
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private void c29_Checked(object sender, RoutedEventArgs e)
        //{

        //}

        //private void c25_Checked(object sender, RoutedEventArgs e)
        //{

        //}

        //private void c24_Checked(object sender, RoutedEventArgs e)
        //{

        //}
    }
}

