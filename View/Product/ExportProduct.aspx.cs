using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XHD.Common;
using XHD.Controller;
using XHD.Model;

namespace XHD.View.Product
{
    public partial class ExportProduct : System.Web.UI.Page
    {
        public static BLL.Product product = new BLL.Product();

        hr_employee employee = new hr_employee();

        protected void Page_Load(object sender, EventArgs e)
        {
            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(HttpContext.Current);


            if (!IsPostBack)
            {
                bool candel = true;
                if (employee.uid != "admin")
                {
                    //controll auth
                    var getauth = new GetAuthorityByUid();
                    candel = getauth.GetBtnAuthority(employee.id, "0DBE7337-D4D3-4895-8C8F-5E0A4ECEEE0A");
                    if (!candel)
                    {
                        Response.Write("无此权限");
                        Response.End();
                        return;
                    }

                }

                if (candel)
                {
                    Export(Request["ids"]);
                }

            }

        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string Export(string ids)
        {
            ids = PageValidate.InputText(ids, 50000);
            if (string.IsNullOrWhiteSpace(ids)) { return ""; }
            ids = "'" + ids.Trim(',') + "'";
            ids = ids.Replace(",", "','");

            //string where = $" id in({ids})";
            //DataSet ds = product.GetList(where);
            //DataToExcel excel = new DataToExcel();
            //string absoluFilePath = AppDomain.CurrentDomain.BaseDirectory + "File\\ExcelReport\\Product\\";
            //if (!Directory.Exists(absoluFilePath))
            //{
            //    Directory.CreateDirectory(absoluFilePath);
            //}


            //Hashtable nameList = new Hashtable();
            //nameList.Add("BarCode", "条形码");
            //nameList.Add("product_name", "产品名称");
            //nameList.Add("category_name", "产品分类");
            //nameList.Add("Weight", "重量(克)");
            //nameList.Add("GoldTotal", "金价小计(￥)");
            //nameList.Add("CostsTotal", "工费小计(￥)");
            //nameList.Add("Totals", "成本总价(￥)");
            //nameList.Add("SalesCostsTotal", "销售工费(￥)");
            //nameList.Add("SalesTotalPrice", "销售价格(￥)");
            ////利用excel对象
            //DataToExcel dte = new DataToExcel();
            //string filename = "";
            //try
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        filename = dte.OutputExcel(ds.Tables[0], "产品导出_" + DateTime.Now.ToString("yyyyMMddHHmmss"), absoluFilePath);
            //    }
            //}
            //catch (Exception error)
            //{
            //    dte.KillExcelProcess();
            //    SoftLog.LogStr(ids + "__error:" + error.ToString(), "ExportProduct");
            //}

            //if (filename != "")
            //{
            //    HttpContext.Current.Response.Clear();
            //    HttpContext.Current.Response.Buffer = true;
            //    HttpContext.Current.Response.Charset = "utf-8";
            //    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename="+ filename);
            //    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");

            //    HttpContext.Current.Response.ContentType = "application/octet-stream";
            //    HttpContext.Current.Response.WriteFile(absoluFilePath + "\\" + filename);

            //}
            return "";
        }
    }
}