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
    /// Retrieval.xaml 的交互逻辑
    /// </summary>
    public partial class Retrieval : Window
    {
        public Retrieval()
        {
            InitializeComponent();
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {
            t1.Text = "";
            t2.Text = "";
            t3.Text = "";
            t4.Text = "";
            c1.Text = "";
            c3.Text = "";
            c2.Text = "";
        }

        private void B3_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
