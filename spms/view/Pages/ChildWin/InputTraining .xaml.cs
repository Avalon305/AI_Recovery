using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
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
using spms.constant;
using spms.dao;
using spms.entity;
using spms.service;
using spms.util;

namespace spms.view.Pages.ChildWin
{
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
        public InputTraining()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show(LanguageUtils.ConvertLanguage("是否所有编辑都无效？", "Whether all editors are invalid?"), LanguageUtils.ConvertLanguage("提示", "Point"), MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
            if (dr == MessageBoxResult.OK)
            {
                this.Close();
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

        private void checkbox1_Checked(object sender, RoutedEventArgs e)
        {
            combobox_01.IsEnabled = true;
            combobox_02.IsEnabled = true;
            combobox_03.IsEnabled = true;
            combobox_04.IsEnabled = true;
            combobox_05.IsEnabled = true;
            com_01.IsEnabled = true;
            com_04.IsEnabled = true;
            //com_02.IsEnabled = true;
            com_03.IsEnabled = true;
            
        }

        private void checkbox2_Checked(object sender, RoutedEventArgs e)
        {
            combobox_11.IsEnabled = true;
            combobox_12.IsEnabled = true;
            combobox_13.IsEnabled = true;
            combobox_14.IsEnabled = true;
            combobox_15.IsEnabled = true;
            com_11.IsEnabled = true;
            //com_12.IsEnabled = true;
            com_13.IsEnabled = true;
            com_14.IsEnabled = true;
        }


        private void checkbox3_Checked(object sender, RoutedEventArgs e)
        {
            combobox_21.IsEnabled = true;
            combobox_22.IsEnabled = true;
            combobox_23.IsEnabled = true;
            combobox_24.IsEnabled = true;
            combobox_25.IsEnabled = true;
            com_21.IsEnabled = true;
            com_24.IsEnabled = true;
            //com_22.IsEnabled = true;
            com_23.IsEnabled = true;

        }

        private void checkbox4_Checked(object sender, RoutedEventArgs e)
        {
            combobox_31.IsEnabled = true;
            combobox_32.IsEnabled = true;
            combobox_33.IsEnabled = true;
            combobox_34.IsEnabled = true;
            combobox_35.IsEnabled = true;
            com_31.IsEnabled = true;
            com_34.IsEnabled = true;
            //com_32.IsEnabled = true;
            com_33.IsEnabled = true;
            com_35.IsEnabled = true;
        }

        private void checkbox5_Checked(object sender, RoutedEventArgs e)
        {
            combobox_41.IsEnabled = true;
            combobox_42.IsEnabled = true;
            combobox_43.IsEnabled = true;
            combobox_44.IsEnabled = true;
            combobox_45.IsEnabled = true;
            com_41.IsEnabled = true;
            //com_42.IsEnabled = true;
            com_43.IsEnabled = true;


        }

        private void checkbox6_Checked(object sender, RoutedEventArgs e)
        {
            combobox_51.IsEnabled = true;
            combobox_52.IsEnabled = true;
            combobox_53.IsEnabled = true;
            combobox_54.IsEnabled = true;
            combobox_55.IsEnabled = true;
            com_51.IsEnabled = true;
            //com_52.IsEnabled = true;
            com_53.IsEnabled = true;
        }

        private void checkbox1_Unchecked(object sender, RoutedEventArgs e)
        {
            //combobox_01.IsEnabled = false;
            combobox_01.Text = null;
            //combobox_02.IsEnabled = false;
            combobox_02.Text = null; 
            //combobox_03.IsEnabled = false;
            combobox_03.Text = null;
            //combobox_04.IsEnabled = false;
            combobox_04.Text = null;
            //combobox_05.IsEnabled = false;
            combobox_05.Text = null;
            combobox_06.Text = null;
            select_change(sender, e);
            combobox_07.Text = null;
            combobox_08.Text = null;
            //com_01.IsEnabled = false;
            com_01.Text = null;
            //com_04.IsEnabled = false;
            com_04.Text = null;
            //com_02.IsEnabled = false;
            //com_03.IsEnabled = false;
            com_03.Text = null;
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
            t5.Text = null;
        }

        private void checkbox6_Unchecked(object sender, RoutedEventArgs e)
        {
            //combobox_51.IsEnabled = false;
            //combobox_52.IsEnabled = false;
            //combobox_53.IsEnabled = false;
            //combobox_54.IsEnabled = false;
            //combobox_55.IsEnabled = false;
            //com_51.IsEnabled = false;
            ////com_52.IsEnabled = false;
            //com_53.IsEnabled = false;
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
            t6.Text = null;
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveTrainInfo2DB(TrainInfoStatus.Save);
            }
            catch (Exception exception)
            {
                return;
            }
            MessageBox.Show(LanguageUtils.ConvertLanguage("已存储", "Finished storage"));
            this.Close();
        }

        private void SaveTrainInfo2DB(TrainInfoStatus status)
        {
            List<DevicePrescription> devicePrescriptions = new List<DevicePrescription>();
            
            string devName; //设备名字
            string attr1; //属性1
            string attr2; //2
            string attr3; //3
            string attr4; //4
            string attr5; //5
            DeviceSortDAO deviceSortDao = new DeviceSortDAO();
            if (checkbox1.IsChecked == true)
            {
                //胸部推举机
                //attr1 = com_01.Text; //属性1
                //attr2 = com_02.Text; //2
                attr3 = com_03.Text; //3
                attr4 = com_04.Text; //4

                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3 + "*" +
                                              attr4;
                devicePrescription.DP_Memo = t1.Text; //注意点
                devicePrescription.Fk_DS_Id = (int)DeviceType.X01;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(com_01.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_01.Text); //组数;
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_02.Text); //个数;
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_05.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_03.Text); //间隔时间;
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(combobox_04.Text); //砝码;
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
                    throw e;
                }

