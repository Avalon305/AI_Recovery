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
        public delegate void OnPortDataReceived(Object sender, SerialDataReceivedEventArgs e);

        /// <summary>
        /// 获取串口
        /// </summary>
        /// <param name="portName">串口名</param>
        /// <param name="onPortDataReceived">串口监听方法</param>
        /// <returns></returns>
        public static SerialPort ConnectSerialPort(string portName, OnPortDataReceived onPortDataReceived)
        {
            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = 115200;
            serialPort.ReadTimeout = 3000; //单位毫秒
            serialPort.WriteTimeout = 3000; //单位毫秒
            serialPort.ReceivedBytesThreshold = 1;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(onPortDataReceived);

            return serialPort;
        }
    }
}
