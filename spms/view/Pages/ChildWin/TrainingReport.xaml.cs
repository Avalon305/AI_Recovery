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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

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
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        public TrainingReport()
        {
            InitializeComponent();
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
        private List<DateTime?> selectedDate = new List<DateTime?>();
        private ExcelService excelService = new ExcelService();

        private void Button_Click_Print(object sender, RoutedEventArgs e)
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

            //打印
            PdfViewer pDF = new PdfViewer();
            pDF.Left = 200;
            pDF.Top = 10;
            pDF.Show();
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

            FileInfo newFile = new FileInfo(@"e:\test.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(@"e:\test.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("训练报告");

                int tableRow = 12;
                int length = list.Count;
                //设置所有的行高
                for (int i = 1; i <= tableRow; i++)
                {
                    worksheet.Row(i).Height = 20;
                }

                //设置数据的高度
                for (int i = tableRow + 2; i <= tableRow + 2 + length; i++)
                {
                    worksheet.Row(i).Height = 25;
                }

                int userRow = 4;
                ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, "训练报告", Current_User);

                /*
                    2.设置实施状况的表格
                 */
                worksheet.Cells[userRow + 7, 1, userRow + 7, 2].Merge = true;
                worksheet.Cells[userRow + 7, 1].Value = "实施状况";
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
                worksheet.Cells[tableRow, 1].Value = "实施日期";
                worksheet.Cells[tableRow, 3].Value = "血压";
                worksheet.Cells[tableRow, 5].Value = "水分摄取量";
                worksheet.Cells[tableRow, 6].Value = "平均指数";
                worksheet.Cells[tableRow, 7].Value = "总运动时间";
                worksheet.Cells[tableRow, 8].Value = "总消耗热量";
                worksheet.Cells[tableRow, 9].Value = "看护记录";
                worksheet.Cells[tableRow + 1, 3].Value = "运动前";
                worksheet.Cells[tableRow + 1, 4].Value = "运动后";

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


                for (int i = 0; i < length; i++)
                {
                    //表头行+两个表头
                    int row = tableRow + 2 + i;
                    worksheet.Cells[row, 1, row, 2].Merge = true;
                    worksheet.Cells[row, 9, row, 11].Merge = true;
                    worksheet.Cells[row, 1].Value = list[i].Gmt_Create.ToString();
                    worksheet.Cells[row, 3].Value = list[i].SI_Pre_HighPressure + "/" + list[i].SI_Pre_LowPressure;
                    worksheet.Cells[row, 4].Value = list[i].SI_Suf_HighPressure + "/" + list[i].SI_Suf_LowPressure;
                    worksheet.Cells[row, 5].Value = list[i].SI_WaterInput;
                    worksheet.Cells[row, 6].Value = list[i].PR_Index;
                    worksheet.Cells[row, 7].Value = list[i].PR_Time2 - list[i].PR_Time1;
                    worksheet.Cells[row, 8].Value = list[i].PR_Cal;
                    worksheet.Cells[row, 9].Value = list[i].SI_CareInfo;
                }


                using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow + 1 + list.Count, 11])
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
                chart.SetPosition(29, 0, 0, 0);
                chart.SetSize(726, 300);
                chart.Title.Text = "实施报告";
                chart.Title.Font.Color = System.Drawing.Color.FromArgb(89, 89, 89);
                chart.Title.Font.Size = 15;
                chart.Title.Font.Bold = true;
                chart.Style = eChartStyle.Style15;
                //chart.Legend.Border.LineStyle = eLineStyle.Solid;
                //chart.Legend.Border.Fill.Color = Color.FromArgb(217, 217, 217);

                //备注
                int remarkRow = 46;
                ExcelUtil.GenerateRemark(ref worksheet, remarkRow, "备注信息");

                //保存
                package.Save();
            }
        }

        /// <summary>
        /// 生成详细训练报告
        /// </summary>
        private void GenerateDetailReport()
        {
            List<DevicePrescription> list = new List<DevicePrescription>();
            for (int i = 0; i < datalist.Items.Count; i++)
            {
                //判断选中哪些时间
                if (selectedDate.Contains((datalist.Items[i] as DevicePrescription).Gmt_Create))
                {
                    //Console.WriteLine("打印的内容" + datalist.Items[i].ToString());
                    list.Add((datalist.Items[i] as DevicePrescription));
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
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("详细训练报告");

                int tableRow = 11;
                int length = list.Count;
                //设置所有的行高
                for (int i = 1; i <= tableRow + length; i++)
                {
                    worksheet.Row(i).Height = 20;
                }
                int userRow = 4;
                ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, "详细训练报告", Current_User);

                worksheet.Cells[tableRow, 1, tableRow, 3].Merge = true;//合并单元格
                worksheet.Cells[tableRow, 10, tableRow, 11].Merge = true;//合并单元格
                worksheet.Cells[tableRow, 1].Value = "器械名称";
                worksheet.Cells[tableRow, 4].Value = "实施日期";
                worksheet.Cells[tableRow, 5].Value = "移乘方法";
                worksheet.Cells[tableRow, 6].Value = "砝码";
                worksheet.Cells[tableRow, 7].Value = "组数";
                worksheet.Cells[tableRow, 8].Value = "个数";
                worksheet.Cells[tableRow, 9].Value = "Borg";
                worksheet.Cells[tableRow, 10].Value = "动作时机、姿势等";

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
                for (int i = 0; i < length; i++)
                {
                    //表头行+两个表头
                    row = tableRow + 1 + i;
                    worksheet.Cells[row, 1, row, 3].Merge = true;
                    worksheet.Cells[row, 10, row, 11].Merge = true;
                    worksheet.Cells[row, 1].Value = list[i].DP_Attrs.Substring(0,list[i].DP_Attrs.IndexOf("*"));
                    worksheet.Cells[row, 4].Value = string.Format("{0:d}", list[i].Gmt_Create);
                    worksheet.Cells[row, 5].Value = list[i].dp_moveway;
                    worksheet.Cells[row, 6].Value = list[i].dp_weight;
                    worksheet.Cells[row, 7].Value = list[i].dp_groupcount;
                    worksheet.Cells[row, 8].Value = list[i].dp_groupnum;
                    worksheet.Cells[row, 9].Value = list[i].dp_relaxtime;
                    worksheet.Cells[row, 10].Value = list[i].DP_Memo;
                }


                using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow + length, 11])
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

            FileInfo newFile = new FileInfo(@"e:\test.xlsx");
            if (newFile.Exists)
            {

                newFile.Delete();
                newFile = new FileInfo(@"e:\test.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("看护记录报告");

                int tableRow = 12;
                int length = list.Count;
                //设置所有的行高
                for (int i = 1; i <= tableRow + length*2+2; i++)
                {
                    worksheet.Row(i).Height = 20;
                }

                int userRow = 4;
                ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, "看护记录报告", Current_User);


                /*
                   2.设置实施状况的表格
                */
                worksheet.Cells[userRow + 7, 1, userRow + 7, 2].Merge = true;
                worksheet.Cells[userRow + 7, 1].Value = "实施状况";
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
                worksheet.Cells[tableRow, 1].Value = "实施日期";
                worksheet.Cells[tableRow, 3].Value = "血压";
                worksheet.Cells[tableRow, 5].Value = "水分摄取量";
                worksheet.Cells[tableRow, 7].Value = "平均指数";
                worksheet.Cells[tableRow, 8].Value = "总运动时间";
                worksheet.Cells[tableRow, 10].Value = "总消耗热量";
                worksheet.Cells[tableRow + 1, 3].Value = "运动前";
                worksheet.Cells[tableRow + 1, 4].Value = "运动后";

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


                for (int i = 0; i < length; i++)
                {
                    //表头行+两个表头
                    int row = tableRow + 2 + i * 3;
                    worksheet.Cells[row, 1, row, 2].Merge = true;//实施日期
                    worksheet.Cells[row, 5, row, 6].Merge = true;//水分摄取量
                    worksheet.Cells[row, 8, row, 9].Merge = true;//总运动时间
                    worksheet.Cells[row, 10, row, 11].Merge = true;//总消耗热量
                    worksheet.Cells[row + 1, 1, row + 2, 11].Merge = true;

                    worksheet.Cells[row, 1].Value = list[i].Gmt_Create.ToString();
                    worksheet.Cells[row, 3].Value = list[i].SI_Pre_HighPressure + "/" + list[i].SI_Pre_LowPressure;
                    worksheet.Cells[row, 4].Value = list[i].SI_Suf_HighPressure + "/" + list[i].SI_Suf_LowPressure;
                    worksheet.Cells[row, 5].Value = list[i].SI_WaterInput;
                    worksheet.Cells[row, 7].Value = list[i].PR_Index;
                    worksheet.Cells[row, 8].Value = list[i].PR_Time2 - list[i].PR_Time1;
                    worksheet.Cells[row, 10].Value = list[i].PR_Cal;
                    worksheet.Cells[row + 1, 1].Value = list[i].SI_CareInfo;
                }

                using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow + 1 + length * 3, 11])
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
            if (is_comprehensiv.IsChecked == true || is_nurse.IsChecked == true)//训练报告或看护记录报告
            {
                if (Current_User != null)
                {
                    List<TrainingAndSymptomBean> list = excelService.ListTrainingAndSymptomByUserId(Current_User.Pk_User_Id);
                    datalist.DataContext = list;
                    Console.WriteLine(list.ToString());
                }
                
            }
            else if (is_detail.IsChecked == true)//详细报告
            {
                if (Current_User != null)
                {
                    List<DevicePrescription> list = excelService.ListTrainingDetailByUserId(Current_User.Pk_User_Id);
                    Console.WriteLine(list.ToString());
                    datalist.DataContext = list;
                }
            }
        }
    }
}
