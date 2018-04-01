using spms.util;
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

namespace spms
{
    /// <summary>
    /// PhotoCutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PhotoCutWindow : Window
    {
        private BitmapImage image;
        private string fileName;
        //照片的名字
        public string photoName { get; set; }
        //得到用户的名字
        public string getName { get; set; }
        public string id { get; set; }
        public string oldPhotoName { get; set; }
        public void SetImage(BitmapImage image)
        {
            this.image = image;
        }
        public PhotoCutWindow()
        {
            InitializeComponent();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
             
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            photoName = oldPhotoName;
            this.Close();
        }
    }
}
