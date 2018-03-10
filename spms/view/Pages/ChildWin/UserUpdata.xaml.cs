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
    /// UserCompile.xaml 的交互逻辑
    /// </summary>
    public partial class UserUpdata : Window
    {
        //小组的名称列表
        List<string> list = new List<string> { "aa", "bb", "abc", "csd", "sdlfks", "osdi", "awd" };
        //疾病名称列表
        List<string> list2 = new List<string> { "单侧麻痹", "心脏病", "脑梗赛", "脑出血", "高血压", "帕金森病", "糖尿病", "变形性膝关节炎", "没有" };
        //残障名称列表
        List<string> list3 = new List<string> { "上肢的脱离或截肢", "下肢的脱离或截肢", "上肢的外伤性运动障碍", "下肢的外伤性运动障碍", "脊髓损伤", "脑源性运动机能障碍", "左上下肢麻痹", "右上下肢麻痹", "没有" };
        //初期介护度列表
        List<string> list4 = new List<string> { "没有申请", "自理", "要支援一", "要支援二", "要介护1", "要介护2", "要介护3", "要介护4", "要介护5" };
        public UserUpdata()
        {
            InitializeComponent();
            //初始化下拉框值
            c2.ItemsSource = list;
            c3.ItemsSource = list4;
            c4.ItemsSource = list4;
            c5.ItemsSource = list2;
            c6.ItemsSource = list3;
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
            //获取用户ID的内容
            string userID = t1.Text;
            //获取用户姓名的内容
            string userName = t2.Text;
            //获取用户姓名拼音的内容
            string username = t3.Text;
            //获取用户性别的内容
            string usersex = c1.Text;
            //获取用户出生年月的内容
            string brithday = t4.Text;
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


            if (userID.Equals(""))
            {
                MessageBoxResult dr = MessageBox.Show("用户ID不能为空");
            }
            else
            {
                try
                {
                    //里面填写接口内容
                    int i = Convert.ToInt32(t1.Text);
                }
                catch
                {
                    MessageBoxResult dr = MessageBox.Show("用户ID必须为数字");
                }

            }
        }
        //摄影按钮
        private void Button_TakePhoto(object sender, RoutedEventArgs e)
        {

        }
        //参照按钮
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
