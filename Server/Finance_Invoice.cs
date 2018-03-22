
using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Finance_Invoice
    {
        private static BLL.Finance_Invoice invoice = new BLL.Finance_Invoice();
        private static Model.Finance_Invoice model = new Model.Finance_Invoice();
        private static BLL.Sale_order order = new BLL.Sale_order();

        private HttpContext Context;
        private string emp_id;
        private string emp_name;
        private Model.hr_employee employee;
        private HttpRequest request;
        private string uid;
        

        public Finance_Invoice()
        {
        }

        public Finance_Invoice(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        public string save()
        {            
            model.order_id = PageValidate.InputText(request["T_order_val"], 50);
            model.empid = PageValidate.InputText(request["T_emp_val"], 50);

            model.invoice_num= PageValidate.InputText(request["T_no"], 50);
            model.invoice_amount = decimal.Parse(request["T_amount"]);
            model.invoice_date = DateTime.Parse(request["T_date"]);
            model.invoice_type_id = PageValidate.InputText(request["T_type_val"], 50);
            model.invoice_content = PageValidate.InputText(request["T_Remark"], int.MaxValue);

            string cid = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(cid))
            {
                model.id = (cid);

                DataSet ds = invoice.GetList($" Finance_Invoice.id = '{cid}' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                invoice.Update(model);

                //日志

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.invoice_num;
                string EventType = "开票修改";
                string EventID = model.id;
                string Log_Content = null;

                string logcontent = "";
                logcontent += Syslog.get_log_content(dr["order_id"].ToString(), request["T_order_val"], "订单", dr["order_no"].ToString(), request["T_order"]);
                logcontent += Syslog.get_log_content(dr["empid"].ToString(), request["T_emp_val"], "开票人", dr["name"].ToString(), request["T_emp"]);
                logcontent += Syslog.get_log_content(dr["invoice_num"].ToString(), request["T_no"], "发票号码", dr["invoice_num"].ToString(), request["T_no"]);
                logcontent += Syslog.get_log_content(dr["invoice_type_id"].ToString(), request["T_type_val"], "发票类别", dr["invoice_type"].ToString(), request["T_type"]);
                logcontent += Syslog.get_log_content(dr["invoice_content"].ToString(), request["T_Remark"], "开票内容", dr["invoice_content"].ToString(), request["T_Remark"]);

                Log_Content += Syslog.get_log_content(
                    DateTime.Parse(dr["invoice_date"].ToString()).ToShortDateString(),
                    DateTime.Parse(request["T_date"]).ToShortDateString(),
                    "开票日期",
                    dr["invoice_date"].ToString(),
                    request["T_date"]
                    );

                Log_Content += Syslog.get_log_content(
                    decimal.Parse(dr["invoice_amount"].ToString()).ToString(),
                    decimal.Parse(request["T_amount"]).ToString(),
                    "开票金额",
                    dr["invoice_amount"].ToString(),
                    request["T_amount"]
                    );

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                
                model.id = Guid.NewGuid().ToString();
                model.create_id = emp_id;
                model.create_time = DateTime.Now;

                invoice.Add(model);
            }
            //更新订单发票金额
            order.UpdateInvoice(model.order_id);

            return XhdResult.Success().ToString();
        }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " Finance_Invoice.create_time";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";

            string order_id = PageValidate.InputText(request["orderid"], 50);
            if (!string.IsNullOrEmpty(order_id))
                serchtxt += $" and order_id= '{order_id}'";

            if (!string.IsNullOrEmpty(request["company"]))
                serchtxt += $" and  customer_id in (select id from CRM_Customer where Customer like N'%{ PageValidate.InputText(request["company"], 255)}%')";

            if (!string.IsNullOrEmpty(request["receive_num"]))
                serchtxt += " and invoice_num like N'%" + PageValidate.InputText(request["receive_num"], 255) + "%'";

            if (!string.IsNullOrEmpty(request["pay_type"]))
                serchtxt += " and invoice_type_id =" + int.Parse(request["pay_type_val"]);

            if (!string.IsNullOrEmpty(request["department"]))
                serchtxt += " and C_depid =" + int.Parse(request["department_val"]);

            if (!string.IsNullOrEmpty(request["employee"]))
                serchtxt += " and C_empid =" + int.Parse(request["employee_val"]);

            if (!string.IsNullOrEmpty(request["startdate"]))
                serchtxt += " and invoice_date >= '" + PageValidate.InputText(request["startdate"], 255) + "'";

            if (!string.IsNullOrEmpty(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]);
                serchtxt += " and invoice_date  <= '" +
                            DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59) + "'";
            }
            if (!string.IsNullOrEmpty(request["startdate_del"]))
            {
                serchtxt += " and Delete_time >= '" + PageValidate.InputText(request["startdate_del"], 255) + "'";
            }
            if (!string.IsNullOrEmpty(request["enddate_del"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate_del"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += " and Delete_time  <= '" + enddate + "'";
            }
            //权限
            DataSet ds = invoice.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return (dt);
        }

        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);

            DataSet ds = invoice.GetList($"[Finance_Invoice].id='{id}' ");
            return DataToJson.DataToJSON(ds);

        }

        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();
            id = PageValidate.InputText(id, 50);

            DataSet ds = invoice.GetList($"Finance_Invoice.id = '{id}'  ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            bool candel = true;
            if (uid != "admin")
            {
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "78EED529-5538-4DE0-9FC7-7A7DBBABE9B1");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = invoice.Delete(id);

            //更新订单发票金额
            var order = new BLL.Sale_order();
            string orderid = ds.Tables[0].Rows[0]["order_id"].ToString();
            order.UpdateInvoice(orderid);

            if (!isdel)
                return XhdResult.Error("删除失败，请检查参数！").ToString();

            //日志
            string EventType = "开票删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["invoice_num"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();


        }
    }
}