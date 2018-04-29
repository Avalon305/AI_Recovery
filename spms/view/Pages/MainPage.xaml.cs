using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
using Spire.Xls;
using spms.bean;
using spms.constant;
using spms.dao;
using spms.entity;
using spms.http;
using spms.http.entity;
using spms.service;
using spms.util;
using spms.view.dto;
using spms.view.Pages.ChildWin;
using spms.view.Pages.Frame;
using static spms.bean.TrainExcelVO;

namespace spms.view.Pages
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public bool ifSelecUser = false;

        ///病人信息一览表
        public List<User> users = new List<User>();

        //当前选择的User
        public User selectUser = null;

        //用到的业务层实例
        UserService userService = new UserService();

        //报表的Excel业务层实例
        ExcelService excelService = new ExcelService();

        private AuthDAO authDao = new AuthDAO();




        //后台心跳更新UI线程
        public System.Timers.Timer timerNotice = null;

        /// <summary>
        /// 启动时的构造函数
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            //WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //this.Height = SystemParameters.WorkArea.Size.Height;
            //this.Width = SystemParameters.WorkArea.Size.Width;
            ///心跳线程部分-load方法启动
            //WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.UsersInfo.LoadingRow += new EventHandler<DataGridRowEventArgs>(this.dataGridEquipment_LoadingRow);
            //加载表头
            Radio_Check_Action();

        }

        private void dataGridEquipment_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;

        }
        //private void dgData_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    e.Row.Header = e.Row.GetIndex() + 1;
        //}



        /// <summary>
        /// 选中使用者信息时触发，将详细信息展示在左下角
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Click(object sender, MouseButtonEventArgs e)
        {
            ifSelecUser = true;
            if(UsersInfo.SelectedIndex == -1)
            {
                id.Content = null;
            }
            else {
            id.Content = UsersInfo.SelectedIndex + 1;//使用者详细信息展示框id设置
            }
            selectUser = (User)UsersInfo.SelectedItem;
            
            //UserInfo
            UserInfo.DataContext = selectUser;
            string path = null;

            // 选中用户的时候初始化
            is_signinformationrecord.Focus();

            //选中用户时展示 症状 训练 体力的记录框的frame
            //Radio_Check_Action();
            // 给frame加入数据
            Refresh_RecordFrame_Action();

            if (selectUser != null && selectUser.User_IDCard != null && selectUser.User_Namepinyin != null && selectUser.User_IDCard != "" && selectUser.User_Namepinyin != "")
            {
                path = CommUtil.GetUserPic();
                path += selectUser.User_PhotoLocation;
 
            }
            else
            {
                return;
            }

            //看照片是否存在
            if (!File.Exists(path))
            {


                BitmapImage bitmap = new BitmapImage(new Uri(@"\view\images\NoPhoto.png", UriKind.Relative));

                UserPhoto.Source = bitmap;

                return;
            }
            else
            {
                
             UserPhoto.Source = new BitmapImage(new Uri(path)).Clone();

            }
        }

        /// <summary>
        /// 定时器心跳间隔，load时设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //加载图片
            if (LanguageUtils.IsChainese())
            {
                title_pic.Source = new BitmapImage(new Uri(@"\view\Images\12.PNG", UriKind.Relative));
                DesignerHead4.Source = new BitmapImage(new Uri(@"\view\images\6.png", UriKind.Relative));
            }
            else
            {
                //TODO 英文图片
                title_pic.Source = new BitmapImage(new Uri(@"\view\Images\12.PNG", UriKind.Relative));
                DesignerHead4.Source = new BitmapImage(new Uri(@"\view\images\6.png", UriKind.Relative));
            }
            ///载入时数据装填到list,默认选中第一个
            users = userService.GetAllUsers();
            UsersInfo.ItemsSource = users;
            UsersInfo.SelectedIndex = 1;
            selectUser = (User)UsersInfo.SelectedItem;
            Refresh_RecordFrame_Action();
            ///心跳部分

            #region 通知公告   未激活不心跳
            SetterDAO setterDao = new SetterDAO();
            if (timerNotice == null)
            {

                while (setterDao.ListAll() != null)
                {
                    break;
                }
                BindNotice();

                timerNotice = new System.Timers.Timer();
                timerNotice.Elapsed += new System.Timers.ElapsedEventHandler((o, eea) => { BindNotice(); });

                timerNotice.Interval = CommUtil.GetHeartBeatRate();
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
            //检索后若结果唯一，则用户被选中
            //selectUser = null;
            if (users!=null&& users.Count==1) {
                UsersInfo.SelectedIndex = 0;
                Grid_Click(null,null);
            }
            //Grid_Click
        }

        //按钮：更新
        private void UserUpdata(object sender, RoutedEventArgs e)
        {
            // 切换用户图片的显示，解决线程占用问题
            BitmapImage bitmap = new BitmapImage(new Uri(@"\view\images\NoPhoto.png", UriKind.Relative));
            UserPhoto.Source = bitmap;

            //检查是否选中
            if((User)UsersInfo.SelectedItem == null)
            //if (selectUser == null)//4.7 lzh
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
               // MessageBox.Show(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
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
            //Console.WriteLine("123123:   " + user.User_Privateinfo);
            userUpdata.noPublicInfoText.Text = user.User_Privateinfo;
            //Console.WriteLine("123123aaaaa:   " + userUpdata.noPublicInfoText.Text);
            userUpdata.ShowDialog();

            //关闭后刷新界面
            users = userService.GetAllUsers();
            UsersInfo.ItemsSource = users;

            // 加载用户头像
            string photoUrl = CommUtil.GetUserPic() + selectUser.User_PhotoLocation;
            if (selectUser.User_PhotoLocation != null)
            {
                try
                {
                    BitmapImage b = new BitmapImage(new Uri(photoUrl, UriKind.Absolute));//打开图片
                    UserPhoto.Source = b.Clone();//将控件和图片绑定
                }
                catch (Exception ee)
                {

                }

            }
            //更新之后，刷新左下角
            Refresh_RecordFrame_Action();


        }

        //按钮：删除
        private void Delete_User(object sender, RoutedEventArgs e)
        {

            //检查是否选中
            if (selectUser == null)
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
                return;
            }

            Boolean dr = MessageBoxX.Question(LanguageUtils.ConvertLanguage("您确定删除该使用者信息？\n 使用者：", "Do You Want Delete The Subject?\n Subject:") + ((User)UsersInfo.SelectedItem).User_Name);
            if (dr == true)
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

        // 切换frame
        private void Radio_Check(object sender, RoutedEventArgs e)
        {
            Radio_Check_Action();
            Refresh_RecordFrame_Action();
        }

        //记录类型切换
        private void Radio_Check_Action()
        {

            User user = (User)UsersInfo.SelectedItem;
            if (user == null)
            {
                //MessageBox.Show("没选中用户");
                //return;
            }


            //MessageBox.Show("界面1 之前");
            if (is_signinformationrecord.IsChecked == true)
            {
                //MessageBox.Show("界面1");
                record.Source = new Uri("/view/Pages/Frame/SignInformationRecord_Frame.xaml", UriKind.Relative);
                return;
            }
            else if (is_trainingrecord.IsChecked == true)
            {
                //MessageBox.Show("界面2");
                record.Source = new Uri("/view/Pages/Frame/TrainingRecord_Frame.xaml", UriKind.Relative);
                return;
            }
            else if (is_physicalevaluation.IsChecked == true)
            {
                //MessageBox.Show("界面3");
                record.Source = new Uri("/view/Pages/Frame/PhysicaleValuation_Frame.xaml", UriKind.Relative);
            }

        }

        //按钮：文档输出
        private void Output_Document(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog
            {
                Filter = LanguageUtils.ConvertLanguage("Excel表格（*.xlsx）|*.xlsx", "Excel Sheet (*.xlsx)|*.xlsx")
            };
            //设置默认文件类型显示顺序
            sfd.FilterIndex = 1;
            //保存对话框是否记忆上次打开的目录
            sfd.RestoreDirectory = true;

            if (selectUser != null)
            {
                if (is_signinformationrecord.IsChecked == true)
                {
                    sfd.FileName = selectUser.User_Name + "-" + LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.VitalHistory") + "-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
                }
                if (is_trainingrecord.IsChecked == true)
                {
                    sfd.FileName = selectUser.User_Name + "-" + LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.TrainingHistory") + "-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
                }
                if (is_physicalevaluation.IsChecked == true)
                {
                    sfd.FileName = selectUser.User_Name + "-" + LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.PhysicalEvaluationHistory") + "-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
                }
            }

            if (sfd.ShowDialog() == true)
            {
                //此处做你想做的事 ...=ofd.FileName; 
                //获取当前
                try
                {
                    if (is_signinformationrecord.IsChecked == true)
                    {
                        //导出症状信息记录
                        if (selectUser != null)
                        {
                            //获取用户症状信息
                            List<SymptomInfo> lists = new SymptomService().GetByUserId(selectUser);
                            if (lists.Count > 0)
                            {
                                List<object> symptomInfoDtos = new List<object>();
                                foreach (SymptomInfo symptomInfo in lists)
                                {
                                    symptomInfoDtos.Add(new SymptomInfoExcelVO(symptomInfo));
                                }

                                //存放信息导出的列名
                                if (LanguageUtils.IsChainese())
                                {
                                    string[] colNames = { "训练日期", "血压(前)", "脉搏(前)", "心率(前)", "体温(前)", "血压(后)", "脉搏(后)", "心率(后)", "体温(后)", "身体倦怠", "腹泻", "摇晃", "心跳、气喘", "咳嗽、有痰", "发烧", "胸部、肚子痛", "没有食欲", "持续便秘", "感到头晕", "头痛", "其他", "没有相关症状", "是否参加", "水分摄取", "看护记录" };
                                    ExcelUtil.GenerateOrdinaryExcel(sfd.FileName.ToString(), selectUser, ExcelUtil.ToDataTable("症状信息记录", colNames, symptomInfoDtos));
                                }
                                else
                                {
                                    string[] colNames = { "Training date", "Blood pressure (front)", "Pulse (front)", "Heart rate (front)", "Body temperature (front)", "Blood pressure (after)", "Pulse (after)", "Heart rate (after)", "Body temperature (after)", "Physical exhaustion", "Diarrhea", "Shake", "Heartbeat, asthma", "Cough with phlegm", "Fever", "Chest and stomach pain", "No appetite", "Continuous constipation", "Feeling dizzy", "Headache", "Other", "No related symptoms", "Whether to participate", "Moisture intake", "Care record" };
                                    ExcelUtil.GenerateOrdinaryExcel(sfd.FileName.ToString(), selectUser, ExcelUtil.ToDataTable("Symptom information record", colNames, symptomInfoDtos));
                                }
                            }
                            else
                            {
                                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("抱歉，没有数据", "Sorry, No Data!"));
                            }
                        }
                    }
                    else if (is_trainingrecord.IsChecked == true)
                    {
                        //导出训练记录
                        if (selectUser != null)
                        {
                            List<TrainComprehensive> lists = new ExcelService().ListTrainExcekVOByUserId(selectUser.Pk_User_Id);
                            if (lists.Count > 0)
                            {
                                List<object> excelLists = new List<object>();
                                foreach (TrainComprehensive trainComprehensive in lists)
                                {
                                    excelLists.Add(new TrainExcelVO(trainComprehensive));
                                }
                                //Console.WriteLine(lists.ToString());
                                //存放信息导出的列名
                                if (LanguageUtils.IsChainese())
                                {
                                    string[] colNames = { "实施日期", "使用器械", "组数", "组的个数", "组间隔休息时间", " 砝码", "移乘方法", "自觉运动强度", "时间（秒）", " 距离（mm）", "总工作量（J）", "热量（cal）", "指数", "已完成组数", "时机、姿势", "备忘", "注意点", "利用者感想" };
                                    ExcelUtil.GenerateOrdinaryExcel(sfd.FileName.ToString(), selectUser, ExcelUtil.ToDataTable("训练记录", colNames, excelLists));
                                }
                                else
                                {
                                    string[] colNames = { "Date", "Device name", "Groups", "Number", "Break time", " Weights", "Transfer method", "Conscious exercise intensity", "Time (seconds)", " Distance (mm)", "Total workload (J)", "Calories (cal)", "Index", "Completed groups", "Timing, posture", "Remark", "Be careful", "User feelings" };
                                    ExcelUtil.GenerateOrdinaryExcel(sfd.FileName.ToString(), selectUser, ExcelUtil.ToDataTable("Training record", colNames, excelLists));
                                }
                            }
                            else
                            {
                                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("抱歉，没有数据", "Sorry, No Data!"));
                            }
                        }
                    }
                    else if (is_physicalevaluation.IsChecked == true)
                    {
                        //导出体力评价记录
                        if (selectUser != null)
                        {
                            List<PhysicalPower> lists = new ExcelService().ListPhysicalPowerExcelVO(selectUser.Pk_User_Id);
                            if (lists.Count > 0)
                            {
                                List<object> excelLists = new List<object>();
                                foreach (PhysicalPower physicalPower in lists)
                                {
                                    excelLists.Add(new PhysicaleExcelVO(physicalPower));
                                }

                                //存放信息导出的列名
                                if (LanguageUtils.IsChainese())
                                {
                                    string[] colNames = { "日期", "身高", "体重", "握力", "睁眼单脚站立", "功能性前伸", "坐姿体前屈", "time&up go", "5m步行-通常", "5m步行-最快", "10m步行", "6分钟步行", "2分钟踏步", "2分钟抬腿", "使用用者感想", "工作人员感想" };

                                    ExcelUtil.GenerateOrdinaryExcel(sfd.FileName.ToString(), selectUser,
                                        ExcelUtil.ToDataTable("体力评价记录", colNames, excelLists));
                                }
                                else
                                {
                                    string[] colNames = { "Date", "High", "Weight", "Grip", "Wink stand on one foot", "Functional reach", "Sitting body flexion", "time&up go", "5m walk - usually", "5m walk - fastest", "10m walk", "6 minutes walk", "2 String step", "2 minutes leg lift", "User experience", "Staff feelings" };

                                    ExcelUtil.GenerateOrdinaryExcel(sfd.FileName.ToString(), selectUser,
                                        ExcelUtil.ToDataTable("Physical Assessment Record", colNames, excelLists));
                                }

                            }
                            else
                            {
                                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("抱歉，没有数据", "Sorry, No Data!"));
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("文件被占用，请先关闭文件", "The file is occupied. Please close the file first"));
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("生成普通Excel异常");
                    return;
                }
            }
        }

        //按钮：制作报告，报表
        private void MakeReport(object sender, RoutedEventArgs e)
        {
            User user = (User)UsersInfo.SelectedItem;
            if (user == null)
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
                return;
            }

            //打开训练报告
            if (is_signinformationrecord.IsChecked == true || is_trainingrecord.IsChecked == true)
            {
                TrainingReport trainingReport = new TrainingReport
                {
                    Owner = Window.GetWindow(this),
                    ShowActivated = true,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                MaxHeight = SystemParameters.WorkArea.Size.Height,
                MaxWidth = SystemParameters.WorkArea.Size.Width

            };

                //设置用户信息
                if (selectUser != null)
                {
                    trainingReport.Pk_User_Id.Content = selectUser.Pk_User_Id;
                    trainingReport.User_Name.Content = selectUser.User_Name;
                    trainingReport.Current_User = selectUser;

                    List<TrainingAndSymptomBean> list = excelService.ListTrainingAndSymptomByUserId(selectUser.Pk_User_Id);
                    trainingReport.datalist.DataContext = list;
                    trainingReport.trainingAndSymptomBeans = list;//赋值全局变量
                    List<DateTime?> dateTimes = new List<DateTime?>();
                    foreach (TrainingAndSymptomBean tas in list)
                    {
                        dateTimes.Add(tas.Gmt_Create);
                    }
                    trainingReport.selectedDate = dateTimes;

                    if (list.Count != 0)
                    {
                        trainingReport.start_date.SelectedDate = list[0].Gmt_Create;//起始时间
                        trainingReport.end_date.SelectedDate = list[list.Count - 1].Gmt_Create;//终止时间
                    }
                    else
                    {
                        trainingReport.start_date.SelectedDate = DateTime.Now;
                        trainingReport.end_date.SelectedDate = DateTime.Now;
                    }
                    trainingReport.ShowDialog();
                }
            }
            //打开训练报告页面
            //else if (is_trainingrecord.IsChecked == true)
            //{
            //    TrainingReport trainingReport = new TrainingReport
            //    {
            //        Owner = Window.GetWindow(this),
            //        ShowActivated = true,
            //        ShowInTaskbar = false,
            //        WindowStartupLocation = WindowStartupLocation.CenterScreen
            //    };
            //    List<TrainInfo> list = new List<TrainInfo>();
            //    TrainInfo trainInfo = new TrainInfo
            //    {
            //        Gmt_Create = new DateTime(2012, 01, 02)
            //    };
            //    list.Add(trainInfo);
            //    Console.WriteLine(trainInfo.Gmt_Create);
            //    list.Add(trainInfo);
            //    trainingReport.datalist.DataContext = list;
            //    trainingReport.ShowDialog();
            //}
            //打开体力评价报告页面
            else
            {
                PhysicalAssessmentReport physicalAssessmentReport = new PhysicalAssessmentReport
                {
                    Owner = Window.GetWindow(this),
                    ShowActivated = true,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    MaxHeight = SystemParameters.WorkArea.Size.Height,
                    MaxWidth = SystemParameters.WorkArea.Size.Width
                };

                //设置用户信息
                if (selectUser != null)
                {
                    physicalAssessmentReport.Pk_User_Id.Content = selectUser.Pk_User_Id;
                    physicalAssessmentReport.User_Name.Content = selectUser.User_Name;
                    physicalAssessmentReport.Current_User = selectUser;

                    List<PhysicalPowerExcekVO> list = excelService.ListPhysicalPowerExcekVOByUserId(selectUser.Pk_User_Id);
                    physicalAssessmentReport.datalist.DataContext = list;
                    physicalAssessmentReport.physicalPowerExcekVOs = list;

                    List<DateTime?> dateTimes = new List<DateTime?>();
                    foreach (PhysicalPowerExcekVO tas in list)
                    {
                        dateTimes.Add(tas.Gmt_Create);
                    }
                    physicalAssessmentReport.selectedDate = dateTimes;

                    if (list.Count != 0)
                    {
                        physicalAssessmentReport.start_date.SelectedDate = list[0].Gmt_Create;//起始时间
                        physicalAssessmentReport.end_date.SelectedDate = list[list.Count - 1].Gmt_Create;//终止时间
                    }
                    else
                    {
                        physicalAssessmentReport.start_date.SelectedDate = DateTime.Now;
                        physicalAssessmentReport.end_date.SelectedDate = DateTime.Now;
                    }
                    physicalAssessmentReport.ShowDialog();
                }
                //List<TrainInfo> list = new List<TrainInfo>();
                //TrainInfo trainInfo = new TrainInfo
                //{
                //    Gmt_Create = new DateTime(2012, 01, 02)
                //};
                //list.Add(trainInfo);
                //Console.WriteLine(trainInfo.Gmt_Create);
                //list.Add(trainInfo);

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
                //WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            User user = (User)UsersInfo.SelectedItem;
            if (user == null)
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
                return;
            }
            if (string.IsNullOrEmpty(user.User_Phone) || string.IsNullOrEmpty(user.User_IDCard))
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("用户信息不完整！", "Subject information is incomplete!"));
                return;
            }
            inputTrainingResults.DataContext = user;
            inputTrainingResults.ShowDialog();
            Refresh_RecordFrame_Action();
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

                if (record.Content == null)
                {
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择症状信息再进行操作！", "Please Select A Symptom Info!"));
                    return;
                }

                DataGrid dataGrid = ((SignInformationRecord_Frame)record.Content).SignInformationRecord;

                SymptomInfoDTO symptomInfoDto = (SymptomInfoDTO)dataGrid.SelectedItem;
                User user = (User)UsersInfo.SelectedItem;
                if (user == null)
                {
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
                    return;
                }

                if (symptomInfoDto == null)
                {
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择症状信息再进行操作！", "Please Select A Symptom Info!"));
                    return;
                }

                Dictionary<string, Object> dictionary = new Dictionary<string, object>();
                dictionary.Add("user", user);
                dictionary.Add("symptomInfoDto", symptomInfoDto);
                viewSymptomInformation.DataContext = dictionary;
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

                Object o = record.Content;
                TrainDTO trainDto = null;
                User user = (User)UsersInfo.SelectedItem;
                if (o is TrainingRecord_Frame)
                {
                    TrainingRecord_Frame trainingRecordFrame = (TrainingRecord_Frame)o;
                    int index = trainingRecordFrame.TabControl1.SelectedIndex;
                    switch (index)
                    {
                        case 0:
                            trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord1.SelectedItem;
                            break;
                        case 1:
                            trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord2.SelectedItem;
                            break;
                        case 2:
                            trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord3.SelectedItem;
                            break;
                        case 3:
                            trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord4.SelectedItem;
                            break;
                        case 4:
                            trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord5.SelectedItem;
                            break;
                        case 5:
                            trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord6.SelectedItem;
                            break;
                    }
                }

                if (user == null)
                {
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
                    return;
                }
                if (trainDto == null)
                {//判断是否选择了训练信息
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择训练信息再进行操作！", "Please Select A Train Info!"));
                    return;
                }
                Dictionary<string, Object> dic = new Dictionary<string, object>();
                dic.Add("user", user);
                dic.Add("trainDto", trainDto);
                viewTrainingResults.DataContext = dic;
                viewTrainingResults.ShowDialog();
            }
            //打开体力评价详细信息
            else if (is_physicalevaluation.IsChecked == true)
            {
                ViewManualMvaluation viewManualMvaluation = new ViewManualMvaluation
                {
                    Owner = Window.GetWindow(this),
                    ShowActivated = true,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                Object o = record.Content;
                PhysicaleDTO physicaleDto = null;
                User user = (User)UsersInfo.SelectedItem;
                if (o is PhysicaleValuation_Frame)
                {
                    PhysicaleValuation_Frame physicaleValuationFrame = (PhysicaleValuation_Frame)o;
                    physicaleDto = (PhysicaleDTO)physicaleValuationFrame.PhysicaleValuation.SelectedItem;
                }

                if (user == null)
                {
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
                    return;
                }
                if (physicaleDto == null)
                {//判断是否选择了训练信息
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择体力评价再进行操作！", "Please Select A Physical Evalution!"));
                    return;
                }
                Dictionary<string, Object> dic = new Dictionary<string, object>();
                dic.Add("user", user);
                dic.Add("physicaleDto", physicaleDto);
                viewManualMvaluation.DataContext = dic;
                viewManualMvaluation.ShowDialog();
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {

            // Window window = (Window)this.Parent;
            ////window.Content = new Login();
            //// System.Environment.Exit(0);
            //window.Content = 
            //window.Show();
            Login mwin = new Login();
            Application.Current.MainWindow = mwin;
            ((Window)this.Parent).Close();
            mwin.Show();
            //Login login = new Login
            //{
            //    Owner = Window.GetWindow(this),
            //    ShowActivated = true,
            //    ShowInTaskbar = false,
            //    WindowStartupLocation = WindowStartupLocation.CenterScreen
            //};

            //login.ShowDialog();
            //window.Close();

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

                    //如果用户没有被上传则return，不允许发心跳，否则就按照不合法冻结了
                    if (new UploadManagementDAO().CheckExistAuth() != null)
                    {
                        return;
                    }

                    HeartBeatOffice heartBeatOffice = new HeartBeatOffice();
                    HttpHeartBeat result = heartBeatOffice.GetHeartBeatByCurrent();
                    //心跳直接上传   !HttpSender.Ping() ||
                    if (result == null)
                    {
                        //如果没有取到值
                        return;
                    }
                    string jsonStr = HttpSender.POSTByJsonStr("communicationController/analysisJson",
                        JsonTools.Obj2JSONStrNew<HttpHeartBeat>(result));
                    HttpHeartBeat webResult = JsonTools.DeserializeJsonToObject<HttpHeartBeat>(jsonStr);
                    //本地数据更改
                    if (webResult == null)
                    {
                        return;
                    }
                    heartBeatOffice.SolveHeartbeat(webResult);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (webResult.authStatus == 0)
                        {
                            //正常心跳不处理
                        }
                        else if (webResult.authStatus == 1)
                        {
                            //冻结，弹窗，然后关闭窗口
                            // 程序强制退出
                            authDao.UpdateByUserName(webResult.username, 1);
                            // 停止定时器
                            timerNotice.Stop();

                            MessageBoxX.Warning(LanguageUtils.ConvertLanguage("用户被冻结，即将退出，请联系宝德龙管理员解冻！", "The user is frozen, will exit, please contact the administrator thaw!"));
                            Environment.Exit(0);
                        }
                        else if (webResult.authStatus == 2)
                        {
                            //解冻，只需要更改数据库。界面无反馈，不处理
                            //authDao.UpdateByUserName(webResult.username, 2);
                        }
                        else if (webResult.authStatus == 3)
                        {
                            //永久离线，只需要更改数据库。界面无反馈，不处理
                            //authDao.UpdateByUserName(webResult.username, 3);
                        }
                        else if (webResult.authStatus == 4)
                        {
                            //已删除，按照冻结处理
                            //authDao.UpdateByUserName(webResult.username, 1);
                            
                            timerNotice.Stop();
                            MessageBoxX.Warning(LanguageUtils.ConvertLanguage("用户被删除，即将退出，请联系宝德龙管理员恢复！", "The user is removed, will exit, please contact the administrator to restore!"));
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
            Object o = record.Content;
            TrainDTO trainDto = null;
            User user = (User)UsersInfo.SelectedItem;
            if (o is TrainingRecord_Frame)
            {
                TrainingRecord_Frame trainingRecordFrame = (TrainingRecord_Frame)o;
                int index = trainingRecordFrame.TabControl1.SelectedIndex;
                switch (index)
                {
                    case 0:
                        trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord1.SelectedItem;
                        break;
                    case 1:
                        trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord2.SelectedItem;
                        break;
                    case 2:
                        trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord3.SelectedItem;
                        break;
                    case 3:
                        trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord4.SelectedItem;
                        break;
                    case 4:
                        trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord5.SelectedItem;
                        break;
                    case 5:
                        trainDto = (TrainDTO)trainingRecordFrame.TrainingRecord6.SelectedItem;
                        break;
                }
            }

            if (user == null)
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
                return;
            }
            //if (trainDto == null)
            //{
            //    //判断是否选择了训练信息
            //    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择训练信息再进行操作！", "Please Select A Train Info!"));
            //    return;
            //}

            InputSymptomInformation w2 = new InputSymptomInformation
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            Dictionary<string, Object> dic = new Dictionary<string, object>();
            dic.Add("user", user);
            //dic.Add("trainDto", trainDto);
            w2.DataContext = dic;
            w2.ShowDialog();
            Refresh_RecordFrame_Action();
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
            User user = (User)UsersInfo.SelectedItem;
            if (user == null)
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
                return;
            }
            if (string.IsNullOrEmpty(user.User_Phone) || string.IsNullOrEmpty(user.User_IDCard))
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("用户信息不完整！", "Subject information is incomplete!"));
                return;
            }
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
            if (selectUser != null)
            {
                inputManualMvaluation.Pk_User_Id.Content = selectUser.Pk_User_Id;
                inputManualMvaluation.User_Name.Content = selectUser.User_Name;
                inputManualMvaluation.Current_User = selectUser;
                inputManualMvaluation.ShowDialog();
                Refresh_RecordFrame_Action();
            }
            else
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
            }

        }

        //设置按钮
        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            Window window = (Window)this.Parent;
            window.Content = new DesignPage1();
        }

        private void UsersInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh_RecordFrame_Action();
        }

        /// <summary>
        /// 刷新右下角frame
        /// </summary>
        /// 给frame加入数据
        private void Refresh_RecordFrame_Action()
        {

            User user = selectUser;

            if (user == null)
            {
                //MessageBox.Show("请选择用户");
                return;
            }

            if (user.User_Name != "" && user.User_Name != null)
            {
                if (is_signinformationrecord.IsChecked == true)
                {
                    List<SymptomInfo> symptomInfos = new SymptomService().GetByUserId(user);
                    List<SymptomInfoDTO> symptomInfoDtos = new List<SymptomInfoDTO>();
                    foreach (SymptomInfo symptomInfo in symptomInfos)
                    {
                        symptomInfoDtos.Add(new SymptomInfoDTO(symptomInfo));
                    }

                    //展示在frame
                    SignInformationRecord_Frame signInformationRecordFrame = new SignInformationRecord_Frame();//(SignInformationRecord_Frame)o;
                    signInformationRecordFrame.SignInformationRecord.ItemsSource = symptomInfoDtos;
                    record.Content = signInformationRecordFrame;
                }
                else if (is_trainingrecord.IsChecked == true)
                {

//                    Dictionary<int, List<TrainDTO>> dic = new TrainService().getTrainDTOByUser(user);
                    Dictionary<int, List<TrainDTO>> dic = new TrainService().getTrainDTOByUserA(user);
                    TrainingRecord_Frame trainingRecordFrame = new TrainingRecord_Frame();
                    List<TrainDTO> trainDtos = new List<TrainDTO>();
                    dic.TryGetValue((int)DeviceType.X01, out trainDtos);
                    trainingRecordFrame.TrainingRecord1.ItemsSource = trainDtos;
                    dic.TryGetValue((int)DeviceType.X05, out trainDtos);
                    trainingRecordFrame.TrainingRecord2.ItemsSource = trainDtos;
                    dic.TryGetValue((int)DeviceType.X04, out trainDtos);
                    trainingRecordFrame.TrainingRecord3.ItemsSource = trainDtos;
                    dic.TryGetValue((int)DeviceType.X03, out trainDtos);
                    trainingRecordFrame.TrainingRecord4.ItemsSource = trainDtos;
                    dic.TryGetValue((int)DeviceType.X06, out trainDtos);
                    trainingRecordFrame.TrainingRecord5.ItemsSource = trainDtos;
                    dic.TryGetValue((int)DeviceType.X02, out trainDtos);
                    trainingRecordFrame.TrainingRecord6.ItemsSource = trainDtos;

                    record.Content = trainingRecordFrame;
                }
                else if (is_physicalevaluation.IsChecked == true)
                {
                    List<PhysicalPower> physicalPowers = new PhysicaleValuationService().GetByUserId(user);
                    List<PhysicaleDTO> physicaleDTOs = new List<PhysicaleDTO>();
                    foreach (PhysicalPower physicalPower in physicalPowers)
                    {
                        PhysicaleDTO physicaleDTO = new PhysicaleDTO(physicalPower);
                        //为空，说明有可能是不符合格式的数据
                        if (physicaleDTO.ID != null)
                        {
                            physicaleDTOs.Add(physicaleDTO);
                        }
                    }

                    //展示在frame
                    PhysicaleValuation_Frame physicaleValuation_Frame = new PhysicaleValuation_Frame();
                    physicaleValuation_Frame.PhysicaleValuation.ItemsSource = physicaleDTOs;
                    record.Content = physicaleValuation_Frame;
                }
            }
        }

        private void record_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            Refresh_RecordFrame_Action();
        }

        private void image_load(object sender, RoutedEventArgs e)
        {
            //加载图片
            if (LanguageUtils.IsChainese())
            {
                DesignerHead4.Source = new BitmapImage(new Uri("/view/Images/6.png", UriKind.RelativeOrAbsolute));
                DesignerHead4.Margin = new Thickness(10, 0, 0, 0);
                DesignerHead4.Height = 60;
                DesignerHead4.Width = 225;
                DesignerHead3.Height = 70;
                subhead.FontSize = 18;
            }
                else
                {
                DesignerHead4.Source = new BitmapImage(new Uri("/view/Images/5_5.png", UriKind.RelativeOrAbsolute));
                DesignerHead4.Margin = new Thickness(97, 18, 0, 0);
                DesignerHead4.Height = 25;
                DesignerHead4.Width = 136;
                DesignerHead3.Height = 60;
                subhead.FontSize = 15;
               
                }
            }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Object o = record.Content;
            User user = (User)UsersInfo.SelectedItem;

            if (user == null)
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择用户再进行操作！", "Please Select A Subject!"));
                return;
            }

            if (is_signinformationrecord.IsChecked ==false || record.Content == null)
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择症状信息再进行操作！", "Please Select A Symptom Info!"));
                return;
            }

            DataGrid dataGrid = ((SignInformationRecord_Frame)record.Content).SignInformationRecord;

            SymptomInfoDTO symptomInfoDto = (SymptomInfoDTO)dataGrid.SelectedItem;
            if (symptomInfoDto == null)
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择症状信息再进行操作！", "Please Select A Symptom Info!"));
                return;
            }

            UpdateSymptominfo w2 = new UpdateSymptominfo
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            Dictionary<string, Object> dic = new Dictionary<string, object>();
            dic.Add("user", user);
            dic.Add("symptom", symptomInfoDto);
            w2.DataContext = dic;
            w2.ShowDialog();
            Refresh_RecordFrame_Action();
        }
    }
    }
