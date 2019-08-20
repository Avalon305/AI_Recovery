﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using spms.entity.newEntity;
using spms.http.dto;
using spms.service;
using spms.util;
using SymptomInfoDTO = spms.view.dto.SymptomInfoDTO;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// 如果名字长度大于4则是非法
    /// </summary>
    public class NameCheck : ValidationRule
    {
        /// <summary>
        /// 如果名字长度大于4则是非法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var name = Convert.ToString(value);

            //如果名字长度大于4则是非法
            if (name.Length > 8)
                
                return new ValidationResult(false, "内容不能大于8个长度！");

            return ValidationResult.ValidResult;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TextBoxText { 
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Text5 { get; set; }
        public string Text6 { get; set; }
        public string Text7 { get; set; }
        public string Text8 { get; set; }
        public string Text9 { get; set; }
        public string Text10 { get; set; }

    }

    /// <summary>
    /// InputTraining.xaml 的交互逻辑
    /// </summary>
    public partial class InputTraining : Window
    {
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private User user;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // 训练信息的缓存
        //List<DevicePrescription> devicePrescriptionsTmp = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InputTraining()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            Boolean dr = MessageBoxX.Question(LanguageUtils.ConvertLanguage("是否所有编辑都无效？", "Whether all editors are invalid?"));
            if (dr == true)
            {
                this.Close();
            }
        }

        /// <summary>
        /// List增加规则
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type">1为正常，2为联动</param>
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

        private void checkbox1_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_01.Text = null;
            combobox_02.Text = null;
            combobox_03.Text = null;
            combobox_05.Text = null;
            combobox_06.Text = null;
            select_change(sender, e);
            t1.Text = null;
        }

        private void checkbox2_Unchecked(object sender, RoutedEventArgs e)
        {
            //combobox_11.IsEnabled = false;
            combobox_11.Text = null;
            // combobox_12.IsEnabled = false;
            combobox_12.Text = null;
            //combobox_13.IsEnabled = false;
            combobox_13.Text = null;
            //combobox_14.IsEnabled = false;
            combobox_14.Text = null;
            //combobox_15.IsEnabled = false;
            combobox_15.Text = null;
            combobox_16.Text = null;
            select_change2(sender, e);
            combobox_17.Text = null;
            combobox_18.Text = null;
            //com_11.IsEnabled = false;
            com_11.Text = null;
            //com_12.IsEnabled = false;
            //com_13.IsEnabled = false;
            com_13.Text = null;
            //com_14.IsEnabled = false;
            com_14.Text = null;
            t2.Text = null;
        }

        private void checkbox3_Unchecked(object sender, RoutedEventArgs e)
        {
            //combobox_21.IsEnabled = false;
            combobox_21.Text = null;
            //combobox_22.IsEnabled = false;
            combobox_22.Text = null;
            //combobox_23.IsEnabled = false;
            combobox_23.Text = null;
            //combobox_24.IsEnabled = false;
            combobox_24.Text = null;
            //combobox_25.IsEnabled = false;
            combobox_25.Text = null;
            combobox_26.Text = null;
            select_change3(sender, e);
            combobox_27.Text = null;
            combobox_28.Text = null;
            //com_21.IsEnabled = false;
            com_21.Text = null;
            // com_24.IsEnabled = false;
            com_24.Text = null;
            com_24_Copy.Text = null;
            com_24_Copy1.Text = null;
            //com_22.IsEnabled = false;
            // com_23.IsEnabled = false;
            com_23.Text = null;
            t3.Text = null;

        }

        private void checkbox4_Unchecked(object sender, RoutedEventArgs e)
        {
            //combobox_31.IsEnabled = false;

            //combobox_32.IsEnabled = false;
            //combobox_33.IsEnabled = false;
            //combobox_34.IsEnabled = false;
            //combobox_35.IsEnabled = false;
            //com_31.IsEnabled = false;
            //com_34.IsEnabled = false;
            ////com_32.IsEnabled = false;
            //com_33.IsEnabled = false;
            //com_35.IsEnabled = false;
            combobox_31.Text = null;
            combobox_32.Text = null;
            combobox_33.Text = null;

            combobox_34.Text = null;
            combobox_35.Text = null;
            combobox_36.Text = null;
            select_change4(sender, e);
            combobox_37.Text = null;
            combobox_38.Text = null;
            com_31.Text = null;
            com_34.Text = null;
            com_33.Text = null;
            com_35.Text = null;
            com_35_copy.Text = null;
            t4.Text = null;
        }

        private void checkbox5_Unchecked(object sender, RoutedEventArgs e)
        {
            //combobox_41.IsEnabled = false;
            //combobox_42.IsEnabled = false;
            //combobox_43.IsEnabled = false;
            //combobox_44.IsEnabled = false;
            //combobox_45.IsEnabled = false;
            //com_41.IsEnabled = false;
            ////com_42.IsEnabled = false;
            //com_43.IsEnabled = false;
            combobox_41.Text = null;
            combobox_42.Text = null;

            combobox_43.Text = null;
            combobox_44.Text = null;
            combobox_45.Text = null;
            combobox_46.Text = null;
            select_change5(sender, e);
            combobox_47.Text = null;
            combobox_48.Text = null;
            com_41.Text = null;
            com_43.Text = null;
            com_43_Copy2.Text = null;
            com_43_Copy1.Text = null;
            com_43_Copy.Text = null;
            t5.Text = null;
        }

        private void checkbox6_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_51.Text = null;
            combobox_52.Text = null;
            combobox_53.Text = null;
            combobox_54.Text = null;
            combobox_55.Text = null;
            combobox_56.Text = null;
            select_change6(sender, e);
            combobox_57.Text = null;
            combobox_58.Text = null;
            com_51.Text = null;
            com_53.Text = null;
            com_53_Copy.Text = null;
            t6.Text = null;
        }

        private void checkbox7_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_61.Text = null;
            combobox_62.Text = null;
            combobox_63.Text = null;
            combobox_64.Text = null;
            combobox_65.Text = null;
            combobox_66.Text = null;
            select_change7(sender, e);
            combobox_67.Text = null;
            combobox_68.Text = null;
            com_61.Text = null;
            com_63.Text = null;
            com_64.Text = null;
            t7.Text = null;
        }

        private void checkbox8_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_71.Text = null;
            combobox_72.Text = null;
            combobox_73.Text = null;
            combobox_74.Text = null;
            combobox_75.Text = null;
            combobox_76.Text = null;
            select_change8(sender, e);
            combobox_77.Text = null;
            combobox_78.Text = null;
            com_71.Text = null;
            com_73.Text = null;
            com_74.Text = null;
            t8.Text = null;
        }

        private void checkbox9_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_81.Text = null;
            combobox_82.Text = null;
            combobox_83.Text = null;
            combobox_84.Text = null;
            combobox_85.Text = null;
            combobox_86.Text = null;
            select_change9(sender, e);
            combobox_87.Text = null;
            combobox_88.Text = null;
            com_81.Text = null;
            com_83.Text = null;
            com_84.Text = null;
            t9.Text = null;
        }

        private void checkbox10_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_91.Text = null;
            combobox_92.Text = null;
            combobox_93.Text = null;
            combobox_94.Text = null;
            combobox_95.Text = null;
            combobox_96.Text = null;
            select_change10(sender, e);
            combobox_97.Text = null;
            combobox_98.Text = null;
            com_91.Text = null;
            com_93.Text = null;
            t10.Text = null;
        }

        /// <summary>
        /// 点击页面保存触发的方法：1.先缓存界面内容。2.然后存表，状态为save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            //缓存当前页面的数据
            CacheDevicePrescriptions();
            try
            {
                SaveTrainInfo2DB(TrainInfoStatus.Save);
            }
            catch (Exception exception)
            {
                logger.Warn(exception);
                return;
            }
            MessageBoxX.Info(LanguageUtils.ConvertLanguage("已存储", "Finished storage"));
            this.Close();
        }

        //private volatile List<DevicePrescription> devicePrescriptionList;
        private volatile List<NewDevicePrescription> devicePrescriptionList;

        /// <summary>
        /// 得到当前的训练计划，用于save时候的调用，强制进行数据一致性的设置。
        /// </summary>
        /// <returns></returns>
        private List<NewDevicePrescription> GetDevicePrescriptions()
        {
            return this.devicePrescriptionList;
        }

        /// <summary>
        /// 缓存当前的训练计划，在保存与写卡的时候调用
        /// </summary>
        //        private void  CacheDevicePrescriptions(){
        //            //当前页面的临时数据
        //            List<DevicePrescription> devicePrescriptionsTmp = new List<DevicePrescription>();

        //            string devName; //设备名字
        //            string attr1; //属性1
        //            string attr2; //2
        //            string attr3; //3
        //            string attr4; //4
        //            string attr5; //5
        //            DeviceSortDAO deviceSortDao = new DeviceSortDAO();
        //            if (checkbox1.IsChecked == true)
        //            {
        //                //胸部推举机
        //                //attr1 = com_01.Text; //属性1
        //                //attr2 = com_02.Text; //2
        //                attr3 = com_03.Text; //3
        //                attr4 = com_04.Text; //4

        //                //构建对象
        //                DevicePrescription devicePrescription = new DevicePrescription();
        //        devicePrescription.DP_Attrs = //attr1 + "*" +
        //                                              //attr2 + "*" +
        //                                              attr3 + "*" +
        //                                              attr4;
        //                devicePrescription.DP_Memo = t1.Text; //注意点
        //              //  devicePrescription.Fk_DS_Id = (int) DeviceType.X01;
        //        devicePrescription.Gmt_Create = DateTime.Now;
        //                devicePrescription.Gmt_Modified = DateTime.Now;
        //                try
        //                {
        //                    devicePrescription.dp_movedistance = Convert.ToDouble(com_01.Text);
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_01.Text); //组数;
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_02.Text); //个数;
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_05.Text)); //移乘方式
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_03.Text); //间隔时间;
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_weight = Convert.ToDouble(combobox_04.Text); //砝码;
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
        //                    throw e;
        //                }

        //                if (LanguageUtils.EqualsResource(combobox_06.Text, "TrainingListView.Valid"))
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

        //                    try
        //                    {
        //                        devicePrescription.dp_timecount = Byte.Parse(combobox_07.Text);

        //                    }
        //                    catch(Exception e)
        //                    {
        //                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
        //                        throw e;
        //                    }
        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
        //                }
        //                if (LanguageUtils.EqualsResource(combobox_08.Text, "TrainingListView.CountReverse"))
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
        //                }

        //                devicePrescription.Dp_status = 0;
        //                devicePrescriptionsTmp.Add(devicePrescription);
        //            }

        //            if (checkbox2.IsChecked == true)
        //            {
        //                //坐姿划船机
        //                devName = "坐姿划船机";
        //                //attr1 = com_11.Text; //属性1
        //                //attr2 = com_12.Text; //2
        //                attr3 = com_13.Text; //3
        //                attr4 = com_14.Text; //4

        //                //构建对象
        //                DevicePrescription devicePrescription = new DevicePrescription();
        //devicePrescription.DP_Attrs = //attr1 + "*" +
        //                                              //attr2 + "*" +
        //                                              attr3 + "*" +
        //                                              attr4;
        //                devicePrescription.DP_Memo = t2.Text; //注意点
        //               // devicePrescription.Fk_DS_Id = (int) DeviceType.X05;
        //devicePrescription.Gmt_Create = DateTime.Now;
        //                devicePrescription.Gmt_Modified = DateTime.Now;
        //                try
        //                {
        //                    devicePrescription.dp_movedistance = Convert.ToDouble(com_11.Text);
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
        //                    throw e;
        //                }
        //                try
        //                {
        //                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_11.Text); //组数
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_12.Text); //个数
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_15.Text)); //移乘方式
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_13.Text); //间隔时间
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_weight = Convert.ToDouble(combobox_14.Text); //砝码
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
        //                    throw e;
        //                }
        //                if (LanguageUtils.EqualsResource(combobox_16.Text, "TrainingListView.Valid"))
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

        //                    try
        //                    {
        //                        devicePrescription.dp_timecount = Byte.Parse(combobox_17.Text);

        //                    }
        //                    catch (Exception e)
        //                    {
        //                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
        //                        throw e;
        //                    }


        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
        //                }
        //                if (LanguageUtils.EqualsResource(combobox_18.Text, "TrainingListView.CountReverse"))
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
        //                }
        //                devicePrescription.Dp_status = 0;
        //                devicePrescriptionsTmp.Add(devicePrescription);
        //            }

        //            if (checkbox3.IsChecked == true)
        //            {
        //                //身体伸展弯曲机
        //                devName = "身体伸展弯曲机";
        //                //attr1 = com_21.Text; //属性1
        //                //attr2 = com_22.Text; //2
        //                attr3 = com_23.Text; //3
        //                attr4 = com_24.Text; //4
        //                attr1 = com_24_Copy.Text;
        //                attr2 = com_24_Copy1.Text;
        //                //构建对象
        //                DevicePrescription devicePrescription = new DevicePrescription();
        //devicePrescription.DP_Attrs = //attr1 + "*" +
        //                                              //attr2 + "*" +
        //                                              attr3 + "*" +
        //                                              attr4 + "*" +
        //                                              attr1 + "*" +
        //                                              attr2;
        //                devicePrescription.DP_Memo = t3.Text; //注意点
        //              //  devicePrescription.Fk_DS_Id = (int) DeviceType.X04;
        //devicePrescription.Gmt_Create = DateTime.Now;
        //                devicePrescription.Gmt_Modified = DateTime.Now;
        //                try
        //                {
        //                    devicePrescription.dp_movedistance = Convert.ToDouble(com_21.Text);
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
        //                    throw e;
        //                }
        //                try
        //                {
        //                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_21.Text); //组数
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_22.Text); //个数
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_25.Text)); //移乘方式
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_23.Text); //间隔时间
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_weight = Convert.ToDouble(combobox_24.Text); //砝码
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
        //                    throw e;
        //                }
        //                if (LanguageUtils.EqualsResource(combobox_26.Text, "TrainingListView.Valid"))
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

        //                    try
        //                    {
        //                        devicePrescription.dp_timecount = Byte.Parse(combobox_27.Text);

        //                    }
        //                    catch (Exception e)
        //                    {
        //                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
        //                        throw e;
        //                    }


        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
        //                }
        //                if (LanguageUtils.EqualsResource(combobox_28.Text, "TrainingListView.CountReverse"))
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
        //                }
        //                devicePrescription.Dp_status = 0;
        //                devicePrescriptionsTmp.Add(devicePrescription);
        //            }

        //            if (checkbox4.IsChecked == true)
        //            {
        //                //腿部伸展弯曲机
        //                devName = "腿部伸展弯曲机";
        //                //attr1 = com_31.Text; //属性1
        //                //attr2 = com_32.Text; //2
        //                attr3 = com_33.Text; //3
        //                attr4 = com_34.Text; //4
        //                attr5 = com_35.Text; //5
        //                attr1 = com_35_copy.Text;

        //                //构建对象
        //                DevicePrescription devicePrescription = new DevicePrescription();
        //devicePrescription.DP_Attrs = //attr1 + "*" +
        //                                              //attr2 + "*" +
        //                                              attr3 + "*" +
        //                                              attr4 + "*" +
        //                                              attr5 + "*" +
        //                                              attr1;
        //                devicePrescription.DP_Memo = t4.Text; //注意点
        //              //  devicePrescription.Fk_DS_Id = (int) DeviceType.X03;
        //devicePrescription.Gmt_Create = DateTime.Now;
        //                devicePrescription.Gmt_Modified = DateTime.Now;
        //                try
        //                {
        //                    devicePrescription.dp_movedistance = Convert.ToDouble(com_31.Text);
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
        //                    throw e;
        //                }
        //                try
        //                {
        //                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_31.Text); //组数
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_32.Text); //个数
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_35.Text)); //移乘方式
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_33.Text); //间隔时间
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_weight = Convert.ToDouble(combobox_34.Text); //砝码
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
        //                    throw e;
        //                }
        //                if (LanguageUtils.EqualsResource(combobox_36.Text, "TrainingListView.Valid"))
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

        //                    try
        //                    {
        //                        devicePrescription.dp_timecount = Byte.Parse(combobox_37.Text);

        //                    }
        //                    catch (Exception e)
        //                    {
        //                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
        //                        throw e;
        //                    }


        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
        //                }
        //                if (LanguageUtils.EqualsResource(combobox_38.Text, "TrainingListView.CountReverse"))
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
        //                }
        //                devicePrescription.Dp_status = 0;
        //                devicePrescriptionsTmp.Add(devicePrescription);
        //            }

        //            if (checkbox5.IsChecked == true)
        //            {
        //                //腿部腿蹬机
        //                //attr1 = com_41.Text; //属性1
        //                //attr2 = com_42.Text; //2
        //                attr3 = com_43.Text; //3
        //                attr1 = com_43_Copy.Text; //3
        //                attr2 = com_43_Copy1.Text; //3
        //                attr4 = com_43_Copy2.Text; //3

        //                //构建对象
        //                DevicePrescription devicePrescription = new DevicePrescription();
        //devicePrescription.DP_Attrs = //attr1 + "*" +
        //                                              //attr2 + "*" +
        //                                              attr3 + "*" +
        //                                              attr1 + "*" +
        //                                              attr2 + "*" +
        //                                              attr4;
        //                devicePrescription.DP_Memo = t5.Text; //注意点
        //              //  devicePrescription.Fk_DS_Id = (int) DeviceType.X06;
        //devicePrescription.Gmt_Create = DateTime.Now;
        //                devicePrescription.Gmt_Modified = DateTime.Now;
        //                try
        //                {
        //                    devicePrescription.dp_movedistance = Convert.ToDouble(com_41.Text);
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
        //                    throw e;
        //                }
        //                try
        //                {
        //                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_41.Text); //组数
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_42.Text); //个数
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_45.Text)); //移乘方式
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_43.Text); //间隔时间
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_weight = Convert.ToDouble(combobox_44.Text); //砝码
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
        //                    throw e;
        //                }
        //                if (LanguageUtils.EqualsResource(combobox_46.Text, "TrainingListView.Valid"))
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

        //                    try
        //                    {
        //                        devicePrescription.dp_timecount =Byte.Parse(combobox_47.Text);

        //                    }
        //                    catch (Exception e)
        //                    {
        //                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
        //                        throw e;
        //                    }
        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
        //                }
        //                if (LanguageUtils.EqualsResource(combobox_48.Text, "TrainingListView.CountReverse"))
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
        //                }
        //                devicePrescription.Dp_status = 0;
        //                devicePrescriptionsTmp.Add(devicePrescription);
        //            }

        //            if (checkbox6.IsChecked == true)
        //            {
        //                //腿部内外弯机
        //                //attr1 = com_51.Text; //属性1
        //                //attr2 = com_52.Text; //2
        //                attr3 = com_53.Text; //3
        //                attr4 = com_53_Copy.Text; //3

        //                //构建对象
        //                DevicePrescription devicePrescription = new DevicePrescription();
        //devicePrescription.DP_Attrs = //attr1 + "*" +
        //                                              //attr2 + "*" +
        //                                              attr3 + "*" +
        //                                              attr4;
        //                devicePrescription.DP_Memo = t6.Text; //注意点
        //               // devicePrescription.Fk_DS_Id = (int) DeviceType.X02;
        //devicePrescription.Gmt_Create = DateTime.Now;
        //                devicePrescription.Gmt_Modified = DateTime.Now;
        //                try
        //                {
        //                    devicePrescription.dp_movedistance = Convert.ToDouble(com_51.Text);
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
        //                    throw e;
        //                }
        //                try
        //                {
        //                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_51.Text); //组数
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_52.Text); //个数
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_55.Text)); //移乘方式
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_53.Text); //间隔时间
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
        //                    throw e;
        //                }

        //                try
        //                {
        //                    devicePrescription.dp_weight = Convert.ToDouble(combobox_54.Text); //砝码
        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
        //                    throw e;
        //                }
        //                if (LanguageUtils.EqualsResource(combobox_56.Text, "TrainingListView.Valid"))
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

        //                    try
        //                    {
        //                        devicePrescription.dp_timecount = Byte.Parse(combobox_57.Text);

        //                    }
        //                    catch (Exception e)
        //                    {
        //                        MessageBoxX.Info(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
        //                        throw e;
        //                    }
        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
        //                }
        //                if (LanguageUtils.EqualsResource(combobox_58.Text, "TrainingListView.CountReverse"))
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
        //                }
        //                else
        //                {
        //                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
        //                }
        //                devicePrescription.Dp_status = 0;
        //                devicePrescriptionsTmp.Add(devicePrescription);
        //            }
        //            //如果还没有赋值，则赋值
        //            if (this.devicePrescriptionList == null) {
        //                this.devicePrescriptionList = new List<DevicePrescription>();
        //            }
        //            //移除所有元素，保险起见
        //            this.devicePrescriptionList.Clear();
        //            //复制当前元素，而不是简单的内存指向
        //            devicePrescriptionsTmp.ForEach(i => this.devicePrescriptionList.Add(i));


        //        }
        private void CacheDevicePrescriptions()
        {
            //当前页面的临时数据
            List<NewDevicePrescription> devicePrescriptionsTmp = new List<NewDevicePrescription>();

            string devName; //设备名字

            DeviceSortDAO deviceSortDao = new DeviceSortDAO();
            // 坐式推胸机
            if (checkbox1.IsChecked == true)
            {
                devName = "坐式推胸机";

                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                devicePrescription.Dp_memo = t1.Text; //注意点
                                                      //  devicePrescription.Fk_DS_Id = (int) DeviceType.X01;
                devicePrescription.Gmt_create = DateTime.Now;
                devicePrescription.Gmt_modified = DateTime.Now;

                // 移动距离
                // ...

                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(combobox_01.Text); //组数;
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(combobox_02.Text); //个数;
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_05.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(combobox_03.Text); //间隔时间;
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                // 砝码
                // ...
                // 计时器
                // ...

                devicePrescription.Dp_status = 0;
                devicePrescriptionsTmp.Add(devicePrescription);
            }
            // 坐姿划船机
            if (checkbox2.IsChecked == true)
            {
                //坐姿划船机
                devName = "坐姿划船机";

                NewDevicePrescription devicePrescription = new NewDevicePrescription();
                devicePrescription.Dp_memo = t2.Text; //注意点
                devicePrescription.Gmt_create = DateTime.Now;
                devicePrescription.Gmt_modified = DateTime.Now;

                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(combobox_11.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(combobox_12.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_15.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(combobox_13.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                devicePrescription.Dp_status = 0;
                devicePrescriptionsTmp.Add(devicePrescription);
            }
            // 坐式背部伸展机
            if (checkbox3.IsChecked == true)
            {
                devName = "坐式背部伸展机";
                NewDevicePrescription devicePrescription = new NewDevicePrescription();

                devicePrescription.Dp_memo = t3.Text; //注意点
                devicePrescription.Gmt_create = DateTime.Now;
                devicePrescription.Gmt_modified = DateTime.Now;

                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(combobox_21.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(combobox_22.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_25.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(combobox_23.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                devicePrescription.Dp_status = 0;
                devicePrescriptionsTmp.Add(devicePrescription);
            }
            // 腿部内弯机
            if (checkbox4.IsChecked == true)
            {
                devName = "腿部内弯机";
                NewDevicePrescription devicePrescription = new NewDevicePrescription();

                devicePrescription.Dp_memo = t4.Text; //注意点
                                                      //  devicePrescription.Fk_DS_Id = (int) DeviceType.X03;
                devicePrescription.Gmt_create = DateTime.Now;
                devicePrescription.Gmt_modified = DateTime.Now;

                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(combobox_31.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(combobox_32.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_35.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(combobox_33.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                devicePrescription.Dp_status = 0;
                devicePrescriptionsTmp.Add(devicePrescription);
            }
            // 腿部推蹬机
            if (checkbox5.IsChecked == true)
            {
                // 腿部推蹬机
                devName = "腿部推蹬机";
                NewDevicePrescription devicePrescription = new NewDevicePrescription();

                devicePrescription.Dp_memo = t5.Text; //注意点
                devicePrescription.Gmt_create = DateTime.Now;
                devicePrescription.Gmt_modified = DateTime.Now;

                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(combobox_41.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(combobox_42.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_45.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                devicePrescription.Dp_status = 0;
                devicePrescriptionsTmp.Add(devicePrescription);
            }
            // 腿部外弯机
            if (checkbox6.IsChecked == true)
            {
                devName = "腿部外弯机";
                NewDevicePrescription devicePrescription = new NewDevicePrescription();

                devicePrescription.Dp_memo = t6.Text; //注意点
                devicePrescription.Gmt_create = DateTime.Now;
                devicePrescription.Gmt_modified = DateTime.Now;

                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(combobox_51.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(combobox_52.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_55.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(combobox_53.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                devicePrescription.Dp_status = 0;
                devicePrescriptionsTmp.Add(devicePrescription);
            }
            // 腹肌训练机
            if (checkbox7.IsChecked == true)
            {
                devName = "腹肌训练机";
                NewDevicePrescription devicePrescription = new NewDevicePrescription();

                devicePrescription.Dp_memo = t7.Text; //注意点
                devicePrescription.Gmt_create = DateTime.Now;
                devicePrescription.Gmt_modified = DateTime.Now;

                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(combobox_61.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(combobox_62.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_65.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(combobox_63.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                devicePrescription.Dp_status = 0;
                devicePrescriptionsTmp.Add(devicePrescription);
            }
            // 三头肌训练机
            if (checkbox8.IsChecked == true)
            {
                devName = "三头训练机";
                NewDevicePrescription devicePrescription = new NewDevicePrescription();

                devicePrescription.Dp_memo = t8.Text; //注意点
                devicePrescription.Gmt_create = DateTime.Now;
                devicePrescription.Gmt_modified = DateTime.Now;

                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(combobox_71.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(combobox_72.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_75.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(combobox_73.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                devicePrescription.Dp_status = 0;
                devicePrescriptionsTmp.Add(devicePrescription);
            }
            // 蝴蝶机
            if (checkbox9.IsChecked == true)
            {
                devName = "蝴蝶机";
                NewDevicePrescription devicePrescription = new NewDevicePrescription();

                devicePrescription.Dp_memo = t9.Text; //注意点
                devicePrescription.Gmt_create = DateTime.Now;
                devicePrescription.Gmt_modified = DateTime.Now;

                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(combobox_81.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(combobox_82.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_85.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(combobox_83.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                devicePrescription.Dp_status = 0;
                devicePrescriptionsTmp.Add(devicePrescription);
            }
            // 反向蝴蝶机
            if (checkbox10.IsChecked == true)
            {
                devName = "反向蝴蝶机";
                NewDevicePrescription devicePrescription = new NewDevicePrescription();

                devicePrescription.Dp_memo = t10.Text; //注意点
                devicePrescription.Gmt_create = DateTime.Now;
                devicePrescription.Gmt_modified = DateTime.Now;

                try
                {
                    devicePrescription.Dp_groupcount = Convert.ToInt32(combobox_91.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_groupnum = Convert.ToInt32(combobox_92.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_95.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.Dp_relaxtime = Convert.ToInt32(combobox_93.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                devicePrescription.Dp_status = 0;
                devicePrescriptionsTmp.Add(devicePrescription);
            }

            //如果还没有赋值，则赋值
            if (this.devicePrescriptionList == null)
            {
                this.devicePrescriptionList = new List<NewDevicePrescription>();
            }
            //移除所有元素，保险起见
            this.devicePrescriptionList.Clear();
            //复制当前元素，而不是简单的内存指向
            devicePrescriptionsTmp.ForEach(i => this.devicePrescriptionList.Add(i));
        }

        /// <summary>
        /// 症状记录
        /// </summary>
        /// <param name="status"></param>
        private void SaveTrainInfo2DB(TrainInfoStatus status)
        {

            //将点击保存与写卡时候的一瞬间的截面数据获取
            //List<DevicePrescription> devicePrescriptions = GetDevicePrescriptions();
            //devicePrescriptionsTmp = GetDevicePrescriptions();
            //TrainInfo trainInfoTmp = null;
            TrainInfo trainInfoTmp = new TrainInfo();
            trainInfoTmp.Gmt_Create = DateTime.Now;
            trainInfoTmp.Gmt_Modified = DateTime.Now;
            trainInfoTmp.FK_User_Id = user.Pk_User_Id;
            trainInfoTmp.Status = (int) status;
            if (!string.IsNullOrEmpty(symp.Text))
            {
                //如果选择了症状记录
                new TrainService().SaveTraininfo(symp.SelectedValue, trainInfoTmp, GetDevicePrescriptions());
            }
            else
            {
                //存储到数据库
                new TrainService().SaveTraininfo(null, trainInfoTmp, GetDevicePrescriptions());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetAlignment();
            //去除窗体叉号

            this.Width = SystemParameters.WorkArea.Size.Width;
            this.Height = SystemParameters.WorkArea.Size.Height;
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            //确定哪些设备可用
            Certain_Dev();
           
            
            
            //
            user = (User) DataContext;
            List<DataCode> dataCodes = DataCodeCache.GetInstance().GetDateCodeList(DataCodeTypeEnum.MoveWay);
            List<string> dataItems = new List<string>();
            if (dataCodes != null)
            {
                foreach (var dataCode in dataCodes)
                {
                    dataItems.Add(dataCode.Code_D_Value);
                }
            }
            
            l1.Content = user.User_Name;
            l2.Content = user.Pk_User_Id;
            var nullTiIdByUserId = new SymptomInfoDao().GetNullTiIdByUserId(user.Pk_User_Id);
            symp.ItemsSource = new SymptomInfoDTO().ConvertDtoList(nullTiIdByUserId);

            combobox_01.ItemsSource = Add(1, 3, 2);
            combobox_02.ItemsSource = Add(1, 20, 2);
            combobox_03.ItemsSource = Add(1, 60, 2);
            //combobox_04.ItemsSource = Add(0.5, 30.0, 1);
            combobox_05.ItemsSource = dataItems;

            com_11.ItemsSource = Add(1, 43, 2);
            //com_12.ItemsSource = Add(0, 30, 1);
            com_13.ItemsSource = Add(1, 8, 2);
            com_14.ItemsSource = Add(1, 4, 2);
            combobox_11.ItemsSource = Add(1, 3, 2);
            combobox_12.ItemsSource = Add(1, 20, 2);
            combobox_13.ItemsSource = Add(1, 60, 2);
            combobox_14.ItemsSource = Add(0.5, 30.0, 1);
            combobox_15.ItemsSource = dataItems;

            com_21.ItemsSource = Add(1, 47, 2);
            //com_22.ItemsSource = Add(0, 30, 1);
            com_23.ItemsSource = Add(1, 6, 2);
            com_24.ItemsSource = Add(1, 3, 2);
            com_24_Copy.ItemsSource = Add(1, 4, 2);
            com_24_Copy1.ItemsSource = Add(46, 60, 2);
            combobox_21.ItemsSource = Add(1, 3, 2);
            combobox_22.ItemsSource = Add(1, 20, 2);
            combobox_23.ItemsSource = Add(1, 60, 2);
            combobox_24.ItemsSource = Add(0.5, 30.0, 1);
            combobox_25.ItemsSource = dataItems;

            com_31.ItemsSource = Add(1, 47, 2);
            //com_32.ItemsSource = Add(0, 30, 1);
            com_33.ItemsSource = Add(1, 5, 2);
            com_34.ItemsSource = Add(1, 6, 2);
            com_35.ItemsSource = Add(1, 5, 2);
            com_35_copy.ItemsSource = Add(1, 2, 2);
            combobox_31.ItemsSource = Add(1, 3, 2);
            combobox_32.ItemsSource = Add(1, 20, 2);
            combobox_33.ItemsSource = Add(1, 60, 2);
            combobox_34.ItemsSource = Add(0.5, 30.0, 1);
            combobox_35.ItemsSource = dataItems;

            com_41.ItemsSource = Add(1, 50, 2);
            //com_42.ItemsSource = Add(0, 30, 1);
            com_43.ItemsSource = Add(1, 6, 2);
            com_43_Copy.ItemsSource = Add(1, 5, 2);
            com_43_Copy1.ItemsSource = Add(1, 2, 2);
            com_43_Copy2.ItemsSource = Add(1, 3, 2);
            combobox_41.ItemsSource = Add(1, 3, 2);
            combobox_42.ItemsSource = Add(1, 20, 2);
            combobox_43.ItemsSource = Add(1, 60, 2);
            combobox_44.ItemsSource = Add(0.5, 75.0, 1);
            combobox_45.ItemsSource = dataItems;

            com_51.ItemsSource = Add(1, 20, 2);
            //com_52.ItemsSource = Add(0, 30, 1);
            com_53.ItemsSource = Add(1, 4, 2);
            com_53_Copy.ItemsSource = Add(0, 30, 2);
            combobox_51.ItemsSource = Add(1, 3, 2);
            combobox_52.ItemsSource = Add(1, 20, 2);
            combobox_53.ItemsSource = Add(1, 60, 2);
            combobox_54.ItemsSource = Add(0.5, 30.0, 1);
            combobox_55.ItemsSource = dataItems;

            com_61.ItemsSource = Add(1, 43, 2);
            //com_12.ItemsSource = Add(0, 30, 1);
            com_63.ItemsSource = Add(1, 8, 2);
            com_64.ItemsSource = Add(1, 4, 2);
            combobox_61.ItemsSource = Add(1, 3, 2);
            combobox_62.ItemsSource = Add(1, 20, 2);
            combobox_63.ItemsSource = Add(1, 60, 2);
            combobox_64.ItemsSource = Add(0.5, 30.0, 1);
            combobox_65.ItemsSource = dataItems;

            com_71.ItemsSource = Add(1, 47, 2);
            //com_22.ItemsSource = Add(0, 30, 1);
            com_73.ItemsSource = Add(1, 6, 2);
            com_74.ItemsSource = Add(1, 3, 2);
            com_74_Copy.ItemsSource = Add(1, 4, 2);
            com_74_Copy1.ItemsSource = Add(46, 60, 2);
            combobox_71.ItemsSource = Add(1, 3, 2);
            combobox_72.ItemsSource = Add(1, 20, 2);
            combobox_73.ItemsSource = Add(1, 60, 2);
            combobox_74.ItemsSource = Add(0.5, 30.0, 1);
            combobox_75.ItemsSource = dataItems;

            com_81.ItemsSource = Add(1, 47, 2);
            //com_32.ItemsSource = Add(0, 30, 1);
            com_83.ItemsSource = Add(1, 5, 2);
            com_84.ItemsSource = Add(1, 6, 2);
            com_85.ItemsSource = Add(1, 5, 2);
            com_85_copy.ItemsSource = Add(1, 2, 2);
            combobox_81.ItemsSource = Add(1, 3, 2);
            combobox_82.ItemsSource = Add(1, 20, 2);
            combobox_83.ItemsSource = Add(1, 60, 2);
            combobox_84.ItemsSource = Add(0.5, 30.0, 1);
            combobox_85.ItemsSource = dataItems;

            com_91.ItemsSource = Add(1, 50, 2);
            //com_42.ItemsSource = Add(0, 30, 1);
            com_93.ItemsSource = Add(1, 6, 2);
            com_93_Copy.ItemsSource = Add(1, 5, 2);
            com_93_Copy1.ItemsSource = Add(1, 2, 2);
            com_93_Copy2.ItemsSource = Add(1, 3, 2);
            combobox_91.ItemsSource = Add(1, 3, 2);
            combobox_92.ItemsSource = Add(1, 20, 2);
            combobox_93.ItemsSource = Add(1, 60, 2);
            combobox_94.ItemsSource = Add(0.5, 75.0, 1);
            combobox_95.ItemsSource = dataItems;

            List<NewDevicePrescription> devicePrescriptions = new TrainService().GetSaveDevicePrescriptionsByUser(user);
            if (devicePrescriptions == null)
            {
                return;
            }
            foreach (NewDevicePrescription devicePrescription in devicePrescriptions)
            {
                long devName = devicePrescription.Fk_ds_id;
                switch (devName)
                {
                    case (int)DeviceType.P01:
                        checkbox1.IsChecked = true;
                        //设置处方信息
                        combobox_01.Text = devicePrescription.Dp_groupcount.ToString();
                        combobox_02.Text = devicePrescription.Dp_groupnum.ToString();
                        combobox_03.Text = devicePrescription.Dp_relaxtime.ToString();
                        combobox_05.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.Dp_moveway.ToString());
                        t1.Text = devicePrescription.Dp_memo;
                        combobox_06.Text = "0";
                        break;
                    case (int)DeviceType.P00:
                        checkbox2.IsChecked = true;
                        combobox_11.Text = devicePrescription.Dp_groupcount.ToString();
                        combobox_12.Text = devicePrescription.Dp_groupnum.ToString();
                        combobox_13.Text = devicePrescription.Dp_relaxtime.ToString();
                        combobox_15.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.Dp_moveway.ToString());
                        t2.Text = devicePrescription.Dp_memo;
                        combobox_16.Text = "0";
                        break;
                    case (int)DeviceType.P09:
                        checkbox3.IsChecked = true;
                        combobox_21.Text = devicePrescription.Dp_groupcount.ToString();
                        combobox_22.Text = devicePrescription.Dp_groupnum.ToString();
                        combobox_23.Text = devicePrescription.Dp_relaxtime.ToString();
                        combobox_25.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.Dp_moveway.ToString());
                        t3.Text = devicePrescription.Dp_memo;
                        combobox_26.Text = "0";
                        break;
                    case (int)DeviceType.P06:
                        checkbox4.IsChecked = true;
                        combobox_31.Text = devicePrescription.Dp_groupcount.ToString();
                        combobox_32.Text = devicePrescription.Dp_groupnum.ToString();
                        combobox_33.Text = devicePrescription.Dp_relaxtime.ToString();
                        combobox_35.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.Dp_moveway.ToString());
                        t4.Text = devicePrescription.Dp_memo;
                        combobox_36.Text = "0";
                        break;
                    case (int)DeviceType.P02:
                        checkbox5.IsChecked = true;
                        combobox_41.Text = devicePrescription.Dp_groupcount.ToString();
                        combobox_42.Text = devicePrescription.Dp_groupnum.ToString();
                        combobox_43.Text = devicePrescription.Dp_relaxtime.ToString();
                        combobox_45.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.Dp_moveway.ToString());
                        t5.Text = devicePrescription.Dp_memo;
                        combobox_46.Text = "0";
                        break;
                    case (int)DeviceType.P05:
                        checkbox6.IsChecked = true;
                        combobox_51.Text = devicePrescription.Dp_groupcount.ToString();
                        combobox_52.Text = devicePrescription.Dp_groupnum.ToString();
                        combobox_53.Text = devicePrescription.Dp_relaxtime.ToString();
                        combobox_55.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.Dp_moveway.ToString());
                        t6.Text = devicePrescription.Dp_memo;
                        combobox_56.Text = "0";
                        break;
                    case (int)DeviceType.P03:
                        checkbox7.IsChecked = true;
                        combobox_61.Text = devicePrescription.Dp_groupcount.ToString();
                        combobox_62.Text = devicePrescription.Dp_groupnum.ToString();
                        combobox_63.Text = devicePrescription.Dp_relaxtime.ToString();
                        combobox_65.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.Dp_moveway.ToString());
                        t7.Text = devicePrescription.Dp_memo;
                        combobox_66.Text = "0";
                        break;
                    case (int)DeviceType.P04:
                        checkbox8.IsChecked = true;
                        combobox_71.Text = devicePrescription.Dp_groupcount.ToString();
                        combobox_72.Text = devicePrescription.Dp_groupnum.ToString();
                        combobox_73.Text = devicePrescription.Dp_relaxtime.ToString();
                        combobox_75.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.Dp_moveway.ToString());
                        t8.Text = devicePrescription.Dp_memo;
                        combobox_76.Text = "0";
                        break;
                    case (int)DeviceType.P07:
                        checkbox9.IsChecked = true;
                        combobox_81.Text = devicePrescription.Dp_groupcount.ToString();
                        combobox_82.Text = devicePrescription.Dp_groupnum.ToString();
                        combobox_83.Text = devicePrescription.Dp_relaxtime.ToString();
                        combobox_85.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.Dp_moveway.ToString());
                        t9.Text = devicePrescription.Dp_memo;
                        combobox_86.Text = "0";
                        break;
                    case (int)DeviceType.P08:
                        checkbox10.IsChecked = true;
                        combobox_91.Text = devicePrescription.Dp_groupcount.ToString();
                        combobox_92.Text = devicePrescription.Dp_groupnum.ToString();
                        combobox_93.Text = devicePrescription.Dp_relaxtime.ToString();
                        combobox_95.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.Dp_moveway.ToString());
                        t10.Text = devicePrescription.Dp_memo;
                        combobox_96.Text = "0";
                        break;
                }

            }
        }

        /// <summary>
        /// 确定哪些设备可用
        /// </summary>
        private void Certain_Dev()
        {
            var devs = new DeviceSortDAO().ListAll();
            foreach(DeviceSort dev in devs)
            {
                if(dev.DS_Status == 1)
                {
                    continue;
                }

                if (LanguageUtils.EqualsResource(dev.DS_name, "NewDev.Rowing"))
                {
                    checkbox2.IsChecked = false;
                    checkbox2.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "NewDev.ChestPress"))
                {
                    checkbox1.IsChecked = false;
                    checkbox1.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "NewDev.HorizontalLegPress"))
                {
                    checkbox5.IsChecked = false;
                    checkbox5.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "NewDev.AbdominalMuscleTraining"))
                {
                    checkbox7.IsChecked = false;
                    checkbox7.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "NewDev.TricepsTraining"))
                {
                    checkbox8.IsChecked = false;
                    checkbox8.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "NewDev.LegAbduction"))
                {
                    checkbox6.IsChecked = false;
                    checkbox6.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "NewDev.LegInturn"))
                {
                    checkbox4.IsChecked = false;
                    checkbox4.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "NewDev.ButterflyMachine"))
                {
                    checkbox9.IsChecked = false;
                    checkbox9.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "NewDev.ReverseButterflyMachine"))
                {
                    checkbox10.IsChecked = false;
                    checkbox10.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "NewDev.SittingBackExtender"))
                {
                    checkbox3.IsChecked = false;
                    checkbox3.IsEnabled = false;
                }
            }
        }

        //定时任务
        Timer threadTimer;
        int times = 0;//发送次数
        private static bool isReceive = false;//是否收到回执
        private SerialPort serialPort;
 
        /// <summary>
        /// 点击写卡时触发的方法
        /// 对于数据库内容的操作：1.先把当前用户的所有暂存状态的数据置为废弃。2.往数据库里面插入一条暂存状态的训练信息。3.等收到写卡成功的反馈之后，把数据库中该用户的唯一一条暂存状态的数据置为nomal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            

            try
            {
                //写卡
                TrainInfo trainInfo = new TrainService().GetTrainInfoByUserIdAndStatus(user.Pk_User_Id, (int)TrainInfoStatus.Normal);
                if (trainInfo != null)
                {
                   
                    if (!MessageBoxX.Question(LanguageUtils.ConvertLanguage("是否覆盖用户" + user.User_Name + "的已有训练计划？", "Whether or not to cover?")))
                    {
                        return;
                    }
                }

                //触发写卡之前，缓存界面数据的方法，保证当前对象存储的是最新的界面数据
                CacheDevicePrescriptions();

                //new TrainService().AbandonAllTempTrainInfo(user.Pk_User_Id);
                // 将当前训练信息存入数据库表，此时是暂存状态，在此之前设置该用户所有暂存状态的数据为废弃。此过程在servie中实现
                SaveTrainInfo2DB(TrainInfoStatus.Temp);



                if (user != null)
                {
                    byte[] data = new byte[90];
                    //用户id
                    string str = new UserService().getUserByUserId(user.Pk_User_Id).User_IDCard + "";
                    byte[] idBytes = Encoding.ASCII.GetBytes(str);
                    Array.Copy(idBytes, 0, data, 0, idBytes.Length);

                    //用户名
                    byte[] nameBytes = Encoding.GetEncoding("GBK").GetBytes(user.User_Name);
                    Array.Copy(nameBytes, 0, data, 32, nameBytes.Length);
                    //拼音
                    byte[] pingYinBytes = Encoding.ASCII.GetBytes(user.User_Namepinyin);
                    Array.Copy(pingYinBytes, 0, data, 52, pingYinBytes.Length);

                    int position = 84;
                    //设备
                    if (checkbox1.IsChecked == true)
                    {
                        //胸部推举机0x01
                        data[position] = (byte)DeviceType.P00;
                        position += 1;
                    }

                    if (checkbox2.IsChecked == true)
                    {
                        //坐姿划船机 0x05
                        data[position] = (byte)DeviceType.P01;
                        position += 1;
                    }

                    if (checkbox3.IsChecked == true)
                    {
                        //身体伸展弯曲机 0x04
                        //data[position] = (byte)DeviceType.X04;
                        position += 1;
                    }

                    if (checkbox4.IsChecked == true)
                    {
                        //腿部伸展弯曲机 0x03
                       // data[position] = (byte)DeviceType.X03;
                        position += 1;
                    }

                    if (checkbox5.IsChecked == true)
                    {
                        //胸部推举机 0x06
                        //data[position] = (byte)DeviceType.X06;
                        position += 1;
                    }

                    if (checkbox6.IsChecked == true)
                    {
                        //腿部内外弯机 0x02
                       // data[position] = (byte)DeviceType.X02;
                    }

                    //Console.WriteLine("发卡的内容：" + ProtocolUtil.ByteToStringOk(data));
                    logger.Debug("发卡的内容：" + ProtocolUtil.ByteToStringOk(data));

                    byte[] send = ProtocolUtil.packHairpinData(0x01, data);

                    //检查当前是否有多个串口
                    if (SerialPortUtil.SerialPort == null)
                    {
                        string oldPort = CommUtil.GetSettingString("port");
                        if (CommUtil.GetSettingString("port") == null || CommUtil.GetSettingString("port") == "")
                        {
                            SerialPortUtil.CheckPort();
                        }
                        else
                        {
                            SerialPortUtil.portName = oldPort;
                        }
                    }
                    else
                    {
                        SerialPortUtil.portName = SerialPortUtil.SerialPort.PortName;
                    }

                    if (SerialPortUtil.portName == "")
                    {
                        //MessageBox.Show(LanguageUtils.ConvertLanguage("请先连接串口", "Please Connect the serial port"));
                        MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请先连接串口", "Please Connect the serial port"));
                        return;
                    }

                    if (serialPort == null)
                    {
                        serialPort = SerialPortUtil.ConnectSerialPort(OnPortDataReceived);
                        try
                        {
                            if (!serialPort.IsOpen)
                            {
                                serialPort.Open();
                            }
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            MessageBoxX.Warning(LanguageUtils.ConvertLanguage("串口被占用", "Serial port is occupied"));
                            //清空缓存
                            SerialPortUtil.SerialPort = null;
                            serialPort = null;
                            CommUtil.UpdateSettingString("port", "");
                            return;
                        }
                        catch (IOException ex)
                        {
                            MessageBoxX.Warning(LanguageUtils.ConvertLanguage("串口不存在", "Serial port does not exist"));
                            SerialPortUtil.SerialPort = null;
                            serialPort = null;
                            // 串口不存在后，重新选择
                            SerialPortUtil.CheckPort();
                            return;
                        }
                    }
                    else
                    {
                        if (!serialPort.IsOpen)
                        {
                            try
                            {
                                serialPort.Open();
                            }
                            catch (UnauthorizedAccessException ex)
                            {
                                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("串口被占用", "Serial port is occupied"));
                                CommUtil.UpdateSettingString("port", "");
                                return;
                            }
                            catch (IOException ex)
                            {
                                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("串口不存在", "Serial port does not exist"));

                                // 串口不存在后，重新选择
                                SerialPortUtil.CheckPort();

                                return;
                            }
                        }
                    }

                    isReceive = false;
                    serialPort.Write(send, 0, send.Length);

                    //发送的定时器
                    Button_Write.IsEnabled = false;
                    times = 0;//发送之前次数至空
                    threadTimer = new Timer(new System.Threading.TimerCallback(ReissueThreeTimes), send, 500, 500);
                    //SaveTrainInfo2DB(TrainInfoStatus.Normal);
                    //MessageBoxX.Info(LanguageUtils.ConvertLanguage("写卡成功", "Write card success"));
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("写卡异常");
                logger.Error("写卡异常");
                SerialPortUtil.SerialPort = null;
                SerialPortUtil.portName = "";
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("写卡异常，请重新选择串口", "The card is abnormal, please re-select the serial port."));
                return;
            }
           

        }
        // 本机写卡使用方法
        private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
        {
             
            try
            {
                //写卡
                TrainInfo trainInfo = new TrainService().GetTrainInfoByUserIdAndStatus(user.Pk_User_Id, (int)TrainInfoStatus.Normal);
                if (trainInfo != null)
                {
                    //MessageBox.Show(LanguageUtils.ConvertLanguage("是否覆盖", "Whether or not to cover"));

                    //MessageBoxResult dr = MessageBox.Show(LanguageUtils.ConvertLanguage("是否覆盖", "Whether or not to cover?"), LanguageUtils.ConvertLanguage("提示", "Point"), MessageBoxButton.OKCancel,
                    //    MessageBoxImage.Question);
                    //if (dr == MessageBoxResult.Cancel)
                    //{
                    //    return;
                    //}
                    if (!MessageBoxX.Question(LanguageUtils.ConvertLanguage("是否覆盖用户" + user.User_Name + "的已有训练计划？", "Whether or not to cover?")))
                    {
                        return;
                    }
                }
                //触发写卡之前，缓存界面数据的方法，保证当前对象存储的是最新的界面数据
                CacheDevicePrescriptions();

                //new TrainService().AbandonAllTempTrainInfo(user.Pk_User_Id);
                // 将当前训练信息存入数据库表，此时是暂存状态，在此之前设置该用户所有暂存状态的数据为废弃。此过程在servie中实现
                SaveTrainInfo2DB(TrainInfoStatus.Temp);
                if (user != null)
                {
                    byte[] data = new byte[90];
                    //用户id
                    string str = new UserService().getUserByUserId(user.Pk_User_Id).User_IDCard + "";
                    byte[] idBytes = Encoding.ASCII.GetBytes(str);
                    Array.Copy(idBytes, 0, data, 0, idBytes.Length);

                    //用户名
                    byte[] nameBytes = Encoding.GetEncoding("GBK").GetBytes(user.User_Name);
                    Array.Copy(nameBytes, 0, data, 32, nameBytes.Length);
                    //拼音
                    byte[] pingYinBytes = Encoding.ASCII.GetBytes(user.User_Namepinyin);
                    Array.Copy(pingYinBytes, 0, data, 52, pingYinBytes.Length);

                    int position = 84;
                    //设备
                    if (checkbox1.IsChecked == true)
                    {
                        //胸部推举机0x01
                        data[position] = (byte)DeviceType.P01;
                        position += 1;
                    }

                    if (checkbox2.IsChecked == true)
                    {
                        //坐姿划船机 0x05
                        data[position] = (byte)DeviceType.P02;
                        position += 1;
                    }

                    if (checkbox3.IsChecked == true)
                    {
                        //身体伸展弯曲机 0x04
                        data[position] = (byte)DeviceType.P03;
                        position += 1;
                    }

                    if (checkbox4.IsChecked == true)
                    {
                        //腿部伸展弯曲机 0x03
                        data[position] = (byte)DeviceType.P04;
                        position += 1;
                    }

                    if (checkbox5.IsChecked == true)
                    {
                        //胸部推举机 0x06
                        data[position] = (byte)DeviceType.P05;
                        position += 1;
                    }

                    if (checkbox6.IsChecked == true)
                    {
                        //腿部内外弯机 0x02
                        data[position] = (byte)DeviceType.P06;
                    }

                    //Console.WriteLine("发卡的内容：" + ProtocolUtil.ByteToStringOk(data));
                    logger.Debug("发卡的内容：" + ProtocolUtil.ByteToStringOk(data));

                   

                    //发送的定时器
                    Button_Write.IsEnabled = false;
                    //times = 0;//发送之前次数至空

                    Dispatcher.Invoke(new Action(() =>
                    {
                        try
                        {
                            //写卡成功，记录此次用到的串口
                            CommUtil.UpdateSettingString("port", SerialPortUtil.portName);
                            //将该用户的所有暂存信息设置为nomal，service中判断多种条件的情况，取日期最新的设置为0.其他的设置为废弃 
                            new TrainService().SetTmpToNomal(user.Pk_User_Id);
                        }
                        catch (Exception exception)
                        {
                            //Console.WriteLine("捕获异常了");
                            logger.Error("保存数据异常");
                            return;
                        }
                    }));
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("写卡成功", "Write card success"));
                    Button_Write.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("写卡异常");
                logger.Error("写卡异常");
            }


            //保存到数据库,在接收数据方法中
            //SaveTrainInfo2DB(TrainInfoStatus.Normal);
            //MessageBox.Show("已写卡");
            //this.Close();
        }

        /// <summary>
        /// 定时任务的回调方法
        /// </summary>
        /// <param name="state"></param>
        public void ReissueThreeTimes(Object state)
        {
            if (times < 3 && !isReceive)
            {
                byte[] send = (byte[])state;
                if (serialPort != null)
                {
                    if (!serialPort.IsOpen)
                    {
                        try
                        {
                            serialPort.Open();
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            MessageBoxX.Warning(LanguageUtils.ConvertLanguage("串口被占用", "Serial port is occupied"));
                            // 如果串口被占用，初始化app.config
                            CommUtil.UpdateSettingString("port", "");
                            return;
                        }
                        catch (IOException ex)
                        {
                            MessageBoxX.Warning(LanguageUtils.ConvertLanguage("串口不存在", "Serial port does not exist"));
                            return;
                        }
                    }

                    serialPort.Write(send, 0, send.Length);
                }

                times++;
            }
            else if (times >= 3 && !isReceive)
            {
                threadTimer.Dispose();
                //关闭串口
                SerialPortUtil.ClosePort(ref serialPort);
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("设备长时间未应答，请查看是否选对串口，或设备未启动", "The device has not answered for a long time. Check whether the serial port is selected or the device is not started."));
                    Button_Write.IsEnabled = true;
                    // 如果串口没有给出响应，初始化app.config
                    CommUtil.UpdateSettingString("port","");
                }));
               
            }
            else
            {
                threadTimer.Dispose();
                Dispatcher.Invoke(new Action(() =>
                {
                    Button_Write.IsEnabled = true;
                }));
            }

        }

        /// <summary>
        /// 接收数据的监听方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPortDataReceived(Object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(50);
                isReceive = true;//收到消息

                byte[] buffer = null; ;
                int len = serialPort.BytesToRead;

                byte[] receiveData = new byte[len];
                serialPort.Read(receiveData, 0, len);

                //Console.WriteLine("收到数据："+ProtocolUtil.ByteToStringOk(receiveData));
                logger.Debug("收到数据：" + ProtocolUtil.ByteToStringOk(receiveData));

                int offset = 0;

                if (len > 0 && receiveData[0] == 0xAA)//第一包数据
                {
                    buffer = new byte[6];

                    for (int i = 0; i < receiveData.Length; i++)
                    {
                        buffer[i] = receiveData[i];
                    }
                    offset = receiveData.Length;
                }
                else
                {
                    return;
                }


                while (buffer != null && buffer[buffer.Length - 1] != 0xCC)
                {

                    Thread.Sleep(50);
                    int len2 = serialPort.BytesToRead;

                    if (len2 <= 0)
                    {
                        return;
                    }

                    serialPort.Read(buffer, offset, len2);
                    offset += len2;

                    if (offset > buffer.Length)
                    {
                        return;
                    }
                }

                //下面是完整数据
                if (buffer != null)
                {
                    byte[] data = new byte[3];
                    Array.Copy(buffer, 1, data, 0, 3);
                    if (buffer[buffer.Length - 2] == ProtocolUtil.XorByByte(data))
                    {
                        //Console.WriteLine("校验成功");
                        if (buffer[buffer.Length - 3] == 0x00)
                        {
                            //Console.WriteLine("发卡成功");
                            //SaveTrainInfo2DB(TrainInfoStatus.Normal);

                            //此时处于发卡成功的逻辑，需要把打击写卡时候的暂存状态的数据，设置为nomal状态
                            Dispatcher.Invoke(new Action(() =>
                            {
                                try
                                {
                                    //写卡成功，记录此次用到的串口
                                    CommUtil.UpdateSettingString("port", SerialPortUtil.portName);
                                    //将该用户的所有暂存信息设置为nomal，service中判断多种条件的情况，取日期最新的设置为0.其他的设置为废弃 
                                    new TrainService().SetTmpToNomal(user.Pk_User_Id);
                                }
                                catch (Exception exception)
                                {
                                    //Console.WriteLine("捕获异常了");
                                    logger.Error("保存数据异常");
                                    return;
                                }
                            }));
                            //MessageBox.Show(LanguageUtils.ConvertLanguage("写卡成功", "Write card success"), "结果", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                            MessageBoxX.Info(LanguageUtils.ConvertLanguage("写卡成功", "Write card success"));
                        }
                        else
                        {
                            //this.Close();
                            //Console.WriteLine("发卡失败");
                            //MessageBox.Show(LanguageUtils.ConvertLanguage("写卡失败", "Failed to write to card"));
                            //MessageBox.Show(LanguageUtils.ConvertLanguage("写卡失败", "Failed to write to card"), "结果", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                            MessageBoxX.Error(LanguageUtils.ConvertLanguage("写卡失败", "Failed to write to card"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                isReceive = false;//处理完消息后
                Dispatcher.Invoke(new Action(() =>
                {
                    Button_Write.IsEnabled = true;
                }));
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //    //保存到数据库,在接收数据方法中
        //    try
        //    {
        //        SaveTrainInfo2DB(TrainInfoStatus.Normal);
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine("捕获异常了");
        //        return;
        //    }

        //    MessageBoxX.Info(LanguageUtils.ConvertLanguage("写卡成功", "Write card success"));
        //    this.Close();
        //}
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Save(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }

        private void select_change(object sender, EventArgs e)
        {

            //if (String.IsNullOrEmpty(combobox_06.Text)|| LanguageUtils.EqualsResource(combobox_06.Text, "TrainingListView.Invalid"))
            //{
            //    border1.Background = Brushes.White;
            //    border2.Background = Brushes.White;
            //    combobox_07.Visibility = Visibility.Hidden;
            //    border3.Background = Brushes.White;
            //    border4.Background = Brushes.White;
            //    combobox_08.Visibility = Visibility.Hidden;
            //    stackpanel.Margin = new Thickness(0, 149.8, 0, 0);
            //    t1.Height = 170;
            //}
            //else
            //{
            //    border1.Background = Brushes.Gray;
            //    border2.Background = Brushes.Gray;
            //    combobox_07.Visibility = Visibility.Visible;
            //    border3.Background = Brushes.Gray;
            //    border4.Background = Brushes.Gray;
            //    combobox_08.Visibility = Visibility.Visible;
            //    stackpanel.Margin = new Thickness(0, 199.8, 0, 0);
            //    t1.Height = 120;
            //}
        }
        private void select_change2(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(combobox_16.Text) || LanguageUtils.EqualsResource(combobox_16.Text, "TrainingListView.Invalid"))
            {
                border11.Background = Brushes.White;
                border12.Background = Brushes.White;
                combobox_17.Visibility = Visibility.Hidden;
                border13.Background = Brushes.White;
                border14.Background = Brushes.White;
                combobox_18.Visibility = Visibility.Hidden;
                stackpanel2.Margin = new Thickness(0, 149.8, 0, 0);
                t2.Height = 170;
            }
            else
            {
                border11.Background = Brushes.Gray;
                border12.Background = Brushes.Gray;
                combobox_17.Visibility = Visibility.Visible;
                border13.Background = Brushes.Gray;
                border14.Background = Brushes.Gray;
                combobox_18.Visibility = Visibility.Visible;
                stackpanel2.Margin = new Thickness(0, 199.8, 0, 0);
                t2.Height = 120;
            }
        }
        private void select_change3(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(combobox_26.Text) || LanguageUtils.EqualsResource(combobox_26.Text, "TrainingListView.Invalid"))
            {
                border21.Background = Brushes.White;
                border22.Background = Brushes.White;
                combobox_27.Visibility = Visibility.Hidden;
                border23.Background = Brushes.White;
                border24.Background = Brushes.White;
                combobox_28.Visibility = Visibility.Hidden;
                stackpanel3.Margin = new Thickness(0, 149.8, 0, 0);
                t3.Height = 170;
            }
            else
            {
                border21.Background = Brushes.Gray;
                border22.Background = Brushes.Gray;
                combobox_27.Visibility = Visibility.Visible;
                border23.Background = Brushes.Gray;
                border24.Background = Brushes.Gray;
                combobox_28.Visibility = Visibility.Visible;
                stackpanel3.Margin = new Thickness(0, 199.8, 0, 0);
                t3.Height = 120;
            }
        }
        private void select_change4(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(combobox_36.Text) || LanguageUtils.EqualsResource(combobox_36.Text, "TrainingListView.Invalid"))
            {
                border31.Background = Brushes.White;
                border32.Background = Brushes.White;
                combobox_37.Visibility = Visibility.Hidden;
                border33.Background = Brushes.White;
                border34.Background = Brushes.White;
                combobox_38.Visibility = Visibility.Hidden;
                stackpanel4.Margin = new Thickness(0, 149.8, 0, 0);
                t4.Height = 170;
            }
            else
            {
                border31.Background = Brushes.Gray;
                border32.Background = Brushes.Gray;
                combobox_37.Visibility = Visibility.Visible;
                border33.Background = Brushes.Gray;
                border34.Background = Brushes.Gray;
                combobox_38.Visibility = Visibility.Visible;
                stackpanel4.Margin = new Thickness(0, 199.8, 0, 0);
                t4.Height = 120;
            }
        }
        private void select_change5(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(combobox_46.Text) || LanguageUtils.EqualsResource(combobox_46.Text, "TrainingListView.Invalid"))
            {
                border41.Background = Brushes.White;
                border42.Background = Brushes.White;
                combobox_47.Visibility = Visibility.Hidden;
                border43.Background = Brushes.White;
                border44.Background = Brushes.White;
                combobox_48.Visibility = Visibility.Hidden;
                stackpanel5.Margin = new Thickness(0, 149.8, 0, 0);
                t5.Height = 170;
            }
            else
            {
                border41.Background = Brushes.Gray;
                border42.Background = Brushes.Gray;
                combobox_47.Visibility = Visibility.Visible;
                border43.Background = Brushes.Gray;
                border44.Background = Brushes.Gray;
                combobox_48.Visibility = Visibility.Visible;
                stackpanel5.Margin = new Thickness(0, 199.8, 0, 0);
                t5.Height = 120;
            }
        }
        private void select_change6(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(combobox_56.Text) || LanguageUtils.EqualsResource(combobox_56.Text, "TrainingListView.Invalid"))
            {
                border51.Background = Brushes.White;
                border52.Background = Brushes.White;
                combobox_57.Visibility = Visibility.Hidden;
                border53.Background = Brushes.White;
                border54.Background = Brushes.White;
                combobox_58.Visibility = Visibility.Hidden;
                stackpanel6.Margin = new Thickness(0, 149.8, 0, 0);
                t6.Height = 170;
            }
            else
            {
                border51.Background = Brushes.Gray;
                border52.Background = Brushes.Gray;
                combobox_57.Visibility = Visibility.Visible;
                border53.Background = Brushes.Gray;
                border54.Background = Brushes.Gray;
                combobox_58.Visibility = Visibility.Visible;
                stackpanel6.Margin = new Thickness(0, 199.8, 0, 0);
                t6.Height = 120;
            }
        }
        private void select_change7(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(combobox_16.Text) || LanguageUtils.EqualsResource(combobox_16.Text, "TrainingListView.Invalid"))
            {
                border11.Background = Brushes.White;
                border12.Background = Brushes.White;
                combobox_17.Visibility = Visibility.Hidden;
                border13.Background = Brushes.White;
                border14.Background = Brushes.White;
                combobox_18.Visibility = Visibility.Hidden;
                stackpanel2.Margin = new Thickness(0, 149.8, 0, 0);
                t2.Height = 170;
            }
            else
            {
                border11.Background = Brushes.Gray;
                border12.Background = Brushes.Gray;
                combobox_17.Visibility = Visibility.Visible;
                border13.Background = Brushes.Gray;
                border14.Background = Brushes.Gray;
                combobox_18.Visibility = Visibility.Visible;
                stackpanel2.Margin = new Thickness(0, 199.8, 0, 0);
                t2.Height = 120;
            }
        }
        private void select_change8(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(combobox_26.Text) || LanguageUtils.EqualsResource(combobox_26.Text, "TrainingListView.Invalid"))
            {
                border21.Background = Brushes.White;
                border22.Background = Brushes.White;
                combobox_27.Visibility = Visibility.Hidden;
                border23.Background = Brushes.White;
                border24.Background = Brushes.White;
                combobox_28.Visibility = Visibility.Hidden;
                stackpanel3.Margin = new Thickness(0, 149.8, 0, 0);
                t3.Height = 170;
            }
            else
            {
                border21.Background = Brushes.Gray;
                border22.Background = Brushes.Gray;
                combobox_27.Visibility = Visibility.Visible;
                border23.Background = Brushes.Gray;
                border24.Background = Brushes.Gray;
                combobox_28.Visibility = Visibility.Visible;
                stackpanel3.Margin = new Thickness(0, 199.8, 0, 0);
                t3.Height = 120;
            }
        }
        private void select_change9(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(combobox_36.Text) || LanguageUtils.EqualsResource(combobox_36.Text, "TrainingListView.Invalid"))
            {
                border31.Background = Brushes.White;
                border32.Background = Brushes.White;
                combobox_37.Visibility = Visibility.Hidden;
                border33.Background = Brushes.White;
                border34.Background = Brushes.White;
                combobox_38.Visibility = Visibility.Hidden;
                stackpanel4.Margin = new Thickness(0, 149.8, 0, 0);
                t4.Height = 170;
            }
            else
            {
                border31.Background = Brushes.Gray;
                border32.Background = Brushes.Gray;
                combobox_37.Visibility = Visibility.Visible;
                border33.Background = Brushes.Gray;
                border34.Background = Brushes.Gray;
                combobox_38.Visibility = Visibility.Visible;
                stackpanel4.Margin = new Thickness(0, 199.8, 0, 0);
                t4.Height = 120;
            }
        }
        private void select_change10(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(combobox_46.Text) || LanguageUtils.EqualsResource(combobox_46.Text, "TrainingListView.Invalid"))
            {
                border41.Background = Brushes.White;
                border42.Background = Brushes.White;
                combobox_47.Visibility = Visibility.Hidden;
                border43.Background = Brushes.White;
                border44.Background = Brushes.White;
                combobox_48.Visibility = Visibility.Hidden;
                stackpanel5.Margin = new Thickness(0, 149.8, 0, 0);
                t5.Height = 170;
            }
            else
            {
                border41.Background = Brushes.Gray;
                border42.Background = Brushes.Gray;
                combobox_47.Visibility = Visibility.Visible;
                border43.Background = Brushes.Gray;
                border44.Background = Brushes.Gray;
                combobox_48.Visibility = Visibility.Visible;
                stackpanel5.Margin = new Thickness(0, 199.8, 0, 0);
                t5.Height = 120;
            }
        }

        private void Checkbox1_OnChecked(object sender, RoutedEventArgs e)
        {
            combobox_01.SelectedIndex = 0;
            combobox_02.SelectedIndex = 0;
            combobox_03.SelectedIndex = 0;
            combobox_05.SelectedIndex = 0;
            combobox_06.SelectedIndex = 1;
            select_change(sender, e);
            t1.Text = "";
        }
        private void Checkbox2_OnChecked(object sender, RoutedEventArgs e)
        {
            //combobox_01.IsEnabled = false;
            combobox_11.SelectedIndex = 0;
            //combobox_02.IsEnabled = false;
            combobox_12.SelectedIndex = 0;
            //combobox_03.IsEnabled = false;
            combobox_13.SelectedIndex = 0;
            //combobox_04.IsEnabled = false;
            combobox_14.SelectedIndex = 0;
            //combobox_05.IsEnabled = false;
            combobox_15.SelectedIndex = 0;
            combobox_16.SelectedIndex = 1;
            select_change(sender, e);
            combobox_17.SelectedIndex = 0;
            combobox_18.SelectedIndex = 0;
            //com_01.IsEnabled = false;
            com_11.SelectedIndex = 0;
            //com_04.IsEnabled = false;
            com_14.SelectedIndex = 0;
            //com_02.IsEnabled = false;
            //com_03.IsEnabled = false;
            com_13.SelectedIndex = 0;
            t2.Text = "";
        }
        private void Checkbox3_OnChecked(object sender, RoutedEventArgs e)
        {
            //combobox_01.IsEnabled = false;
            combobox_21.SelectedIndex = 0;
            //combobox_02.IsEnabled = false;
            combobox_22.SelectedIndex = 0;
            //combobox_03.IsEnabled = false;
            combobox_23.SelectedIndex = 0;
            //combobox_04.IsEnabled = false;
            combobox_24.SelectedIndex = 0;
            //combobox_05.IsEnabled = false;
            combobox_25.SelectedIndex = 0;
            combobox_26.SelectedIndex = 1;
            select_change2(sender, e);
            combobox_27.SelectedIndex = 0;
            combobox_28.SelectedIndex = 0;
            //com_01.IsEnabled = false;
            com_21.SelectedIndex = 0;
            //com_04.IsEnabled = false;
            com_24.SelectedIndex = 0;
            com_24_Copy.SelectedIndex = 0;
            com_24_Copy1.SelectedIndex = 0;
            //com_02.IsEnabled = false;
            //com_03.IsEnabled = false;
            com_23.SelectedIndex = 0;
            t3.Text = "";
        }
        private void Checkbox4_OnChecked(object sender, RoutedEventArgs e)
        {
            //combobox_01.IsEnabled = false;
            combobox_31.SelectedIndex = 0;
            //combobox_02.IsEnabled = false;
            combobox_32.SelectedIndex = 0;
            //combobox_03.IsEnabled = false;
            combobox_33.SelectedIndex = 0;
            //combobox_04.IsEnabled = false;
            combobox_34.SelectedIndex = 0;
            //combobox_05.IsEnabled = false;
            combobox_35.SelectedIndex = 0;
            combobox_36.SelectedIndex = 1;
            select_change3(sender, e);
            combobox_37.SelectedIndex = 0;
            combobox_38.SelectedIndex = 0;
            //com_01.IsEnabled = false;
            com_31.SelectedIndex = 0;
            //com_04.IsEnabled = false;
            com_34.SelectedIndex = 0;
            //com_02.IsEnabled = false;
            com_35.SelectedIndex = 0;
            com_35_copy.SelectedIndex = 0;
            com_33.SelectedIndex = 0;
            t4.Text = "";
        }
        private void Checkbox5_OnChecked(object sender, RoutedEventArgs e)
        {
            //combobox_01.IsEnabled = false;
            combobox_41.SelectedIndex = 0;
            //combobox_02.IsEnabled = false;
            combobox_42.SelectedIndex = 0;
            //combobox_03.IsEnabled = false;
            combobox_43.SelectedIndex = 0;
            //combobox_04.IsEnabled = false;
            combobox_44.SelectedIndex = 0;
            //combobox_05.IsEnabled = false;
            combobox_45.SelectedIndex = 0;
            combobox_46.SelectedIndex = 1;
            select_change4(sender, e);
            combobox_47.SelectedIndex = 0;
            combobox_48.SelectedIndex = 0;
            //com_01.IsEnabled = false;
            com_41.SelectedIndex = 0;
            //com_04.IsEnabled = false;
            //com_02.IsEnabled = false;
            //com_03.IsEnabled = false;
            com_43.SelectedIndex = 0;
            com_43_Copy2.SelectedIndex = 0;
            com_43_Copy1.SelectedIndex = 0;
            com_43_Copy.SelectedIndex = 0;
            t5.Text = "";
        }
        private void Checkbox6_OnChecked(object sender, RoutedEventArgs e)
        {
            //combobox_01.IsEnabled = false;
            combobox_51.SelectedIndex = 0;
            //combobox_02.IsEnabled = false;
            combobox_52.SelectedIndex = 0;
            //combobox_03.IsEnabled = false;
            combobox_53.SelectedIndex = 0;
            //combobox_04.IsEnabled = false;
            combobox_54.SelectedIndex = 0;
            //combobox_05.IsEnabled = false;
            combobox_55.SelectedIndex = 0;
            combobox_56.SelectedIndex = 1;
            select_change5(sender, e);
            combobox_57.SelectedIndex = 0;
            combobox_58.SelectedIndex = 0;
            //com_01.IsEnabled = false;
            com_51.SelectedIndex = 0;
            //com_04.IsEnabled = false;
            //com_02.IsEnabled = false;
            com_53.SelectedIndex = 0;
            com_53_Copy.SelectedIndex = 0;
            t6.Text = "";
        }
        private void Checkbox7_OnChecked(object sender, RoutedEventArgs e)
        {
            //combobox_01.IsEnabled = false;
            combobox_11.SelectedIndex = 0;
            //combobox_02.IsEnabled = false;
            combobox_12.SelectedIndex = 0;
            //combobox_03.IsEnabled = false;
            combobox_13.SelectedIndex = 0;
            //combobox_04.IsEnabled = false;
            combobox_14.SelectedIndex = 0;
            //combobox_05.IsEnabled = false;
            combobox_15.SelectedIndex = 0;
            combobox_16.SelectedIndex = 1;
            select_change(sender, e);
            combobox_17.SelectedIndex = 0;
            combobox_18.SelectedIndex = 0;
            //com_01.IsEnabled = false;
            com_11.SelectedIndex = 0;
            //com_04.IsEnabled = false;
            com_14.SelectedIndex = 0;
            //com_02.IsEnabled = false;
            //com_03.IsEnabled = false;
            com_13.SelectedIndex = 0;
            t2.Text = "";
        }
        private void Checkbox8_OnChecked(object sender, RoutedEventArgs e)
        {
            //combobox_01.IsEnabled = false;
            combobox_21.SelectedIndex = 0;
            //combobox_02.IsEnabled = false;
            combobox_22.SelectedIndex = 0;
            //combobox_03.IsEnabled = false;
            combobox_23.SelectedIndex = 0;
            //combobox_04.IsEnabled = false;
            combobox_24.SelectedIndex = 0;
            //combobox_05.IsEnabled = false;
            combobox_25.SelectedIndex = 0;
            combobox_26.SelectedIndex = 1;
            select_change2(sender, e);
            combobox_27.SelectedIndex = 0;
            combobox_28.SelectedIndex = 0;
            //com_01.IsEnabled = false;
            com_21.SelectedIndex = 0;
            //com_04.IsEnabled = false;
            com_24.SelectedIndex = 0;
            com_24_Copy.SelectedIndex = 0;
            com_24_Copy1.SelectedIndex = 0;
            //com_02.IsEnabled = false;
            //com_03.IsEnabled = false;
            com_23.SelectedIndex = 0;
            t3.Text = "";
        }
        private void Checkbox9_OnChecked(object sender, RoutedEventArgs e)
        {
            //combobox_01.IsEnabled = false;
            combobox_31.SelectedIndex = 0;
            //combobox_02.IsEnabled = false;
            combobox_32.SelectedIndex = 0;
            //combobox_03.IsEnabled = false;
            combobox_33.SelectedIndex = 0;
            //combobox_04.IsEnabled = false;
            combobox_34.SelectedIndex = 0;
            //combobox_05.IsEnabled = false;
            combobox_35.SelectedIndex = 0;
            combobox_36.SelectedIndex = 1;
            select_change3(sender, e);
            combobox_37.SelectedIndex = 0;
            combobox_38.SelectedIndex = 0;
            //com_01.IsEnabled = false;
            com_31.SelectedIndex = 0;
            //com_04.IsEnabled = false;
            com_34.SelectedIndex = 0;
            //com_02.IsEnabled = false;
            com_35.SelectedIndex = 0;
            com_35_copy.SelectedIndex = 0;
            com_33.SelectedIndex = 0;
            t4.Text = "";
        }
        private void Checkbox10_OnChecked(object sender, RoutedEventArgs e)
        {
            //combobox_01.IsEnabled = false;
            combobox_41.SelectedIndex = 0;
            //combobox_02.IsEnabled = false;
            combobox_42.SelectedIndex = 0;
            //combobox_03.IsEnabled = false;
            combobox_43.SelectedIndex = 0;
            //combobox_04.IsEnabled = false;
            combobox_44.SelectedIndex = 0;
            //combobox_05.IsEnabled = false;
            combobox_45.SelectedIndex = 0;
            combobox_46.SelectedIndex = 1;
            select_change4(sender, e);
            combobox_47.SelectedIndex = 0;
            combobox_48.SelectedIndex = 0;
            //com_01.IsEnabled = false;
            com_41.SelectedIndex = 0;
            //com_04.IsEnabled = false;
            //com_02.IsEnabled = false;
            //com_03.IsEnabled = false;
            com_43.SelectedIndex = 0;
            com_43_Copy2.SelectedIndex = 0;
            com_43_Copy1.SelectedIndex = 0;
            com_43_Copy.SelectedIndex = 0;
            t5.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveTrainInfo2DB(TrainInfoStatus.Normal);
        }

        private void viewbox_load(object sender, RoutedEventArgs e)
        {
            this.Width = viewbox.ActualWidth;
            this.Height = viewbox.ActualHeight;

            Left = (SystemParameters.WorkArea.Size.Width - this.ActualWidth) / 2;
            Top = (SystemParameters.WorkArea.Size.Height - this.ActualHeight) / 2;
        }

        //private void text_length(object sender, KeyEventArgs e)
        //{
        //    if (((TextBox)sender).Text.Length > 8)
        //    {

        //    }
        //}
        public static void SetAlignment()
        {
            //获取系统是以Left-handed（true）还是Right-handed（false）
            var ifLeft = SystemParameters.MenuDropAlignment;

            if (ifLeft)
            {
                // change to false
                var t = typeof(SystemParameters);
                var field = t.GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
                field.SetValue(null, false);

                ifLeft = SystemParameters.MenuDropAlignment;
            }
        }
    }
}
