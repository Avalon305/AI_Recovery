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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace spms.view.Pages
{
    /// <summary>
    /// DesignPage1.xaml 的交互逻辑
    /// </summary>
    public partial class DesignPage1 : Page
    {
        public DesignPage1()
        {
            InitializeComponent();
            //DataGrid2.ColumnHeadersDefaultCellStyle.BackColor = Color.Ivory;
        }
        //按钮：高级设置
        private void AdvancedSettings(object sender, RoutedEventArgs e)
        {
            Window window = (Window)this.Parent;
            window.Content = new AdvancedSettings();
            
        }
        //返回上一页
        private void GoBack(object sender, RoutedEventArgs e)
        {
          


        }
    }
}
