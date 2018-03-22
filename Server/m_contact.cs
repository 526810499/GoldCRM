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
    public class m_contact
    {
        public static BLL.CRM_Contact contact = new BLL.CRM_Contact();
        public static Model.CRM_Contact model = new Model.CRM_Contact();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public m_contact()
        {
        }

        public m_contact(HttpContext context)
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
            string serchtxt = $"1=1  ";

            if (PageValidate.checkID(request["customer_id"]))
                serchtxt += $" and customer_id ='{PageValidate.InputText(request["customer_id"], 50)}'";

            if (!string.IsNullOrEmpty(request["C_name"]))
                serchtxt += $" and C_name like N'%{PageValidate.InputText(request["C_name"], 200)}%'";

            if (!string.IsNullOrEmpty(request["C_tel"]))
                serchtxt += $" and C_tel like N'%{PageValidate.InputText(request["C_tel"], 200)}%'";

            if (!string.IsNullOrEmpty(request["C_mob"]))
                serchtxt += $" and C_mob like N'%{PageValidate.InputText(request["C_mob"], 200)}%'";

            if (!string.IsNullOrEmpty(request["C_fax"]))
                serchtxt += $" and C_fax like N'%{PageValidate.InputText(request["C_fax"], 200)}%'";

            if (!string.IsNullOrEmpty(request["C_QQ"]))
                serchtxt += $" and C_QQ like N'%{PageValidate.InputText(request["C_QQ"], 200)}%'";

            if (!string.IsNullOrEmpty(request["C_add"]))
                serchtxt += $" and C_add like N'%{PageValidate.InputText(request["C_add"], 200)}%'";

            if (!string.IsNullOrEmpty(request["C_hobby"]))
                serchtxt += $" and C_hobby like N'%{PageValidate.InputText(request["C_hobby"], 200)}%'";

            if (!string.IsNullOrEmpty(request["C_remarks"]))
                serchtxt += $" and C_remarks like N'%{PageValidate.InputText(request["C_remarks"], 200)}%'";

            //权限  
            serchtxt += $" and customer_id in (select id from CRM_Customer where 1=1 { Auth() })";

            DataSet ds = contact.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        public string form()
        {
            string id = PageValidate.InputText(request["id"], 50);

            if (!PageValidate.checkID(id))
                return "{}";

            DataSet ds = contact.GetList($" id = '{id}'   and customer_id in (select id from CRM_Customer where 1=1 { Auth() })");
            return DataToJson.DataToJSON(ds);
        }

        public string save()
        {
            model.C_name = PageValidate.InputText(request["C_name"], 250);
            model.C_tel = PageValidate.InputText(request["C_tel"], 250);
            model.C_mob = PageValidate.InputText(request["C_mob"], 250);
            model.C_sex = int.Parse(request["C_sex"]);
            model.C_add = PageValidate.InputText(request["C_add"], 250);
            model.C_fax = PageValidate.InputText(request["C_fax"], 50);
            model.C_department = PageValidate.InputText(request["C_department"], 50);
            model.C_position = PageValidate.InputText(request["C_position"], 50);
            model.C_QQ = PageValidate.InputText(request["C_QQ"], 50);

            if (PageValidate.IsDateTime(request["C_birthday"]))
                model.C_birthday = DateTime.Parse(request["C_birthday"]);
            model.C_hobby = PageValidate.InputText(request["C_hobby"], 50);
            model.C_remarks = PageValidate.InputText(request["C_remarks"], int.MaxValue);
            model.customer_id = PageValidate.InputText(request["customer_id"], 50);

            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                model.id = id;

                DataSet ds = contact.GetList($"id = '{id}' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                if (uid != "admin")
                {
                    //controll auth                    
                    var getauth = new GetAuthorityByUid();
                    bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "1114131E-A934-4983-831C-A3FC448E803E");
                    if (!candel)
                        return XhdResult.Error("您没有修改权限，请联系管理员！").ToString();
                }

                bool isupdate = contact.Update(model);

                if (!isupdate) return XhdResult.Error("更新失败！").ToString();

                string logcontent = "";
                logcontent += Syslog.get_log_content(dr["C_name"].ToString(), request["C_name"], "姓名", dr["C_name"].ToString(), request["C_name"]);
                logcontent += Syslog.get_log_content(dr["C_tel"].ToString(), request["C_tel"], "电话", dr["C_tel"].ToString(), request["C_tel"]);
                logcontent += Syslog.get_log_content(dr["C_mob"].ToString(), request["C_mob"], "手机", dr["C_mob"].ToString(), request["C_mob"]);
                logcontent += Syslog.get_log_content(dr["C_sex"].ToString(), request["C_sex"], "性别", dr["C_sex"].ToString(), request["C_sex"]);
                logcontent += Syslog.get_log_content(dr["C_add"].ToString(), request["C_add"], "地址", dr["C_add"].ToString(), request["C_add"]);
                logcontent += Syslog.get_log_content(dr["C_fax"].ToString(), request["C_fax"], "微信", dr["C_fax"].ToString(), request["C_fax"]);
                logcontent += Syslog.get_log_content(dr["C_department"].ToString(), request["C_department"], "部门", dr["C_department"].ToString(), request["C_department"]);
                logcontent += Syslog.get_log_content(dr["C_position"].ToString(), request["C_position"], "职务", dr["C_position"].ToString(), request["C_position"]);
                logcontent += Syslog.get_log_content(dr["C_QQ"].ToString(), request["C_QQ"], "QQ", dr["C_QQ"].ToString(), request["C_QQ"]);
                logcontent += Syslog.get_log_content(dr["C_birthday"].ToString(), request["C_birthday"], "生日", dr["C_birthday"].ToString(), request["C_birthday"]);
                logcontent += Syslog.get_log_content(dr["C_hobby"].ToString(), request["C_hobby"], "爱好", dr["C_hobby"].ToString(), request["C_hobby"]);
                logcontent += Syslog.get_log_content(dr["C_remarks"].ToString(), request["C_remarks"], "备注", dr["C_remarks"].ToString(), request["C_remarks"]);
                logcontent += Syslog.get_log_content(dr["customer_id"].ToString(), request["customer_id"], "所属客户", dr["customer"].ToString(), request["customer"]);


                if (!string.IsNullOrEmpty(logcontent))
                {
                    //日志

                    string UserID = emp_id;
                    string UserName = emp_name;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.C_name;
                    string EventType = "[APP]联系人修改";
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

                if (uid != "admin")
                {
                    //controll auth                    
                    var getauth = new GetAuthorityByUid();
                    bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "2B22EF84-A2EF-4FC1-B1A3-5F548BD81A48");
                    if (!candel)
                        return XhdResult.Error("您没有新增权限，请联系管理员！").ToString();
                }

                bool isadd = contact.Add(model);

                if (!isadd) return XhdResult.Error("添加失败！").ToString();

                return XhdResult.Success().ToString();
            }
        }
        
        //62BD8016-4684-4977-BC77-AB10F51432C4 del

        //删除
        public string del()
        {
            string id = PageValidate.InputText(request["id"], 50);
            DataSet ds = contact.GetList($"id='{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，找不到数据！").ToString();           

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "62BD8016-4684-4977-BC77-AB10F51432C4");
                if (!candel)
                    return XhdResult.Error("无权限！").ToString();
            }


            bool isdel = contact.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，请检查参数！").ToString();

            //日志
            string EventType = "[APP]联系人删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["C_name"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();


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
                    return $" and ( emp_id in ({auth.authtext}) or isPrivate =1)";
                case 5: return "";
            }

            return auth.authtype + ":" + auth.authtext;
        }
    }
}
