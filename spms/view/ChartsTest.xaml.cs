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

namespace spms.view
{
    /// <summary>
    /// ChartsTest.xaml 的交互逻辑
    /// </summary>
    public partial class ChartsTest : Window
    {
        public ChartsTest()
        {
            InitializeComponent();

            string path = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine(path);
            string rootpath = path.Substring(0, path.LastIndexOf("bin"));
            Console.WriteLine(rootpath);
            HLPWeb.Navigate(new Uri(rootpath + "spms/Echarts/dist/HLPLine.html"));
        }
    }
}
