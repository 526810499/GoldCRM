
using System.Data;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Sys_App : BaseCRMServer
    {
        public static BLL.Sys_App app = new BLL.Sys_App();
        public static Model.Sys_App model = new Model.Sys_App();
 
        public Sys_App()
        {
        }

        public Sys_App(HttpContext context) : base(context) { }

        public string GetSysApp()
        {
            var getappauth = new GetAuthorityByUid();
            string apps = getappauth.GetAuthority(emp_id.ToString(), "Apps");

            bool BtnAble = false;

            if (uid == "admin")
            {
                BtnAble = true;
            }

            DataSet ds = app.GetList(0, "", "App_order");
            string toolbarscript = "{Items:[";

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                toolbarscript += "{";
                toolbarscript += "type: 'button',";
                toolbarscript += "text: '" + ds.Tables[0].Rows[i]["App_name"] + "',";
                toolbarscript += "icon: '" + ds.Tables[0].Rows[i]["App_icon"] + "',";

                if (BtnAble)
                {
                    toolbarscript += "disable: true,";
                }
                else
                {
                    toolbarscript += "disable: " + getappauth.GetAppAuthority(emp_id.ToString(), ds.Tables[0].Rows[i]["id"].ToString()) + ",";
                }
                toolbarscript += "click: function () {";
                toolbarscript += "f_according('" + ds.Tables[0].Rows[i]["App_no"] + "')";
                toolbarscript += "}";
                toolbarscript += "},";
            }
            toolbarscript = toolbarscript.Substring(0, toolbarscript.Length - 1);
            toolbarscript += "]}";
            return (toolbarscript);
        }

        public string GetAppList()
        {
            var app = new BLL.Sys_App();
            DataSet ds = app.GetList(0, " ", "App_order");

            var str = new StringBuilder();
            str.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append("{\"id\":\"" + ds.Tables[0].Rows[i]["id"] + "\",\"text\":\"" + ds.Tables[0].Rows[i]["App_name"] + "\",\"App_icon\":\"" + ds.Tables[0].Rows[i]["App_icon"] + "\",\"App_order\":\"" + ds.Tables[0].Rows[i]["App_order"] + "\"},");
            }
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");
            return str.ToString();
        }

        public string GetAppListV2()
        {
            var app = new BLL.Sys_App();
            DataSet ds = app.GetList(0, " ", "App_order");

            var str = new StringBuilder();
            str.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append("{\"id\":\"" + ds.Tables[0].Rows[i]["id"] + "\",\"text\":\"" + ds.Tables[0].Rows[i]["App_name"] + "\",\"App_icon\":\"" + ds.Tables[0].Rows[i]["App_icon"] + "\"},");
            }
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");
            return str.ToString();
        }


        //表格json
        public string GridData()
        {
            var app = new BLL.Sys_App();
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " App_order ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";

            //context.Response.Write(serchtxt);
            DataSet ds = app.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }



        public string form(string id)
        {
            var app = new BLL.Sys_App();
            string dt;
            if (PageValidate.checkID(id, false))
            {
                id = PageValidate.InputText(id, 50);
                DataSet ds = app.GetList($"id='{id}'");
                dt = DataToJson.DataToJSON(ds);
            }
            else
                dt = "{}";
            return dt;
        }



        public string save()
        {
            Model.Sys_App model = new Model.Sys_App();
            string hid = PageValidate.InputText(request["hid"], 50);
            model.id = PageValidate.InputText(request["T_menu_id"], 50);
            model.App_name = PageValidate.InputText(request["T_menu_name"], 255);

            model.App_icon = PageValidate.InputText(request["T_menu_icon"], 255);
            model.App_order = int.Parse(request["T_menu_order"]);

            var app = new BLL.Sys_App();

            if (PageValidate.checkID(hid, false))
            {
                app.Update(model);
            }
            else
            {
                app.Add(model);
            }

            return XhdResult.Success().ToString();
        }

        public string delete()
        {
            string id = PageValidate.InputText(request["id"], 50);
            BLL.Sys_Menu menu = new BLL.Sys_Menu();
            var app = new BLL.Sys_App();

            int count = menu.CountMenu(id);
            if (count > 0)
            {
                return XhdResult.Error("请先删除关联的菜单").ToString();
            }
            app.Delete(id);
            return XhdResult.Success().ToString();
        }

    }
}