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
using Recovery.util;
using Recovery.view.Pages;

namespace Recovery
{
    /// <summary>
    /// ZscTest.xaml 的交互逻辑
    /// </summary>
    public partial class ZscTest : Window
    {
        public ZscTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //LanguageUtils.EqualsResource("倒计时", "TrainingListView.CountReverse");
        }

        private void click1(object sender, RoutedEventArgs e)
        {
            MessageBoxX.Info("我里边的值随便写");
            MessageBoxX.Error("我里边的值真随便写");
            MessageBoxX.Warning("我里边的值真真随便写");
            MessageBoxX.Question("我里边的值真真真随便写");
        }
    }
}
