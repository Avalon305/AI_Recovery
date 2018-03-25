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
using WPFMediaKit.DirectShow.Controls;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// Photograph.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class Photograph : Window

    {
        //得到用户的身份证
        public string getIdCard { get; set; }
        //得到用户的名字
        public string getName { get; set; }
        // 照片保存
        byte[] Pic = null;
        //GWL_STYLE表示获得窗口风格
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]

        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        //SetWindowLong：更改指定窗口的属性。
        
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
       
        //取消按钮，关闭此窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
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

        //拍照按钮
        private void btn_photo(object sender, RoutedEventArgs e)
        {
            //vce是前台wpfmedia控件的name
            //为避免抓不全的情况，需要在Render之前调用Measure、Arrange
            //为避免VideoCaptureElement显示不全，需要把
            //VideoCaptureElement的Stretch="Fill"
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)vce.ActualWidth,
                (int)vce.ActualHeight, 96, 96, PixelFormats.Default);

            vce.Measure(vce.RenderSize);
            vce.Arrange(new Rect(vce.RenderSize));

            bmp.Render(vce);
            //展示照片
            pic.Source = bmp;
            
            //这里需要创建一个流以便存储摄像头拍摄到的图片。
            //当然，可以使文件流，也可以使内存流。
            BitmapEncoder encoder = new JpegBitmapEncoder();
            // 获取图像的帧
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            
            //System.Console.WriteLine(path);

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                byte[] captureData = ms.ToArray();
                Pic = ms.ToArray();
                ms.Close();
                //File.WriteAllBytes("/1" + Guid.NewGuid().ToString().Substring(0, 5) + ".jpg", captureData);
            }

        }

        // 保存图片按钮
        private void SavePic(object sender, RoutedEventArgs e)
        {
            if (getIdCard != null && getName != null && getIdCard != "" && getName != "")
            {

                String path = CommUtil.GetUserPic(getName + getIdCard);
                String dirPath = CommUtil.GetUserPic();

                Console.WriteLine(dirPath);

                if (Directory.Exists(dirPath))//判断是否存在
                {
                    //Response.Write("已存在");
                }
                else
                {
                    //Response.Write("不存在，正在创建");
                    Directory.CreateDirectory(dirPath);//创建新路径
                }
                File.WriteAllBytes(path + "temp.gif", Pic);
                // 压缩图片
                GetPicThumbnail(path + "temp.gif", path + ".gif", 300, 300, 10);
                File.Delete(path + "temp.gif");
            }
            else
            {
                System.Windows.MessageBox.Show("没有填写身份证或者名字（拼音）", "信息提示");
                return;
            }

            this.Close();
        }

        /// 无损压缩图片    
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
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    //MessageBox.Show("保存1");
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径    
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

        // 压缩图片   （原路径，压缩后的路径，压缩比例）
        /*public static bool GetPicThumbnail(string sFile, string outPath, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;

            //以下代码为保存图片时，设置压缩质量  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100  
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    iSource.Save(outPath, jpegICIinfo, ep);//outFile是压缩后的新路径  
                }
                else
                {
                    iSource.Save(outPath, tFormat);
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
                iSource.Dispose();
            }
        }
        */

        // 加载摄像头按钮
        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vce.VideoCaptureSource = (string)cb.SelectedItem;
        }
    }
}
