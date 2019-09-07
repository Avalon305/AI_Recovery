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
    /// NfcTip.xaml 的交互逻辑
    /// </summary>
    public partial class NfcTip : Window
    {
        public string G_nfcInfo = "";

        public NfcTip()
        {
            InitializeComponent();
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NfcInfo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(nfcInfo.Text.Length == 16)
            {
                G_nfcInfo = nfcInfo.Text;
                nfcInfo.Focusable = false;
            }
        }
    }
}
