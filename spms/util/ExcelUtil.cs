using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Reflection;
using System.Collections;
using spms.entity;
using System.Drawing;
using System.Windows.Forms;
using spms.view.dto;

namespace spms.util
{
    /// <summary>
    /// Excel操作工具类
    /// </summary>
    class ExcelUtil
    {

        /// <summary>
        /// 导出普通的Excel
        /// </summary>
        /// <param name="path">保存路径，包含文件名，不用带后缀</param>
        /// <param name="dataTable">表格内容（含表名、列名、内容）</param>
        public static void GenerateOrdinaryExcel(string path, User user, DataTable dataTable)
        {
            FileInfo newFile = new FileInfo(path);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path);
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = null;
                if (dataTable != null)
                {
                    worksheet = package.Workbook.Worksheets.Add(dataTable.TableName);
                }
                else
                {
                    worksheet = package.Workbook.Worksheets.Add(LanguageUtils.ConvertLanguage("默认", "Default"));
                }

                worksheet.Cells.Style.WrapText = true;
                worksheet.Cells.Style.ShrinkToFit = false;//单元格自动适应大小

                //worksheet.Cells[1, 1].Value = LanguageUtils.ConvertLanguage("利用者ID", "User ID");
                //worksheet.Cells[1, 2].Value = LanguageUtils.ConvertLanguage("利用者名称", "User name");
                //worksheet.Cells[1, 3].Value = LanguageUtils.ConvertLanguage("利用者拼音", "User name pinyin");
                //worksheet.Cells[1, 4].Value = LanguageUtils.ConvertLanguage("性别", "Gender"); 
                //worksheet.Cells[1, 5].Value = LanguageUtils.ConvertLanguage("年龄", "Age");
                //worksheet.Cells[1, 6].Value = LanguageUtils.ConvertLanguage("小组名称", "Group name");
                //worksheet.Cells[1, 7].Value = LanguageUtils.ConvertLanguage("初期要介护度", "Initial care"); 
                //worksheet.Cells[1, 8].Value = LanguageUtils.ConvertLanguage("现在要介护度", "Now care"); 
                //worksheet.Cells[1, 9].Value = LanguageUtils.ConvertLanguage("疾病名称", "Illness name"); 
                //worksheet.Cells[1, 10].Value = LanguageUtils.ConvertLanguage("残障名称", "Physical disabilities");
                worksheet.Cells[1, 1].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.SubjectID");
                worksheet.Cells[1, 2].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.LastName");
                worksheet.Cells[1, 3].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.FirstName");
                worksheet.Cells[1, 4].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.Gender");
                worksheet.Cells[1, 5].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.Age"); 
                worksheet.Cells[1, 6].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.Group"); 
                worksheet.Cells[1, 7].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.InitialCareLevel");
                worksheet.Cells[1, 8].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.CurrentCareLevel"); 
                worksheet.Cells[1, 9].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.Disease"); 
                worksheet.Cells[1, 10].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.Diagnosis"); 

                //设置用户基本信息的列宽
                for (int i = 1; i <= 10; i++)
                {
                    worksheet.Column(i).Width = 16;
                }
                worksheet.Column(1).Width = 20;
                //worksheet.Column(2).Width = 16;
                //worksheet.Column(3).Width = 16;
                //worksheet.Column(6).Width = 16;
                //worksheet.Column(7).Width = 16;
                //worksheet.Column(8).Width = 16;
                //worksheet.Column(9).Width = 16;
                //worksheet.Column(10).Width = 16;


