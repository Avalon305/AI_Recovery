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
    /// PicView.xaml 的交互逻辑
    /// </summary>
    public partial class PicView : Window
    {
        public PicView()
        {
            InitializeComponent();
        }

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

            //PicImage.Source = PicBitmapImage;
            //测试用
            PicImage.Source = new BitmapImage(new Uri(@"\view\Images\excel\1234.gif", UriKind.Relative));
            //PicImage.Source = PicBitmapImage = new BitmapImage(new Uri("view//Images//excel//1234.gif", UriKind.Relative));
        }

        //public BitmapImage PicBitmapImage{ set; get; }

        ScaleTransform temp = new ScaleTransform();
        /// <summary>
        /// 放大图片
        /// </summary>
        private void BtnEnlarge_Click(object sender, RoutedEventArgs e)
        {
            temp.CenterX = PicShow.ActualWidth / 2;
            temp.CenterY = PicShow.ActualHeight / 2;
            if (temp.ScaleX < 6d)
            {
                temp.ScaleX += 0.5d;
            }
            if (temp.ScaleY < 6d)
            {
                temp.ScaleY += 0.5d;
            }

            numberedItemsStackPanel.Height = temp.ScaleY * 300;
            numberedItemsStackPanel.Width = temp.ScaleX * 800;

            PicShow.RenderTransform = temp;
        }

        /// <summary>
        /// 缩小图片
        /// </summary>
        private void BtnNarrow_Click(object sender, RoutedEventArgs e)
        {
            temp.CenterX = PicShow.ActualWidth / 2;
            temp.CenterY = PicShow.ActualHeight / 2;
            if (temp.ScaleX > 0)
            {
                temp.ScaleX -= 0.5d;
            }
            if (temp.ScaleY > 0)
            {
                temp.ScaleY -= 0.5d;
            }

            numberedItemsStackPanel.Height = temp.ScaleY * 300;
            numberedItemsStackPanel.Width = temp.ScaleX * 800;

            PicShow.RenderTransform = temp;
        }
    }
}
