using NLog;
using spms.dao;
using spms.entity;
using spms.server;
using spms.service;
using spms.util;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace spms
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
    private static Logger logger = LogManager.GetCurrentClassLogger();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logger.Warn("测试{0}参数{1}","1","2");
 
            try
            {
            new AuthService().updateTest();

            }catch(Exception ee)
            {

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            entity.Setter setter = new SetterService().getSetter();
            MessageBox.Show(setter.Set_OrganizationSort.ToString()+"-");
        }

        private SerialPort serialPort;
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[2] { 0x11, 0x12 };
            if (serialPort == null)
            {
                serialPort = util.SerialPortUtil.ConnectSerialPort("COM3", OnPortDataReceived);
                serialPort.Open();
            }

            serialPort.Write(data, 0, data.Length);
        }

        private void OnPortDataReceived(Object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(50);

                byte[] buffer = null; ;
                int len = serialPort.BytesToRead;

                byte[] ReceiveData = new byte[len];
                serialPort.Read(ReceiveData, 0, len);
                string str = Encoding.Default.GetString(ReceiveData);
                Console.WriteLine(str);
            }
            catch (Exception ex)
            {
            }
        }
        private void Buttonlz_Click_2(object sender, RoutedEventArgs e)
        {
            entity.Setter setter = new entity.Setter();
            setter.Pk_Set_Id = 1;
            setter.Set_Language = entity.Setter.SET_LANGUAGE_CHINA;
            setter.Set_OrganizationName = "1";
            setter.Set_OrganizationPhone = "2";
            setter.Set_OrganizationSort = "3";
            setter.Set_PhotoLocation = "4";
            //自封装
            //string str = JsonTools.Obj2JSONStr<entity.Setter>(setter);
            //blog
            string str = JsonTools.Obj2JSONStrNew(setter);
            MessageBox.Show(str);
        }

        private void Buttonlz_Click_3(object sender, RoutedEventArgs e)
        {
            entity.Setter setter = new entity.Setter();
            setter.Pk_Set_Id = 1;
            setter.Set_Language = entity.Setter.SET_LANGUAGE_CHINA;
            setter.Set_OrganizationName = "1";
            setter.Set_OrganizationPhone = "2";
            setter.Set_OrganizationSort = "3";
            setter.Set_PhotoLocation = "4";


            entity.Setter setter2 = new entity.Setter();
            setter2.Pk_Set_Id = 1;
            setter2.Set_Language = entity.Setter.SET_LANGUAGE_CHINA;
            setter2.Set_OrganizationName = "1";
            setter2.Set_OrganizationPhone = "2";
            setter2.Set_OrganizationSort = "3";
            setter2.Set_PhotoLocation = "4";
            List<entity.Setter> list = new List<entity.Setter>();
            list.Add(setter);
            list.Add(setter2);
            //string str = JsonTools.List2JSONStr<entity.Setter>(list);
            string str = JsonTools.List2JSONStrNew(list);
            MessageBox.Show(str);
        }
    }
}
