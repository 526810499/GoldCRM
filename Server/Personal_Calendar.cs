
using System;
using System.Data;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Personal_Calendar
    {
        public static BLL.Personal_Calendar calendar = new BLL.Personal_Calendar();
        public static Model.Personal_Calendar model = new Model.Personal_Calendar();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public Personal_Calendar()
        {
        }

        public Personal_Calendar(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }


        public string get()
        {
            var viewType = (CalendarViewType)Enum.Parse(typeof(CalendarViewType), request["viewtype"]);
            string strshowday = request["showdate"];
            int clientzone = Convert.ToInt32(request["timezone"]);
            int serverzone = GetTime.GetTimeZone();

            int zonediff = serverzone - clientzone;

            var format = new CalendarViewFormat(viewType, DateTime.Parse(strshowday), DayOfWeek.Monday);

            DataSet ds = calendar.GetList($"emp_id='{ emp_id }' and StartTime>='{format.StartDate.ToString("yyyy-MM-dd HH:mm:ss")}' and EndTime<='{format.EndDate.ToString("yyyy-MM-dd HH:mm:ss") }' ");
            string dt = DataToJSON(ds);

            var data = new JsonCalendarViewData(calendar.DataTableToList(ds.Tables[0]), format.StartDate, format.EndDate);
            return ("{\"start\":\"\\/Date(" + GetTime.MilliTimeStamp(format.StartDate) + ")\\/\",\"end\":\"\\/Date(" +
                    GetTime.MilliTimeStamp(format.EndDate) + ")\\/\",\"error\":null,\"issort\":true,\"events\":[" + dt +
                    "]}");
            //context.Response.Write(dt);
        }

        public string quickadd()
        {
            int clientzone = Convert.ToInt32(request["timezone"]);
            int serverzone = GetTime.GetTimeZone();
            int zonediff = serverzone - clientzone;

            model.Subject = PageValidate.InputText(request["CalendarTitle"], 4000);
            model.StartTime = DateTime.Parse(request["CalendarStartTime"]).AddHours(zonediff);
            model.EndTime = DateTime.Parse(request["CalendarEndTime"]).AddHours(zonediff);
            model.IsAllDayEvent = PageValidate.InputText(request["IsAllDayEvent"], 255) == "1" ? true : false;

            model.CalendarType = 1;
            model.InstanceType = 0;

            model.UPAccount = emp_id.ToString();
            model.UPTime = DateTime.Now;
            model.MasterId = clientzone;

            model.emp_id = emp_id;
            
            model.Category = 3;
            var id = Guid.NewGuid().ToString();
            model.id = id;

            calendar.Add(model);

            return ("{\"IsSuccess\":true,\"Msg\":\"\u64cd\u4f5c\u6210\u529f!\",\"Data\":\"" + id + "\"}");
        }

        public string quickupdate()
        {
            string calendarId = PageValidate.InputText(request["calendarId"], 50);
            if (!PageValidate.checkID(calendarId)) return XhdResult.Error("参数错误！").ToString();

            DataSet ds = calendar.GetList($"id = '{calendarId}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            //DataSet ds = calendar.GetList()

            int clientzone = Convert.ToInt32(request["timezone"]);
            int serverzone = GetTime.GetTimeZone();
            int zonediff = serverzone - clientzone;

            model.StartTime = DateTime.Parse(request["CalendarStartTime"]).AddHours(zonediff);
            model.EndTime = DateTime.Parse(request["CalendarEndTime"]).AddHours(zonediff);

            model.UPAccount = emp_id.ToString();
            model.UPTime = DateTime.Now;
            model.MasterId = clientzone;

            model.id = calendarId;

            calendar.quickUpdate(model);

            return ("{IsSuccess:true}");
        }

        public string quickdel(string calendarId)
        {
            if (!PageValidate.checkID(calendarId)) return XhdResult.Error("参数错误！").ToString();
            calendarId = PageValidate.InputText(calendarId, 50);

            DataSet ds = calendar.GetList($"id = '{calendarId}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            bool isDelete = calendar.Delete(calendarId);

            if (!isDelete) return XhdResult.Error().ToString();
            return ("{IsSuccess:true}");
        }



        public string save()
        {
            string calendarId = PageValidate.InputText(request["calendarId"], 50);
            if (!PageValidate.checkID(calendarId)) return XhdResult.Error("参数错误！").ToString();

            DataSet ds = calendar.GetList($"id = '{calendarId}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            int clientzone = 8;
            int serverzone = GetTime.GetTimeZone();
            int zonediff = serverzone - clientzone;

            model.StartTime = DateTime.Parse(request["T_starttime"]).AddHours(zonediff);
            model.EndTime = DateTime.Parse(request["T_endtime"]).AddHours(zonediff);

            model.Subject = PageValidate.InputText(request["T_content"], 4000);

            
            model.Category = 3;
            model.emp_id = emp_id;
            model.UPAccount = emp_id.ToString();
            model.UPTime = DateTime.Now;
            model.MasterId = clientzone;
            model.CalendarType = 1;
            model.InstanceType = 0;
            model.IsAllDayEvent = PageValidate.InputText(request["allday"], 255) == "True" ? true : false;

            model.id = calendarId;

            calendar.Update(model);

            return XhdResult.Success().ToString();
        }

        public string Today()
        {
            DateTime starttime = DateTime.Parse(DateTime.Now.ToShortDateString() + " 00:00:00");
            DateTime endtime = DateTime.Parse(DateTime.Now.AddDays(1).ToShortDateString() + " 00:00:00");

            //DataSet ds = calendar.GetList(0, "datediff(day,[StartTime],getdate())=0 and datediff(day,[EndTime],getdate())=0 and emp_id=" + int.Parse(emp_id), "[StartTime] desc");

            DataSet ds = calendar.GetList(0,$"'{ DateTime.Now.ToShortDateString() } 23:59:50' >= [StartTime] and '{DateTime.Now.ToShortDateString()} 0:00:00' <= [EndTime] and emp_id='{ emp_id}' ", "[StartTime] desc");
            return (GetGridJSON.DataTableToJSON(ds.Tables[0]));
        }

        public string form(string calendarid)
        {
            if (!PageValidate.checkID(calendarid)) return "{}";
            calendarid = PageValidate.InputText(calendarid, 50);
            DataSet ds = calendar.GetList($"id = '{calendarid}' ");
            return DataToJson.DataToJSON(ds);
        }


        private static string DataToJSON(DataSet ds)
        {
            var JsonString = new StringBuilder();
            DataTable dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("[");
                    JsonString.Append("\"" + (dt.Rows[i]["id"]) + "\",");
                    JsonString.Append("\"" + HttpUtility.UrlEncode((dt.Rows[i]["Subject"].ToString())) + "\",");
                    JsonString.Append("\"\\/Date(" + GetTime.MilliTimeStamp(DateTime.Parse(dt.Rows[i]["StartTime"].ToString())) + ")\\/\",");
                    JsonString.Append("\"\\/Date(" + GetTime.MilliTimeStamp(DateTime.Parse(dt.Rows[i]["EndTime"].ToString())) + ")\\/\",");
                    JsonString.Append("" + (dt.Rows[i]["IsAllDayEvent"].ToString() == "True" ? 1 : 0) + ",");
                    JsonString.Append("" + (dt.Rows[i]["StartTime"].ToString() == dt.Rows[i]["StartTime"].ToString() ? 0 : 1) + ",");
                    JsonString.Append("" + (dt.Rows[i]["InstanceType"].ToString() == "2" ? 1 : 0) + ",");
                    JsonString.Append("" + (dt.Rows[i]["Category"].ToString() == "4" ? "4" : "3") + ",");
                    //JsonString.Append("3,");
                    JsonString.Append("1,\"" + dt.Rows[i]["customer_id"] + "\",\"\"");

                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("]");
                    }
                    else
                    {
                        JsonString.Append("],");
                    }
                }
                return JsonString.ToString();
            }
            return null;
        }
    }
}