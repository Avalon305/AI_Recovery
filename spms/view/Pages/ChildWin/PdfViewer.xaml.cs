﻿using Spire.Pdf;
using Spire.Xls;
using spms.bean;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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

        public string SaveToPath { get; set; }

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
                    workbook.SaveToFile(@"e:\test.pdf", Spire.Xls.FileFormat.PDF);
                    //Worksheet sheet = workbook.Worksheets[0];
                    //sheet.SaveToPdf(@"e:\test.pdf");
                    //Console.WriteLine("转换执行完成了");
                    ////获取第一张工作表  
                    //Worksheet sheet = workbook.Worksheets[0];
                    ////设置打印区域（设置你想要转换的单元格范围）  
                    //sheet.PageSetup.PrintArea = "A1:K48";
                    ////将指定范围内的单元格保存为PDF              
                    //sheet.SaveToPdf(@"e:\test.pdf");
                    //文档输出
                    if (SaveToPath != "")
                    {
                        File.Copy(@"e:\test.pdf", SaveToPath, true);
                    }
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
            //直接打印Excel文件
            //Workbook workbook = new Workbook();
            //workbook.LoadFromFile("e:/test.xlsx");
            //PrintDialog dialog = new PrintDialog();
            //dialog.AllowPrintToFile = true;
            //dialog.AllowCurrentPage = true;
            //dialog.AllowSomePages = true;
            //dialog.AllowSelection = true;
            //dialog.UseEXDialog = true;
            //dialog.PrinterSettings.Duplex = Duplex.Simplex;
            //dialog.PrinterSettings.FromPage = 0;
            //dialog.PrinterSettings.ToPage = 8;
            //dialog.PrinterSettings.PrintRange = PrintRange.SomePages;
            //workbook.PrintDialog = dialog;
            //PrintDocument pd = workbook.PrintDocument;
            //if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{ pd.Print(); }

            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile("e:/test.pdf");
            PrintDialog dialogPrint = new PrintDialog();
            dialogPrint.AllowPrintToFile = true;
            dialogPrint.AllowSomePages = true;
            dialogPrint.PrinterSettings.MinimumPage = 1;
            dialogPrint.PrinterSettings.MaximumPage = doc.Pages.Count;
            dialogPrint.PrinterSettings.FromPage = 1;
            dialogPrint.PrinterSettings.ToPage = doc.Pages.Count;

            if (dialogPrint.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //设置打印的起始页码
                doc.PrintFromPage = dialogPrint.PrinterSettings.FromPage;

                //设置打印的终止页码
                doc.PrintToPage = dialogPrint.PrinterSettings.ToPage;

                //选择打印机
                doc.PrinterName = dialogPrint.PrinterSettings.PrinterName;

                PrintDocument printDoc = doc.PrintDocument;
                dialogPrint.Document = printDoc;
                printDoc.Print();
            }
        }
    }
}
