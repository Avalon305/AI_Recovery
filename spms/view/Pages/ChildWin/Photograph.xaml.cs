using spms.entity;
using spms.service;
using spms.util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp2;
using WPFMediaKit.DirectShow.Controls;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// Photograph.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class Photograph : Window
    {
        //照片的名字
        public string photoName { get; set; }
        //得到用户的名字
        public string getName { get; set; }
        public string id { get; set; }
        public string oldPhotoName { get; set; }
        // 照片保存
        byte[] Pic = null;
        //GWL_STYLE表示获得窗口风格
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //取消按钮，关闭此窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            photoName = oldPhotoName;
            this.Close();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //获得窗口句柄
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        // 获取摄像头
        public Photograph()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;

            // combox 获得摄像头列表
            cb.ItemsSource = MultimediaUtil.VideoInputNames;
            if (MultimediaUtil.VideoInputNames.Length > 0)
            {
                cb.SelectedIndex = 0;//第0个摄像头为默认摄像头
            }
            else
            {
                System.Windows.MessageBox.Show(LanguageUtils.ConvertLanguage("电脑没有安装任何可用摄像头", "Computer does not have any available camera"));
            }

        }

        public static Bitmap BitmapFromSource(BitmapSource source)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(source));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                // return bitmap; <-- leads to problems, stream is closed/closing ...
                return new Bitmap(bitmap);
            }
        }

        //拍照按钮
        private void btn_photo(object sender, RoutedEventArgs e)
        {
            save.IsEnabled = true;
            //vce是前台wpfmedia控件的name
            //为避免抓不全的情况，需要在Render之前调用Measure、Arrange
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)vce.ActualWidth,
                (int)vce.ActualHeight,
                96, 96, PixelFormats.Default);

            vce.Measure(vce.RenderSize);
            vce.Arrange(new Rect(vce.RenderSize));

            bmp.Render(vce);

            var renderTargetBitmap = bmp;
            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new JpegBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }

            //显示拍摄结果
            picResult.Source = bitmapImage;

        }
        //根据文件夹全路径创建文件夹
        public static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        // 保存图片按钮
        private void SavePic(object sender, RoutedEventArgs e)
        {
            vce.Stop();

            Bitmap bit = BitmapFromSource((BitmapSource)picResult.Source);

            //指定照片尺寸这么大
            var bmcpy = new Bitmap(183, 256);
            Graphics gh = Graphics.FromImage(bmcpy);
            gh.DrawImage(bit, new System.Drawing.Rectangle(0, 0, 183, 256));
            photoName = getName + id;
            if (oldPhotoName == getName + id + ".jpg" || oldPhotoName == getName + id + ".gif")
            {
                photoName += "_1";
            }

            photoName += ".jpg";


            var bytes = Bitmap2Byte(bmcpy);

            CreateDir(CommUtil.GetUserPic());
            //大于10K再压缩
            if (bytes.Length > 1024 * 10)
            {
                PicZipUtil.GetPicThumbnail(bmcpy, CommUtil.GetUserPic() + photoName, 80);
            }
            else
            {
               bmcpy.Save(CommUtil.GetUserPic() +  photoName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }




            this.Close();
        }
        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }

        // 加载摄像头按钮
        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vce.VideoCaptureSource = (string)cb.SelectedItem;
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SavePic(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }
    }
}

