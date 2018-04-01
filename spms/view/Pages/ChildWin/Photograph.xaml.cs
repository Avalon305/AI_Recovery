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

            // combox 获得摄像头列表
            cb.ItemsSource = MultimediaUtil.VideoInputNames;
            if (MultimediaUtil.VideoInputNames.Length > 0)
            {
                cb.SelectedIndex = 0;//第0个摄像头为默认摄像头
            }
            else
            {
                System.Windows.MessageBox.Show("电脑没有安装任何可用摄像头");
            }

        }
        /// <summary>
        /// 箭筒截图是否成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageDealer_OnOnCutImaging(object sender, RoutedEventArgs e)
        {
            
            var ddd = (BitmapSource)e.OriginalSource;
            Bitmap bit = BitmapFromSource(ddd);

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
            bmcpy.Save(CommUtil.GetUserPic() + photoName, System.Drawing.Imaging.ImageFormat.Jpeg);

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
                (int)vce.ActualHeight, 96, 96, PixelFormats.Default);

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

            vce.Pause();
            ImageDealer.BitSource = bitmapImage;
        }

        // 保存图片按钮
        private void SavePic(object sender, RoutedEventArgs e)
        {
            //创建文件夹
            CreateDir(CommUtil.GetUserPic());
            ImageDealer.CutImage();
            
            string newFileName = photoName.Replace(".jpg", ".gif");
            
            if (PicZipUtil.GetPicThumbnail(CommUtil.GetUserPic() + photoName, CommUtil.GetUserPic() + newFileName, 50))
            {
                File.Delete(CommUtil.GetUserPic() + photoName);
                photoName = newFileName;
            }
            this.Close();
        }
        //根据文件夹全路径创建文件夹
        public static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <param name="sFile">原图片</param>    
        /// <param name="dFile">压缩后保存位置</param>    
        /// <param name="dHeight">高度</param>    
        /// <param name="dWidth"></param>    
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>    
        /// <returns></returns>    
        /// (源文件，目标文件，高度，宽度，压缩比例)
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放  
            System.Drawing.Size tem_size = new System.Drawing.Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * (int)tem_size.Height) / (int)tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = ((int)tem_size.Width * dHeight) / (int)tem_size.Height;
                }
            }
            else
            {
                sW = (int)tem_size.Width;
                sH = (int)tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(System.Drawing.Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new System.Drawing.Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            //以下代码为保存图片时，设置压缩质量    
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100    
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo gifICIinfo = null;
               
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    
                    if (arrayICI[x].FormatDescription.Equals("GIF"))
                    {
                        gifICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (gifICIinfo != null)
                {
                    //MessageBox.Show("保存1");
                    ob.Save(dFile, gifICIinfo, ep);//dFile是压缩后的新路径    
                }
                else
                {
                    //MessageBox.Show("保存2");
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
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
