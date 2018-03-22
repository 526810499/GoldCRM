using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using XHD.BLL;
using XHD.Common;
using XHD.Controller;
using System.Text;

namespace XHD.Server
{
    public class m_customer
    {
        public static BLL.CRM_Customer customer = new BLL.CRM_Customer();
        public static BLL.Sys_log log = new BLL.Sys_log();
        public static BLL.CRM_Contact contact = new BLL.CRM_Contact();
        public static BLL.CRM_follow follow = new BLL.CRM_follow();
        public static BLL.Sale_order order = new BLL.Sale_order();
        public static BLL.Finance_Receivable receivable = new BLL.Finance_Receivable();

        public static Model.CRM_Customer model = new Model.CRM_Customer();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public m_customer()
        {
        }

        public m_customer(HttpContext context)
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
                sortname = " create_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $"isDelete=0 ";

            if (!string.IsNullOrEmpty(request["cus_name"]))
                serchtxt += $" and cus_name like N'%{PageValidate.InputText(request["cus_name"],200)}%'";

            if (!string.IsNullOrEmpty(request["cus_tel"]))
                serchtxt += $" and cus_tel like N'%{PageValidate.InputText(request["cus_tel"], 50)}%'";

            if (!string.IsNullOrEmpty(request["emp_id"]))
                serchtxt += $" and emp_id =  '{PageValidate.InputText(request["emp_id"], 50)}'";

            if (!string.IsNullOrEmpty(request["City_id"]))
                serchtxt += $" and City_id =  '{PageValidate.InputText(request["City_id"], 50)}'";

            if (!string.IsNullOrEmpty(request["cus_type_id"]))
                serchtxt += $" and cus_type_id =  '{PageValidate.InputText(request["cus_type_id"], 50)}'";

            if (!string.IsNullOrEmpty(request["cus_level_id"]))
                serchtxt += $" and cus_level_id =  '{PageValidate.InputText(request["cus_level_id"], 50)}'";

            if (!string.IsNullOrEmpty(request["cus_source_id"]))
                serchtxt += $" and cus_source_id =  '{PageValidate.InputText(request["cus_source_id"], 50)}'";

            if (!string.IsNullOrEmpty(request["cus_industry_id"]))
                serchtxt += $" and cus_industry_id =  '{PageValidate.InputText(request["cus_industry_id"], 50)}'";

            //权限
            serchtxt += Auth();

            DataSet ds = customer.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;

           
        }

        public string form()
        {
            string id = PageValidate.InputText(request["id"], 50);

            if (!PageValidate.checkID(id))
                return "{}";

            DataSet ds = customer.GetList($" id = '{id}'  and isDelete=0 {Auth()}");
            return DataToJson.DataToJSON(ds);
        }

