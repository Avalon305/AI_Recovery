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
using spms.view.dto;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// ViewTrainingResults.xaml 的交互逻辑
    /// </summary>
    public partial class ViewTrainingResults : Window
    {
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
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            Dictionary<string, object> dic = (Dictionary<string, Object>) DataContext;
            user = (User) dic["user"];
            trainDto = (TrainDTO) dic["trainDto"];
            //绑定数据
            Load_Data();
        }

        private void Load_Data()
        {
            //用户信息
            l1.Content = user.User_Name;
            l2.Content = user.Pk_User_Id;
            //训练日期
            da.Text = trainDto.prescriptionResult.Gmt_Create.ToString();

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
                        HLPAttr1.Text = attrs[0];
                        HLPAttr2.Text = attrs[1];
                        HLPAttr3.Text = attrs[2];
                        HLPAttr4.Text = attrs[3];
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        HLPSportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        HLPTime1.Text = trainDto.prescriptionResult.PR_Time1.ToString();
                        HLPDistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        HLPCountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        HLPCal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        HLPIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        HLPTime2.Text = trainDto.prescriptionResult.PR_Time2.ToString();
                        HLPFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
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
                        ROWAttr1.Text = attrs[0];
                        ROWAttr2.Text = attrs[1];
                        ROWAttr3.Text = attrs[2];
                        ROWAttr4.Text = attrs[3];
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        ROWSportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        ROWTime1.Text = trainDto.prescriptionResult.PR_Time1.ToString();
                        ROWDistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        ROWCountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        ROWCal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        ROWIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        ROWTime2.Text = trainDto.prescriptionResult.PR_Time2.ToString();
                        ROWFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
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
                        TFAttr1.Text = attrs[0];
                        TFAttr2.Text = attrs[1];
                        TFAttr3.Text = attrs[2];
                        TFAttr4.Text = attrs[3];
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        TFSportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        TFTime1.Text = trainDto.prescriptionResult.PR_Time1.ToString();
                        TFDistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        TFCountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        TFCal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        TFIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        TFTime2.Text = trainDto.prescriptionResult.PR_Time2.ToString();
                        TFFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
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
                        LEAttr1.Text = attrs[0];
                        LEAttr2.Text = attrs[1];
                        LEAttr3.Text = attrs[2];
                        LEAttr4.Text = attrs[3];
                        LEAttr5.Text = attrs[4];
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        LESportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        LETime1.Text = trainDto.prescriptionResult.PR_Time1.ToString();
                        LEDistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        LECountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        LECal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        LEIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        LETime2.Text = trainDto.prescriptionResult.PR_Time2.ToString();
                        LEFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
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
                        HAAttr1.Text = attrs[0];
                        HAAttr2.Text = attrs[1];
                        HAAttr3.Text = attrs[2];
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        HASportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        HATime1.Text = trainDto.prescriptionResult.PR_Time1.ToString();
                        HADistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        HACountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        HACal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        HAIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        HATime2.Text = trainDto.prescriptionResult.PR_Time2.ToString();
                        HAFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
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
                        CPAttr1.Text = attrs[0];
                        CPAttr2.Text = attrs[1];
                        CPAttr3.Text = attrs[2];
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        CPSportstrength.Text = trainDto.prescriptionResult.PR_SportStrength.ToString();
                        CPTime1.Text = trainDto.prescriptionResult.PR_Time1.ToString();
                        CPDistance.Text = trainDto.prescriptionResult.PR_Distance.ToString();
                        CPCountworkqu.Text = trainDto.prescriptionResult.PR_CountWorkQuantity.ToString();
                        CPCal.Text = trainDto.prescriptionResult.PR_Cal.ToString();
                        CPIndex.Text = trainDto.prescriptionResult.PR_Index.ToString();
                        CPTime2.Text = trainDto.prescriptionResult.PR_Time2.ToString();
                        CPFinishgroup.Text = trainDto.prescriptionResult.PR_FinishGroup.ToString();
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
    }
}