using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using spms.entity;
using spms.http;
using spms.view.Pages.ChildWin;
namespace spms.view.Pages
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        List<User> users = new List<User>();
        //大数据线程，主要上传除心跳之外的所有数据信息
        Thread bigDataThread;
        

        public MainPage()
        {

            InitializeComponent();
            //启动大数据线程,切换界面记得关闭该线程
            bigDataThread = new Thread(new ThreadStart(UploadDataToWEB));
            //暂时先不启动
            //bigDataThread.Start();
            //添加使用者
            User user = new User
            {
                User_Name = "123",
                User_Namepinyin = "123"
            };
            users.Add(user);
            User user1 = new User
            {
                User_Name = "1234",
                User_Namepinyin = "1234"
            };
            User user2 = new User
            {
                User_Name = "12345",
                User_Namepinyin = "12345"
            };
            users.Add(user2);
            UsersInfo.ItemsSource = users;
            UsersInfo.SelectedIndex = 0;
            //初始显示的记录
            if (is_signinformationrecord.IsChecked == true)
            {
                //显示征状信息记录
                record.Source = new Uri("/view/Pages/Frame/SignInformationRecord_Frame.xaml", UriKind.Relative);
            }
            else if (is_trainingrecord.IsChecked == true)
            {
                //显示训练信息记录
                record.Source = new Uri("/view/Pages/Frame/TrainingRecord_Frame.xaml", UriKind.Relative);
            }
            else
            {
                //显示体力评价记录
                record.Source = new Uri("/view/Pages/Frame/PhysicaleValuation_Frame.xaml", UriKind.Relative);
            }
        }
        /// <summary>
        /// 上传的方法，参数为秒
        /// </summary>
        public static void UploadDataToWEB() {
            //300秒-5分钟一次上传
            BigDataOfficer bigDataOfficer = new BigDataOfficer(300 * 1000);
        }
        //按钮：输入征状信息
        private void InputSymptomInformation(object sender, RoutedEventArgs e)
        {

            InputSymptomInformation w2 = new InputSymptomInformation
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            w2.ShowDialog();

        }
        //按钮：输入训练
        private void InputTraining(object sender, RoutedEventArgs e)
        {
            InputTraining inputTraining = new InputTraining
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            inputTraining.ShowDialog();
        }
        //按钮：添加
        private void Register(object sender, RoutedEventArgs e)
        {
            Register register = new Register
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            register.ShowDialog();
        }
        //按钮：条件检索
        private void Retrieval(object sender, RoutedEventArgs e)
        {
            Retrieval retrieval = new Retrieval
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            retrieval.ShowDialog();
        }
        //按钮：更新
        private void UserUpdata(object sender, RoutedEventArgs e)
        {
            UserUpdata userUpdata = new UserUpdata
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            User user = (User)UsersInfo.SelectedItem;
            userUpdata.User_Update.DataContext = user;
            userUpdata.ShowDialog();
        }
        //按钮：输入
        private void InputManualMvaluation(object sender, RoutedEventArgs e)
        {
            InputManualMvaluation inputManualMvaluation = new InputManualMvaluation
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            inputManualMvaluation.ShowDialog();
        }
        //使用者信息选中，将详细信息展示在左下角
        private void Grid_Click(object sender, MouseButtonEventArgs e)
        {
            User user = (User)UsersInfo.SelectedItem;
            UserInfo.DataContext = user;



        }
        //记录类型切换
        private void Radio_Check(object sender, RoutedEventArgs e)
        {
            if (is_signinformationrecord.IsChecked == true)
            {
                //record.Source = new Uri("/Pages/Frame/TrainingRecord_Frame.xaml", UriKind.Relative);
                record.Source = new Uri("/view/Pages/Frame/SignInformationRecord_Frame.xaml", UriKind.Relative);
            }
            else if (is_trainingrecord.IsChecked == true)
            {
                record.Source = new Uri("/view/Pages/Frame/TrainingRecord_Frame.xaml", UriKind.Relative);

            }
            else
            {
                record.Source = new Uri("/view/Pages/Frame/PhysicaleValuation_Frame.xaml", UriKind.Relative);
            }
        }
        //按钮：删除
        private void Delete_User(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("您确定删除该使用者信息？\n 使用者：" + ((User)UsersInfo.SelectedItem).User_Name, "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr == MessageBoxResult.OK)
            {

            }
        }
        //按钮：文档输出
        private void Output_Document(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".xml",
                Filter = "xml file|*.xml"
            };
            if (ofd.ShowDialog() == true)
            {
                //此处做你想做的事 ...=ofd.FileName; 
            }
        }
        //按钮：制作报告
        private void MakeReport(object sender, RoutedEventArgs e)
        {
            //打开训练报告
            if (is_signinformationrecord.IsChecked == true)
            {
                TrainingReport trainingReport = new TrainingReport
                {
                    Owner = Window.GetWindow(this),
                    ShowActivated = true,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                List<TrainInfo> list = new List<TrainInfo>();
                TrainInfo trainInfo = new TrainInfo
                {
                    Gmt_Create = DateTime.Parse("2010-2-12")
                };
                list.Add(trainInfo);
                Console.WriteLine(trainInfo.Gmt_Create);
                list.Add(trainInfo);
                trainingReport.datalist.DataContext = list;
                trainingReport.ShowDialog();
            }
            //打开训练报告页面
            else if (is_trainingrecord.IsChecked == true)
            {
                TrainingReport trainingReport = new TrainingReport
                {
                    Owner = Window.GetWindow(this),
                    ShowActivated = true,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                List<TrainInfo> list = new List<TrainInfo>();
                TrainInfo trainInfo = new TrainInfo
                {
                    Gmt_Create = new DateTime(2012, 01, 02)
                };
                list.Add(trainInfo);
                Console.WriteLine(trainInfo.Gmt_Create);
                list.Add(trainInfo);
                trainingReport.datalist.DataContext = list;
                trainingReport.ShowDialog();
            }
            //打开体力评价报告页面
            else
            {
                PhysicalAssessmentReport physicalAssessmentReport = new PhysicalAssessmentReport
                {
                    Owner = Window.GetWindow(this),
                    ShowActivated = true,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                List<TrainInfo> list = new List<TrainInfo>();
                TrainInfo trainInfo = new TrainInfo
                {
                    Gmt_Create = new DateTime(2012, 01, 02)
                };
                list.Add(trainInfo);
                Console.WriteLine(trainInfo.Gmt_Create);
                list.Add(trainInfo);
                physicalAssessmentReport.datalist.DataContext = list;
                physicalAssessmentReport.ShowDialog();

            }
            ////List<String> list = new List<string>();
            ////for (int i = 0; i < 100; i++)
            ////{
            ////    list.Add("sas" + i);
            ////}


        }
        //按钮：输入训练结果
        private void InputTrainingResults(object sender, RoutedEventArgs e)
        {
            InputTrainingResults inputTrainingResults = new ChildWin.InputTrainingResults
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            inputTrainingResults.ShowDialog();
        }
        //按钮：查看详细信息
        private void ViewDetails(object sender, RoutedEventArgs e)
        {
            //查看征状详细信息
            if (is_signinformationrecord.IsChecked == true)
            {
                ViewSymptomInformation viewSymptomInformation = new ViewSymptomInformation
                {
                    Owner = Window.GetWindow(this),
                    ShowActivated = true,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                List<TrainInfo> list = new List<TrainInfo>();
                TrainInfo trainInfo = new TrainInfo
                {
                    Gmt_Create = new DateTime(2012, 01, 02)
                };
                list.Add(trainInfo);
                Console.WriteLine(trainInfo.Gmt_Create);
                list.Add(trainInfo);

                viewSymptomInformation.ShowDialog();
            }
            //打开训练详细信息
            else if (is_trainingrecord.IsChecked == true)
            {
                ViewTrainingResults viewTrainingResults = new ViewTrainingResults
                {
                    Owner = Window.GetWindow(this),
                    ShowActivated = true,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                List<TrainInfo> list = new List<TrainInfo>();
                TrainInfo trainInfo = new TrainInfo
                {
                    Gmt_Create = new DateTime(2012, 01, 02)
                };
                list.Add(trainInfo);
                Console.WriteLine(trainInfo.Gmt_Create);
                list.Add(trainInfo);

                viewTrainingResults.ShowDialog();
            }
            //打开体力评价详细信息
            else
            {
                PhysicalAssessmentReport physicalAssessmentReport = new PhysicalAssessmentReport
                {
                    Owner = Window.GetWindow(this),
                    ShowActivated = true,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                List<TrainInfo> list = new List<TrainInfo>();
                TrainInfo trainInfo = new TrainInfo
                {
                    Gmt_Create = new DateTime(2012, 01, 02)
                };
                list.Add(trainInfo);
                Console.WriteLine(trainInfo.Gmt_Create);
                list.Add(trainInfo);
                physicalAssessmentReport.datalist.DataContext = list;
                physicalAssessmentReport.ShowDialog();

            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Window window = (Window)this.Parent;
            window.Show();
            //window.Content = new MainWindow();
        }

        //private void SignInformationRecord_Frame(object sender, RoutedEventArgs e)
        //{
        //    this.record.Source = new Uri("/Pages/Frame/SignInformationRecord_Frame.xaml", UriKind.Relative);

        //}
    }
}
