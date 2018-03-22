
using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Personal_notes
    {
        public static BLL.Personal_notes notes = new BLL.Personal_notes();
        public static Model.Personal_notes model = new Model.Personal_notes();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public Personal_notes()
        {
        }

        public Personal_notes(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }


        public string Get()
        {
            DataSet ds = notes.GetList($"emp_id = '{emp_id}'" );
            return (GetGridJSON.DataTableToJSON2(ds.Tables[0]));
        }

        public string save()
        {
            model.emp_id = emp_id;
            
            model.note_content = PageValidate.InputText(request["body"], int.MaxValue);
            model.note_time = DateTime.Now;
            model.note_color = PageValidate.InputText(request["color"], 50);
            model.xyz = decimal.Parse(request["left"]) + "," + decimal.Parse(request["top"]) + "," +decimal.Parse(request["zindex"]);
            string id = Guid.NewGuid().ToString();
            model.id = id;
            notes.Add(model);

            return id;
        }

        public string update()
        {
            string id = PageValidate.InputText(request["id"], 50);
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            DataSet ds = notes.GetList($"id='{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            model.xyz = decimal.Parse(request["x"]) + "," + decimal.Parse(request["y"]) + "," + decimal.Parse(request["z"]);
            model.id = id;

            notes.Update(model);

            return XhdResult.Success().ToString();
        }

        public string delete(string id)
        {
            id = PageValidate.InputText(id, 50);
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            DataSet ds = notes.GetList($"id='{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();
            
            bool a = notes.Delete(id);
            return a.ToString();
        }

        public string grid()
        {
            DataSet ds = notes.GetList(0, $"emp_id= '{emp_id}' " , "note_time desc");
            DataTable dt = ds.Tables[0];

            return (GetGridJSON.DataTableToJSON(dt));
        }

        public string notesremind()
        {
            DataSet ds = notes.GetList(7, $"emp_id = '{emp_id}' " , " note_time desc");
            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }
    }
}