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
using NLog;
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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private User user;
        private PhysicaleDTO physicaleDto;
        /*
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
        };*/
        
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
            logger.Info("user:" + user + "physicaleDto:" + physicaleDto);
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
            if (LanguageUtils.EqualsResource(ppHigh[3], "PhysicalEvaluationFormView.Picture(front/lateral)"))
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
            if (LanguageUtils.EqualsResource(ppGrip[0], "PhysicalEvaluationFormView.L"))
            {
                grip_left.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppGrip[0], "PhysicalEvaluationFormView.R"))
            {
                grit_right.IsChecked = true;
            }

            grip_first.Text = ppGrip[1];
            grip_second.Text = ppGrip[2];
            if (LanguageUtils.EqualsResource(ppGrip[3], "PhysicalEvaluationFormView.Standing"))
            {
                grid_stand.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppGrip[3], "PhysicalEvaluationFormView.Seated"))
            {
                grid_sit.IsChecked = true;
            }

            grip_duty.Text = ppGrip[4];


            //睁眼单脚站立
            string[] ppEyeOpenStand = Regex.Replace(physicalPower.PP_EyeOpenStand, @"param\d", "").Split(new char[] { ',' });
            if (LanguageUtils.EqualsResource(ppEyeOpenStand[0], "PhysicalEvaluationFormView.L"))
            {
                stand_left.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppEyeOpenStand[0], "PhysicalEvaluationFormView.R"))
            {
                stand_right.IsChecked = true;
            }
            stand_first.Text = ppEyeOpenStand[1];
            stand_second.Text = ppEyeOpenStand[2];
            if (LanguageUtils.EqualsResource(ppEyeOpenStand[3], "PhysicalEvaluationFormView.nosupport"))
            {
                stand_nosupport.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppEyeOpenStand[3], "PhysicalEvaluationFormView.Supportedbyassistance"))
            {
                stand_support.IsChecked = true;
            }
            else if (ppEyeOpenStand[3] != "")
            {
                string[] strings = ppEyeOpenStand[3].Split(new char[] { '(' });
                stand_toolsupport.IsChecked = true;
                string stand2 = strings[1].Substring(0, strings[1].Length - 1);
                //第一个框
                if (LanguageUtils.EqualsResource(strings[0], "PhysicalEvaluationFormView.T-Kane"))
                {
                    stand_comBox1.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.T-Kane");
                }
                else if (LanguageUtils.EqualsResource(strings[0], "PhysicalEvaluationFormView.Qtr-Cane"))
                {
                    stand_comBox1.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Qtr-Cane");
                }
                else if (LanguageUtils.EqualsResource(strings[0], "PhysicalEvaluationFormView.Walker"))
                {
                    stand_comBox1.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Walker");
                }
                else if (LanguageUtils.EqualsResource(strings[0], "PhysicalEvaluationFormView.Other"))
                {
                    stand_comBox1.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Other");
                }
                //第二个框
                if (LanguageUtils.EqualsResource(stand2, "PhysicalEvaluationFormView.RightFrontal"))
                {
                    stand_comBox2.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.RightFrontal");
                }
                else if (LanguageUtils.EqualsResource(stand2, "PhysicalEvaluationFormView.RightLateral"))
                {
                    stand_comBox2.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.RightLateral");
                }
                else if (LanguageUtils.EqualsResource(stand2, "PhysicalEvaluationFormView.LeftFrontal"))
                {
                    stand_comBox2.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.LeftFrontal");
                }
                else if (LanguageUtils.EqualsResource(stand2, "PhysicalEvaluationFormView.LeftLateral"))
                {
                    stand_comBox2.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.LeftLateral");
                }
                else if (LanguageUtils.EqualsResource(stand2, "PhysicalEvaluationFormView.BothFrontal"))
                {
                    stand_comBox2.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.BothFrontal");
                }
                else if (LanguageUtils.EqualsResource(stand2, "PhysicalEvaluationFormView.BothLateral"))
                {
                    stand_comBox2.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.BothLateral");
                }
            }
            
            stand_duty.Text = ppEyeOpenStand[4];

            //功能性前伸
            string[] ppFunctionProtract = Regex.Replace(physicalPower.PP_FunctionProtract, @"param\d", "").Split(new char[] { ',' });
            if (LanguageUtils.EqualsResource(ppFunctionProtract[0], "PhysicalEvaluationFormView.Bothhands"))
            {
                protrack_twohands.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppFunctionProtract[0], "PhysicalEvaluationFormView.L"))
            {
                protrack_left.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppFunctionProtract[0], "PhysicalEvaluationFormView.R"))
            {
                protrack_right.IsChecked = true;
            }

            protrack_first.Text = ppFunctionProtract[1];
            protrack_second.Text = ppFunctionProtract[2];
            if (LanguageUtils.EqualsResource(ppFunctionProtract[3], "PhysicalEvaluationFormView.Difficulttorisearmstoshoulder(ShoulderFlex)"))
            {
                protrack_curvature.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppFunctionProtract[3], "PhysicalEvaluationFormView.SeatedPosition"))
            {
                protrack_sit.IsChecked = true;
            }

            protrack_duty.Text = ppFunctionProtract[4];

            //坐姿体前屈
            string[] ppSitandReach = Regex.Replace(physicalPower.PP_SitandReach, @"param\d", "").Split(new char[] { ',' });
            if (LanguageUtils.EqualsResource(ppSitandReach[0], "PhysicalEvaluationFormView.L"))
            {
                c17.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppSitandReach[0], "PhysicalEvaluationFormView.R"))
            {
                c18.IsChecked = true;
            }

            c_first.Text = ppSitandReach[1];
            c_second.Text = ppSitandReach[2];
            if (LanguageUtils.EqualsResource(ppSitandReach[3], "PhysicalEvaluationFormView.KneeFlexion"))
            {
                c19.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppSitandReach[3], "PhysicalEvaluationFormView.NoKneeFlexion"))
            {
                c20.IsChecked = true;
            }
            c_duty.Text = ppSitandReach[4];

            //Time UP & GO
            string[] ppTimeUpGo = Regex.Replace(physicalPower.PP_TimeUpGo, @"param\d", "").Split(new char[] { ',' });
            if (LanguageUtils.EqualsResource(ppTimeUpGo[0], "PhysicalEvaluationFormView.Basic"))
            {
                c21.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppTimeUpGo[0], "PhysicalEvaluationFormView.Modified"))
            {
                c22.IsChecked = true;
            }
            time_first.Text = ppTimeUpGo[1];
            time_second.Text = ppTimeUpGo[2];
            if (LanguageUtils.EqualsResource(ppTimeUpGo[3], "PhysicalEvaluationFormView.Independent"))
            {
                c23.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppTimeUpGo[3], "PhysicalEvaluationFormView.T-Kane"))
            {
                c24.IsChecked = true;
                comBox3.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.T-Kane");
            }
            else if (LanguageUtils.EqualsResource(ppTimeUpGo[3], "PhysicalEvaluationFormView.Qtr-Cane"))
            {
                c24.IsChecked = true;
                comBox3.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Qtr-Cane");
            }
            else if (LanguageUtils.EqualsResource(ppTimeUpGo[3], "PhysicalEvaluationFormView.Walker"))
            {
                c24.IsChecked = true;
                comBox3.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Walker");
            }
            else if (LanguageUtils.EqualsResource(ppTimeUpGo[3], "PhysicalEvaluationFormView.Other"))
            {
                c24.IsChecked = true;
                comBox3.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Other");
            }
            else if(ppTimeUpGo[3] != "")
            {
                c25.IsChecked = true;
                text1.Text = ppTimeUpGo[3];

            }
            time_duty.Text = ppTimeUpGo[4];


            //5m步行，通常
            string[] ppWalk5MileGeneral = Regex.Replace(physicalPower.PP_Walk5MileGeneral, @"param\d", "").Split(new char[] { ',' });
            if (LanguageUtils.EqualsResource(ppWalk5MileGeneral[0], "PhysicalEvaluationFormView.Basic"))
            {
                c26.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppWalk5MileGeneral[0], "PhysicalEvaluationFormView.Modified"))
            {
                c27.IsChecked = true;
            }
            five1_first.Text = ppWalk5MileGeneral[1];
            five1_second.Text = ppWalk5MileGeneral[2];
            if (LanguageUtils.EqualsResource(ppWalk5MileGeneral[3], "PhysicalEvaluationFormView.Independent"))
            {
                c28.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppWalk5MileGeneral[3], "PhysicalEvaluationFormView.T-Kane"))
            {
                c29.IsChecked = true;
                comBox4.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.T-Kane");
            }
            else if (LanguageUtils.EqualsResource(ppWalk5MileGeneral[3], "PhysicalEvaluationFormView.Qtr-Cane"))
            {
                c29.IsChecked = true;
                comBox4.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Qtr-Cane");
            }
            else if (LanguageUtils.EqualsResource(ppWalk5MileGeneral[3], "PhysicalEvaluationFormView.Walker"))
            {
                c29.IsChecked = true;
                comBox4.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Walker");
            }
            else if (LanguageUtils.EqualsResource(ppWalk5MileGeneral[3], "PhysicalEvaluationFormView.Other"))
            {
                c29.IsChecked = true;
                comBox4.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Other");
            }
            else if (ppWalk5MileGeneral[3] != "")
            {
                c30.IsChecked = true;
                text2.Text = ppWalk5MileGeneral[3];
            }
            five1_duty.Text = ppWalk5MileGeneral[4];

            //5m步行，最快
            string[] ppWalk5MileFast = Regex.Replace(physicalPower.PP_Walk5MileFast, @"param\d", "").Split(new char[] { ',' });
            if (LanguageUtils.EqualsResource(ppWalk5MileFast[0], "PhysicalEvaluationFormView.Basic"))
            {
                c31.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppWalk5MileFast[0], "PhysicalEvaluationFormView.Modified"))
            {
                c32.IsChecked = true;
            }

            five2_first.Text = ppWalk5MileFast[1];
            five2_second.Text = ppWalk5MileFast[2];
            if (LanguageUtils.EqualsResource(ppWalk5MileFast[3], "PhysicalEvaluationFormView.Independent"))
            {
                c33.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppWalk5MileFast[3], "PhysicalEvaluationFormView.T-Kane"))
            {
                c34.IsChecked = true;
                comBox5.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.T-Kane");
            }
            else if (LanguageUtils.EqualsResource(ppWalk5MileFast[3], "PhysicalEvaluationFormView.Qtr-Cane"))
            {
                c34.IsChecked = true;
                comBox5.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Qtr-Cane");
            }
            else if (LanguageUtils.EqualsResource(ppWalk5MileFast[3], "PhysicalEvaluationFormView.Walker"))
            {
                c34.IsChecked = true;
                comBox5.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Walker");
            }
            else if (LanguageUtils.EqualsResource(ppWalk5MileFast[3], "PhysicalEvaluationFormView.Other"))
            {
                c34.IsChecked = true;
                comBox5.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Other");
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
                methodStr = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Normal");
            }
            else if (ppWalk10Mile[0].Contains("最快")|| ppWalk10Mile[0].Contains("utmost"))
            {
                methodStr = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.utmost");
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
            if (LanguageUtils.EqualsResource(ppWalk10Mile[3], "PhysicalEvaluationFormView.Independent"))
            {
                c38.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppWalk10Mile[3], "PhysicalEvaluationFormView.T-Kane"))
            {
                c39.IsChecked = true;
                comBox6.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.T-Kane");
            }
            else if (LanguageUtils.EqualsResource(ppWalk10Mile[3], "PhysicalEvaluationFormView.Qtr-Cane"))
            {
                c39.IsChecked = true;
                comBox6.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Qtr-Cane");
            }
            else if (LanguageUtils.EqualsResource(ppWalk10Mile[3], "PhysicalEvaluationFormView.Walker"))
            {
                c39.IsChecked = true;
                comBox6.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Walker");
            }
            else if (LanguageUtils.EqualsResource(ppWalk10Mile[3], "PhysicalEvaluationFormView.Other"))
            {
                c39.IsChecked = true;
                comBox6.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Other");
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
            if (LanguageUtils.EqualsResource(ppWalk6Minute[3], "PhysicalEvaluationFormView.Independent"))
            {
                c43.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppWalk6Minute[3], "PhysicalEvaluationFormView.T-Kane"))
            {
                c44.IsChecked = true;
                comBox7.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.T-Kane");
            }
            else if (LanguageUtils.EqualsResource(ppWalk6Minute[3], "PhysicalEvaluationFormView.Qtr-Cane"))
            {
                c44.IsChecked = true;
                comBox7.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Qtr-Cane");
            }
            else if (LanguageUtils.EqualsResource(ppWalk6Minute[3], "PhysicalEvaluationFormView.Walker"))
            {
                c44.IsChecked = true;
                comBox7.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Walker");
            }
            else if (LanguageUtils.EqualsResource(ppWalk6Minute[3], "PhysicalEvaluationFormView.Other"))
            {
                c44.IsChecked = true;
                comBox7.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Other");
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
            if (LanguageUtils.EqualsResource(ppStep2Minute[3], "PhysicalEvaluationFormView.Independent"))
            {
                c46.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppStep2Minute[3], "PhysicalEvaluationFormView.T-Kane"))
            {
                c47.IsChecked = true;
                comBox8.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.T-Kane");
            }
            else if (LanguageUtils.EqualsResource(ppStep2Minute[3], "PhysicalEvaluationFormView.Qtr-Cane"))
            {
                c47.IsChecked = true;
                comBox8.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Qtr-Cane");
            }
            else if (LanguageUtils.EqualsResource(ppStep2Minute[3], "PhysicalEvaluationFormView.Walker"))
            {
                c47.IsChecked = true;
                comBox8.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Walker");
            }
            else if (LanguageUtils.EqualsResource(ppStep2Minute[3], "PhysicalEvaluationFormView.Other"))
            {
                c47.IsChecked = true;
                comBox8.Text = LanguageUtils.GetCurrentLanuageStrByKey("PhysicalEvaluationFormView.Other");
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
            if (LanguageUtils.EqualsResource(ppLegRaise2Minute[3], "PhysicalEvaluationFormView.Standing"))
            {
                c49.IsChecked = true;
            }
            else if (LanguageUtils.EqualsResource(ppLegRaise2Minute[3], "PhysicalEvaluationFormView.Seated"))
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
