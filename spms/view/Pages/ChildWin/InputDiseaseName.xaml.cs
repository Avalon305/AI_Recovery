
using spms.entity;
using spms.service;
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
    /// InputDiseaseName.xaml 的交互逻辑
    /// </summary>
    public partial class InputDiseaseName : Window
    {
        public InputDiseaseName()
        {
            InitializeComponent();
        }
        //取消按钮，关闭此窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataCodeService dataCodeService = new DataCodeService();

             
            string name = this.DiseaseName.Text;
           
            DataCode dataCode = new DataCode();

            dataCodeService.AddCustomDataCode(constant.DataCodeTypeEnum.Disease, name);
            this.Close();
        }
    }
}
