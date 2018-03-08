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
    /// InputNonPublicInformationPassword.xaml 的交互逻辑
    /// </summary>
    public partial class InputNonPublicInformationPassword : Window
    {
        public InputNonPublicInformationPassword()
        {
            InitializeComponent();
        }
        //确定按钮
        private void Determine(object sender, RoutedEventArgs e)
        {
            String password = NonPublicInformationPassword.Text;
            Console.WriteLine("123123");
            this.Close();
        }
    }
}
