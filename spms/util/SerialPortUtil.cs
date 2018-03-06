using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace spms.util
{
    class SerialPortUtil
    {
        private static SerialPort MySerialPort;
        public delegate void OnPortDataReceived(Object sender, SerialDataReceivedEventArgs e);

        public static SerialPort ConnectSerialPort(string PortName, OnPortDataReceived MyOnPortDataReceived)
        {
            MySerialPort = new SerialPort();
            MySerialPort.PortName = PortName;
            MySerialPort.BaudRate = 115200;
            MySerialPort.ReadTimeout = 3000; //单位毫秒
            MySerialPort.WriteTimeout = 3000; //单位毫秒
            MySerialPort.ReceivedBytesThreshold = 1;
            MySerialPort.DataReceived += new SerialDataReceivedEventHandler(MyOnPortDataReceived);

            return MySerialPort;
        }
    }
}
