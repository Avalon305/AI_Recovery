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

namespace UI.Pages
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

        private void AdvancedSettings(object sender, RoutedEventArgs e)
        {
            NavigationWindow window = new NavigationWindow();
            window.Source = new Uri("/Pages/AdvancedSettings.xaml", UriKind.RelativeOrAbsolute);
            window.Show();
            // NavigationService.GetNavigationService(this).Navigate(new Uri("AdvancedSettings.xaml", UriKind.Relative));
            //AdvancedSettings advancedSettings = new Pages.AdvancedSettings();
            //advancedSettings.Owner
            //this.Content = advancedSettings;
            //this.Content = new AdvancedSettings();
            //Window w = new Window();
            //w.ShowDialog();
            //this.NavigationService.Navigate(advancedSettings);
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            //AdvancedSettings advancedSettings = new Pages.AdvancedSettings();
            //this.Content = advancedSettings;
            //this.NavigationService.GoBack();
        }
    }
}
