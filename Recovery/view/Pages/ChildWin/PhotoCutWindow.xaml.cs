using Recovery.util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp2;

namespace Recovery
{

    /// <summary>
    /// PhotoCutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PhotoCutWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
       
          
       
        private BitmapImage image;
        private string fileName;
        //照片的名字
        public string photoName { get; set; }
        //得到用户的名字
        public string getName { get; set; }
        public string id { get; set; }
        public string oldPhotoName { get; set; }

        public PhotoCutWindow(BitmapImage image)
        {
            InitializeComponent();
            
            this.Width = SystemParameters.WorkArea.Size.Width * 0.4;
            this.Height = this.Width / 1.1;
            this.image = image;
            ImageDealer.Width = image.Width;
            ImageDealer.Height = image.Height;
            confirm.Width = image.Width / 6;
            confirm.Height = image.Height / 19;
            confirm.FontSize = confirm.Height / 2;
            cancel.Width = image.Width / 6;
            cancel.Height = image.Height / 19;
            cancel.FontSize = cancel.Height / 2;
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
            
            bmcpy.Save(CommUtil.GetUserPic() + photoName, System.Drawing.Imaging.ImageFormat.Jpeg);

        }
        public string getPhotoName()
        {
            if (string.IsNullOrEmpty(oldPhotoName))
            {
                photoName = getName + id;
            }
            else
            {
                photoName = oldPhotoName.Substring(0, oldPhotoName.LastIndexOf(".")) + "z";
                // 如果该文件已存在，继续在尾部追加“z”，知道文件不存在，就确定这个名字为文件名
                while (File.Exists(CommUtil.GetUserPic() + photoName + ".jpg") || File.Exists(CommUtil.GetUserPic() + photoName + ".gif"))
                {
                    oldPhotoName = photoName + ".jpg";
                    photoName = oldPhotoName.Substring(0, oldPhotoName.LastIndexOf(".")) + "z";
                }
                
            }

            photoName += ".jpg";
            return photoName;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //去掉图标和最大化关闭按钮
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            //ImageDealer.Width = image.Width;
            //ImageDealer.Height = image.Height;
            photoName = getPhotoName();
            ImageDealer.BitSource = image;
        }
        /// <summary>
        /// 点击裁剪按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //创建文件夹
            CreateDir(CommUtil.GetUserPic());
            ImageDealer.CutImage();
            /*
            string newFileName = photoName.Replace(".jpg", ".gif");

            if (PicZipUtil.GetPicThumbnail(CommUtil.GetUserPic() + photoName, CommUtil.GetUserPic() + newFileName, 50))
            {
                
                photoName = newFileName;
            }*/
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            photoName = oldPhotoName;
            this.Close();

        }
    }
}
