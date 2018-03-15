using spms.http.entity;
using spms.util;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using spms.service;
using spms.http.dto;
using spms.dao;
using spms.entity;
using spms.protocol;
using spms.constant;
using NLog;

namespace spms
{
    /// <summary>
    /// zeroTest.xaml 的交互逻辑
    /// </summary>
    public partial class zeroTest : Page
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public zeroTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string pingJsonStr = JsonTools.Obj2JSONStrNew(new HttpHeartBeat("ping", "ping"));
            MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string pingJsonStr = JsonTools.Obj2JSONStrNew(new UploadManagementService().ListLimit30());
                 MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AutherDTO autherDTO = new AutherDTO();
            autherDTO.username = "XXX";
            string pingJsonStr = JsonTools.Obj2JSONStrNew(autherDTO);
            MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AuthDAO authDAO = new AuthDAO();
            Auther auther = authDAO.GetByAuthLevel(Auther.AUTH_LEVEL_ADMIN);
            string pingJsonStr = JsonTools.Obj2JSONStrNew(auther);
            MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            byte[] data = { 0x76, 0x7E, 0x12, 0x7D, 0x77 };
            byte[] result =  MakerTCPFrame.GetInstance().PackData(MsgId.X8001,2, "123456789012", data);
            byte[] buffer = ProtocolUtil.UnTransfer(result);
           // MessageBox.Show(ProtocolUtil.BytesToString(buffer));
            MsgId msgId =  ProtocolUtil.BytesToMsgId(buffer, 1);
            Int16 data_len = BitConverter.ToInt16(buffer, 3);
           // MessageBox.Show(ProtocolUtil.XorByByte(buffer, 1, 12 + data_len).ToString());
            var ra =MsgId.X0001;
            var bbb = MakerTCPFrame.GetInstance().Make0001Frame();
            logger.Info(ProtocolUtil.BytesToString(bbb));
        }
    }
}
