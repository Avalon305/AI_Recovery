using NLog;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using Spire.Xls;
using Recovery.bean;
using Recovery.entity;
using Recovery.service;
using Recovery.util;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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

namespace Recovery.view.Pages.ChildWin
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

            this.Height = SystemParameters.WorkArea.Size.Height;

            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        public PhysicalAssessmentReport()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;
            //启动初始化Excel转PDF的线程
            initializationExcelToPdfThread = new Thread(new ThreadStart(InitializationExcelToPdf));
            initializationExcelToPdfThread.Start();
        }

        //取消操作，关闭窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
        Thread initializationExcelToPdfThread;
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

        //当前需要打印的用户
        public User Current_User { set; get; }
        public List<PhysicalPowerExcekVO> physicalPowerExcekVOs { get; set; }
        //用户存放选中的时间
        public List<DateTime?> selectedDate { set; get; }
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

        /// <summary>
        /// 生成Excel文件
        /// </summary>
        private void GeneralTrainEvaluate()
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

            FileInfo newFile = ExcelUtil.GetExcelFile();
            int count = 3;//包含的数据条数
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                int pageSize = list.Count % count == 0 ? list.Count / count : (list.Count / count) + 1;

                if (pageSize == 0)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(LanguageUtils.ConvertLanguage("体力评价报告", "Physical Assessment"));

                    int tableRow = 11;
                    int length = list.Count;
                    //设置所有的行高
                    for (int i = 1; i <= tableRow + 14; i++)
                    {
                        worksheet.Row(i).Height = 20;
                    }

                    int userRow = 4;
                    ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, LanguageUtils.ConvertLanguage("体力评价报告", "Physical Assessment"), Current_User);
                }

                for (int j = 0; j < pageSize; j++)
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(LanguageUtils.ConvertLanguage("体力评价报告"+j, "Physical Assessment"+j));

                    int tableRow = 11;
                    int length = list.Count;
                    //设置所有的行高
                    for (int i = 1; i <= tableRow + 14; i++)
                    {
                        worksheet.Row(i).Height = 20;
                    }

                    int userRow = 4;
                    ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, LanguageUtils.ConvertLanguage("体力评价报告", "Physical Assessment"), Current_User);

                    //worksheet.Cells[tableRow, 1, tableRow, 3].Merge = true;
                    //worksheet.Cells[tableRow + 1, 1, tableRow + 1, 3].Merge = true;
                    //worksheet.Cells[tableRow + 2, 1, tableRow + 2, 3].Merge = true;
                    //worksheet.Cells[tableRow + 3, 1, tableRow + 3, 3].Merge = true;
                    //worksheet.Cells[tableRow + 4, 1, tableRow + 4, 3].Merge = true;
                    //worksheet.Cells[tableRow + 5, 1, tableRow + 5, 3].Merge = true;
                    //worksheet.Cells[tableRow + 6, 1, tableRow + 6, 3].Merge = true;

                    //worksheet.Cells[tableRow + 1, 1].Value = LanguageUtils.ConvertLanguage("身高（cm）", "Height (cm)");
                    //worksheet.Cells[tableRow + 2, 1].Value = LanguageUtils.ConvertLanguage("体重（kg）", "Weight (kg)");
                    //worksheet.Cells[tableRow + 3, 1].Value = LanguageUtils.ConvertLanguage("握力（kg）", "Grip (kg)");
                    //worksheet.Cells[tableRow + 4, 1].Value = LanguageUtils.ConvertLanguage("睁眼单脚站立（秒）", "Wink stand on one foot (seconds)");
                    //worksheet.Cells[tableRow + 5, 1].Value = LanguageUtils.ConvertLanguage("功能性前伸（cm）", "Functional reach (cm)");
                    //worksheet.Cells[tableRow + 6, 1].Value = LanguageUtils.ConvertLanguage("坐姿体前驱（cm）", "Sitting body precursor (cm)");

                    int r = 1;
                    try
                    {
                        if (SubstringParams(list[j * count].PP_High) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("身高（cm）", "Height (cm)");
                            r++;
                        }
                       if (SubstringParams(list[j * count].PP_Weight) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("体重（kg）", "Weight (kg)");
                            r++;
                        }
                        if (SubstringParams(list[j * count].PP_Grip) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("握力（kg）", "Grip (kg)");
                            r++;
                        }
                        if (SubstringParams(list[j * count].PP_EyeOpenStand) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("睁眼单脚站立（秒）", "Wink stand on one foot (s)");
                            r++;
                        }
                        if (SubstringParams(list[j * count].PP_FunctionProtract) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("功能性前伸（cm）", "Functional reach (cm)");
                            r++;
                        }
                        if (SubstringParams(list[j * count].PP_SitandReach) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("坐姿体前驱（cm）", "Sitting body precursor (cm)");
                            r++;
                        }
                        if (SubstringParams(list[j * count].PP_TimeUpGo) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("time&up go（秒）", "time&up g(s)o");
                            r++;
                        }
                        if (SubstringParams(list[j * count].PP_Walk5MileGeneral) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("5m步行-通常（秒）", "5m walk - usually(s)");
                            r++;
                        }
                        if (SubstringParams(list[j * count].PP_Walk5MileFast) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("5m步行-最快（秒）", "5m walk - fastest(s)");
                            r++;
                        }
                        if (SubstringParams(list[j * count].PP_Walk10Mile) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("10m步行（秒）", "10m walk(s)");
                            r++;
                        }
                        if (SubstringParams(list[j * count].PP_Walk6Minute) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("6分钟步行（米）", "6 minutes walk(m)");
                            r++;
                        }
                        if (SubstringParams(list[j * count].PP_Step2Minute) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("2分钟踏步（次）", "2 String step(times)");
                            r++;
                        }
                        if (SubstringParams(list[j * count].PP_LegRaise2Minute) != null)
                        {
                            worksheet.Cells[tableRow + r, 1].Value = LanguageUtils.ConvertLanguage("2分钟抬腿（次）", "2 minutes leg lift(times)");
                            r++;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("含有非法数据");
                        logger.Error("含有非法数据");
                    }


                    List<PhysicalPowerExcekVO> newList = new List<PhysicalPowerExcekVO>();
                    for (int k = j * count; k < (j + 1) * count && k < list.Count;k++)
                    {
                        PhysicalPowerExcekVO pp = new PhysicalPowerExcekVO();
                        int r2 = 1;
                        pp.Gmt_Create = list[k].Gmt_Create;
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("身高（cm）", "Height (cm)"))
                        {
                            pp.PP_High = list[k].PP_High;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("体重（kg）", "Weight (kg)"))
                        {
                            pp.PP_Weight = list[k].PP_Weight;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("握力（kg）", "Grip (kg)"))
                        {
                            pp.PP_Grip = list[k].PP_Grip;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("睁眼单脚站立（秒）", "Wink stand on one foot (s)"))
                        {
                            pp.PP_EyeOpenStand = list[k].PP_EyeOpenStand;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("功能性前伸（cm）", "Functional reach (cm)"))
                        {
                            pp.PP_FunctionProtract = list[k].PP_FunctionProtract;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("坐姿体前驱（cm）", "Sitting body precursor (cm)"))
                        {
                            pp.PP_SitandReach = list[k].PP_SitandReach;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("time&up go（秒）", "time&up g(s)o"))
                        {
                            pp.PP_TimeUpGo = list[k].PP_TimeUpGo;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("5m步行-通常（秒）", "5m walk - usually(s)"))
                        {
                            pp.PP_Walk5MileGeneral = list[k].PP_Walk5MileGeneral;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("5m步行-最快（秒）", "5m walk - fastest(s)"))
                        {
                            pp.PP_Walk5MileFast = list[k].PP_Walk5MileFast;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("10m步行（秒）", "10m walk(s)"))
                        {
                            pp.PP_Walk10Mile = list[k].PP_Walk10Mile;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("6分钟步行（米）", "6 minutes walk(m)"))
                        {
                            pp.PP_Walk6Minute = list[k].PP_Walk6Minute;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("2分钟踏步（次）", "2 String step(times)"))
                        {
                            pp.PP_Step2Minute = list[k].PP_Step2Minute;
                            r2++;
                        }
                        if ((string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("2分钟抬腿（次）", "2 minutes leg lift(times)"))
                        {
                            pp.PP_LegRaise2Minute = list[k].PP_LegRaise2Minute;
                            r2++;
                        }
                        
                        newList.Add(pp);
                    }

                    for (int i = 0; i < 24; i++)
                    {
                        worksheet.Cells[tableRow + i, 1, tableRow + i, 3].Merge = true;
                        worksheet.Cells[tableRow + i, 4, tableRow + i, 5].Merge = true;
                        worksheet.Cells[tableRow + i, 6, tableRow + i, 7].Merge = true;
                        worksheet.Cells[tableRow + i, 8, tableRow + i, 9].Merge = true;
                        worksheet.Cells[tableRow + i, 10, tableRow + i, 11].Merge = true;
                    }

                    int col = 4;
                    for (int i = 0; i< newList.Count; i++)
                    {
                        col = i * 2 + 4;
                        int r2 = 1;
                        worksheet.Cells[tableRow, col].Value = ((DateTime)newList[i].Gmt_Create).GetDateTimeFormats('g')[0].ToString();
                        if (newList[i].PP_High != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_High);
                            r2++;
                        }
                        if (newList[i].PP_Weight != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_Weight);
                            r2++;
                        }
                        if (newList[i].PP_Grip != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_Grip);
                            r2++;
                        }
                        if (newList[i].PP_EyeOpenStand != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_EyeOpenStand);
                            r2++;
                        }
                        if (newList[i].PP_FunctionProtract != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_FunctionProtract);
                            r2++;
                        }
                        if (newList[i].PP_SitandReach != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_SitandReach);
                            r2++;
                        }
                        if (newList[i].PP_TimeUpGo != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_TimeUpGo);
                            r2++;
                        }
                        if (newList[i].PP_Walk5MileGeneral != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_Walk5MileGeneral);
                            r2++;
                        }
                        if (newList[i].PP_Walk5MileFast != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_Walk5MileFast);
                            r2++;
                        }
                        if (newList[i].PP_Walk10Mile != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_Walk10Mile);
                            r2++;
                        }
                        if (newList[i].PP_Walk6Minute != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_Walk6Minute);
                            r2++;
                        }
                        if (newList[i].PP_Step2Minute != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_Step2Minute);
                            r2++;
                        }
                        if (newList[i].PP_LegRaise2Minute != null)
                        {
                            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(newList[i].PP_LegRaise2Minute);
                            r2++;
                        }
                    }

                    //体力报告只有三条记录
                    //for (int i = 0,k=j* count; k < (j+1)* count && k < list.Count; i++,k++)
                    //{
                    //    col = i * 2 + 4;
                    //    int r2 = 1;
                    //    //表头行+两个表头，必须是数值
                    //    try
                    //    {
                    //        worksheet.Cells[tableRow, col].Value = ((DateTime)list[k].Gmt_Create).GetDateTimeFormats('g')[0].ToString();////ToShortDateString().ToString();
                    //        if (SubstringParams(list[k].PP_High) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("身高（cm）", "Height (cm)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_High);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[k].PP_Weight) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("体重（kg）", "Weight (kg)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_Weight);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[k].PP_Grip) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("握力（kg）", "Grip (kg)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_Grip);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[k].PP_EyeOpenStand) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("睁眼单脚站立（秒）", "Wink stand on one foot (s)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_EyeOpenStand);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[k].PP_FunctionProtract) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("功能性前伸（cm）", "Functional reach (cm)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_FunctionProtract);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[k].PP_SitandReach) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("坐姿体前驱（cm）", "Sitting body precursor (cm)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_SitandReach);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[j * count].PP_TimeUpGo) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("time&up go（秒）", "time&up g(s)o"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_TimeUpGo);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[j * count].PP_Walk5MileGeneral) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("5m步行-通常（秒）", "5m walk - usually(s)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_Walk5MileGeneral);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[j * count].PP_Walk5MileFast) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("5m步行-最快（秒）", "5m walk - fastest(s)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_Walk5MileFast);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[j * count].PP_Walk10Mile) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("10m步行（秒）", "10m walk(s)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_Walk10Mile);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[j * count].PP_Walk6Minute) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("6分钟步行（米）", "6 minutes walk(m)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_Walk6Minute);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[j * count].PP_Step2Minute) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("2分钟踏步（次）", "2 String step(times)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_Step2Minute);
                    //            r2++;
                    //        }
                    //        if (SubstringParams(list[j * count].PP_LegRaise2Minute) != null && (string)worksheet.Cells[tableRow + r2, 1].Value == LanguageUtils.ConvertLanguage("2分钟抬腿（次）", "2 minutes leg lift(times)"))
                    //        {
                    //            worksheet.Cells[tableRow + r2, col].Value = SubstringParams(list[k].PP_LegRaise2Minute);
                    //            r2++;
                    //        }
                    //        //worksheet.Cells[tableRow + 1, col].Value = SubstringParams(list[k].PP_High);
                    //        //worksheet.Cells[tableRow + 2, col].Value = SubstringParams(list[k].PP_Weight);
                    //        //worksheet.Cells[tableRow + 3, col].Value = SubstringParams(list[k].PP_Grip);
                    //        //worksheet.Cells[tableRow + 4, col].Value = SubstringParams(list[k].PP_EyeOpenStand);
                    //        //worksheet.Cells[tableRow + 5, col].Value = SubstringParams(list[k].PP_FunctionProtract);
                    //        //worksheet.Cells[tableRow + 6, col].Value = SubstringParams(list[k].PP_SitandReach);

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine("含有非法数据");
                    //    }

                    //}

                    worksheet.Cells[tableRow, 10].Value = LanguageUtils.ConvertLanguage("初次未变化", " No change for           the first time");

                    worksheet.Cells[tableRow, 10, tableRow, 11].Merge = true;
                    worksheet.Cells[tableRow + 1, 10, tableRow + 1, 11].Merge = true;
                    worksheet.Cells[tableRow + 2, 10, tableRow + 2, 11].Merge = true;
                    worksheet.Cells[tableRow + 3, 10, tableRow + 3, 11].Merge = true;
                    worksheet.Cells[tableRow + 4, 10, tableRow + 4, 11].Merge = true;
                    worksheet.Cells[tableRow + 5, 10, tableRow + 5, 11].Merge = true;
                    worksheet.Cells[tableRow + 6, 10, tableRow + 6, 11].Merge = true;

                    for (int i = 1 ; i < tableRow + r ; i++)
                    {
                        if (worksheet.Cells[tableRow + i, 4].Value != null)
                        {
                            if (worksheet.Cells[tableRow + i, 8].Value != null)
                            {
                                int value = (int)worksheet.Cells[tableRow + i, 8].Value - (int)worksheet.Cells[tableRow + i, 4].Value;
                                if (value < 0)
                                {
                                    worksheet.Cells[tableRow + i, 10].Value = value + "（↓）";
                                }
                                else if (value > 0)
                                {
                                    worksheet.Cells[tableRow + i, 10].Value = value + "（↑）";
                                }
                            }
                            else if(worksheet.Cells[tableRow + i, 6].Value != null)
                            {
                                int value = (int)worksheet.Cells[tableRow + i, 6].Value - (int)worksheet.Cells[tableRow + i, 4].Value;
                                if (value < 0)
                                {
                                    worksheet.Cells[tableRow + i, 10].Value = value + "（↓）";
                                }
                                else if (value > 0)
                                {
                                    worksheet.Cells[tableRow + i, 10].Value = value + "（↑）";
                                }
                            }
                        }
                    }


                    //初次未变化
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
                    using (ExcelRange range = worksheet.Cells[tableRow + 1, 1, tableRow + r - 1, 11])
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

                    int borderRows = (j + 1) * count < list.Count ? count : list.Count - j * count;
                    //插入图表
                    ExcelChart chart = worksheet.Drawings.AddChart("chart3", eChartType.RadarFilled);
                    //Y轴数据源，X轴数据源
                    ExcelChartSerie serie1 = chart.Series.Add(worksheet.Cells[tableRow + 1, 4, tableRow + r - 1, 4], worksheet.Cells[tableRow + 1, 1, tableRow + r - 1, 1]);
                    serie1.HeaderAddress = worksheet.Cells[tableRow, 4];
                    serie1.Border.Fill.Color = System.Drawing.Color.Red;
                    if (borderRows >= 2)
                    {
                        ExcelChartSerie serie2 = chart.Series.Add(worksheet.Cells[tableRow + 1, 6, tableRow + r - 1, 6], worksheet.Cells[tableRow + 1, 1, tableRow + r - 1, 1]);
                        serie2.HeaderAddress = worksheet.Cells[tableRow, 6];
                        serie2.Border.Fill.Color = System.Drawing.Color.Green;
                        if (borderRows == 3)
                        {
                            ExcelChartSerie serie3 = chart.Series.Add(worksheet.Cells[tableRow + 1, 8, tableRow + r - 1, 8], worksheet.Cells[tableRow + 1, 1, tableRow + r - 1, 1]);
                            serie3.HeaderAddress = worksheet.Cells[tableRow, 8];
                            serie3.Border.Fill.Color = System.Drawing.Color.Blue;
                        }
                    }

                    chart.SetPosition(25, 0, 0, 0);
                    chart.SetSize(726, 300);
                    chart.Title.Text = LanguageUtils.ConvertLanguage("体力检测", "Physical testing");
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
                    ExcelUtil.GenerateRemark(ref worksheet, remarkRow, Current_User.User_Memo);
                }

                //ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(LanguageUtils.ConvertLanguage("体力评价报告", "Physical Assessment"));

                //int tableRow = 11;
                //int length = list.Count;
                ////设置所有的行高
                //for (int i = 1; i <= tableRow + 14; i++)
                //{
                //    worksheet.Row(i).Height = 20;
                //}

                //int userRow = 4;
                //ExcelUtil.GenerateUserBaseInfoToExcel(ref worksheet, userRow, LanguageUtils.ConvertLanguage("体力评价报告", "Physical Assessment"), Current_User);

                //worksheet.Cells[tableRow, 1, tableRow, 3].Merge = true;
                //worksheet.Cells[tableRow + 1, 1, tableRow + 1, 3].Merge = true;
                //worksheet.Cells[tableRow + 2, 1, tableRow + 2, 3].Merge = true;
                //worksheet.Cells[tableRow + 3, 1, tableRow + 3, 3].Merge = true;
                //worksheet.Cells[tableRow + 4, 1, tableRow + 4, 3].Merge = true;
                //worksheet.Cells[tableRow + 5, 1, tableRow + 5, 3].Merge = true;
                //worksheet.Cells[tableRow + 6, 1, tableRow + 6, 3].Merge = true;

                //worksheet.Cells[tableRow + 1, 1].Value = LanguageUtils.ConvertLanguage("身高（cm）", "Height (cm)");
                //worksheet.Cells[tableRow + 2, 1].Value = LanguageUtils.ConvertLanguage("体重（kg）", "Weight (kg)"); 
                //worksheet.Cells[tableRow + 3, 1].Value = LanguageUtils.ConvertLanguage("握力（kg）", "Grip (kg)"); 
                //worksheet.Cells[tableRow + 4, 1].Value = LanguageUtils.ConvertLanguage("睁眼单脚站立（秒）", "Wink stand on one foot (seconds)"); 
                //worksheet.Cells[tableRow + 5, 1].Value = LanguageUtils.ConvertLanguage("功能性前伸（cm）", "Functional reach (cm)");
                //worksheet.Cells[tableRow + 6, 1].Value = LanguageUtils.ConvertLanguage("坐姿体前驱（cm）", "Sitting body precursor (cm)"); 


                //for (int i = 0; i < 24; i++)
                //{
                //    worksheet.Cells[tableRow + i, 1, tableRow + i, 3].Merge = true;
                //    worksheet.Cells[tableRow + i, 4, tableRow + i, 5].Merge = true;
                //    worksheet.Cells[tableRow + i, 6, tableRow + i, 7].Merge = true;
                //    worksheet.Cells[tableRow + i, 8, tableRow + i, 9].Merge = true;
                //    worksheet.Cells[tableRow + i, 10, tableRow + i, 11].Merge = true;
                //}

                //int col = 4;
                ////体力报告只有三条记录
                //for (int i = 0; i < list.Count; i++)
                //{
                //    col = i * 2 + 4;
                //    //表头行+两个表头，必须是数值
                //    try
                //    {
                //        worksheet.Cells[tableRow, col].Value = string.Format("{0:d}", list[i].Gmt_Create);////ToShortDateString().ToString();
                //        worksheet.Cells[tableRow + 1, col].Value = SubstringParams(list[i].PP_High);
                //        worksheet.Cells[tableRow + 2, col].Value = SubstringParams(list[i].PP_Weight);
                //        worksheet.Cells[tableRow + 3, col].Value = SubstringParams(list[i].PP_Grip);
                //        worksheet.Cells[tableRow + 4, col].Value = SubstringParams(list[i].PP_EyeOpenStand);
                //        worksheet.Cells[tableRow + 5, col].Value = SubstringParams(list[i].PP_FunctionProtract);
                //        worksheet.Cells[tableRow + 6, col].Value = SubstringParams(list[i].PP_SitandReach);

                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine("含有非法数据");
                //    }
                   
                //}

                //worksheet.Cells[tableRow, 10].Value = LanguageUtils.ConvertLanguage("初次未变化", " No change for           the first time"); 

                //worksheet.Cells[tableRow, 10, tableRow, 11].Merge = true;
                //worksheet.Cells[tableRow + 1, 10, tableRow + 1, 11].Merge = true;
                //worksheet.Cells[tableRow + 2, 10, tableRow + 2, 11].Merge = true;
                //worksheet.Cells[tableRow + 3, 10, tableRow + 3, 11].Merge = true;
                //worksheet.Cells[tableRow + 4, 10, tableRow + 4, 11].Merge = true;
                //worksheet.Cells[tableRow + 5, 10, tableRow + 5, 11].Merge = true;
                //worksheet.Cells[tableRow + 6, 10, tableRow + 6, 11].Merge = true;
                ////初次未变化

                //using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow, 11])
                //{
                //    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //    range.Style.Font.Bold = true;
                //    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                //    range.Style.Font.Name = "等线";
                //    range.Style.Font.Size = 11;
                //    //设置边框
                //    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 0, 139));
                //}

                //worksheet.Cells[tableRow, 3].Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                //worksheet.Cells[tableRow, 4].Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                //worksheet.Cells[tableRow, 5].Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                //worksheet.Cells[tableRow, 6].Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                //worksheet.Cells[tableRow, 7].Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                //worksheet.Cells[tableRow, 8].Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                //worksheet.Cells[tableRow, 9].Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                //worksheet.Cells[tableRow, 10].Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));

                ////空表表格
                //using (ExcelRange range = worksheet.Cells[tableRow + 1, 1, tableRow + 6, 11])
                //{
                //    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //    range.Style.Font.Bold = true;
                //    range.Style.Font.Name = "等线";
                //    range.Style.Font.Size = 11;
                //    //设置边框
                //    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //}

                ////插入图表
                //ExcelChart chart = worksheet.Drawings.AddChart("chart3", eChartType.RadarFilled);
                ////Y轴数据源，X轴数据源
                //ExcelChartSerie serie1 = chart.Series.Add(worksheet.Cells[tableRow + 1, 4, tableRow + 6, 4], worksheet.Cells[tableRow + 1, 1, tableRow + 6, 1]);
                //serie1.HeaderAddress = worksheet.Cells[tableRow, 4];
                //serie1.Border.Fill.Color = System.Drawing.Color.Red;
                //if (list.Count >= 2)
                //{
                //    ExcelChartSerie serie2 = chart.Series.Add(worksheet.Cells[tableRow + 1, 6, tableRow + 6, 6], worksheet.Cells[tableRow + 1, 1, tableRow + 6, 1]);
                //    serie2.HeaderAddress = worksheet.Cells[tableRow, 6];
                //    serie2.Border.Fill.Color = System.Drawing.Color.Green;
                //    if (list.Count == 3)
                //    {
                //        ExcelChartSerie serie3 = chart.Series.Add(worksheet.Cells[tableRow + 1, 8, tableRow + 6, 8], worksheet.Cells[tableRow + 1, 1, tableRow + 6, 1]);
                //        serie3.HeaderAddress = worksheet.Cells[tableRow, 8];
                //        serie3.Border.Fill.Color = System.Drawing.Color.Blue;
                //    }
                //}

                //chart.SetPosition(25, 0, 0, 0);
                //chart.SetSize(726, 300);
                //chart.Title.Text = LanguageUtils.ConvertLanguage("体力检测", "Physical testing");
                //chart.Title.Font.Color = System.Drawing.Color.FromArgb(89, 89, 89);
                //chart.Title.Font.Size = 15;
                //chart.Title.Font.Bold = true;
                //chart.Style = eChartStyle.Style15;
                ////chart.Legend.Border.LineStyle = eLineStyle.Solid;
                ////chart.Legend.Border.Fill.Color = Color.FromArgb(217, 217, 217);


                //using (ExcelRange range = worksheet.Cells[tableRow + 1, 1, 24, 11])
                //{
                //    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //    //设置边框
                //    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //}

                //int remarkRow = 42;
                //ExcelUtil.GenerateRemark(ref worksheet, remarkRow, Current_User.User_PhysicalDisabilities);
                //保存
                package.Save();
            }
        }

        private void Button_Click_Print(object sender, RoutedEventArgs e)
        {
            try
            {
                GeneralTrainEvaluate();
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
                //Console.WriteLine("体力评价报告打印异常");
                logger.Error("体力评价报告打印异常");
            }
           
        }

        /// <summary>
        /// 从字符串中截取第一个参数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int? SubstringParams(string value)
        {
            int startIndex = value.IndexOf(",") + 1;
            int endIndex = value.IndexOf(",", startIndex);
            string result = value.Substring(startIndex, endIndex - startIndex);
            
            if (result.Contains("param"))
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(result);
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
                sfd.FileName = Current_User.User_Name + "-" + LanguageUtils.ConvertLanguage("体力评价报告", "Physical Assessment Report") + "-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";

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

            }
           
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Print_View(object sender, RoutedEventArgs e)
        {

            try
            {
                GeneralTrainEvaluate();
            }
            catch (IOException ex)
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("文件可能被占用，请关闭相关文件", "The file may be occupied. Please close the relevant file"));
                return;
            }
            catch (Exception ex)
            {
                logger.Error("生成Excel异常");
                //Console.WriteLine("生成Excel异常");
                return;
            }


            //打印
            try
            {
                PdfViewer pDF = new PdfViewer();
                pDF.SaveToPath = text_output_document.Text;
                pDF.Left = 200;
                pDF.Top = 10;
                pDF.Show();
            }
            catch (Exception ex)
            {

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
                if (Current_User != null)
                {
                    for (int i = 0; i < physicalPowerExcekVOs.Count; i++)
                    {
                        //判断选中哪些时间
                        //记录的时间
                        DateTime gmt_Create = (DateTime)physicalPowerExcekVOs[i].Gmt_Create;
                        if (startTime != null && endTime == null)
                        {
                            if (DateTime.Compare((DateTime)startTime, gmt_Create) < 0 || DateTime.Compare((DateTime)startTime, gmt_Create) == 0)
                            {
                                newList.Add(physicalPowerExcekVOs[i]);
                            }
                        }
                        else if (startTime == null && endTime != null)
                        {
                            if (DateTime.Compare(((DateTime)endTime).AddDays(1), gmt_Create) > 0 || DateTime.Compare((DateTime)endTime, gmt_Create) == 0)
                            {
                                newList.Add(physicalPowerExcekVOs[i]);
                            }
                        }
                        else if (startTime != null && endTime != null)
                        {
                            if ((DateTime.Compare((DateTime)startTime, gmt_Create) < 0 || DateTime.Compare((DateTime)startTime, gmt_Create) == 0) && (DateTime.Compare(gmt_Create, ((DateTime)endTime).AddDays(1)) < 0 || DateTime.Compare(gmt_Create, (DateTime)endTime) == 0))
                            {
                                newList.Add(physicalPowerExcekVOs[i]);
                            }
                        }
                        else
                        {
                            newList.Add(physicalPowerExcekVOs[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

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
