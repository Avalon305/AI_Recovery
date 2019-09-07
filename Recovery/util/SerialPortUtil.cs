using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Recovery.view.Pages.ChildWin;

namespace Recovery.util
{
    /// <summary>
    /// 串口操作类
    /// </summary>
    class SerialPortUtil
    {
        private static SerialPort serialPort;
        //当前的串口号
        public static string portName = "";
        //定义可以操作当前串口的方法
        public static SerialPort SerialPort { get => serialPort; set => serialPort = value; }

        public delegate void OnPortDataReceived(Object sender, SerialDataReceivedEventArgs e);

        /// <summary>
        /// 获取串口
        /// </summary>
        /// <param name="portName">串口名</param>
        /// <param name="onPortDataReceived">串口监听方法</param>
        /// <returns></returns>
        public static SerialPort ConnectSerialPort(OnPortDataReceived onPortDataReceived)
        {
            if (SerialPort == null)
            {
                if (portName != "")
                {
                    SerialPort = new SerialPort();
                    SerialPort.PortName = portName;
                    SerialPort.BaudRate = 115200;
                    SerialPort.ReadTimeout = 3000; //单位毫秒
                    SerialPort.WriteTimeout = 3000; //单位毫秒
                    SerialPort.ReceivedBytesThreshold = 1;
                    SerialPort.DataReceived += new SerialDataReceivedEventHandler(onPortDataReceived);
                }
            }
     
            return SerialPort;
        }


        /// <summary>
        /// 检查当前是否有多个串口
        /// </summary>
        public static void CheckPort()
        {
            string[] names = SerialPort.GetPortNames();
            if (names.Length == 1)
            {
                portName = names[0];
            }
            else if (names.Length == 0)
            {
                portName = "";
            }
            else
            {
                SerialPortSelection serialPortSelection = new SerialPortSelection();
                serialPortSelection.datalist.DataContext = names;
                serialPortSelection.Top = 200;
                serialPortSelection.Left = 500;
                serialPortSelection.ShowDialog();
            }
        }

        public static void ClosePort(ref SerialPort serialPort)
        {
            if (serialPort != null)
            {
                try
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Close();
                        Console.WriteLine("串口正常关闭");
                    }
                    serialPort = null;
                    SerialPort = null;
                }
                catch (Exception e)
                {
                    Console.WriteLine("串口关闭异常");
                }
            }
        }

    }
}
