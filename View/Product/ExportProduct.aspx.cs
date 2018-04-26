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

            int exportType = 0;
            if (!IsPostBack)
            {

                //导出数据类型 0 条形码导出  1销售利润表导出 
                exportType = Request["etype"].CInt(0, false);

                switch (exportType)
                {
                    case 0:
                        Export();
                        break;
                    case 1:
                        ExportOrderLiR();
                        break;
       
                }


            }

        }

        private bool CheckBtnAuthority(string btnID)
        {
            if (employee.uid == "admin") { return true; }
            //controll auth
            var getauth = new GetAuthorityByUid();
            bool candel = getauth.GetBtnAuthority(employee.id, btnID);

            return candel;
        }

        #region 导出商品条形码
        /// <summary>
        /// 导出商品条形码
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string Export()
        {

            bool candel = CheckBtnAuthority("0DBE7337-D4D3-4895-8C8F-5E0A4ECEEE0A");

            if (!candel)
            {
                ExportError("无此权限");
                return "";
            }


            string ids = Request["ids"];
            ids = PageValidate.InputText(ids, 50000);
            if (string.IsNullOrWhiteSpace(ids)) { return ""; }
            ids = "'" + ids.Trim(',') + "'";
            ids = ids.Replace(",", "','");

            string where = $" id in({ids})";
            DataSet ds = product.GetList(where);
            if (ds != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                string fname = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

                Dictionary<string, string> nameList = new Dictionary<string, string>();
                nameList.Add("BarCode", "条形码");
                nameList.Add("product_name", "商品名称");
                nameList.Add("category_name", "商品分类");
                nameList.Add("Weight", "重量(克)");
                nameList.Add("GoldTotal", "金价小计(￥)");
                nameList.Add("CostsTotal", "工费小计(￥)");
                nameList.Add("Totals", "成本总价(￥)");
                nameList.Add("SalesCostsTotal", "销售工费(￥)");
                nameList.Add("SalesTotalPrice", "销售价格(￥)");
                nameList.Add("remark", "备注");

                ExportHelper.ExportDataTableToExcel(ds.Tables[0], nameList, nameList.Keys.ToArray(), DateTime.Now.ToString("MMddHHmmss"), fname);
            }
            else {
                ExportError("无数据导出");
            }

            return "";
        }

        #endregion
        private void ExportError(string msg)
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=导出结果失败_错误提示信息.txt");
            Response.ContentType = "text/plain";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(msg);
            Response.End();

        }
        #region 导出销售利润报表



        /// <summary>
        /// 销售利润表
        /// </summary>
        /// <returns></returns>
        private string ExportOrderLiR()
        {
            string rid = "003E7F31-1407-4828-A3E9-32DFA10A5BA8";
            int userData = Request["user"].CInt(0, false);
            if (userData == 1)
            {
                rid = "5181CD41-1933-440D-ADAE-82D44E859D11";
            }
            bool candel = CheckBtnAuthority(rid);

            if (!candel)
            {
                ExportError("无此权限");
                return "";
            }

            Server.Sale_order sbll = new XHD.Server.Sale_order(HttpContext.Current);
            DataTable table = sbll.ExportData();
            if (table != null && table.Rows.Count > 0)
            {
                bool right = true;
                //判断是否能读取成本信息
                if (employee.uid != "admin")
                {
                    right = new BLL.Sys_role_emp().CheckUserRoleRight(employee.id, "9A513452-7CD6-4A80-AC47-53493DF86DB9");
                }
                string fname = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Dictionary<string, int> hcWidth = new Dictionary<string, int>();
                Dictionary<string, string> nameList = new Dictionary<string, string>();
                nameList.Add("rows", "序号");
                nameList.Add("Serialnumber", "订单号");
                nameList.Add("product_name", "商品名称");
                nameList.Add("category_name", "商品分类");
                nameList.Add("BarCode", "条形码");
                nameList.Add("Weight", "重量(克)");
                if (right && userData == 0)
                {
                    nameList.Add("StockPrice", "进货价(￥)");
                    nameList.Add("Totals", "成本总价(￥)");
                    nameList.Add("profits", "利润");
                }
                nameList.Add("CostsTotal", "工费小计(￥)");
                nameList.Add("amount", "销售总价(￥)");
                nameList.Add("dep_name", "成交部门");
                nameList.Add("emp_name", "成交人");
                nameList.Add("Order_date", "成交时间");
                nameList.Add("cus_name", "客户");
                nameList.Add("cus_tel", "客户电话");
                nameList.Add("Remark", "备注");
                hcWidth.Add("条形码", 40);
                hcWidth.Add("订单号", 40);

                ExportHelper.ExportDataTableToExcel(table, nameList, nameList.Keys.ToArray(), "sheet1", fname, hcWidth, null, true, true);
            }
            else {
                ExportError("无数据导出");
            }

            return "";
        }

        #endregion


        #region 打印商品条形码
        /// <summary>
        /// 导出商品条形码
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string ExportPrint()
        {

            bool candel = CheckBtnAuthority("0DBE7337-D4D3-4895-8C8F-5E0A4ECEEE0A");

            if (!candel)
            {
                return null;
            }


            string ids = Request["ids"];
            ids = PageValidate.InputText(ids, 50000);
            if (string.IsNullOrWhiteSpace(ids)) { return null; }
            ids = "'" + ids.Trim(',') + "'";
            ids = ids.Replace(",", "','");

            string where = $" id in({ids})";
            DataSet ds = product.GetList(where);
            if (ds != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                return JsonDyamicHelper.NetJsonConvertObject(ds.Tables[0]);
            }
            else {
                return null;
            }
        }

        #endregion
    }
}