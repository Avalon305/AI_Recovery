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
using spms.dao.app;
using spms.util;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// OnlineDeviceDetails.xaml 的交互逻辑
    /// </summary>
    public partial class OnlineDeviceDetails : Window
    {
        
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        //在线设备DAO
        OnlineDeviceDAO onlineDeviceDAO = new OnlineDeviceDAO();
        //更新UI定时器
        public System.Timers.Timer timerNotice = null;
        public OnlineDeviceDetails()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 页面载入时的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            //terminalstatus  
            var resultOnline = onlineDeviceDAO.ListAll();
            terminalstatus.ItemsSource = resultOnline;
            //
            if (timerNotice == null)
            {

                BindNotice();

                timerNotice = new System.Timers.Timer();
                timerNotice.Elapsed += new System.Timers.ElapsedEventHandler((o, eea) => { BindNotice(); });

                timerNotice.Interval = 10*1000;
                timerNotice.Start();
            }
        }

        private void BindNotice()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                //范给的多线程UI更新示例代码
                //App.Current.Dispatcher.Invoke(new Action(() =>
                //{
                //    MessageBox.Show(LanguageUtils.GetCurrentLanuageStrByKey("App.PortOccupy"));
                //    System.Environment.Exit(0);
                //}));
                try
                {
                    this.terminalstatus.Dispatcher.Invoke(
                           new Action(
                                delegate
                                {
                                    var resultOnline = onlineDeviceDAO.ListAll();
                                    terminalstatus.ItemsSource = resultOnline;
                                    Console.WriteLine(DateTime.Now.ToString());
                                }
                           )
                     );
                }
                catch(Exception ee)
                {
                    Console.WriteLine(ee);
                }
            });
        }

        //取消按钮，关闭此窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            timerNotice.Stop();
            this.Close();
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_OK(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }

        private void Button_OK(object sender, RoutedEventArgs e)
        {

        }

        

        private void viewbox_load(object sender, RoutedEventArgs e)
        {
            this.Width = viewbox.ActualWidth;
            this.Height = viewbox.ActualHeight;
            Left = (SystemParameters.WorkArea.Size.Width - this.ActualWidth) / 2;
            Top = (SystemParameters.WorkArea.Size.Height - this.ActualHeight) / 2;
        }
    }
}
