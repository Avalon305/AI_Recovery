using Spire.Xls;
using spms.bean;
using System;
using System.Collections.Generic;
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

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// PdfViewer.xaml 的交互逻辑
    /// </summary>
    public partial class PdfViewer : Window
    {
        private bool _isLoaded = false;
        //public static PDFViewer pDFViewer;
        public PdfViewer()
        {
            InitializeComponent();
            //pDFViewer = this;
        }

        public void WPFPdfViewerWindow_Activated(object sender, System.EventArgs e)
        {
            //moonPdfPanel.OpenFile(@"e:\123.pdf");
            //_isLoaded = true;
            //moonPdfPanel.ZoomIn();
            //moonPdfPanel.OpenFile(@"e:\123.pdf");
            //_isLoaded = true;
            //moonPdfPanel.ZoomIn();
            //二、将Excel转PDF
            new Thread(new ThreadStart(ExcelToPdf)).Start();
            Open_File();

        }

        private void ExcelToPdf()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                using (Workbook workbook = new Workbook())
                {
                    workbook.LoadFromFile(@"e:\test.xlsx");
                    //workbook.SaveToFile(@"e:\123.pdf", FileFormat.PDF);
                    Worksheet sheet = workbook.Worksheets[0];
                    sheet.SaveToPdf(@"e:\test.pdf");
                    //Console.WriteLine("转换执行完成了");
                }
                PdfViewer.valueChange.Flag = true;

            }));
        }

        internal static ValueChange valueChange = new ValueChange();
        public void Open_File()
        {
            //moonPdfPanel.OpenFile(@"e:\123.pdf");
            //_isLoaded = true;
            //moonPdfPanel.ZoomIn();
            //var valueChange = new ValueChange();

            //备注是部分
            valueChange.OnStringChangeEvent += (oo, ee) =>
            {

                moonPdfPanel.OpenFile(@"e:\test.pdf");
                _isLoaded = true;
                moonPdfPanel.ZoomIn();
            };

            //valueChange.Flag = true;

        }


        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                moonPdfPanel.ZoomIn();
            }
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                moonPdfPanel.ZoomOut();
            }
        }

        private void NormalButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                moonPdfPanel.Zoom(1.0);
            }
        }

        private void FitToHeightButton_Click(object sender, RoutedEventArgs e)
        {
            moonPdfPanel.ZoomToHeight();
        }

        private void FacingButton_Click(object sender, RoutedEventArgs e)
        {
            moonPdfPanel.ViewType = MoonPdfLib.ViewType.Facing;
        }

        private void SinglePageButton_Click(object sender, RoutedEventArgs e)
        {
            moonPdfPanel.ViewType = MoonPdfLib.ViewType.SinglePage;
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
