using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;

namespace XHD.Server
{
    public class m_note
    {
        public static BLL.Personal_notes note = new BLL.Personal_notes();
        public static Model.Personal_notes model = new Model.Personal_notes();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public m_note()
        {
        }

        public m_note(HttpContext context)
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
                sortname = " note_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;

            string serchtxt = $" emp_id='{ emp_id }' ";

            DataSet ds = note.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }        

        public string del()
        {
            string id = PageValidate.InputText(request["id"], 50);
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            DataSet ds = note.GetList($"id='{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            bool a = note.Delete(id);
            if (a)
                return XhdResult.Success().ToString();

            return XhdResult.Error().ToString();
        }

        public string save()
        {
            Random rnd = new Random();
            model.note_content = PageValidate.InputText(request["note_content"], 200);
            model.emp_id = emp_id;
            model.note_color = "yellow";
            model.xyz = $"{ rnd.Next(1, 800) },{ rnd.Next(1, 600) },1";
            model.note_time = DateTime.Now;
            model.id = Guid.NewGuid().ToString();

            note.Add(model);

            return XhdResult.Success().ToString();
        }
    }
}
