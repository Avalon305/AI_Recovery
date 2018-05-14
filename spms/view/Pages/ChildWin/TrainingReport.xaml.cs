using NLog;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using Spire.Xls;
using spms.bean;
using spms.entity;
using spms.service;
using spms.util;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static spms.bean.TrainExcelVO;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// MakeReport.xaml 的交互逻辑
    /// </summary>
    public partial class TrainingReport : Window
    {
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Height = SystemParameters.WorkArea.Size.Height;
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }


        Thread initializationExcelToPdfThread;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 作用：经过初步测试，第一次Excel转pdf相对较慢，所以在进入程序的时候，执行一次Excel转PDF
        /// </summary>
        public void InitializationExcelToPdf()
        {
            try
            {
                using (Workbook workbook = new Workbook())
                {
                    workbook.LoadFromFile(CommUtil.GetDocPath("test1.xlsx"));
                    workbook.SaveToFile(CommUtil.GetDocPath("test1.pdf"), Spire.Xls.FileFormat.PDF);
                }
            }
            catch (Exception e)
            {
                initializationExcelToPdfThread.Abort();
            }
           
        }

        public TrainingReport()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;
            //启动初始化Excel转PDF的线程
            initializationExcelToPdfThread = new Thread(new ThreadStart(InitializationExcelToPdf));
            initializationExcelToPdfThread.Start();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            RelativeSource rs = new RelativeSource(RelativeSourceMode.FindAncestor);
            rs.AncestorType = typeof(ListBoxItem);
            Binding binding = new Binding("Tag") { RelativeSource = rs };
            btn.SetBinding(Button.ContentProperty, binding);
        }

        //取消操作，关闭窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //当前需要打印的用户
        public User Current_User { set; get; }
        //用于存放选中的时间
        public List<DateTime?> selectedDate { set; get; }
        private ExcelService excelService = new ExcelService();

        private void Button_Click_Print(object sender, RoutedEventArgs e)
        {
            try
            {
                if (is_comprehensiv.IsChecked == true)//训练报告
                {
                    GenerateTrainReport();
                }
                else if (is_detail.IsChecked == true)//详细报告
                {
                    GenerateDetailReport();
                }
                else if (is_nurse.IsChecked == true)//看护记录报告
                {
                    GenerateNurseReport();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("生成Excel文件异常");
                logger.Error("生成Excel文件异常");
                return;
            }

            try
            {
                //直接打印Excel文件
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(CommUtil.GetDocPath("test.xlsx"));
                System.Windows.Forms.PrintDialog dialog = new System.Windows.Forms.PrintDialog();
                dialog.AllowPrintToFile = true;
                dialog.AllowCurrentPage = true;
                dialog.AllowSomePages = true;
                dialog.AllowSelection = true;
                dialog.UseEXDialog = true;
                dialog.PrinterSettings.Duplex = Duplex.Simplex;
                dialog.PrinterSettings.FromPage = 0;
                dialog.PrinterSettings.ToPage = 8;
                dialog.PrinterSettings.PrintRange = PrintRange.SomePages;
                workbook.PrintDialog = dialog;
                PrintDocument pd = workbook.PrintDocument;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                { pd.Print(); }

                if (DocumentInput_Check.IsChecked == true)
                {
                    workbook.LoadFromFile(CommUtil.GetDocPath("test.xlsx"));
                    workbook.SaveToFile(@text_output_document.Text, FileFormat.PDF);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("训练报告打印异常");
                logger.Error("训练报告打印异常");
            }
            
        }


        /// <summary>
        /// 生成训练报告
        /// </summary>
        private void GenerateTrainReport()
        {
            List<TrainingAndSymptomBean> list = new List<TrainingAndSymptomBean>();
            for (int i = 0; i < datalist.Items.Count; i++)
            {
                //判断选中哪些时间
                if (selectedDate.Contains((datalist.Items[i] as TrainingAndSymptomBean).Gmt_Create))
                {
                    //Console.WriteLine("打印的内容" + datalist.Items[i].ToString());
                    list.Add((datalist.Items[i] as TrainingAndSymptomBean));
                }
            }

            FileInfo newFile = ExcelUtil.GetExcelFile();
            int count = 10;//包含的数据条数
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                int pageSize = list.Count % count == 0 ? list.Count / count : (list.Count / count) + 1;

                if (pageSize == 0)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(LanguageUtils.ConvertLanguage("训练报告", "Training report"));

                    int tableRow = 12;
                    int length = list.Count;
                    //设置所有的行高
                    for (int i = 1; i <= tableRow; i++)
                    {
                        worksheet.Row(i).Height = 20;
                    }

                    int userRow = 4;
                    ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, LanguageUtils.ConvertLanguage("训练报告", "Training report"), Current_User);
                }

                for (int j=0; j<pageSize; j++)
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(LanguageUtils.ConvertLanguage("训练报告" + j, "Training report" + j));

                    int tableRow = 12;
                    int length = list.Count;
                    //设置所有的行高
                    for (int i = 1; i <= tableRow; i++)
                    {
                        worksheet.Row(i).Height = 20;
                    }

                    //设置数据的高度
                    for (int i = tableRow + 2; i <= tableRow + 11; i++)
                    {
                        worksheet.Row(i).Height = 25;
                    }

                    int userRow = 4;
                    ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, LanguageUtils.ConvertLanguage("训练报告", "Training report"), Current_User);

                    /*
                        2.设置实施状况的表格
                     */
                    worksheet.Cells[userRow + 7, 1, userRow + 7, 2].Merge = true;
                    worksheet.Cells[userRow + 7, 1].Value = LanguageUtils.ConvertLanguage("实施状况", "Status");
                    using (ExcelRange range = worksheet.Cells[userRow + 7, 1, userRow + 7, 2])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.Font.Bold = true;
                        range.Style.Font.Name = "等线";
                        range.Style.Font.Size = 11;
                        range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 0, 139));
                        //设置边框
                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                    //设置表头
                    worksheet.Cells[tableRow, 1, tableRow + 1, 2].Merge = true;
                    worksheet.Cells[tableRow, 3, tableRow, 4].Merge = true;
                    worksheet.Cells[tableRow, 1, tableRow + 1, 2].Merge = true;
                    worksheet.Cells[tableRow, 5, tableRow + 1, 5].Merge = true;
                    worksheet.Cells[tableRow, 6, tableRow + 1, 6].Merge = true;
                    worksheet.Cells[tableRow, 7, tableRow + 1, 7].Merge = true;
                    worksheet.Cells[tableRow, 8, tableRow + 1, 8].Merge = true;
                    worksheet.Cells[tableRow, 9, tableRow + 1, 11].Merge = true;
                    worksheet.Cells[tableRow, 1].Value = LanguageUtils.ConvertLanguage("实施日期", "Date");
                    worksheet.Cells[tableRow, 3].Value = LanguageUtils.ConvertLanguage("血压", "Blood pressure");
                    worksheet.Cells[tableRow, 5].Value = LanguageUtils.ConvertLanguage("水分摄取量", "Moisture intake");
                    worksheet.Cells[tableRow, 6].Value = LanguageUtils.ConvertLanguage("平均指数", "Average index");
                    worksheet.Cells[tableRow, 7].Value = LanguageUtils.ConvertLanguage("总运动时间", "Total exercise time");
                    worksheet.Cells[tableRow, 8].Value = LanguageUtils.ConvertLanguage("总消耗热量", "Total calories consumed");
                    worksheet.Cells[tableRow, 9].Value = LanguageUtils.ConvertLanguage("看护记录", "Care record");
                    worksheet.Cells[tableRow + 1, 3].Value = LanguageUtils.ConvertLanguage("运动前", "Before exercise");
                    worksheet.Cells[tableRow + 1, 4].Value = LanguageUtils.ConvertLanguage("运动后", "After exercise");

                    using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow + 1, 11])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //range.Style.Font.Bold = true;
                        range.Style.Font.Name = "等线";
                        range.Style.Font.Size = 11;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 240, 244));
                    }


                    for (int i = 0,k=j* count; k < (j + 1) * count && k < list.Count; i++,k++)
                    {
                        //Console.WriteLine(k);
                        //表头行+两个表头
                        int row = tableRow + 2 + i;
                        worksheet.Cells[row, 1, row, 2].Merge = true;
                        worksheet.Cells[row, 9, row, 11].Merge = true;
                        worksheet.Cells[row, 1].Value = ((DateTime)list[k].Gmt_Create).ToString();
                        worksheet.Cells[row, 3].Value = list[k].SI_Pre_HighPressure + "/" + list[k].SI_Pre_LowPressure;
                        worksheet.Cells[row, 4].Value = list[k].SI_Suf_HighPressure + "/" + list[k].SI_Suf_LowPressure;
                        worksheet.Cells[row, 5].Value = list[k].SI_WaterInput;
                        worksheet.Cells[row, 6].Value = list[k].PR_Index;
                        //worksheet.Cells[row, 7].Value = list[k].PR_Time2 - list[i].PR_Time1;
                        worksheet.Cells[row, 7].Value = list[k].PR_Time1;
                        worksheet.Cells[row, 8].Value = list[k].PR_Cal;
                        worksheet.Cells[row, 9].Value = list[k].SI_CareInfo;
                    }

                    int borderRows = (j + 1) * count < list.Count ? count : list.Count - j * count;
                    using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow + 1 + borderRows, 11])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.Font.Bold = true;
                        range.Style.Font.Name = "等线";
                        range.Style.Font.Size = 10;
                        //设置边框
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }

                    //图表
                    //定义数据源
                    ExcelChart chart = worksheet.Drawings.AddChart("chart", eChartType.LineMarkersStacked);
                    //Y轴数据源，X轴数据源
                    var cs2 = chart.PlotArea.ChartTypes.Add(eChartType.Line);
                    var s = cs2.Series.Add(worksheet.Cells[tableRow + 2, 6, tableRow + 1 + length, 6], worksheet.Cells[tableRow + 2, 1, tableRow + 1 + length, 2]);
                    s.Border.Fill.Color = System.Drawing.Color.Red;
                    s.HeaderAddress = worksheet.Cells[tableRow, 6];
                    var cs3 = chart.PlotArea.ChartTypes.Add(eChartType.Line);
                    s = cs3.Series.Add(worksheet.Cells[tableRow + 2, 7, tableRow + 1 + length, 7], worksheet.Cells[tableRow + 2, 1, tableRow + 1 + length, 2]);
                    s.HeaderAddress = worksheet.Cells[tableRow, 7];
                    s.Border.Fill.Color = System.Drawing.Color.Green;
                    cs3.UseSecondaryAxis = true;
                    var cs4 = chart.PlotArea.ChartTypes.Add(eChartType.Line);
                    s = cs4.Series.Add(worksheet.Cells[tableRow + 2, 8, tableRow + 1 + length, 8], worksheet.Cells[tableRow + 2, 1, tableRow + 1 + length, 2]);
                    s.HeaderAddress = worksheet.Cells[tableRow, 8];
                    s.Border.Fill.Color = System.Drawing.Color.Blue;
                    cs4.UseSecondaryAxis = true;

                    //图表的相关设置
                    chart.SetPosition(24, 0, 0, 0);
                    chart.SetSize(726, 300);
                    chart.Title.Text = LanguageUtils.ConvertLanguage("实施报告", "Implementation report");
                    chart.Title.Font.Color = System.Drawing.Color.FromArgb(89, 89, 89);
                    chart.Title.Font.Size = 15;
                    chart.Title.Font.Bold = true;
                    chart.Style = eChartStyle.Style15;

                    //备注
                    int remarkRow = 41;
                    ExcelUtil.GenerateRemark(ref worksheet, remarkRow, ExcelUtil.GetObjContent(Current_User.User_Memo));
                }

                //保存
                package.Save();
            }
        }

        /// <summary>
        /// 生成详细训练报告
        /// </summary>
        private void GenerateDetailReport()
        {
            List<DevicePrescriptionExcel> list = new List<DevicePrescriptionExcel>();
            for (int i = 0; i < datalist.Items.Count; i++)
            {
                //判断选中哪些时间
                if (selectedDate.Contains((datalist.Items[i] as DevicePrescriptionExcel).Gmt_Create))
                {
                    //Console.WriteLine("打印的内容" + datalist.Items[i].ToString());
                    list.Add((datalist.Items[i] as DevicePrescriptionExcel));
                }
            }
            FileInfo newFile = ExcelUtil.GetExcelFile();
            int count = 22;//包含的数据条数
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                int pageSize = list.Count % count == 0 ? list.Count / count : (list.Count / count) + 1;

                if (pageSize == 0)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(LanguageUtils.ConvertLanguage("详细训练报告", "Detailed report"));

                    int tableRow = 11;
                    int length = list.Count;
                    //设置所有的行高
                    for (int i = 1; i <= tableRow; i++)
                    {
                        worksheet.Row(i).Height = 20;
                    }
                    int userRow = 4;
                    ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, LanguageUtils.ConvertLanguage("详细训练报告", "Detailed report"), Current_User);
                }

                for (int j = 0; j < pageSize; j++)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(LanguageUtils.ConvertLanguage("详细训练报告"+j, "Detailed report" + j));

                    int tableRow = 11;
                    int length = list.Count;
                    //设置所有的行高
                    for (int i = 1; i <= tableRow; i++)
                    {
                        worksheet.Row(i).Height = 20;
                    }
                    for (int i = tableRow; i <= tableRow + length; i++)
                    {
                        worksheet.Row(i).Height = 25;
                    }
                    int userRow = 4;
                    ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, LanguageUtils.ConvertLanguage("详细训练报告", "Detailed report"), Current_User);

                    worksheet.Cells[tableRow, 1, tableRow, 3].Merge = true;//合并单元格
                    worksheet.Cells[tableRow, 10, tableRow, 11].Merge = true;//合并单元格
                    worksheet.Cells[tableRow, 1].Value = LanguageUtils.ConvertLanguage("器械名称", "Device name");
                    worksheet.Cells[tableRow, 4].Value = LanguageUtils.ConvertLanguage("实施日期", "Date");
                    worksheet.Cells[tableRow, 5].Value = LanguageUtils.ConvertLanguage("移乘方法", "Transfer method");
                    worksheet.Cells[tableRow, 6].Value = LanguageUtils.ConvertLanguage("砝码", "Weights");
                    worksheet.Cells[tableRow, 7].Value = LanguageUtils.ConvertLanguage("组数", "Groups");
                    worksheet.Cells[tableRow, 8].Value = LanguageUtils.ConvertLanguage("个数", "Number");
                    worksheet.Cells[tableRow, 9].Value = "Borg";
                    worksheet.Cells[tableRow, 10].Value = LanguageUtils.ConvertLanguage("动作时机、姿势等", "Movement timing,posture..");

                    using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow, 11])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //range.Style.Font.Bold = true;
                        range.Style.Font.Name = "等线";
                        range.Style.Font.Size = 11;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 240, 244));
                    }

                    length = list.Count;
                    int row = 0;
                    for (int i = 0,k=j* count; k< (j + 1) * count && k < length; i++,k++)
                    {
                        //表头行+两个表头
                        row = tableRow + 1 + i;
                        worksheet.Cells[row, 1, row, 3].Merge = true;
                        worksheet.Cells[row, 10, row, 11].Merge = true;
                        worksheet.Cells[row, 1].Value = list[k].DS_name;
                        worksheet.Cells[row, 4].Value = ((DateTime)list[k].Gmt_Create).ToString();//string.Format("{0:d}", list[i].Gmt_Create);
                        //worksheet.Cells[row, 5].Value = list[k].dp_moveway;
                        if (list[k].dp_moveway == 0)
                        {
                            worksheet.Cells[row, 5].Value = LanguageUtils.ConvertLanguage("自理", "Self-care");
                        }
                        else if (list[k].dp_moveway == 1)
                        {
                            worksheet.Cells[row, 5].Value = LanguageUtils.ConvertLanguage("照看", "Look after");
                        }
                        else if (list[k].dp_moveway == 2)
                        {
                            worksheet.Cells[row, 5].Value = LanguageUtils.ConvertLanguage("完全失能", "Completely disabled");
                        }
                        worksheet.Cells[row, 6].Value = list[k].dp_weight;
                        worksheet.Cells[row, 7].Value = list[k].dp_groupcount;
                        worksheet.Cells[row, 8].Value = list[k].dp_groupnum;
                        worksheet.Cells[row, 9].Value = list[k].dp_relaxtime;
                        Console.WriteLine(list[k].PR_Evaluate);
                        if (list[k].PR_Evaluate == 0)
                        {
                            worksheet.Cells[row, 10].Value = LanguageUtils.ConvertLanguage("没问题", "No problem");
                        }
                        else if (list[k].PR_Evaluate == 1)
                        {
                            worksheet.Cells[row, 10].Value = LanguageUtils.ConvertLanguage("有些许问题", "Some problems");
                        }
                        else if (list[k].PR_Evaluate == 2)
                        {
                            worksheet.Cells[row, 10].Value = LanguageUtils.ConvertLanguage("有问题", "Has a problem");
                        }
                        //worksheet.Cells[row, 10].Value = list[k].PR_Evaluate;
                    }

                    int borderRows = (j + 1) * count < list.Count ? count : list.Count - j * count;
                    using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow + borderRows, 11])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.Font.Bold = true;
                        range.Style.Font.Name = "等线";
                        range.Style.Font.Size = 10;
                        //设置边框
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                }

                package.Save();
            }
        }

        /// <summary>
        /// 生成看护记录报告
        /// </summary>
        private void GenerateNurseReport()
        {
            List<TrainingAndSymptomBean> list = new List<TrainingAndSymptomBean>();
            for (int i = 0; i < datalist.Items.Count; i++)
            {
                //判断选中哪些时间
                if (selectedDate.Contains((datalist.Items[i] as TrainingAndSymptomBean).Gmt_Create))
                {
                    //Console.WriteLine("打印的内容" + datalist.Items[i].ToString());
                    list.Add((datalist.Items[i] as TrainingAndSymptomBean));
                }
            }

            FileInfo newFile = ExcelUtil.GetExcelFile();
            //FileInfo newFile = new FileInfo(CommUtil.GetDocPath("test.xlsx"));
            //if (newFile.Exists)
            //{

            //    newFile.Delete();
            //    newFile = new FileInfo(CommUtil.GetDocPath("test.xlsx"));
            //}
            int count = 9;//包含的数据条数
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                int pageSize = list.Count % count == 0 ? list.Count / count : (list.Count / count) + 1;

                if (pageSize == 0)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(LanguageUtils.ConvertLanguage("看护记录报告", "Care record report"));

                    int tableRow = 12;
                    int length = list.Count;
                    //设置所有的行高
                    for (int i = 1; i <= tableRow + length * 2 + 2; i++)
                    {
                        worksheet.Row(i).Height = 20;
                    }

                    int userRow = 4;
                    ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, LanguageUtils.ConvertLanguage("看护记录报告", "Care record report"), Current_User);
                }

                for (int j = 0; j < pageSize; j++)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(LanguageUtils.ConvertLanguage("看护记录报告"+j, "Care record report"+j));

                    int tableRow = 12;
                    int length = list.Count;
                    //设置所有的行高
                    for (int i = 1; i <= tableRow + length * 2 + 4; i++)
                    {
                        worksheet.Row(i).Height = 20;
                    }

                    int userRow = 4;
                    ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, LanguageUtils.ConvertLanguage("看护记录报告", "Care record report"), Current_User);


                    /*
                       2.设置实施状况的表格
                    */
                    worksheet.Cells[userRow + 7, 1, userRow + 7, 2].Merge = true;
                    worksheet.Cells[userRow + 7, 1].Value = LanguageUtils.ConvertLanguage("实施状况", "Status");
                    using (ExcelRange range = worksheet.Cells[userRow + 7, 1, userRow + 7, 2])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.Font.Bold = true;
                        range.Style.Font.Name = "等线";
                        range.Style.Font.Size = 11;
                        range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 0, 139));
                        //设置边框
                        worksheet.Cells[userRow + 7, 1, userRow + 7, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                    //设置表头
                    worksheet.Cells[tableRow, 1, tableRow + 1, 2].Merge = true;//试试日期
                    worksheet.Cells[tableRow, 3, tableRow, 4].Merge = true;//血压
                    worksheet.Cells[tableRow, 1, tableRow + 1, 2].Merge = true;
                    worksheet.Cells[tableRow, 5, tableRow + 1, 6].Merge = true;//水分摄取量
                    worksheet.Cells[tableRow, 7, tableRow + 1, 7].Merge = true;//平均指数
                    worksheet.Cells[tableRow, 8, tableRow + 1, 9].Merge = true;//总运动时间
                    worksheet.Cells[tableRow, 10, tableRow + 1, 11].Merge = true;//总消耗热量
                    worksheet.Cells[tableRow, 1].Value = LanguageUtils.ConvertLanguage("实施日期", "Date");
                    worksheet.Cells[tableRow, 3].Value = LanguageUtils.ConvertLanguage("血压", "Blood pressure");
                    worksheet.Cells[tableRow, 5].Value = LanguageUtils.ConvertLanguage("水分摄取量", "Moisture intake");
                    worksheet.Cells[tableRow, 7].Value = LanguageUtils.ConvertLanguage("平均指数", "Average index");
                    worksheet.Cells[tableRow, 8].Value = LanguageUtils.ConvertLanguage("总运动时间", "Total exercise time");
                    worksheet.Cells[tableRow, 10].Value = LanguageUtils.ConvertLanguage("总消耗热量", "Total calories consumed");
                    worksheet.Cells[tableRow + 1, 3].Value = LanguageUtils.ConvertLanguage("运动前", "Before exercise");
                    worksheet.Cells[tableRow + 1, 4].Value = LanguageUtils.ConvertLanguage("运动后", "After exercise");

                    using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow + 1, 11])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //range.Style.Font.Bold = true;
                        range.Style.Font.Name = "等线";
                        range.Style.Font.Size = 11;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 240, 244));
                    }


                    for (int i = 0, k=j* count; k<(j+1)* count && k < length; i++,k++)
                    {
                        //表头行+两个表头
                        int row = tableRow + 2 + i * 3;
                        worksheet.Cells[row, 1, row, 2].Merge = true;//实施日期
                        worksheet.Cells[row, 5, row, 6].Merge = true;//水分摄取量
                        worksheet.Cells[row, 8, row, 9].Merge = true;//总运动时间
                        worksheet.Cells[row, 10, row, 11].Merge = true;//总消耗热量
                        worksheet.Cells[row + 1, 1, row + 2, 11].Merge = true;

                        worksheet.Cells[row, 1].Value = ((DateTime)list[k].Gmt_Create).ToString();
                        worksheet.Cells[row, 3].Value = list[k].SI_Pre_HighPressure + "/" + list[k].SI_Pre_LowPressure;
                        worksheet.Cells[row, 4].Value = list[k].SI_Suf_HighPressure + "/" + list[k].SI_Suf_LowPressure;
                        worksheet.Cells[row, 5].Value = list[k].SI_WaterInput;
                        worksheet.Cells[row, 7].Value = list[k].PR_Index;
                        //worksheet.Cells[row, 8].Value = list[k].PR_Time2 - list[i].PR_Time1;
                        worksheet.Cells[row, 8].Value = list[k].PR_Time1;
                        worksheet.Cells[row, 10].Value = list[k].PR_Cal;
                        worksheet.Cells[row + 1, 1].Value = list[k].SI_CareInfo;
                    }

                    int borderRows = (j + 1) * count < list.Count ? count : list.Count - j * count;
                    using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow + 1 + borderRows * 3, 11])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.Font.Bold = true;
                        range.Style.Font.Name = "等线";
                        range.Style.Font.Size = 10;
                        //设置边框
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                }

                package.Save();
            }
        }

        /// <summary>
        /// 时间的选中时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void List_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            selectedDate.Add((DateTime)cb.Content);
            //Console.WriteLine("选中："+cb.Content);
        }


        /// <summary>
        /// 时间的撤销选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void List_CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            selectedDate.Remove((DateTime)cb.Content);
            //Console.WriteLine("未选中："+cb.Content);
        }

        /// <summary>
        /// 文档种类的选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Document_Type_Checked (object sender, RoutedEventArgs e)
        {
            try
            {
                //清空之前选中的时间
                selectedDate.Clear();
                if (is_comprehensiv.IsChecked == true || is_nurse.IsChecked == true)//训练报告或看护记录报告
                {
                    if (Current_User != null)
                    {
                        List<TrainingAndSymptomBean> list = excelService.ListTrainingAndSymptomByUserId(Current_User.Pk_User_Id);
                        trainingAndSymptomBeans = list;//赋值全局

                        List<DateTime?> dateTimes = new List<DateTime?>();
                        foreach (TrainingAndSymptomBean tas in list)
                        {
                            dateTimes.Add(tas.Gmt_Create);
                        }
                        selectedDate = dateTimes;

                        //更新数据
                        DateTime? startTime = getDateByStr(start_date.Text);
                        DateTime? endTime = getDateByStr(end_date.Text);
                        datalist.DataContext = listBeansByStartToEndTime(startTime, endTime);
                    }

                    //修改输出文档的名称
                    if (DocumentInput_Check != null)
                    {
                        if (DocumentInput_Check.IsChecked == true)
                        {
                            if (text_output_document.Text != "")
                            {
                                if (is_comprehensiv.IsChecked == true)
                                {
                                    if (text_output_document.Text.IndexOf(LanguageUtils.ConvertLanguage("综合报告", "Comprehensive report")) == -1)
                                    {
                                        text_output_document.Text = text_output_document.Text.Replace(LanguageUtils.ConvertLanguage("详细报告", "Detailed report"), LanguageUtils.ConvertLanguage("综合报告", "Comprehensive report"));
                                        text_output_document.Text = text_output_document.Text.Replace(LanguageUtils.ConvertLanguage("看护记录报告", " Care record report"), LanguageUtils.ConvertLanguage("综合报告", "Comprehensive report"));
                                    }
                                }
                                if (is_nurse.IsChecked == true)
                                {
                                    if (text_output_document.Text.IndexOf(LanguageUtils.ConvertLanguage("看护记录报告", " Care record report")) == -1)
                                    {
                                        text_output_document.Text = text_output_document.Text.Replace(LanguageUtils.ConvertLanguage("详细报告", "Detailed report"), LanguageUtils.ConvertLanguage("看护记录报告", " Care record report"));
                                        text_output_document.Text = text_output_document.Text.Replace(LanguageUtils.ConvertLanguage("综合报告", "Comprehensive report"), LanguageUtils.ConvertLanguage("看护记录报告", " Care record report"));
                                    }
                                }

                            }
                        }
                    }

                }
                else if (is_detail.IsChecked == true)//详细报告
                {
                    if (Current_User != null)
                    {
                        List<DevicePrescriptionExcel> list = excelService.ListTrainingDetailByUserId(Current_User.Pk_User_Id);
                        Console.WriteLine(list.ToString());
                        devicePrescriptionExcels = list;//赋值全局

                        List<DateTime?> dateTimes = new List<DateTime?>();
                        foreach (DevicePrescriptionExcel dpe in list)
                        {
                            dateTimes.Add(dpe.Gmt_Create);
                        }
                        selectedDate = dateTimes;

                        //更新数据
                        DateTime? startTime = getDateByStr(start_date.Text);
                        DateTime? endTime = getDateByStr(end_date.Text);
                        datalist.DataContext = listBeansByStartToEndTime(startTime, endTime);
                    }

                    //修改输出文档的名称
                    if (DocumentInput_Check != null)
                    {
                        if (DocumentInput_Check.IsChecked == true)
                        {
                            if (text_output_document.Text != "")
                            {
                                if (text_output_document.Text.IndexOf(LanguageUtils.ConvertLanguage("详细报告", "Detailed report")) == -1)
                                {
                                    text_output_document.Text = text_output_document.Text.Replace(LanguageUtils.ConvertLanguage("综合报告", "Comprehensive report"), LanguageUtils.ConvertLanguage("详细报告", "Detailed report"));
                                    text_output_document.Text = text_output_document.Text.Replace(LanguageUtils.ConvertLanguage("看护记录报告", " Care record report"), LanguageUtils.ConvertLanguage("详细报告", "Detailed report"));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("文档切换异常");
                logger.Error("文档切换异常");
            }
        }

        /// <summary>
        /// 文档输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_OutputDocument(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                sfd.Filter = LanguageUtils.ConvertLanguage("PDF文档（*.pdf）|*.pdf", "PDF document (*.pdf)|*.pdf");
                if (is_comprehensiv.IsChecked == true)
                {
                    sfd.FileName = Current_User.User_Name + "-" + LanguageUtils.ConvertLanguage("综合报告", "Comprehensive report") + "-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";
                }
                if (is_detail.IsChecked == true)
                {
                    sfd.FileName = Current_User.User_Name + "-" + LanguageUtils.ConvertLanguage("详细报告", "Detailed report") + "-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";
                }
                if (is_nurse.IsChecked == true)
                {
                    sfd.FileName = Current_User.User_Name + "-" + LanguageUtils.ConvertLanguage("看护记录报告", " Care record report") + "-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";
                }

                //设置默认文件类型显示顺序
                sfd.FilterIndex = 1;
                //保存对话框是否记忆上次打开的目录
                sfd.RestoreDirectory = true;
                if (sfd.ShowDialog() == true)
                {
                    text_output_document.Text = sfd.FileName;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("文档输出异常");
                logger.Error("文档输出异常");
            }
        }

        /// <summary>
        /// 打印预览功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Print_View(object sender, RoutedEventArgs e)
        {
            try
            {
                if (is_comprehensiv.IsChecked == true)//训练报告
                {
                    GenerateTrainReport();
                }
                else if (is_detail.IsChecked == true)//详细报告
                {
                    GenerateDetailReport();
                }
                else if (is_nurse.IsChecked == true)//看护记录报告
                {
                    GenerateNurseReport();
                }
                else
                {
                    return;
                }
            }
            catch (IOException ex)
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("文件可能被占用，请关闭相关文件", "The file may be occupied. Please close the relevant file"));
                return;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("生成Excel异常");
                logger.Error("生成Excel异常");
                return;
            }

            try
            { 
                //打印
                PdfViewer pDF = new PdfViewer();

                if (DocumentInput_Check.IsChecked == true)
                {
                    if (text_output_document.Text != "")
                    {
                        pDF.SaveToPath = text_output_document.Text;
                    }
                }

                pDF.Left = 200;
                pDF.Top = 10;
                pDF.Show();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("打印界面展示异常");
                logger.Error("打印界面展示异常");
            }
           
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click_Print(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }

        //所有的训练信息
        public List<TrainingAndSymptomBean> trainingAndSymptomBeans { get; set; }
        //所有的看护记录
        public List<DevicePrescriptionExcel> devicePrescriptionExcels { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void start_date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //时间范围 - 开始时间
            DateTime startTime = Convert.ToDateTime(start_date.Text);
            DateTime? endTime = getDateByStr(end_date.Text);

            if (endTime != null)
            {
                if (DateTime.Compare(startTime, (DateTime)endTime) > 0)
                {
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("起始时间不能大于终止时间", "Start time cannot be greater than termination time"));
                }
            }

            datalist.DataContext = listBeansByStartToEndTime(startTime, endTime);
        }

        //结束时间
        private void end_date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //时间范围 - 开始时间
            DateTime endTime = Convert.ToDateTime(end_date.Text);
            DateTime? startTime = getDateByStr(start_date.Text);

            if (startTime != null)
            {
                if (DateTime.Compare(endTime, (DateTime)startTime) < 0)
                {
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("终止时间不能小于起始时间", "The termination time cannot be less than the start time"));
                }
            }

            datalist.DataContext = listBeansByStartToEndTime(startTime, endTime);
        }

        private DateTime? getDateByStr(string time)
        {
            if (time != "")
            {
                return (DateTime?)(Convert.ToDateTime(time));
            }
            return null;
        }

        private List<object> listBeansByStartToEndTime(DateTime? startTime, DateTime? endTime)
        {
            List<object> newList = null;
            try
            {
                newList = new List<object>();
                if (is_comprehensiv.IsChecked == true || is_nurse.IsChecked == true)
                {
                    if (Current_User != null)
                    {
                        for (int i = 0; i < trainingAndSymptomBeans.Count; i++)
                        {
                            //判断选中哪些时间
                            //记录的时间
                            DateTime gmt_Create = (DateTime)trainingAndSymptomBeans[i].Gmt_Create;
                            if (startTime != null && endTime == null)
                            {
                                if (DateTime.Compare((DateTime)startTime, gmt_Create) < 0 || DateTime.Compare((DateTime)startTime, gmt_Create) == 0)
                                {
                                    newList.Add(trainingAndSymptomBeans[i]);
                                }
                            }
                            else if (startTime == null && endTime != null)
                            {
                                if (DateTime.Compare(((DateTime)endTime).AddDays(1), gmt_Create) > 0 || DateTime.Compare((DateTime)endTime, gmt_Create) == 0)
                                {
                                    newList.Add(trainingAndSymptomBeans[i]);
                                }
                            }
                            else if (startTime != null && endTime != null)
                            {
                                if ((DateTime.Compare((DateTime)startTime, gmt_Create) < 0 || DateTime.Compare((DateTime)startTime, gmt_Create) == 0) && (DateTime.Compare(gmt_Create, ((DateTime)endTime).AddDays(1)) < 0 || DateTime.Compare(gmt_Create, (DateTime)endTime) == 0))
                                {
                                    newList.Add(trainingAndSymptomBeans[i]);
                                }
                            }
                            else
                            {
                                newList.Add(trainingAndSymptomBeans[i]);
                            }
                        }
                    }
                }
                else if (is_detail.IsChecked == true)
                {
                    if (Current_User != null)
                    {
                        for (int i = 0; i < devicePrescriptionExcels.Count; i++)
                        {
                            //判断选中哪些时间
                            //记录的时间
                            DateTime gmt_Create = (DateTime)devicePrescriptionExcels[i].Gmt_Create;
                            if (startTime != null && endTime == null)
                            {
                                if (DateTime.Compare((DateTime)startTime, gmt_Create) < 0 || DateTime.Compare((DateTime)startTime, gmt_Create) == 0)
                                {
                                    newList.Add(devicePrescriptionExcels[i]);
                                }
                            }
                            else if (startTime == null && endTime != null)
                            {
                                if (DateTime.Compare(((DateTime)endTime).AddDays(1), gmt_Create) > 0 || DateTime.Compare((DateTime)endTime, gmt_Create) == 0)
                                {
                                    newList.Add(devicePrescriptionExcels[i]);
                                }
                            }
                            else if (startTime != null && endTime != null)
                            {
                                if ((DateTime.Compare((DateTime)startTime, gmt_Create) < 0 || DateTime.Compare((DateTime)startTime, gmt_Create) == 0) && (DateTime.Compare(gmt_Create, ((DateTime)endTime).AddDays(1)) < 0 || DateTime.Compare(gmt_Create, (DateTime)endTime) == 0))
                                {
                                    newList.Add(devicePrescriptionExcels[i]);
                                }
                            }
                            else
                            {
                                newList.Add(devicePrescriptionExcels[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("报表获取时间范围内的对象异常");
                logger.Error("报表获取时间范围内的对象异常");
            }
            return newList;
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
