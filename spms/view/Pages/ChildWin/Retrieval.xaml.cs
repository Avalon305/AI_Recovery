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
    /// Retrieval.xaml 的交互逻辑
    /// </summary>

    public partial class Retrieval : Window
    {
        //小组的名称列表
        List<string> list = new List<string> { "aa", "bb", "abc", "csd", "sdlfks", "osdi", "awd" };
        //疾病名称列表
        List<string> list2 = new List<string> { "单侧麻痹", "心脏病", "脑梗赛", "脑出血", "高血压", "帕金森病", "糖尿病", "变形性膝关节炎", "没有" };
        //残障名称列表
        List<string> list3 = new List<string> { "上肢的脱离或截肢", "下肢的脱离或截肢", "上肢的外伤性运动障碍", "下肢的外伤性运动障碍", "脊髓损伤", "脑源性运动机能障碍", "左上下肢麻痹", "右上下肢麻痹", "没有" };
        public Retrieval()
        {
            InitializeComponent();
            //初始化下拉列表内容
            comboBox1.ItemsSource = list;
            c2.ItemsSource = list2;
            c3.ItemsSource = list3;
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            //获取用户ID的内容
            string userID = t1.Text;
            //获取用户姓名的内容
            string userName = t2.Text;
            //获取用户姓名拼音的内容
            string username = t3.Text;

            //获取用户性别的内容
            string usersex = c1.Text;
            //获取小组名称的内容
            string groupName = comboBox1.Text;
            //获取疾病名称的内容
            string sicknessName = c2.Text;
            //获取残障名称的内容
            string disabilityName = c3.Text;

            if (userID.Equals(""))
            {
                MessageBoxResult dr = MessageBox.Show("用户ID不能为空");
            }
            else
            {
                try
                {
                    //将用户ID转成整型，如果转换失败，说明有非数字
                    int i = Convert.ToInt32(t1.Text);

                    //在此处写添加的主要逻辑代码
                }
                catch
                {
                    MessageBoxResult dr = MessageBox.Show("用户ID必须为数字");
                }

            }
        }



        //关闭检索窗口
        private void GoBack(object sender, RoutedEventArgs e)

        {
            this.Close();
            //Window window = (Window)this.Parent;
            //window.Content = new DesignPage1();
        }
        //置空检索条件
        private void Emptying_Condition(object sender, RoutedEventArgs e)
        {
            entity.User user = new entity.User();
            Retrieval_Conditon.DataContext = user;

            t1.Text = "";
            t2.Text = "";
            t3.Text = "";
            comboBox1.Text = "";
            c1.Text = "";
            c3.Text = "";
            c2.Text = "";
        }

        //小组名称过滤事件
        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            List<string> mylist = new List<string>();
            mylist = list.FindAll(delegate (string s) { return s.Contains(comboBox1.Text.Trim()); });
            comboBox1.ItemsSource = mylist;
            comboBox1.IsDropDownOpen = true;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
