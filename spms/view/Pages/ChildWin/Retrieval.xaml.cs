using spms.dao.app;
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

        /// <summary>
        /// 辅助类
        /// </summary>
        AssistDAO assistDAO = new AssistDAO();
        DiseaseDAO DiseaseDAO = new DiseaseDAO();
        DiagnosisDAO DiagnosisDAO = new DiagnosisDAO();
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

            groupList = assistDAO.GetGroupStr();
            diseaseList = DiseaseDAO.GetDiseaseStr();
            diagnosisList = DiagnosisDAO.GetDiagnosisStr();
            //初始化下拉列表内容
            comboBox1.ItemsSource = groupList;
            c2.ItemsSource = diseaseList;
            c3.ItemsSource = diagnosisList;
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
    }
}