                worksheet.Cells[2, 1].Value = GetObjContent(user.Pk_User_Id);
                worksheet.Cells[2, 2].Value = GetObjContent(user.User_Name);
                worksheet.Cells[2, 3].Value = GetObjContent(user.User_Namepinyin);
                worksheet.Cells[2, 4].Value = GetObjContent(user.User_Sex == 0 ? LanguageUtils.GetCurrentLanuageStrByKey("AddOrEditView.F") : LanguageUtils.GetCurrentLanuageStrByKey("AddOrEditView.M"));
                worksheet.Cells[2, 5].Value = GetObjContent(ConvertAge(user.User_Birth));
                worksheet.Cells[2, 6].Value = GetObjContent(user.User_GroupName);
                worksheet.Cells[2, 7].Value = GetObjContent(user.User_InitCare);
                worksheet.Cells[2, 8].Value = GetObjContent(user.User_Nowcare);
                worksheet.Cells[2, 9].Value = GetObjContent(user.User_IllnessName);
                worksheet.Cells[2, 10].Value = GetObjContent(user.User_PhysicalDisabilities);

                //设置用户信息的样式
                using (ExcelRange range = worksheet.Cells[1, 1, 1, 10])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "微软雅黑";
                    range.Style.Font.Size = 11;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                using (ExcelRange range = worksheet.Cells[2, 1, 2, 10])
                {
                    range.Style.Font.Name = "微软雅黑";
                    range.Style.Font.Size = 11;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                //设置用户信息的行高
                if (LanguageUtils.IsChainese())
                {
                    for (int i = 0; i < 2; i++)
                    {
                        worksheet.Row(i + 1).Height = 25;
                    }
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        worksheet.Row(i + 1).Height = 30;
                    }
                }  

                int tableRow = 4;

                if (dataTable != null)
                {
                    //症状信息
                    if (dataTable.TableName == LanguageUtils.ConvertLanguage("症状信息记录", "Symptom information record"))
                    {
                        worksheet.Cells[tableRow , 2, tableRow, 5].Merge = true;
                        worksheet.Cells[tableRow , 6, tableRow, 9].Merge = true;
                        worksheet.Cells[tableRow, 2].Value = LanguageUtils.ConvertLanguage("康复前", "Before recovery");
                        worksheet.Cells[tableRow, 6].Value = LanguageUtils.ConvertLanguage("康复后", "After rehabilitation"); 


                        using (ExcelRange range = worksheet.Cells[tableRow, 2, tableRow, 9])
                        {
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            range.Style.Font.Bold = true;
                            range.Style.Font.Name = "微软雅黑";
                            range.Style.Font.Size = 11;
                        }

                        tableRow += 1;
                    }

                    //设置列名和格式
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        worksheet.Cells[tableRow, col.Ordinal + 1].Value = col.Caption;
                    }

                    //设置超过数据的列宽
                    for (int i=11; i<= dataTable.Columns.Count; i++)
                    {
                        worksheet.Column(i).Width = 16;
                    }

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {
                            worksheet.Cells[i + tableRow +1, j + 1].Value = dataTable.Rows[i][j].ToString();
                        }
                    }

                    //统一设置表格的内容
                    int maxCol = 10;
                    if (dataTable.Columns.Count > 10)
                    {
                        maxCol = dataTable.Columns.Count;
                    }
                    using (ExcelRange range = worksheet.Cells[3, 1, dataTable.Rows.Count + tableRow, maxCol])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    //设置所有的行高
                    int maxRow = dataTable.Rows.Count + tableRow;
                    if (LanguageUtils.IsChainese())
                    {
                        for (int i = 2; i < maxRow; i++)
                        {
                            worksheet.Row(i + 1).Height = 25;
                        }
                    }
                    else
                    {
                        for (int i = 2; i < maxRow; i++)
                        {
                            worksheet.Row(i + 1).Height = 30;
                        }
                    }  

                    using (ExcelRange range = worksheet.Cells[tableRow, 1, tableRow, dataTable.Columns.Count])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Name = "微软雅黑";
                        range.Style.Font.Size = 11;
                        //range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    }

                    //统一设置表格的内容
                    using (ExcelRange range = worksheet.Cells[tableRow+1, 1, tableRow + 1+dataTable.Rows.Count, dataTable.Columns.Count])
                    {
                        //range.Style.Font.Bold = true;
                        range.Style.Font.Name = "微软雅黑";
                        range.Style.Font.Size = 10;
                        //range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    }
                }

