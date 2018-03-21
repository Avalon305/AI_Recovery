
using Microsoft.Win32;
using spms.entity;
using spms.service;
using spms.util;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Window
    {
        //保存用户照片的路径
        string userPhotoPath = null;

        //service层初始化
        UserService userService = new UserService();
        CustomDataService customDataService = new CustomDataService();

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

            groupList = customDataService.GetAllByType(CustomDataEnum.Group);
            diseaseList = customDataService.GetAllByType(CustomDataEnum.Disease);
            diagnosisList = customDataService.GetAllByType(CustomDataEnum.Diagiosis);
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
            diseaseList = customDataService.GetAllByType(CustomDataEnum.Disease);
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
            diagnosisList = customDataService.GetAllByType(CustomDataEnum.Diagiosis);
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
        /// <summary>
        /// 添加按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            //获取身份证与手机号之后马上查重
            if (userService.CheckExistByPhoneAndIDCard(IDCard,phone)) {
                //重复弹框提示
            }

            string IdCard = this.IDCard.Text;
            string name = t3.Text;
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

            if (IdCard == null || name == null || IDCard == "" || name == "")
            {
                System.Windows.MessageBox.Show("没有填写身份证或者名字（拼音）", "信息提示");
                return;
            }

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

            

            if (IdCard != null && name != null && IDCard != "" && name != "")
            {
                // 如果用户是自己选择现成的图片，将图片保存在安装目录下
                string sourcePic = userPhotoPath;
                string targetPic = CommUtil.GetUserPic(usernamePY + IDCard);
                targetPic += ".jpg";
                bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                System.IO.File.Copy(sourcePic, targetPic, isrewrite);
                
            }
            else
            {
                System.Windows.MessageBox.Show("没有填写身份证或者名字（拼音）", "信息提示");
                return;
            }
            

            userService.InsertUser(user);
            this.Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Photograph(object sender, RoutedEventArgs e)
        {

            Photograph photograph = new Photograph
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            photograph.getIdCard = IDCard.Text;
            photograph.getName = t3.Text;

            photograph.ShowDialog();
        }

        // 用户自主选择照片
        private void Select_Picture_Show(object sender, RoutedEventArgs e)
        {

            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Title = "请选择要插入的图片";
                ofd.Filter = "JPG图片|*.jpg|BMP图片|*.bmp|Gif图片|*.gif";
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // 获取图片路径
                    string picPath = ofd.FileName;

                    long picLen = 0;

                    FileInfo di = new FileInfo(picPath);
                    picLen = di.Length;
                    picLen /= 1024;
                    Console.WriteLine("~~~~~~~~ 图片的大小" + picLen + "~~~~~~~~");

                    if (picLen > 40)
                    {
                        System.Windows.MessageBox.Show("图片过大，请重新选择", "信息提示");
                        return;
                    }

                    BitmapImage image = new BitmapImage(new Uri(ofd.FileName, UriKind.Absolute));//打开图片
                    pic.Source = image;//将控件和图片绑定

                    userPhotoPath = ofd.FileName;
                }
                else
                {
                    System.Windows.MessageBox.Show("你没有选择图片", "信息提示");
                }
            }
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
                Error_Info_IDCard.Content = "请输入正确的身份证号码";
                bubble_IDCard.IsOpen = true;
            }
            else if (userService.GetByIdCard(IDCard.Text) != null)
            {
                Error_Info_IDCard.Content = "该身份证已注册";
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
            if (!inputlimited.InputLimited.IsHandset(phoneNum.Text) && !String.IsNullOrEmpty(phoneNum.Text))
            {
                Error_Info_Phone.Content = "请输入正确的手机号";
                bubble_phone.IsOpen = true;
            }
            else if (userService.GetByPhone(phoneNum.Text) != null)
            {
                Error_Info_Phone.Content = "该手机号已注册";
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
            mi.Invoke(bubble_name, null);

        }
        //验证用户是否存在
        private void IsName(object sender, RoutedEventArgs e)
        {
            User user = new User
            {
                User_Name = t2.Text
            };
            UserService userService = new UserService();
            userService.SelectByCondition(user);
            if(userService.SelectByCondition(user).Count != 0)
            {
                Error_Info_Name.Content = "该用户名已注册";
                bubble_name.IsOpen = true;
            }
        }
    }
}
