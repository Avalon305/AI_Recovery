
using spms.entity;
using spms.service;
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

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Window
    {
        DataCodeService dataCodeService = new DataCodeService();

        //service层初始化
        UserService userService = new UserService();

        //小组的名称列表
        List<string> groupList;
        //疾病名称列表
        List<string> diseaseList ;
        //残障名称列表
        List<string> diagnosisList;
        //护理度列表
        List<string> careList = new List<string> { "没有申请", "自理", "要支援一", "要支援二", "要介护1", "要介护2", "要介护3", "要介护4", "要介护5" };
        public Register()
        {
            InitializeComponent();

            groupList = dataCodeService.GetDataStrByTypeId(constant.DataCodeTypeEnum.Group);
            diseaseList = dataCodeService.GetDataStrByTypeId(constant.DataCodeTypeEnum.Disease);
            diagnosisList = dataCodeService.GetDataStrByTypeId(constant.DataCodeTypeEnum.Diagiosis);
            //初始化下拉框值
            c2.ItemsSource = groupList;
            c5.ItemsSource = diseaseList;
            c6.ItemsSource = diagnosisList;
            //护理程度下拉框
            c3.ItemsSource = careList;
            c4.ItemsSource = careList;
        }

        private void c1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //添加疾病名称
        private void DiseaseNameAddition(object sender, RoutedEventArgs e)
        {
            InputDiseaseName inputDiseaseName = new InputDiseaseName
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            inputDiseaseName.ShowDialog();
            //flush 界面
            diseaseList = dataCodeService.GetDataStrByTypeId(constant.DataCodeTypeEnum.Disease);
            c5.ItemsSource = diseaseList;
        }
        //添加残障名称
        private void DisabilityNameAddition(object sender, RoutedEventArgs e)
        {
            InputDisabilityName inputDisabilityName = new InputDisabilityName
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            inputDisabilityName.ShowDialog();
            //flush 界面
            diagnosisList = dataCodeService.GetDataStrByTypeId(constant.DataCodeTypeEnum.Diagiosis);
            c6.ItemsSource = diagnosisList;
        }
        //输入非公开信息
        private void InputNonPublicInformationPassword(object sender, RoutedEventArgs e)
        {
            InputNonPublicInformationPassword inputNonPublicInformationPassword = new InputNonPublicInformationPassword
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            inputNonPublicInformationPassword.ShowDialog();
            //将非公开信息框显示
            this.Non_Public_Information.Visibility = System.Windows.Visibility.Visible;
            //调整该窗体宽度
            this.Width = 710;
        }
        //取消操作，关闭窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_OK(object sender, RoutedEventArgs e)
        {

            //获取用户身份证的内容
            //string userID = t1.Text;
            //获取用户姓名的内容
            string userName = t2.Text;
            //获取用户姓名拼音的内容
            string usernamePY = t3.Text;
            //获取用户性别的内容
            string usersex = c1.Text;
            //获取用户出生年月的内容
            string brithday = t4.Text;
            //获得身份证号
            string IDCard = this.IDCard.Text;
            //获得手机号
            string phone = this.phoneNum.Text;
            //获取小组名称的内容
            string groupName = c2.Text;
            //获取初期要介护度的内容
            string initial = c3.Text;
            //获取现在要介护度的内容
            string now = c4.Text;
            //获取疾病名称的内容
            string sicknessName = c5.Text;
            //获取残障名称的内容
            string disabilityName = c6.Text;
            //获取备忘的内容
            TextRange text = new TextRange(t6.Document.ContentStart, t6.Document.ContentEnd);
            string memo = text.Text;

            User user = new User();
            user.User_Birth = Convert.ToDateTime(brithday);
            user.User_GroupName = groupName;
            user.User_IDCard = IDCard;
            user.User_IllnessName = sicknessName;
            user.User_InitCare = initial;
            user.User_Memo = memo;
            user.User_Name = userName;
            user.User_Namepinyin = usernamePY;
            user.User_Nowcare = now;
            user.User_PhysicalDisabilities = disabilityName;
            user.User_Sex = (byte?)(usersex.Equals("男") ? 1 : 0);
            user.User_Phone = phone;
            ///补齐照片的部分



            userService.InsertUser(user);
            this.Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Photograph(object sender, RoutedEventArgs e)
        {

        }
    }
}
