using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using XHD.BLL;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class m_follow
    {
        public static BLL.CRM_follow follow = new BLL.CRM_follow();
        public static Model.CRM_follow model = new Model.CRM_follow();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public m_follow()
        {
        }

        public m_follow(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetEmpWithToken(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        public string list()
        {
            int PageIndex = int.Parse(request["pageindex"] == null ? "1" : request["pageindex"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "10" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " follow_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $"1=1  ";

            if (!string.IsNullOrEmpty(request["customer_id"]))
                serchtxt += $" and customer_id ='{PageValidate.InputText(request["customer_id"], 50)}'";

            if (!string.IsNullOrEmpty(request["follow_aim_id"]))
                serchtxt += $" and follow_aim_id ='{PageValidate.InputText(request["follow_aim_id"], 50)}'";

            if (!string.IsNullOrEmpty(request["follow_type_id"]))
                serchtxt += $" and follow_type_id ='{PageValidate.InputText(request["follow_type_id"], 50)}'";


            if (!string.IsNullOrEmpty(request["C_name"]))
                serchtxt += $" and C_name like N'%{PageValidate.InputText(request["C_name"], 200)}%'";

            //权限
            serchtxt += DataAuth();

            DataSet ds = follow.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        public string form()
        {
            string id = PageValidate.InputText(request["id"], 50);

            if (!PageValidate.checkID(id))
                return "{}";

            DataSet ds = follow.GetList($" id = '{id}' " + DataAuth());
            return DataToJson.DataToJSON(ds);
        }

        public string save()
        {
            model.customer_id = PageValidate.InputText(request["customer_id"], 50);
          
            model.follow_content = PageValidate.InputText(request["follow_content"], int.MaxValue);
            model.follow_type_id = PageValidate.InputText(request["follow_type_id"], 50);
            model.follow_aim_id = PageValidate.InputText(request["follow_aim_id"], 50);
            model.contact_id = PageValidate.InputText(request["contact_id"], 50);

            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                DataSet ds = follow.GetList($"id= '{id}' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];
                model.id = id;

                if (uid != "admin")
                {
                    //controll auth                    
                    var getauth = new GetAuthorityByUid();
                    bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "F30F97AC-A5C9-4BCB-AF45-7F6073080F0E");
                    if (!candel)
                        return XhdResult.Error("您没有修改权限，请联系管理员！").ToString();
                }

                follow.Update(model);

                //日志

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = dr["Customer_name"].ToString();
                string EventType = "[APP]跟进修改";
                string EventID = model.id;
                string logcontent = "";

                logcontent += Syslog.get_log_content(dr["follow_content"].ToString(), request["follow_content"], "内容", dr["follow_content"].ToString(), request["follow_content"]);
                logcontent += Syslog.get_log_content(dr["follow_type_id"].ToString(), request["follow_type_id"], "类型", dr["Follow_Type"].ToString(), request["Follow_Type"]);
                logcontent += Syslog.get_log_content(dr["follow_aim_id"].ToString(), request["follow_aim_id"], "目的", dr["Follow_aim"].ToString(), request["Follow_aim"]);
                logcontent += Syslog.get_log_content(dr["contact_id"].ToString(), request["contact_id"], "联系人", dr["contact_name"].ToString(), request["contact_name"]);

                if (!string.IsNullOrEmpty(logcontent))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, logcontent);
            }
            else
            {
                model.id = Guid.NewGuid().ToString();
                model.employee_id = emp_id;
                model.follow_time = DateTime.Now;

                if (uid != "admin")
                {
                    //controll auth                    
                    var getauth = new GetAuthorityByUid();
                    bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "03BC7486-301F-405F-86A2-93D39C2C127D");
                    if (!candel)
                        return XhdResult.Error("您没有新增权限，请联系管理员！").ToString();
                }

                follow.Add(model);
            }
            //最后跟进
            var customer = new BLL.CRM_Customer();
            customer.UpdateLastFollow(model.customer_id.ToString());

            return XhdResult.Success().ToString();
        }

        //删除
        public string del()
        {
            string id = PageValidate.InputText(request["id"], 50);
            DataSet ds = follow.GetList($"id='{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，找不到数据！").ToString();           

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "D3224307-059A-4768-A3F7-6D440614C427");
                if (!candel)
                    return XhdResult.Error("无权限！").ToString();
            }


            bool isdel = follow.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，请检查参数！").ToString();

            //日志
            string EventType = "[APP]跟进删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["Customer_name"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();


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
