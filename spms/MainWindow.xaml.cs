using NLog;
using spms.dao;
using spms.entity;
using spms.server;
using spms.service;
using spms.util;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFMediaKit.DirectShow.Controls;
using spms.constant;
using spms.http;
using spms.protocol;
using spms.view.Pages.ChildWin;

namespace spms
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private BigDataOfficer bigDataOfficer;
        private string path = null;
        public MainWindow()
        {
            InitializeComponent();
            bigDataOfficer = new BigDataOfficer(12);
            // 初始化摄像头
            Camera_CB.ItemsSource = MultimediaUtil.VideoInputNames;
            if (MultimediaUtil.VideoInputNames.Length > 0)
            {
                Camera_CB.SelectedIndex = 0;//第0个摄像头为默认摄像头
            }
            else
            {
                System.Windows.MessageBox.Show("电脑没有安装任何可用摄像头");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logger.Warn("测试{0}参数{1}", "1", "2");

            try
            {
                new AuthService().updateTest();

            }
            catch (Exception ee)
            {

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            entity.Setter setter = new SetterService().getSetter();
            System.Windows.MessageBox.Show(setter.Set_OrganizationSort.ToString() + "-");
        }

        /// <summary>
        /// 串口测试
        /// </summary>
        private SerialPort serialPort;
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[2] { 0x11, 0x12 };
            if (serialPort == null)
            {
                serialPort = util.SerialPortUtil.ConnectSerialPort(OnPortDataReceived);
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
            System.Windows.MessageBox.Show(str);
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
            System.Windows.MessageBox.Show(str);
        }

        //选择照片文件的储存路径
        private void Select_Path_Click(object sender, RoutedEventArgs e)
        {

            // FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.Description = "选择照片储存路径";

            //  if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //  {
            //      path = fbd.SelectedPath;
            // }

        }

        //拍照按钮
        private void Capture_Click_Btn(object sender, RoutedEventArgs e)
        {

            //vce是前台wpfmedia控件的name
            //为避免抓不全的情况，需要在Render之前调用Measure、Arrange
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)VCE.ActualWidth,
                (int)VCE.ActualHeight, 96, 96, PixelFormats.Default);

            VCE.Measure(VCE.RenderSize);
            VCE.Arrange(new Rect(VCE.RenderSize));

            bmp.Render(VCE);

            //这里需要创建一个流以便存储摄像头拍摄到的图片。
            //当然，可以使文件流，也可以使内存流。
            BitmapEncoder encoder = new JpegBitmapEncoder();
            // 获取图像的帧
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            Console.WriteLine(path + "1.jpg");

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                byte[] captureData = ms.ToArray();
                File.WriteAllBytes(path + "/1" + Guid.NewGuid().ToString().Substring(0, 5) + ".jpg", captureData);
                //@"./photo/" + "1" + Guid.NewGuid().ToString().Substring(0, 5) + 
                //@"./photo/" + Guid.NewGuid().ToString().Substring(0, 5) + ".jpg"
            }
            VCE.Pause();
        }

        //加载ComBox中的摄像头选择
        private void SelectionChanged_CB(object sender, SelectionChangedEventArgs e)
        {
            //vce是前台wpfmedia控件的name
            VCE.VideoCaptureSource = (string)Camera_CB.SelectedItem;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //打包协议到result;
            byte[] result = null;
            ProtocolConstant.USB_DOG_CONTENT = new byte[] { 0x0A, 0x0B, 0x0C };
            new MakerUSBDogFrame().PackData(ref result, new byte[] { 0xFF }, ProtocolConstant.USB_DOG_CONTENT);
            string a = ProtocolUtil.BytesToString(result);
            MessageBox.Show(a);

            //解析这个协议
            object rr = null;
            new ParserUSBDogFrame().Parser(ref rr, result);
            string b = ProtocolUtil.BytesToString((byte[])rr);
            //解析出的数据体
            MessageBox.Show(b);
            //是否和发送的数据体相等
            MessageBox.Show(ProtocolUtil.ArrayEqual((byte[])rr, ProtocolConstant.USB_DOG_CONTENT).ToString());

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
//            bigDataOfficer.Run();
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            PicView picView = new PicView();
            picView.Show();
        }
    }
}
