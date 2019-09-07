using Recovery.dao;
using Recovery.entity;
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

namespace Recovery.view.Pages.ChildWin
{
    /// <summary>
    /// NfcMyodynamia.xaml 的交互逻辑
    /// </summary>
    public partial class NfcMyodynamia : Window
    {
        public NfcMyodynamia()
        {
            InitializeComponent();
        }

        private void Button_OK(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
