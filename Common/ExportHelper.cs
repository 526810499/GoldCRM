using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Reflection;
using System.IO;
using System.Web;

namespace XHD.Common
{
    public class ExportHelper
    {
        #region 导出 Excel

        /// <summary>
        ///  T转为model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {

            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                return new DataTable();
            }

            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable("dt");
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }

            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);

                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        /// <summary>
        ///  T转为model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static MemoryStream ListToDataTable<T>(List<T> entitys, string sheetName)
        {
            DataTable dt = ListToDataTable<T>(entitys);
            return DtToMs(dt, sheetName);
        }

        private static MemoryStream DtToMs(DataTable sourceTable, string sheetName)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet1 = hssfworkbook.CreateSheet(sheetName);
            IRow rowhead = sheet1.CreateRow(0);
            // handling header.


            //取得列宽
            int[] arrColWidth = new int[sourceTable.Columns.Count];
            foreach (DataColumn item in sourceTable.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }

            foreach (DataColumn column in sourceTable.Columns)
            {
                rowhead.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                sheet1.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 10) * 256);
            }
            // handling value.
            int rowIndex = 1;

            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet1.CreateRow(rowIndex);
                foreach (DataColumn column in sourceTable.Columns)
                {

                    string Values = row[column].ToString();
                    string ColumnName = column.ColumnName;

                    ICell celltemp = dataRow.CreateCell(column.Ordinal);
                    if (ColumnName == "条形码" || ColumnName == "条码")
                    {
                        celltemp.SetCellValue(Values);
                        continue;
                    }
                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型   
                            celltemp.SetCellValue(Values);
                            break;
                        case "System.DateTime"://日期类型   
                            DateTime dateV;
                            DateTime.TryParse(Values, out dateV);
                            celltemp.SetCellValue(dateV);
                            break;
                        case "System.Boolean"://布尔型   
                            bool boolV = false;
                            bool.TryParse(Values, out boolV);
                            celltemp.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型   
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(Values, out intV);
                            celltemp.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型  
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(Values, out doubV);
                            celltemp.SetCellValue(doubV);
                            break;
                        default:
                            celltemp.SetCellValue(Values);
                            break;
                    }



                }
                rowIndex++;
            }
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }


        private static MemoryStream DtToMs(DataTable sourceTable, string sheetName, Dictionary<string, int> cloumnWidth, Dictionary<string, Dictionary<string, string>> CloumnValues, bool? IsLine = false, bool? IsFormatter = false, string HeadTitle = "", string BottomTitle = "")
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet1 = hssfworkbook.CreateSheet(sheetName);
            int rowIndex = 0;
            int CloumnLength = sourceTable.Columns.Count;

            if (!string.IsNullOrEmpty(HeadTitle))
            {
                //设置报表的主标题信息
                IRow rowTitle = sheet1.CreateRow(rowIndex);
                rowTitle.HeightInPoints = 45;

                ICell cellTitle = rowTitle.CreateCell(0);
                cellTitle.SetCellValue(HeadTitle);
                ICellStyle styleTitle = hssfworkbook.CreateCellStyle();
                styleTitle.Alignment = HorizontalAlignment.Center;
                styleTitle.VerticalAlignment = VerticalAlignment.Center;//横样式
                styleTitle.WrapText = true;
                IFont font = hssfworkbook.CreateFont();
                font.Boldweight = (short)FontBoldWeight.Bold;//加粗
                font.FontHeightInPoints = 14;
                styleTitle.SetFont(font);
                cellTitle.CellStyle = styleTitle;
                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, CloumnLength - 1));
                rowIndex++;
            }

            //取得列宽
            int[] arrColWidth = new int[CloumnLength];
            foreach (DataColumn item in sourceTable.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }


            #region 头部样式
            ICellStyle HeadStyleCell = hssfworkbook.CreateCellStyle();//创建单元格的样式
            IFont HeadFontCell = hssfworkbook.CreateFont();//创建字体样式
            HeadFontCell.Boldweight = (short)FontBoldWeight.Bold;//加粗
            HeadFontCell.FontHeightInPoints = 12;
            HeadFontCell.FontName = "Arial";
            if (IsLine == true)
            {
                HeadStyleCell.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadStyleCell.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadStyleCell.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadStyleCell.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                //边框颜色  
                HeadStyleCell.BottomBorderColor = HSSFColor.OliveGreen.Black.Index;
                HeadStyleCell.TopBorderColor = HSSFColor.OliveGreen.Black.Index;
            }
            HeadStyleCell.SetFont(HeadFontCell);
            HeadStyleCell.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//横样式
            HeadStyleCell.VerticalAlignment = VerticalAlignment.Center;//垂直样式
            #endregion

            #region 单元格样式
            IDataFormat dataformat = hssfworkbook.CreateDataFormat();
            ICellStyle dateStyle = hssfworkbook.CreateCellStyle();
            dateStyle.DataFormat = dataformat.GetFormat("yyyy-MM-dd HH:mm:ss");

            ICellStyle StyleCell = hssfworkbook.CreateCellStyle();//创建单元格的样式
            IFont FontCell = hssfworkbook.CreateFont();//创建字体样式
            FontCell.FontHeightInPoints = 10;
            FontCell.FontName = "Arial";


            StyleCell.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            StyleCell.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            StyleCell.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            StyleCell.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            //边框颜色  
            StyleCell.BottomBorderColor = HSSFColor.OliveGreen.Black.Index;
            StyleCell.TopBorderColor = HSSFColor.OliveGreen.Black.Index;

            StyleCell.SetFont(FontCell);
            StyleCell.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//横样式
            StyleCell.VerticalAlignment = VerticalAlignment.Center;//垂直样式
            StyleCell.WrapText = true;

            dateStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            dateStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            dateStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            dateStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            //边框颜色  
            dateStyle.BottomBorderColor = HSSFColor.OliveGreen.Black.Index;
            dateStyle.TopBorderColor = HSSFColor.OliveGreen.Black.Index;

            dateStyle.SetFont(FontCell);
            dateStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//横样式
            dateStyle.VerticalAlignment = VerticalAlignment.Center;//垂直样式
            dateStyle.WrapText = true;
            #endregion

            IRow rowhead = sheet1.CreateRow(rowIndex);
            foreach (DataColumn column in sourceTable.Columns)
            {
                ICell celltemp = rowhead.CreateCell(column.Ordinal);
                string ColumnName = column.ColumnName;

                int cWidth = (arrColWidth[column.Ordinal] + 10);
                if (cloumnWidth != null)
                {
                    ColumnName = ColumnName.ToLower();
                    if (cloumnWidth.ContainsKey(ColumnName))
                    {
                        cWidth = cloumnWidth[ColumnName].CInt(cWidth, false);
                    }
                }

                celltemp.SetCellValue(ColumnName);
                celltemp.CellStyle = HeadStyleCell;
                sheet1.SetColumnWidth(column.Ordinal, cWidth * 256);
            }
            rowIndex++;

            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet1.CreateRow(rowIndex);
                foreach (DataColumn column in sourceTable.Columns)
                {
                    string Values = row[column].ToString();
                    string ColumnName = column.ColumnName;
                    if (CloumnValues != null)
                    {
                        ColumnName = ColumnName.ToLower();
                        if (CloumnValues.ContainsKey(ColumnName))
                        {
                            Dictionary<string, string> dict = CloumnValues[ColumnName];
                            if (dict != null && dict.ContainsKey(Values))
                            {
                                Values = dict[Values];
                            }
                        }
                    }

                    ICell celltemp = dataRow.CreateCell(column.Ordinal);
                    celltemp.CellStyle = StyleCell;
                    if (IsFormatter == true)
                    {
                        switch (column.DataType.ToString())
                        {
                            case "System.String"://字符串类型   
                                celltemp.SetCellValue(Values);
                                break;
                            case "System.DateTime"://日期类型   
                                celltemp.CellStyle = dateStyle;
                                DateTime dateV = Values.CDateTime(DateTime.Now, false);
                                celltemp.SetCellValue(dateV);
                                break;
                            case "System.Boolean"://布尔型   
                                bool boolV = false;
                                bool.TryParse(Values, out boolV);
                                celltemp.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型   
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(Values, out intV);
                                celltemp.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型  
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(Values, out doubV);
                                celltemp.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理   
                                celltemp.SetCellValue("");
                                break;
                            default:
                                celltemp.SetCellValue(Values);
                                break;
                        }
                    }
                    else
                        celltemp.SetCellValue(Values);
                }
                rowIndex++;
            }

            if (!string.IsNullOrEmpty(BottomTitle))
            {
                //设置底部合计信息
                IRow rowTitle = sheet1.CreateRow(rowIndex);
                rowTitle.HeightInPoints = 15;

                ICell cellTitle = rowTitle.CreateCell(0);
                cellTitle.SetCellValue(BottomTitle);
                ICellStyle styleTitle = hssfworkbook.CreateCellStyle();
                styleTitle.Alignment = HorizontalAlignment.Left;
                styleTitle.VerticalAlignment = VerticalAlignment.Center;//横样式
                styleTitle.WrapText = true;
                IFont font = hssfworkbook.CreateFont();
                font.Boldweight = (short)FontBoldWeight.Bold;//加粗
                font.FontHeightInPoints = 11;
                styleTitle.SetFont(font);
                cellTitle.CellStyle = styleTitle;
                sheet1.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, CloumnLength - 1));
                rowIndex++;
            }

            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }




        private static DataTable DoTb(DataTable dt, int[] remove, string[] contan, Dictionary<string, string> headname)
        {

            if (remove != null && remove.Length > 0)
            {//处理删除列
                for (int i = remove.Length; i > 0; i--)
                {
                    dt.Columns.RemoveAt(remove[i - 1]);
                }
            }
            if (contan != null && contan.Length > 0)
            {//处理包含列
                int t = 0;
                while (t < dt.Columns.Count)
                {
                    bool flag = false;
                    for (int j = 0; j < contan.Length; j++)
                    {
                        if (String.Compare(dt.Columns[t].ColumnName, contan[j], true) == 0)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        t++;
                    }
                    else
                    {
                        dt.Columns.Remove(dt.Columns[t].ColumnName);
                    }
                }
            }

            if (headname != null)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string headerName = headname[dt.Columns[i].ColumnName];
                    if (!string.IsNullOrEmpty(headerName))
                    {
                        dt.Columns[i].ColumnName = headerName;
                    }
                }
            }

            return dt;
        }


        /// <summary>
        /// 由DataTable导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="fname">文件名称, 不含扩展名</param>
        /// <param name="headname">需要重命名的列</param>
        public static void ExportDataTableToExcel(DataTable sourceTable, string fname, Dictionary<string, string> headname)
        {
            string filename = string.Format("{0}.xls", fname);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BinaryWrite(DtToMs(DoTb(sourceTable, null, null, headname), "sheet1").GetBuffer());
            HttpContext.Current.Response.End();
        }



        /// <summary>
        /// 由DataTable导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="fname">文件名称, 不含扩展名</param>
        /// <param name="header">字段别名</param>
        /// <param name="remove">排除的列(注意从小到大)</param>
        /// <returns>Excel工作表</returns>
        public static void ExportDataTableToExcel(DataTable sourceTable, string fname, Dictionary<string, string> headname, int[] remove)
        {
            string filename = string.Format("{0}.xls", fname);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BinaryWrite(DtToMs(DoTb(sourceTable, remove, null, headname), "sheet1").GetBuffer());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 由DataTable导出Excel
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="headname">字段别名</param>
        /// <param name="contan">存在值时,只导出这里面的字段</param>
        /// <param name="sheetname"></param>
        /// <param name="fname">文件名称, 不含扩展名</param>
        public static void ExportDataTableToExcel(DataTable sourceTable, Dictionary<string, string> headname, string[] contan, string sheetname, string fname)
        {
            string filename = string.Format("{0}.xls", fname);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BinaryWrite(DtToMs(DoTb(sourceTable, null, contan, headname), sheetname).GetBuffer());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 由DataTable导出Excel
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="TableHeadCloumnName">字段别名</param>
        /// <param name="contan">存在值时,只导出这里面的字段</param>
        /// <param name="sheetname"></param>
        /// <param name="fname">文件名称, 不含扩展名</param>
        /// <param name="HeadCloumnWidth">表格字段宽度(以excel 拖拉显示的列宽填入,注意列宽excel 中将*265 最大填入值不能超过255)</param>
        /// <param name="CloumnValues">字段值转换</param>
        /// <param name="IsLine">单元格是否要加实线边框</param>
        /// <param name="IsFormatter">单元格是否要按本身的格式进行格式化（用于命令统计）</param>
        /// <param name="HeadTitle">表格标题</param>
        /// <param name="BottomTitle">表格底部合计信息</param>

        public static void ExportDataTableToExcel(DataTable sourceTable, Dictionary<string, string> TableHeadCloumnName, string[] contan,
            string sheetname, string fname, Dictionary<string, int> HeadCloumnWidth, Dictionary<string, Dictionary<string, string>> CloumnValues,
            bool? IsLine = false, bool? IsFormatter = false, string HeadTitle = "", string BottomTitle = "")
        {
            if (System.Web.HttpContext.Current.Request.Browser.Browser.ToLower().IndexOf("firefox") <= -1)
            {
                fname = HttpUtility.UrlEncode(fname, System.Text.Encoding.UTF8);
            }
            string filename = string.Format("{0}.xls", fname);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BinaryWrite(DtToMs(DoTb(sourceTable, null, contan, TableHeadCloumnName), sheetname,
                HeadCloumnWidth, CloumnValues, IsLine, IsFormatter, HeadTitle, BottomTitle).GetBuffer());
            HttpContext.Current.Response.End();
        }

        #endregion

        #region 读取Excel
        /// <summary>
        /// 读取Excel
        /// </summary>
        /// <param name="stream">字节流</param>
        /// <param name="extension">excel 扩展名</param>
        /// <returns></returns>
        public static IWorkbook ReadExcelToWorkBook(Stream stream, ExcelExtension extension)
        {
            IWorkbook workbook = null;
            if (extension == ExcelExtension.XLS)
            {
                workbook = new HSSFWorkbook(stream);
            }
            else
            {
                workbook = new XSSFWorkbook(stream);
            }
            return workbook;
        }

        /// <summary>
        /// 将Excel 转为DataTable
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="sheetIndex">读取excel 第几个工作表</param>
        /// <returns></returns>
        public static DataTable ReadExcelToDataTable(IWorkbook workBook, int? sheetIndex = 0)
        {
            if (workBook.isDbNullOrNull()) return null;

            ISheet sheet = workBook.GetSheetAt(sheetIndex ?? 0);
            if (sheet.isDbNullOrNull()) return null;

            DataTable dt = new DataTable();
            IRow headerRow = sheet.GetRow(0);

            //一行最后一个方格的编号 即总的列数
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {//头部
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                dt.Columns.Add(column);
            }
            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum;
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (!row.isDbNullOrNull())
                {
                    DataRow dataRow = dt.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (!cell.isDbNullOrNull())
                        {
                            if (cell.CellType == CellType.Formula)
                            {
                                try
                                {
                                    HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                                    e.EvaluateInCell(cell);
                                    dataRow[j] = cell.ToString();
                                }
                                catch (Exception error)
                                {
                                    if (DateUtil.IsCellDateFormatted(cell))//日期
                                    {
                                        dataRow[j] = cell.DateCellValue;
                                    }
                                    else
                                    {
                                        dataRow[j] = cell.NumericCellValue;
                                    }
                                }
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).CString("");
                            }

                        }
                    }
                    dt.Rows.Add(dataRow);
                }
                else
                {
                    break;
                }
            }

            return dt;
        }

        /// <summary>
        /// 将Excel 转为DataTable
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="sheetIndex">读取excel 第几个工作表</param>
        /// <returns></returns>
        public static DataTable ReadExcelToDataTable(Stream stream, ExcelExtension extension, int? sheetIndex = 0)
        {
            IWorkbook workbook = ReadExcelToWorkBook(stream, extension);
            return ReadExcelToDataTable(workbook, sheetIndex);
        }

        #endregion
    }



    /// <summary>
    /// Excel扩展名
    /// </summary>
    public enum ExcelExtension
    {
        /// <summary>
        /// .xls
        /// </summary>

        XLS = 0,
        /// <summary>
        /// .xlsx
        /// </summary>

        XLSX = 1
    }
}