                //保存
                package.Save();
            }
        }


        /// <summary>
        /// 将数据转换成datatable
        /// </summary>
        /// <param name="sheetName">sheet表名</param>
        /// <param name="columnNames">顶部列名（标题名）</param>
        /// <param name="list">数据集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable(string sheetName, string[] columnNames, List<object> list)
        {
            DataTable dataTable = null;
            try
            {
                if (list != null)
                {
                    dataTable = new DataTable();
                    dataTable.TableName = sheetName;
                    PropertyInfo[] propertys = list[0].GetType().GetProperties();
                    for (int i = 0; i < propertys.Length; i++)
                    {
                        dataTable.Columns.Add(columnNames[i], propertys[i].PropertyType);
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        ArrayList tempList = new ArrayList();
                        foreach (PropertyInfo pi in propertys)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        object[] array = tempList.ToArray();
                        dataTable.LoadDataRow(array, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("将数据转换成datatable发生异常");
            }
            return dataTable;
        }

        public static int ConvertAge(Object value)
        {
            try
            {
                DateTime revalue = System.Convert.ToDateTime(value);
                int age = DateTime.Now.Year - revalue.Year;
                return age;
            }
            catch (Exception e)
            {
                return 0;
            }

        }


        /// <summary>
        /// 将用户基本信息插入转pdf的Excel
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="userRow">用户基本信息的起始行</param>
        /// <param name="userBaseInfo"></param>
        public static void GenerateUserBaseInfoToExcel(ref ExcelWorksheet worksheet, int userRow, string title, User user)
        {
            /*
                0.设置头部信息
             */
            worksheet.Cells.Style.WrapText = true;
            //worksheet.View.ShowGridLines = false;//去掉sheet的网格线
            worksheet.PrinterSettings.TopMargin = 0;
            worksheet.PrinterSettings.RightMargin = 0;
            worksheet.PrinterSettings.LeftMargin = 0.28M;
            worksheet.PrinterSettings.BottomMargin = 0;
            worksheet.PrinterSettings.HeaderMargin = 0;
            worksheet.PrinterSettings.FooterMargin = 0;
            worksheet.Cells.Style.ShrinkToFit = true;//单元格自动适应大小

            //设置列宽
            worksheet.Column(1).Width = 5;
            worksheet.Column(5).Width = 10;
            worksheet.Column(7).Width = 10;
            worksheet.Column(8).Width = 10;
            worksheet.Column(11).Width = 14;

            /*
                0.头部部分
                */
            //插入图片
            //string fn = System.IO.Path.Combine((Application.StartupPath + @"\..\..\图片\"),"1.jpg");
            //Console.WriteLine(fn);
            int henderRow = 0;
            //string Path = Application.StartupPath.Substring(0, Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("\\")).LastIndexOf("\\"));
            //OfficeOpenXml.Drawing.ExcelPicture picture = worksheet.Drawings.AddPicture("logo", System.Drawing.Image.FromFile(Path+"//view//Images//logo.png"));//插入图片
            OfficeOpenXml.Drawing.ExcelPicture picture = null;
            if (LanguageUtils.IsChainese())
            {
                picture = worksheet.Drawings.AddPicture("logo", System.Drawing.Image.FromFile(CommUtil.GetDocPath("logo.png")));//插入图片
            }
            else
            {
                picture = worksheet.Drawings.AddPicture("logo", System.Drawing.Image.FromFile(CommUtil.GetDocPath("logo_english.png")));//插入图片
            }   

            picture.SetPosition(8, 5);//设置图片的位置
            picture.SetSize(120, 47);//设置图片的大小

            //标题
            worksheet.Cells[henderRow + 1, 4, henderRow + 2, 9].Merge = true;//合并单元格
            worksheet.Cells[henderRow + 1, 4].Value = title;
            worksheet.Cells[henderRow + 1, 4].Style.Font.Bold = true;
            worksheet.Cells[henderRow + 1, 4].Style.Font.Name = "微软雅黑";
            worksheet.Cells[henderRow + 1, 4].Style.Font.Size = 27;

            worksheet.Cells[henderRow + 1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[henderRow + 1, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //时间
            worksheet.Cells[henderRow + 2, 10, henderRow + 2, 11].Merge = true;//合并单元格
            worksheet.Cells[henderRow + 2, 10].Value = LanguageUtils.ConvertLanguage("作成日期", "Date")+" : " + DateTime.Now.ToString("yyyy-MM-dd");
            worksheet.Cells[henderRow + 2, 10].Style.Font.Name = "等线";
            worksheet.Cells[henderRow + 2, 10].Style.Font.Bold = true;
            worksheet.Cells[henderRow + 2, 10].Style.Font.Size = 10;
            worksheet.Cells[henderRow + 2, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[henderRow + 2, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            /*
                  1.用户的基本信息
                 */
            //int userRow = 4;
            worksheet.Cells[userRow, 1, userRow, 2].Merge = true;//合并单元格
            worksheet.Cells[userRow, 3, userRow, 5].Merge = true;
            worksheet.Cells[userRow + 1, 1, userRow + 1, 2].Merge = true;
            worksheet.Cells[userRow + 1, 3, userRow + 1, 5].Merge = true;
            worksheet.Cells[userRow + 1, 6, userRow + 1, 7].Merge = true;
            worksheet.Cells[userRow + 1, 8, userRow + 1, 9].Merge = true;

            worksheet.Cells[userRow + 2, 1, userRow + 2, 2].Merge = true;
            worksheet.Cells[userRow + 2, 6, userRow + 2, 7].Merge = true;
            worksheet.Cells[userRow + 2, 3, userRow + 2, 5].Merge = true;
            worksheet.Cells[userRow + 2, 8, userRow + 2, 9].Merge = true;
            worksheet.Cells[userRow + 3, 1, userRow + 3, 9].Merge = true;

            worksheet.Cells[userRow + 4, 1, userRow + 4, 3].Merge = true;
            worksheet.Cells[userRow + 4, 4, userRow + 4, 5].Merge = true;
            worksheet.Cells[userRow + 4, 6, userRow + 4, 7].Merge = true;
            worksheet.Cells[userRow + 4, 8, userRow + 4, 9].Merge = true;
            worksheet.Cells[userRow + 5, 1, userRow + 5, 3].Merge = true;
            worksheet.Cells[userRow + 5, 4, userRow + 5, 5].Merge = true;
            worksheet.Cells[userRow + 5, 6, userRow + 5, 7].Merge = true;
            worksheet.Cells[userRow + 5, 8, userRow + 5, 9].Merge = true;

            worksheet.Cells[userRow, 1].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.LastName");
            worksheet.Cells[userRow, 3].Value = GetObjContent(user.User_Name);
            worksheet.Cells[userRow, 6].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.Gender");
            if (user.User_Sex == 0x01)
            {
                worksheet.Cells[userRow, 7].Value = LanguageUtils.GetCurrentLanuageStrByKey("AddOrEditView.M");
            }
            else
            {
                worksheet.Cells[userRow, 7].Value = LanguageUtils.GetCurrentLanuageStrByKey("AddOrEditView.F");
            }

            worksheet.Cells[userRow, 8].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.Age");
            worksheet.Cells[userRow, 9].Value = ConvertAge(user.User_Birth);
            worksheet.Cells[userRow + 1, 3].Value = GetObjContent(user.User_Namepinyin);
            worksheet.Cells[userRow + 1, 6].Value = LanguageUtils.ConvertLanguage("出生年月日", "Date of birth"); 
            worksheet.Cells[userRow + 1, 8].Value = GetObjContent(string.Format("{0:d}", user.User_Birth));
            worksheet.Cells[userRow + 2, 1].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.SubjectID");
            worksheet.Cells[userRow + 2, 3].Value = GetObjContent(user.Pk_User_Id);
            worksheet.Cells[userRow + 2, 6].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.Group");
            worksheet.Cells[userRow + 2, 8].Value = GetObjContent(user.User_GroupName);
            worksheet.Cells[userRow + 4, 1].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.InitialCareLevel");
            worksheet.Cells[userRow + 4, 4].Value = GetObjContent(user.User_InitCare);
            worksheet.Cells[userRow + 4, 6].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.Disease");
            worksheet.Cells[userRow + 4, 8].Value = GetObjContent(user.User_IllnessName);
            worksheet.Cells[userRow + 5, 1].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.CurrentCareLevel");
            worksheet.Cells[userRow + 5, 4].Value = GetObjContent(user.User_Nowcare);
            worksheet.Cells[userRow + 5, 6].Value = LanguageUtils.GetCurrentLanuageStrByKey("SubjectInfoView.Diagnosis");
            worksheet.Cells[userRow + 5, 8].Value = GetObjContent(user.User_PhysicalDisabilities);

            //插入用户图片
            //OfficeOpenXml.Drawing.ExcelPicture userPicture = worksheet.Drawings.AddPicture("user", System.Drawing.Image.FromFile(user.User_PhotoLocation));//插入图片
            OfficeOpenXml.Drawing.ExcelPicture userPicture = null;
            try
            {
                Console.WriteLine("图片路径："+ CommUtil.GetUserPic(user.User_PhotoLocation));
                userPicture = worksheet.Drawings.AddPicture("user", Image.FromFile(CommUtil.GetUserPic(user.User_PhotoLocation)));//插入图片
                
                //userPicture.Border.LineStyle = eLineStyle.Solid;
                //userPicture.Fill.Style = eFillStyle.NoFill;//设置形状的填充样式
                //userPicture.Border.Fill.Style = eFillStyle.NoFill;//边框样式
            }
            catch (Exception e)
            {
                Console.WriteLine("用户指定照片路径下没有图片，照片不存在");
                if (LanguageUtils.IsChainese())
                {
                    userPicture = worksheet.Drawings.AddPicture("user", System.Drawing.Image.FromFile(CommUtil.GetDocPath("none.png")));//插入图片
                }
                else
                {
                    userPicture = worksheet.Drawings.AddPicture("user", System.Drawing.Image.FromFile(CommUtil.GetDocPath("none_english.png")));//插入图片
                }
                
            }
            userPicture.SetPosition(userRow - 1, 0, 9, 8);//设置图片的位置
            userPicture.SetSize(150, 145);//设置图片的大小
                                          //OfficeOpenXml.Drawing.ExcelPicture userPicture = worksheet.Drawings.AddPicture("user", System.Drawing.Image.FromFile(Path+"\\view\\Images\\excel\\timg.jpg"));//插入图片
                                          //userPicture.SetPosition(userRow - 1, 0, 9, 8);//设置图片的位置
                                          //userPicture.SetSize(150, 145);//设置图片的大小
                                          //userPicture.Border.LineStyle = eLineStyle.Solid;
                                          //userPicture.Fill.Style = eFillStyle.NoFill;//设置形状的填充样式
                                          //userPicture.Border.Fill.Style = eFillStyle.NoFill;//边框样式

            //设置用户基本信息的基本样式
            string cellRange1 = String.Format("{0}{1}{2}{3}", "A", userRow, ":I", userRow + 5);
            using (ExcelRange range = worksheet.Cells[cellRange1])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Font.Bold = true;
                //range.Style.Font.Color.SetColor(Color.White);
                range.Style.Font.Name = "等线";
                range.Style.Font.Size = 11;
                //设置边框
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }

            //设置用户信息某些特别的边框
            worksheet.Cells[userRow, 1].Style.Border.Bottom.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 1, 1].Style.Border.Top.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow, 2].Style.Border.Bottom.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 1, 2].Style.Border.Top.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow, 6].Style.Border.Bottom.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 1, 6].Style.Border.Top.Color.SetColor(Color.FromArgb(255, 255, 255));

            worksheet.Cells[userRow + 1, 6].Style.Border.Bottom.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 2, 6].Style.Border.Top.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 1, 7].Style.Border.Bottom.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 2, 7].Style.Border.Top.Color.SetColor(Color.FromArgb(255, 255, 255));

            worksheet.Cells[userRow + 4, 1].Style.Border.Bottom.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 5, 1].Style.Border.Top.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 4, 2].Style.Border.Bottom.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 5, 2].Style.Border.Top.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 4, 3].Style.Border.Bottom.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 5, 3].Style.Border.Top.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 4, 6].Style.Border.Bottom.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 5, 6].Style.Border.Top.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 4, 7].Style.Border.Bottom.Color.SetColor(Color.FromArgb(255, 255, 255));
            worksheet.Cells[userRow + 5, 7].Style.Border.Top.Color.SetColor(Color.FromArgb(255, 255, 255));

            //设置比较特别的样式
            //string cellRange2 = "A4:B4,F4,H4,F5:G5,A7:C7,F7:G7,A8:C8,F8:G8,A6:B6,F6:G6";
            string cellRange2 = "A" + userRow + ":B" + userRow + ",F" + userRow + ",H" + userRow + ",F" + (userRow + 1) + ":G" + (userRow + 1) + ",A" + (userRow + 4) + ":C" + (userRow + 4) + ",F" + (userRow + 4) + ":G" + (userRow + 4) + ",A" + (userRow + 5) + ":C" + (userRow + 5) + ",F" + (userRow + 5) + ":G" + (userRow + 5) + ",A" + (userRow + 2) + ":B" + (userRow + 2) + ",F" + (userRow + 2) + ":G" + (userRow + 2);
            //Console.WriteLine(cellRange2);
            using (ExcelRange range = worksheet.Cells[cellRange2.ToString()])
            {
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 0, 139));
            }
            //设置第七行的格式
            worksheet.Row(userRow + 3).Height = 5;
            worksheet.Row(userRow + 3).Style.Border.Left.Style = ExcelBorderStyle.None;
            worksheet.Row(userRow + 3).Style.Border.Right.Style = ExcelBorderStyle.None;
        }

        public static void GenerateRemark(ref ExcelWorksheet worksheet, int remarkRow, string remark)
        {
            worksheet.Cells[remarkRow, 1, remarkRow, 2].Merge = true;
            worksheet.Cells[remarkRow, 1].Value = LanguageUtils.ConvertLanguage("备注", "Remark");
            using (ExcelRange range = worksheet.Cells["A" + remarkRow + ":B" + remarkRow])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Font.Bold = true;
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Font.Name = "等线";
                range.Style.Font.Size = 11;
                //设置边框
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 0, 139));
            }
            worksheet.Cells["C" + remarkRow + ":K" + remarkRow + ",A" + (remarkRow + 1) + ":K" + (remarkRow + 4)].Merge = true;
            worksheet.Cells[remarkRow + 1, 1].Value = "     " + remark;
            using (ExcelRange range = worksheet.Cells["A" + remarkRow + ":K" + (remarkRow + 4)])
            {
                range.Style.Font.Bold = true;
                range.Style.Font.Name = "等线";
                range.Style.Font.Size = 11;
                //设置边框
                //range.Style.Border. = ExcelBorderStyle.Thin;
                //range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
        }


        /// <summary>
        /// 获取Excel文件
        /// </summary>
        /// <returns></returns>
        public static FileInfo GetExcelFile()
        {
            FileInfo newFile = new FileInfo(CommUtil.GetDocPath("test.xlsx"));
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(CommUtil.GetDocPath("test.xlsx"));
            }
            return newFile;
        }


        /// <summary>
        /// 返回对象的内容
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetObjContent(object obj)
        {
            if (obj != null)
            {
                if (obj.GetType() == typeof(String))
                {
                    return obj.ToString();
                }
                else if (obj.GetType() == typeof(DateTime))
                {
                    return obj.ToString();
                }
                else if (obj.GetType() == typeof(int) || obj.GetType() == typeof(double) || obj.GetType() == typeof(float))
                {
                    return obj + "";
                }
            }
            else
            {
                return "";
            }
            return "";
        }

    }
}
