
using spms.entity;
using spms.service;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static spms.entity.CustomData;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// Retrieval.xaml 的交互逻辑
    /// </summary>

    public partial class Retrieval : Window
    {
        /// <summary>
        /// 查询的最终返回结果
        /// </summary>
        public List<User> QueryResult { get; set; }
        /// <summary>
        /// 初始化用户service和自定义三项service
        /// </summary>
        UserService userService = new UserService();
        CustomDataService customDataService = new CustomDataService();
        //小组的名称列表
        List<string> groupList;
        //疾病名称列表
        List<string> diseaseList;
        //残障名称列表
        List<string> diagnosisList;
        //护理度列表
        List<string> careList = new List<string> { "没有申请", "自理", "要支援一", "要支援二", "要介护1", "要介护2", "要介护3", "要介护4", "要介护5" };

        public Retrieval()
        {
            InitializeComponent();

            groupList = customDataService.GetAllByType(CustomDataEnum.Group);
            diseaseList = customDataService.GetAllByType(CustomDataEnum.Disease);
            diagnosisList = customDataService.GetAllByType(CustomDataEnum.Diagiosis);
            //初始化下拉列表内容
            comboBox1.ItemsSource = groupList;
            c4.ItemsSource = diseaseList;
            c3.ItemsSource = diagnosisList;
        }
        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            //获取用户ID的内容
            //string userID = t1.Text;
            //获取用户姓名的内容
            string userName = t2.Text;
            //获取用户姓名拼音的内容
            string usernamePY = t3.Text;

            //获取用户性别的内容
            string usersex = c1.Text;
            //手机号
            string phone = this.phone.Text;
            //身份证号
            string IDCard = this.IDCard.Text;


            //获取小组名称的内容
            string groupName = comboBox1.Text;
            //获取疾病名称的内容
            string sicknessName = c4.Text;
            //获取残障名称的内容
            string disabilityName = c3.Text;

            User user = new User();

            //将用户ID转成整型，如果转换失败，说明有非数字
            //int i = Convert.ToInt32(t1.Text);
            //user.Pk_User_Id = (!string.IsNullOrEmpty(userID)) ? Convert.ToInt32(userID) : 0;


            user.User_Name = userName;
            user.User_Namepinyin = usernamePY;

            user.User_Sex =  (byte?)(usersex.Equals("男") ? 1 : usersex.Equals("女") ? 0 : 2);
            user.User_Phone = phone;
            user.User_IDCard = IDCard;

            user.User_GroupName = groupName;
            user.User_IllnessName = sicknessName;
            user.User_PhysicalDisabilities = disabilityName;

            QueryResult = userService.SelectByCondition(user);
            //Console.WriteLine(JsonTools.Obj2JSONStrNew<User>(user));
            this.Close();
        }



        //关闭检索窗口
        private void GoBack(object sender, RoutedEventArgs e)

        {
            QueryResult = userService.SelectByCondition(null);
            this.Close();
            //Window window = (Window)this.Parent;
            //window.Content = new DesignPage1();
        }
        //置空检索条件
        private void Emptying_Condition(object sender, RoutedEventArgs e)
        {
            entity.User user = new entity.User();
            Retrieval_Conditon.DataContext = user;

            //t1.Text = "";
            t2.Text = "";
            t3.Text = "";
            comboBox1.Text = "";
            c1.Text = "";
            c3.Text = "";
            c4.Text = "";
        }

        //小组名称过滤事件
        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            List<string> mylist = new List<string>();
            mylist = groupList.FindAll(delegate (string s) { return s.Contains(comboBox1.Text.Trim()); });
            comboBox1.ItemsSource = mylist;
            comboBox1.IsDropDownOpen = true;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        //设置手机号输入框只能输入数字
        private void OnlyInputNumbers(object sender, TextCompositionEventArgs e)
        {
            inputlimited.InputLimited.OnlyInputNumbers(e);
        }
        //身份证号验证和查重
        private void IsIDCard(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            if (!inputlimited.InputLimited.IsIDcard(IDCard.Text) && !String.IsNullOrEmpty(IDCard.Text))
            {
                Error_Info_IDCard.Content = "请输入正确的身份证号";
                bubble_IDCard.IsOpen = true;
            }
           else
            {
                bubble_IDCard.IsOpen = false;
            }
        }
        //手机号验证和查重
        private void IsPhone(object sender, RoutedEventArgs e)
        {
            if(!inputlimited.InputLimited.IsHandset(phone.Text) && !String.IsNullOrEmpty(phone.Text)){
                Error_Info_Phone.Content = "请输入正确的手机号";
                bubble_phone.IsOpen = true;
            }
            else
            {
                bubble_phone.IsOpen = false;
            }
        }
        //解决气泡不随着窗体移动问题
        private void windowmove(object sender, EventArgs e)
        {

            var mi = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            mi.Invoke(bubble_phone, null);
            mi.Invoke(bubble_IDCard, null);

        }
    }
}
