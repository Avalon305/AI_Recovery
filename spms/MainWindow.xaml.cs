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
                serialPort = SerialPortUtil.ConnectSerialPort("COM3", OnPortDataReceived);
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
    }
}
