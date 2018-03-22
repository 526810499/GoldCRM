using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using COM.Excel;
using Excel;
using DataTable = System.Data.DataTable;

namespace XHD.Common
{
    /// <summary>
    ///     操作EXCEL导出数据报表的类
    ///     Copyright (C) XHD
    /// </summary>
    public class DataToExcel
    {
        #region 操作EXCEL的一个类(需要Excel.dll支持)

        private DateTime afterTime; //Excel启动之后时间
        private DateTime beforeTime; //Excel启动之前时间
        private int titleColorindex = 15;

        #region 创建一个Excel示例

        /// <summary>
        ///     创建一个Excel示例
        /// </summary>
        public void CreateExcel()
        {
            var excel = new Application();
            excel.Application.Workbooks.Add(true);
            excel.Cells[1, 1] = "第1行第1列";
            excel.Cells[1, 2] = "第1行第2列";
            excel.Cells[2, 1] = "第2行第1列";
            excel.Cells[2, 2] = "第2行第2列";
            excel.Cells[3, 1] = "第3行第1列";
            excel.Cells[3, 2] = "第3行第2列";

            //保存
            excel.ActiveWorkbook.SaveAs("./tt.xls", XlFileFormat.xlExcel9795, null, null, false, false,
                XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);
            //打开显示
            excel.Visible = true;
            //			excel.Quit();
            //			excel=null;            
            //			GC.Collect();//垃圾回收
        }

        #endregion

        #region 将DataTable的数据导出显示为报表

