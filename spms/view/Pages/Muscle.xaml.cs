using spms.entity;
using spms.service;
using spms.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
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
using System.Runtime.Serialization.Json;
using System.IO;

namespace spms.view.Pages
{
    /// <summary>
    /// muscle.xaml 的交互逻辑
    /// </summary>
    public partial class Muscle : Window
    {

        //语音分析的后台任务 不适用后台任务则界面卡死 无法进行其他操作
        //private BackgroundWorker worker = new BackgroundWorker();

        public static User user;

        public Muscle()
        {
            InitializeComponent();

            //user = (User)DataContext;
            //各部位肌肉运动个数饼图表
            this.Web.ObjectForScripting = new WebPie();
            //设备力度平均值柱状图
            this.DeviceForce.ObjectForScripting = new DeviceForceStacked();
            //设备耗能饼图
            this.DeviceEnergy.ObjectForScripting = new DeviceEnergyPie();
            //Endurance.ObjectForScripting = new WebEndurancePie();
            //获取项目的根路径
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string rootpath = path.Substring(0, path.LastIndexOf("bin"));
            //Console.WriteLine("项目根路径"+rootpath);
            //Endurance.Navigate(new Uri(rootpath + "AISports.Echarts/dist/EndurancePie.html"));

            this.Web.Navigate(new Uri(rootpath + "spms/Echarts/dist/Pie.html"));

            this.DeviceForce.Navigate(new Uri(rootpath + "spms/Echarts/dist/StrengthStacked.html"));

            this.DeviceEnergy.Navigate(new Uri(rootpath + "spms/Echarts/dist/EndurancePie.html"));


        }



        /// <summary>
        /// 后退按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new Uri("/AI_Sports;component/AISports.View/Pages/analyze.xaml", UriKind.Relative));


        }

        ///// <summary>
        ///// 初始化后台任务worker
        ///// </summary>
        //private void InitializeBackgroundWorker()
        //{
        //    //初始化注册后台事件
        //    worker.WorkerReportsProgress = true;
        //    worker.WorkerSupportsCancellation = true;
        //    // worker 要做的事情 使用了匿名的事件响应函数
        //    worker.DoWork += (o, ea) =>
        //    {
        //        StringBuilder speechBuilder = new StringBuilder();
        //        speechBuilder.Append("您可以在此查看您的主动肌对抗肌锻炼进度和各大肌肉群锻炼比例，以及各个设备对应锻炼的肌肉群分布，从而结合您的训练目标选择最适合您的设备进行锻炼。");
        //        Console.WriteLine("肌肉页面语音文本：" + speechBuilder.ToString());
        //        SpeechUtil.read(speechBuilder.ToString());
        //    };
        //    //worker完成事件
        //    worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        //}

        /// <summary>
        /// 语音分析按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //    private void Speech_Click(object sender, RoutedEventArgs e)
        //    {

        //        //防止连续点击造成的后台任务繁忙异常
        //        if (worker.IsBusy == true)
        //        {
        //            Console.WriteLine("后台语音任务正忙");

        //            return;
        //        }
        //        else
        //        {
        //            //显示停止按钮
        //            this.stop.Visibility = Visibility.Visible;
        //            //禁用分析按钮
        //            this.speech.IsEnabled = false;


        //            //注意：运行了下面这一行代码，worker才真正开始工作。上面都只是声明定义而已。
        //            worker.RunWorkerAsync();
        //        }





        //    }
        //    /// <summary>
        //    /// 语音后台任务完成事件
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //    {
        //        try
        //        {
        //            Console.WriteLine("语音播放完毕");
        //            //隐藏停止按钮
        //            this.stop.Visibility = Visibility.Hidden;
        //            //可用分析按钮
        //            this.speech.IsEnabled = true;
        //        }
        //        catch (Exception)
        //        {

        //            throw new NotImplementedException();

        //        }
        //    }





        //    /// <summary>
        //    /// 停止语音分析
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void Stop_Click(object sender, RoutedEventArgs e)
        //    {
        //        try
        //        {
        //            //取消朗读
        //            SpeechUtil.stop();
        //            //取消后台任务
        //            this.worker.CancelAsync();
        //            //隐藏停止按钮
        //            this.stop.Visibility = Visibility.Hidden;
        //            //可用分析按钮
        //            this.speech.IsEnabled = true;
        //        }
        //        catch (Exception)
        //        {
        //            Console.WriteLine("停止语音按钮异常");
        //            throw;
        //        }

        //    }

        //}



        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        [System.Runtime.InteropServices.ComVisible(true)]//给予权限并设置可见
        public class WebPie
        {
            //选中的用户id
            long userId = CommUtil.ParseInt(CommUtil.GetSettingString("selectedUserId"));

            public String Show()
            {
                return "60";
            }
            MusclePieChartService musclePieChartService = new MusclePieChartService();

            //后背锻炼个数
            public int? backTraining()
            {

                return musclePieChartService.selectNumByUserAndMuscle(userId, "背部");
            }