        public string save()
        {
            model.cus_name = PageValidate.InputText(request["cus_name"], 250);
            model.cus_add = PageValidate.InputText(request["cus_add"], 250);
            model.cus_tel = PageValidate.InputText(request["cus_tel"], 250);
            model.cus_fax = PageValidate.InputText(request["cus_fax"], 250);
            model.cus_website = PageValidate.InputText(request["cus_website"], 250);
            model.cus_industry_id = PageValidate.InputText(request["cus_industry_id"], 50);
            model.Provinces_id = PageValidate.InputText(request["Provinces_id"], 50);
            model.City_id = PageValidate.InputText(request["City_id"], 50);
            model.cus_type_id = PageValidate.InputText(request["cus_type_id"], 50);
            model.cus_level_id = PageValidate.InputText(request["cus_level_id"], 50);
            model.cus_source_id = PageValidate.InputText(request["cus_source_id"], 50);
            model.DesCripe = PageValidate.InputText(request["DesCripe"], int.MaxValue);
            model.Remarks = PageValidate.InputText(request["Remarks"], int.MaxValue);
            model.emp_id = PageValidate.InputText(request["emp_id"], 50);
            model.isPrivate = int.Parse(request["isPrivate"]);

            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                model.id = id;

                DataSet ds = customer.GetList($"id = '{id}' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                if (uid != "admin")
                {
                    //controll auth                    
                     var getauth = new GetAuthorityByUid();
                    bool  candel = getauth.GetBtnAuthority(emp_id.ToString(), "d1e3966e-2dbd-482b-924c-27042e329863");
                    if (!candel)
                        return XhdResult.Error("您没有修改权限，请联系管理员！").ToString();
                }

                //判断公客权限
                if (dr["isPrivate"].ToString() == "1" && uid != "admin")
                {
                    var dataauth = new GetDataAuth();
                    bool canEdit = dataauth.getPrivateCusEdit(emp_id);

                    if (!canEdit)
                        return XhdResult.Error("您不具备公客的修改权限！").ToString();

                }



                bool isupdate = customer.UpdateApp(model);

                if (!isupdate) return XhdResult.Error("更新失败！").ToString();

                string logcontent = "";
                logcontent += Syslog.get_log_content(dr["cus_name"].ToString(), request["cus_name"], "客户名", dr["cus_name"].ToString(), request["cus_name"]);
                logcontent += Syslog.get_log_content(dr["cus_add"].ToString(), request["cus_add"], "地址", dr["cus_add"].ToString(), request["cus_add"]);
                logcontent += Syslog.get_log_content(dr["cus_tel"].ToString(), request["cus_tel"], "电话", dr["cus_tel"].ToString(), request["cus_tel"]);
                logcontent += Syslog.get_log_content(dr["cus_fax"].ToString(), request["cus_fax"], "传真", dr["cus_fax"].ToString(), request["cus_fax"]);
                logcontent += Syslog.get_log_content(dr["cus_website"].ToString(), request["cus_website"], "网址", dr["cus_website"].ToString(), request["cus_website"]);
                logcontent += Syslog.get_log_content(dr["cus_industry_id"].ToString(), request["cus_industry"], "行业", dr["cus_industry"].ToString(), request["cus_industry"]);
                logcontent += Syslog.get_log_content(dr["Provinces_id"].ToString(), request["Provinces_id"], "省份", dr["Provinces"].ToString(), request["Provinces"]);
                logcontent += Syslog.get_log_content(dr["City_id"].ToString(), request["City_id"], "城市", dr["City"].ToString(), request["City"]);
                logcontent += Syslog.get_log_content(dr["cus_type_id"].ToString(), request["cus_type_id"], "客户类别", dr["cus_type"].ToString(), request["cus_type"]);
                logcontent += Syslog.get_log_content(dr["cus_level_id"].ToString(), request["cus_level_id"], "客户级别", dr["cus_level"].ToString(), request["cus_level"]);
                logcontent += Syslog.get_log_content(dr["cus_source_id"].ToString(), request["cus_source_id"], "客户来源", dr["cus_source"].ToString(), request["cus_source"]);
                logcontent += Syslog.get_log_content(dr["DesCripe"].ToString(), request["DesCripe"], "客户描述", dr["DesCripe"].ToString(), request["DesCripe"]);
                logcontent += Syslog.get_log_content(dr["Remarks"].ToString(), request["Remarks"], "备注", dr["Remarks"].ToString(), request["Remarks"]);
                logcontent += Syslog.get_log_content(dr["emp_id"].ToString(), request["emp_id"], "归属", dr["employee"].ToString(), request["employee"]);
                logcontent += Syslog.get_log_content(dr["isPrivate"].ToString(), request["isPrivate"], "公私", dr["isPrivate"].ToString() == "0" ? "私客" : "公客", request["isPrivate"]);

                if (!string.IsNullOrEmpty(logcontent))
                {
                    //日志

                    string UserID = emp_id;
                    string UserName = emp_name;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.cus_name;
                    string EventType = "[APP]客户修改";
                    string EventID = model.id;

                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, logcontent);
                }

                return XhdResult.Success().ToString();
            }
            else
            {
                id = Guid.NewGuid().ToString();
                model.id = id;
                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                model.isDelete = 0;

                if (uid != "admin")
                {
                    //controll auth                    
                    var getauth = new GetAuthorityByUid();
                    bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "00b8e3c9-7c68-430b-bbce-a660f435a76a");
                    if (!candel)
                        return XhdResult.Error("您没有新增权限，请联系管理员！").ToString();
                }

                bool isadd = customer.Add(model);

                if (!isadd) return XhdResult.Error("添加失败！").ToString();               