        /// <summary>
        ///     将DataTable的数据导出显示为报表
        /// </summary>
        /// <param name="dt">要导出的数据</param>
        /// <param name="strTitle">导出报表的标题</param>
        /// <param name="FilePath">保存文件的路径</param>
        /// <returns></returns>
        public string OutputExcel(DataTable dt, string strTitle, string FilePath)
        {
            beforeTime = DateTime.Now;

            Application excel;
            _Workbook xBk;
            _Worksheet xSt;

            int rowIndex = 4;
            int colIndex = 1;

            excel = new ApplicationClass();
            xBk = excel.Workbooks.Add(true);
            xSt = (_Worksheet) xBk.ActiveSheet;

            //取得列标题			
            foreach (DataColumn col in dt.Columns)
            {
                colIndex++;
                excel.Cells[4, colIndex] = col.ColumnName;

                //设置标题格式为居中对齐
                xSt.get_Range(excel.Cells[4, colIndex], excel.Cells[4, colIndex]).Font.Bold = true;
                xSt.get_Range(excel.Cells[4, colIndex], excel.Cells[4, colIndex]).HorizontalAlignment =
                    XlVAlign.xlVAlignCenter;
                xSt.get_Range(excel.Cells[4, colIndex], excel.Cells[4, colIndex]).Select();
                xSt.get_Range(excel.Cells[4, colIndex], excel.Cells[4, colIndex]).Interior.ColorIndex = titleColorindex;
                    //19;//设置为浅黄色，共计有56种
            }


            //取得表格中的数据			
            foreach (DataRow row in dt.Rows)
            {
                rowIndex++;
                colIndex = 1;
                foreach (DataColumn col in dt.Columns)
                {
                    colIndex++;
                    if (col.DataType == Type.GetType("System.DateTime"))
                    {
                        excel.Cells[rowIndex, colIndex] =
                            (Convert.ToDateTime(row[col.ColumnName].ToString())).ToString("yyyy-MM-dd");
                        xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex])
                            .HorizontalAlignment = XlVAlign.xlVAlignCenter; //设置日期型的字段格式为居中对齐
                    }
                    else if (col.DataType == Type.GetType("System.String"))
                    {
                        excel.Cells[rowIndex, colIndex] = "'" + row[col.ColumnName];
                        xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex])
                            .HorizontalAlignment = XlVAlign.xlVAlignCenter; //设置字符型的字段格式为居中对齐
                    }
                    else
                    {
                        excel.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString();
                    }
                }
            }

            //加载一个合计行			
            int rowSum = rowIndex + 1;
            int colSum = 2;
            excel.Cells[rowSum, 2] = "合计";
            xSt.get_Range(excel.Cells[rowSum, 2], excel.Cells[rowSum, 2]).HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //设置选中的部分的颜色			
            xSt.get_Range(excel.Cells[rowSum, colSum], excel.Cells[rowSum, colIndex]).Select();
            //xSt.get_Range(excel.Cells[rowSum,colSum],excel.Cells[rowSum,colIndex]).Interior.ColorIndex =Assistant.GetConfigInt("ColorIndex");// 1;//设置为浅黄色，共计有56种

            //取得整个报表的标题			
            excel.Cells[2, 2] = strTitle;

            //设置整个报表的标题格式			
            xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, 2]).Font.Bold = true;
            xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, 2]).Font.Size = 22;

            //设置报表表格为最适应宽度			
            xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, colIndex]).Select();
            xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, colIndex]).Columns.AutoFit();

            //设置整个报表的标题为跨列居中			
            xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, colIndex]).Select();
            xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, colIndex]).HorizontalAlignment =
                XlHAlign.xlHAlignCenterAcrossSelection;

            //绘制边框			
            xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, colIndex]).Borders.LineStyle = 1;
            xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, 2]).Borders[XlBordersIndex.xlEdgeLeft].Weight =
                XlBorderWeight.xlThick; //设置左边线加粗
            xSt.get_Range(excel.Cells[4, 2], excel.Cells[4, colIndex]).Borders[XlBordersIndex.xlEdgeTop].Weight =
                XlBorderWeight.xlThick; //设置上边线加粗
            xSt.get_Range(excel.Cells[4, colIndex], excel.Cells[rowSum, colIndex]).Borders[XlBordersIndex.xlEdgeRight]
                .Weight = XlBorderWeight.xlThick; //设置右边线加粗
            xSt.get_Range(excel.Cells[rowSum, 2], excel.Cells[rowSum, colIndex]).Borders[XlBordersIndex.xlEdgeBottom]
                .Weight = XlBorderWeight.xlThick; //设置下边线加粗


            afterTime = DateTime.Now;

            //显示效果			
            //excel.Visible=true;			
            //excel.Sheets[0] = "sss";

            ClearFile(FilePath);
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssff") + ".xls";
            excel.ActiveWorkbook.SaveAs(FilePath + filename, XlFileFormat.xlExcel9795, null, null, false, false,
                XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);

            //wkbNew.SaveAs strBookName;
            //excel.Save(strExcelFileName);

            #region  结束Excel进程

            //需要对Excel的DCOM对象进行配置:dcomcnfg


            //excel.Quit();
            //excel=null;            

            xBk.Close(null, null, null);
            excel.Workbooks.Close();
            excel.Quit();


            //注意：这里用到的所有Excel对象都要执行这个操作，否则结束不了Excel进程
            //			if(rng != null)
            //			{
            //				System.Runtime.InteropServices.Marshal.ReleaseComObject(rng);
            //				rng = null;
            //			}
            //			if(tb != null)
            //			{
            //				System.Runtime.InteropServices.Marshal.ReleaseComObject(tb);
            //				tb = null;
            //			}
            if (xSt != null)
            {
                Marshal.ReleaseComObject(xSt);
                xSt = null;
            }
            if (xBk != null)
            {
                Marshal.ReleaseComObject(xBk);
                xBk = null;
            }
            if (excel != null)
            {
                Marshal.ReleaseComObject(excel);
                excel = null;
            }
            GC.Collect(); //垃圾回收

            #endregion

            return filename;
        }

        #endregion

        #region Kill Excel进程

        /// <summary>
        ///     结束Excel进程
        /// </summary>
        public void KillExcelProcess()
        {
            Process[] myProcesses;
            DateTime startTime;
            myProcesses = Process.GetProcessesByName("Excel");

            //得不到Excel进程ID，暂时只能判断进程启动时间
            foreach (Process myProcess in myProcesses)
            {
                startTime = myProcess.StartTime;
                if (startTime > beforeTime && startTime < afterTime)
                {
                    myProcess.Kill();
                }
            }
        }

        #endregion

        /// <summary>
        ///     标题背景色
        /// </summary>
        public int TitleColorIndex
        {
            set { titleColorindex = value; }
            get { return titleColorindex; }
        }

        #endregion

        #region 将DataTable的数据导出显示为报表(不使用Excel对象，使用COM.Excel)

        #region 使用示例

        /*使用示例：
		 * DataSet ds=(DataSet)Session["AdBrowseHitDayList"];
			string ExcelFolder=Assistant.GetConfigString("ExcelFolder");
			string FilePath=Server.MapPath(".")+"\\"+ExcelFolder+"\\";
			
			//生成列的中文对应表
			Hashtable nameList = new Hashtable();
			nameList.Add("ADID", "广告编码");
			nameList.Add("ADName", "广告名称");
			nameList.Add("year", "年");
			nameList.Add("month", "月");
			nameList.Add("browsum", "显示数");
			nameList.Add("hitsum", "点击数");
			nameList.Add("BrowsinglIP", "独立IP显示");
			nameList.Add("HitsinglIP", "独立IP点击");
			//利用excel对象
			DataToExcel dte=new DataToExcel();
			string filename="";
			try
			{			
				if(ds.Tables[0].Rows.Count>0)
				{
					filename=dte.DataExcel(ds.Tables[0],"标题",FilePath,nameList);
				}
			}
			catch
			{
				//dte.KillExcelProcess();
			}
			
			if(filename!="")
			{
				Response.Redirect(ExcelFolder+"\\"+filename,true);
			}
		 * 
		 * */

        #endregion

        /// <summary>
        ///     将DataTable的数据导出显示为报表(不使用Excel对象)
        /// </summary>
        /// <param name="dt">数据DataTable</param>
        /// <param name="strTitle">标题</param>
        /// <param name="FilePath">生成文件的路径</param>
        /// <param name="nameList"></param>
        /// <returns></returns>
        public string DataExcel(DataTable dt, string strTitle, string FilePath, Hashtable nameList)
        {
            var excel = new cExcelFile();
            ClearFile(FilePath);
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssff") + ".xls";
            excel.CreateFile(FilePath + filename);
            excel.PrintGridLines = false;

            var mt1 = cExcelFile.MarginTypes.xlsTopMargin;
            var mt2 = cExcelFile.MarginTypes.xlsLeftMargin;
            var mt3 = cExcelFile.MarginTypes.xlsRightMargin;
            var mt4 = cExcelFile.MarginTypes.xlsBottomMargin;

            double height = 1.5;
            excel.SetMargin(ref mt1, ref height);
            excel.SetMargin(ref mt2, ref height);
            excel.SetMargin(ref mt3, ref height);
            excel.SetMargin(ref mt4, ref height);

            var ff = cExcelFile.FontFormatting.xlsNoFormat;
            string font = "宋体";
            short fontsize = 9;
            excel.SetFont(ref font, ref fontsize, ref ff);

            byte b1 = 1,
                b2 = 12;
            short s3 = 12;
            excel.SetColumnWidth(ref b1, ref b2, ref s3);

            string header = "页眉";
            string footer = "页脚";
            excel.SetHeader(ref header);
            excel.SetFooter(ref footer);


            var vt = cExcelFile.ValueTypes.xlsText;
            var cf = cExcelFile.CellFont.xlsFont0;
            var ca = cExcelFile.CellAlignment.xlsCentreAlign;
            var chl = cExcelFile.CellHiddenLocked.xlsNormal;

            // 报表标题
            int cellformat = 1;
            //			int rowindex = 1,colindex = 3;					
            //			object title = (object)strTitle;
            //			excel.WriteValue(ref vt, ref cf, ref ca, ref chl,ref rowindex,ref colindex,ref title,ref cellformat);

            int rowIndex = 1; //起始行
            int colIndex = 0;


            //取得列标题				
            foreach (DataColumn colhead in dt.Columns)
            {
                colIndex++;
                string name = colhead.ColumnName.Trim();
                object namestr = name;
                IDictionaryEnumerator Enum = nameList.GetEnumerator();
                while (Enum.MoveNext())
                {
                    if (Enum.Key.ToString().Trim() == name)
                    {
                        namestr = Enum.Value;
                    }
                }
                excel.WriteValue(ref vt, ref cf, ref ca, ref chl, ref rowIndex, ref colIndex, ref namestr,
                    ref cellformat);
            }

            //取得表格中的数据			
            foreach (DataRow row in dt.Rows)
            {
                rowIndex++;
                colIndex = 0;
                foreach (DataColumn col in dt.Columns)
                {
                    colIndex++;
                    if (col.DataType == Type.GetType("System.DateTime"))
                    {
                        object str = (Convert.ToDateTime(row[col.ColumnName].ToString())).ToString("yyyy-MM-dd");
                        ;
                        excel.WriteValue(ref vt, ref cf, ref ca, ref chl, ref rowIndex, ref colIndex, ref str,
                            ref cellformat);
                    }
                    else
                    {
                        object str = row[col.ColumnName].ToString();
                        excel.WriteValue(ref vt, ref cf, ref ca, ref chl, ref rowIndex, ref colIndex, ref str,
                            ref cellformat);
                    }
                }
            }
            int ret = excel.CloseFile();

            //			if(ret!=0)
            //			{
            //				//MessageBox.Show(this,"Error!");
            //			}
            //			else
            //			{
            //				//MessageBox.Show(this,"请打开文件c:\\test.xls!");
            //			}
            return filename;
        }

        #endregion

        #region  清理过时的Excel文件

        private void ClearFile(string FilePath)
        {
            String[] Files = Directory.GetFiles(FilePath);
            if (Files.Length > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        File.Delete(Files[i]);
                    }
                    catch
                    {
                    }
                }
            }
        }

        #endregion
    }
}