                if (LanguageUtils.EqualsResource(combobox_06.Text, "TrainingListView.Valid"))
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount = Byte.Parse(combobox_07.Text);
                        
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        throw e;
                    }
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (LanguageUtils.EqualsResource(combobox_08.Text, "TrainingListView.CountReverse"))
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }

                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            if (checkbox2.IsChecked == true)
            {
                //坐姿划船机
                devName = "坐姿划船机";
                //attr1 = com_11.Text; //属性1
                //attr2 = com_12.Text; //2
                attr3 = com_13.Text; //3
                attr4 = com_14.Text; //4

                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3 + "*" +
                                              attr4;
                devicePrescription.DP_Memo = t2.Text; //注意点
                devicePrescription.Fk_DS_Id = (int)DeviceType.X05;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(com_11.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
                    throw e;
                }
                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_11.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_12.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_15.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_13.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(combobox_14.Text); //砝码
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
                    throw e;
                }
                if (LanguageUtils.EqualsResource(combobox_16.Text, "TrainingListView.Valid"))
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount = Byte.Parse(combobox_17.Text);

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        throw e;
                    }

                    
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (LanguageUtils.EqualsResource(combobox_18.Text, "TrainingListView.CountReverse"))
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }
                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            if (checkbox3.IsChecked == true)
            {
                //身体伸展弯曲机
                devName = "身体伸展弯曲机";
                //attr1 = com_21.Text; //属性1
                //attr2 = com_22.Text; //2
                attr3 = com_23.Text; //3
                attr4 = com_24.Text; //4
                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3 + "*" +
                                              attr4;
                devicePrescription.DP_Memo = t3.Text; //注意点
                devicePrescription.Fk_DS_Id = (int)DeviceType.X04;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(com_21.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
                    throw e;
                }
                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_21.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_22.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_25.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_23.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(combobox_24.Text); //砝码
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
                    throw e;
                }
                if (LanguageUtils.EqualsResource(combobox_26.Text, "TrainingListView.Valid"))
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount = Byte.Parse(combobox_27.Text);

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        throw e;
                    }

                    
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (LanguageUtils.EqualsResource(combobox_28.Text, "TrainingListView.CountReverse"))
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }
                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            if (checkbox4.IsChecked == true)
            {
                //腿部伸展弯曲机
                devName = "腿部伸展弯曲机";
                //attr1 = com_31.Text; //属性1
                //attr2 = com_32.Text; //2
                attr3 = com_33.Text; //3
                attr4 = com_34.Text; //4
                attr5 = com_35.Text; //5

                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3 + "*" +
                                              attr4 + "*" +
                                              attr5;
                devicePrescription.DP_Memo = t4.Text; //注意点
                devicePrescription.Fk_DS_Id = (int)DeviceType.X03;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(com_31.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
                    throw e;
                }
                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_31.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_32.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_35.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_33.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(combobox_34.Text); //砝码
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
                    throw e;
                }
                if (LanguageUtils.EqualsResource(combobox_36.Text, "TrainingListView.Valid"))
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount = Byte.Parse(combobox_37.Text);

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        throw e;
                    }

                    
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (LanguageUtils.EqualsResource(combobox_38.Text, "TrainingListView.CountReverse"))
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }
                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            if (checkbox5.IsChecked == true)
            {
                //腿部腿蹬机
                //attr1 = com_41.Text; //属性1
                //attr2 = com_42.Text; //2
                attr3 = com_43.Text; //3

                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3;
                devicePrescription.DP_Memo = t5.Text; //注意点
                devicePrescription.Fk_DS_Id = (int)DeviceType.X06;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(com_41.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
                    throw e;
                }
                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_41.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_42.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_45.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_43.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(combobox_44.Text); //砝码
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
                    throw e;
                }
                if (LanguageUtils.EqualsResource(combobox_46.Text, "TrainingListView.Valid"))
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount =Byte.Parse(combobox_47.Text);

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        throw e;
                    }
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (LanguageUtils.EqualsResource(combobox_48.Text, "TrainingListView.CountReverse"))
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }
                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            if (checkbox6.IsChecked == true)
            {
                //腿部内外弯机
                //attr1 = com_51.Text; //属性1
                //attr2 = com_52.Text; //2
                attr3 = com_53.Text; //3

                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = //attr1 + "*" +
                                              //attr2 + "*" +
                                              attr3;
                devicePrescription.DP_Memo = t6.Text; //注意点
                devicePrescription.Fk_DS_Id = (int)DeviceType.X02;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                try
                {
                    devicePrescription.dp_movedistance = Convert.ToDouble(com_51.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移动距离", "Please select the correct moving distance"));
                    throw e;
                }
                try
                {
                    devicePrescription.dp_groupcount = Convert.ToInt32(combobox_51.Text); //组数
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的组数", "Please select the correct number of groups"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_groupnum = Convert.ToInt32(combobox_52.Text); //个数
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的个数", "Please select the correct number"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_55.Text)); //移乘方式
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的移乘方式", "Please select the correct moveway"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_53.Text); //间隔时间
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的间隔时间", "Please choose the right interval"));
                    throw e;
                }

                try
                {
                    devicePrescription.dp_weight = Convert.ToDouble(combobox_54.Text); //砝码
                }
                catch (Exception e)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请选择正确的砝码", "Please choose the right weight"));
                    throw e;
                }
                if (LanguageUtils.EqualsResource(combobox_56.Text, "TrainingListView.Valid"))
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_VALID;

                    try
                    {
                        devicePrescription.dp_timecount = Byte.Parse(combobox_57.Text);

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("请输入正确的计时时间", "Please choose the right timecount"));
                        throw e;
                    }
                }
                else
                {
                    devicePrescription.dp_timer = DevConstants.TIMER_INVALID;
                }
                if (LanguageUtils.EqualsResource(combobox_58.Text, "TrainingListView.CountReverse"))
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_REVERSE;
                }
                else
                {
                    devicePrescription.dp_timetype = DevConstants.COUNT_FORWARD;
                }
                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            TrainInfo trainInfo = new TrainInfo();
            trainInfo.Gmt_Create = DateTime.Now;
            trainInfo.Gmt_Modified = DateTime.Now;
            trainInfo.FK_User_Id = user.Pk_User_Id;
            trainInfo.Status = (int) status;
            if (!string.IsNullOrEmpty(symp.Text))
            {
                //如果选择了症状记录
                new TrainService().SaveTraininfo(symp.SelectedValue, trainInfo, devicePrescriptions);
            }
            else
            {
                //存储到数据库
                new TrainService().SaveTraininfo(null, trainInfo, devicePrescriptions);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //确定哪些设备可用
            Certain_Dev();
            //去除窗体叉号
           
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
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
            symp.ItemsSource = nullTiIdByUserId;
            com_01.ItemsSource = Add(0, 70, 2);
            //com_02.ItemsSource = Add(0, 30, 2);
            com_03.ItemsSource = Add(1, 5, 2);
            com_04.ItemsSource = Add(1, 9, 2);
            combobox_01.ItemsSource = Add(1, 3, 2);
            combobox_02.ItemsSource = Add(1, 20, 2);
            combobox_03.ItemsSource = Add(1, 60, 2);
            combobox_04.ItemsSource = Add(0.5, 32.0, 1);
            combobox_05.ItemsSource = dataItems;

            com_11.ItemsSource = Add(0, 70, 2);
            //com_12.ItemsSource = Add(0, 30, 1);
            com_13.ItemsSource = Add(1, 4, 2);
            com_14.ItemsSource = Add(1, 4, 2);
            combobox_11.ItemsSource = Add(1, 3, 2);
            combobox_12.ItemsSource = Add(1, 20, 2);
            combobox_13.ItemsSource = Add(1, 60, 2);
            combobox_14.ItemsSource = Add(0.5, 32.0, 1);
            combobox_15.ItemsSource = dataItems;

            com_21.ItemsSource = Add(0, 40, 2);
            //com_22.ItemsSource = Add(0, 30, 1);
            com_23.ItemsSource = Add(1, 6, 2);
            com_24.ItemsSource = Add(1, 9, 2);
            combobox_21.ItemsSource = Add(1, 3, 2);
            combobox_22.ItemsSource = Add(1, 20, 2);
            combobox_23.ItemsSource = Add(1, 60, 2);
            combobox_24.ItemsSource = Add(0.5, 32.0, 1);
            combobox_25.ItemsSource = dataItems;

            com_31.ItemsSource = Add(0, 26, 1);
            //com_32.ItemsSource = Add(0, 30, 1);
            com_33.ItemsSource = Add(1, 5, 2);
            com_34.ItemsSource = Add(1, 5, 2);
            com_35.ItemsSource = Add(1, 9, 2);
            combobox_31.ItemsSource = Add(1, 3, 2);
            combobox_32.ItemsSource = Add(1, 20, 2);
            combobox_33.ItemsSource = Add(1, 60, 2);
            combobox_34.ItemsSource = Add(0.5, 32.0, 1);
            combobox_35.ItemsSource = dataItems;

            com_41.ItemsSource = Add(0, 50, 2);
            //com_42.ItemsSource = Add(0, 30, 1);
            com_43.ItemsSource = Add(1, 9, 2);
            combobox_41.ItemsSource = Add(1, 3, 2);
            combobox_42.ItemsSource = Add(1, 20, 2);
            combobox_43.ItemsSource = Add(1, 60, 2);
            combobox_44.ItemsSource = Add(0.5, 32.0, 1);
            combobox_45.ItemsSource = dataItems;

            com_51.ItemsSource = Add(0, 18, 2);
            //com_52.ItemsSource = Add(0, 30, 1);
            com_53.ItemsSource = Add(1, 8, 2);
            combobox_51.ItemsSource = Add(1, 3, 2);
            combobox_52.ItemsSource = Add(1, 20, 2);
            combobox_53.ItemsSource = Add(1, 60, 2);
            combobox_54.ItemsSource = Add(0.5, 32.0, 1);
            combobox_55.ItemsSource = dataItems;

            List<DevicePrescription> devicePrescriptions = new TrainService().GetSaveDevicePrescriptionsByUser(user);
            if (devicePrescriptions == null)
            {
                return;
            }
            foreach (DevicePrescription devicePrescription in devicePrescriptions)
            {
                if (devicePrescription.DP_Attrs == null)
                {
                    //如果没设置属性，直接跳过
                    continue;
                }
                string[] attrs = devicePrescription.DP_Attrs.Split(new char[] {'*'});
                int devName = devicePrescription.Fk_DS_Id;
                switch (devName)
                {
                    case (int)DeviceType.X01:
                        checkbox1.IsChecked = true;
                        //设置属性
                        com_01.Text = devicePrescription.dp_movedistance.ToString();
                        //com_02.Text = attrs[1];
                        com_03.Text = attrs[0];
                        com_04.Text = attrs[1];
                        //设置处方信息
                        combobox_01.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_02.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_03.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_04.Text = devicePrescription.dp_weight.ToString();
                        combobox_05.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t1.Text = devicePrescription.DP_Memo;
                        if(devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            combobox_06.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Valid");
                            select_change(combobox_06, new EventArgs());
                            combobox_07.Text = devicePrescription.dp_timecount.ToString();
                            if(devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                combobox_08.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountForward");
                            }
                            else
                            {
                                combobox_08.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountReverse");
                            }
                        }
                        else
                        {
                            combobox_06.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Invalid");
                        }
                        break;
                    case (int)DeviceType.X05:
                        checkbox2.IsChecked = true;
                        com_11.Text = devicePrescription.dp_movedistance.ToString();
                        //com_12.Text = attrs[1];
                        com_13.Text = attrs[0];
                        com_14.Text = attrs[1];
                        combobox_11.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_12.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_13.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_14.Text = devicePrescription.dp_weight.ToString();
                        combobox_15.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t2.Text = devicePrescription.DP_Memo;
                        if (devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            combobox_16.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Valid");
                            select_change2(combobox_16, new EventArgs());
                            combobox_17.Text = devicePrescription.dp_timecount.ToString();
                            if (devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                combobox_18.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountForward");
                            }
                            else
                            {
                                combobox_18.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountReverse");
                            }
                        }
                        else
                        {
                            combobox_16.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Invalid");
                        }
                        break;
                    case (int)DeviceType.X04:
                        checkbox3.IsChecked = true;
                        com_21.Text = devicePrescription.dp_movedistance.ToString();
                        //com_22.Text = attrs[1];
                        com_23.Text = attrs[0];
                        com_24.Text = attrs[1];
                        combobox_21.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_22.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_23.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_24.Text = devicePrescription.dp_weight.ToString();
                        combobox_25.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t3.Text = devicePrescription.DP_Memo;
                        if (devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            combobox_26.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Valid");
                            select_change3(combobox_26, new EventArgs());
                            combobox_27.Text = devicePrescription.dp_timecount.ToString();
                            if (devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                combobox_28.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountForward");
                            }
                            else
                            {
                                combobox_28.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountReverse");
                            }
                        }
                        else
                        {
                            combobox_26.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Invalid");
                        }
                        break;
                    case (int)DeviceType.X03:
                        checkbox4.IsChecked = true;
                        com_31.Text = devicePrescription.dp_movedistance.ToString();
                        //com_32.Text = attrs[1];
                        com_33.Text = attrs[0];
                        com_34.Text = attrs[1];
                        com_35.Text = attrs[2];
                        combobox_31.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_32.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_33.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_34.Text = devicePrescription.dp_weight.ToString();
                        combobox_35.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t4.Text = devicePrescription.DP_Memo;
                        if (devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            combobox_36.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Valid");
                            select_change4(combobox_36, new EventArgs());
                            combobox_37.Text = devicePrescription.dp_timecount.ToString();
                            if (devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                combobox_38.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountForward");
                            }
                            else
                            {
                                combobox_38.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountReverse");
                            }
                        }
                        else
                        {
                            combobox_36.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Invalid");
                        }
                        break;
                    case (int)DeviceType.X06:
                        checkbox5.IsChecked = true;
                        com_41.Text = devicePrescription.dp_movedistance.ToString();
                        //com_42.Text = attrs[1];
                        com_43.Text = attrs[0];
                        combobox_41.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_42.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_43.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_44.Text = devicePrescription.dp_weight.ToString();
                        combobox_45.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t5.Text = devicePrescription.DP_Memo;
                        if (devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            combobox_46.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Valid");
                            select_change5(combobox_46, new EventArgs());
                            combobox_47.Text = devicePrescription.dp_timecount.ToString();
                            if (devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                combobox_48.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountForward");
                            }
                            else
                            {
                                combobox_48.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountReverse");
                            }
                        }
                        else
                        {
                            combobox_46.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Invalid");
                        }
                        break;
                    case (int)DeviceType.X02:
                        checkbox6.IsChecked = true;
                        com_51.Text = devicePrescription.dp_movedistance.ToString();
                        //com_52.Text = attrs[1];
                        com_53.Text = attrs[0];
                        combobox_51.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_52.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_53.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_54.Text = devicePrescription.dp_weight.ToString();
                        combobox_55.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t6.Text = devicePrescription.DP_Memo;
                        if (devicePrescription.dp_timer == DevConstants.TIMER_VALID)
                        {
                            combobox_56.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Valid");
                            select_change6(combobox_56, new EventArgs());
                            combobox_57.Text = devicePrescription.dp_timecount.ToString();
                            if (devicePrescription.dp_timetype == DevConstants.COUNT_FORWARD)
                            {
                                combobox_58.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountForward");
                            }
                            else
                            {
                                combobox_58.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.CountReverse");
                            }
                        }
                        else
                        {
                            combobox_56.Text = LanguageUtils.GetCurrentLanuageStrByKey("TrainingListView.Invalid");
                        }
                        break;
                }

            }
        }

        private void Certain_Dev()
        {
            var devs = new DeviceSortDAO().ListAll();
            foreach(DeviceSort dev in devs)
            {
                if(dev.DS_Status == 1)
                {
                    continue;
                }

                if (LanguageUtils.EqualsResource(dev.DS_name, "Dev.ChestPress"))
                {
                    checkbox1.IsChecked = false;
                    checkbox1.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "Dev.Rowing"))
                {
                    checkbox2.IsChecked = false;
                    checkbox2.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "Dev.TorsoFlexion"))
                {
                    checkbox3.IsChecked = false;
                    checkbox3.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "Dev.LegExtension"))
                {
                    checkbox4.IsChecked = false;
                    checkbox4.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "Dev.HorizontalLegPress"))
                {
                    checkbox5.IsChecked = false;
                    checkbox5.IsEnabled = false;
                }
                else if (LanguageUtils.EqualsResource(dev.DS_name, "Dev.HipAbduction"))
                {
                    checkbox6.IsChecked = false;
                    checkbox6.IsEnabled = false;
                }
            }
        }

        //定时任务
        Timer threadTimer;
        int times = 0;//发送次数
        private static bool isReceive = false;//是否收到回执
        private SerialPort serialPort;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //写卡
            TrainInfo trainInfo = new TrainService().GetTrainInfoByUserIdAndStatus(user.Pk_User_Id, (int)TrainInfoStatus.Normal);
            if (trainInfo != null)
            {
                //MessageBox.Show(LanguageUtils.ConvertLanguage("是否覆盖", "Whether or not to cover"));

                MessageBoxResult dr = MessageBox.Show(LanguageUtils.ConvertLanguage("是否覆盖", "Whether or not to cover?"), LanguageUtils.ConvertLanguage("提示", "Point"), MessageBoxButton.OKCancel,
                    MessageBoxImage.Question);
                if (dr == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            if (user != null)
            {
                byte[] data = new byte[90];
                //用户id
                string str = new UserService().getUserByUserId(user.Pk_User_Id).User_IDCard + "";
                Console.WriteLine(str);
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
                    data[position] = (byte)DeviceType.X01;
                    position += 1;
                }

                if (checkbox2.IsChecked == true)
                {
                    //坐姿划船机 0x05
                    data[position] = (byte)DeviceType.X05;
                    position += 1;
                }

                if (checkbox3.IsChecked == true)
                {
                    //身体伸展弯曲机 0x04
                    data[position] = (byte)DeviceType.X04;
                    position += 1;
                }

                if (checkbox4.IsChecked == true)
                {
                    //腿部伸展弯曲机 0x03
                    data[position] = (byte)DeviceType.X03;
                    position += 1;
                }

                if (checkbox5.IsChecked == true)
                {
                    //胸部推举机 0x06
                    data[position] = (byte)DeviceType.X06;
                    position += 1;
                }

                if (checkbox6.IsChecked == true)
                {
                    //腿部内外弯机 0x02
                    data[position] = (byte)DeviceType.X02;
                }

                Console.WriteLine("发卡的内容：" + ProtocolUtil.ByteToStringOk(data));

                byte[] send = ProtocolUtil.packHairpinData(0x01, data);

                //检查当前是否有多个串口
                if (SerialPortUtil.SerialPort == null)
                {
                    SerialPortUtil.CheckPort();
                }
                else
                {
                    SerialPortUtil.portName = SerialPortUtil.SerialPort.PortName;
                }

                if (SerialPortUtil.portName == "")
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("请先连接串口", "Please Connect the serial port"));
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
                        MessageBox.Show(LanguageUtils.ConvertLanguage("串口被占用", "Serial port is occupied"), LanguageUtils.ConvertLanguage("温馨提示", "Kindly Reminder "), MessageBoxButton.OK, MessageBoxImage.Warning);
                        //清空缓存
                        SerialPortUtil.SerialPort = null;
                        serialPort = null;
                        return;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(LanguageUtils.ConvertLanguage("串口不存在", "Serial port does not exist"), LanguageUtils.ConvertLanguage("温馨提示", "Kindly Reminder "), MessageBoxButton.OK, MessageBoxImage.Warning);
                        SerialPortUtil.SerialPort = null;
                        serialPort = null;
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
                            MessageBox.Show(LanguageUtils.ConvertLanguage("串口被占用", "Serial port is occupied"), LanguageUtils.ConvertLanguage("温馨提示", "Kindly Reminder "), MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(LanguageUtils.ConvertLanguage("串口不存在", "Serial port does not exist"), LanguageUtils.ConvertLanguage("温馨提示", "Kindly Reminder "), MessageBoxButton.OK, MessageBoxImage.Warning);
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
                            MessageBox.Show(LanguageUtils.ConvertLanguage("串口被占用", "Serial port is occupied"), LanguageUtils.ConvertLanguage("温馨提示", "Kindly Reminder "), MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(LanguageUtils.ConvertLanguage("串口不存在", "Serial port does not exist"), LanguageUtils.ConvertLanguage("温馨提示", "Kindly Reminder "), MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show(LanguageUtils.ConvertLanguage("设备长时间未应答，请查看是否选对串口，或设备未启动", "The device has not answered for a long time. Check whether the serial port is selected or the device is not started."));
                    Button_Write.IsEnabled = true;
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

                Console.WriteLine("收到数据："+ProtocolUtil.ByteToStringOk(receiveData));

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

                            //保存到数据库,在接收数据方法中
                            Dispatcher.Invoke(new Action(() =>
                            {
                                try
                                {
                                    SaveTrainInfo2DB(TrainInfoStatus.Normal);
                                }
                                catch (Exception exception)
                                {
                                    Console.WriteLine("捕获异常了");
                                    return;
                                }
                            }));
                            
                            MessageBox.Show(LanguageUtils.ConvertLanguage("写卡成功", "Write card success"));

                        }
                        else
                        {
                            //this.Close();
                            //Console.WriteLine("发卡失败");
                            MessageBox.Show(LanguageUtils.ConvertLanguage("写卡失败", "Failed to write to card"));
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //保存到数据库,在接收数据方法中
            try
            {
                SaveTrainInfo2DB(TrainInfoStatus.Normal);
            }
            catch (Exception exception)
            {
                Console.WriteLine("捕获异常了");
                return;
            }

            MessageBox.Show(LanguageUtils.ConvertLanguage("写卡成功", "Write card success"));
            this.Close();
        }
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

            if (String.IsNullOrEmpty(combobox_06.Text)|| LanguageUtils.EqualsResource(combobox_06.Text, "TrainingListView.Invalid"))
            {
                border1.Background = Brushes.White;
                border2.Background = Brushes.White;
                combobox_07.Visibility = Visibility.Hidden;
                border3.Background = Brushes.White;
                border4.Background = Brushes.White;
                combobox_08.Visibility = Visibility.Hidden;
                stackpanel.Margin = new Thickness(0, 149.8, 0, 0);
                t1.Height = 170;
            }
            else
            {
                border1.Background = Brushes.Gray;
                border2.Background = Brushes.Gray;
                combobox_07.Visibility = Visibility.Visible;
                border3.Background = Brushes.Gray;
                border4.Background = Brushes.Gray;
                combobox_08.Visibility = Visibility.Visible;
                stackpanel.Margin = new Thickness(0, 199.8, 0, 0);
                t1.Height = 120;
            }
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

        private void Checkbox1_OnChecked(object sender, RoutedEventArgs e)
        {
            //combobox_01.IsEnabled = false;
            combobox_01.SelectedIndex = 0;
            //combobox_02.IsEnabled = false;
            combobox_02.SelectedIndex = 0;
            //combobox_03.IsEnabled = false;
            combobox_03.SelectedIndex = 0;
            //combobox_04.IsEnabled = false;
            combobox_04.SelectedIndex = 0;
            //combobox_05.IsEnabled = false;
            combobox_05.SelectedIndex = 0;
            combobox_06.SelectedIndex = 1;
            select_change(sender, e);
            combobox_07.SelectedIndex = 0;
            combobox_08.SelectedIndex = 0;
            //com_01.IsEnabled = false;
            com_01.SelectedIndex = 0;
            //com_04.IsEnabled = false;
            com_04.SelectedIndex = 0;
            //com_02.IsEnabled = false;
            //com_03.IsEnabled = false;
            com_03.SelectedIndex = 0;
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
            //com_24.SelectedIndex = 0;
            //com_02.IsEnabled = false;
            //com_03.IsEnabled = false;
            com_23.SelectedIndex = 0;
            t3.Text = "";
        }private void Checkbox4_OnChecked(object sender, RoutedEventArgs e)
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
            t5.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveTrainInfo2DB(TrainInfoStatus.Normal);
        }
    }
}
