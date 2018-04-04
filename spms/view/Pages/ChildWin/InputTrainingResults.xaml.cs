using System;
using System.Collections.Generic;
using System.Linq;
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
using spms.constant;
using spms.dao;
using spms.entity;
using spms.service;
using spms.util;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// InputTrainingResults.xaml 的交互逻辑
    /// </summary>
    public partial class InputTrainingResults : Window
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
            viewbox.MaxHeight = SystemParameters.WorkArea.Size.Height;
            viewbox.MaxWidth = SystemParameters.WorkArea.Size.Width;
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            Load_Data();//载入数据
            Certain_Dev();//确定购买了哪些设备
            HLPAttr1.ItemsSource = Add(0, 700, 2);
            //com_02.ItemsSource = Add(0, 30, 2);
            HLPAttr3.ItemsSource = Add(1, 5, 2);
            HLPAttr4.ItemsSource = Add(1, 9, 2);
            RowAttr1.ItemsSource = Add(0, 400, 2);
            //com_12.ItemsSource = Add(0, 30, 1);
            RowAttr3.ItemsSource = Add(1, 4, 2);
            RowAttr4.ItemsSource = Add(1, 4, 2);
            TFAttr1.ItemsSource = Add(0, 700, 2);
            //com_22.ItemsSource = Add(0, 30, 1);
            TFAttr3.ItemsSource = Add(1, 6, 2);
            TFAttr4.ItemsSource = Add(1, 9, 2);
            LEAttr1.ItemsSource = Add(0, 260, 1);
            //com_32.ItemsSource = Add(0, 30, 1);
            LEAttr3.ItemsSource = Add(1, 5, 2);
            LEAttr4.ItemsSource = Add(1, 5, 2);
            LEAttr5.ItemsSource = Add(1, 9, 2);
            HAAttr1.ItemsSource = Add(0, 500, 2);
            //com_42.ItemsSource = Add(0, 30, 1);
            HAAttr3.ItemsSource = Add(1, 9, 2);
            CPAttr1.ItemsSource = Add(0, 180, 2);
            //com_52.ItemsSource = Add(0, 30, 1);
            CPAttr3.ItemsSource = Add(1, 8, 2);
        }

        private void Certain_Dev()
        {
            var devs = new DeviceSortDAO().ListAll();
            foreach (DeviceSort dev in devs)
            {
                if (dev.DS_Status == 1)
                {
                    continue;
                }
                switch (dev.DS_name)
                {
                    case "胸部推举机":
                    case "Chest Press":
                        tab1.IsEnabled = false;
                        sp1.IsEnabled = false;
                        break;
                    case "坐姿划船机":
                    case "Rowing":
                        tab2.IsEnabled = false;
                        sp2.IsEnabled = false;
                        break;
                    case "身体伸展弯曲机":
                    case "Torso Flexion":
                        tab3.IsEnabled = false;
                        sp3.IsEnabled = false;
                        break;
                    case "腿部伸展弯曲机":
                    case "Leg Extension":
                        tab4.IsEnabled = false;
                        sp4.IsEnabled = false;
                        break;
                    case "腿部推蹬机":
                    case "Horizontal Leg Press":
                        tab5.IsEnabled = false;
                        sp5.IsEnabled = false;
                        break;
                    case "腿部内外弯机":
                    case "Hip Abduction":
                        tab6.IsEnabled = false;
                        sp6.IsEnabled = false;
                        break;
                }
            }
        }
        private List<double> Add(double start, double end, int type)

        {
            List<double> list = new List<double>();
            switch (type)
            {
                case 1:
                    for (double s = start; s <= end; s += 0.5)
                    {
                        list.Add(s);
                    }

                    break;
                case 2:
                    for (double s = start; s <= end; s++)
                    {
                        list.Add(s);
                    }

                    break;

            }

            return list;
        }
        private User user;
        public InputTrainingResults()
        {
            InitializeComponent();
        }
        //取消操作，关闭窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime? da = dp.DateTime;
            //DateTime? da = dp.SelectedDate;//实施日期
            Dictionary<DevicePrescription, PrescriptionResult> prescription = new Dictionary<DevicePrescription, PrescriptionResult>();
            
            TrainInfo trainInfo = new TrainInfo();
            trainInfo.FK_User_Id = user.Pk_User_Id;
            trainInfo.Gmt_Create = da;
            trainInfo.Gmt_Modified = DateTime.Now;
            trainInfo.Status = (int) TrainInfoStatus.Finish;//手动输入的结果状态为已完成
            DeviceSortDAO deviceSortDao = new DeviceSortDAO();
            string devName;
            if (HLPGroupcount.Text != "")
            {
                //胸部推举机
                PrescriptionResult prescriptionResult = new PrescriptionResult();
                DevicePrescription devicePrescription = new DevicePrescription();
                //string attr1 = HLPAttr1.Text;
                //string attr2 = HLPAttr2.Text;
                string attr3 = HLPAttr3.Text;
                string attr4 = HLPAttr4.Text;
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3 + "*" +
                                              attr4;
                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(HLPAttr1.Text);
                }
                catch (Exception exception)
                {
                }

                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(HLPGroupcount.Text);
                }
                catch (Exception exception)
                {
                }

                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(HLPGroupcount.Text);
                }
                catch (Exception exception)
                {
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(HLPGroupnum.Text);
                }
                catch (Exception exception)
                {
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(HLPRelaxTime.Text);
                }
                catch (Exception exception)
                {
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(HLPWeight.Text);
                }
                catch (Exception exception)
                {
                }
                devicePrescription.Dp_status = 1;
                try
                {
                    devicePrescription.dp_moveway = int.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, HLPMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                }
                devicePrescription.Fk_DS_Id = (int) DeviceType.X01;
                if (HLPTimer.Text == "有效" || HLPTimer.Text == "Valid")
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount = Byte.Parse(HLPTime.Text);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        return;
                    }
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (HLPTiming.Text == "倒计时" || HLPTiming.Text == "Count Reverse")
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }

                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                try
                {
                    prescriptionResult.PR_SportStrength = byte.Parse(HLPSportstrength.Text);
                }
                catch (Exception exception)
                {
                }

                try
                {
                    prescriptionResult.PR_Time1 = Convert.ToDouble(HLPTime1.Text);
                }
                catch (Exception exception)
                {
                }

                try
                {
                    prescriptionResult.PR_Time2 = Convert.ToDouble(HLPTime2.Text);
                }
                catch (Exception exception)
                {
                }

                try
                {
                    prescriptionResult.PR_Distance = Convert.ToInt32(HLPDistance.Text);
                }
                catch (Exception exception)
                {
                }

                try
                {
                    prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(HLPCountworkqu.Text);
                }
                catch (Exception exception)
                {
                }

                try
                {
                    prescriptionResult.PR_Cal = Convert.ToDouble(HLPCal.Text);
                }
                catch (Exception exception)
                {
                }

                try
                {
                    prescriptionResult.PR_Index = Convert.ToDouble(HLPIndex.Text);
                }
                catch (Exception exception)
                {
                }

                //try
                //{
                //    prescriptionResult.PR_FinishGroup = Convert.ToInt32(HLPFinishgroup.Text);
                //}
                //catch (Exception exception)
                //{
                //}

                try
                {
                    prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, HLPEvaluate.Text));
                }
                catch (Exception exception)
                {
                }
                prescriptionResult.PR_AttentionPoint = HLPAttentionpoint.Text;
                prescriptionResult.PR_UserThoughts = HLPUserthoughts.Text;
                prescriptionResult.PR_Memo = HLPMemo.Text;
                
                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (ROWGroupcount.Text != "")
            {
                //坐姿划船机
                PrescriptionResult prescriptionResult = new PrescriptionResult();
                DevicePrescription devicePrescription = new DevicePrescription();
                //string attr1 = RowAttr1.Text;
                //string attr2 = RowAttr1.Text;
                string attr3 = RowAttr1.Text;
                string attr4 = RowAttr1.Text;
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3 + "*" +
                                              attr4;
                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(RowAttr1.Text);
                }
                catch (Exception exception)
                {
                }
                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(ROWGroupcount.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(ROWGroupnum.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(ROWRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(ROWWeight.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }
                devicePrescription.Dp_status = 1;
                try
                {
                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, ROWMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    
                    
                }
                devicePrescription.Fk_DS_Id = (int)DeviceType.X05;
                if (ROWTimer.Text == "有效" || ROWTimer.Text == "Valid")
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount = Byte.Parse(ROWTime.Text);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        return;
                    }
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (ROWTiming.Text == "倒计时" || ROWTiming.Text == "Count Reverse")
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }

                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                try
                {
                    prescriptionResult.PR_SportStrength = byte.Parse(ROWSportstrength.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Time1 = Convert.ToDouble(ROWTime1.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Time2 = Convert.ToDouble(ROWTime2.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Distance = Convert.ToInt32(ROWDistance.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(ROWCountworkqu.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Cal = Convert.ToDouble(ROWCal.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Index = Convert.ToDouble(ROWIndex.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                //try
                //{
                //    prescriptionResult.PR_FinishGroup = Convert.ToInt32(ROWFinishgroup.Text);
                //}
                //catch (Exception exception)
                //{
                    
                    
                //}

                try
                {
                    prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, ROWEvaluate.Text));
                }
                catch (Exception exception)
                {
                    
                    
                }
                prescriptionResult.PR_AttentionPoint = ROWAttentionpoint.Text;
                prescriptionResult.PR_UserThoughts = ROWUserthoughts.Text;
                prescriptionResult.PR_Memo = ROWMemo.Text;

                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (TFGroupcount.Text != "")
            {
                //身体伸展弯曲机
                PrescriptionResult prescriptionResult = new PrescriptionResult();
                DevicePrescription devicePrescription = new DevicePrescription();
                //string attr1 = TFAttr1.Text;
                //string attr2 = TFAttr2.Text;
                string attr3 = TFAttr3.Text;
                string attr4 = TFAttr4.Text;
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3 + "*" +
                                              attr4;
                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(TFAttr1.Text);
                }
                catch (Exception exception)
                {
                }
                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(TFGroupcount.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(TFGroupnum.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(TFRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(TFWeight.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }
                devicePrescription.Dp_status = 1;
                try
                {
                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, TFMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    
                    
                }
                devicePrescription.Fk_DS_Id = (int)DeviceType.X04;
                if (TFTimer.Text == "有效" || TFTimer.Text == "Valid")
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount = Byte.Parse(TFTime.Text);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        return;
                    }
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (TFTiming.Text == "倒计时" || TFTiming.Text == "Count Reverse")
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }

                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                try
                {
                    prescriptionResult.PR_SportStrength = byte.Parse(TFSportstrength.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Time1 = Convert.ToDouble(TFTime1.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Time2 = Convert.ToDouble(TFTime2.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Distance = Convert.ToInt32(TFDistance.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(TFCountworkqu.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Cal = Convert.ToDouble(TFCal.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Index = Convert.ToDouble(TFIndex.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                //try
                //{
                //    prescriptionResult.PR_FinishGroup = Convert.ToInt32(TFFinishgroup.Text);
                //}
                //catch (Exception exception)
                //{
                    
                    
                //}

                try
                {
                    prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, TFEvaluate.Text));
                }
                catch (Exception exception)
                {
                    
                    
                }
                prescriptionResult.PR_AttentionPoint = TFAttentionpoint.Text;
                prescriptionResult.PR_UserThoughts = TFUserthoughts.Text;
                prescriptionResult.PR_Memo = TFMemo.Text;

                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (LEGroupcount.Text != "")
            {
                //腿部伸展弯曲机
                PrescriptionResult prescriptionResult = new PrescriptionResult();
                DevicePrescription devicePrescription = new DevicePrescription();
                //string attr1 = LEAttr1.Text;
                //string attr2 = LEAttr2.Text;
                string attr3 = LEAttr3.Text;
                string attr4 = LEAttr4.Text;
                string attr5 = LEAttr5.Text;
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3 + "*" +
                                              attr4 + "*" +
                                              attr5;
                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(LEAttr1.Text);
                }
                catch (Exception exception)
                {
                }
                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(LEGroupcount.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(LEGroupnum.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(LERelaxTime.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(LEWeight.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }
                devicePrescription.Dp_status = 1;
                try
                {
                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, LEMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    
                    
                }
                devicePrescription.Fk_DS_Id = (int)DeviceType.X03;
                if (LETimer.Text == "有效" || LETimer.Text == "Valid")
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount = Byte.Parse(LETime.Text);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        return;
                    }
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (LETiming.Text == "倒计时" || LETiming.Text == "Count Reverse")
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }

                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                try
                {
                    prescriptionResult.PR_SportStrength = byte.Parse(LESportstrength.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Time1 = Convert.ToDouble(LETime1.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Time2 = Convert.ToDouble(LETime2.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Distance = Convert.ToInt32(LEDistance.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(LECountworkqu.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Cal = Convert.ToDouble(LECal.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Index = Convert.ToDouble(LEIndex.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                //try
                //{
                //    prescriptionResult.PR_FinishGroup = Convert.ToInt32(LEFinishgroup.Text);
                //}
                //catch (Exception exception)
                //{
                    
                    
                //}

                try
                {
                    prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, LEEvaluate.Text));
                }
                catch (Exception exception)
                {
                    
                    
                }
                prescriptionResult.PR_AttentionPoint = LEAttentionpoint.Text;
                prescriptionResult.PR_UserThoughts = LEUserthoughts.Text;
                prescriptionResult.PR_Memo = LEMemo.Text;

                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (HAGroupcount.Text != "")
            {
                //腿部腿蹬机
                PrescriptionResult prescriptionResult = new PrescriptionResult();
                DevicePrescription devicePrescription = new DevicePrescription();
                //string attr1 = HAAttr1.Text;
                //string attr2 = HAAttr2.Text;
                string attr3 = HAAttr3.Text;
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3;
                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(HAAttr1.Text);
                }
                catch (Exception exception)
                {
                }
                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(HAGroupcount.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(HAGroupnum.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(HARelaxTime.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(HAWeight.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }
                devicePrescription.Dp_status = 1;
                try
                {
                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, HAMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    
                    
                }
                devicePrescription.Fk_DS_Id = (int)DeviceType.X06;
                if (HATimer.Text == "有效" || HATimer.Text == "Valid")
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount = Byte.Parse(HATime.Text);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        return;
                    }
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (HATiming.Text == "倒计时" || HATiming.Text == "Count Reverse")
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }


                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                try
                {
                    prescriptionResult.PR_SportStrength = byte.Parse(HASportstrength.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Time1 = Convert.ToDouble(HATime1.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Time2 = Convert.ToDouble(HATime2.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Distance = Convert.ToInt32(HADistance.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(HACountworkqu.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Cal = Convert.ToDouble(HACal.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Index = Convert.ToDouble(HAIndex.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                //try
                //{
                //    prescriptionResult.PR_FinishGroup = Convert.ToInt32(HAFinishgroup.Text);
                //}
                //catch (Exception exception)
                //{
                    
                    
                //}

                try
                {
                    prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, HAEvaluate.Text));
                }
                catch (Exception exception)
                {
                    
                    
                }
                prescriptionResult.PR_AttentionPoint = HAAttentionpoint.Text;
                prescriptionResult.PR_UserThoughts = HAUserthoughts.Text;
                prescriptionResult.PR_Memo = HAMemo.Text;

                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (CPGroupcount.Text != "")
            {
                //腿部内外弯机
                PrescriptionResult prescriptionResult = new PrescriptionResult();
                DevicePrescription devicePrescription = new DevicePrescription();
                //string attr1 = CPAttr1.Text;
                //string attr2 = CPAttr2.Text;
                string attr3 = CPAttr3.Text;
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3;
                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(CPAttr1.Text);
                }
                catch (Exception exception)
                {
                }
                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(CPGroupcount.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(CPGroupnum.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(CPRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(CPWeight.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }
                devicePrescription.Dp_status = 1;
                try
                {
                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, CPMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    
                    
                }
                devicePrescription.Fk_DS_Id = (int)DeviceType.X02;
                if (CPTimer.Text == "有效" || CPTimer.Text == "Valid")
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount = Byte.Parse(CPTime.Text);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        return;
                    }
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (CPTiming.Text == "倒计时" || CPTiming.Text == "Count Reverse")
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }

                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                try
                {
                    prescriptionResult.PR_SportStrength = byte.Parse(CPSportstrength.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Time1 = Convert.ToDouble(CPTime1.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Time2 = Convert.ToDouble(CPTime2.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Distance = Convert.ToInt32(CPDistance.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(CPCountworkqu.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Cal = Convert.ToDouble(CPCal.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                try
                {
                    prescriptionResult.PR_Index = Convert.ToDouble(CPIndex.Text);
                }
                catch (Exception exception)
                {
                    
                    
                }

                //try
                //{
                //    prescriptionResult.PR_FinishGroup = Convert.ToInt32(CPFinishgroup.Text);
                //}
                //catch (Exception exception)
                //{
                    
                    
                //}

                try
                {
                    prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, CPEvaluate.Text));
                }
                catch (Exception exception)
                {
                    
                    
                }
                prescriptionResult.PR_AttentionPoint = CPAttentionpoint.Text;
                prescriptionResult.PR_UserThoughts = CPUserthoughts.Text;
                prescriptionResult.PR_Memo = CPMemo.Text;

                prescription.Add(devicePrescription, prescriptionResult);
            }

            if (prescription.Count == 0)
            {
                MessageBox.Show(LanguageUtils.ConvertLanguage("没有输入训练结果", "No input of training results"));
                return;
            }
            //插入训练结果
            new TrainService().AddPrescriptionResult(trainInfo, prescription);
            //打印
            MessageBox.Show(LanguageUtils.ConvertLanguage("已存储", "Finished storage"));
            this.Close();
        }

        private void Load_Data()
        {
            user = (User) DataContext;

            l1.Content = user.User_Name;
            l2.Content = user.Pk_User_Id;

            List<DataCode> dataCodesEvaluate = DataCodeCache.GetInstance().GetDateCodeList(DataCodeTypeEnum.Evaluate);
            List<string> dataItemsEvaluate = new List<string>();
            if (dataCodesEvaluate != null)
            {
                foreach (var dataCode in dataCodesEvaluate)
                {
                    dataItemsEvaluate.Add(dataCode.Code_D_Value);
                }
            }
            List<DataCode> dataCodesMoveWay = DataCodeCache.GetInstance().GetDateCodeList(DataCodeTypeEnum.MoveWay);
            List<string> dataItemsMoveWay = new List<string>();
            if (dataCodesMoveWay != null)
            {
                foreach (var dataCode in dataCodesMoveWay)
                {
                    dataItemsMoveWay.Add(dataCode.Code_D_Value);
                }
            }

            HLPMoveway.ItemsSource = dataItemsMoveWay;
            HLPEvaluate.ItemsSource = dataItemsEvaluate;

            ROWMoveway.ItemsSource = dataItemsMoveWay;
            ROWEvaluate.ItemsSource = dataItemsEvaluate;

            TFMoveway.ItemsSource = dataItemsMoveWay;
            TFEvaluate.ItemsSource = dataItemsEvaluate;

            LEMoveway.ItemsSource = dataItemsMoveWay;
            LEEvaluate.ItemsSource = dataItemsEvaluate;

            HAMoveway.ItemsSource = dataItemsMoveWay;
            HAEvaluate.ItemsSource = dataItemsEvaluate;

            CPMoveway.ItemsSource = dataItemsMoveWay;
            CPEvaluate.ItemsSource = dataItemsEvaluate;
        }
        
        //输入正数
        private void OnlyInputNumbers(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");

            e.Handled = re.IsMatch(e.Text);
        }

        //输入小数
        private void OnlyInputDouble(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.]+");

            e.Handled = re.IsMatch(e.Text);
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonBase_OnClick(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }
        private void HLPselect_change(object sender, EventArgs e)
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
        private void ROWselect_change(object sender, EventArgs e)
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
        private void TFselect_change(object sender, EventArgs e)
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
        private void LEselect_change(object sender, EventArgs e)
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
        private void HAselect_change(object sender, EventArgs e)
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
        private void CPselect_change(object sender, EventArgs e)
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
    }
}
