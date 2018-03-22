

using System;
using System.Data;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Sys_Menu
    {
        public static BLL.Sys_Menu menu = new BLL.Sys_Menu();
        public static Model.Sys_Menu model = new Model.Sys_Menu();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;


        public Sys_Menu()
        {
        }

        public Sys_Menu(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);

        }

        public string GetMenu(string appid)
        {
            string dt;
            if (PageValidate.checkID(appid, false))
            {
                appid = PageValidate.InputText(appid, 50);
                DataSet ds = menu.GetList(0, $"App_id = '{ appid}' and Menu_id not in ('base_menu','base_button')", "Menu_order");
                dt = "{Rows:[" + GetTasks.GetMenuTree("root", ds.Tables[0]) + "]}";
            }
            else
                dt = "{}";

            return dt;
        }

        public string GetMenuV2(string appid)
        {
            string dt;
            if (PageValidate.checkID(appid, false))
            {
                appid = PageValidate.InputText(appid, 50);
                DataSet ds = menu.GetList(0, $"App_id = '{ appid}' and Menu_id not in ('base_menu','base_button')", "Menu_order");
                dt = "{\"Rows\":[" + GetTasks.GetMenuTree("root", ds.Tables[0]) + "]}";
            }
            else
                dt = "{}";

            return dt;
        }

        public string SysTreeV2(string appid)
        {
            var str = new StringBuilder();
            if (PageValidate.checkID(appid, false))
            {
                appid = PageValidate.InputText(appid, 50);
                DataSet ds = menu.GetList(0, $" parentid = 'root' and App_id='{appid}'", "Menu_order");

                str.Append("[{\"id\":\"root\",\"pid\":\"root\",\"text\":\"无\",\"Menu_icon\":\"\"},");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{\"id\":\"" + ds.Tables[0].Rows[i]["menu_id"] + "\",\"pid\":\"" + ds.Tables[0].Rows[i]["parentid"] + "\",\"text\":\"" + ds.Tables[0].Rows[i]["menu_name"] + "\",\"Menu_icon\":\"" + ds.Tables[0].Rows[i]["Menu_icon"] + "\"},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
            }
            else
            {
                str.Append("[{\"id\":\"root\",\"pid\":\"root\",\"text\":\"无\",\"Menu_icon\":\"\"}]");
            }

            return str.ToString();
        }

        public string form(string id)
        {
            string dt;
            if (PageValidate.checkID(id, false))
            {
                id = PageValidate.InputText(id, 50);
                DataSet ds = menu.GetList($"Menu_id='{id}'");
                dt = DataToJson.DataToJSON(ds);
            }
            else
                dt = "{}";
            return dt;
        }

        public string SysTree(string appid)
        {
            var str = new StringBuilder();
            if (PageValidate.checkID(appid, false))
            {
                appid = PageValidate.InputText(appid, 50);
                DataSet ds = menu.GetList(0, $" parentid = 'root' and App_id='{appid}'", "Menu_order");

                str.Append("[{id:'root',pid:'root',text:'无',Menu_icon:''},");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:'" + ds.Tables[0].Rows[i]["menu_id"] + "',pid:'" + ds.Tables[0].Rows[i]["parentid"] + "',text:'" + ds.Tables[0].Rows[i]["menu_name"] + "',Menu_icon:'" + ds.Tables[0].Rows[i]["Menu_icon"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
            }
            else
            {
                str.Append("[{id:'root',pid:'root',text:'无',Menu_icon:''}]");
            }

            return str.ToString();
        }

        public void save()
        {
            model.Menu_id = PageValidate.InputText(request["T_menu_id"], 50);
            model.Menu_name = PageValidate.InputText(request["T_menu_name"], 255);
            model.Menu_url = PageValidate.InputText(request["T_menu_url"], 255);
            model.Menu_icon = PageValidate.InputText(request["T_menu_icon"], 255);
            model.Menu_order = int.Parse(request["T_menu_order"]);
            model.Menu_type = "sys";
            model.parentid = PageValidate.InputText(request["T_menu_parent_val"], 50);
            model.App_id = PageValidate.InputText(request["appid"], 50);


            var emp = new BLL.hr_employee();

            string id = PageValidate.InputText(request["menuid"], 50);

            if (PageValidate.checkID(id, false))
            {
                model.Menu_id = id;
                DataSet ds = menu.GetList($"Menu_id='{id}'");
                DataRow dr = ds.Tables[0].Rows[0];

                menu.Update(model);
            }
            else
            {
                menu.Add(model);
            }
        }
    }
}