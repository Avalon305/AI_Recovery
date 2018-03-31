using spms.constant;
using spms.dao;
using spms.dao.app;
using spms.entity;
using spms.http.dto;
using spms.http.entity;
using spms.service;
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
using static spms.entity.CustomData;

namespace spms
{
    /// <summary>
    /// ganDongPinMing.xaml 的交互逻辑
    /// </summary>
    public partial class ganDongPinMing : Page
    {
        public ganDongPinMing()
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


            //已删除，按照冻结处理
            MessageBox.Show("用户被删除，即将退出，请联系宝德龙管理员恢复！");
            Environment.Exit(0);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            AuthDAO authDAO = new AuthDAO();
            Auther auther = authDAO.Login("123", "123");
            string pingJsonStr = JsonTools.Obj2JSONStrNew(auther);
            MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(SystemInfo.GetMacAddress());

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            string pingJsonStr = JsonTools.Obj2JSONStrNew(userService.GetAllUsers());
            MessageBox.Show(pingJsonStr);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
           DateTime dateTime = Convert.ToDateTime("2000/02/12");
            MessageBox.Show(dateTime.ToShortDateString());
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            User user = userService.GetByIdCard("438");
            List<User> queryResult = userService.SelectByCondition(user);
            foreach (var i in queryResult) {
                MessageBox.Show(JsonTools.Obj2JSONStrNew(i));
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(DataCodeTypeEnum.Diagiosis.ToString());
        }
        /// <summary>
        /// 测试获得自定义类型的list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
             

            CustomDataDAO customDataDAO = new CustomDataDAO();
            Console.WriteLine(CustomDataEnum.Disease.ToString());
            MessageBox.Show(JsonTools.Obj2JSONStrNew(customDataDAO.GetListByTypeID(CustomDataEnum.Diagiosis)));
        }
        /// <summary>
        /// 测试自定义模糊查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            CustomDataDAO customDataDAO = new CustomDataDAO();
            MessageBox.Show(JsonTools.Obj2JSONStrNew(customDataDAO.GetExistByValue(CustomDataEnum.Group,"C")));
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ba123123".IndexOf("a123").ToString());
        }
        //获得aes加密insert
        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            string mac = System.Text.Encoding.Default.GetString(AesUtil.Encrypt(System.Text.Encoding.Default.GetBytes("E4:02:9B:55:8E:30"), ProtocolConstant.USB_DOG_PASSWORD));
            //string mac = AesUtil.AesEncrypt("E4:02:9B:55:8E:30", "E4:02:9B:55:8E:30");
            SetterService setterService = new SetterService();
            entity.Setter setter = new entity.Setter();
            setter.Set_Unique_Id = mac;
            setterService.InsertSetter(setter);
            
            //MessageBox.Show(mac);
        }
        //获得AES加密
        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            SetterService setterService = new SetterService();
            entity.Setter setter = setterService.getSetter();



            MessageBox.Show(setter.Set_Unique_Id);
        }
        /// <summary>
        /// 测试激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //string strMac = CommUtil.GetMacAddress();
            List<string> Macs = CommUtil.GetMacByWMI();
            foreach (string mac in Macs)
            {
                stringBuilder.Append(mac);
            }
            //Console.WriteLine(stringBuilder.ToString());
            entity.Setter setter = new entity.Setter();
            //mac地址先变为byte[]再aes加密
            byte[] byteMac = Encoding.GetEncoding("GBK").GetBytes(stringBuilder.ToString());
            byte[] AesMac = AesUtil.Encrypt(byteMac, ProtocolConstant.USB_DOG_PASSWORD);
            //存入数据库
            //setter.Set_Unique_Id = Encoding.GetEncoding("GBK").GetString(AesMac);
            setter.Set_Unique_Id = ProtocolUtil.BytesToString(AesMac);
            /*AES解密
             * byte[] a = ProtocolUtil.StringToBcd(setter.Set_Unique_Id);
            byte[] b = AesUtil.Decrypt(a, ProtocolConstant.USB_DOG_PASSWORD);
            Console.WriteLine(Encoding.GetEncoding("GBK").GetString(b));*/
           
            new SetterDAO().InsertOneMacAdress(setter);




        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            byte[] a = ProtocolUtil.StringToBcd(new SetterDAO().getSetter().Set_Unique_Id);
            byte[] b = AesUtil.Decrypt(a, ProtocolConstant.USB_DOG_PASSWORD);
            string mac = Encoding.GetEncoding("GBK").GetString(b);
            MessageBox.Show(mac);
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            UploadManagementDAO uploadManagementDAO = new UploadManagementDAO();
            string result = uploadManagementDAO.CheckExistAuth() == null ? "null" : "you";
            MessageBox.Show(result);
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            DateTime? a = DateTime.Now;
            MessageBox.Show(a.ToString());
        }
    }
}
