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
            DateTime? da = dp.SelectedDate;//实施日期
            Dictionary<DevicePrescription, PrescriptionResult> prescription = new Dictionary<DevicePrescription, PrescriptionResult>();
            
            TrainInfo trainInfo = new TrainInfo();
            trainInfo.FK_User_Id = user.Pk_User_Id;
            trainInfo.Gmt_Create = da;
            trainInfo.Gmt_Modified = DateTime.Now;

            if (HLPGroupcount.Text != "")
            {
                //水平腿部推蹬机
                PrescriptionResult prescriptionResult = new PrescriptionResult();
                DevicePrescription devicePrescription = new DevicePrescription();

                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(HLPGroupcount.Text);
                devicePrescription.dp_groupnum = Convert.ToInt32(HLPGroupnum.Text);
                devicePrescription.dp_relaxtime = Convert.ToInt32(HLPRelaxTime.Text);
                devicePrescription.dp_weight = Convert.ToDouble(HLPWeight.Text);
                devicePrescription.Dp_status = 1;
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, HLPMoveway.Text)); //移乘方式
                //TODO 设备属性、类型没有获取

                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                prescriptionResult.PR_SportStrength = byte.Parse(HLPSportstrength.Text);
                prescriptionResult.PR_Time1 = Convert.ToDouble(HLPTime1.Text);
                prescriptionResult.PR_Time2 = Convert.ToDouble(HLPTime2.Text);
                prescriptionResult.PR_Distance = Convert.ToInt32(HLPDistance.Text);
                prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(HLPCountworkqu.Text);
                prescriptionResult.PR_Cal = Convert.ToDouble(HLPCal.Text);
                prescriptionResult.PR_Index = Convert.ToDouble(HLPIndex.Text);
                prescriptionResult.PR_FinishGroup = Convert.ToInt32(HLPFinishgroup.Text);
                prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, HLPEvaluate.Text));
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

                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(ROWGroupcount.Text);
                devicePrescription.dp_groupnum = Convert.ToInt32(ROWGroupnum.Text);
                devicePrescription.dp_relaxtime = Convert.ToInt32(ROWRelaxTime.Text);
                devicePrescription.dp_weight = Convert.ToDouble(ROWWeight.Text);
                devicePrescription.Dp_status = 1;
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, ROWMoveway.Text)); //移乘方式
                //TODO 设备属性、类型没有获取

                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                prescriptionResult.PR_SportStrength = byte.Parse(ROWSportstrength.Text);
                prescriptionResult.PR_Time1 = Convert.ToDouble(ROWTime1.Text);
                prescriptionResult.PR_Time2 = Convert.ToDouble(ROWTime2.Text);
                prescriptionResult.PR_Distance = Convert.ToInt32(ROWDistance.Text);
                prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(ROWCountworkqu.Text);
                prescriptionResult.PR_Cal = Convert.ToDouble(ROWCal.Text);
                prescriptionResult.PR_Index = Convert.ToDouble(ROWIndex.Text);
                prescriptionResult.PR_FinishGroup = Convert.ToInt32(ROWFinishgroup.Text);
                prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, HLPEvaluate.Text));
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

                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(TFGroupcount.Text);
                devicePrescription.dp_groupnum = Convert.ToInt32(TFGroupnum.Text);
                devicePrescription.dp_relaxtime = Convert.ToInt32(TFRelaxTime.Text);
                devicePrescription.dp_weight = Convert.ToDouble(TFWeight.Text);
                devicePrescription.Dp_status = 1;
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, TFMoveway.Text)); //移乘方式
                //TODO 设备属性、类型没有获取

                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                prescriptionResult.PR_SportStrength = byte.Parse(TFSportstrength.Text);
                prescriptionResult.PR_Time1 = Convert.ToDouble(TFTime1.Text);
                prescriptionResult.PR_Time2 = Convert.ToDouble(TFTime2.Text);
                prescriptionResult.PR_Distance = Convert.ToInt32(TFDistance.Text);
                prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(TFCountworkqu.Text);
                prescriptionResult.PR_Cal = Convert.ToDouble(TFCal.Text);
                prescriptionResult.PR_Index = Convert.ToDouble(TFIndex.Text);
                prescriptionResult.PR_FinishGroup = Convert.ToInt32(TFFinishgroup.Text);
                prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, HLPEvaluate.Text));
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

                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(LEGroupcount.Text);
                devicePrescription.dp_groupnum = Convert.ToInt32(LEGroupnum.Text);
                devicePrescription.dp_relaxtime = Convert.ToInt32(LERelaxTime.Text);
                devicePrescription.dp_weight = Convert.ToDouble(LEWeight.Text);
                devicePrescription.Dp_status = 1;
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, LEMoveway.Text)); //移乘方式
                //TODO 设备属性、类型没有获取

                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                prescriptionResult.PR_SportStrength = byte.Parse(LESportstrength.Text);
                prescriptionResult.PR_Time1 = Convert.ToDouble(LETime1.Text);
                prescriptionResult.PR_Time2 = Convert.ToDouble(LETime2.Text);
                prescriptionResult.PR_Distance = Convert.ToInt32(LEDistance.Text);
                prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(LECountworkqu.Text);
                prescriptionResult.PR_Cal = Convert.ToDouble(LECal.Text);
                prescriptionResult.PR_Index = Convert.ToDouble(LEIndex.Text);
                prescriptionResult.PR_FinishGroup = Convert.ToInt32(LEFinishgroup.Text);
                prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, HLPEvaluate.Text));
                prescriptionResult.PR_AttentionPoint = LEAttentionpoint.Text;
                prescriptionResult.PR_UserThoughts = LEUserthoughts.Text;
                prescriptionResult.PR_Memo = LEMemo.Text;

                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (HAGroupcount.Text != "")
            {
                //臂部外展内收机
                PrescriptionResult prescriptionResult = new PrescriptionResult();
                DevicePrescription devicePrescription = new DevicePrescription();

                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(HAGroupcount.Text);
                devicePrescription.dp_groupnum = Convert.ToInt32(HAGroupnum.Text);
                devicePrescription.dp_relaxtime = Convert.ToInt32(HARelaxTime.Text);
                devicePrescription.dp_weight = Convert.ToDouble(HAWeight.Text);
                devicePrescription.Dp_status = 1;
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, HAMoveway.Text)); //移乘方式
                //TODO 设备属性、类型没有获取

                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                prescriptionResult.PR_SportStrength = byte.Parse(HASportstrength.Text);
                prescriptionResult.PR_Time1 = Convert.ToDouble(HATime1.Text);
                prescriptionResult.PR_Time2 = Convert.ToDouble(HATime2.Text);
                prescriptionResult.PR_Distance = Convert.ToInt32(HADistance.Text);
                prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(HACountworkqu.Text);
                prescriptionResult.PR_Cal = Convert.ToDouble(HACal.Text);
                prescriptionResult.PR_Index = Convert.ToDouble(HAIndex.Text);
                prescriptionResult.PR_FinishGroup = Convert.ToInt32(HAFinishgroup.Text);
                prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, HLPEvaluate.Text));
                prescriptionResult.PR_AttentionPoint = HAAttentionpoint.Text;
                prescriptionResult.PR_UserThoughts = HAUserthoughts.Text;
                prescriptionResult.PR_Memo = HAMemo.Text;

                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (CPGroupcount.Text != "")
            {
                //胸部推举机
                PrescriptionResult prescriptionResult = new PrescriptionResult();
                DevicePrescription devicePrescription = new DevicePrescription();

                devicePrescription.Gmt_Create = da;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(CPGroupcount.Text);
                devicePrescription.dp_groupnum = Convert.ToInt32(CPGroupnum.Text);
                devicePrescription.dp_relaxtime = Convert.ToInt32(CPRelaxTime.Text);
                devicePrescription.dp_weight = Convert.ToDouble(CPWeight.Text);
                devicePrescription.Dp_status = 1;
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, CPMoveway.Text)); //移乘方式
                //TODO 设备属性、类型没有获取

                prescriptionResult.Gmt_Create = da;
                prescriptionResult.Gmt_Modified = DateTime.Now;
                prescriptionResult.PR_SportStrength = byte.Parse(CPSportstrength.Text);
                prescriptionResult.PR_Time1 = Convert.ToDouble(CPTime1.Text);
                prescriptionResult.PR_Time2 = Convert.ToDouble(CPTime2.Text);
                prescriptionResult.PR_Distance = Convert.ToInt32(CPDistance.Text);
                prescriptionResult.PR_CountWorkQuantity = Convert.ToDouble(CPCountworkqu.Text);
                prescriptionResult.PR_Cal = Convert.ToDouble(CPCal.Text);
                prescriptionResult.PR_Index = Convert.ToDouble(CPIndex.Text);
                prescriptionResult.PR_FinishGroup = Convert.ToInt32(CPFinishgroup.Text);
                prescriptionResult.PR_Evaluate = Byte.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.Evaluate, HLPEvaluate.Text));
                prescriptionResult.PR_AttentionPoint = CPAttentionpoint.Text;
                prescriptionResult.PR_UserThoughts = CPUserthoughts.Text;
                prescriptionResult.PR_Memo = CPMemo.Text;

                prescription.Add(devicePrescription, prescriptionResult);
            }
            //插入训练结果
            new TrainService().AddPrescriptionResult(trainInfo, prescription);
            //打印
            MessageBox.Show("已存储");
            this.Close();
        }

        private void InputTrainingResults_OnLoaded(object sender, RoutedEventArgs e)
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
        //错误：OnlyInputNumbers
        //设置手机号输入框只能输入数字
        private void OnlyInputNumbers(object sender, TextCompositionEventArgs e)
        {
            inputlimited.InputLimited.OnlyInputNumbers(e);
        }

    }
}
