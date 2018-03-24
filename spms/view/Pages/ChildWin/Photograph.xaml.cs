﻿using spms.util;
using System;
using System.Collections.Generic;
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
                
                //string fileName = System.IO.Path.GetDirectoryName(path + ".jpg");
                //MessageBox.Show("!!!!!!!!!!"+ fileName +"!!!!!!!!!!!!!");

                //Thread.Sleep(2000);

                File.WriteAllBytes(path + ".jpg", Pic);

                //System.IO.FileStream fs = new System.IO.FileStream(fileName,System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);

                //System.Windows.MessageBox.Show("图片保存完成", "信息提示");
            }
            else
            {
                System.Windows.MessageBox.Show("没有填写身份证或者名字（拼音）", "信息提示");
                return;
            }

            this.Close();
        }

        // 加载摄像头按钮
        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vce.VideoCaptureSource = (string)cb.SelectedItem;
        }
    }
}
