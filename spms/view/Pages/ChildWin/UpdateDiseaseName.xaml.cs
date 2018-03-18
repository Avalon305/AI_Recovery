
using Dapper;
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
    /// InputDiseaseName.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateDiseaseName : Window
    {
        public CustomData selectedDisease = new CustomData();
        /// <summary>
        /// 自定义三项service
        /// </summary>
        CustomDataService customDataService = new CustomDataService();

        public UpdateDiseaseName()
        {
            InitializeComponent();
        }
        //取消按钮，关闭此窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_SaveClick(object sender, RoutedEventArgs e)
        {
            string value = this.DiseaseName.Text;
            selectedDisease.CD_CustomName = value;
            using (var conn = DbUtil.getConn())
            {
                conn.Execute("update bdl_customdata set CD_CustomName=@CD_CustomName where Pk_CD_Id=@Pk_CD_Id", selectedDisease);
            }
            this.Close();
        }
    }
}
