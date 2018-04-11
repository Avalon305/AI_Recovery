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
using NLog;
using spms.constant;
using spms.dao;
using spms.entity;
using spms.service;
using spms.util;
using spms.view.dto;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// ViewTrainingResults.xaml 的交互逻辑
    /// </summary>
    public partial class ViewTrainingResults : Window
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private User user;
        private TrainDTO trainDto;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewbox.MaxHeight = SystemParameters.WorkArea.Size.Height;
            viewbox.MaxWidth = SystemParameters.WorkArea.Size.Width;
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            Dictionary<string, object> dic = (Dictionary<string, Object>) DataContext;
            user = (User) dic["user"];
            trainDto = (TrainDTO) dic["trainDto"];
            logger.Info("user:" + user + "; trainDto:" + trainDto);
            //绑定数据
            Load_Data();
            certain_dev();
        }

        private void certain_dev()
        {
            var deviceSorts = new DeviceSortDAO().ListAll();
            foreach(DeviceSort dev in deviceSorts)
            {
                switch(dev.DS_name)
                {
                    case "":
                        break;
                }
            }
        }

        private void Load_Data()
        {
            //用户信息
            l1.Content = user.User_Name;
            l2.Content = user.Pk_User_Id;
            //训练日期
            da.Content = trainDto.prescriptionResult.Gmt_Create.ToString();

            //查询处方和结果
            List<TrainDTO> trainDtos = new TrainService().GetTrainDTOByPRId(trainDto.prescriptionResult.Pk_PR_Id);
            DeviceSortDAO deviceSortDao = new DeviceSortDAO();
            //循环判断填充数据
            foreach (TrainDTO trainDto in trainDtos)
            {
                string[] attrs = trainDto.devicePrescription.DP_Attrs.Split(new char[]{'*'});
                switch (trainDto.devicePrescription.Fk_DS_Id)
                {
                    case (int)DeviceType.X01:
                        HLPGroupcount.Text = trainDto.devicePrescription.dp_groupcount.ToString();
                        HLPGroupnum.Text = trainDto.devicePrescription.dp_groupnum.ToString();
                        HLPRelaxTime.Text = trainDto.devicePrescription.dp_relaxtime.ToString();
                        HLPWeight.Text = trainDto.devicePrescription.dp_weight.ToString();
                        HLPMoveway.Text = trainDto.moveway;
                        HLPAttr1.Text = trainDto.devicePrescription.dp_movedistance.ToString();
                        //HLPAttr2.Text = attrs[1];
                        HLPAttr3.Text = attrs[0];
                        HLPAttr4.Text = attrs[1];
                        if (trainDto.devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            HLPTimer.Text = LanguageUtils.ConvertLanguage("有效", "Valid");
                            HLPselect_change();
                            HLPTime.Text = trainDto.devicePrescription.dp_timecount.ToString();
                            if (trainDto.devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                HLPTiming.Text = LanguageUtils.ConvertLanguage("正计时", "Count Forward");
                            }
                            else
                            {
                                HLPTiming.Text = LanguageUtils.ConvertLanguage("倒计时", "Count Reverse");
                            }
                        }
                        else
                        {
                            HLPTimer.Text = LanguageUtils.ConvertLanguage("无效", "Invalid");
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        HLPSportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        HLPTime1.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time1).ToLongTimeString();
                        HLPDistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        HLPCountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        HLPCal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        HLPIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        HLPTime2.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time2).ToLongTimeString();
                        //HLPFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
                        HLPEvaluate.Text = trainDto.evaluate;
                        HLPAttentionpoint.Text = trainDto.prescriptionResult.PR_AttentionPoint;
                        HLPUserthoughts.Text = trainDto.prescriptionResult.PR_UserThoughts;
                        HLPMemo.Text = trainDto.prescriptionResult.PR_Memo;
                        break;
                    case (int)DeviceType.X05:
                        ROWGroupcount.Text = trainDto.devicePrescription.dp_groupcount.ToString();
                        ROWGroupnum.Text = trainDto.devicePrescription.dp_groupnum.ToString();
                        ROWRelaxTime.Text = trainDto.devicePrescription.dp_relaxtime.ToString();
                        ROWWeight.Text = trainDto.devicePrescription.dp_weight.ToString();
                        ROWMoveway.Text = trainDto.moveway;
                        ROWAttr1.Text = trainDto.devicePrescription.dp_movedistance.ToString();
                        //ROWAttr2.Text = attrs[1];
                        ROWAttr3.Text = attrs[0];
                        ROWAttr4.Text = attrs[1];
                        if (trainDto.devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            ROWTimer.Text = LanguageUtils.ConvertLanguage("有效", "Valid");
                            ROWselect_change();
                            ROWTime.Text = trainDto.devicePrescription.dp_timecount.ToString();
                            if (trainDto.devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                ROWTiming.Text = LanguageUtils.ConvertLanguage("正计时", "Count Forward");
                            }
                            else
                            {
                                ROWTiming.Text = LanguageUtils.ConvertLanguage("倒计时", "Count Reverse");
                            }
                        }
                        else
                        {
                            ROWTimer.Text = LanguageUtils.ConvertLanguage("无效", "Invalid");
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        ROWSportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        ROWTime1.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time1).ToLongTimeString();
                        ROWDistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        ROWCountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        ROWCal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        ROWIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        ROWTime2.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time2).ToLongTimeString();
                        //ROWFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
                        ROWEvaluate.Text = trainDto.evaluate;
                        ROWAttentionpoint.Text = trainDto.prescriptionResult.PR_AttentionPoint;
                        ROWUserthoughts.Text = trainDto.prescriptionResult.PR_UserThoughts;
                        ROWMemo.Text = trainDto.prescriptionResult.PR_Memo;
                        
                        break;
                    case (int)DeviceType.X04:
                        TFGroupcount.Text = trainDto.devicePrescription.dp_groupcount.ToString();
                        TFGroupnum.Text = trainDto.devicePrescription.dp_groupnum.ToString();
                        TFRelaxTime.Text = trainDto.devicePrescription.dp_relaxtime.ToString();
                        TFWeight.Text = trainDto.devicePrescription.dp_weight.ToString();
                        TFMoveway.Text = trainDto.moveway;
                        TFAttr1.Text = trainDto.devicePrescription.dp_movedistance.ToString();
                        //TFAttr2.Text = attrs[1];
                        TFAttr3.Text = attrs[0];
                        TFAttr4.Text = attrs[1];
                        if (trainDto.devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            TFTimer.Text = LanguageUtils.ConvertLanguage("有效", "Valid");
                            TFselect_change();
                            TFTime.Text = trainDto.devicePrescription.dp_timecount.ToString();
                            if (trainDto.devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                TFTiming.Text = LanguageUtils.ConvertLanguage("正计时", "Count Forward");
                            }
                            else
                            {
                                TFTiming.Text = LanguageUtils.ConvertLanguage("倒计时", "Count Reverse");
                            }
                        }
                        else
                        {
                            TFTimer.Text = LanguageUtils.ConvertLanguage("无效", "Invalid");
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        TFSportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        TFTime1.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time1).ToLongTimeString();
                        TFDistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        TFCountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        TFCal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        TFIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        TFTime2.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time2).ToLongTimeString();
                        //TFFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
                        TFEvaluate.Text = trainDto.evaluate;
                        TFAttentionpoint.Text = trainDto.prescriptionResult.PR_AttentionPoint;
                        TFUserthoughts.Text = trainDto.prescriptionResult.PR_UserThoughts;
                        TFMemo.Text = trainDto.prescriptionResult.PR_Memo;
                        
                        break;
                    case (int)DeviceType.X03:
                        LEGroupcount.Text = trainDto.devicePrescription.dp_groupcount.ToString();
                        LEGroupnum.Text = trainDto.devicePrescription.dp_groupnum.ToString();
                        LERelaxTime.Text = trainDto.devicePrescription.dp_relaxtime.ToString();
                        LEWeight.Text = trainDto.devicePrescription.dp_weight.ToString();
                        LEMoveway.Text = trainDto.moveway;
                        LEAttr1.Text = trainDto.devicePrescription.dp_movedistance.ToString();
                        //LEAttr2.Text = attrs[1];
                        LEAttr3.Text = attrs[0];
                        LEAttr4.Text = attrs[1];
                        LEAttr5.Text = attrs[2];
                        if (trainDto.devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            LETimer.Text = LanguageUtils.ConvertLanguage("有效", "Valid");
                            LEselect_change();
                            LETime.Text = trainDto.devicePrescription.dp_timecount.ToString();
                            if (trainDto.devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                LETiming.Text = LanguageUtils.ConvertLanguage("正计时", "Count Forward");
                            }
                            else
                            {
                                LETiming.Text = LanguageUtils.ConvertLanguage("倒计时", "Count Reverse");
                            }
                        }
                        else
                        {
                            LETimer.Text = LanguageUtils.ConvertLanguage("无效", "Invalid");
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        LESportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        LETime1.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time1).ToLongTimeString();
                        LEDistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        LECountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        LECal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        LEIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        LETime2.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time2).ToLongTimeString();
                        //LEFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
                        LEEvaluate.Text = trainDto.evaluate;
                        LEAttentionpoint.Text = trainDto.prescriptionResult.PR_AttentionPoint;
                        LEUserthoughts.Text = trainDto.prescriptionResult.PR_UserThoughts;
                        LEMemo.Text = trainDto.prescriptionResult.PR_Memo;
                        
                        break;
                    case (int)DeviceType.X06:
                        HAGroupcount.Text = trainDto.devicePrescription.dp_groupcount.ToString();
                        HAGroupnum.Text = trainDto.devicePrescription.dp_groupnum.ToString();
                        HARelaxTime.Text = trainDto.devicePrescription.dp_relaxtime.ToString();
                        HAWeight.Text = trainDto.devicePrescription.dp_weight.ToString();
                        HAMoveway.Text = trainDto.moveway;
                        HAAttr1.Text = trainDto.devicePrescription.dp_movedistance.ToString();
                        //HAAttr2.Text = attrs[1];
                        HAAttr3.Text = attrs[0];
                        if (trainDto.devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            HATimer.Text = LanguageUtils.ConvertLanguage("有效", "Valid");
                            HAselect_change();
                            HATime.Text = trainDto.devicePrescription.dp_timecount.ToString();
                            if (trainDto.devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                HATiming.Text = LanguageUtils.ConvertLanguage("正计时", "Count Forward");
                            }
                            else
                            {
                                HATiming.Text = LanguageUtils.ConvertLanguage("倒计时", "Count Reverse");
                            }
                        }
                        else
                        {
                            HATimer.Text = LanguageUtils.ConvertLanguage("无效", "Invalid");
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        HASportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        HATime1.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time1).ToLongTimeString();
                        HADistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        HACountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        HACal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        HAIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        HATime2.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time2).ToLongTimeString();
                        //HAFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
                        HAEvaluate.Text = trainDto.evaluate;
                        HAAttentionpoint.Text = trainDto.prescriptionResult.PR_AttentionPoint;
                        HAUserthoughts.Text = trainDto.prescriptionResult.PR_UserThoughts;
                        HAMemo.Text = trainDto.prescriptionResult.PR_Memo;
                        
                        break;
                    case (int)DeviceType.X02:
                        CPGroupcount.Text = trainDto.devicePrescription.dp_groupcount.ToString();
                        CPGroupnum.Text = trainDto.devicePrescription.dp_groupnum.ToString();
                        CPRelaxTime.Text = trainDto.devicePrescription.dp_relaxtime.ToString();
                        CPWeight.Text = trainDto.devicePrescription.dp_weight.ToString();
                        CPMoveway.Text = trainDto.moveway;
                        CPAttr1.Text = trainDto.devicePrescription.dp_movedistance.ToString();
                        //CPAttr2.Text = attrs[1];
                        CPAttr3.Text = attrs[0];
                        if (trainDto.devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            CPTimer.Text = LanguageUtils.ConvertLanguage("有效", "Valid");
                            CPselect_change();
                            CPTime.Text = trainDto.devicePrescription.dp_timecount.ToString();
                            if (trainDto.devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                CPTiming.Text = LanguageUtils.ConvertLanguage("正计时", "Count Forward");
                            }
                            else
                            {
                                CPTiming.Text = LanguageUtils.ConvertLanguage("倒计时", "Count Reverse");
                            }
                        }
                        else
                        {
                            CPTimer.Text = LanguageUtils.ConvertLanguage("无效", "Invalid");
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        CPSportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        CPTime1.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time1).ToLongTimeString();
                        CPDistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        CPCountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        CPCal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        CPIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        CPTime2.Text = this.TimeConverter(trainDto.prescriptionResult.PR_Time2).ToLongTimeString();
                        //CPFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
                        CPEvaluate.Text = trainDto.evaluate;
                        CPAttentionpoint.Text = trainDto.prescriptionResult.PR_AttentionPoint;
                        CPUserthoughts.Text = trainDto.prescriptionResult.PR_UserThoughts;
                        CPMemo.Text = trainDto.prescriptionResult.PR_Memo;
                        
                        break;
                }
            }
        }

        public ViewTrainingResults()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;
        }

        //取消操作，关闭窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Cancel(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }
        private void HLPselect_change()
        {

            if (HLPTimer.Text.Equals("无效") || HLPTimer.Text.Equals("Invalid"))
            {
                HLPTime_Label.Background = Brushes.White;
                HLPTiming_Label.Background = Brushes.White;
                HLPTiming.Visibility = Visibility.Hidden;

                HLPTime.Visibility = Visibility.Hidden;

            }
            else if (HLPTimer.Text.Equals("有效") || HLPTimer.Text.Equals("Valid"))
            {
                HLPTime_Label.Background = Brushes.Gray;
                HLPTiming_Label.Background = Brushes.Gray;
                HLPTiming.Visibility = Visibility.Visible;

                HLPTime.Visibility = Visibility.Visible;
            }
        }
        private void ROWselect_change()
        {

            if (ROWTimer.Text.Equals("无效") || ROWTimer.Text.Equals("Invalid"))
            {
                ROWTime_Label.Background = Brushes.White;
                ROWTiming_Label.Background = Brushes.White;
                ROWTiming.Visibility = Visibility.Hidden;

                ROWTime.Visibility = Visibility.Hidden;

            }
            else if (ROWTimer.Text.Equals("有效") || ROWTimer.Text.Equals("Valid"))
            {
                ROWTime_Label.Background = Brushes.Gray;
                ROWTiming_Label.Background = Brushes.Gray;
                ROWTiming.Visibility = Visibility.Visible;

                ROWTime.Visibility = Visibility.Visible;
            }
        }
        private void TFselect_change()
        {

            if (TFTimer.Text.Equals("无效") || TFTimer.Text.Equals("Invalid"))
            {
                TFTime_Label.Background = Brushes.White;
                TFTiming_Label.Background = Brushes.White;
                TFTiming.Visibility = Visibility.Hidden;

                TFTime.Visibility = Visibility.Hidden;

            }
            else if (TFTimer.Text.Equals("有效") || TFTimer.Text.Equals("Valid"))
            {
                TFTime_Label.Background = Brushes.Gray;
                TFTiming_Label.Background = Brushes.Gray;
                TFTiming.Visibility = Visibility.Visible;

                TFTime.Visibility = Visibility.Visible;
            }
        }
        private void LEselect_change()
        {

            if (LETimer.Text.Equals("无效") || LETimer.Text.Equals("Invalid"))
            {
                LETime_Label.Background = Brushes.White;
                LETiming_Label.Background = Brushes.White;
                LETiming.Visibility = Visibility.Hidden;

                LETime.Visibility = Visibility.Hidden;

            }
            else if (LETimer.Text.Equals("有效") || LETimer.Text.Equals("Valid"))
            {
                LETime_Label.Background = Brushes.Gray;
                LETiming_Label.Background = Brushes.Gray;
                LETiming.Visibility = Visibility.Visible;

                LETime.Visibility = Visibility.Visible;
            }
        }
        private void HAselect_change()
        {

            if (HATimer.Text.Equals("无效") || HATimer.Text.Equals("Invalid"))
            {
                HATime_Label.Background = Brushes.White;
                HATiming_Label.Background = Brushes.White;
                HATiming.Visibility = Visibility.Hidden;

                HATime.Visibility = Visibility.Hidden;

            }
            else if (HATimer.Text.Equals("有效") || HATimer.Text.Equals("Valid"))
            {
                HATime_Label.Background = Brushes.Gray;
                HATiming_Label.Background = Brushes.Gray;
                HATiming.Visibility = Visibility.Visible;

                HATime.Visibility = Visibility.Visible;
            }
        }
        private void CPselect_change()
        {

            if (CPTimer.Text.Equals("无效") || CPTimer.Text.Equals("Invalid"))
            {
                CPTime_Label.Background = Brushes.White;
                CPTiming_Label.Background = Brushes.White;
                CPTiming.Visibility = Visibility.Hidden;

                CPTime.Visibility = Visibility.Hidden;

            }
            else if (CPTimer.Text.Equals("有效") || CPTimer.Text.Equals("Valid"))
            {
                CPTime_Label.Background = Brushes.Gray;
                CPTiming_Label.Background = Brushes.Gray;
                CPTiming.Visibility = Visibility.Visible;

                CPTime.Visibility = Visibility.Visible;
            }
        }
        private DateTime TimeConverter(double dateTime)
        {
            int hours = (int)(dateTime / 3600.0);
            int minute = (int)(dateTime/60.0 - hours * 60.0);
            int second = (int)(dateTime / 1 - minute*60 - hours*3600);
            DateTime time = new DateTime(0001,1,1,hours,minute,second);
            //time.AddHours(hours);
            //time.AddMinutes(minute);
           // time.AddSeconds(second);
            return time;
        }
    }
}