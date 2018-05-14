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

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// SerialPortSelection.xaml 的交互逻辑
    /// </summary>
    public partial class SerialPortSelection : Window
    {
       
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        private string portName = "";

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        public SerialPortSelection()
        {
            InitializeComponent();
            this.Width = SystemParameters.WorkArea.Size.Width * 0.4;
            this.Height = this.Width / 2.1;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            //如果点击取消，则相当于没有选择串口
            SerialPortUtil.portName = "";
            this.Close();
        }

        private void Determine(object sender, RoutedEventArgs e)
        {
            //如果不为空，赋值
            if (portName != "")
            {
                SerialPortUtil.portName = portName;
                this.Close();
            }
            else
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("请选择串口", "Please select the serial port"));
            }
        }

        private void Port_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton cb = sender as RadioButton;
            portName = (string)cb.Content;
            //Console.WriteLine("选中"+ cb.Content);
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Determine(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }
    }
}
