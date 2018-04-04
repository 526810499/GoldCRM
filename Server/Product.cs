using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Product : BaseCRMServer
    {
        public static BLL.Product product = new BLL.Product();
        public static Model.Product model = new Model.Product();


        public Product()
        {
        }

        public Product(HttpContext context) : base(context)
        {
            allDataBtnid = "1992FBF3-206A-4DE5-A1C4-1EB3A4F597D8";
            depDataBtnid = "ADE0DF75-65F1-4D8A-93C9-597D60140046";
        }

        public string save()
        {
            model.category_id = PageValidate.InputText(request["T_product_category_val"], 50);
            model.product_name = PageValidate.InputText(request["T_product_name"], 255);
            model.StockPrice = request["T_StockPrice"].CDecimal(0, false);
            model.Weight = request["T_Weight"].CDecimal(0, false);
            model.AttCosts = request["T_AttCosts"].CDecimal(0, false);
            model.MainStoneWeight = request["T_MainStoneWeight"].CDecimal(0, false);
            model.AttStoneWeight = request["T_AttStoneWeight"].CDecimal(0, false);
            model.AttStoneNumber = request["T_AttStoneNumber"].CDecimal(0, false);
            model.StonePrice = request["T_StonePrice"].CDecimal(0, false);
            model.GoldTotal = request["T_GoldTotal"].CDecimal(0, false);
            model.CostsTotal = request["T_CostsTotal"].CDecimal(0, false);
            model.Totals = request["T_Totals"].CDecimal(0, false);

            model.SalesTotalPrice = request["T_SalesTotalPrice"].CDecimal(0, false);
            model.SalesCostsTotal = request["T_SalesCostsTotal"].CDecimal(0, false);
            model.SupplierID = request["T_SupplierID_val"].CInt(0, false);
            model.Sbarcode = PageValidate.InputText(request["T_Sbarcode"], 255);
            model.remarks = PageValidate.InputText(request["T_Remark"], 255);
            model.IsGold = (request["T_GType"].CString("") == "是" ? 1 : 0);


            string pid = PageValidate.InputText(request["pid"], 50);
            if (PageValidate.checkID(pid))
            {
                bool candel = true;
                if (uid != "admin")
                {
                    var getauth = new GetAuthorityByUid();
                    candel = getauth.GetBtnAuthority(emp_id.ToString(), "4AF81556-E83B-4305-A1F2-6863A59C29F0");
                    if (!candel)
                        return XhdResult.Error("无此权限！").ToString();
                }

                model.id = pid;
                DataSet ds = product.GetList($" id= '{pid}' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];
                product.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.product_name;
                string EventType = "产品修改";
                string EventID = model.id;

                string Log_Content = null;

                if (dr["category_name"].ToString() != request["T_product_category"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "产品类别", dr["category_name"],
                        request["T_product_category"]);

                if (dr["product_name"].ToString() != request["T_product_name"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "产品名字", dr["product_name"],
                        request["T_product_name"]);

                if (dr["StockPrice"].ToString() != request["T_StockPrice"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "进货金价", dr["StockPrice"],
                        request["T_StockPrice"]);

                if (dr["Weight"].ToString() != request["T_Weight"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "重量", dr["unit"], request["T_Weight"]);

                if (dr["remarks"].ToString() != request["T_Remark"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "备注", dr["remarks"], request["T_Remark"]);

                if (decimal.Parse(dr["AttCosts"].ToString()) != decimal.Parse(request["T_AttCosts"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "附工费", dr["AttCosts"], request["T_AttCosts"]);

                if (decimal.Parse(dr["MainStoneWeight"].ToString()) != decimal.Parse(request["T_MainStoneWeight"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "MainStoneWeight", dr["MainStoneWeight"], request["T_MainStoneWeight"]);

                if (decimal.Parse(dr["AttStoneWeight"].ToString()) != decimal.Parse(request["T_AttStoneWeight"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "AttStoneWeight", dr["AttStoneWeight"], request["T_AttStoneWeight"]);

                if (decimal.Parse(dr["AttStoneNumber"].ToString()) != decimal.Parse(request["T_AttStoneNumber"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "AttStoneNumber", dr["AttStoneNumber"], request["T_AttStoneNumber"]);

                if (decimal.Parse(dr["StonePrice"].ToString()) != decimal.Parse(request["T_StonePrice"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "StonePrice", dr["StonePrice"], request["T_StonePrice"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                bool candel = true;
                if (uid != "admin")
                {
                    var getauth = new GetAuthorityByUid();
                    candel = getauth.GetBtnAuthority(emp_id.ToString(), "3FD3566B-B807-452A-B3B3-00A709CCAF229");
                    if (!candel)
                        return XhdResult.Error("无此权限！").ToString();
                }


                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                model.id = Guid.NewGuid().ToString();
                model.createdep_id = dep_id;
                model.status = 1;
                string code = StringPlus.GetRandomLetters() + DateTime.Now.GetHashCode().ToString().Replace("-", "");
                string T_product_category = request["T_product_category"].CString("HJ");
                string str = T_product_category.GetSpellCode(true);
                code = str.PadRight(2, '0').Substring(0, 2) + code;

                model.BarCode = code.ToUpper();
                product.Add(model);
            }

            return XhdResult.Success().ToString();
        }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " create_time";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";
            if (!string.IsNullOrEmpty(request["categoryid"]))
                serchtxt += $" and category_id='{PageValidate.InputText(request["categoryid"], 50)}'";

            if (!string.IsNullOrEmpty(request["stext"]))
                serchtxt += $" and product_name like N'%{ PageValidate.InputText(request["stext"], 255) }%'";

            if (!string.IsNullOrEmpty(request["scode"]))
                serchtxt += $" and BarCode='{ PageValidate.InputText(request["scode"], 255) }'";
            if (!string.IsNullOrEmpty(request["status"]))
                serchtxt += $" and status={request["status"].CInt(0, false)}";
            if (!string.IsNullOrEmpty(request["status"]) && request["status"].CString("") != "null")
                serchtxt += $" and status={request["status"].CInt(0, false)}";
            if (!string.IsNullOrEmpty(request["SupplierID"]) && request["SupplierID"].CString("") != "null")
                serchtxt += $" and SupplierID='{request["SupplierID"].CString("")}'";
            //权限
            serchtxt = GetSQLCreateIDWhere(serchtxt, true);
            DataSet ds = product.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return (dt);
        }

        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);
            DataSet ds = product.GetList($" id= '{id}' ");
            return DataToJson.DataToJSON(ds);

        }


        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();
            id = PageValidate.InputText(id, 50);
            DataSet ds = product.GetList($" id= '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            var ccod = new BLL.Sale_order_details();
            if (ccod.GetList($"product_id = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此产品下含有订单，不允许删除！").ToString();
            int status = ds.Tables[0].Rows[0]["status"].CInt(1, false);

            if (status != 1)
            {
                return XhdResult.Error("此产品下含有调拨单，不允许删除！").ToString();
            }

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "C55A626D-1A32-4EDF-832A-19DA1C2A4569");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = product.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();

            //日志
            string EventType = "产品删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["product_name"].ToString();



            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();

        }
    }
}
