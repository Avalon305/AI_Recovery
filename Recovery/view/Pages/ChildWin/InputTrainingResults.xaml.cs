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
using NLog;
using Recovery.constant;
using Recovery.dao;
using Recovery.entity;
using Recovery.entity.newEntity;
using Recovery.service;
using Recovery.util;
using Recovery.view.dto;

namespace Recovery.view.Pages.ChildWin
{
    /// <summary>
    /// InputTrainingResults.xaml 的交互逻辑
    /// </summary>
    public partial class InputTrainingResults : Window
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// 初始加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Height = SystemParameters.WorkArea.Size.Height;
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            Load_Data();//载入数据
            Certain_Dev();//确定购买了哪些设备

            var nullTiIdByUserId = new SymptomInfoDao().GetNullTiIdByUserId(user.Pk_User_Id);
            symp.ItemsSource = new SymptomInfoDTO().ConvertDtoList(nullTiIdByUserId);

            HLPConsequentForce.ItemsSource = Add(5, 100, 2);
            HLPReverseForce.ItemsSource = Add(5, 100, 2);
            HLPSpeedRank.ItemsSource = Add(1, 7, 2);

            ROWConsequentForce.ItemsSource = Add(5, 100, 2);
            ROWReverseForce.ItemsSource = Add(5, 100, 2);
            ROWSpeedRank.ItemsSource = Add(1, 7, 2);

            LEConsequentForce.ItemsSource = Add(5, 100, 2);
            LEReverseForce.ItemsSource = Add(5, 100, 2);
            LESpeedRank.ItemsSource = Add(1, 7, 2);

            TFConsequentForce.ItemsSource = Add(5, 100, 2);
            TFReverseForce.ItemsSource = Add(5, 100, 2);
            TFSpeedRank.ItemsSource = Add(1, 7, 2);

            HAConsequentForce.ItemsSource = Add(5, 100, 2);
            HAReverseForce.ItemsSource = Add(5, 100, 2);
            HASpeedRank.ItemsSource = Add(1, 7, 2);

            CPConsequentForce.ItemsSource = Add(5, 100, 2);
            CPReverseForce.ItemsSource = Add(5, 100, 2);
            CPSpeedRank.ItemsSource = Add(1, 7, 2);

            NewAConsequentForce.ItemsSource = Add(5, 100, 2);
            NewAReverseForce.ItemsSource = Add(5, 100, 2);
            NewASpeedRank.ItemsSource = Add(1, 7, 2);

            NewBConsequentForce.ItemsSource = Add(5, 100, 2);
            NewBReverseForce.ItemsSource = Add(5, 100, 2);
            NewBSpeedRank.ItemsSource = Add(1, 7, 2);

            NewCConsequentForce.ItemsSource = Add(5, 100, 2);
            NewCReverseForce.ItemsSource = Add(5, 100, 2);
            NewCSpeedRank.ItemsSource = Add(1, 7, 2);

