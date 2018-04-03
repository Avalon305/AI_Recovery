using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using spms.dao;
using spms.entity;
using spms.util;
using spms.view.dto;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// ManualMvaluation.xaml 的交互逻辑
    /// </summary>
    public partial class ViewManualMvaluation : Window
    {
        private User user;
        private PhysicaleDTO physicaleDto;
        List<String> list = new List<string>
        {
            "T字拐杖",
            "T-Kane",
            "4字拐杖",
            "Qtr-Cane",
            "步行器",
            "Walker",
            "其他",
            "Other",
            "右手前面支持",
            "Right Frontal",
            "右手侧面支持",
            "Right Lateral",
            "左手前面支持",
            "Left Frontal",
            "左手侧面支持",
            "Left Lateral",
            "两手前面支持",
            "Both Frontal",
            "两手侧面支持",
            "Both Lateral"
        };
        public ViewManualMvaluation()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;
        }

        //取消操作，关闭窗口
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewbox.MaxHeight = SystemParameters.WorkArea.Size.Height;
            viewbox.MaxWidth = SystemParameters.WorkArea.Size.Width;
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            //绑定数据
            Dictionary<string, object> dic = (Dictionary<string, Object>)DataContext;
            user = (User)dic["user"];
            physicaleDto = (PhysicaleDTO)dic["physicaleDto"];
            Load_Data();
        }

        private void Load_Data()
        {
            l1.Content = user.User_Name;
            l2.Content = user.Pk_User_Id;
            implementation_date.Content = physicaleDto.Gmt_Create.ToString();
            PhysicalPower physicalPower = new PhysicalPowerDAO().Load(physicaleDto.ID);
            //身高
            string[] ppHigh = Regex.Replace(physicalPower.PP_High, @"param\d", "").Split(new char[] { ',' });
            height_first.Text = ppHigh[1];
            if (ppHigh[3] == "照片（侧面、前面）" || ppHigh[3] == "Picture(front/lateral)")
            {
                height_condition.IsChecked = true;
            }
            height_duty.Text = ppHigh[4];

            //体重
            string[] ppWeight = Regex.Replace(physicalPower.PP_Weight, @"param\d", "").Split(new char[] { ',' });
            weight_first.Text = ppWeight[1];
            if (ppWeight[3] != "")
            {
                weight_condition.IsChecked = true;
                weight_condition_text.Text = ppWeight[3].Split(new char[]{':'})[1];
            }
            weight_duty.Text = ppWeight[4];

            //握力
            string[] ppGrip = Regex.Replace(physicalPower.PP_Grip, @"param\d", "").Split(new char[] { ',' });
            if (ppGrip[0] == "左" || ppGrip[0] == "L")
            {
                grip_left.IsChecked = true;
            }
            else if (ppGrip[0] == "右" || ppGrip[0] == "R")
            {
                grit_right.IsChecked = true;
            }

            grip_first.Text = ppGrip[1];
            grip_second.Text = ppGrip[2];
            if (ppGrip[3] == "站姿" || ppGrip[3] == "Standing")
            {
                grid_stand.IsChecked = true;
            }
            else if (ppGrip[3] == "坐姿" || ppGrip[3] == "Seated")
            {
                grid_sit.IsChecked = true;
            }

            grip_duty.Text = ppGrip[4];


            //睁眼单脚站立
            string[] ppEyeOpenStand = Regex.Replace(physicalPower.PP_EyeOpenStand, @"param\d", "").Split(new char[] { ',' });
            if (ppEyeOpenStand[0] == "左" || ppEyeOpenStand[0] == "L")
            {
                stand_left.IsChecked = true;
            }
            else if (ppEyeOpenStand[0] == "右" || ppEyeOpenStand[0] == "R")
            {
                stand_right.IsChecked = true;
            }
            stand_first.Text = ppEyeOpenStand[1];
            stand_second.Text = ppEyeOpenStand[2];
            if (ppEyeOpenStand[3] == "不需要支持" || ppEyeOpenStand[3] == "no support")
            {
                stand_nosupport.IsChecked = true;
            }
            else if (ppEyeOpenStand[3] == "通过介助支持" || ppEyeOpenStand[3] == "Supported by assistance")
            {
                stand_support.IsChecked = true;
            }
            else if (ppEyeOpenStand[3] != "")
            {
                string[] strings = ppEyeOpenStand[3].Split(new char[] { '(' });
                stand_toolsupport.IsChecked = true;
                stand_comBox1.Text = strings[0];
                stand_comBox2.Text = strings[1].Substring(0, strings[1].Length - 1);
            }
            
            stand_duty.Text = ppEyeOpenStand[4];

            //功能性前伸
            string[] ppFunctionProtract = Regex.Replace(physicalPower.PP_FunctionProtract, @"param\d", "").Split(new char[] { ',' });
            if (ppFunctionProtract[0] == "两手" || ppFunctionProtract[0] == "Both hands")
            {
                protrack_twohands.IsChecked = true;
            }
            else if (ppFunctionProtract[0] == "左" || ppFunctionProtract[0] == "L")
            {
                protrack_left.IsChecked = true;
            }
            else if (ppFunctionProtract[0] == "右"|| ppFunctionProtract[0] == "R")
            {
                protrack_right.IsChecked = true;
            }

            protrack_first.Text = ppFunctionProtract[1];
            protrack_second.Text = ppFunctionProtract[2];
            if (ppFunctionProtract[3] == "胳膊不能举到肩的高度（肩关节弯曲度）"|| ppFunctionProtract[3] == "Difficult to rise arms to shoulder(Shoulder Flex)")
            {
                protrack_curvature.IsChecked = true;
            }
            else if (ppFunctionProtract[3] == "以坐姿进行测定" || ppFunctionProtract[3] == "Seated Position")
            {
                protrack_sit.IsChecked = true;
            }

            protrack_duty.Text = ppFunctionProtract[4];

            //坐姿体前屈
            string[] ppSitandReach = Regex.Replace(physicalPower.PP_SitandReach, @"param\d", "").Split(new char[] { ',' });
            if (ppSitandReach[0] == "左" || ppSitandReach[0] == "L")
            {
                c17.IsChecked = true;
            }
            else if (ppSitandReach[0] == "右" || ppSitandReach[0] == "R")
            {
                c18.IsChecked = true;
            }

            c_first.Text = ppSitandReach[1];
            c_second.Text = ppSitandReach[2];
            if (ppSitandReach[3] == "膝弯曲（有）" || ppSitandReach[3] == "Knee Flexion")
            {
                c19.IsChecked = true;
            }
            else if (ppSitandReach[3] == "膝弯曲（无）"|| ppSitandReach[3] == "No Knee Flexion")
            {
                c20.IsChecked = true;
            }
            c_duty.Text = ppSitandReach[4];

            //Time UP & GO
            string[] ppTimeUpGo = Regex.Replace(physicalPower.PP_TimeUpGo, @"param\d", "").Split(new char[] { ',' });
            if (ppTimeUpGo[0] == "基本方法"|| ppTimeUpGo[0] == "Basic")
            {
                c21.IsChecked = true;
            }
            else if (ppTimeUpGo[0] == "常规外方法"|| ppTimeUpGo[0] == "Modified")
            {
                c22.IsChecked = true;
            }
            time_first.Text = ppTimeUpGo[1];
            time_second.Text = ppTimeUpGo[2];
            if (ppTimeUpGo[3] == "独自步行"|| ppTimeUpGo[3] == "Independent")
            {
                c23.IsChecked = true;
            }
            else if (list.Contains(ppTimeUpGo[3]))
            {
                c24.IsChecked = true;
                comBox3.Text = ppTimeUpGo[3];
            }
            else if(ppTimeUpGo[3] != "")
            {
                c25.IsChecked = true;
                text1.Text = ppTimeUpGo[3];

            }
            time_duty.Text = ppTimeUpGo[4];


            //5m步行，通常
            string[] ppWalk5MileGeneral = Regex.Replace(physicalPower.PP_Walk5MileGeneral, @"param\d", "").Split(new char[] { ',' });
            if (ppWalk5MileGeneral[0] == "基本方法"|| ppWalk5MileGeneral[0] == "Basic")
            {
                c26.IsChecked = true;
            }
            else if (ppWalk5MileGeneral[0] == "常规外方法"|| ppWalk5MileGeneral[0] == "Modified")
            {
                c27.IsChecked = true;
            }
            five1_first.Text = ppWalk5MileGeneral[1];
            five1_second.Text = ppWalk5MileGeneral[2];
            if (ppWalk5MileGeneral[3] == "独自步行"|| ppWalk5MileGeneral[3] == "Independent")
            {
                c28.IsChecked = true;
            }
            else if (list.Contains(ppWalk5MileGeneral[3]))
            {
                c29.IsChecked = true;
                comBox4.Text = ppWalk5MileGeneral[3];
            }
            else if (ppWalk5MileGeneral[3] != "")
            {
                c30.IsChecked = true;
                text2.Text = ppWalk5MileGeneral[3];
            }
            five1_duty.Text = ppWalk5MileGeneral[4];

            //5m步行，最快
            string[] ppWalk5MileFast = Regex.Replace(physicalPower.PP_Walk5MileFast, @"param\d", "").Split(new char[] { ',' });
            if (ppWalk5MileFast[0] == "基本方法"|| ppWalk5MileFast[0] == "Basic")
            {
                c31.IsChecked = true;
            }
            else if (ppWalk5MileFast[0] == "常规外方法"|| ppWalk5MileFast[0] == "Modified")
            {
                c32.IsChecked = true;
            }

            five2_first.Text = ppWalk5MileFast[1];
            five2_second.Text = ppWalk5MileFast[2];
            if (ppWalk5MileFast[3] == "独自步行"|| ppWalk5MileFast[3] == "Independent")
            {
                c33.IsChecked = true;
            }
            else if (list.Contains(ppWalk5MileFast[3]))
            {
                c34.IsChecked = true;
                comBox5.Text = ppWalk5MileFast[3];
            }
            else if (ppWalk5MileFast[3] != "")
            {
                c35.IsChecked = true;
                text3.Text = ppWalk5MileFast[3];
            }

            five2_duty.Text = ppWalk5MileFast[4];

            //10m步行
            string[] ppWalk10Mile = Regex.Replace(physicalPower.PP_Walk10Mile, @"param\d", "").Split(new char[] { ',' });
            string methodStr = "";
            if (ppWalk10Mile[0].Contains("通常")|| ppWalk10Mile[0].Contains("Normal"))
            {
                methodStr = LanguageUtils.ConvertLanguage("通常", "Normal");
            }
            else if (ppWalk10Mile[0].Contains("最快")|| ppWalk10Mile[0].Contains("utmost"))
            {
                methodStr = LanguageUtils.ConvertLanguage("最快", "utmost");
            }
            method.Text = methodStr;
            if (ppWalk10Mile[0].Contains("基本方法")|| ppWalk10Mile[0].Contains("Basic"))
            {
                c36.IsChecked = true;
            }
            else if (ppWalk10Mile[0].Contains("常规外方法")|| ppWalk10Mile[0].Contains("Modified"))
            {
                c37.IsChecked = true;
            }

            ten_first.Text = ppWalk10Mile[1];
            ten_second.Text = ppWalk10Mile[2];
            if (ppWalk10Mile[3] == "独自步行"|| ppWalk10Mile[3] == "Independent")
            {
                c38.IsChecked = true;
            }
            else if (list.Contains(ppWalk10Mile[3]))
            {
                c39.IsChecked = true;
                comBox6.Text = ppWalk10Mile[3];
            }
            else if (ppWalk10Mile[3] != "")
            {
                c40.IsChecked = true;
                text4.Text = ppWalk10Mile[3];
            }

            ten_duty.Text = ppWalk10Mile[4];

            //6分钟步行
            string[] ppWalk6Minute = Regex.Replace(physicalPower.PP_Walk6Minute, @"param\d", "").Split(new char[] { ',' });
            six_first.Text = ppWalk6Minute[1];
            six_second.Text = ppWalk6Minute[2];
            if (ppWalk6Minute[3] == "独自步行"|| ppWalk6Minute[3] == "Independent")
            {
                c43.IsChecked = true;
            }
            else if (list.Contains(ppWalk6Minute[3]))
            {
                c44.IsChecked = true;
                comBox7.Text = ppWalk6Minute[3];
            }
            else if (ppWalk6Minute[3] != "")
            {
                c45.IsChecked = true;
                text5.Text = ppWalk6Minute[3];
            }

            six_duty.Text = ppWalk6Minute[4];

            //2分钟踏步
            string[] ppStep2Minute = Regex.Replace(physicalPower.PP_Step2Minute, @"param\d", "").Split(new char[] { ',' });
            two_first.Text = ppStep2Minute[1];
            two_second.Text = ppStep2Minute[2];
            if (ppStep2Minute[3] == "独自步行"|| ppStep2Minute[3] == "Independent")
            {
                c46.IsChecked = true;
            }
            else if (list.Contains(ppStep2Minute[3]))
            {
                c47.IsChecked = true;
                comBox8.Text = ppStep2Minute[3];
            }
            else if (ppStep2Minute[3] != "")
            {
                c48.IsChecked = true;
                text6.Text = ppStep2Minute[3];
            }

            two_duty.Text = ppStep2Minute[4];

            //2分钟抬腿
            string[] ppLegRaise2Minute = Regex.Replace(physicalPower.PP_LegRaise2Minute, @"param\d", "").Split(new char[] { ',' });
            twoleg_first.Text = ppLegRaise2Minute[1];
            twoleg_second.Text = ppLegRaise2Minute[2];
            if (ppLegRaise2Minute[3] == "站姿"|| ppLegRaise2Minute[3] == "Standing")
            {
                c49.IsChecked = true;
            }
            else if (ppLegRaise2Minute[3] == "坐姿"|| ppLegRaise2Minute[3] == "Seated")
            {
                c50.IsChecked = true;
            }

            twoleg_duty.Text = ppLegRaise2Minute[4];

            //使用者感想
            string ppUserMemo = physicalPower.PP_UserMemo;
            user_think.Text = ppUserMemo;

            //工作人员感想
            string ppWorkerMemo = physicalPower.PP_WorkerMemo;
            worker_think.Text = ppWorkerMemo;

        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GoBack(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }
    }
}
