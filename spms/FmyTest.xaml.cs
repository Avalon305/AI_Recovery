using spms.constant;
using spms.dao;
using spms.entity;
using spms.protocol;
using spms.util;
using spms.view;
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

namespace spms
{
    /// <summary>
    /// FmyTest.xaml 的交互逻辑
    /// </summary>
    public partial class FmyTest : Window
    {
        public FmyTest()
        {
            InitializeComponent();
        }
 
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            byte[] idcard = Encoding.GetEncoding("GBK").GetBytes("370111111111111115");

            byte[] req = MakerTCPFrame.GetInstance().PackData(MsgId.X0006, 1, "123456789012", idcard);
            byte[] request = new byte[req.Length - 2];
            Array.Copy(req, 1, request, 0, req.Length - 2);

            new ParserTCPFrame().Parser(request);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] idcard = Encoding.GetEncoding("GBK").GetBytes("370111111111111115");

            byte[] req = MakerTCPFrame.GetInstance().PackData(MsgId.X000A, 1, "123456789012", idcard);
            byte[] request = new byte[req.Length - 2];
            Array.Copy(req, 1, request, 0, req.Length - 2);

            new ParserTCPFrame().Parser(request);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            byte[] body = new byte[34];
            byte[] idcard = Encoding.GetEncoding("GBK").GetBytes("370111111111111115");
            Array.Copy(idcard, 0, body, 0, idcard.Length);
            body[32] = 0x01;
            body[33] = 0x00;

            byte[] req = MakerTCPFrame.GetInstance().PackData(MsgId.X0007, 1, "123456789012", body);
            byte[] request = new byte[req.Length - 2];
            Array.Copy(req, 1, request, 0, req.Length - 2);

            new ParserTCPFrame().Parser(request);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new MakerTCPFrame.MakePrescription().Make8008Frame("222", DeviceType.X02);
            
        }
    }
}