            NewDConsequentForce.ItemsSource = Add(5, 100, 2);
            NewDReverseForce.ItemsSource = Add(5, 100, 2);
            NewDSpeedRank.ItemsSource = Add(1, 7, 2);
        }

        /// <summary>
        /// 设备
        /// </summary>
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
                    case "坐式推胸机":
                    case "Chest Press":
                        tab1.IsEnabled = false;
                        sp1.IsEnabled = false;
                        break;
                    case "坐式划船机":
                    case "Rowing":
                        tab2.IsEnabled = false;
                        sp2.IsEnabled = false;
                        break;
                    case "坐式背部伸展机":
                    case "Sitting Back Extender":
                        tab3.IsEnabled = false;
                        sp3.IsEnabled = false;
                        break;
                    case "腿部内弯机":
                    case "Leg Inturn":
                        tab4.IsEnabled = false;
                        sp4.IsEnabled = false;
                        break;
                    case "腿部推蹬机":
                    case "Horizontal Leg Press":
                        tab5.IsEnabled = false;
                        sp5.IsEnabled = false;
                        break;
                    case "腿部外弯机":
                    case "Leg Abduction":
                        tab6.IsEnabled = false;
                        sp6.IsEnabled = false;
                        break;
                    case "腹肌训练机":
                    case "Abdominal Muscle Training":
                        tab7.IsEnabled = false;
                        sp7.IsEnabled = false;
                        break;
                    case "三头肌训练机":
                    case "Triceps Training":
                        tab8.IsEnabled = false;
                        sp8.IsEnabled = false;
                        break;
                    case "蝴蝶机":
                    case "Butterfly Machine":
                        tab9.IsEnabled = false;
                        sp9.IsEnabled = false;
                        break;
                    case "反向蝴蝶机":
                    case "Reverse Butterfly Machine":
                        tab10.IsEnabled = false;
                        sp10.IsEnabled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// List增加规则
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type">1为一次+0.5，2为一次+1</param>
        /// <returns></returns>
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
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;
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
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime? da = dp.DateTime;
            Dictionary<NewDevicePrescription, PrescriptionResultTwo> prescription = new Dictionary<NewDevicePrescription, PrescriptionResultTwo>();
            
            TrainInfo trainInfo = new TrainInfo();
            trainInfo.FK_User_Id = user.Pk_User_Id;
            trainInfo.Gmt_Create = da;
            trainInfo.Gmt_Modified = DateTime.Now;
            trainInfo.Status = (int) TrainInfoStatus.Finish;//手动输入的结果状态为已完成
            DeviceSortDAO deviceSortDao = new DeviceSortDAO();
            string devName;
            if (HLPGroupcount.Text != "")
            {
                // 坐式推胸机
                PrescriptionResultTwo prescriptionResult = new PrescriptionResultTwo();
                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                UserRelationDao userRelationDao = new UserRelationDao();
                UserRelation userRelation = new UserRelation();
                userRelation = userRelationDao.FindUserRelationByuserID(user.Pk_User_Id);
                prescriptionResult.Bind_id = userRelation.Bind_id;
                devicePrescription.Gmt_create = da;
                devicePrescription.Gmt_modified = DateTime.Now;
                #region 处方
                // 组数
                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(HLPGroupcount.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 每组数量
                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(HLPGroupnum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 间隔时间
                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(HLPRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 完成状态
                devicePrescription.Dp_status = 1;
                // 移乘方式
                try
                {
                    devicePrescription.Dp_moveway = int.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, HLPMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 关联的单一设备类型ID
                devicePrescription.Fk_ds_id = (int)DeviceType.P01;
                // 注意点
                devicePrescription.Dp_memo = HLPAttentionpoint.Text;
                // 训练模式
                if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.RehabilitationModel"))
                {
                    devicePrescription.Device_mode = DevConstants.REHABILITATION_MODEL;

                    try
                    {
                        devicePrescription.Consequent_force = int.Parse(HLPConsequentForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的顺向力", "Please choose the right consequent force"));
                        return;
                    }
                    try
                    {
                        devicePrescription.Reverse_force = int.Parse(HLPReverseForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的反向力", "Please choose the right reverse force"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.ActiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.ACTIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(HLPSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                else if(LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.PassiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.PASSIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(HLPSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                // 顺向力
                try
                {
                    devicePrescription.Consequent_force = Convert.ToInt32(HLPRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 反向力
                try
                {
                    devicePrescription.Reverse_force = Convert.ToInt32(HLPRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动速度等级
                try
                {
                    devicePrescription.Speed_rank = Convert.ToInt32(HLPRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                #region 训练结果
                prescriptionResult.Fk_ds_id = (int)DeviceType.P01;
                prescriptionResult.Gmt_create = da;
                prescriptionResult.Gmt_modified = DateTime.Now;
                // 病人感想
                try
                {
                    if (Convert.ToInt32(HLPSportstrength.Text) < 6)
                    {
                        prescriptionResult.pr_userthoughts = "6";
                    }else if(Convert.ToInt32(HLPSportstrength.Text) > 15)
                    {
                        prescriptionResult.pr_userthoughts = "15";
                    }
                    else { 
                        prescriptionResult.pr_userthoughts = HLPSportstrength.Text;
                    }
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 功率
                try
                {
                    prescriptionResult.Power = Convert.ToInt32(HLPPower.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 训练总耗能
                try
                {
                    prescriptionResult.Energy = Convert.ToInt32(HLPEnergy.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 心率集合
                try
                {
                    prescriptionResult.Heart_rate_list = HLPHeartRateList.Text;
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动完成个数
                try
                {
                    prescriptionResult.Finish_num = Convert.ToInt32(HLPFinishNum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (ROWGroupcount.Text != "")
            {
                //坐姿划船机
                PrescriptionResultTwo prescriptionResult = new PrescriptionResultTwo();
                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                UserRelationDao userRelationDao = new UserRelationDao();
                UserRelation userRelation = new UserRelation();
                userRelation = userRelationDao.FindUserRelationByuserID(user.Pk_User_Id);
                prescriptionResult.Bind_id = userRelation.Bind_id;
                devicePrescription.Gmt_create = da;
                devicePrescription.Gmt_modified = DateTime.Now;
                #region 处方
                // 组数
                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(ROWGroupcount.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 每组数量
                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(ROWGroupnum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 间隔时间
                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(ROWRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 完成状态
                devicePrescription.Dp_status = 1;
                // 移乘方式
                try
                {
                    devicePrescription.Dp_moveway = int.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, ROWMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 关联的单一设备类型ID
                devicePrescription.Fk_ds_id = (int)DeviceType.P00;
                // 注意点
                devicePrescription.Dp_memo = ROWAttentionpoint.Text;
                // 训练模式
                if (LanguageUtils.EqualsResource(ROWTrainingModel.Text, "TrainingResultView.RehabilitationModel"))
                {
                    devicePrescription.Device_mode = DevConstants.REHABILITATION_MODEL;

                    try
                    {
                        devicePrescription.Consequent_force = int.Parse(ROWConsequentForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的顺向力", "Please choose the right consequent force"));
                        return;
                    }
                    try
                    {
                        devicePrescription.Reverse_force = int.Parse(ROWReverseForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的反向力", "Please choose the right reverse force"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(ROWTrainingModel.Text, "TrainingResultView.ActiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.ACTIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(ROWSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.PassiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.PASSIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(ROWSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                // 顺向力
                try
                {
                    devicePrescription.Consequent_force = Convert.ToInt32(ROWRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 反向力
                try
                {
                    devicePrescription.Reverse_force = Convert.ToInt32(ROWRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动速度等级
                try
                {
                    devicePrescription.Speed_rank = Convert.ToInt32(ROWRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                #region 训练结果
                prescriptionResult.Fk_ds_id = (int)DeviceType.P00;
                prescriptionResult.Gmt_create = da;
                prescriptionResult.Gmt_modified = DateTime.Now;
                // 病人感想
                try
                {
                    if (Convert.ToInt32(ROWSportstrength.Text) < 6)
                    {
                        prescriptionResult.pr_userthoughts = "6";
                    }
                    else if (Convert.ToInt32(ROWSportstrength.Text) > 15)
                    {
                        prescriptionResult.pr_userthoughts = "15";
                    }
                    else
                    {
                        prescriptionResult.pr_userthoughts = ROWSportstrength.Text;
                    }
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 功率
                try
                {
                    prescriptionResult.Power = Convert.ToInt32(ROWPower.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 训练总耗能
                try
                {
                    prescriptionResult.Energy = Convert.ToInt32(ROWEnergy.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 心率集合
                try
                {
                    prescriptionResult.Heart_rate_list = ROWHeartRateList.Text;
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动完成个数
                try
                {
                    prescriptionResult.Finish_num = Convert.ToInt32(ROWFinishNum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (TFGroupcount.Text != "")
            {
                // 坐式推胸机
                PrescriptionResultTwo prescriptionResult = new PrescriptionResultTwo();
                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                UserRelationDao userRelationDao = new UserRelationDao();
                UserRelation userRelation = new UserRelation();
                userRelation = userRelationDao.FindUserRelationByuserID(user.Pk_User_Id);
                prescriptionResult.Bind_id = userRelation.Bind_id;
                devicePrescription.Gmt_create = da;
                devicePrescription.Gmt_modified = DateTime.Now;
                #region 处方
                // 组数
                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(TFGroupcount.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 每组数量
                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(TFGroupnum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 间隔时间
                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(TFRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 完成状态
                devicePrescription.Dp_status = 1;
                // 移乘方式
                try
                {
                    devicePrescription.Dp_moveway = int.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, TFMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 关联的单一设备类型ID
                devicePrescription.Fk_ds_id = (int)DeviceType.P09;
                // 注意点
                devicePrescription.Dp_memo = TFAttentionpoint.Text;
                // 训练模式
                if (LanguageUtils.EqualsResource(TFTrainingModel.Text, "TrainingResultView.RehabilitationModel"))
                {
                    devicePrescription.Device_mode = DevConstants.REHABILITATION_MODEL;

                    try
                    {
                        devicePrescription.Consequent_force = int.Parse(TFConsequentForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的顺向力", "Please choose the right consequent force"));
                        return;
                    }
                    try
                    {
                        devicePrescription.Reverse_force = int.Parse(TFReverseForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的反向力", "Please choose the right reverse force"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(TFTrainingModel.Text, "TrainingResultView.ActiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.ACTIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(TFSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.PassiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.PASSIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(TFSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                // 顺向力
                try
                {
                    devicePrescription.Consequent_force = Convert.ToInt32(TFRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 反向力
                try
                {
                    devicePrescription.Reverse_force = Convert.ToInt32(TFRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动速度等级
                try
                {
                    devicePrescription.Speed_rank = Convert.ToInt32(TFRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                #region 训练结果
                prescriptionResult.Fk_ds_id = (int)DeviceType.P09;
                prescriptionResult.Gmt_create = da;
                prescriptionResult.Gmt_modified = DateTime.Now;
                // 病人感想
                try
                {
                    if (Convert.ToInt32(TFSportstrength.Text) < 6)
                    {
                        prescriptionResult.pr_userthoughts = "6";
                    }
                    else if (Convert.ToInt32(TFSportstrength.Text) > 15)
                    {
                        prescriptionResult.pr_userthoughts = "15";
                    }
                    else
                    {
                        prescriptionResult.pr_userthoughts = TFSportstrength.Text;
                    }
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 功率
                try
                {
                    prescriptionResult.Power = Convert.ToInt32(TFPower.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 训练总耗能
                try
                {
                    prescriptionResult.Energy = Convert.ToInt32(TFEnergy.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 心率集合
                try
                {
                    prescriptionResult.Heart_rate_list = TFHeartRateList.Text;
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动完成个数
                try
                {
                    prescriptionResult.Finish_num = Convert.ToInt32(TFFinishNum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (LEGroupcount.Text != "")
            {
                // 坐式推胸机
                PrescriptionResultTwo prescriptionResult = new PrescriptionResultTwo();
                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                UserRelationDao userRelationDao = new UserRelationDao();
                UserRelation userRelation = new UserRelation();
                userRelation = userRelationDao.FindUserRelationByuserID(user.Pk_User_Id);
                prescriptionResult.Bind_id = userRelation.Bind_id;
                devicePrescription.Gmt_create = da;
                devicePrescription.Gmt_modified = DateTime.Now;
                #region 处方
                // 组数
                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(LEGroupcount.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 每组数量
                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(LEGroupnum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 间隔时间
                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(LERelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 完成状态
                devicePrescription.Dp_status = 1;
                // 移乘方式
                try
                {
                    devicePrescription.Dp_moveway = int.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, LEMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 关联的单一设备类型ID
                devicePrescription.Fk_ds_id = (int)DeviceType.P06;
                // 注意点
                devicePrescription.Dp_memo = LEAttentionpoint.Text;
                // 训练模式
                if (LanguageUtils.EqualsResource(LETrainingModel.Text, "TrainingResultView.RehabilitationModel"))
                {
                    devicePrescription.Device_mode = DevConstants.REHABILITATION_MODEL;

                    try
                    {
                        devicePrescription.Consequent_force = int.Parse(LEConsequentForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的顺向力", "Please choose the right consequent force"));
                        return;
                    }
                    try
                    {
                        devicePrescription.Reverse_force = int.Parse(LEReverseForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的反向力", "Please choose the right reverse force"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(LETrainingModel.Text, "TrainingResultView.ActiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.ACTIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(LESpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.PassiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.PASSIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(LESpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                // 顺向力
                try
                {
                    devicePrescription.Consequent_force = Convert.ToInt32(LERelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 反向力
                try
                {
                    devicePrescription.Reverse_force = Convert.ToInt32(LERelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动速度等级
                try
                {
                    devicePrescription.Speed_rank = Convert.ToInt32(LERelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                #region 训练结果
                prescriptionResult.Fk_ds_id = (int)DeviceType.P06;
                prescriptionResult.Gmt_create = da;
                prescriptionResult.Gmt_modified = DateTime.Now;
                // 病人感想
                try
                {
                    if (Convert.ToInt32(LESportstrength.Text) < 6)
                    {
                        prescriptionResult.pr_userthoughts = "6";
                    }
                    else if (Convert.ToInt32(LESportstrength.Text) > 15)
                    {
                        prescriptionResult.pr_userthoughts = "15";
                    }
                    else
                    {
                        prescriptionResult.pr_userthoughts = LESportstrength.Text;
                    }
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 功率
                try
                {
                    prescriptionResult.Power = Convert.ToInt32(LEPower.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 训练总耗能
                try
                {
                    prescriptionResult.Energy = Convert.ToInt32(LEEnergy.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 心率集合
                try
                {
                    prescriptionResult.Heart_rate_list = LEHeartRateList.Text;
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动完成个数
                try
                {
                    prescriptionResult.Finish_num = Convert.ToInt32(LEFinishNum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (HAGroupcount.Text != "")
            {
                // 坐式推胸机
                PrescriptionResultTwo prescriptionResult = new PrescriptionResultTwo();
                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                UserRelationDao userRelationDao = new UserRelationDao();
                UserRelation userRelation = new UserRelation();
                userRelation = userRelationDao.FindUserRelationByuserID(user.Pk_User_Id);
                prescriptionResult.Bind_id = userRelation.Bind_id;
                devicePrescription.Gmt_create = da;
                devicePrescription.Gmt_modified = DateTime.Now;
                #region 处方
                // 组数
                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(HAGroupcount.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 每组数量
                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(HAGroupnum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 间隔时间
                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(HARelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 完成状态
                devicePrescription.Dp_status = 1;
                // 移乘方式
                try
                {
                    devicePrescription.Dp_moveway = int.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, HAMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 关联的单一设备类型ID
                devicePrescription.Fk_ds_id = (int)DeviceType.P02;
                // 注意点
                devicePrescription.Dp_memo = HAAttentionpoint.Text;
                // 训练模式
                if (LanguageUtils.EqualsResource(HATrainingModel.Text, "TrainingResultView.RehabilitationModel"))
                {
                    devicePrescription.Device_mode = DevConstants.REHABILITATION_MODEL;

                    try
                    {
                        devicePrescription.Consequent_force = int.Parse(HAConsequentForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的顺向力", "Please choose the right consequent force"));
                        return;
                    }
                    try
                    {
                        devicePrescription.Reverse_force = int.Parse(HAReverseForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的反向力", "Please choose the right reverse force"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(HATrainingModel.Text, "TrainingResultView.ActiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.ACTIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(HASpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.PassiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.PASSIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(HASpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                // 顺向力
                try
                {
                    devicePrescription.Consequent_force = Convert.ToInt32(HARelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 反向力
                try
                {
                    devicePrescription.Reverse_force = Convert.ToInt32(HARelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动速度等级
                try
                {
                    devicePrescription.Speed_rank = Convert.ToInt32(HARelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                #region 训练结果
                prescriptionResult.Fk_ds_id = (int)DeviceType.P02;
                prescriptionResult.Gmt_create = da;
                prescriptionResult.Gmt_modified = DateTime.Now;
                // 病人感想
                try
                {
                    if (Convert.ToInt32(HASportstrength.Text) < 6)
                    {
                        prescriptionResult.pr_userthoughts = "6";
                    }
                    else if (Convert.ToInt32(HASportstrength.Text) > 15)
                    {
                        prescriptionResult.pr_userthoughts = "15";
                    }
                    else
                    {
                        prescriptionResult.pr_userthoughts = HASportstrength.Text;
                    }
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 功率
                try
                {
                    prescriptionResult.Power = Convert.ToInt32(HAPower.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 训练总耗能
                try
                {
                    prescriptionResult.Energy = Convert.ToInt32(HAEnergy.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 心率集合
                try
                {
                    prescriptionResult.Heart_rate_list = HAHeartRateList.Text;
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动完成个数
                try
                {
                    prescriptionResult.Finish_num = Convert.ToInt32(HAFinishNum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (CPGroupcount.Text != "")
            {
                // 坐式推胸机
                PrescriptionResultTwo prescriptionResult = new PrescriptionResultTwo();
                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                UserRelationDao userRelationDao = new UserRelationDao();
                UserRelation userRelation = new UserRelation();
                userRelation = userRelationDao.FindUserRelationByuserID(user.Pk_User_Id);
                prescriptionResult.Bind_id = userRelation.Bind_id;
                devicePrescription.Gmt_create = da;
                devicePrescription.Gmt_modified = DateTime.Now;
                #region 处方
                // 组数
                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(CPGroupcount.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 每组数量
                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(CPGroupnum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 间隔时间
                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(CPRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 完成状态
                devicePrescription.Dp_status = 1;
                // 移乘方式
                try
                {
                    devicePrescription.Dp_moveway = int.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, CPMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 关联的单一设备类型ID
                devicePrescription.Fk_ds_id = (int)DeviceType.P05;
                // 注意点
                devicePrescription.Dp_memo = CPAttentionpoint.Text;
                // 训练模式
                if (LanguageUtils.EqualsResource(CPTrainingModel.Text, "TrainingResultView.RehabilitationModel"))
                {
                    devicePrescription.Device_mode = DevConstants.REHABILITATION_MODEL;

                    try
                    {
                        devicePrescription.Consequent_force = int.Parse(CPConsequentForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的顺向力", "Please choose the right consequent force"));
                        return;
                    }
                    try
                    {
                        devicePrescription.Reverse_force = int.Parse(CPReverseForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的反向力", "Please choose the right reverse force"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(CPTrainingModel.Text, "TrainingResultView.ActiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.ACTIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(CPSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.PassiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.PASSIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(CPSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                // 顺向力
                try
                {
                    devicePrescription.Consequent_force = Convert.ToInt32(CPRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 反向力
                try
                {
                    devicePrescription.Reverse_force = Convert.ToInt32(CPRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动速度等级
                try
                {
                    devicePrescription.Speed_rank = Convert.ToInt32(CPRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                #region 训练结果
                prescriptionResult.Fk_ds_id = (int)DeviceType.P05;
                prescriptionResult.Gmt_create = da;
                prescriptionResult.Gmt_modified = DateTime.Now;
                // 病人感想
                try
                {
                    if (Convert.ToInt32(CPSportstrength.Text) < 6)
                    {
                        prescriptionResult.pr_userthoughts = "6";
                    }
                    else if (Convert.ToInt32(CPSportstrength.Text) > 15)
                    {
                        prescriptionResult.pr_userthoughts = "15";
                    }
                    else
                    {
                        prescriptionResult.pr_userthoughts = CPSportstrength.Text;
                    }
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 功率
                try
                {
                    prescriptionResult.Power = Convert.ToInt32(CPPower.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 训练总耗能
                try
                {
                    prescriptionResult.Energy = Convert.ToInt32(CPEnergy.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 心率集合
                try
                {
                    prescriptionResult.Heart_rate_list = CPHeartRateList.Text;
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动完成个数
                try
                {
                    prescriptionResult.Finish_num = Convert.ToInt32(CPFinishNum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (NewAGroupcount.Text != "")
            {
                // 坐式推胸机
                PrescriptionResultTwo prescriptionResult = new PrescriptionResultTwo();
                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                UserRelationDao userRelationDao = new UserRelationDao();
                UserRelation userRelation = new UserRelation();
                userRelation = userRelationDao.FindUserRelationByuserID(user.Pk_User_Id);
                prescriptionResult.Bind_id = userRelation.Bind_id;
                devicePrescription.Gmt_create = da;
                devicePrescription.Gmt_modified = DateTime.Now;
                #region 处方
                // 组数
                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(NewAGroupcount.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 每组数量
                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(NewAGroupnum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 间隔时间
                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(NewARelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 完成状态
                devicePrescription.Dp_status = 1;
                // 移乘方式
                try
                {
                    devicePrescription.Dp_moveway = int.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, NewAMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 关联的单一设备类型ID
                devicePrescription.Fk_ds_id = (int)DeviceType.P03;
                // 注意点
                devicePrescription.Dp_memo = NewAAttentionpoint.Text;
                // 训练模式
                if (LanguageUtils.EqualsResource(NewATrainingModel.Text, "TrainingResultView.RehabilitationModel"))
                {
                    devicePrescription.Device_mode = DevConstants.REHABILITATION_MODEL;

                    try
                    {
                        devicePrescription.Consequent_force = int.Parse(NewAConsequentForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的顺向力", "Please choose the right consequent force"));
                        return;
                    }
                    try
                    {
                        devicePrescription.Reverse_force = int.Parse(NewAReverseForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的反向力", "Please choose the right reverse force"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(NewATrainingModel.Text, "TrainingResultView.ActiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.ACTIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(NewASpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.PassiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.PASSIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(NewASpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                // 顺向力
                try
                {
                    devicePrescription.Consequent_force = Convert.ToInt32(NewARelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 反向力
                try
                {
                    devicePrescription.Reverse_force = Convert.ToInt32(NewARelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动速度等级
                try
                {
                    devicePrescription.Speed_rank = Convert.ToInt32(NewARelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                #region 训练结果
                prescriptionResult.Fk_ds_id = (int)DeviceType.P03;
                prescriptionResult.Gmt_create = da;
                prescriptionResult.Gmt_modified = DateTime.Now;
                // 病人感想
                try
                {
                    if (Convert.ToInt32(NewASportstrength.Text) < 6)
                    {
                        prescriptionResult.pr_userthoughts = "6";
                    }
                    else if (Convert.ToInt32(NewASportstrength.Text) > 15)
                    {
                        prescriptionResult.pr_userthoughts = "15";
                    }
                    else
                    {
                        prescriptionResult.pr_userthoughts = NewASportstrength.Text;
                    }
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 功率
                try
                {
                    prescriptionResult.Power = Convert.ToInt32(NewAPower.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 训练总耗能
                try
                {
                    prescriptionResult.Energy = Convert.ToInt32(NewAEnergy.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 心率集合
                try
                {
                    prescriptionResult.Heart_rate_list = NewAHeartRateList.Text;
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动完成个数
                try
                {
                    prescriptionResult.Finish_num = Convert.ToInt32(NewAFinishNum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (NewBGroupcount.Text != "")
            {
                // 坐式推胸机
                PrescriptionResultTwo prescriptionResult = new PrescriptionResultTwo();
                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                UserRelationDao userRelationDao = new UserRelationDao();
                UserRelation userRelation = new UserRelation();
                userRelation = userRelationDao.FindUserRelationByuserID(user.Pk_User_Id);
                prescriptionResult.Bind_id = userRelation.Bind_id;
                devicePrescription.Gmt_create = da;
                devicePrescription.Gmt_modified = DateTime.Now;
                #region 处方
                // 组数
                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(NewBGroupcount.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 每组数量
                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(NewBGroupnum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 间隔时间
                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(NewBRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 完成状态
                devicePrescription.Dp_status = 1;
                // 移乘方式
                try
                {
                    devicePrescription.Dp_moveway = int.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, NewBMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 关联的单一设备类型ID
                devicePrescription.Fk_ds_id = (int)DeviceType.P04;
                // 注意点
                devicePrescription.Dp_memo = NewBAttentionpoint.Text;
                // 训练模式
                if (LanguageUtils.EqualsResource(NewBTrainingModel.Text, "TrainingResultView.RehabilitationModel"))
                {
                    devicePrescription.Device_mode = DevConstants.REHABILITATION_MODEL;

                    try
                    {
                        devicePrescription.Consequent_force = int.Parse(NewBConsequentForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的顺向力", "Please choose the right consequent force"));
                        return;
                    }
                    try
                    {
                        devicePrescription.Reverse_force = int.Parse(NewBReverseForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的反向力", "Please choose the right reverse force"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(NewBTrainingModel.Text, "TrainingResultView.ActiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.ACTIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(NewBSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.PassiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.PASSIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(NewBSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                // 顺向力
                try
                {
                    devicePrescription.Consequent_force = Convert.ToInt32(NewBRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 反向力
                try
                {
                    devicePrescription.Reverse_force = Convert.ToInt32(NewBRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动速度等级
                try
                {
                    devicePrescription.Speed_rank = Convert.ToInt32(NewBRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                #region 训练结果
                prescriptionResult.Fk_ds_id = (int)DeviceType.P04;
                prescriptionResult.Gmt_create = da;
                prescriptionResult.Gmt_modified = DateTime.Now;
                // 病人感想
                try
                {
                    if (Convert.ToInt32(NewBSportstrength.Text) < 6)
                    {
                        prescriptionResult.pr_userthoughts = "6";
                    }
                    else if (Convert.ToInt32(NewBSportstrength.Text) > 15)
                    {
                        prescriptionResult.pr_userthoughts = "15";
                    }
                    else
                    {
                        prescriptionResult.pr_userthoughts = NewBSportstrength.Text;
                    }
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 功率
                try
                {
                    prescriptionResult.Power = Convert.ToInt32(NewBPower.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 训练总耗能
                try
                {
                    prescriptionResult.Energy = Convert.ToInt32(NewBEnergy.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 心率集合
                try
                {
                    prescriptionResult.Heart_rate_list = NewBHeartRateList.Text;
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动完成个数
                try
                {
                    prescriptionResult.Finish_num = Convert.ToInt32(NewBFinishNum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (NewCGroupcount.Text != "")
            {
                // 坐式推胸机
                PrescriptionResultTwo prescriptionResult = new PrescriptionResultTwo();
                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                UserRelationDao userRelationDao = new UserRelationDao();
                UserRelation userRelation = new UserRelation();
                userRelation = userRelationDao.FindUserRelationByuserID(user.Pk_User_Id);
                prescriptionResult.Bind_id = userRelation.Bind_id;
                devicePrescription.Gmt_create = da;
                devicePrescription.Gmt_modified = DateTime.Now;
                #region 处方
                // 组数
                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(NewCGroupcount.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 每组数量
                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(NewCGroupnum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 间隔时间
                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(NewCRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 完成状态
                devicePrescription.Dp_status = 1;
                // 移乘方式
                try
                {
                    devicePrescription.Dp_moveway = int.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, NewCMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 关联的单一设备类型ID
                devicePrescription.Fk_ds_id = (int)DeviceType.P07;
                // 注意点
                devicePrescription.Dp_memo = NewCAttentionpoint.Text;
                // 训练模式
                if (LanguageUtils.EqualsResource(NewCTrainingModel.Text, "TrainingResultView.RehabilitationModel"))
                {
                    devicePrescription.Device_mode = DevConstants.REHABILITATION_MODEL;

                    try
                    {
                        devicePrescription.Consequent_force = int.Parse(NewCConsequentForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的顺向力", "Please choose the right consequent force"));
                        return;
                    }
                    try
                    {
                        devicePrescription.Reverse_force = int.Parse(NewCReverseForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的反向力", "Please choose the right reverse force"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(NewCTrainingModel.Text, "TrainingResultView.ActiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.ACTIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(NewCSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.PassiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.PASSIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(NewCSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                // 顺向力
                try
                {
                    devicePrescription.Consequent_force = Convert.ToInt32(NewCRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 反向力
                try
                {
                    devicePrescription.Reverse_force = Convert.ToInt32(NewCRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动速度等级
                try
                {
                    devicePrescription.Speed_rank = Convert.ToInt32(NewCRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                #region 训练结果
                prescriptionResult.Fk_ds_id = (int)DeviceType.P07;
                prescriptionResult.Gmt_create = da;
                prescriptionResult.Gmt_modified = DateTime.Now;
                // 病人感想
                try
                {
                    if (Convert.ToInt32(NewCSportstrength.Text) < 6)
                    {
                        prescriptionResult.pr_userthoughts = "6";
                    }
                    else if (Convert.ToInt32(NewCSportstrength.Text) > 15)
                    {
                        prescriptionResult.pr_userthoughts = "15";
                    }
                    else
                    {
                        prescriptionResult.pr_userthoughts = NewCSportstrength.Text;
                    }
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 功率
                try
                {
                    prescriptionResult.Power = Convert.ToInt32(NewCPower.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 训练总耗能
                try
                {
                    prescriptionResult.Energy = Convert.ToInt32(NewCEnergy.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 心率集合
                try
                {
                    prescriptionResult.Heart_rate_list = NewCHeartRateList.Text;
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动完成个数
                try
                {
                    prescriptionResult.Finish_num = Convert.ToInt32(NewCFinishNum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                prescription.Add(devicePrescription, prescriptionResult);
            }
            if (NewDGroupcount.Text != "")
            {
                // 坐式推胸机
                PrescriptionResultTwo prescriptionResult = new PrescriptionResultTwo();
                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                UserRelationDao userRelationDao = new UserRelationDao();
                UserRelation userRelation = new UserRelation();
                userRelation = userRelationDao.FindUserRelationByuserID(user.Pk_User_Id);
                prescriptionResult.Bind_id = userRelation.Bind_id;
                devicePrescription.Gmt_create = da;
                devicePrescription.Gmt_modified = DateTime.Now;
                #region 处方
                // 组数
                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(NewDGroupcount.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 每组数量
                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(NewDGroupnum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 间隔时间
                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(NewDRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 完成状态
                devicePrescription.Dp_status = 1;
                // 移乘方式
                try
                {
                    devicePrescription.Dp_moveway = int.Parse(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, NewDMoveway.Text)); //移乘方式
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 关联的单一设备类型ID
                devicePrescription.Fk_ds_id = (int)DeviceType.P08;
                // 注意点
                devicePrescription.Dp_memo = NewDAttentionpoint.Text;
                // 训练模式
                if (LanguageUtils.EqualsResource(NewDTrainingModel.Text, "TrainingResultView.RehabilitationModel"))
                {
                    devicePrescription.Device_mode = DevConstants.REHABILITATION_MODEL;

                    try
                    {
                        devicePrescription.Consequent_force = int.Parse(NewDConsequentForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的顺向力", "Please choose the right consequent force"));
                        return;
                    }
                    try
                    {
                        devicePrescription.Reverse_force = int.Parse(NewDReverseForce.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的反向力", "Please choose the right reverse force"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(NewDTrainingModel.Text, "TrainingResultView.ActiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.ACTIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(NewDSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                else if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingResultView.PassiveModel"))
                {
                    devicePrescription.Device_mode = DevConstants.PASSIVE_MODEL;

                    try
                    {
                        devicePrescription.Speed_rank = int.Parse(NewDSpeedRank.Text);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex);
                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的运动速度等级", "Please choose the right speed rank"));
                        return;
                    }
                }
                // 顺向力
                try
                {
                    devicePrescription.Consequent_force = Convert.ToInt32(NewDRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 反向力
                try
                {
                    devicePrescription.Reverse_force = Convert.ToInt32(NewDRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动速度等级
                try
                {
                    devicePrescription.Speed_rank = Convert.ToInt32(NewDRelaxTime.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                #region 训练结果
                prescriptionResult.Fk_ds_id = (int)DeviceType.P08;
                prescriptionResult.Gmt_create = da;
                prescriptionResult.Gmt_modified = DateTime.Now;
                // 病人感想
                try
                {
                    if (Convert.ToInt32(NewDSportstrength.Text) < 6)
                    {
                        prescriptionResult.pr_userthoughts = "6";
                    }
                    else if (Convert.ToInt32(NewDSportstrength.Text) > 15)
                    {
                        prescriptionResult.pr_userthoughts = "15";
                    }
                    else
                    {
                        prescriptionResult.pr_userthoughts = NewDSportstrength.Text;
                    }
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 功率
                try
                {
                    prescriptionResult.Power = Convert.ToInt32(NewDPower.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 训练总耗能
                try
                {
                    prescriptionResult.Energy = Convert.ToInt32(NewDEnergy.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 心率集合
                try
                {
                    prescriptionResult.Heart_rate_list = NewDHeartRateList.Text;
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                // 运动完成个数
                try
                {
                    prescriptionResult.Finish_num = Convert.ToInt32(NewDFinishNum.Text);
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
                #endregion
                prescription.Add(devicePrescription, prescriptionResult);
            }

            if (prescription.Count == 0)
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("没有输入训练结果", "No input of training results"));
                return;
            }
            if (!string.IsNullOrEmpty(symp.Text))
            {
                // 选择了症状记录，插入训练结果
                new TrainService().AddPrescriptionResultTwo(symp.SelectedValue, trainInfo, prescription);
            }
            else
            {
                // 没有选择了症状记录，插入训练结果
                new TrainService().AddPrescriptionResultTwo(null, trainInfo, prescription);
            }

            //打印
            MessageBoxX.Info(LanguageUtils.ConvertLanguage("已存储", "Finished storage"));
            this.Close();
        }

        private void Load_Data()
        {
            user = (User) DataContext;

            l1.Content = user.User_Name;
            l2.Content = user.Pk_User_Id;

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

            ROWMoveway.ItemsSource = dataItemsMoveWay;

            TFMoveway.ItemsSource = dataItemsMoveWay;

            LEMoveway.ItemsSource = dataItemsMoveWay;

            HAMoveway.ItemsSource = dataItemsMoveWay;

            CPMoveway.ItemsSource = dataItemsMoveWay;

            NewAMoveway.ItemsSource = dataItemsMoveWay;

            NewBMoveway.ItemsSource = dataItemsMoveWay;

            NewCMoveway.ItemsSource = dataItemsMoveWay;

            NewDMoveway.ItemsSource = dataItemsMoveWay;
        }

        /// <summary>
        /// 输入正数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnlyInputNumbers(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");

            e.Handled = re.IsMatch(e.Text);
        }

        /// <summary>
        /// 输入小数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnlyInputDouble(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.]+");

            e.Handled = re.IsMatch(e.Text);
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
                ButtonBase_OnClick(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }

        private void HLPselect_change(object sender, EventArgs e)
        {
            if (LanguageUtils.EqualsResource(HLPTrainingModel.Text, "TrainingListView.RehabilitationModel"))
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
            if (String.IsNullOrEmpty(HLPTrainingModel.Text))
            {
                HLPCF_Label.Background = Brushes.White;
                HLPConsequentForce.Visibility = Visibility.Hidden;
                HLPRF_Label.Background = Brushes.White;
                HLPReverseForce.Visibility = Visibility.Hidden;
                HLPSR_Label.Background = Brushes.White;
                HLPSpeedRank.Visibility = Visibility.Hidden;
            }
        }

        private void ROWselect_change(object sender, EventArgs e)
        {
            if (LanguageUtils.EqualsResource(ROWTrainingModel.Text, "TrainingListView.RehabilitationModel"))
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
            if (String.IsNullOrEmpty(ROWTrainingModel.Text))
            {
                ROWCF_Label.Background = Brushes.White;
                ROWConsequentForce.Visibility = Visibility.Hidden;
                ROWRF_Label.Background = Brushes.White;
                ROWReverseForce.Visibility = Visibility.Hidden;
                ROWSR_Label.Background = Brushes.White;
                ROWSpeedRank.Visibility = Visibility.Hidden;
            }
        }

        private void TFselect_change(object sender, EventArgs e)
        {
            if (LanguageUtils.EqualsResource(TFTrainingModel.Text, "TrainingListView.RehabilitationModel"))
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
            if (String.IsNullOrEmpty(TFTrainingModel.Text))
            {
                TFCF_Label.Background = Brushes.White;
                TFConsequentForce.Visibility = Visibility.Hidden;
                TFRF_Label.Background = Brushes.White;
                TFReverseForce.Visibility = Visibility.Hidden;
                TFSR_Label.Background = Brushes.White;
                TFSpeedRank.Visibility = Visibility.Hidden;
            }
        }

        private void LEselect_change(object sender, EventArgs e)
        {
            if (LanguageUtils.EqualsResource(LETrainingModel.Text, "TrainingListView.RehabilitationModel"))
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
            if (String.IsNullOrEmpty(LETrainingModel.Text))
            {
                LECF_Label.Background = Brushes.White;
                LEConsequentForce.Visibility = Visibility.Hidden;
                LERF_Label.Background = Brushes.White;
                LEReverseForce.Visibility = Visibility.Hidden;
                LESR_Label.Background = Brushes.White;
                LESpeedRank.Visibility = Visibility.Hidden;
            }
        }

        private void HAselect_change(object sender, EventArgs e)
        {
            if (LanguageUtils.EqualsResource(HATrainingModel.Text, "TrainingListView.RehabilitationModel"))
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
            if (String.IsNullOrEmpty(HATrainingModel.Text))
            {
                HACF_Label.Background = Brushes.White;
                HAConsequentForce.Visibility = Visibility.Hidden;
                HARF_Label.Background = Brushes.White;
                HAReverseForce.Visibility = Visibility.Hidden;
                HASR_Label.Background = Brushes.White;
                HASpeedRank.Visibility = Visibility.Hidden;
            }
        }

        private void CPselect_change(object sender, EventArgs e)
        {
            if (LanguageUtils.EqualsResource(CPTrainingModel.Text, "TrainingListView.RehabilitationModel"))
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
            if (String.IsNullOrEmpty(CPTrainingModel.Text))
            {
                CPCF_Label.Background = Brushes.White;
                CPConsequentForce.Visibility = Visibility.Hidden;
                CPRF_Label.Background = Brushes.White;
                CPReverseForce.Visibility = Visibility.Hidden;
                CPSR_Label.Background = Brushes.White;
                CPSpeedRank.Visibility = Visibility.Hidden;
            }
        }

        private void NewAselect_change(object sender, EventArgs e)
        {
            if (LanguageUtils.EqualsResource(NewATrainingModel.Text, "TrainingListView.RehabilitationModel"))
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
            if (String.IsNullOrEmpty(NewATrainingModel.Text))
            {
                NewACF_Label.Background = Brushes.White;
                NewAConsequentForce.Visibility = Visibility.Hidden;
                NewARF_Label.Background = Brushes.White;
                NewAReverseForce.Visibility = Visibility.Hidden;
                NewASR_Label.Background = Brushes.White;
                NewASpeedRank.Visibility = Visibility.Hidden;
            }
        }

        private void NewBselect_change(object sender, EventArgs e)
        {
            if (LanguageUtils.EqualsResource(NewBTrainingModel.Text, "TrainingListView.RehabilitationModel"))
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
            if (String.IsNullOrEmpty(NewBTrainingModel.Text))
            {
                NewBCF_Label.Background = Brushes.White;
                NewBConsequentForce.Visibility = Visibility.Hidden;
                NewBRF_Label.Background = Brushes.White;
                NewBReverseForce.Visibility = Visibility.Hidden;
                NewBSR_Label.Background = Brushes.White;
                NewBSpeedRank.Visibility = Visibility.Hidden;
            }
        }

        private void NewCselect_change(object sender, EventArgs e)
        {
            if (LanguageUtils.EqualsResource(NewCTrainingModel.Text, "TrainingListView.RehabilitationModel"))
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
            if (String.IsNullOrEmpty(NewCTrainingModel.Text))
            {
                NewCCF_Label.Background = Brushes.White;
                NewCConsequentForce.Visibility = Visibility.Hidden;
                NewCRF_Label.Background = Brushes.White;
                NewCReverseForce.Visibility = Visibility.Hidden;
                NewCSR_Label.Background = Brushes.White;
                NewCSpeedRank.Visibility = Visibility.Hidden;
            }
        }

        private void NewDselect_change(object sender, EventArgs e)
        {
            if (LanguageUtils.EqualsResource(NewDTrainingModel.Text, "TrainingListView.RehabilitationModel"))
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
            if (String.IsNullOrEmpty(NewDTrainingModel.Text))
            {
                NewDCF_Label.Background = Brushes.White;
                NewDConsequentForce.Visibility = Visibility.Hidden;
                NewDRF_Label.Background = Brushes.White;
                NewDReverseForce.Visibility = Visibility.Hidden;
                NewDSR_Label.Background = Brushes.White;
                NewDSpeedRank.Visibility = Visibility.Hidden;
            }
        }

        private void viewbox_load(object sender, RoutedEventArgs e)
        {
            this.Width = viewbox.ActualWidth;
            this.Height = viewbox.ActualHeight;

            Left = (SystemParameters.WorkArea.Size.Width - this.ActualWidth) / 2;
            Top = (SystemParameters.WorkArea.Size.Height - this.ActualHeight) / 2;
        }
    }
}
