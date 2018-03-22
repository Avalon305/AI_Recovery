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
    /// InputTraining.xaml 的交互逻辑
    /// </summary>
    public partial class InputTraining : Window
    {
        private User user; 
        public InputTraining()
        {
            InitializeComponent();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否所有编辑都无效？", "提示", MessageBoxButton.OKCancel,
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
            com_02.IsEnabled = true;
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
            com_12.IsEnabled = true;
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
            com_22.IsEnabled = true;
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
            com_32.IsEnabled = true;
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
            com_42.IsEnabled = true;
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
            com_52.IsEnabled = true;
            com_53.IsEnabled = true;
        }

        private void checkbox1_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_01.IsEnabled = false;
            combobox_02.IsEnabled = false;
            combobox_03.IsEnabled = false;
            combobox_04.IsEnabled = false;
            combobox_05.IsEnabled = false;
            com_01.IsEnabled = false;
            com_04.IsEnabled = false;
            com_02.IsEnabled = false;
            com_03.IsEnabled = false;
        }

        private void checkbox2_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_11.IsEnabled = false;
            combobox_12.IsEnabled = false;
            combobox_13.IsEnabled = false;
            combobox_14.IsEnabled = false;
            combobox_15.IsEnabled = false;
            com_11.IsEnabled = false;
            com_12.IsEnabled = false;
            com_13.IsEnabled = false;
            com_14.IsEnabled = false;
        }

        private void checkbox3_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_21.IsEnabled = false;
            combobox_22.IsEnabled = false;
            combobox_23.IsEnabled = false;
            combobox_24.IsEnabled = false;
            combobox_25.IsEnabled = false;
            com_21.IsEnabled = false;
            com_24.IsEnabled = false;
            com_22.IsEnabled = false;
            com_23.IsEnabled = false;


        }

        private void checkbox4_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_31.IsEnabled = false;
            combobox_32.IsEnabled = false;
            combobox_33.IsEnabled = false;
            combobox_34.IsEnabled = false;
            combobox_35.IsEnabled = false;
            com_31.IsEnabled = false;
            com_34.IsEnabled = false;
            com_32.IsEnabled = false;
            com_33.IsEnabled = false;
            com_35.IsEnabled = false;
        }

        private void checkbox5_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_41.IsEnabled = false;
            combobox_42.IsEnabled = false;
            combobox_43.IsEnabled = false;
            combobox_44.IsEnabled = false;
            combobox_45.IsEnabled = false;
            com_41.IsEnabled = false;
            com_42.IsEnabled = false;
            com_43.IsEnabled = false;

        }

        private void checkbox6_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox_51.IsEnabled = false;
            combobox_52.IsEnabled = false;
            combobox_53.IsEnabled = false;
            combobox_54.IsEnabled = false;
            combobox_55.IsEnabled = false;
            com_51.IsEnabled = false;
            com_52.IsEnabled = false;
            com_53.IsEnabled = false;
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            SaveTrainInfo2DB(TrainInfoStatus.Save);
            MessageBox.Show("已存储");
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
                //水平腿部推蹬机
                devName = "水平腿部推蹬机";
                attr1 = com_01.Text; //属性1
                attr2 = com_02.Text; //2
                attr3 = com_03.Text; //3
                attr4 = com_04.Text; //4

                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = "attr1-" + attr1 + "*" +
                                              "attr2-" + attr2 + "*" +
                                              "attr3-" + attr3 + "*" +
                                              "attr4-" + attr4 + "*";
                devicePrescription.DP_Memo = t1.Text; //注意点
                devicePrescription.Fk_DS_Id = deviceSortDao.GetByName(devName).Pk_DS_Id;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(combobox_01.Text); //组数;
                devicePrescription.dp_groupnum = Convert.ToInt32(combobox_02.Text); //个数;
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_05.Text)); //移乘方式
                devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_03.Text); //间隔时间;
                devicePrescription.dp_weight = Convert.ToDouble(combobox_04.Text); //砝码;
                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            if (checkbox2.IsChecked == true)
            {
                //坐姿划船机
                devName = "坐姿划船机";
                attr1 = com_11.Text; //属性1
                attr2 = com_12.Text; //2
                attr3 = com_13.Text; //3
                attr4 = com_14.Text; //4

                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = "attr1-" + attr1 + "*" +
                                              "attr2-" + attr2 + "*" +
                                              "attr3-" + attr3 + "*" +
                                              "attr4-" + attr4 + "*";
                devicePrescription.DP_Memo = t2.Text; //注意点
                devicePrescription.Fk_DS_Id = deviceSortDao.GetByName(devName).Pk_DS_Id;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(combobox_11.Text); //组数
                devicePrescription.dp_groupnum = Convert.ToInt32(combobox_12.Text); //个数
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_15.Text)); //移乘方式
                devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_13.Text); //间隔时间
                devicePrescription.dp_weight = Convert.ToDouble(combobox_14.Text); //砝码
                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            if (checkbox3.IsChecked == true)
            {
                //身体伸展弯曲机
                devName = "身体伸展弯曲机";
                attr1 = com_21.Text; //属性1
                attr2 = com_22.Text; //2
                attr3 = com_23.Text; //3
                attr4 = com_24.Text; //4
                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = "attr1-" + attr1 + "*" +
                                              "attr2-" + attr2 + "*" +
                                              "attr3-" + attr3 + "*" +
                                              "attr4-" + attr4 + "*";
                devicePrescription.DP_Memo = t3.Text; //注意点
                devicePrescription.Fk_DS_Id = deviceSortDao.GetByName(devName).Pk_DS_Id;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(combobox_21.Text); //组数
                devicePrescription.dp_groupnum = Convert.ToInt32(combobox_22.Text); //个数
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_25.Text)); //移乘方式
                devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_23.Text); //间隔时间
                devicePrescription.dp_weight = Convert.ToDouble(combobox_24.Text); //砝码
                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            if (checkbox4.IsChecked == true)
            {
                //腿部伸展弯曲机
                devName = "腿部伸展弯曲机";
                attr1 = com_31.Text; //属性1
                attr2 = com_32.Text; //2
                attr3 = com_33.Text; //3
                attr4 = com_34.Text; //4
                attr5 = com_35.Text; //5

                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = "attr1-" + attr1 + "*" +
                                              "attr2-" + attr2 + "*" +
                                              "attr3-" + attr3 + "*" +
                                              "attr4-" + attr4 + "*" +
                                              "attr5-" + attr5 + "*";
                devicePrescription.DP_Memo = t4.Text; //注意点
                devicePrescription.Fk_DS_Id = deviceSortDao.GetByName(devName).Pk_DS_Id;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(combobox_31.Text); //组数
                devicePrescription.dp_groupnum = Convert.ToInt32(combobox_32.Text); //个数
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_35.Text)); //移乘方式
                devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_33.Text); //间隔时间
                devicePrescription.dp_weight = Convert.ToDouble(combobox_34.Text); //砝码
                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            if (checkbox5.IsChecked == true)
            {
                //臀部外展内收机
                devName = "臀部外展内收机";
                attr1 = com_41.Text; //属性1
                attr2 = com_42.Text; //2
                attr3 = com_43.Text; //3

                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = "attr1-" + attr1 + "*" +
                                              "attr2-" + attr2 + "*" +
                                              "attr3-" + attr3 + "*";
                devicePrescription.DP_Memo = t5.Text; //注意点
                devicePrescription.Fk_DS_Id = deviceSortDao.GetByName(devName).Pk_DS_Id;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(combobox_41.Text); //组数
                devicePrescription.dp_groupnum = Convert.ToInt32(combobox_42.Text); //个数
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_45.Text)); //移乘方式
                devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_43.Text); //间隔时间
                devicePrescription.dp_weight = Convert.ToDouble(combobox_44.Text); //砝码
                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            if (checkbox6.IsChecked == true)
            {
                //胸部推举机
                devName = "胸部推举机";
                attr1 = com_51.Text; //属性1
                attr2 = com_52.Text; //2
                attr3 = com_53.Text; //3

                //构建对象
                DevicePrescription devicePrescription = new DevicePrescription();
                devicePrescription.DP_Attrs = "attr1-" + attr1 + "*" +
                                              "attr2-" + attr2 + "*" +
                                              "attr3-" + attr3 + "*";
                devicePrescription.DP_Memo = t6.Text; //注意点
                devicePrescription.Fk_DS_Id = deviceSortDao.GetByName(devName).Pk_DS_Id;
                devicePrescription.Gmt_Create = DateTime.Now;
                devicePrescription.Gmt_Modified = DateTime.Now;
                devicePrescription.dp_groupcount = Convert.ToInt32(combobox_51.Text); //组数
                devicePrescription.dp_groupnum = Convert.ToInt32(combobox_52.Text); //个数
                devicePrescription.dp_moveway = Convert.ToInt32(DataCodeCache.GetInstance().GetCodeSValue(DataCodeTypeEnum.MoveWay, combobox_55.Text)); //移乘方式
                devicePrescription.dp_relaxtime = Convert.ToInt32(combobox_53.Text); //间隔时间
                devicePrescription.dp_weight = Convert.ToDouble(combobox_54.Text); //砝码
                devicePrescription.Dp_status = 0;
                devicePrescriptions.Add(devicePrescription);
            }

            TrainInfo trainInfo = new TrainInfo();
            trainInfo.Gmt_Create = DateTime.Now;
            trainInfo.Gmt_Modified = DateTime.Now;
            trainInfo.FK_User_Id = user.Pk_User_Id;
            trainInfo.Status = (int) status;
            //存储到数据库
            new TrainService().SaveTraininfo(trainInfo, devicePrescriptions);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            com_01.ItemsSource = Add(0, 700, 2);
            com_02.ItemsSource = Add(0, 30, 2);
            com_03.ItemsSource = Add(1, 5, 2);
            com_04.ItemsSource = Add(1, 9, 2);
            combobox_01.ItemsSource = Add(1, 3, 2);
            combobox_02.ItemsSource = Add(1, 20, 2);
            combobox_03.ItemsSource = Add(1, 60, 2);
            combobox_04.ItemsSource = Add(1.0, 84.0, 2);
            combobox_05.ItemsSource = dataItems;

            com_11.ItemsSource = Add(0, 400, 2);
            com_12.ItemsSource = Add(0, 30, 1);
            com_13.ItemsSource = Add(1, 4, 2);
            com_14.ItemsSource = Add(1, 4, 2);
            combobox_11.ItemsSource = Add(1, 3, 2);
            combobox_12.ItemsSource = Add(1, 20, 2);
            combobox_13.ItemsSource = Add(1, 60, 2);
            combobox_14.ItemsSource = Add(1.0, 32.0, 1);
            combobox_15.ItemsSource = dataItems;

            com_21.ItemsSource = Add(0, 700, 2);
            com_22.ItemsSource = Add(0, 30, 1);
            com_23.ItemsSource = Add(1, 6, 2);
            com_24.ItemsSource = Add(1, 9, 2);
            combobox_21.ItemsSource = Add(1, 3, 2);
            combobox_22.ItemsSource = Add(1, 20, 2);
            combobox_23.ItemsSource = Add(1, 60, 2);
            combobox_24.ItemsSource = Add(0.5, 32.0, 1);
            combobox_25.ItemsSource = dataItems;

            com_31.ItemsSource = Add(0, 260, 1);
            com_32.ItemsSource = Add(0, 30, 1);
            com_33.ItemsSource = Add(1, 5, 2);
            com_34.ItemsSource = Add(1, 5, 2);
            com_35.ItemsSource = Add(1, 9, 2);
            combobox_31.ItemsSource = Add(1, 3, 2);
            combobox_32.ItemsSource = Add(1, 20, 2);
            combobox_33.ItemsSource = Add(1, 60, 2);
            combobox_34.ItemsSource = Add(1.0, 32.0, 1);
            combobox_35.ItemsSource = dataItems;

            com_41.ItemsSource = Add(0, 500, 2);
            com_42.ItemsSource = Add(0, 30, 1);
            com_43.ItemsSource = Add(1, 9, 2);
            combobox_41.ItemsSource = Add(1, 3, 2);
            combobox_42.ItemsSource = Add(1, 20, 2);
            combobox_43.ItemsSource = Add(1, 60, 2);
            combobox_44.ItemsSource = Add(1.0, 32.0, 1);
            combobox_45.ItemsSource = dataItems;

            com_51.ItemsSource = Add(0, 180, 2);
            com_52.ItemsSource = Add(0, 30, 1);
            com_53.ItemsSource = Add(1, 8, 2);
            combobox_51.ItemsSource = Add(1, 3, 2);
            combobox_52.ItemsSource = Add(1, 20, 2);
            combobox_53.ItemsSource = Add(1, 60, 2);
            combobox_54.ItemsSource = Add(1.0, 32.0, 1);
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
                string devName = new DeviceSortDAO().Load(devicePrescription.Fk_DS_Id).DS_name;
                switch (devName)
                {
                    case "水平腿部推蹬机":
                        checkbox1.IsChecked = true;
                        //设置属性
                        com_01.Text = attrs[0].Split(new char[] {'-'})[1];
                        com_02.Text = attrs[1].Split(new char[] {'-'})[1];
                        com_03.Text = attrs[2].Split(new char[] {'-'})[1];
                        com_04.Text = attrs[3].Split(new char[] {'-'})[1];
                        //设置处方信息
                        combobox_01.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_02.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_03.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_04.Text = devicePrescription.dp_weight.ToString();
                        combobox_05.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t1.Text = devicePrescription.DP_Memo;
                        break;
                    case "坐姿划船机":
                        checkbox2.IsChecked = true;
                        com_11.Text = attrs[0].Split(new char[] {'-'})[1];
                        com_12.Text = attrs[1].Split(new char[] {'-'})[1];
                        com_13.Text = attrs[2].Split(new char[] {'-'})[1];
                        com_14.Text = attrs[3].Split(new char[] {'-'})[1];
                        combobox_11.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_12.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_13.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_14.Text = devicePrescription.dp_weight.ToString();
                        combobox_15.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t2.Text = devicePrescription.DP_Memo;
                        break;
                    case "身体伸展弯曲机":
                        checkbox3.IsChecked = true;
                        com_21.Text = attrs[0].Split(new char[] {'-'})[1];
                        com_22.Text = attrs[1].Split(new char[] {'-'})[1];
                        com_23.Text = attrs[2].Split(new char[] {'-'})[1];
                        com_24.Text = attrs[3].Split(new char[] {'-'})[1];
                        combobox_21.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_22.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_23.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_24.Text = devicePrescription.dp_weight.ToString();
                        combobox_25.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t3.Text = devicePrescription.DP_Memo;
                        break;
                    case "腿部伸展弯曲机":
                        checkbox4.IsChecked = true;
                        com_31.Text = attrs[0].Split(new char[] {'-'})[1];
                        com_32.Text = attrs[1].Split(new char[] {'-'})[1];
                        com_33.Text = attrs[2].Split(new char[] {'-'})[1];
                        com_34.Text = attrs[3].Split(new char[] {'-'})[1];
                        com_35.Text = attrs[4].Split(new char[] {'-'})[1];
                        combobox_31.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_32.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_33.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_34.Text = devicePrescription.dp_weight.ToString();
                        combobox_35.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t4.Text = devicePrescription.DP_Memo;
                        break;
                    case "臀部外展内收机":
                        checkbox5.IsChecked = true;
                        com_41.Text = attrs[0].Split(new char[] {'-'})[1];
                        com_42.Text = attrs[1].Split(new char[] {'-'})[1];
                        com_43.Text = attrs[2].Split(new char[] {'-'})[1];
                        combobox_41.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_42.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_43.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_44.Text = devicePrescription.dp_weight.ToString();
                        combobox_45.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t5.Text = devicePrescription.DP_Memo;
                        break;
                    case "胸部推举机":
                        checkbox6.IsChecked = true;
                        com_51.Text = attrs[0].Split(new char[] {'-'})[1];
                        com_52.Text = attrs[1].Split(new char[] {'-'})[1];
                        com_53.Text = attrs[2].Split(new char[] {'-'})[1];
                        combobox_51.Text = devicePrescription.dp_groupcount.ToString();
                        combobox_52.Text = devicePrescription.dp_groupnum.ToString();
                        combobox_53.Text = devicePrescription.dp_relaxtime.ToString();
                        combobox_54.Text = devicePrescription.dp_weight.ToString();
                        combobox_55.Text = DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString());
                        t6.Text = devicePrescription.DP_Memo;
                        break;
                }

            }
        }
    }
}
