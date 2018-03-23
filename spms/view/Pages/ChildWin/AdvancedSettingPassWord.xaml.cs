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
    /// AdvancedSettingPassWord.xaml 的交互逻辑
    /// </summary>
    public partial class AdvancedSettingPassWord : Window
    {
        public AdvancedSettingPassWord()
        {
            InitializeComponent();
        }
        private void Confirm(object sender, RoutedEventArgs e)
        {
            string Password = PasswordText.Text;
            if (Password == "111")
            {
                this.Close();
            }
            else {
                MessageBox.Show("密码错误，请重新输入");
            }
            

        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