            //力量循环胸部训练个数
            public int? chestTraining()
            {
                return musclePieChartService.selectNumByUserAndMuscle(userId, "胸");
            }

            //力量循环腿部训练个数
            public int? legTraining()
            {
                return musclePieChartService.selectNumByUserAndMuscle(userId, "腿部");

            }

            //力量循环手臂训练个数
            public int? armTraining()
            {
                return musclePieChartService.selectNumByUserAndMuscle(userId, "手臂");

            }

            //力量循环躯干训练个数
            public int? trunkTraining()
            {
                return musclePieChartService.selectNumByUserAndMuscle(userId, "躯干");

            }




        }
        /// <summary>
        /// 平均力度柱状图
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        [System.Runtime.InteropServices.ComVisible(true)]//给予权限并设置可见
        public class DeviceForceStacked
        {
            ProcessService processService = new ProcessService();
            //选中的用户id
            long userId = CommUtil.ParseInt(CommUtil.GetSettingString("selectedUserId"));

            ////力量循环X轴动态数据加载(查询系统时间前24小时的创建时间)
            //public string selectCreateTime()
            //{
            //    List<DateTime> CreateTime = processService.selectCreateTime(trainingCourseId);
            //    DataContractJsonSerializer json = new DataContractJsonSerializer(CreateTime.GetType());
            //    string szJson = "";
            //    //序列化
            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        json.WriteObject(stream, CreateTime);
            //        szJson = Encoding.UTF8.GetString(stream.ToArray());

            //    }

            //    Console.WriteLine("时间串" + szJson);
            //    return szJson;
            //}

            //每台设备力度平均值  三代
            public String avgValue()
            {
                List<double> avgValue = processService.selectAvgValue(userId);
                DataContractJsonSerializer json = new DataContractJsonSerializer(avgValue.GetType());
                string szJson = "";
                //序列化
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, avgValue);
                    szJson = Encoding.UTF8.GetString(stream.ToArray());

                }

                Console.WriteLine("图表力度平均值序列化成功：" + szJson.ToString());
                return szJson;
            }

            ////返回值行数
            //public int count()
            //{
            //    int count = processService.selectCount().Value;
            //    return count;
            //}


        }
        /// <summary>
        /// 设备耗能饼图
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        [System.Runtime.InteropServices.ComVisible(true)]//给予权限并设置可见
        public class DeviceEnergyPie
        {
            //选中的用户id
            long userId = CommUtil.ParseInt(CommUtil.GetSettingString("selectedUserId"));

            public String Show()
            {
                return "60";
            }

            MusclePieChartService musclePieChartService = new MusclePieChartService();

            /// <summary>
            /// 坐式推胸机耗能
            /// </summary>
            /// <returns></returns>
            public int? chestTraining()
            {

                return musclePieChartService.selectEnergyByUserAndMuscle(userId,1);
            }

            /// <summary>
            /// 腿部推蹬机
            /// </summary>
            /// <returns></returns>
            public int? legPushTraining()
            {
                return musclePieChartService.selectEnergyByUserAndMuscle(userId,2);
            }

            /// <summary>
            /// 腹肌训练机
            /// </summary>
            /// <returns></returns>
            public int? abdomenTraining()
            {
                return musclePieChartService.selectEnergyByUserAndMuscle(userId,3);

            }

            /// <summary>
            /// 三头肌训练机
            /// </summary>
            /// <returns></returns>
            public int? tricepsTraining()
            {
                return musclePieChartService.selectEnergyByUserAndMuscle(userId,4);

            }

            /// <summary>
            /// 腿部外弯机
            /// </summary>
            /// <returns></returns>
            public int? legOutTraining()
            {
                return musclePieChartService.selectEnergyByUserAndMuscle(userId,5);

            }
            /// <summary>
            /// 腿部内弯机
            /// </summary>
            /// <returns></returns>
            public int? legInTraining()
            {
                return musclePieChartService.selectEnergyByUserAndMuscle(userId, 6);

            }
            /// <summary>
            /// 蝴蝶机
            /// </summary>
            /// <returns></returns>
            public int? butterflyTraining()
            {
                return musclePieChartService.selectEnergyByUserAndMuscle(userId, 7);

            }
            /// <summary>
            /// 反向蝴蝶机
            /// </summary>
            /// <returns></returns>
            public int? reverseButterflyTraining()
            {
                return musclePieChartService.selectEnergyByUserAndMuscle(userId, 8);

            }

            /// <summary>
            /// 坐式背部伸展机
            /// </summary>
            /// <returns></returns>
            public int? sitBackTraining()
            {
                return musclePieChartService.selectEnergyByUserAndMuscle(userId, 9);

            }
            /// <summary>
            /// 坐式划船机
            /// </summary>
            /// <returns></returns>
            public int? sitRowingTraining()
            {
                return musclePieChartService.selectEnergyByUserAndMuscle(userId, 10);

            }


        }

    }
}
