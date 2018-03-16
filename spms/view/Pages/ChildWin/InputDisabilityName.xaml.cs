using spms.dao.app;
using spms.entity;
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
    /// InputDisabilityName.xaml 的交互逻辑
    /// </summary>
    public partial class InputDisabilityName : Window
    {
        public InputDisabilityName()
        {
            InitializeComponent();
        }
        //取消按钮，关闭此窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DiagnosisDAO diagnosisDAO = new DiagnosisDAO();
            string name = this.Diagnosis.Text;
            Diagnosis diagnosis = new Diagnosis(name);
            diagnosisDAO.Insert(diagnosis);
            Console.WriteLine("点击");
            this.Close();
        }
    }
}
