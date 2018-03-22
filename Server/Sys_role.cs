

using System;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Sys_role
    {
        public static BLL.Sys_role role = new BLL.Sys_role();
        public static Model.Sys_role model = new Model.Sys_role();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public Sys_role()
        {
        }

        public Sys_role(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        //save
        public string SysSave()
        {
            model.RoleName = PageValidate.InputText(request["T_role"], 250);
            model.RoleSort = int.Parse(request["T_RoleOrder"]);
            model.RoleDscript = PageValidate.InputText(request["T_Descript"], 255);

            model.DataAuth = int.Parse(request["T_auth_val"]);
            model.PublicAuth = int.Parse(request["T_public_val"]);

            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                DataSet ds = role.GetList($"id = '{id}' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];
                model.id = id;

                role.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = dr["RoleName"].ToString();

                string EventType = "角色修改";
                string EventID = model.id;
                string Log_Content = null;

                Log_Content += Syslog.get_log_content(dr["RoleName"].ToString(), request["T_role"], "角色名", dr["RoleName"].ToString(), request["T_role"]);
                Log_Content += Syslog.get_log_content(dr["RoleSort"].ToString(), request["T_RoleOrder"], "序号", dr["RoleSort"].ToString(), request["T_RoleOrder"]);
                Log_Content += Syslog.get_log_content(dr["RoleDscript"].ToString(), request["T_Descript"], "描述", dr["RoleDscript"].ToString(), request["T_Descript"]);
                Log_Content += Syslog.get_log_content(dr["DataAuth"].ToString(), request["T_auth_val"], "数据权限", getDataAuthText(dr["DataAuth"].ToString()), getDataAuthText(request["T_auth_val"]));
                Log_Content += Syslog.get_log_content(dr["PublicAuth"].ToString(), request["T_public_val"], "公客修改", dr["PublicAuth"].ToString() == "0" ? "否" : "是", request["T_public"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                
                string rid = Guid.NewGuid().ToString().ToUpper();
                model.id = rid;
                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                role.Add(model);
            }

            return XhdResult.Success().ToString();
        }

        //validate
        public string Exist()
        {
            string ex = PageValidate.InputText(request["T_role"], 250);
            string id = PageValidate.InputText(request["T_cid"], 50);
            DataSet ds1 = role.GetList($" RoleName = '{ ex }' and id != '{id}' ");
            return (ds1.Tables[0].Rows.Count > 0 ? "false" : "true");
        }

        //表格json
        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " RoleSort ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";

            //context.Response.Write(serchtxt);
            DataSet ds = role.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;           
        }

        //Form JSON
        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";

            id = PageValidate.InputText(id, 50);
            DataSet ds = role.GetList($" id = '{id}' ");

            return DataToJson.DataToJSON(ds);

        }

        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            id = PageValidate.InputText(id, 50);

            DataSet ds = role.GetList($"id='{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "2EB328DD-E7F3-426D-BCD3-CA47FB6AAB30");
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }

            bool isdel = role.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();

            //角色下员工删除
            var rm = new BLL.Sys_role_emp();
            rm.Delete($"RoleID='{id}'");

            //角色下数据权限删除
            var dataauth = new BLL.Sys_data_authority();
            dataauth.Delete($"Role_id = '{id}'");

            //角色权限
            var auth = new BLL.Sys_authority();
            auth.Delete($"Role_id = '{id}'");

            //日志
            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["RoleName"].ToString();
            string EventType = "角色删除";

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, EventTitle);

            return XhdResult.Success().ToString();
        }

        //auth
        public string treegrid(string appid)
        {
            var menu = new BLL.Sys_Menu();

            //string dt1 = 
            DataTable dt = menu.GetList(0, $"App_id='{ appid}'", "Menu_order").Tables[0];
            dt.Columns.Add(new DataColumn("Sysroler", typeof(string)));

            var btn = new BLL.Sys_Button();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataSet ds = btn.GetList(0, $"Menu_id='{ dt.Rows[i]["Menu_id"]}'", " Btn_order");
                string roler = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        roler += ds.Tables[0].Rows[j]["Btn_id"] + "|" + ds.Tables[0].Rows[j]["Btn_name"];
                        roler += ",";
                    }
                }
                dt.Rows[i][dt.Columns.Count - 1] = roler;
            }
            string dt1 = "{\"Rows\":[" + GetTasks.GetMenuTree("root", dt) + "]}";
            return dt1;
        }

        //get auth
        public string getauth()
        {
            string roleid = PageValidate.InputText(request["roleid"], 50);
            string appid = PageValidate.InputText(request["appid"], 50);

            BLL.Sys_authority sysau = new BLL.Sys_authority();

            string roledata = "{ \"menus\":[],\"btns\":[] }";
            string menus = "";
            string btns = "";
            DataSet ds = sysau.GetList(string.Format("Role_id='{0}' and App_id ='{1}'", roleid, appid));
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    if (dr["Auth_type"].ToString() == "0")
                    {
                        menus += "\"" + dr["Auth_id"].ToString() + "\",";
                    }
                    else
                    {
                        btns += "\"" + dr["Auth_id"].ToString() + "\",";
                    }
                }
                menus = menus.TrimEnd(',');
                btns = btns.TrimEnd(',');
                roledata = "{ \"menus\":[" + menus + "],\"btns\":[" + btns + "] }";
            }

            return (roledata);
        }

        // save auth
        public void saveauth()
        {
            string postdata = request["postdata"];
            JavaScriptSerializer json = new JavaScriptSerializer();
            save sa = json.Deserialize<save>(postdata);
            Model.Sys_authority modelauth = new Model.Sys_authority();
            modelauth.Role_id = PageValidate.InputText(sa.role_id, 50);
            modelauth.App_id = PageValidate.InputText(sa.app, 50);
            modelauth.create_id = emp_id;
            modelauth.create_time = DateTime.Now;

            string Menu_ids = PageValidate.InputText(sa.menu, int.MaxValue);
            string Button_ids = PageValidate.InputText(sa.btn, int.MaxValue);

            BLL.Sys_authority sysau = new BLL.Sys_authority();

            if (!string.IsNullOrEmpty(postdata))
            {
                sysau.Delete(string.Format("Role_id='{0}' and App_id ='{1}'", modelauth.Role_id, modelauth.App_id));

                Menu_ids = Menu_ids.TrimEnd(',');
                Button_ids = Button_ids.TrimEnd(',');

                string[] menu = Menu_ids.Split(',');
                string[] btn = Button_ids.Split(',');

                for (int i = 0; i < menu.Length; i++)
                {
                    modelauth.Auth_type = 0;
                    modelauth.Auth_id = menu[i];
                    sysau.Add(modelauth);
                }

                for (int j = 0; j < btn.Length; j++)
                {
                    modelauth.Auth_type = 1;
                    modelauth.Auth_id = btn[j];
                    if (!string.IsNullOrEmpty(btn[j]))
                        sysau.Add(modelauth);
                }

                var log = new BLL.Sys_log();
                var modellog = new Model.Sys_log();
               
                modellog.id = Guid.NewGuid().ToString();
                modellog.EventDate = DateTime.Now;
                modellog.UserID = emp_id;
                modellog.IPStreet = request.UserHostAddress;

                modellog.EventType = "权限调整";
                modellog.EventID = modelauth.Role_id;
                log.Add(modellog);
            }
        }

        public string getEmpMenus()
        {
            if (uid == "admin")
            {
                return "{\"menulist\":[\"customer_list\",\"customer_contact\",\"contact_follow\",\"sale_order\",\"finance_receivable\",\"sale_contract\"]}";
            }
            var auth = new GetAuthorityByUid();

            string txt = auth.GetMenus(emp_id.ToString());

            txt = txt.Replace("'", "\"");

            return $"{{\"menulist\":[{txt}]}}";
        }

        [Serializable]
        private class save
        {
            public string role_id { get; set; }
            public string app { get; set; }
            public string menu { get; set; }
            public string btn { get; set; }
        }

        private string getDataAuthText(string authid)
        {
            string returntxt = "";
            switch (authid)
            {
                case "1": returntxt = "本人"; break;
                case "2": returntxt = "本部"; break;
                case "3": returntxt = "本部及下级"; break;
                case "4": returntxt = "指定部门"; break;
                case "5": returntxt = "全部"; break;
            }

            return returntxt;
        }
    }
}