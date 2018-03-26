using spms.entity;
using spms.service;
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
using System.Windows.Shapes;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// InputManualMvaluation.xaml 的交互逻辑
    /// </summary>
    public partial class InputManualMvaluation : Window
    {
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private new bool IsEnabled = true;
        private bool UnEnabled = false;
        public InputManualMvaluation()
        {
            InitializeComponent();

            List<String> list = new List<string>
            {
                "T字拐杖",
                "4字拐杖",
                "步行器",
                "其他"
            };
            stand_comBox1.ItemsSource = list;
            time_up_other_selected.ItemsSource = list;
            walk5_tools_selected.ItemsSource = list;
            walk5_fastest_tools_selected.ItemsSource = list;
            walk10_tools_selected.ItemsSource = list;
            walk6_tools_selected.ItemsSource = list;
            step2_tools_selected.ItemsSource = list;
        }

        public User Current_User { set; get; }

        /// <summary>
        /// 保存按钮的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            PhysicalPower physicalPower = new PhysicalPower();

            //id
            physicalPower.FK_user_Id = Current_User.Pk_User_Id;
            //日期
            physicalPower.Gmt_Create = implementation_date.SelectedDate;
            physicalPower.Gmt_Modified = implementation_date.SelectedDate;

            //1.拼接身高
            physicalPower.PP_High = "param1,"+(height_first.Text != "" ? height_first.Text + "," : "param2,") + "param3,"+ (height_condition.IsChecked == true ? height_condition.Content : "param4," + (height_duty.Text.ToString() != "" ? height_duty.Text.ToString() + "," : "param5"));
            //2.拼接体重
            physicalPower.PP_Weight = "param1," + (weight_first.Text != "" ? weight_first.Text + "," : "param2,") + "param3," + (weight_condition.IsChecked == true ? "服装:"+weight_condition_text.Text.ToString() : "param4," + (weight_duty.Text.ToString() != "" ? weight_duty.Text.ToString() + "," : "param5"));
            //3.拼接握力
            if (grip_left.IsChecked == true)
            {
                builder.Append(grip_left.Content + ",");
            }
            else if (grit_right.IsChecked == true)
            {
                builder.Append(grip_left.Content + ",");
            }
            else
            {
                builder.Append("param1,");
            }
            builder.Append(grip_first.Text != "" ? grip_first.Text + "," : "param2,");
            builder.Append(grip_second.Text != "" ? grip_second.Text + "," : "param3,");
            if (grid_stand.IsChecked == true)
            {
                builder.Append(grid_stand.Content + ",");
            }
            else if (grid_sit.IsChecked == true)
            {
                builder.Append(grid_sit.Content + ",");
            }
            else
            {
                builder.Append("param4,");
            }
            builder.Append(grip_duty.Text != "" ? grip_duty.Text : "param5");
            physicalPower.PP_Grip = builder.ToString();
            builder.Clear();
            //4.睁眼单脚站立
            if (stand_left.IsChecked == true)
            {
                builder.Append(stand_left.Content + ",");
            }
            else if (stand_right.IsChecked == true)
            {
                builder.Append(stand_right.Content + ",");
            }
            else
            {
                builder.Append("param1,");
            }
            builder.Append(stand_first.Text != "" ? stand_first.Text + "," : "param2,");
            builder.Append(stand_second.Text != "" ? stand_second.Text + "," : "param3,");
            if (stand_nosupport.IsChecked == true)
            {
                builder.Append(stand_nosupport.Content + ",");
            }
            else if (stand_toolsupport.IsChecked == true)
            {
                builder.Append(stand_toolsupport.Content + ",");
            }
            else if (stand_support.IsChecked == true)
            {
                builder.Append(stand_support.Content + ",");
            }
            else
            {
                builder.Append("param4,");
            }
            builder.Append(stand_duty.Text != "" ? stand_duty.Text : "param5");
            physicalPower.PP_EyeOpenStand = builder.ToString();
            builder.Clear();
            //5.功能性前伸
            if (protrack_twohands.IsChecked == true)
            {
                builder.Append(protrack_twohands.Content + ",");
            }
            else if (protrack_left.IsChecked == true)
            {
                builder.Append(protrack_left.Content + ",");
            }
            else if (protrack_right.IsChecked == true)
            {
                builder.Append(protrack_right.Content + ",");
            }
            else
            {
                builder.Append("param1,");
            }
            builder.Append(protrack_first.Text != "" ? protrack_first.Text + "," : "param2,");
            builder.Append(protrack_second.Text != "" ? protrack_second.Text + "," : "param3,");
            if (protrack_curvature.IsChecked == true)
            {
                builder.Append(protrack_curvature.Content + ",");
            }
            else if (protrack_sit.IsChecked == true)
            {
                builder.Append(protrack_sit.Content + ",");
            }
            else
            {
                builder.Append("param4,");
            }
            builder.Append(protrack_duty.Text != "" ? protrack_duty.Text : "param5");
            physicalPower.PP_FunctionProtract = builder.ToString();
            builder.Clear();
            //6.坐姿体前屈
            if (measuring_leg_left.IsChecked == true)
            {
                builder.Append(measuring_leg_left.Content + ",");
            }
            else if (measuring_leg_right.IsChecked == true)
            {
                builder.Append(measuring_leg_right.Content + ",");
            }
            else
            {
                builder.Append("param1,");
            }
            builder.Append(sitand_reach_first.Text != "" ? sitand_reach_first.Text + "," : "param2,");
            builder.Append(sitand_reach_second.Text != "" ? sitand_reach_second.Text + "," : "param3,");
            if (knee_flexure_is.IsChecked == true)
            {
                builder.Append(knee_flexure_is.Content + ",");
            }
            else if (knee_flexure_not.IsChecked == true)
            {
                builder.Append(knee_flexure_not.Content + ",");
            }
            else
            {
                builder.Append("param4,");
            }
            builder.Append(sitand_reach_duty.Text != "" ? sitand_reach_duty.Text : "param5");
            physicalPower.PP_SitandReach = builder.ToString();
            builder.Clear();
            //7.time&up go
            if (time_up_base.IsChecked == true)
            {
                builder.Append(time_up_base.Content + ",");
            }
            else if (time_up_routine.IsChecked == true)
            {
                builder.Append(time_up_routine.Content + ",");
            }
            else
            {
                builder.Append("param1,");
            }
            builder.Append(time_up_first.Text != "" ? time_up_first.Text + "," : "param2,");
            builder.Append(time_up_second.Text != "" ? time_up_second.Text + "," : "param3,");
            if (time_up_walk.IsChecked == true)
            {
                builder.Append(time_up_walk.Content + ",");
            }
            else if (time_up_tools.IsChecked == true)
            {
                builder.Append(time_up_other_selected.SelectedValue.ToString() + ",");
            }
            else if (time_up_other.IsChecked == true)
            {
                builder.Append(time_up_other_content.Text.ToString() + ",");
            }
            else
            {
                builder.Append("param4,");
            }
            builder.Append(time_up_duty.Text != "" ? time_up_duty.Text : "param5");
            physicalPower.PP_TimeUpGo = builder.ToString();
            builder.Clear();
            //7.5m步行&#13;（通常）
            if (walk5_base.IsChecked == true)
            {
                builder.Append(walk5_base.Content + ",");
            }
            else if (walk5_routine.IsChecked == true)
            {
                builder.Append(walk5_routine.Content + ",");
            }
            else
            {
                builder.Append("param1,");
            }
            builder.Append(walk5_first.Text != "" ? walk5_first.Text + "," : "param2,");
            builder.Append(walk5_second.Text != "" ? walk5_second.Text + "," : "param3,");
            if (walk5_walk.IsChecked == true)
            {
                builder.Append(walk5_walk.Content + ",");
            }
            else if (walk5_tools.IsChecked == true)
            {
                builder.Append(walk5_tools_selected.SelectedValue.ToString() + ",");
            }
            else if (walk5_other.IsChecked == true)
            {
                builder.Append(walk5_other_content.Text.ToString() + ",");
            }
            else
            {
                builder.Append("param4,");
            }
            builder.Append(walk5_duty.Text != "" ? walk5_duty.Text : "param5");
            physicalPower.PP_Walk5MileGeneral = builder.ToString();
            builder.Clear();
            //8.5m步行&#13;（很快）
            if (walk5_fastest_base.IsChecked == true)
            {
                builder.Append(walk5_fastest_base.Content + ",");
            }
            else if (walk5_fastest_routine.IsChecked == true)
            {
                builder.Append(walk5_fastest_routine.Content + ",");
            }
            else
            {
                builder.Append("param1,");
            }
            builder.Append(walk5_fastest_first.Text != "" ? walk5_fastest_first.Text + "," : "param2,");
            builder.Append(walk5_fastest_second.Text != "" ? walk5_fastest_second.Text + "," : "param3,");
            if (walk5_fastest_walk.IsChecked == true)
            {
                builder.Append(walk5_fastest_walk.Content + ",");
            }
            else if (walk5_fastest_tools.IsChecked == true)
            {
                builder.Append(walk5_fastest_tools_selected.SelectedValue.ToString() + ",");
            }
            else if (walk5_fastest_other.IsChecked == true)
            {
                builder.Append(walk5_fastest_other_content.Text.ToString() + ",");
            }
            else
            {
                builder.Append("param4,");
            }
            builder.Append(walk5_fastest_duty.Text != "" ? walk5_fastest_duty.Text : "param5");
            physicalPower.PP_Walk5MileFast = builder.ToString();
            builder.Clear();
            //9.10m步行
            if (walk10_base.IsChecked == true)
            {
                builder.Append(walk10_base.Content + "(" + walk10_comboBox.SelectedValue.ToString() + "),");
            }
            else if (walk10_routine.IsChecked == true)
            {
                builder.Append(walk10_routine.Content + "(" + walk10_comboBox.SelectedValue.ToString() + "),");
            }
            else
            {
                builder.Append(walk10_comboBox.SelectedValue.ToString()+",");
            }
            builder.Append(walk10_first.Text != "" ? walk10_first.Text + "," : "param2,");
            builder.Append(walk10_second.Text != "" ? walk10_second.Text + "," : "param3,");
            if (walk10_walk.IsChecked == true)
            {
                builder.Append(walk10_walk.Content + ",");
            }
            else if (walk10_tools.IsChecked == true)
            {
                builder.Append(walk10_tools_selected.SelectedValue.ToString() + ",");
            }
            else if (walk10_other.IsChecked == true)
            {
                builder.Append(walk10_other_content.Text.ToString() + ",");
            }
            else
            {
                builder.Append("param4,");
            }
            builder.Append(walk10_duty.Text != "" ? walk10_duty.Text : "param5");
            physicalPower.PP_Walk10Mile = builder.ToString();
            builder.Clear();
            //10.6分钟步行
            builder.Append("param1,");
            builder.Append(walk6_first.Text != "" ? walk6_first.Text + "," : "param2,");
            builder.Append(walk6_second.Text != "" ? walk6_second.Text + "," : "param3,");
            if (walk6_walk.IsChecked == true)
            {
                builder.Append(walk6_walk.Content + ",");
            }
            else if (walk6_tools.IsChecked == true)
            {
                builder.Append(walk6_tools_selected.SelectedValue.ToString() + ",");
            }
            else if (walk6_other.IsChecked == true)
            {
                builder.Append(walk6_other_content.Text.ToString() + ",");
            }
            else
            {
                builder.Append("param4,");
            }
            builder.Append(walk6_duty.Text != "" ? walk6_duty.Text : "param5");
            physicalPower.PP_Walk6Minute = builder.ToString();
            builder.Clear();
            //11.2分钟踏步
            builder.Append("param1,");
            builder.Append(step2_first.Text != "" ? step2_first.Text + "," : "param2,");
            builder.Append(step2_second.Text != "" ? step2_second.Text + "," : "param3,");
            if (step2_walk.IsChecked == true)
            {
                builder.Append(step2_walk.Content + ",");
            }
            else if (step2_tools.IsChecked == true)
            {
                builder.Append(step2_tools_selected.SelectedValue.ToString() + ",");
            }
            else if (step2_other.IsChecked == true)
            {
                builder.Append(step2_other_content.Text.ToString() + ",");
            }
            else
            {
                builder.Append("param4,");
            }
            builder.Append(step2_duty.Text != "" ? step2_duty.Text : "param5");
            physicalPower.PP_Step2Minute = builder.ToString();
            builder.Clear();
            //12.2分钟抬腿
            builder.Append("param1,");
            builder.Append(leg2_first.Text != "" ? leg2_first.Text + "," : "param2,");
            builder.Append(leg2_second.Text != "" ? leg2_second.Text + "," : "param3,");
            if (leg2_stand.IsChecked == true)
            {
                builder.Append(leg2_stand.Content + ",");
            }
            else if (leg2_sit.IsChecked == true)
            {
                builder.Append(leg2_sit.Content + ",");
            }
            else
            {
                builder.Append("param4,");
            }
            builder.Append(leg2_duty.Text != "" ? leg2_duty.Text : "param5");
            physicalPower.PP_LegRaise2Minute = builder.ToString();
            builder.Clear();
            //13.使用者感想
            TextRange text = new TextRange(user_feel.Document.ContentStart, user_feel.Document.ContentEnd);
            physicalPower.PP_UserMemo = text.Text.ToString();
            //13.使用者感想
            TextRange text1 = new TextRange(worker_feel.Document.ContentStart, worker_feel.Document.ContentEnd);
            physicalPower.PP_WorkerMemo = text1.Text.ToString();

            Console.WriteLine(physicalPower.ToString());

            if (new PhysicaleValuationService().AddPhysicalPower(physicalPower) == 1)
            {
                MessageBox.Show("保存成功");
                this.Close();
            } 
        }
        //取消操作，关闭窗口
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //一下所有函数为控制输入框和单选框的 没有逻辑的实现 不需要理会

        private void Stand_left_Checked(object sender, RoutedEventArgs e)
        {
            stand_right.IsChecked = false;
        }

        private void Stand_right_Checked(object sender, RoutedEventArgs e)
        {
            stand_left.IsChecked = false;
        }

        private void Stand_nosupport_Checked(object sender, RoutedEventArgs e)
        {
            stand_toolsupport.IsChecked = false;
            stand_support.IsChecked = false;
        }

        private void Stand_toolsupport_Checked(object sender, RoutedEventArgs e)
        {
            stand_support.IsChecked = false;
            stand_nosupport.IsChecked = false;
            stand_comBox1.IsEnabled = true;
            stand_comBox2.IsEnabled = true;
        }
        private void c6_Unchecked(object sender, RoutedEventArgs e)
        {
            stand_comBox1.IsEnabled = false;
            stand_comBox2.IsEnabled = false;
        }

        private void Stand_support_Checked(object sender, RoutedEventArgs e)
        {
           
            stand_nosupport.IsChecked = false;
            stand_toolsupport.IsChecked = false;

        }




        private void Grip_left_Checked(object sender, RoutedEventArgs e)
        {
            grit_right.IsChecked = false;
        }

        private void Grit_right_Checked(object sender, RoutedEventArgs e)
        {
            grip_left.IsChecked = false;
        }

        private void Grid_stand_Checked(object sender, RoutedEventArgs e)
        {
            grid_sit.IsChecked = false;
        }

        private void Grid_sit_Checked(object sender, RoutedEventArgs e)
        {
            grid_stand.IsChecked = false;
        }




        private void Protrack_twohands_Checked(object sender, RoutedEventArgs e)
        {
            protrack_left.IsChecked = false;
            protrack_right.IsChecked = false;
        }

        private void Protrack_left_Checked(object sender, RoutedEventArgs e)
        {
            protrack_right.IsChecked = false;
            protrack_twohands.IsChecked = false;
        }

        private void Protrack_right_Checked(object sender, RoutedEventArgs e)
        {
            protrack_left.IsChecked = false;
            protrack_twohands.IsChecked = false;
        }

        private void Protrack_curvature_Checked(object sender, RoutedEventArgs e)
        {
            protrack_sit.IsChecked = false;
        }

        private void Protrack_sit_Checked(object sender, RoutedEventArgs e)
        {
            protrack_curvature.IsChecked = false;
        }













        //改到此处了


        private void c17_Checked(object sender, RoutedEventArgs e)
        {
            measuring_leg_right.IsChecked = false;
        }

        private void c18_Checked(object sender, RoutedEventArgs e)
        {
            measuring_leg_left.IsChecked = false;
        }

        private void c19_Checked(object sender, RoutedEventArgs e)
        {
            knee_flexure_not.IsChecked = false;
        }

        private void c20_Checked(object sender, RoutedEventArgs e)
        {
            knee_flexure_is.IsChecked = false;
        }

        private void c21_Checked(object sender, RoutedEventArgs e)
        {
            time_up_routine.IsChecked = false;
        }

        private void c22_Checked(object sender, RoutedEventArgs e)
        {
            time_up_base.IsChecked = false;
        }

        private void c23_Checked(object sender, RoutedEventArgs e)
        {
            time_up_tools.IsChecked = false;
            time_up_other.IsChecked = false;
        }

        private void c24_Checked(object sender, RoutedEventArgs e)
        {
            time_up_walk.IsChecked = false;
            time_up_other.IsChecked = false;
            time_up_other_selected.IsEnabled = IsEnabled;
        }


        private void c24_Unchecked(object sender, RoutedEventArgs e)
        {
            time_up_other_selected.IsEnabled = false;
        }

        private void c25_Checked(object sender, RoutedEventArgs e)
        {
            time_up_walk.IsChecked = false;
            time_up_tools.IsChecked = false;
            //text1.IsEnabled = IsEnabled;
        }

        //private void c25_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    text1.IsEnabled = UnEnabled;
        //}

        private void c26_Checked(object sender, RoutedEventArgs e)
        {
            walk5_routine.IsChecked = false;

        }

        private void c27_Checked(object sender, RoutedEventArgs e)
        {
            walk5_base.IsChecked = false;
        }

        private void c28_Checked(object sender, RoutedEventArgs e)
        {
            walk5_tools.IsChecked = false;
            walk5_other.IsChecked = false;
        }

        private void c29_Unchecked(object sender, RoutedEventArgs e)
        {
            walk5_tools_selected.IsEnabled = false;
        }

        //private void c30_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    text2.IsEnabled = UnEnabled;
        //}

        private void c29_Checked(object sender, RoutedEventArgs e)
        {
            walk5_walk.IsChecked = false;
            walk5_other.IsChecked = false;
            walk5_tools_selected.IsEnabled = IsEnabled;
        }

        private void c30_Checked(object sender, RoutedEventArgs e)
        {
            walk5_walk.IsChecked = false;
            walk5_tools.IsChecked = false;
            //text2.IsEnabled = IsEnabled;
        }

        private void c31_Checked(object sender, RoutedEventArgs e)
        {
            walk5_fastest_routine.IsChecked = false;
        }

        private void c32_Checked(object sender, RoutedEventArgs e)
        {
            walk5_fastest_base.IsChecked = false;
        }

        private void c33_Checked(object sender, RoutedEventArgs e)
        {
            walk5_fastest_tools.IsChecked = false;
            walk5_fastest_other.IsChecked = false;
        }

        private void c34_Checked(object sender, RoutedEventArgs e)
        {
            walk5_fastest_walk.IsChecked = false;
            walk5_fastest_other.IsChecked = false;

            walk5_fastest_tools_selected.IsEnabled = IsEnabled;
        }

        private void c34_Unchecked(object sender, RoutedEventArgs e)
        {
            walk5_fastest_tools_selected.IsEnabled = UnEnabled;
        }

        private void c35_Checked(object sender, RoutedEventArgs e)
        {
            walk5_fastest_walk.IsChecked = false;
            walk5_fastest_tools.IsChecked = false;


            //text3.IsEnabled = IsEnabled;
        }

        //private void c35_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    text3.IsEnabled = UnEnabled;
        //}

        private void c36_Checked(object sender, RoutedEventArgs e)
        {
            walk10_routine.IsChecked = false;
        }

        private void c37_Checked(object sender, RoutedEventArgs e)
        {
            walk10_base.IsChecked = false;
        }

        private void c38_Checked(object sender, RoutedEventArgs e)
        {
            walk10_tools.IsChecked = false;
            walk10_other.IsChecked = false;

        }

        private void c39_Unchecked(object sender, RoutedEventArgs e)
        {
            walk10_tools_selected.IsEnabled = UnEnabled;
        }


        private void c39_Checked(object sender, RoutedEventArgs e)
        {
            walk10_walk.IsChecked = false;
            walk10_other.IsChecked = false;
            walk10_tools_selected.IsEnabled = IsEnabled;
        }

        private void c40_Checked(object sender, RoutedEventArgs e)
        {
            walk10_walk.IsChecked = false;
            walk10_tools.IsChecked = false;
            //text4.IsEnabled = IsEnabled;
        }

        private void c43_Checked(object sender, RoutedEventArgs e)
        {
            walk6_tools.IsChecked = false;
            walk6_other.IsChecked = false;
        }

        private void c44_Checked(object sender, RoutedEventArgs e)
        {
            walk6_walk.IsChecked = false;
            walk6_other.IsChecked = false;
            walk6_tools_selected.IsEnabled = IsEnabled;
        }

        private void c44_Unchecked(object sender, RoutedEventArgs e)
        {
            walk6_tools_selected.IsEnabled = UnEnabled;
        }

        private void c45_Checked(object sender, RoutedEventArgs e)
        {
            walk6_walk.IsChecked = false;
            walk6_tools.IsChecked = false;
            //text5.IsEnabled = IsEnabled;
        }

        //private void c45_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    text5.IsEnabled = UnEnabled;
        //}

        private void c46_Checked(object sender, RoutedEventArgs e)
        {
            step2_tools.IsChecked = false;
            step2_other.IsChecked = false;
        }

        private void c47_Checked(object sender, RoutedEventArgs e)
        {
            step2_walk.IsChecked = false;
            step2_other.IsChecked = false;
            step2_tools_selected.IsEnabled = IsEnabled;
        }

        private void c47_Unchecked(object sender, RoutedEventArgs e)
        {
            step2_tools_selected.IsEnabled = UnEnabled;
        }

        private void c48_Checked(object sender, RoutedEventArgs e)
        {
            step2_walk.IsChecked = false;
            step2_tools.IsChecked = false;
            //text6.IsEnabled = IsEnabled;
        }

        //private void c48_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    text6.IsEnabled = UnEnabled;
        //}

        private void c49_Checked(object sender, RoutedEventArgs e)
        {
            leg2_sit.IsChecked = false;
        }

        private void c50_Checked(object sender, RoutedEventArgs e)
        {
            leg2_stand.IsChecked = false;
        }
        //错误：OnlyInputNumbers
        //设置手机号输入框只能输入数字
        private void OnlyInputNumbers(object sender, TextCompositionEventArgs e)
        {
            inputlimited.InputLimited.OnlyInputNumbers(e);
        }




    }
}

