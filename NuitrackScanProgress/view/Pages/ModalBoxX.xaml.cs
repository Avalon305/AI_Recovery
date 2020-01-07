using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace NuitrackScanProgress.view.Pages
{
    /// <summary>
    /// InitWeight.xaml 的交互逻辑
    /// </summary>
    public partial class ModalBoxX : Window
    {
        /// <summary>
        /// 禁止在外部实例化
        /// </summary>
        public ModalBoxX()
        {
            InitializeComponent();
        }

        public new string Title
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        public string InitWeight
        {
            get { return this.initWeight.Text; }
            set { this.initWeight.Text = value; }
        }
        /// <summary>
        /// 静态方法 模拟MESSAGEBOX.Show方法
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static bool? Show(string title, string weight)
        {
            var msgBox = new ModalBoxX();
            msgBox.Title = title;
            msgBox.InitWeight = weight;
            return msgBox.ShowDialog();
        }
        private void Yes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.DialogResult = true;
            // 更改配置文件
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("initWeight");
            config.AppSettings.Settings.Add("initWeight", this.initWeight.Text);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            this.Close();
        }


        private void No_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.DialogResult = false;
            this.Close();
        }
    }
}
