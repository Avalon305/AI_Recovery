using spms.dao;
using spms.util;
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

namespace spms
{
    /// <summary>
    /// FmyTest.xaml 的交互逻辑
    /// </summary>
    public partial class FmyTest : Window
    {
        public FmyTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var d = DataCodeCache.GetInstance().GetCodeDValue("PR_Evaluate", "1");
            var s = DataCodeCache.GetInstance().GetCodeSValue("PR_Evaluate", "有些许问题");
            MessageBox.Show(d);
            MessageBox.Show(s);
        }
    }
}
