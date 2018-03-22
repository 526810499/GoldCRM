using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;
namespace XHD.Server
{
    public class Finance_Receive
    {
        private static BLL.Finance_Receive receive = new BLL.Finance_Receive();
        private static Model.Finance_Receive model = new Model.Finance_Receive();

        private HttpContext Context;
        private string emp_id;
        private string emp_name;
        private Model.hr_employee employee;
        private HttpRequest request;
        private string uid;
        

        public Finance_Receive()
        {
        }

        public Finance_Receive(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " Finance_Receive.create_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $"1=1 ";

            if (!string.IsNullOrEmpty(request["startdate"]))
                serchtxt += $" and Finance_Receive.Receive_date >= '{ PageValidate.InputText(request["startdate"], 255) }'";

            if (!string.IsNullOrEmpty(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and Finance_Receive.Receive_date <= '{request["enddate"] }'";
            }

            if (!string.IsNullOrEmpty(request["T_emp"]))
                serchtxt += $" and hr_employee.name like N'%{ PageValidate.InputText(request["T_emp"], 255)}%'";

            //权限           


            //context.Response.Write(serchtxt);
            DataSet ds = receive.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        public string form(string id)
        {
            string dt;
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);

            DataSet ds = receive.GetList($"Finance_Receive.id = '{id}' ");
            dt = DataToJson.DataToJSON(ds);

            return dt;
        }

        public string save()
        {
            model.Receivable_id = PageValidate.InputText(request["T_receivable_val"], 250);
            model.Receive_date = DateTime.Parse(request["T_date"]);

            model.Receive_amount = decimal.Parse(request["T_amount"]);
            model.Pay_type_id= PageValidate.InputText(request["T_pay_type_val"], 50);

            model.Payee_id= PageValidate.InputText(request["T_Payee_val"], 50);

            model.Remarks = PageValidate.InputText(request["T_Remark"], int.MaxValue);

            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                DataSet ds = receive.GetList($"Finance_Receive.id = '{id}' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                model.id = id;

                receive.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.Receive_num;

                string EventType = "收款修改";
                string EventID = model.id;
                string Log_Content = null;

                Log_Content += Syslog.get_log_content(dr["Receivable_id"].ToString(), request["T_receivable_val"], "收款单", dr["receivable_no"].ToString(), request["T_receivable"]);
                Log_Content += Syslog.get_log_content(dr["Pay_type_id"].ToString(), request["T_pay_type_val"], "付款方式", dr["Pay_type"].ToString(), request["T_pay_type"]);
                Log_Content += Syslog.get_log_content(dr["Payee_id"].ToString(), request["T_Payee_val"], "收款人", dr["payee"].ToString(), request["T_Payee_val"]);
                Log_Content += Syslog.get_log_content(
                    DateTime.Parse(dr["Receive_date"].ToString()).ToShortDateString(),
                    DateTime.Parse(request["T_date"]).ToShortDateString(),
                    "收款时间",
                    DateTime.Parse(dr["Receive_date"].ToString()).ToShortDateString(),
                    DateTime.Parse(request["T_date"]).ToShortDateString()
                    );
                Log_Content += Syslog.get_log_content(
                    decimal.Parse(dr["Receive_amount"].ToString()).ToString(),
                    decimal.Parse(request["T_amount"]).ToString(),
                    "收款金额",
                    decimal.Parse(dr["Receive_amount"].ToString()).ToString(),
                    decimal.Parse(request["T_amount"]).ToString()
                    );
                Log_Content += Syslog.get_log_content(dr["Remarks"].ToString(), request["T_Remark"], "备注", dr["Remarks"].ToString(), request["T_Remark"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                model.id = Guid.NewGuid().ToString();
                
                model.create_id = emp_id;
                model.create_time = DateTime.Now;

                
                    model.Receive_num = "SK-" + DateTime.Now.ToString("yyyy-MM-dd-") + DateTime.Now.GetHashCode().ToString().Replace("-", "");
                receive.Add(model);

            }

            string orderid = PageValidate.InputText(request["T_order_id"], 50);

            //更新收款
            receive.UpdateReceive(model.Receivable_id, orderid);

            return XhdResult.Success().ToString();
        }

        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();
            id = PageValidate.InputText(id, 50);
            DataSet ds = receive.GetList($" Finance_Receive.id = '{id}' ");

            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();            

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "3D175449-0461-44B4-A400-3BA2625AA237");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = receive.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误！").ToString();
           
            //更新收款
            receive.UpdateReceive(ds.Tables[0].Rows[0]["Receivable_id"].ToString(), ds.Tables[0].Rows[0]["order_id"].ToString());

            //日志
            string EventType = "收款删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["Receive_num"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();
        }
    }
}
