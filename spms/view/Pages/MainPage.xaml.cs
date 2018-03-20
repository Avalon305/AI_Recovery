using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using spms.http.entity;
using spms.service;
using spms.util;
using spms.view.Pages.ChildWin;
namespace spms.view.Pages
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        ///病人信息一览表
        public List<User> users = new List<User>();
        //当前选择的User
        public User selectUser = null;
        //用到的业务层实例
        UserService userService = new UserService();


        //大数据线程，主要上传除心跳之外的所有数据信息
        Thread bigDataThread;
        //后台心跳更新UI线程
        public System.Timers.Timer timerNotice = null;
        /// <summary>
        /// 启动时的构造函数
        /// </summary>
        public MainPage()
        {

            InitializeComponent();
            //启动大数据线程,切换界面记得关闭该线程
            bigDataThread = new Thread(new ThreadStart(UploadDataToWEB));
            //暂时先不启动
            //bigDataThread.Start();
            ///心跳线程部分-load方法启动
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

        /// <summary>
        /// 选中使用者信息时触发，将详细信息展示在左下角
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Click(object sender, MouseButtonEventArgs e)
        {
            selectUser = (User)UsersInfo.SelectedItem;
            UserInfo.DataContext = selectUser;
        }
        /// <summary>
        /// 定时器心跳间隔，load时设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ///载入时数据装填到list,默认选中第一个
            users = userService.GetAllUsers();
            UsersInfo.ItemsSource = users;
            UsersInfo.SelectedIndex = 0;
            selectUser = (User)UsersInfo.SelectedItem;
            ///心跳部分
            #region 通知公告
            if (timerNotice == null)
            {
                BindNotice();

                timerNotice = new System.Timers.Timer();
                timerNotice.Elapsed += new System.Timers.ElapsedEventHandler((o, eea) =>
                {
                    BindNotice();
                });
                timerNotice.Interval = 60 * 1000;
                timerNotice.Start();
            }
            #endregion
           

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
            //添加之后，flush界面
            //致空
            selectUser = null;
            //刷新界面
            users = userService.GetAllUsers();
            UsersInfo.ItemsSource = users;
        }
        //按钮：条件检索
        private void Retrieval(object sender, RoutedEventArgs e)
        {
            //查询弹框
            Retrieval retrieval = new Retrieval
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            retrieval.ShowDialog();
            //刷新界面
            users = retrieval.QueryResult;
            UsersInfo.ItemsSource = users;
            //检索后设置无用户被选中
            selectUser = null;
        }
        //按钮：更新
        private void UserUpdata(object sender, RoutedEventArgs e)
        {
            //检查是否选中
            if (selectUser == null) {
                MessageBox.Show("请选择用户再进行操作！");
                return;
            }

            UserUpdata userUpdata = new UserUpdata
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            //类中使用
            User user = (User)UsersInfo.SelectedItem;
            userUpdata.SelectUser = user;
            //UI中使用
            userUpdata.selectUser.DataContext = user;
            userUpdata.ShowDialog();
        }
        //按钮：删除
        private void Delete_User(object sender, RoutedEventArgs e)
        {
            //检查是否选中
            if (selectUser == null)
            {
                MessageBox.Show("请选择用户再进行操作！");
                return;
            }

            MessageBoxResult dr = MessageBox.Show("您确定删除该使用者信息？\n 使用者：" + ((User)UsersInfo.SelectedItem).User_Name, "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr == MessageBoxResult.OK)
            {
                //删除
                userService.DeleteUser(selectUser);
                //致空
                selectUser = null;
                //刷新界面
                users = userService.GetAllUsers();
                UsersInfo.ItemsSource = users;
            }
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
            User user = (User)UsersInfo.SelectedItem;
            inputTrainingResults.DataContext = user;
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
            System.Environment.Exit(0);
            //Window window = (Window)this.Parent;
            //window.Show();
            //window.Content = new MainWindow();
        }
        
        /// <summary>
        /// 异步进程与UI更新
        /// </summary>
        #region 绑定通知公告
        private void BindNotice()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {
                    HeartBeatOffice heartBeatOffice = new HeartBeatOffice();
                    HttpHeartBeat result = heartBeatOffice.GetHeartBeatByCurrent();
                    HttpSender httpSender = new HttpSender("/communicationController/analysisJson", JsonTools.Obj2JSONStrNew<HttpHeartBeat>(result));
                    string jsonStr = httpSender.sendDataToWebPlatform();
                    HttpHeartBeat webResult = JsonTools.DeserializeJsonToObject<HttpHeartBeat>(jsonStr);
                    //本地数据更改
                    heartBeatOffice.SolveHeartbeat(webResult);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (webResult.authStatus == 0) {
                            //正常心跳不处理

                        } else if (webResult.authStatus == 1) {
                            //冻结，弹窗，然后关闭窗口
                            // 程序强制退出
                            MessageBox.Show("用户被冻结，即将退出，请联系宝德龙管理员解冻！");
                            Environment.Exit(0);
                        }
                        else if (webResult.authStatus == 2)
                        {
                            //解冻，只需要更改数据库。界面无反馈，不处理
                        }
                        else if (webResult.authStatus == 3)
                        {
                            //永久离线，只需要更改数据库。界面无反馈，不处理
                        }
                        else if (webResult.authStatus == 4)
                        {
                            //已删除，按照冻结处理
                            MessageBox.Show("用户被删除，即将退出，请联系宝德龙管理员恢复！");
                            Environment.Exit(0);
                        }
                    }));
                }
                catch
                {

                }
            });
        }
        #endregion

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
            User user = (User)UsersInfo.SelectedItem;
            w2.DataContext = user;
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
            User user = (User) UsersInfo.SelectedItem;
            inputTraining.DataContext = user;
            inputTraining.ShowDialog();
        }
        //按钮：输入体力评价
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

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            Window window = (Window)this.Parent;
            window.Content = new DesignPage1();


        }
    }
}
