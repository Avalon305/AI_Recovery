﻿
using Dapper;
using spms.dao.app;
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
        CustomDataDAO CustomDataDAO = new CustomDataDAO();

        public UpdateDiseaseName()
        {
            InitializeComponent();
        }
        //取消按钮，关闭此窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Text_LostFocus(object sender, RoutedEventArgs e)
        {
            //获取文本框的值
            string Name = DiseaseName.Text;
            CustomData CustomData = CustomDataDAO.GetListByTypeIDAndName(CustomDataEnum.Disease, Name);
            if (CustomData != null)
            {
                MessageBox.Show("疾病名称已存在");
            }
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
