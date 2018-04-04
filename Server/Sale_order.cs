
using System;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Sale_order : BaseCRMServer
    {
        public static BLL.Sale_order order = new BLL.Sale_order();
        public static Model.Sale_order model = new Model.Sale_order();
        public static BLL.CRM_Customer customerBll = new BLL.CRM_Customer();

        public Sale_order()
        {
        }

        public Sale_order(HttpContext context) : base(context)
        {

            allDataBtnid = "4C2A57BB-94A5-401A-82AA-24DE1F5DE4DB";
            depDataBtnid = "1F5A29CE-CE24-4A96-9B98-D72D5AF9B924";
        }

        public string save()
        {
            //是否可以家积分 0 不可以 1可以 -1 扣积分
            int canAddIntegal = 0;
            model.Customer_id = PageValidate.InputText(request["T_Customer_val"], 50);
            if (PageValidate.checkID(request["customer_id"]))
                model.Customer_id = PageValidate.InputText(request["customer_id"], 50);

            if (PageValidate.IsDateTime(request["T_date"]))
                model.Order_date = DateTime.Parse(request["T_date"]);

            model.pay_type_id = PageValidate.InputText(request["T_paytype_val"], 50);
            model.Order_details = PageValidate.InputText(request["T_details"], int.MaxValue);
            model.Order_status_id = PageValidate.InputText(request["T_status_val"], 50);

            model.Order_amount = decimal.Parse(request["T_amount"]);
            model.discount_amount = decimal.Parse(request["T_discount"]);
            model.total_amount = decimal.Parse(request["T_total"]);
            model.receive_money = decimal.Parse(request["T_receive"]);
            model.arrears_money = decimal.Parse(request["T_arrears"]);
            model.emp_id = PageValidate.InputText(request["T_emp_val"], 50);
            model.cashier_id = PageValidate.InputText(request["T_cashier_val"], 50);
            model.vipcard = PageValidate.InputText(request["T_vipcard"], 50);
            model.createdep_id = PageValidate.InputText(request["T_dept_id_val"], 50);
            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                model.id = id;
                DataSet ds = order.GetList($"Sale_order.id = '{id}' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                order.Update(model);
                //context.Response.Write(model.id );

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = dr["Serialnumber"].ToString();
                string EventType = "订单修改";
                string EventID = model.id;
                string Log_Content = null;

                Syslog.get_log_content(dr["pay_type_id"].ToString(), request["T_paytype_val"], "支付方式", dr["pay_type"].ToString(), request["T_paytype"]);
                Syslog.get_log_content(dr["Order_status_id"].ToString(), request["T_status_val"], "订单状态", dr["Order_status"].ToString(), request["T_status"]);
                Syslog.get_log_content(dr["Order_details"].ToString(), request["T_details"], "订单详情", dr["Order_details"].ToString(), request["T_details"]);
                Syslog.get_log_content(dr["emp_id"].ToString(), request["T_emp_val"], "成交人员", dr["emp_name"].ToString(), request["T_emp"]);
                Log_Content += Syslog.get_log_content(
                    DateTime.Parse(dr["Order_date"].ToString()).ToShortDateString(),
                    DateTime.Parse(request["T_date"]).ToShortDateString(),
                    "订单日期",
                    dr["Order_date"].ToString(),
                    request["T_date"]
                    );

                Log_Content += Syslog.get_log_content(
                    decimal.Parse(dr["Order_amount"].ToString()).ToString(),
                    decimal.Parse(request["T_amount"]).ToString(),
                    "订单金额",
                    dr["Order_amount"].ToString(),
                    request["T_amount"]
                    );
                Log_Content += Syslog.get_log_content(
                    decimal.Parse(dr["discount_amount"].ToString()).ToString(),
                    decimal.Parse(request["T_discount"]).ToString(),
                    "优惠金额",
                    dr["discount_amount"].ToString(),
                    request["T_discount"]
                    );
                Log_Content += Syslog.get_log_content(
                    decimal.Parse(dr["total_amount"].ToString()).ToString(),
                    decimal.Parse(request["T_total"]).ToString(),
                    "金额总计",
                    dr["total_amount"].ToString(),
                    request["T_total"]
                    );
                Log_Content += Syslog.get_log_content(
                 dr["receive_money"].CString(""),
                  request["T_receive"].CString(""),
                  "应收金额",
                 dr["receive_money"].CString(""),
                  request["T_receive"].CString("")
                  );

                Log_Content += Syslog.get_log_content(
               dr["arrears_money"].CString(""), request["T_arrears"].CString(""), "未收金额", dr["arrears_money"].CString(""), request["T_arrears"].CString(""));

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);

                if (model.Order_status_id != "5587BCED-0A36-4EDF-9562-F962A9B1913C" && dr["Order_status_id"].CString("") == "5587BCED-0A36-4EDF-9562-F962A9B1913C")
                {
                    canAddIntegal = -1;
                }

                if (model.Order_status_id == "5587BCED-0A36-4EDF-9562-F962A9B1913C" && dr["Order_status_id"].CString("") != "5587BCED-0A36-4EDF-9562-F962A9B1913C")
                {
                    canAddIntegal = 1;
                }

                ////更新发票，收款
                //order.UpdateInvoice(id);

                //order.UpdateReceive(id);
            }
            else
            {

                id = Guid.NewGuid().ToString();
                model.id = id;
                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                model.arrears_invoice = model.Order_amount;
                model.invoice_money = 0;
                model.Serialnumber = "DD-" + DateTime.Now.ToString("yyyy-MM-dd-") + DateTime.Now.GetHashCode().ToString().Replace("-", "");
                if (model.Order_status_id == "5587BCED-0A36-4EDF-9562-F962A9B1913C")
                {
                    canAddIntegal = 1;
                }
                order.Add(model);
            }

            //用户积分修改
            if (canAddIntegal != 0)
            {
                customerBll.UpdateIntegral(model.Customer_id, (canAddIntegal * model.total_amount).CInt(0, false));
            }


            //产品
            string json = request["PostData"].ToLower();
            var js = new JavaScriptSerializer();

            PostData[] postdata;
            postdata = js.Deserialize<PostData[]>(json);

            var cod = new BLL.Sale_order_details();
            var modeldel = new Model.Sale_order_details()
            {
                order_id = id,
            };

            bool rs = true;
            //cod.Delete($" order_id= '{modeldel.order_id}'");
            for (int i = 0; i < postdata.Length; i++)
            {
                PostData pdata = postdata[i];

                if (PageValidate.checkID(pdata.id))
                    modeldel.product_id = pdata.id;
                else if (PageValidate.checkID(pdata.product_id))
                    modeldel.product_id = pdata.product_id;

                modeldel.quantity = pdata.Quantity;
                modeldel.agio = pdata.agio;
                modeldel.amount = pdata.Amount;
                modeldel.BarCode = pdata.BarCode;

                if (pdata.__status == "add")
                {
                    rs = cod.Add(modeldel);
                }
                else if (pdata.__status == "update")
                {
                    rs = cod.Update(modeldel);
                }
                else if (pdata.__status == "delete")
                {
                    rs = cod.Delete(modeldel.order_id, modeldel.product_id);
                }
            }
            if (rs)
            {
                return XhdResult.Success().ToString();
            }
            else {
                return XhdResult.Error("部分订单产品信息处理失败,请确认").ToString();
            }

        }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " Sale_order.create_time";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "desc";

            string sorttext = $" { sortname } { sortorder}";

            string Total;
            string serchtxt = $" 1=1 ";

            //string issar = request["issarr"];
            //if (issar == "1")
            //{
            //    serchtxt += " and isnull( arrears_money,0)>0";
            //}

            if (PageValidate.checkID(request["customerid"]))
                serchtxt += $" and Sale_order.Customer_id = '{PageValidate.InputText(request["customerid"], 50)}' ";

            if (!string.IsNullOrEmpty(request["T_cus"]))
                serchtxt += $" and CRM_Customer.cus_name like N'%{ PageValidate.InputText(request["T_cus"], 255)}%'";

            if (PageValidate.checkID(request["T_Status_val"]))
                serchtxt += $" and Order_status_id = '{PageValidate.InputText(request["T_Status_val"], 50)}'";

            if (PageValidate.checkID(request["employee_val"]))
                serchtxt += $" and Sale_order.emp_id = '{PageValidate.InputText(request["employee_val"], 50)}'";
            else if (PageValidate.checkID(request["department_val"]))
                serchtxt += $" and hr_department.id = '{PageValidate.InputText(request["department_val"], 50)}' ";

            if (!string.IsNullOrEmpty(request["startdate"]))
                serchtxt += $" and Order_date >= '{ PageValidate.InputText(request["startdate"], 255) }'";

            if (!string.IsNullOrEmpty(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and Order_date <= '{request["enddate"] }'";
            }

            //权限 
            //serchtxt += DataAuthUserID("Sale_order.emp_id");
            serchtxt = GetSQLCreateIDWhere(serchtxt, true);

            DataSet ds = order.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return (dt);
        }

        public string gridbycustomerid()
        {
            string customerid = PageValidate.InputText(request["customerid"], 50);

            if (!PageValidate.checkID(customerid)) return "{}";

            DataSet ds = order.GetList(0, $" Sale_order.Customer_id = '{customerid}' ", " Order_date desc");
            return (GetGridJSON.DataTableToJSON(ds.Tables[0]));
        }

        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);

            DataSet ds = order.GetList($"Sale_order.id = '{id}' ");
            return DataToJson.DataToJSON(ds);

        }

        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return ("delfalse");

            id = PageValidate.InputText(id, 50);
            DataSet ds = order.GetList($"Sale_order.id = '{id}'");

            var receivable = new BLL.Finance_Receivable();
            if (ds.Tables[0].Rows[0]["receive_money"].CDecimal(0, false) > 0)
            {
                return XhdResult.Error("此订单下含有应收款，不允许删除！").ToString();
            }

            bool candel = true;
            if (uid != "admin")
            {
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "3D175449-0461-44B4-A400-3BA2625AA237");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();

                //dataauth
                var dataauth = new GetDataAuth();
                DataAuth auth = dataauth.getAuth(emp_id);
                string authid = ds.Tables[0].Rows[0]["employee_id"].ToString();
                switch (auth.authtype)
                {
                    case 0: candel = false; break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        if (authid.IndexOf(auth.authtext) == -1) candel = false; break;
                }
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }

            bool isdel = order.Delete(id);
            var cod = new BLL.Sale_order_details();
            cod.Delete($"order_id = '{id}'");

            if (!isdel) return XhdResult.Error().ToString();

            //日志
            string EventType = "订单删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["Serialnumber"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();


        }

        public string Compared_empcusorder()
        {
            string idlist = PageValidate.InputText(request["idlist"], int.MaxValue);
            string year1 = PageValidate.InputText(request["year1"], 50);
            string year2 = PageValidate.InputText(request["year2"], 50);
            string month1 = PageValidate.InputText(request["month1"], 50);
            string month2 = PageValidate.InputText(request["month2"], 50);

            if (idlist.Length < 1)
                idlist = "0";

            string[] pid = idlist.Split(';');
            string pidlist = "";
            for (int i = 0; i < pid.Length; i++)
            {
                pidlist += $"'{pid[i]}',";
            }
            pidlist = pidlist.TrimEnd(',');

            //context.Response.Write(emplist);

            DataSet ds = order.Compared_empcusorder(year1, month1, year2, month2, $" (select emp_id from hr_post where id in ({pidlist}) ) ");

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }

        public string emp_cusorder()
        {
            string idlist = PageValidate.InputText(request["idlist"], int.MaxValue);
            string syear = request["syear"];

            if (idlist.Length < 1)
                idlist = "0";

            string[] pid = idlist.Split(';');
            string pidlist = "";
            for (int i = 0; i < pid.Length; i++)
            {
                pidlist += $"'{pid[i]}',";
            }
            pidlist = pidlist.TrimEnd(',');

            DataSet ds = order.report_emporder(int.Parse(syear), $" (select emp_id from hr_post where id in ({pidlist}) ) ");

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }


        public class PostData
        {
            public string id { get; set; }
            public string product_id { get; set; }
            public decimal agio { get; set; }
            public int Quantity { get; set; }
            public decimal Amount { get; set; }
            public string __status { get; set; }


            public string BarCode { get; set; }
        }
    }
}
