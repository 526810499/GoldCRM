using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;
namespace XHD.Server
{
    public class Finance_Receivable : BaseCRMServer
    {
        private static BLL.Finance_Receivable receivable = new BLL.Finance_Receivable();
        private static Model.Finance_Receivable model = new Model.Finance_Receivable();

        private static BLL.Finance_Receive receive = new BLL.Finance_Receive();

 

        public Finance_Receivable()
        {
        }

        public Finance_Receivable(HttpContext context) : base(context) { }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " Finance_Receivable.create_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";

            if (PageValidate.checkID(request["customerid"]))
                serchtxt += $" and CRM_Customer.id='{PageValidate.InputText(request["customerid"], 50)}'";

            if (!string.IsNullOrEmpty(request["startdate"]))
                serchtxt += $" and Finance_Receivable.receivable_time >= '{ PageValidate.InputText(request["startdate"], 255) }'";

            if (!string.IsNullOrEmpty(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and Finance_Receivable.receivable_time <= '{request["enddate"] }'";
            }

            if (!string.IsNullOrEmpty(request["T_Customer"]))
                serchtxt += $" and CRM_Customer.cus_name like N'%{ PageValidate.InputText(request["T_Customer"], 255)}%'";

            //权限           


            //context.Response.Write(serchtxt);
            DataSet ds = receivable.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        public string form(string id)
        {
            string dt;
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);

            DataSet ds = receivable.GetList($"Finance_Receivable.id = '{id}' ");
            dt = DataToJson.DataToJSON(ds);

            return dt;
        }

        public string save()
        {
            model.order_id = PageValidate.InputText(request["T_order_val"], 250);
            model.receivable_time = DateTime.Parse(request["T_date"]);
            model.receivable_amount = decimal.Parse(request["T_amount"]);

            model.Remark = PageValidate.InputText(request["T_Remark"], int.MaxValue);

            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                DataSet ds = receivable.GetList($"Finance_Receivable.id = '{id}' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                model.id = id;

                receivable.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.receivable_no;

                string EventType = "应收款修改";
                string EventID = model.id;
                string Log_Content = null;

                Log_Content += Syslog.get_log_content(dr["order_id"].ToString(), request["T_order_val"], "订单", dr["Order_no"].ToString(), request["T_order"]);
                Log_Content += Syslog.get_log_content(
                    DateTime.Parse(dr["receivable_time"].ToString()).ToShortDateString(),
                    DateTime.Parse(request["T_date"]).ToShortDateString(),
                    "应收时间",
                    DateTime.Parse(dr["receivable_time"].ToString()).ToShortDateString(),
                    DateTime.Parse(request["T_date"]).ToShortDateString()
                    );
                Log_Content += Syslog.get_log_content(
                    decimal.Parse(dr["receivable_amount"].ToString()).ToString(),
                    decimal.Parse(request["T_amount"]).ToString(),
                    "应收金额",
                    decimal.Parse(dr["receivable_amount"].ToString()).ToString(),
                    decimal.Parse(request["T_amount"]).ToString()
                    );
                Log_Content += Syslog.get_log_content(dr["Remark"].ToString(), request["T_Remark"], "备注", dr["Remark"].ToString(), request["T_Remark"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                model.id = Guid.NewGuid().ToString();
                
                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                model.arrears_amount = model.receivable_amount;
                model.received_amount = 0;


                model.receivable_no = "YSK-" + DateTime.Now.ToString("yyyy-MM-dd-") + DateTime.Now.GetHashCode().ToString().Replace("-", "");
                receivable.Add(model);

            }

            return XhdResult.Success().ToString();
        }

        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();
            id = PageValidate.InputText(id, 50);
            DataSet ds = receivable.GetList($" Finance_Receivable.id = '{id}'");

            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            if (receive.GetList($" Finance_Receive.Receivable_id = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此应收单下含有收款信息，不允许删除！").ToString();

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "3D175449-0461-44B4-A400-3BA2625AA237");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = receivable.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误！").ToString();

            //日志
            string EventType = "应收款删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["receivable_no"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();
        }


    }
}
