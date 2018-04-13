
using spms.dao.app;
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
using spms.util;
using static spms.entity.CustomData;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// InputDisabilityName.xaml 的交互逻辑
    /// </summary>
    public partial class InputDisabilityName : Window
    {

        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        /// <summary>
        /// 自定义三项service
        /// </summary>
        CustomDataService customDataService = new CustomDataService();
        CustomDataDAO CustomDataDAO = new CustomDataDAO();

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
            //获取文本框的值
            string Name = Diagnosis.Text;
            CustomData CustomData = CustomDataDAO.GetListByTypeIDAndName(CustomDataEnum.Diagiosis, Name);
            if (Name=="") {
                MessageBox.Show(LanguageUtils.ConvertLanguage("残障名称不能为空", "The handicap name can not be null"));
            }
            else if (CustomData != null)
            {
                MessageBox.Show(LanguageUtils.ConvertLanguage("残障名称已存在", "The name of the handicap has already existed"));
            }
            else
            {
                string value = this.Diagnosis.Text;
                customDataService.InsertCustomData(CustomDataEnum.Diagiosis, value);
                this.Close();
            }
           
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }
    }
}
