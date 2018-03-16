using spms.constant;
using spms.dao;
using spms.entity;
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

namespace spms
{
    /// <summary>
    /// FmyTest.xaml 的交互逻辑
    /// </summary>
    public partial class FmyTest : Window
    {
        public FmyTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var f = treeView.SelectedItem as DataCode;
            MessageBox.Show(f.Code_Type_Id);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var list = DataCodeCache.GetInstance().GetDateCodeList(DataCodeTypeEnum.DList);
            var topNode = new TreeViewItem() { Header = "数据编码" };
           
            foreach (var d in list)
            {
                topNode.Items.Add(d.Code_D_Value);
            }
            //treeView.Items.Add(topNode);
            treeView.ItemsSource = list;
        }

        private void treeView_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
