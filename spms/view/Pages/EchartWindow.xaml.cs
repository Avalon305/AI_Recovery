using AI_Sports.AISports.Dao;
using spms.dao;
using spms.dao.app;
using spms.entity;
using spms.entity.newEntity;
using spms.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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
using System.Windows.Shapes;

namespace spms.view.Pages
{
    /// <summary>
    /// EchartWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EchartWindow : Window
    {
        DataCodeDAO DataCodeDAO = new DataCodeDAO();
        static MusclePieChartDAO musclePieChartDAO = new MusclePieChartDAO();
        List<DataCode> ListDataCode = new List<DataCode>();
        //心率集合
        static List<String> heartRateList = new List<string>();
        //选中的用户ID
        long userId = new long();
       

        //感想集合
        static List<string> thoughtsList = new List<string>();
        static List<DateTime> createTime = new List<DateTime>();

        public EchartWindow()
        {
            InitializeComponent();
            //在选择框加载可选的设备
            ListDataCode = DataCodeDAO.ListByTypeId("DEVICE");
            this.comboxDevice.ItemsSource = ListDataCode;
            this.comboxDevice2.ItemsSource = ListDataCode;

            //初始化选中的用户id
            userId = CommUtil.ParseInt(CommUtil.GetSettingString("selectedUserId"));
            //选择的设备ID
            int deviceId = CommUtil.ParseInt(comboxDevice.SelectedValue.ToString());
            //初始化心率折线图表 默认加载第一台设备，1条记录
            RefreshChart(userId, 1, 1);
            //初始化感想折线
            drawUserThoughts(1);
        }
        //查询按钮，刷新加载图表
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            //选择的次数
            int num = CommUtil.ParseInt(comboxNum.Text);
            int deviceId = CommUtil.ParseInt(comboxDevice.SelectedValue.ToString());


            RefreshChart(userId, deviceId, num);
        }
        /// <summary>
        /// 感想折线查询 根据设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            int deviceId = CommUtil.ParseInt(comboxDevice2.SelectedValue.ToString());

            drawUserThoughts(deviceId);
        }
        /// <summary>
        /// 根据参数刷新图表界面方法
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceId"></param>
        /// <param name="num"></param>
        public void RefreshChart(long userId, int deviceId, int num)
        {

            //获取心率集合做处理
            List<PrescriptionResultTwo> prescriptionResults = musclePieChartDAO.ListHeartRate(userId, deviceId, num);
            
            if (prescriptionResults.Count > 0)
            {
                //更新心率集合 先清空
                heartRateList.Clear();
                foreach (var item in prescriptionResults)
                {
                    if (item.Heart_rate_list != null && item.Heart_rate_list != "")
                    {
                        string heartRate = item.Heart_rate_list.Replace("*", ",");
                        heartRateList.Add(heartRate);
                    }
                    
                }

                //更新加载html图表
                HeartRateChart.ObjectForScripting = new WebAerobic();
                //Endurance.ObjectForScripting = new WebEndurancePie();
                //获取项目的根路径
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string rootpath = path.Substring(0, path.LastIndexOf("bin"));
                //图表中每一个设置的折线都不能为空必须加载，所以分为三种图表加载
                switch (num)
                {
                    case 1:
                        this.HeartRateChart.Navigate(new Uri(rootpath + "spms/Echarts/dist/Line1.html"));
                        break;
                    case 2:
                        this.HeartRateChart.Navigate(new Uri(rootpath + "spms/Echarts/dist/Line2.html"));
                        break;
                    case 3:
                        this.HeartRateChart.Navigate(new Uri(rootpath + "spms/Echarts/dist/Line3.html"));
                        break;
                    case 4:
                        this.HeartRateChart.Navigate(new Uri(rootpath + "spms/Echarts/dist/Line4.html"));
                        break;
                    case 5:
                        this.HeartRateChart.Navigate(new Uri(rootpath + "spms/Echarts/dist/Line5.html"));
                        break;
                    default:
                        break;
                }


            }
            else
            {
                MessageBox.Show("无数据！","Warning");
                return;
            }
            

            
        }
        /// <summary>
        /// 获得第n条心率集合,从0开始
        /// </summary>
        /// <returns></returns>
        public static string GetHeartRateList(int index)
        {
            if (heartRateList.Count > index)
            {
                return heartRateList[index];

            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 封装方法 根据不同设备查询感想
        /// </summary>
        public void drawUserThoughts(int deviceId)
        {
            //查询处方结果
            List<PrescriptionResultTwo> result = musclePieChartDAO.ListUserThoughts(userId, deviceId);
            
            if (result.Count > 0)
            {
                thoughtsList.Clear();
                createTime.Clear();
                //构建y轴集合 使用感想
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    thoughtsList.Add(result.ElementAt(i).pr_userthoughts.ToString());
                    createTime.Add(result.ElementAt(i).Gmt_create.Value);
                }


                //更新加载html图表
                ThoughtsLine.ObjectForScripting = new WebUserThoughtsLine();
                //Endurance.ObjectForScripting = new WebEndurancePie();
                //获取项目的根路径
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string rootpath = path.Substring(0, path.LastIndexOf("bin"));
                //图表中每一个设置的折线都不能为空必须加载，所以分为三种图表加载

                this.ThoughtsLine.Navigate(new Uri(rootpath + "spms/Echarts/dist/ThoughtsLine.html"));
            }
            else
            {
                MessageBox.Show("无数据！", "Warning");

                return;
            }
            

        }


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        [System.Runtime.InteropServices.ComVisible(true)]
        public class WebAerobic
        {
            //加载一条数据
            public string drawCF()
            {
                string heartRate = EchartWindow.GetHeartRateList(0);
                return heartRate;
            }
            //加载两条
            public string drawRF()
            {
                string heartRate = EchartWindow.GetHeartRateList(1);
                return heartRate;
            }
            //加载第三条数据
            public string drawRate()
            {
                string heartRate = EchartWindow.GetHeartRateList(2);
                return heartRate;
            }
            //加载第四条数据
            public string drawLine4()
            {
                string heartRate = EchartWindow.GetHeartRateList(3);
                return heartRate;
            }
            //加载第五条数据
            public string drawLine5()
            {
                string heartRate = EchartWindow.GetHeartRateList(4);
                return heartRate;
            }
        }


        /// <summary>
        /// 用户使用感想折线图
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        [System.Runtime.InteropServices.ComVisible(true)]
        public class WebUserThoughtsLine
        {
            /// <summary>
            /// 返回y轴使用感想
            /// </summary>
            /// <returns></returns>
            public string drawUserThoughts()
            {
                string result = string.Join(",", EchartWindow.thoughtsList);
                return result;
            }
            /// <summary>
            /// 返回x轴时间
            /// </summary>
            /// <returns></returns>
            public string drawXCreateTime()
            {
                string result = string.Join(",", EchartWindow.createTime);
                return result;
                //DataContractJsonSerializer json = new DataContractJsonSerializer(createTime.GetType());
                //string szJson = "";
                ////序列化
                //using (MemoryStream stream = new MemoryStream())
                //{
                //    json.WriteObject(stream, createTime);
                //    szJson = Encoding.UTF8.GetString(stream.ToArray());

                //}

                //Console.WriteLine("时间串" + szJson);
                //return szJson;
            }

        }

        
    }
}
