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
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(dataTable.TableName);

                worksheet.Cells.Style.WrapText = true;
                worksheet.Cells.Style.ShrinkToFit = false;//单元格自动适应大小

                worksheet.Cells[1, 1].Value = "利用者ID";
                worksheet.Cells[1, 2].Value = "利用者名称";
                worksheet.Cells[1, 3].Value = "利用者名称（拼音）";
                worksheet.Cells[1, 4].Value = "性别";
                worksheet.Cells[1, 5].Value = "年龄";
                worksheet.Cells[1, 6].Value = "小组名称";
                worksheet.Cells[1, 7].Value = "初期要介护度";
                worksheet.Cells[1, 8].Value = "现在要介护度";
                worksheet.Cells[1, 9].Value = "疾病名称";
                worksheet.Cells[1, 10].Value = "残障名称";

                //设置列宽
                worksheet.Column(1).Width = 12;
                worksheet.Column(2).Width = 14;
                worksheet.Column(3).Width = 24;
                worksheet.Column(6).Width = 15;
                worksheet.Column(7).Width = 16;
                worksheet.Column(8).Width = 16;
                worksheet.Column(9).Width = 12;
                worksheet.Column(10).Width = 12;

                worksheet.Cells[2, 1].Value = user.Pk_User_Id;
                worksheet.Cells[2, 2].Value = user.User_Name;
                worksheet.Cells[2, 3].Value = user.User_Namepinyin;
                worksheet.Cells[2, 4].Value = user.User_Sex;
                worksheet.Cells[2, 5].Value = ConvertAge(user.User_Birth);
                worksheet.Cells[2, 6].Value = user.User_GroupName;
                worksheet.Cells[2, 7].Value = user.User_InitCare;
                worksheet.Cells[2, 8].Value = user.User_Nowcare;
                worksheet.Cells[2, 9].Value = user.User_IllnessName;
                worksheet.Cells[2, 10].Value = user.User_PhysicalDisabilities;


                //设置列名和格式
                foreach (DataColumn col in dataTable.Columns)
                {
                    worksheet.Cells[4, col.Ordinal + 1].Value = col.Caption;
                }

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 5, j + 1].Value = dataTable.Rows[i][j].ToString();
                    }
                }

                //统一设置表格的内容
                int maxCol = 10;
                if (dataTable.Columns.Count > 10)
                {
                    maxCol = dataTable.Columns.Count;
                }
                using (ExcelRange range = worksheet.Cells[1, 1, dataTable.Rows.Count + 4, maxCol])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                //设置所有的行高
                int maxRow = dataTable.Rows.Count + 4;
                for (int i = 0; i < maxRow; i++)
                {
                    worksheet.Row(i + 1).Height = 25;
                }

                using (ExcelRange range = worksheet.Cells[1, 1, 1, 10])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "微软雅黑";
                    range.Style.Font.Size = 11;
                    //range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                }
                using (ExcelRange range = worksheet.Cells[2, 1, 2, 10])
                {
                    range.Style.Font.Name = "微软雅黑";
                    range.Style.Font.Size = 11;
                }
                using (ExcelRange range = worksheet.Cells[4, 1, 4, dataTable.Columns.Count])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "微软雅黑";
                    range.Style.Font.Size = 11;
                    //range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                }

                //统一设置表格的内容
                using (ExcelRange range = worksheet.Cells[2, 1, dataTable.Rows.Count + 1, dataTable.Columns.Count])
                {
                    //range.Style.Font.Bold = true;
                    range.Style.Font.Name = "微软雅黑";
                    range.Style.Font.Size = 11;
                    //range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                }

                //保存
                package.Save();
            }
        }

        public static int ConvertAge(Object value)
        {
            DateTime revalue = System.Convert.ToDateTime(value);
            Console.WriteLine(revalue);
            Console.WriteLine(DateTime.Now.Year - revalue.Year);
            int age = DateTime.Now.Year - revalue.Year;
            return age;
        }

    }
}