                return XhdResult.Success().ToString();
            }
        }
        //预删除
        public string AdvanceDelete()
        {
            string id = PageValidate.InputText(request["id"], 50);
            DataSet ds = customer.GetList($"id='{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，找不到数据！").ToString();

            var contact = new BLL.CRM_Contact();
            //var contract = new BLL.CRM_contract();
            var order = new BLL.Sale_order();
            var follow = new BLL.CRM_follow();

            int contactcount = 0, contractcount = 0, followcount = 0, ordercount = 0;
            contactcount = contact.GetList($"customer_id = '{id}'").Tables[0].Rows.Count;
            //contractcount = contract.GetList($"Customer_id= '{id}'").Tables[0].Rows.Count;
            followcount = follow.GetList($" Customer_id= '{id}'").Tables[0].Rows.Count;
            ordercount = order.GetList($" Customer_id= '{id}'").Tables[0].Rows.Count;

            string returntxt = "此客户已放入回收站。 ";
            if (contactcount > 0 || contractcount > 0 || followcount > 0 || ordercount > 0)
            {
                returntxt += "【注意】此客户下含有 ";
                if (contactcount > 0) returntxt += $"【{contactcount}】联系人 ";
                if (followcount > 0) returntxt += $"【{followcount}】跟进 ";
                if (ordercount > 0) returntxt += $"【{ordercount}】订单 ";
                if (contractcount > 0) returntxt += $"【{contractcount}】合同 ";
                returntxt += "，如需彻底删除此客户，需将这些数据删除。 ";
            }

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "bcdedd22-d9c5-4c74-b042-f0a75a4a35be");
                if (!candel)
                    return XhdResult.Error("无权限！").ToString();

                //dataauth
                var dataauth = new GetDataAuth();
                DataAuth auth = dataauth.getAuth(emp_id);
                string authid = ds.Tables[0].Rows[0]["emp_id"].ToString();
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


            bool isdel = customer.AdvanceDelete(id, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (!isdel) return XhdResult.Error("系统错误，请检查参数！").ToString();

            //日志
            string EventType = "[APP]客户预删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["cus_name"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success(returntxt).ToString();
        }

        public string count()
        {
            string id = PageValidate.InputText(request["id"], 50);

            cusInfo ci = new cusInfo();
            if (!PageValidate.checkID(id))
            {
                ci.log = 0;
                ci.contact = 0;
                ci.follow = 0;
                ci.order = 0;
                ci.receivable = 0;

                
            }
            else
            {
                ci.log = log.GetList($"EventID = '{id}'").Tables[0].Rows.Count;
                ci.contact = contact.GetList($"customer_id = '{id}'").Tables[0].Rows.Count;
                ci.follow = follow.GetList($"customer_id = '{id}'").Tables[0].Rows.Count;
                ci.order = order.GetList($"Sale_order.Customer_id = '{id}'").Tables[0].Rows.Count;
                ci.receivable = receivable.GetList($"CRM_Customer.id = '{id}'").Tables[0].Rows.Count;
            }
              
             return tojson(ci);
        }

        private string tojson(cusInfo ci)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append($" \"log\":{ci.log},");
            sb.Append($"\"contact\":{ci.contact},");
            sb.Append($"\"follow\":{ci.follow},");
            sb.Append($"\"order\":{ci.order},");
            sb.Append($"\"receivable\":{ci.receivable}");
            sb.Append("}");

            return sb.ToString();
        }

        public string getMapList()
        {
            string serchtxt = $" isDelete=0 and ( xy is not null and xy <>'' ) ";
            //权限
            // serchtxt += Auth();
            DataSet ds = customer.GetMapList(serchtxt);
            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }

        private string Auth()
        {
            GetDataAuth dataauth = new GetDataAuth();
            DataAuth auth = dataauth.getAuth(emp_id);

            switch (auth.authtype)
            {
                case 0: return " and isPrivate = 1 ";
                case 1:
                case 2:
                case 3:
                case 4:
                    return $" and emp_id in ({auth.authtext}) ";
                case 5: return "";
            }

            return auth.authtype + ":" + auth.authtext;
        }
    }

    public class cusInfo
    {
        public int log { get; set; }
        public int contact { get; set; }
        public int follow { get; set; }
        public int order { get; set; }
        public int receivable { get; set; }
    }
}
