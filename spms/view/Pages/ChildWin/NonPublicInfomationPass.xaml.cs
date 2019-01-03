using spms.dao;
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
    /// NonPublicInfomationPass.xaml 的交互逻辑
    /// </summary>
    public partial class NonPublicInfomationPass : Window
    {
        AuthDAO authDAO = new AuthDAO();
        /// <summary>
        /// 密码结果
        /// </summary>
        public string result { get; set; }
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
        //取消操作，关闭窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            result = "cancel";
            this.Close();
        }
        public NonPublicInfomationPass()
        {
            InitializeComponent();
            this.Width = SystemParameters.WorkArea.Size.Width * 0.277;
            this.Height = this.Width / 2.2;
        }
        /// <summary>
        /// 输入密码点击确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            result = "failed";
            var queryResult =  authDAO.GetByAuthLevel(Auther.AUTH_LEVEL_MANAGER);
            string inputPass = password.Password;
            if (!string.IsNullOrEmpty(inputPass)&& !string.IsNullOrEmpty(queryResult.Auth_UserPass)) {
                if (inputPass == queryResult.Auth_UserPass) {
                    result = "success";                                     
                }
            }
           
            this.Close();
        }

        //回车按钮:快捷键
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
