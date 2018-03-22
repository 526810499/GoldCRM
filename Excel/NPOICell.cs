using System;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;


namespace XHD.Excel
{
    //Aspose帮助类，对常用Aspose用法进行封装
    public class NPOICell
    {
        private HSSFWorkbook _workbook = null;
        private Dictionary<string, Sheet> _worksheets = null;
        private Sheet _currentWorksheet = null;


        public NPOICell(string fullName)
        {
            FileStream fs = new FileStream(fullName, FileMode.Open, FileAccess.Read);
            _workbook = new HSSFWorkbook(fs);
            _worksheets = new Dictionary<string, Sheet>();
            int x = _workbook.Workbook.NumSheets;

            for (int i = 0; i < x; i++)
            {
                _worksheets.Add(_workbook.Workbook.GetSheetName(i), _workbook.GetSheetAt(i));
            }
            _currentWorksheet = _workbook.GetSheetAt(0);
        }

        //设置指定名称的sheet为当前操作sheet
        public void SetCurrentWorksheet(string worksheetName)
        {
            if (_worksheets.ContainsKey(worksheetName))
            {
                _currentWorksheet = _worksheets[worksheetName];
            }
            else
            {
                throw new Exception("当前工作薄不存在\"" + worksheetName + "\"工作表！");
            }
        }

        //设置指定索引（从0开始）的sheet为当前操作sheet
        public void SetCurrentWorksheet(byte worksheetIndex)
        {
            if (worksheetIndex <= _worksheets.Count)
            {
                _currentWorksheet = GetWorkSheetByIndex(worksheetIndex);
            }
            else
            {
                throw new Exception("工作表索引范围超过了总工作表数量！");
            }
        }

        //根据索引得到sheet
        public Sheet GetWorkSheetByIndex(byte index)
        {
            byte i = 0;
            Sheet worksheet = null;
            foreach (string name in _worksheets.Keys)
            {
                if (index == i)
                {
                    worksheet = _worksheets[name];
                }
                i++;
            }
            return worksheet;
        }

        //根据sheet名称得到sheet
        public Sheet GetWorkSheetByName(string sheetName)
        {
            if (_worksheets.ContainsKey(sheetName))
            {
                return _worksheets[sheetName];
            }
            else
            {
                return null;
            }

        }

        public int GetRowCount()
        {
            int rowCount = _currentWorksheet.LastRowNum;
            return rowCount + 1;
        }

        public int GetColumnCount()
        {
            Row headerRow = _currentWorksheet.GetRow(0);

            //最后一个方格的编号 即总的列数
            int cellCount = headerRow.LastCellNum;

            return cellCount + 1;
        }

        /// <summary>
        /// 判断指定行是否有数据
        /// </summary>
        /// <param name="row">从1开始，为Excel行序号</param>
        /// <returns></returns>
        public bool RowHasValue(int row)//指定行是否有数据（以连续50列没有数据为标准）
        {
            bool r = false;
            for (int i = 0; i < 50; ++i)
            {
                Cell cell = _currentWorksheet.GetRow(row).GetCell(i);
                if (cell != null && cell.ToString() != "")
                {
                    r = true;
                    break;
                }
            }
            return r;
        }

        /// <summary>
        /// 根据行列索引得到指定位置的数据
        /// </summary>
        /// <param name="row">从0开始</param>
        /// <param name="column">从0开始</param>
        /// <returns></returns>
        public string GetCellValue(int row, int column)
        {
            if (column == -1)
                return "";

            Cell cell = _currentWorksheet.GetRow(row).GetCell(column);
            if (cell != null)
            {
                return cell.ToString();
            }
            return "";
        }

        /// <summary>
        /// 判断某位置的单元格是否为合并单元格
        /// </summary>
        /// <param name="row">从0开始</param>
        /// <param name="column">从0开始</param>
        /// <returns></returns>
        public bool IsMerged(int row, int column)
        {
            if (column == -1)
                return false;

            Cell cell = _currentWorksheet.GetRow(row).GetCell(column);

            if (cell != null)
                return cell.IsMergedCell;
            return false;
        }

        //取得合并单元格的数据
        public string GetMergedCellValue(int row, int column)//事实上合并单元格只有第一个单元格有值其他的全为空，但是我们知道其实从意义上理解合并单元格除了第一个单元格外其他单元格的值同第一个，因此这里提供这样一个方法
        {
            string r = "";
            int t = row ;
            if (IsMerged(row, column))
            {
                if (GetCellValue(row, column) != "")
                {
                    r = GetCellValue(row, column);
                }
                else//约定合并单元格只是合并行并不和并列，并且合并行数最多50
                {
                    while (t >= 0 && (row - t) < 50)
                    {
                        if (GetCellValue(t, column) != "")
                        {
                            r = GetCellValue(t, column);
                            break;
                        }
                        t--;
                    }
                }
            }
            return r;
        }
    }
}
