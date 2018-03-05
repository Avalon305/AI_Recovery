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
using UI.Pages.ChildWin;
namespace UI.Pages
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void InputSymptomInformation(object sender, RoutedEventArgs e)
        {
            InputSymptomInformation w2 = new InputSymptomInformation();
            w2.Owner = Window.GetWindow(this);
            w2.ShowActivated = true;
            w2.ShowInTaskbar = false;
            w2.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            w2.ShowDialog();

        }

        private void InputTraining(object sender, RoutedEventArgs e)
        {
            InputTraining inputTraining = new InputTraining();
            inputTraining.Owner = Window.GetWindow(this);
            inputTraining.ShowActivated = true;
            inputTraining.ShowInTaskbar = false;
            inputTraining.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            inputTraining.ShowDialog();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Owner = Window.GetWindow(this);
            register.ShowActivated = true;
            register.ShowInTaskbar = false;
            register.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            register.ShowDialog();
        }

        private void Retrieval(object sender, RoutedEventArgs e)
        {
            Retrieval retrieval = new Retrieval();
            retrieval.Owner = Window.GetWindow(this);
            retrieval.ShowActivated = true;
            retrieval.ShowInTaskbar = false;
            retrieval.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            retrieval.ShowDialog();
        }

        private void UserUpdata(object sender, RoutedEventArgs e)
        {
            UserUpdata userUpdata = new UserUpdata();
            userUpdata.Owner = Window.GetWindow(this);
            userUpdata.ShowActivated = true;
            userUpdata.ShowInTaskbar = false;
            userUpdata.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            userUpdata.ShowDialog();
        }

        private void InputManualMvaluation(object sender, RoutedEventArgs e)
        {
            InputManualMvaluation inputManualMvaluation = new InputManualMvaluation();
            inputManualMvaluation.Owner = Window.GetWindow(this);
            inputManualMvaluation.ShowActivated = true;
            inputManualMvaluation.ShowInTaskbar = false;
            inputManualMvaluation.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            inputManualMvaluation.ShowDialog();
        }
    }
}
