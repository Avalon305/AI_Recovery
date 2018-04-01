using spms.constant;
using spms.protocol;
using spms.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
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

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// InputNonPublicInformationPassword.xaml 的交互逻辑
    /// </summary>
    public partial class InputNonPublicInformationPassword : Window
    {
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        public InputNonPublicInformationPassword()
        {
            InitializeComponent();
        }

        //定时任务
        Timer threadTimer;
        int times = 0;//发送次数
        bool isReceive = false;//是否收到回执
        private SerialPort serialPort;
        //确定按钮
        private void Determine(object sender, RoutedEventArgs e)
        {
            String password = NonPublicInformationPassword.Password;
            if ("111" == password)
            {
                //与U盘交互
                //1.打包协议到result;
                byte[] send = null;
                //获取电脑的uuid字节数组-即加密狗的内容
                ProtocolConstant.USB_DOG_CONTENT = Encoding.UTF8.GetBytes(Get_UUID());
                new MakerUSBDogFrame().PackData(ref send, new byte[] { 0xF0 }, ProtocolConstant.USB_DOG_CONTENT);
                //Console.WriteLine("加密后的报文" + ProtocolUtil.ByteToStringOk(send));
                //byte[] test = null;
                //new MakerUSBDogFrame().PackData(ref test, new byte[] { 0xF0 }, Encoding.UTF8.GetBytes("hello"));
                //Console.WriteLine("测试:"+CRC16Util.ByteToStringOk(test));

                //2.判断当前是否已经连接过串口
                SerialPortUtil.CheckPort();
                if (SerialPortUtil.portName == "")
                {
                    MessageBox.Show("请先连接串口");
                    return;
                }
                if (SerialPortUtil.SerialPort != null)
                {
                    SerialPortUtil.SerialPort = null;
                }
                if (serialPort == null)
                {
                    serialPort = SerialPortUtil.ConnectSerialPort(OnPortDataReceived);

                    try
                    {
                        serialPort.Open();
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        MessageBox.Show("串口被占用", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("串口不存在", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.Close();
                        return;
                    }

                    serialPort.Write(send, 0, send.Length);

                    //发送的定时器
                    threadTimer = new System.Threading.Timer(new System.Threading.TimerCallback(ReissueThreeTimes), send, 500, 500);
                }
                //else
                //{
                //    if (!serialPort.IsOpen)
                //    {
                //        try
                //        {
                //            serialPort.Open();
                //        }
                //        catch (UnauthorizedAccessException ex)
                //        {
                //            MessageBox.Show("串口被占用", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                //            return;
                //        }
                //        catch (IOException ex)
                //        {
                //            MessageBox.Show("串口不存在", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                //            return;
                //        }
                //    }
                //}
            }
            else {
                MessageBox.Show("密码错误，请重新输入");
            }
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
                            MessageBox.Show("串口被占用", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show("串口不存在", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("设备长时间未应答，请查看是否选对串口，或设备未启动");
                    this.Close();
                }));
            }
            else
            {
                threadTimer.Dispose();
            }
        }

        /// <summary>
        /// 获取设备的uuid
        /// </summary>
        /// <returns></returns>
        private string Get_UUID ()
        {
            string code = null;
            SelectQuery query = new SelectQuery("select * from Win32_ComputerSystemProduct");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (var item in searcher.Get())
                {
                    using (item) code = item["UUID"].ToString();
                }
            }
            //Console.WriteLine("设备的uuid:"+code);
            return code;
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
                isReceive = true;//收到数据，取消重发

                byte[] buffer = null; ;
                int len = serialPort.BytesToRead;

                byte[] receiveData = new byte[len];
                serialPort.Read(receiveData, 0, len);
                //Console.WriteLine("收到的数据：" + ProtocolUtil.ByteToStringOk(receiveData));
                int offset = 0;

                Int32 datalen = 0;
                if (len > 0 && receiveData[0] == 0xAA)//第一包数据
                {
                    datalen = Convert.ToInt32((receiveData[2].ToString("X2") + receiveData[3].ToString("X2")), 16);
                    //Console.WriteLine("数据的长度：" + datalen);
                    buffer = new byte[datalen + 6];

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
                    byte[] data = new byte[datalen + 3];
                    Array.Copy(buffer, 1, data, 0, data.Length);
                    if (buffer[buffer.Length - 2] == ProtocolUtil.XorByByte(data))
                    {
                        object result = null;//用于存放uuid的鉴权加密
                        new ParserUSBDogFrame().Parser(ref result, buffer);
                        string b = ProtocolUtil.BytesToString((byte[])result);
                        //Console.WriteLine("解密通讯加密后的数据：" + b);

                        byte[] uuidBytes = null;
                        uuidBytes = AesUtil.Decrypt((byte[])result, ProtocolConstant.USB_DOG_AUTH_PASSWORD);
                        //Console.WriteLine("解密鉴权加密后的数据：" + ProtocolUtil.ByteToStringOk(uuidBytes));

                        string strUUID = System.Text.Encoding.ASCII.GetString(uuidBytes);

                        if (strUUID == Get_UUID())
                        {
                            //MessageBox.Show("激活成功");
                            //todo 全局变量
                            ProtocolConstant.USB_SUCCESS = 1;
                            MessageBox.Show("激活成功");
                            Console.WriteLine("激活成功");
                        }
                        else
                        {
                            //MessageBox.Show("激活失败");
                            Console.WriteLine("激活失败");
                        }
                    }
                    else
                    {
                        Console.WriteLine("校验失败");
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //收到消息后至空串口并关闭
                SerialPortUtil.ClosePort(ref serialPort);
                Dispatcher.Invoke(new Action(() =>
                {
                    this.Close();
                }));
            }
        }



        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //错误：OnlyInputNumbers
        //设置手机号输入框只能输入数字
        private void OnlyInputNumbers(object sender, TextCompositionEventArgs e)
        {
            inputlimited.InputLimited.OnlyInputNumbers(e);
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Determine(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }

    }
}
