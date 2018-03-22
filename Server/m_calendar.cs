using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;

namespace XHD.Server
{
    public class m_calendar
    {
        public static BLL.Personal_Calendar calendar = new BLL.Personal_Calendar();
        public static Model.Personal_Calendar model = new Model.Personal_Calendar();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public m_calendar()
        {
        }

        public m_calendar(HttpContext context)
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
                sortname = " StartTime ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;

            string serchtxt = $" emp_id='{ emp_id }' ";

            DataSet ds = calendar.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        public string del()
        {
            string id = PageValidate.InputText(request["id"], 50);
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            DataSet ds = calendar.GetList($"id='{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            bool a = calendar.Delete(id);
            if (a)
                return XhdResult.Success().ToString();

            return XhdResult.Error().ToString();
        }


        public string form()
        {
            string id = PageValidate.InputText(request["id"], 50);

            if (!PageValidate.checkID(id))
                return "{}";

            DataSet ds = calendar.GetList($" id = '{id}'  ");
            return DataToJson.DataToJSON(ds);
        }

        public string save()
        {
            model.Subject = PageValidate.InputText(request["Subject"], 4000);
            model.StartTime = DateTime.Parse(request["StartTime"]);
            model.EndTime = DateTime.Parse(request["EndTime"]);

            if (model.StartTime > model.EndTime)
                return XhdResult.Error("开始时间不能大于结束时间！").ToString();

            model.IsAllDayEvent = PageValidate.InputText(request["IsAllDayEvent"], 255).ToLower() == "true" ? true : false;

            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {//return XhdResult.Error("参数错误！").ToString();

                DataSet ds = calendar.GetList($"id = '{id}' ");
                if (ds.Tables[0].Rows.Count < 1)
                    return XhdResult.Error("系统错误，无数据！").ToString();

                model.id = id;

                calendar.Update(model);

            }
            else
            {
                int clientzone = 8;      
                
                model.Category = 3;
                model.emp_id = emp_id;
                model.UPAccount = emp_id.ToString();
                model.UPTime = DateTime.Now;
                model.MasterId = clientzone;
                model.CalendarType = 1;
                model.InstanceType = 0;
               
                model.id = Guid.NewGuid().ToString();

                calendar.Add(model);
            }

            return XhdResult.Success().ToString();
        }
    }
}
