using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;

namespace XHD.Server
{
    public class CRM_follow
    {
        private static BLL.CRM_follow follow = new BLL.CRM_follow();
        private static Model.CRM_follow model = new Model.CRM_follow();

        private HttpContext Context;
        private string emp_id;
        private string emp_name;
        private Model.hr_employee employee;
        private HttpRequest request;
        private string uid;
        

        public CRM_follow()
        {
        }

        public CRM_follow(HttpContext context)
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
            model.customer_id = PageValidate.InputText(request["T_customer_val"], 50);
            if (PageValidate.checkID(request["customer_id"]))
                model.customer_id = PageValidate.InputText(request["customer_id"], 50);

            model.contact_id = PageValidate.InputText(request["T_contact_val"], 50);
            model.follow_content = PageValidate.InputText(request["T_follow"], int.MaxValue);
            model.follow_type_id = PageValidate.InputText(request["T_followtype_val"], 50);
            model.follow_aim_id = PageValidate.InputText(request["T_followaim_val"], 50);

            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                DataSet ds = follow.GetList($"id= '{id}' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];
                model.id = id;

                follow.Update(model);

                //日志

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = dr["Customer_name"].ToString();
                string EventType = "客户跟进修改";
                string EventID = model.id;
                string Log_Content = null;

                if (dr["follow_content"].ToString() != request["T_follow"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "跟进内容", dr["follow_content"], request["T_follow"]);

                if (dr["follow_type_id"].ToString() != request["T_followtype_val"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "跟进类型", dr["Follow_Type"], request["T_followtype"]);

                if (dr["follow_aim_id"].ToString() != request["T_followaim_val"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "跟进目的", dr["Follow_aim"], request["T_followaim"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);


            }
            else
            {
                model.id = Guid.NewGuid().ToString();
                model.employee_id = emp_id;
                model.follow_time = DateTime.Now;
                

                follow.Add(model);
            }
            //最后跟进
            var customer = new BLL.CRM_Customer();
            customer.UpdateLastFollow(model.customer_id.ToString());

            return XhdResult.Success().ToString();
        }

        public string form(string id)
        {
            id = PageValidate.InputText(id, 50);
            if (!PageValidate.checkID(id)) return "{}";

            DataSet ds = follow.GetList($"id='{id}'  " + DataAuth());
            return DataToJson.DataToJSON(ds);

        }

        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return ("false");
            id = PageValidate.InputText(id, 50);
            DataSet ds = follow.GetList($"id = '{id}' ");

            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "D3224307-059A-4768-A3F7-6D440614C427");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();

                var dataauth = new GetDataAuth();
                DataAuth auth = dataauth.getAuth(emp_id);
                string authid = ds.Tables[0].Rows[0]["employee_id"].ToString();
                switch (auth.authtype)
                {
                    case 0: candel = false; break;
                    case 1:
                    case 2:
                    case 3:
                    case 4: if (authid.IndexOf(auth.authtext) == -1) candel = false; break;
                }

                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }

            bool isdel = follow.Delete(id);

            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();

            //最后跟进
            var customer = new BLL.CRM_Customer();
            string customerid = ds.Tables[0].Rows[0]["customer_id"].ToString();
            customer.UpdateLastFollow(PageValidate.InputText(customerid, 50));

            //日志
            string EventType = "跟进删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["Customer_name"].ToString();

            string Log_Content = null;

            Log_Content += string.Format("【{0}】{1}", "跟进内容", ds.Tables[0].Rows[0]["follow_content"]);

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);

            return XhdResult.Success().ToString();

        }

        //serch
        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " follow_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;

            string serchtxt = $" 1=1 ";

            if (!string.IsNullOrEmpty(request["customer_id"]))
                serchtxt += $" and customer_id= '{PageValidate.InputText(request["customer_id"], 50)}'";

            if (!string.IsNullOrEmpty(request["company"]))
                serchtxt += $" and Customer_id in (select id from CRM_Customer where cus_name like N'%{ PageValidate.InputText(request["company"], 255)}%')";

            if (PageValidate.checkID(request["employee_val"]))
                serchtxt += $" and employee_id = '{PageValidate.InputText(request["employee_val"], 50)}'";
            else if (PageValidate.checkID(request["department_val"]))
                serchtxt += $" and employee_id in (select id from hr_employee where dep_id = '{PageValidate.InputText(request["department_val"], 50)}' )";

            if (PageValidate.checkID(request["followtype_val"]))
                serchtxt += $" and follow_type_id = '{PageValidate.InputText(request["followtype_val"], 50)}'";

            if (!string.IsNullOrEmpty(request["startdate"]))
                serchtxt += $" and Follow_time >= '{ PageValidate.InputText(request["startdate"], 255) }'";

            if (!string.IsNullOrEmpty(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and Follow_time  <= '{ enddate }'";
            }


            if (!string.IsNullOrEmpty(request["T_smart"]))
            {
                if (request["T_smart"] != "输入关键词智能搜索跟进内容")
                    serchtxt += $" and follow_content like N'%{ PageValidate.InputText(request["T_smart"], 255) }%'";
            }
            //权限
            serchtxt += DataAuth();

            DataSet ds = follow.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return (dt);
        }

        public string Compared_follow()
        {
            string year1 = PageValidate.InputText(request["year1"], 50);
            string year2 = PageValidate.InputText(request["year2"], 50);
            string month1 = PageValidate.InputText(request["month1"], 50);
            string month2 = PageValidate.InputText(request["month2"], 50);

            DataSet ds = follow.Compared_follow(year1, month1, year2, month2);

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return (dt);
        }

        public string Compared_empcusfollow()
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

            var post = new BLL.hr_post();
            DataSet dspost = post.GetList("id in(" + pidlist + ")");

            

            //context.Response.Write(emplist);

            DataSet ds = follow.Compared_empcusfollow(year1, month1, year2, month2, $" (select emp_id from hr_post where id in ({pidlist}) )" );

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return (dt);
        }

        public string emp_cusfollow()
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

            //context.Response.Write(emplist);

            DataSet ds = follow.report_empfollow(int.Parse(syear), $" (select emp_id from hr_post where id in ({pidlist}) )");

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return (dt);
        }

        private string DataAuth()
        {
            GetDataAuth dataauth = new GetDataAuth();
            DataAuth auth = dataauth.getAuth(emp_id);

            switch (auth.authtype)
            {
                case 0: return " 1=2 ";
                case 1:
                case 2:
                case 3:
                case 4:
                    return $" and employee_id in ({auth.authtext})";
                case 5: return "";
            }

            return auth.authtype + ":" + auth.authtext;
        }
    }
}
