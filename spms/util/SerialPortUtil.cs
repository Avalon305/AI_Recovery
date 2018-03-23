using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace spms.util
{
    /// <summary>
    /// 串口操作类
    /// </summary>
    class SerialPortUtil
    {
        private static SerialPort serialPort;

        //定义可以操作当前串口的方法
        public static SerialPort SerialPort { get => serialPort; set => serialPort = value; }

        public delegate void OnPortDataReceived(Object sender, SerialDataReceivedEventArgs e);

        /// <summary>
        /// 获取串口
        /// </summary>
        /// <param name="portName">串口名</param>
        /// <param name="onPortDataReceived">串口监听方法</param>
        /// <returns></returns>
        public static SerialPort ConnectSerialPort(string portName, OnPortDataReceived onPortDataReceived)
        {
            if (SerialPort == null)
            {
                SerialPort = new SerialPort();
                SerialPort.PortName = portName;
                SerialPort.BaudRate = 115200;
                SerialPort.ReadTimeout = 3000; //单位毫秒
                SerialPort.WriteTimeout = 3000; //单位毫秒
                SerialPort.ReceivedBytesThreshold = 1;
                SerialPort.DataReceived += new SerialDataReceivedEventHandler(onPortDataReceived);
            }
     
            return SerialPort;
        }
    }
}
