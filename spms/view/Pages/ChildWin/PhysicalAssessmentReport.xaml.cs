using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using spms.bean;
using spms.entity;
using spms.service;
using spms.util;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// PhysicalAssessmentReport.xaml 的交互逻辑
    /// </summary>
    public partial class PhysicalAssessmentReport : Window
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
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        public PhysicalAssessmentReport()
        {
            InitializeComponent();
        }

        //取消操作，关闭窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //当前需要打印的用户
        public User Current_User { set; get; }
        //用户存放选中的时间
        private List<DateTime?> selectedDate = new List<DateTime?>();
        private ExcelService excelService = new ExcelService();

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

        private void Button_Click_Print(object sender, RoutedEventArgs e)
        {
            List<PhysicalPowerExcekVO> list = new List<PhysicalPowerExcekVO>();
            for (int i = 0; i < datalist.Items.Count; i++)
            {
                //判断选中哪些时间
                if (selectedDate.Contains((datalist.Items[i] as PhysicalPowerExcekVO).Gmt_Create))
                {
                    list.Add((datalist.Items[i] as PhysicalPowerExcekVO));
                }
            }


            FileInfo newFile = new FileInfo(@"e:\test.xlsx");
            if (newFile.Exists)
            {

                newFile.Delete();
                newFile = new FileInfo(@"e:\test.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("体力评价报告");

                int tableRow = 11;
                int length = list.Count;
                //设置所有的行高
                for (int i = 1; i <= tableRow + 14; i++)
                {
                    worksheet.Row(i).Height = 20;
                }

                int userRow = 4;
                ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, "体力评价报告", Current_User);

                worksheet.Cells[tableRow, 1, tableRow, 3].Merge = true;
                worksheet.Cells[tableRow + 1, 1, tableRow + 1, 3].Merge = true;
                worksheet.Cells[tableRow + 2, 1, tableRow + 2, 3].Merge = true;
                worksheet.Cells[tableRow + 3, 1, tableRow + 3, 3].Merge = true;
                worksheet.Cells[tableRow + 4, 1, tableRow + 4, 3].Merge = true;
                worksheet.Cells[tableRow + 5, 1, tableRow + 5, 3].Merge = true;
                worksheet.Cells[tableRow + 6, 1, tableRow + 6, 3].Merge = true;

                worksheet.Cells[tableRow + 1, 1].Value = "身高（cm）";
                worksheet.Cells[tableRow + 2, 1].Value = "体重（kg）";
                worksheet.Cells[tableRow + 3, 1].Value = "握力（kg）";
                worksheet.Cells[tableRow + 4, 1].Value = "睁眼单脚站立（秒）";
                worksheet.Cells[tableRow + 5, 1].Value = "功能性前伸（cm）";
                worksheet.Cells[tableRow + 6, 1].Value = "坐姿体前驱（cm）";


                for (int i = 0; i < 24; i++)
                {
                    worksheet.Cells[tableRow + i, 1, tableRow + i, 3].Merge = true;
                    worksheet.Cells[tableRow + i, 4, tableRow + i, 5].Merge = true;
                    worksheet.Cells[tableRow + i, 6, tableRow + i, 7].Merge = true;
                    worksheet.Cells[tableRow + i, 8, tableRow + i, 9].Merge = true;
                    worksheet.Cells[tableRow + i, 10, tableRow + i, 11].Merge = true;
                }

                int col = 4;
                //体力报告只有三条记录
                for (int i = 0; i < list.Count; i++)
                {
                    col = i * 2 + 4;
                    //表头行+两个表头
                    worksheet.Cells[tableRow, col].Value = string.Format("{0:d}", list[i].Gmt_Create);////ToShortDateString().ToString();
                    worksheet.Cells[tableRow + 1, col].Value = Convert.ToInt32(list[i].PP_High);
                    worksheet.Cells[tableRow + 2, col].Value = Convert.ToInt32(list[i].PP_Weight);
                    worksheet.Cells[tableRow + 3, col].Value = Convert.ToInt32(list[i].PP_Grip);
                    worksheet.Cells[tableRow + 4, col].Value = Convert.ToInt32(list[i].PP_EyeOpenStand);
                    worksheet.Cells[tableRow + 5, col].Value = Convert.ToInt32(list[i].PP_FunctionProtract);
                    worksheet.Cells[tableRow + 6, col].Value = Convert.ToInt32(list[i].PP_SitandReach);

                }

                worksheet.Cells[tableRow, 10].Value = "初次未变化";

                worksheet.Cells[tableRow, 10, tableRow, 11].Merge = true;
                worksheet.Cells[tableRow + 1, 10, tableRow + 1, 11].Merge = true;
                worksheet.Cells[tableRow + 2, 10, tableRow + 2, 11].Merge = true;
                worksheet.Cells[tableRow + 3, 10, tableRow + 3, 11].Merge = true;
                worksheet.Cells[tableRow + 4, 10, tableRow + 4, 11].Merge = true;
                worksheet.Cells[tableRow + 5, 10, tableRow + 5, 11].Merge = true;
                worksheet.Cells[tableRow + 6, 10, tableRow + 6, 11].Merge = true;
                //TODO 初次未变化

                using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow, 11])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.Font.Name = "等线";
                    range.Style.Font.Size = 11;
                    //设置边框
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 0, 139));
                }

                worksheet.Cells[tableRow, 3].Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                worksheet.Cells[tableRow, 4].Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                worksheet.Cells[tableRow, 5].Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                worksheet.Cells[tableRow, 6].Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                worksheet.Cells[tableRow, 7].Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                worksheet.Cells[tableRow, 8].Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                worksheet.Cells[tableRow, 9].Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                worksheet.Cells[tableRow, 10].Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));

                //空表表格
                using (ExcelRange range = worksheet.Cells[tableRow + 1, 1, tableRow + 6, 11])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "等线";
                    range.Style.Font.Size = 11;
                    //设置边框
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                //插入图表
                ExcelChart chart = worksheet.Drawings.AddChart("chart3", eChartType.RadarFilled);
                //Y轴数据源，X轴数据源
                ExcelChartSerie serie1 = chart.Series.Add(worksheet.Cells[tableRow + 1, 4, tableRow + 6, 4], worksheet.Cells[tableRow + 1, 1, tableRow + 6, 1]);
                serie1.HeaderAddress = worksheet.Cells[tableRow, 4];
                serie1.Border.Fill.Color = System.Drawing.Color.Red;
                if (list.Count >= 2)
                {
                    ExcelChartSerie serie2 = chart.Series.Add(worksheet.Cells[tableRow + 1, 6, tableRow + 6, 6], worksheet.Cells[tableRow + 1, 1, tableRow + 6, 1]);
                    serie2.HeaderAddress = worksheet.Cells[tableRow, 6];
                    serie2.Border.Fill.Color = System.Drawing.Color.Green;
                    if (list.Count == 3)
                    {
                        ExcelChartSerie serie3 = chart.Series.Add(worksheet.Cells[tableRow + 1, 8, tableRow + 6, 8], worksheet.Cells[tableRow + 1, 1, tableRow + 6, 1]);
                        serie3.HeaderAddress = worksheet.Cells[tableRow, 8];
                        serie3.Border.Fill.Color = System.Drawing.Color.Blue;
                    }
                }

                chart.SetPosition(25, 0, 0, 0);
                chart.SetSize(726, 300);
                chart.Title.Text = "体力检测";
                chart.Title.Font.Color = System.Drawing.Color.FromArgb(89, 89, 89);
                chart.Title.Font.Size = 15;
                chart.Title.Font.Bold = true;
                chart.Style = eChartStyle.Style15;
                //chart.Legend.Border.LineStyle = eLineStyle.Solid;
                //chart.Legend.Border.Fill.Color = Color.FromArgb(217, 217, 217);


                using (ExcelRange range = worksheet.Cells[tableRow + 1, 1, 24, 11])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //设置边框
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                int remarkRow = 42;
                ExcelUtil.GenerateRemark(ref worksheet, remarkRow, "备注信息");
                //保存
                package.Save();
            }

            //打印
            PdfViewer pDF = new PdfViewer();
            pDF.Left = 200;
            pDF.Top = 10;
            pDF.Show();
        }
    }
}
