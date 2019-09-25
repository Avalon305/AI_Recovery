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
using Recovery.constant;
using Recovery.dao;
using Recovery.entity;
using Recovery.service;
using Recovery.util;
using Recovery.view.dto;
using Recovery.view.EchartsClass;

namespace Recovery.view.Pages.ChildWin
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
        private NewTrainDTO trainDto;

        #region 优化
        //private List<NewTrainDTO> trainDtoList;
        #endregion

        /// <summary>
        /// dic未知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Height = SystemParameters.WorkArea.Size.Height;
            //viewbox.MaxHeight = SystemParameters.WorkArea.Size.Height;
            //viewbox.MaxWidth = SystemParameters.WorkArea.Size.Width;
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            Dictionary<string, object> dic = (Dictionary<string, Object>) DataContext;
            user = (User) dic["user"];
            trainDto = (NewTrainDTO) dic["trainDto"];
            logger.Info("user:" + user + "; trainDto:" + trainDto);
            #region 优化
            //Dictionary<string, object> dic = (Dictionary<string, Object>)DataContext;
            //user = (User)dic["user"];
            //trainDtoList = (List<NewTrainDTO>)dic["trainDtos"];
            //logger.Info("user:" + user + "; trainDtos:" + trainDtoList.Count);
            #endregion
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

        /// <summary>
        /// 加载数据
        /// </summary>
        private void Load_Data()
        {
            //用户信息
            l1.Content = user.User_Name;
            l2.Content = user.Pk_User_Id;
            //训练日期
            da.Content = trainDto.prescriptionResult.Gmt_create.ToString();
            string path = AppDomain.CurrentDomain.BaseDirectory;
            //string rootpath = path.Substring(0, path.LastIndexOf("bin"));
            //查询处方和结果
            #region 优化
            //List<NewTrainDTO> trainDtoLists = new List<NewTrainDTO>();
            //for(int i=0; i< trainDtoList.Count; i++)
            //{
            //    if(new TrainService().GetTrainDTOByPRId(Convert.ToInt32(trainDtoList[i].prescriptionResult.Pk_pr_id))[0] != null)
            //    {
            //        trainDtoLists.Add(new TrainService().GetTrainDTOByPRId(Convert.ToInt32(trainDtoList[i].prescriptionResult.Pk_pr_id))[0]);
            //    }
            //}
            #endregion
            List<NewTrainDTO> trainDtos = new TrainService().GetTrainDTOByPRId(Convert.ToInt32(trainDto.prescriptionResult.Pk_pr_id));
            DeviceSortDAO deviceSortDao = new DeviceSortDAO();
            ResultLine resultLine = new ResultLine();
            //循环判断填充数据
            foreach (NewTrainDTO trainDto in trainDtos)
            {
                switch (trainDto.devicePrescription.Fk_ds_id)
                {
                    case (int)DeviceType.P01:
                        HLPGroupcount.Text = trainDto.devicePrescription.Dp_groupcount.ToString();
                        HLPGroupnum.Text = trainDto.devicePrescription.Dp_groupnum.ToString();
                        HLPRelaxTime.Text = trainDto.devicePrescription.Dp_relaxtime.ToString();
                        HLPMoveway.Text = trainDto.moveway;
                        if (trainDto.devicePrescription.Device_mode == DevConstants.REHABILITATION_MODEL)
                        {
                            HLPTrainingModel.Text = LanguageUtils.ConvertLanguage("康复模式","Rehabilitation Model");
                            HLPselect_change();
                            HLPConsequentForce.Text = trainDto.devicePrescription.Consequent_force.ToString();
                            HLPReverseForce.Text = trainDto.devicePrescription.Reverse_force.ToString();
                        }
                        else if (trainDto.devicePrescription.Device_mode == DevConstants.ACTIVE_MODEL)
                        {
                            HLPTrainingModel.Text = LanguageUtils.ConvertLanguage("主被动模式","Active Model");
                            HLPselect_change();
                            HLPSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        else
                        {
                            HLPTrainingModel.Text = LanguageUtils.ConvertLanguage("被动模式", "Passive Model");
                            HLPselect_change();
                            HLPSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        HLPSportstrength.Text = StrengthConverter(trainDto.prescriptionResult.pr_userthoughts.ToString());
                        HLPHeartRateList.Text = trainDto.prescriptionResult.Heart_rate_list;
                        HLPEnergy.Text = trainDto.prescriptionResult.Energy.ToString();
                        HLPFinishNum.Text = trainDto.prescriptionResult.Finish_num.ToString();
                        HLPAttentionpoint.Text = trainDto.devicePrescription.Dp_memo;

                        // 心率折线图
                        HLPWeb.ObjectForScripting = new ResultLine(trainDto.prescriptionResult.Heart_rate_list, 0);
                        HLPWeb.Navigate(new Uri(path + "dist\\HLPLine.html"));
                        break;
                    case (int)DeviceType.P00:
                        ROWGroupcount.Text = trainDto.devicePrescription.Dp_groupcount.ToString();
                        ROWGroupnum.Text = trainDto.devicePrescription.Dp_groupnum.ToString();
                        ROWRelaxTime.Text = trainDto.devicePrescription.Dp_relaxtime.ToString();
                        ROWMoveway.Text = trainDto.moveway;
                        if (trainDto.devicePrescription.Device_mode == DevConstants.REHABILITATION_MODEL)
                        {
                            ROWTrainingModel.Text = LanguageUtils.ConvertLanguage("康复模式", "Rehabilitation Model");
                            ROWselect_change();
                            ROWConsequentForce.Text = trainDto.devicePrescription.Consequent_force.ToString();
                            ROWReverseForce.Text = trainDto.devicePrescription.Reverse_force.ToString();
                        }
                        else if (trainDto.devicePrescription.Device_mode == DevConstants.ACTIVE_MODEL)
                        {
                            ROWTrainingModel.Text = LanguageUtils.ConvertLanguage("主被动模式", "Active Model");
                            ROWselect_change();
                            ROWSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        else
                        {
                            ROWTrainingModel.Text = LanguageUtils.ConvertLanguage("被动模式", "Passive Model");
                            ROWselect_change();
                            ROWSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        ROWSportstrength.Text = StrengthConverter(trainDto.prescriptionResult.pr_userthoughts.ToString());
                        ROWHeartRateList.Text = trainDto.prescriptionResult.Heart_rate_list;
                        ROWEnergy.Text = trainDto.prescriptionResult.Energy.ToString();
                        ROWFinishNum.Text = trainDto.prescriptionResult.Finish_num.ToString();
                        ROWAttentionpoint.Text = trainDto.devicePrescription.Dp_memo;

                        // 心率折线图
                        ROWWeb.ObjectForScripting = new ResultLine(trainDto.prescriptionResult.Heart_rate_list, 1);
                        ROWWeb.Navigate(new Uri(path + "dist\\ROWLine.html"));
                        break;
                    case (int)DeviceType.P09:
                        TFGroupcount.Text = trainDto.devicePrescription.Dp_groupcount.ToString();
                        TFGroupnum.Text = trainDto.devicePrescription.Dp_groupnum.ToString();
                        TFRelaxTime.Text = trainDto.devicePrescription.Dp_relaxtime.ToString();
                        TFMoveway.Text = trainDto.moveway;
                        if (trainDto.devicePrescription.Device_mode == DevConstants.REHABILITATION_MODEL)
                        {
                            TFTrainingModel.Text = LanguageUtils.ConvertLanguage("康复模式", "Rehabilitation Model");
                            TFselect_change();
                            TFConsequentForce.Text = trainDto.devicePrescription.Consequent_force.ToString();
                            TFReverseForce.Text = trainDto.devicePrescription.Reverse_force.ToString();
                        }
                        else if (trainDto.devicePrescription.Device_mode == DevConstants.ACTIVE_MODEL)
                        {
                            TFTrainingModel.Text = LanguageUtils.ConvertLanguage("主被动模式", "Active Model");
                            TFselect_change();
                            TFSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        else
                        {
                            TFTrainingModel.Text = LanguageUtils.ConvertLanguage("被动模式", "Passive Model");
                            TFselect_change();
                            TFSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        TFSportstrength.Text = StrengthConverter(trainDto.prescriptionResult.pr_userthoughts.ToString());
                        TFHeartRateList.Text = trainDto.prescriptionResult.Heart_rate_list;
                        TFEnergy.Text = trainDto.prescriptionResult.Energy.ToString();
                        TFFinishNum.Text = trainDto.prescriptionResult.Finish_num.ToString();
                        TFAttentionpoint.Text = trainDto.devicePrescription.Dp_memo;

                        // 心率折线图
                        TFWeb.ObjectForScripting = new ResultLine(trainDto.prescriptionResult.Heart_rate_list, 2);
                        TFWeb.Navigate(new Uri(path + "dist\\TFLine.html"));
                        break;
                    case (int)DeviceType.P06:
                        LEGroupcount.Text = trainDto.devicePrescription.Dp_groupcount.ToString();
                        LEGroupnum.Text = trainDto.devicePrescription.Dp_groupnum.ToString();
                        LERelaxTime.Text = trainDto.devicePrescription.Dp_relaxtime.ToString();
                        LEMoveway.Text = trainDto.moveway;
                        if (trainDto.devicePrescription.Device_mode == DevConstants.REHABILITATION_MODEL)
                        {
                            LETrainingModel.Text = LanguageUtils.ConvertLanguage("康复模式", "Rehabilitation Model");
                            LEselect_change();
                            LEConsequentForce.Text = trainDto.devicePrescription.Consequent_force.ToString();
                            LEReverseForce.Text = trainDto.devicePrescription.Reverse_force.ToString();
                        }
                        else if (trainDto.devicePrescription.Device_mode == DevConstants.ACTIVE_MODEL)
                        {
                            LETrainingModel.Text = LanguageUtils.ConvertLanguage("主被动模式", "Active Model");
                            LEselect_change();
                            LESpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        else
                        {
                            LETrainingModel.Text = LanguageUtils.ConvertLanguage("被动模式", "Passive Model");
                            LEselect_change();
                            LESpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        LESportstrength.Text = StrengthConverter(trainDto.prescriptionResult.pr_userthoughts.ToString());
                        LEHeartRateList.Text = trainDto.prescriptionResult.Heart_rate_list;
                        LEEnergy.Text = trainDto.prescriptionResult.Energy.ToString();
                        LEFinishNum.Text = trainDto.prescriptionResult.Finish_num.ToString();
                        LEAttentionpoint.Text = trainDto.devicePrescription.Dp_memo;

                        // 心率折线图
                        LEWeb.ObjectForScripting = new ResultLine(trainDto.prescriptionResult.Heart_rate_list, 3);
                        LEWeb.Navigate(new Uri(path + "dist\\LELine.html"));
                        break;
                    case (int)DeviceType.P02:
                        HAGroupcount.Text = trainDto.devicePrescription.Dp_groupcount.ToString();
                        HAGroupnum.Text = trainDto.devicePrescription.Dp_groupnum.ToString();
                        HARelaxTime.Text = trainDto.devicePrescription.Dp_relaxtime.ToString();
                        HAMoveway.Text = trainDto.moveway;
                        if (trainDto.devicePrescription.Device_mode == DevConstants.REHABILITATION_MODEL)
                        {
                            HATrainingModel.Text = LanguageUtils.ConvertLanguage("康复模式", "Rehabilitation Model");
                            HAselect_change();
                            HAConsequentForce.Text = trainDto.devicePrescription.Consequent_force.ToString();
                            HAReverseForce.Text = trainDto.devicePrescription.Reverse_force.ToString();
                        }
                        else if (trainDto.devicePrescription.Device_mode == DevConstants.ACTIVE_MODEL)
                        {
                            HATrainingModel.Text = LanguageUtils.ConvertLanguage("主被动模式", "Active Model");
                            HAselect_change();
                            HASpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        else
                        {
                            HATrainingModel.Text = LanguageUtils.ConvertLanguage("被动模式", "Passive Model");
                            HAselect_change();
                            HASpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        HASportstrength.Text = StrengthConverter(trainDto.prescriptionResult.pr_userthoughts.ToString());
                        HAHeartRateList.Text = trainDto.prescriptionResult.Heart_rate_list;
                        HAEnergy.Text = trainDto.prescriptionResult.Energy.ToString();
                        HAFinishNum.Text = trainDto.prescriptionResult.Finish_num.ToString();
                        HAAttentionpoint.Text = trainDto.devicePrescription.Dp_memo;

                        // 心率折线图
                        HAWeb.ObjectForScripting = new ResultLine(trainDto.prescriptionResult.Heart_rate_list, 4);
                        HAWeb.Navigate(new Uri(path + "dist\\HALine.html"));
                        break;
                    case (int)DeviceType.P05:
                        CPGroupcount.Text = trainDto.devicePrescription.Dp_groupcount.ToString();
                        CPGroupnum.Text = trainDto.devicePrescription.Dp_groupnum.ToString();
                        CPRelaxTime.Text = trainDto.devicePrescription.Dp_relaxtime.ToString();
                        CPMoveway.Text = trainDto.moveway;
                        if (trainDto.devicePrescription.Device_mode == DevConstants.REHABILITATION_MODEL)
                        {
                            CPTrainingModel.Text = LanguageUtils.ConvertLanguage("康复模式", "Rehabilitation Model");
                            CPselect_change();
                            CPConsequentForce.Text = trainDto.devicePrescription.Consequent_force.ToString();
                            CPReverseForce.Text = trainDto.devicePrescription.Reverse_force.ToString();
                        }
                        else if (trainDto.devicePrescription.Device_mode == DevConstants.ACTIVE_MODEL)
                        {
                            CPTrainingModel.Text = LanguageUtils.ConvertLanguage("主被动模式", "Active Model");
                            CPselect_change();
                            CPSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        else
                        {
                            CPTrainingModel.Text = LanguageUtils.ConvertLanguage("被动模式", "Passive Model");
                            CPselect_change();
                            CPSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        CPSportstrength.Text = StrengthConverter(trainDto.prescriptionResult.pr_userthoughts.ToString());
                        CPHeartRateList.Text = trainDto.prescriptionResult.Heart_rate_list;
                        CPEnergy.Text = trainDto.prescriptionResult.Energy.ToString();
                        CPFinishNum.Text = trainDto.prescriptionResult.Finish_num.ToString();
                        CPAttentionpoint.Text = trainDto.devicePrescription.Dp_memo;

                        // 心率折线图
                        CPWeb.ObjectForScripting = new ResultLine(trainDto.prescriptionResult.Heart_rate_list, 5);
                        CPWeb.Navigate(new Uri(path + "dist\\CPLine.html"));
                        break;
                    case (int)DeviceType.P03:
                        NewAGroupcount.Text = trainDto.devicePrescription.Dp_groupcount.ToString();
                        NewAGroupnum.Text = trainDto.devicePrescription.Dp_groupnum.ToString();
                        NewARelaxTime.Text = trainDto.devicePrescription.Dp_relaxtime.ToString();
                        NewAMoveway.Text = trainDto.moveway;
                        if (trainDto.devicePrescription.Device_mode == DevConstants.REHABILITATION_MODEL)
                        {
                            NewATrainingModel.Text = LanguageUtils.ConvertLanguage("康复模式", "Rehabilitation Model");
                            NewAselect_change();
                            NewAConsequentForce.Text = trainDto.devicePrescription.Consequent_force.ToString();
                            NewAReverseForce.Text = trainDto.devicePrescription.Reverse_force.ToString();
                        }
                        else if (trainDto.devicePrescription.Device_mode == DevConstants.ACTIVE_MODEL)
                        {
                            NewATrainingModel.Text = LanguageUtils.ConvertLanguage("主被动模式", "Active Model");
                            NewAselect_change();
                            NewASpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        else
                        {
                            NewATrainingModel.Text = LanguageUtils.ConvertLanguage("被动模式", "Passive Model");
                            NewAselect_change();
                            NewASpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        NewASportstrength.Text = StrengthConverter(trainDto.prescriptionResult.pr_userthoughts.ToString());
                        NewAHeartRateList.Text = trainDto.prescriptionResult.Heart_rate_list;
                        NewAEnergy.Text = trainDto.prescriptionResult.Energy.ToString();
                        NewAFinishNum.Text = trainDto.prescriptionResult.Finish_num.ToString();
                        NewAAttentionpoint.Text = trainDto.devicePrescription.Dp_memo;

                        // 心率折线图
                        NewAWeb.ObjectForScripting = new ResultLine(trainDto.prescriptionResult.Heart_rate_list, 6);
                        NewAWeb.Navigate(new Uri(path + "dist\\NewALine.html"));
                        break;
                    case (int)DeviceType.P04:
                        NewBGroupcount.Text = trainDto.devicePrescription.Dp_groupcount.ToString();
                        NewBGroupnum.Text = trainDto.devicePrescription.Dp_groupnum.ToString();
                        NewBRelaxTime.Text = trainDto.devicePrescription.Dp_relaxtime.ToString();
                        NewBMoveway.Text = trainDto.moveway;
                        if (trainDto.devicePrescription.Device_mode == DevConstants.REHABILITATION_MODEL)
                        {
                            NewBTrainingModel.Text = LanguageUtils.ConvertLanguage("康复模式", "Rehabilitation Model");
                            NewBselect_change();
                            NewBConsequentForce.Text = trainDto.devicePrescription.Consequent_force.ToString();
                            NewBReverseForce.Text = trainDto.devicePrescription.Reverse_force.ToString();
                        }
                        else if (trainDto.devicePrescription.Device_mode == DevConstants.ACTIVE_MODEL)
                        {
                            NewBTrainingModel.Text = LanguageUtils.ConvertLanguage("主被动模式", "Active Model");
                            NewBselect_change();
                            NewBSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        else
                        {
                            NewBTrainingModel.Text = LanguageUtils.ConvertLanguage("被动模式", "Passive Model");
                            NewBselect_change();
                            NewBSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        NewBSportstrength.Text = StrengthConverter(trainDto.prescriptionResult.pr_userthoughts.ToString());
                        NewBHeartRateList.Text = trainDto.prescriptionResult.Heart_rate_list;
                        NewBEnergy.Text = trainDto.prescriptionResult.Energy.ToString();
                        NewBFinishNum.Text = trainDto.prescriptionResult.Finish_num.ToString();
                        NewBAttentionpoint.Text = trainDto.devicePrescription.Dp_memo;

                        // 心率折线图
                        NewBWeb.ObjectForScripting = new ResultLine(trainDto.prescriptionResult.Heart_rate_list, 7);
                        NewBWeb.Navigate(new Uri(path + "dist\\NewBLine.html"));
                        break;
                    case (int)DeviceType.P07:
                        NewCGroupcount.Text = trainDto.devicePrescription.Dp_groupcount.ToString();
                        NewCGroupnum.Text = trainDto.devicePrescription.Dp_groupnum.ToString();
                        NewCRelaxTime.Text = trainDto.devicePrescription.Dp_relaxtime.ToString();
                        NewCMoveway.Text = trainDto.moveway;
                        if (trainDto.devicePrescription.Device_mode == DevConstants.REHABILITATION_MODEL)
                        {
                            NewCTrainingModel.Text = LanguageUtils.ConvertLanguage("康复模式", "Rehabilitation Model");
                            NewCselect_change();
                            NewCConsequentForce.Text = trainDto.devicePrescription.Consequent_force.ToString();
                            NewCReverseForce.Text = trainDto.devicePrescription.Reverse_force.ToString();
                        }
                        else if (trainDto.devicePrescription.Device_mode == DevConstants.ACTIVE_MODEL)
                        {
                            NewCTrainingModel.Text = LanguageUtils.ConvertLanguage("主被动模式", "Active Model");
                            NewCselect_change();
                            NewCSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        else
                        {
                            NewCTrainingModel.Text = LanguageUtils.ConvertLanguage("被动模式", "Passive Model");
                            NewCselect_change();
                            NewCSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        NewCSportstrength.Text = StrengthConverter(trainDto.prescriptionResult.pr_userthoughts.ToString());
                        NewCHeartRateList.Text = trainDto.prescriptionResult.Heart_rate_list;
                        NewCEnergy.Text = trainDto.prescriptionResult.Energy.ToString();
                        NewCFinishNum.Text = trainDto.prescriptionResult.Finish_num.ToString();
                        NewCAttentionpoint.Text = trainDto.devicePrescription.Dp_memo;

                        // 心率折线图
                        NewCWeb.ObjectForScripting = new ResultLine(trainDto.prescriptionResult.Heart_rate_list, 8);
                        NewCWeb.Navigate(new Uri(path + "dist\\NewCLine.html"));
                        break;
                    case (int)DeviceType.P08:
                        NewDGroupcount.Text = trainDto.devicePrescription.Dp_groupcount.ToString();
                        NewDGroupnum.Text = trainDto.devicePrescription.Dp_groupnum.ToString();
                        NewDRelaxTime.Text = trainDto.devicePrescription.Dp_relaxtime.ToString();
                        NewDMoveway.Text = trainDto.moveway;
                        if (trainDto.devicePrescription.Device_mode == DevConstants.REHABILITATION_MODEL)
                        {
                            NewDTrainingModel.Text = LanguageUtils.ConvertLanguage("康复模式", "Rehabilitation Model");
                            NewDselect_change();
                            NewDConsequentForce.Text = trainDto.devicePrescription.Consequent_force.ToString();
                            NewDReverseForce.Text = trainDto.devicePrescription.Reverse_force.ToString();
                        }
                        else if (trainDto.devicePrescription.Device_mode == DevConstants.ACTIVE_MODEL)
                        {
                            NewDTrainingModel.Text = LanguageUtils.ConvertLanguage("主被动模式", "Active Model");
                            NewDselect_change();
                            NewDSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        else
                        {
                            NewDTrainingModel.Text = LanguageUtils.ConvertLanguage("被动模式", "Passive Model");
                            NewDselect_change();
                            NewDSpeedRank.Text = trainDto.devicePrescription.Speed_rank.ToString();
                        }
                        if (trainDto.prescriptionResult == null)
                        {
                            break;
                        }
                        NewDSportstrength.Text = StrengthConverter(trainDto.prescriptionResult.pr_userthoughts.ToString());
                        NewDHeartRateList.Text = trainDto.prescriptionResult.Heart_rate_list;
                        NewDEnergy.Text = trainDto.prescriptionResult.Energy.ToString();
                        NewDFinishNum.Text = trainDto.prescriptionResult.Finish_num.ToString();
                        NewDAttentionpoint.Text = trainDto.devicePrescription.Dp_memo;

                        // 心率折线图
                        NewDWeb.ObjectForScripting = new ResultLine(trainDto.prescriptionResult.Heart_rate_list, 9);
                        NewDWeb.Navigate(new Uri(path + "dist\\NewDLine.html"));
                        break;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ViewTrainingResults()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;

            //string path = AppDomain.CurrentDomain.BaseDirectory;
            //Console.WriteLine(path + "dist\\HLPLine.html");
            ////string rootpath = path.Substring(0, path.LastIndexOf("bin"));
            ////Console.WriteLine(rootpath);
            ////HLPWeb.Navigate(new Uri(rootpath + "Recovery/Echarts/dist/HLPLine.html"));
            //HLPWeb.Navigate(new Uri(path + "dist\\HLPLine.html"));
        }

        /// <summary>
        /// 取消操作，关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 回车按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (HLPTrainingModel.Text.Equals("康复模式") || HLPTrainingModel.Text.Equals("Rehabilitation Model"))
            {
                HLPSR_Label.Background = Brushes.White;
                HLPSpeedRank.Visibility = Visibility.Hidden;
                Grid.SetRow(HLPSR_Label, 13);
                Grid.SetRow(HLPSpeedRank, 13);

                HLPCF_Label.Background = Brushes.Gray;
                HLPConsequentForce.Visibility = Visibility.Visible;
                HLPRF_Label.Background = Brushes.Gray;
                HLPReverseForce.Visibility = Visibility.Visible;
                Grid.SetRow(HLPCF_Label, 11);
                Grid.SetRow(HLPConsequentForce, 11);
            }
            else
            {
                HLPCF_Label.Background = Brushes.White;
                HLPConsequentForce.Visibility = Visibility.Hidden;
                HLPRF_Label.Background = Brushes.White;
                HLPReverseForce.Visibility = Visibility.Hidden;
                Grid.SetRow(HLPCF_Label, 13);
                Grid.SetRow(HLPConsequentForce, 13);

                HLPSR_Label.Background = Brushes.Gray;
                HLPSpeedRank.Visibility = Visibility.Visible;
                Grid.SetRow(HLPSR_Label, 11);
                Grid.SetRow(HLPSpeedRank, 11);
            }
        }
        private void ROWselect_change()
        {
            if (ROWTrainingModel.Text.Equals("康复模式") || ROWTrainingModel.Text.Equals("Rehabilitation Model"))
            {
                ROWSR_Label.Background = Brushes.White;
                ROWSpeedRank.Visibility = Visibility.Hidden;
                Grid.SetRow(ROWSR_Label, 13);
                Grid.SetRow(ROWSpeedRank, 13);

                ROWCF_Label.Background = Brushes.Gray;
                ROWConsequentForce.Visibility = Visibility.Visible;
                ROWRF_Label.Background = Brushes.Gray;
                ROWReverseForce.Visibility = Visibility.Visible;
                Grid.SetRow(ROWCF_Label, 11);
                Grid.SetRow(ROWConsequentForce, 11);
            }
            else
            {
                ROWCF_Label.Background = Brushes.White;
                ROWConsequentForce.Visibility = Visibility.Hidden;
                ROWRF_Label.Background = Brushes.White;
                ROWReverseForce.Visibility = Visibility.Hidden;
                Grid.SetRow(ROWCF_Label, 13);
                Grid.SetRow(ROWConsequentForce, 13);

                ROWSR_Label.Background = Brushes.Gray;
                ROWSpeedRank.Visibility = Visibility.Visible;
                Grid.SetRow(ROWSR_Label, 11);
                Grid.SetRow(ROWSpeedRank, 11);
            }
        }
        private void TFselect_change()
        {
            if (TFTrainingModel.Text.Equals("康复模式") || TFTrainingModel.Text.Equals("Rehabilitation Model"))
            {
                TFSR_Label.Background = Brushes.White;
                TFSpeedRank.Visibility = Visibility.Hidden;
                Grid.SetRow(TFSR_Label, 13);
                Grid.SetRow(TFSpeedRank, 13);

                TFCF_Label.Background = Brushes.Gray;
                TFConsequentForce.Visibility = Visibility.Visible;
                TFRF_Label.Background = Brushes.Gray;
                TFReverseForce.Visibility = Visibility.Visible;
                Grid.SetRow(TFCF_Label, 11);
                Grid.SetRow(TFConsequentForce, 11);
            }
            else
            {
                TFCF_Label.Background = Brushes.White;
                TFConsequentForce.Visibility = Visibility.Hidden;
                TFRF_Label.Background = Brushes.White;
                TFReverseForce.Visibility = Visibility.Hidden;
                Grid.SetRow(TFCF_Label, 13);
                Grid.SetRow(TFConsequentForce, 13);

                TFSR_Label.Background = Brushes.Gray;
                TFSpeedRank.Visibility = Visibility.Visible;
                Grid.SetRow(TFSR_Label, 11);
                Grid.SetRow(TFSpeedRank, 11);
            }
        }
        private void LEselect_change()
        {
            if (LETrainingModel.Text.Equals("康复模式") || LETrainingModel.Text.Equals("Rehabilitation Model"))
            {
                LESR_Label.Background = Brushes.White;
                LESpeedRank.Visibility = Visibility.Hidden;
                Grid.SetRow(LESR_Label, 13);
                Grid.SetRow(LESpeedRank, 13);

                LECF_Label.Background = Brushes.Gray;
                LEConsequentForce.Visibility = Visibility.Visible;
                LERF_Label.Background = Brushes.Gray;
                LEReverseForce.Visibility = Visibility.Visible;
                Grid.SetRow(LECF_Label, 11);
                Grid.SetRow(LEConsequentForce, 11);
            }
            else
            {
                LECF_Label.Background = Brushes.White;
                LEConsequentForce.Visibility = Visibility.Hidden;
                LERF_Label.Background = Brushes.White;
                LEReverseForce.Visibility = Visibility.Hidden;
                Grid.SetRow(LECF_Label, 13);
                Grid.SetRow(LEConsequentForce, 13);

                LESR_Label.Background = Brushes.Gray;
                LESpeedRank.Visibility = Visibility.Visible;
                Grid.SetRow(LESR_Label, 11);
                Grid.SetRow(LESpeedRank, 11);
            }
        }
        private void HAselect_change()
        {
            if (HATrainingModel.Text.Equals("康复模式") || HATrainingModel.Text.Equals("Rehabilitation Model"))
            {
                HASR_Label.Background = Brushes.White;
                HASpeedRank.Visibility = Visibility.Hidden;
                Grid.SetRow(HASR_Label, 13);
                Grid.SetRow(HASpeedRank, 13);

                HACF_Label.Background = Brushes.Gray;
                HAConsequentForce.Visibility = Visibility.Visible;
                HARF_Label.Background = Brushes.Gray;
                HAReverseForce.Visibility = Visibility.Visible;
                Grid.SetRow(HACF_Label, 11);
                Grid.SetRow(HAConsequentForce, 11);
            }
            else
            {
                HACF_Label.Background = Brushes.White;
                HAConsequentForce.Visibility = Visibility.Hidden;
                HARF_Label.Background = Brushes.White;
                HAReverseForce.Visibility = Visibility.Hidden;
                Grid.SetRow(HACF_Label, 13);
                Grid.SetRow(HAConsequentForce, 13);

                HASR_Label.Background = Brushes.Gray;
                HASpeedRank.Visibility = Visibility.Visible;
                Grid.SetRow(HASR_Label, 11);
                Grid.SetRow(HASpeedRank, 11);
            }
        }
        private void CPselect_change()
        {
            if (CPTrainingModel.Text.Equals("康复模式") || CPTrainingModel.Text.Equals("Rehabilitation Model"))
            {
                CPSR_Label.Background = Brushes.White;
                CPSpeedRank.Visibility = Visibility.Hidden;
                Grid.SetRow(CPSR_Label, 13);
                Grid.SetRow(CPSpeedRank, 13);

                CPCF_Label.Background = Brushes.Gray;
                CPConsequentForce.Visibility = Visibility.Visible;
                CPRF_Label.Background = Brushes.Gray;
                CPReverseForce.Visibility = Visibility.Visible;
                Grid.SetRow(CPCF_Label, 11);
                Grid.SetRow(CPConsequentForce, 11);
            }
            else
            {
                CPCF_Label.Background = Brushes.White;
                CPConsequentForce.Visibility = Visibility.Hidden;
                CPRF_Label.Background = Brushes.White;
                CPReverseForce.Visibility = Visibility.Hidden;
                Grid.SetRow(CPCF_Label, 13);
                Grid.SetRow(CPConsequentForce, 13);

                CPSR_Label.Background = Brushes.Gray;
                CPSpeedRank.Visibility = Visibility.Visible;
                Grid.SetRow(CPSR_Label, 11);
                Grid.SetRow(CPSpeedRank, 11);
            }
        }
        private void NewAselect_change()
        {
            if (NewATrainingModel.Text.Equals("康复模式") || NewATrainingModel.Text.Equals("Rehabilitation Model"))
            {
                NewASR_Label.Background = Brushes.White;
                NewASpeedRank.Visibility = Visibility.Hidden;
                Grid.SetRow(NewASR_Label, 13);
                Grid.SetRow(NewASpeedRank, 13);

                NewACF_Label.Background = Brushes.Gray;
                NewAConsequentForce.Visibility = Visibility.Visible;
                NewARF_Label.Background = Brushes.Gray;
                NewAReverseForce.Visibility = Visibility.Visible;
                Grid.SetRow(NewACF_Label, 11);
                Grid.SetRow(NewAConsequentForce, 11);
            }
            else
            {
                NewACF_Label.Background = Brushes.White;
                NewAConsequentForce.Visibility = Visibility.Hidden;
                NewARF_Label.Background = Brushes.White;
                NewAReverseForce.Visibility = Visibility.Hidden;
                Grid.SetRow(NewACF_Label, 13);
                Grid.SetRow(NewAConsequentForce, 13);

                NewASR_Label.Background = Brushes.Gray;
                NewASpeedRank.Visibility = Visibility.Visible;
                Grid.SetRow(NewASR_Label, 11);
                Grid.SetRow(NewASpeedRank, 11);
            }
        }
        private void NewBselect_change()
        {
            if (NewBTrainingModel.Text.Equals("康复模式") || NewBTrainingModel.Text.Equals("Rehabilitation Model"))
            {
                NewBSR_Label.Background = Brushes.White;
                NewBSpeedRank.Visibility = Visibility.Hidden;
                Grid.SetRow(NewBSR_Label, 13);
                Grid.SetRow(NewBSpeedRank, 13);

                NewBCF_Label.Background = Brushes.Gray;
                NewBConsequentForce.Visibility = Visibility.Visible;
                NewBRF_Label.Background = Brushes.Gray;
                NewBReverseForce.Visibility = Visibility.Visible;
                Grid.SetRow(NewBCF_Label, 11);
                Grid.SetRow(NewBConsequentForce, 11);
            }
            else
            {
                NewBCF_Label.Background = Brushes.White;
                NewBConsequentForce.Visibility = Visibility.Hidden;
                NewBRF_Label.Background = Brushes.White;
                NewBReverseForce.Visibility = Visibility.Hidden;
                Grid.SetRow(NewBCF_Label, 13);
                Grid.SetRow(NewBConsequentForce, 13);

                NewBSR_Label.Background = Brushes.Gray;
                NewBSpeedRank.Visibility = Visibility.Visible;
                Grid.SetRow(NewBSR_Label, 11);
                Grid.SetRow(NewBSpeedRank, 11);
            }
        }
        private void NewCselect_change()
        {
            if (NewCTrainingModel.Text.Equals("康复模式") || NewCTrainingModel.Text.Equals("Rehabilitation Model"))
            {
                NewCSR_Label.Background = Brushes.White;
                NewCSpeedRank.Visibility = Visibility.Hidden;
                Grid.SetRow(NewCSR_Label, 13);
                Grid.SetRow(NewCSpeedRank, 13);

                NewCCF_Label.Background = Brushes.Gray;
                NewCConsequentForce.Visibility = Visibility.Visible;
                NewCRF_Label.Background = Brushes.Gray;
                NewCReverseForce.Visibility = Visibility.Visible;
                Grid.SetRow(NewCCF_Label, 11);
                Grid.SetRow(NewCConsequentForce, 11);
            }
            else
            {
                NewCCF_Label.Background = Brushes.White;
                NewCConsequentForce.Visibility = Visibility.Hidden;
                NewCRF_Label.Background = Brushes.White;
                NewCReverseForce.Visibility = Visibility.Hidden;
                Grid.SetRow(NewCCF_Label, 13);
                Grid.SetRow(NewCConsequentForce, 13);

                NewCSR_Label.Background = Brushes.Gray;
                NewCSpeedRank.Visibility = Visibility.Visible;
                Grid.SetRow(NewCSR_Label, 11);
                Grid.SetRow(NewCSpeedRank, 11);
            }
        }
        private void NewDselect_change()
        {
            if (NewDTrainingModel.Text.Equals("康复模式") || NewDTrainingModel.Text.Equals("Rehabilitation Model"))
            {
                NewDSR_Label.Background = Brushes.White;
                NewDSpeedRank.Visibility = Visibility.Hidden;
                Grid.SetRow(NewDSR_Label, 13);
                Grid.SetRow(NewDSpeedRank, 13);

                NewDCF_Label.Background = Brushes.Gray;
                NewDConsequentForce.Visibility = Visibility.Visible;
                NewDRF_Label.Background = Brushes.Gray;
                NewDReverseForce.Visibility = Visibility.Visible;
                Grid.SetRow(NewDCF_Label, 11);
                Grid.SetRow(NewDConsequentForce, 11);
            }
            else
            {
                NewDCF_Label.Background = Brushes.White;
                NewDConsequentForce.Visibility = Visibility.Hidden;
                NewDRF_Label.Background = Brushes.White;
                NewDReverseForce.Visibility = Visibility.Hidden;
                Grid.SetRow(NewDCF_Label, 13);
                Grid.SetRow(NewDConsequentForce, 13);

                NewDSR_Label.Background = Brushes.Gray;
                NewDSpeedRank.Visibility = Visibility.Visible;
                Grid.SetRow(NewDSR_Label, 11);
                Grid.SetRow(NewDSpeedRank, 11);
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

        private void viewbox_load(object sender, RoutedEventArgs e)
        {
            //this.Visibility = Visibility.Collapsed;
            
            this.Width = viewbox.ActualWidth;
            this.Height = viewbox.ActualHeight;

            Left = (SystemParameters.WorkArea.Size.Width - this.ActualWidth) / 2;
            Top = (SystemParameters.WorkArea.Size.Height - this.ActualHeight) / 2;
            //this.ShowDialog();
            //this.
            // this.Opacity = 1;
            //  this.Visibility = Visibility.Visible;

        }

        private String StrengthConverter(String value)
        {
            return value;
            //if(value == "")
            //{
            //    return "";
            //}
            //int reValue = System.Convert.ToInt32(value);
            //if (reValue == 1 || reValue == 2)
            //{
            //    return "非常轻松";
            //}
            //else if (reValue == 3 || reValue == 4)
            //{
            //    return "很轻松";
            //}
            //else if (reValue == 5 || reValue == 6)
            //{
            //    return "轻松";
            //}
            //else if (reValue == 7 || reValue == 8)
            //{
            //    return "有点儿困难";

            //}
            //else
            //{
            //    return "困难";
            //}
        }
    }
}