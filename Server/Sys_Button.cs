

using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Sys_Button : BaseCRMServer
    {
        public static BLL.Sys_Button btn = new BLL.Sys_Button();
        public static Model.Sys_Button model = new Model.Sys_Button();
 

        public Sys_Button()
        {
        }

        public Sys_Button(HttpContext context) : base(context) { }

        public string GetGrid(string menuid)
        {
            string dt;
            menuid = PageValidate.InputText(menuid, 50);
            if (PageValidate.checkID(menuid, false))
            {
                DataSet ds = btn.GetList(0, $"Menu_id='{menuid}'", "Btn_order");
                dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            }
            else
                dt = "{}";

            return dt;
        }

        public string form(string id)
        {
            string dt;
            if (PageValidate.checkID(id))
            {
                id = PageValidate.InputText(id, 50);
                DataSet ds = btn.GetList($"Btn_id='{id}'");

                dt = DataToJson.DataToJSON(ds);
            }
            else
                dt = "{}";
            return dt;
        }

        public void save()
        {
            model.Menu_id = PageValidate.InputText(request["menuid"], 50);
            model.Btn_name = PageValidate.InputText(request["T_btn_name"], 255);
            model.Btn_icon = PageValidate.InputText(request["T_btn_icon"], 255);
            model.Btn_handler = PageValidate.InputText(request["T_btn_handler"], 255);
            if (PageValidate.IsNumber(request["T_btn_order"]))
                model.Btn_order = int.Parse(request["T_btn_order"]);

            string id = PageValidate.InputText(request["btnid"], 50);

            if (PageValidate.checkID(id))
            {
                model.Btn_id = id;
                btn.Update(model);
            }
            else
            {
                model.Btn_id = Guid.NewGuid().ToString().ToUpper();
                btn.Add(model);
            }
        }

        public string del(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return "false";
            id = PageValidate.InputText(id, 50);

            DataSet ds = btn.GetList($"Btn_id='{id}'");

            bool isdel = btn.Delete(id);
            if (isdel)
            {
                return "true";
            }
            return "false";
        }
    }
}