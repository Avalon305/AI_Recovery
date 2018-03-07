using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Reflection;
using System.Collections;

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
        public static void GenerateOrdinaryExcel(string path, DataTable dataTable)
        {
            try
            {
                //创建一个工作簿
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(dataTable.TableName);
                IRow rowH = sheet.CreateRow(0);
                ICell cell = null;
                //单元格格式
                ICellStyle cellStyle = workbook.CreateCellStyle();
                IDataFormat dataFormat = workbook.CreateDataFormat();
                cellStyle.DataFormat = dataFormat.GetFormat("@");

                //TODO:样式后期修改
                //文字水平
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 14;
                font.FontName = "宋体";
                cellStyle.SetFont(font);


                //设置列名和格式
                foreach (DataColumn col in dataTable.Columns)
                {
                    rowH.CreateCell(col.Ordinal).SetCellValue(col.Caption);
                    rowH.Cells[col.Ordinal].CellStyle = cellStyle;
                }

                //写入数据，第一行不写
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.SetCellValue(dataTable.Rows[i][j].ToString());
                        cell.CellStyle = cellStyle;
                    }
                }

                //TODO 后期去掉时间后缀
                //创建文件
                string savePath = path + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xls";
                FileStream file = new FileStream(savePath, FileMode.CreateNew, FileAccess.Write);

                //创建一个 IO 流,写入到流
                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                byte[] bytes = ms.ToArray();
                file.Write(bytes, 0, bytes.Length);
                file.Flush();

                //释放资源
                bytes = null;

                ms.Close();
                ms.Dispose();
            }
            catch (Exception ex)
            {
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
            DataTable dataTable = new DataTable();
            dataTable.TableName = sheetName;
            if (list.Count > 0)
            {
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
            return dataTable;
        }
    }
}
