
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
            //1 为数据汇总
            if (request["datacount"].CInt(0, false) == 0)
            {
                allDataBtnid = "4C2A57BB-94A5-401A-82AA-24DE1F5DE4DB";
                depDataBtnid = "1F5A29CE-CE24-4A96-9B98-D72D5AF9B924";
            }
            else {
                allDataBtnid = "3D873EDF-658B-459A-8944-F75745AF106A";
                depDataBtnid = "570B94D6-A8A2-4D99-B645-EFDEE5149005";
            }
        }

        public string save()
        {

            //是否提交确认保存订单 1是 0 否 
            int saveorder = request["save"].CInt(0, false);

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
            model.receive_money = model.total_amount;// decimal.Parse(request["T_receive"]);
            model.arrears_money = decimal.Parse(request["T_arrears"]);
            model.emp_id = PageValidate.InputText(request["T_emp_val"], 50);
            model.cashier_id = PageValidate.InputText(request["T_cashier_val"], 50);
            model.vipcard = PageValidate.InputText(request["T_vipcard"], 50);
            model.saledep_id = PageValidate.InputText(request["T_saledep_id_val"], 50);
            model.PayTheBill = PageValidate.InputText(request["T_PayTheBill"], 50);

            model.VipCardType = request["T_VipCardType_val"].CInt(0, false);
            model.DiscountType = request["T_DiscountType_val"].CInt(0, false);
            model.SaleType = request["T_SaleType_val"].CInt(0, false);
            model.DiscountCount = request["T_DiscountCount"].CDecimal(0, false);
            model.VerifyStatus = (saveorder == 1 ? 0 : -1);
            if (string.IsNullOrWhiteSpace(model.saledep_id))
            {
                model.saledep_id = dep_id;
            }
            model.createdep_id = dep_id;
            string id = PageValidate.InputText(request["id"], 50);
            decimal oldtotal_amount = 0;
            int oldVerifyStatus = 0;
            decimal jf = 0;
            if (PageValidate.checkID(id))
            {
                model.id = id;
                DataSet ds = order.GetList($"Sale_order.id = '{id}' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];
                model.Customer_id = dr["Customer_id"].CString("");
                oldVerifyStatus = dr["VerifyStatus"].CInt(-2, false);
                oldtotal_amount = dr["total_amount"].CDecimal(0, false);
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
                Syslog.get_log_content(dr["saledep_id"].ToString(), request["T_saledep_id_val"], "saledep_id", dr["saledep_id"].ToString(), request["T_saledep_id_val"]);
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
                 dr["PayTheBill"].CString(""),
                  request["T_PayTheBill"].CString(""),
                  "T_PayTheBill",
                 dr["PayTheBill"].CString(""),
                  request["T_PayTheBill"].CString("")
                  );

                Log_Content += Syslog.get_log_content(
               dr["arrears_money"].CString(""), request["T_arrears"].CString(""), "未收金额", dr["arrears_money"].CString(""), request["T_arrears"].CString(""));

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
                jf = model.total_amount.CDecimal(0, false) - oldtotal_amount;
                if (model.Order_status_id != "5587BCED-0A36-4EDF-9562-F962A9B1913C" && dr["Order_status_id"].CString("") == "5587BCED-0A36-4EDF-9562-F962A9B1913C")
                {
                    //支付变未支付扣掉相应的
                    jf = -model.total_amount.CDecimal(0, false);
                    canAddIntegal = -1;
                }

                if (model.Order_status_id == "5587BCED-0A36-4EDF-9562-F962A9B1913C" && dr["Order_status_id"].CString("") != "5587BCED-0A36-4EDF-9562-F962A9B1913C")
                {
                    canAddIntegal = 1;
                }
                //已支付的
                if (model.Order_status_id == "5587BCED-0A36-4EDF-9562-F962A9B1913C" && dr["Order_status_id"].CString("") == "5587BCED-0A36-4EDF-9562-F962A9B1913C")
                {
                    if (jf > 0)
                    {
                        canAddIntegal = 1;
                    }
                    else {
                        canAddIntegal = -1;
                    }
                }

                if (model.VerifyStatus != 0)
                {
                    canAddIntegal = 0;
                }
                else if (model.VerifyStatus == 0 && oldVerifyStatus == -1)
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
                model.Serialnumber = GetOrderID();
                canAddIntegal = 0;
                if (model.Order_status_id == "5587BCED-0A36-4EDF-9562-F962A9B1913C" && model.VerifyStatus == 0)
                {
                    jf = model.total_amount.CDecimal(0, false); ;
                    canAddIntegal = 1;
                }
                order.Add(model);
            }

            //用户积分修改
            if (canAddIntegal != 0)
            {
                if (jf != 0)
                {
                    customerBll.UpdateIntegral(model.Customer_id, jf.CInt(0, false));
                }
            }


            //商品
            string json = request["PostData"].ToLower();
            var js = new JavaScriptSerializer();

            PostData[] postdata;
            postdata = js.Deserialize<PostData[]>(json);

            var cod = new BLL.Sale_order_details();
            var modeldel = new Model.Sale_order_details()
            {
                order_id = id,
            };
            bool isHQ = dep_id == "7C881F36-3597-483B-BC71-EB5D7CFDA2C7";
            bool rs = true;
            //cod.Delete($" order_id= '{modeldel.order_id}'");
            for (int i = 0; i < postdata.Length; i++)
            {
                PostData pdata = postdata[i];

                if (PageValidate.checkID(pdata.id))
                    modeldel.product_id = pdata.id;
                else if (PageValidate.checkID(pdata.product_id))
                    modeldel.product_id = pdata.product_id;
                modeldel.agio = 0;
                modeldel.quantity = pdata.Quantity;
                modeldel.SalesUnitPrice = pdata.SalesUnitPrice;
                modeldel.RealTotal = pdata.RealTotal;
                modeldel.amount = pdata.Amount;
                modeldel.BarCode = pdata.BarCode;
                modeldel.SaleType = pdata.SaleType;
                modeldel.DiscountCount = pdata.DiscountCount;
                modeldel.Discounts = pdata.Discounts;

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

                    rs = cod.Delete(modeldel.order_id, modeldel.product_id, isHQ);
                }
            }
            if (rs)
            {
                return XhdResult.Success().ToString();
            }
            else {
                return XhdResult.Error("部分订单商品信息处理失败,请确认").ToString();
            }

        }

        /// <summary>
        /// 财务核销
        /// </summary>
        /// <returns></returns>
        public string VerifySave()
        {
            string id = PageValidate.InputText(request["id"], 50);
            DataSet ds = order.GetList($"Sale_order.id = '{id}' ");
            if (ds.Tables[0].Rows.Count == 0)
                return XhdResult.Error("参数不正确，更新失败！").ToString();
            int status = request["vstatus"].CInt(1, false);
            string remark = PageValidate.InputText(request["remark"], 5000);
            try
            {
                order.VerifySave(id, remark, emp_id, status);
                return XhdResult.Success().ToString();
            }
            catch (Exception error)
            {
                SoftLog.LogStr(id + "," + error.ToString(), "OrderVerifySave");
                if (status == 1)
                {
                    order.VerifySave(id, remark, emp_id, 0);
                }
                return XhdResult.Error("核销失败,请确认").ToString();
            }


        }

        private static object objlock = new object();
        /// <summary>
        /// 获取订单号
        /// </summary>
        /// <returns></returns>
        private string GetOrderID()
        {
            lock (objlock)
            {
                BLL.Product_category sbll = new BLL.Product_category();
                Model.Sys_SerialNumber serial = sbll.GetSerialNumber(Model.SerialNumberType.OrderNO);
                string orderid = "DD" + DateTime.Now.ToString("yyMMddHH");
                if (serial.Times.Date != DateTime.Now.Date)
                {
                    sbll.ResetSerialNumber(serial.ID, "1", 1);
                    serial.Counts = 1;
                }

                orderid = "DD" + DateTime.Now.ToString("yyMMddHH") + serial.Counts.CString("1").PadLeft(5, '0');

                return orderid;
            }
        }

        private string GetStWhere()
        {
            string serchtxt = $" 1=1 ";

            if (PageValidate.checkID(request["customerid"]))
                serchtxt += $" and Sale_order.Customer_id = '{PageValidate.InputText(request["customerid"], 50)}' ";

            if (!string.IsNullOrEmpty(request["T_cus"]))
                serchtxt += $" and CRM_Customer.cus_name like N'%{ PageValidate.InputText(request["T_cus"], 255)}%'";


            if (PageValidate.checkID(request["T_Status_val"]))
                serchtxt += $" and Order_status_id = '{PageValidate.InputText(request["T_Status_val"], 50)}'";


            if (PageValidate.checkID(request["employee_val"]))
                serchtxt += $" and Sale_order.emp_id = '{PageValidate.InputText(request["employee_val"], 50)}'";

            if (PageValidate.checkID(request["department_val"]))
                serchtxt += $" and saledep_id = '{PageValidate.InputText(request["department_val"], 50)}' ";

            if (!string.IsNullOrEmpty(request["startdate"]))
                serchtxt += $" and Order_date >= '{ PageValidate.InputText(request["startdate"], 255) }'";

            if (!string.IsNullOrEmpty(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and Order_date <= '{request["enddate"] }'";
            }

            if (request["user"].CInt(0, false) == 1)
            {
                serchtxt += $" and Sale_order.emp_id = '{emp_id }'";
            }
            int verifystatus = request["verifystatus"].CInt(-1, false);
            if (verifystatus > -1 && request["verifystatus"] != null)
            {
                serchtxt += $" and verifystatus = {verifystatus }";
            }
            //是否只取确认提交保存的数据 1是
            int saveorder = request["saveorder"].CInt(0, false);
            if (saveorder == 1)
            {
                serchtxt += $" and verifystatus <>-1 ";
            }
            //财务核销 1 是 0 否
            int cwVerify = request["cwVerify"].CInt(0, false);
            if (cwVerify == 0)
            {
                string temp = GetSQLCreateIDWhere(serchtxt, true, "Sale_order");

            }
            else {
                serchtxt += $" and Sale_order.Order_status_id = '5587BCED-0A36-4EDF-9562-F962A9B1913C' ";
            }
            return serchtxt;
        }

        private string GetStWhereData(bool isExport)
        {
            string serchtxt = $" 1=1 ";

            if (PageValidate.checkID(request["customerid"]))
                serchtxt += $" and Customer_id = '{PageValidate.InputText(request["customerid"], 50)}' ";

            if (!string.IsNullOrWhiteSpace(request["T_OrderID"]))
                serchtxt += $" and Serialnumber = '{PageValidate.InputText(request["T_OrderID"], 50)}' ";

            if (!string.IsNullOrEmpty(request["T_cus"]))
                serchtxt += $" and cus_name like N'%{ PageValidate.InputText(request["T_cus"], 255)}%'";


            serchtxt += $" and Order_status_id ='5587BCED-0A36-4EDF-9562-F962A9B1913C' ";


            if (PageValidate.checkID(request["employee_val"]))
                serchtxt += $" and emp_id = '{PageValidate.InputText(request["employee_val"], 50)}'";

            if (PageValidate.checkID(request["department_val"]))
                serchtxt += $" and saledep_id = '{PageValidate.InputText(request["department_val"], 50)}' ";

            if (!string.IsNullOrEmpty(request["startdate"]))
            {
                serchtxt += $" and Order_date >= '{ PageValidate.InputText(request["startdate"], 255) }'";
            }
            else if (isExport)
            {
                serchtxt += $" and Order_date >= '{DateTime.Now.Date.AddDays(-1)}'";
            }

            if (!string.IsNullOrEmpty(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and Order_date <= '{request["enddate"] }'";
            }
            else if (isExport)
            {
                serchtxt += $" and Order_date < '{DateTime.Now.Date.AddDays(1)}'";
            }
            if (request["user"].CInt(0, false) == 1)
            {
                serchtxt += $" and emp_id = '{emp_id }'";
            }
            int verifystatus = request["vstatus"].CInt(-1, false);
            if (verifystatus > -1 && !string.IsNullOrWhiteSpace(request["vstatus"]))
            {
                serchtxt += $" and verifystatus = {verifystatus }";
            }
            //是否只取确认提交保存的数据 1是
            int saveorder = request["saveorder"].CInt(0, false);
            if (saveorder == 1)
            {
                serchtxt += $" and verifystatus <>-1 ";
            }
            //财务核销 1 是 0 否
            int cwVerify = request["cwVerify"].CInt(0, false);
            if (cwVerify == 0)
            {
                string temp = GetSQLCreateIDWhere(serchtxt, true);
            }
            else {
                serchtxt += $" and  Order_status_id = '5587BCED-0A36-4EDF-9562-F962A9B1913C' ";
            }
            return serchtxt;
        }


        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        public DataTable ExportData()
        {
            string serchtxt = GetStWhereData(true);

            return order.ExportData(serchtxt);
        }

        /// <summary>
        /// 销售数据惠州
        /// </summary>
        /// <returns></returns>
        public string gridData()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " create_time";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "desc";

            string sorttext = $" { sortname } { sortorder}";

            string serchtxt = GetStWhereData(false);

            string Total = "0";

            DataTable otable = new DataTable();

            DataSet ds = order.GetListData(PageSize, PageIndex, serchtxt, sorttext, out otable);
            if (otable != null)
            {
                Total = otable.Rows[0]["counts"].CString("0");
            }


            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0], Total, otable);
            return (dt);

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
            string serchtxt = GetStWhere();
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
            if (!PageValidate.checkID(id))
            {
                DataSet ds1 = order.GetList($"Sale_order.id ='-1'");
                DataTable table = ds1.Tables[0];
                DataRow rows = table.NewRow();
                rows["saledep_id"] = dep_id;
                rows["Order_date"] = DateTime.Now;
                rows["Order_status_id"] = "5587BCED-0A36-4EDF-9562-F962A9B1913C";
                table.Rows.Add(rows);
                return DataToJson.DataToJSON(ds1);
            };
            id = PageValidate.InputText(id, 50);

            DataSet ds = order.GetList($"Sale_order.id = '{id}' ");




            return DataToJson.DataToJSON(ds);

        }



        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return ("delfalse");

            id = PageValidate.InputText(id, 50);
            DataSet ds = order.GetList($"Sale_order.id = '{id}'");

            if (ds.Tables[0].Rows.Count == 0)
            {
                return XhdResult.Error("参数不正确，更新失败！").ToString();
            }
            string logs = "";
            DataRow dr = ds.Tables[0].Rows[0];
            int VerifyStatus = dr["VerifyStatus"].CInt(0, false);
            string Order_status_id = dr["Order_status_id"].CString("");
            if (Order_status_id == "5587BCED-0A36-4EDF-9562-F962A9B1913C" && VerifyStatus != -1 && !CheckIsAdmin())
            {
                return XhdResult.Error("已支付订单不允许删除！").ToString();
            }
            logs += "状态:" + Order_status_id;
            var cod = new BLL.Sale_order_details();
            DataSet odds = cod.GetList($"order_id='{id}'");

            if (odds != null)
            {
                bool isHQ = dep_id == "7C881F36-3597-483B-BC71-EB5D7CFDA2C7";
                DataTable tables = odds.Tables[0];
                foreach (DataRow rows in tables.Rows)
                {
                    string product_id = rows["product_id"].CString("");
                    string BarCode = rows["BarCode"].CString("");
                    logs += $";BarCode={BarCode},productid={product_id},orderid={id}";
                    cod.Delete(id, product_id, isHQ);
                }
            }


            bool candel = true;
            if (uid != "admin")
            {
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "3D175449-0461-44B4-A400-3BA2625AA237");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = order.Delete(id);

            cod.Delete($"order_id = '{id}'");

            if (!isdel) return XhdResult.Error().ToString();

            //已支付的需要扣除相应的积分
            if (Order_status_id == "5587BCED-0A36-4EDF-9562-F962A9B1913C")
            {
                decimal oldtotal_amount = dr["total_amount"].CDecimal(0, false);
                string Customer_id = dr["Customer_id"].CString("");
                if (oldtotal_amount >= 1)
                {
                    customerBll.UpdateIntegral(Customer_id, -oldtotal_amount.CInt(0, false));
                }
            }

            //日志
            string EventType = "订单删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["Serialnumber"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, logs);

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

            public int Quantity { get; set; }
            public decimal Amount { get; set; }
            public string __status { get; set; }

            /// <summary>
            /// 优惠金额
            /// </summary>
            public decimal Discounts { get; set; }

            /// <summary>
            /// 折扣 DiscountType 1 时对应的折扣比
            /// </summary>
            public decimal DiscountCount { get; set; }

            public int SaleType { get; set; }

            public string BarCode { get; set; }

            /// <summary>
            /// 实时计算价
            /// </summary>
            public decimal RealTotal { get; set; }

            /// <summary>
            /// 实时销售单价
            /// </summary>
            public decimal SalesUnitPrice { get; set; }
        }
    }
}
