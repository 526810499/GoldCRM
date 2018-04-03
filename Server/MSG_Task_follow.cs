
using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Task_follow : BaseCRMServer
    {
        public static BLL.Task_follow TaskFollow = new BLL.Task_follow();
        public static Model.Task_follow model = new Model.Task_follow();

 

        public Task_follow()
        {
        }

        public Task_follow(HttpContext context) : base(context) { }
        //save
        public string save()
        {
            model.follow_content = PageValidate.InputText(request["T_follow"], int.MaxValue);

            string id = PageValidate.InputText(request["followid"], 50);

            if (PageValidate.checkID(id))
            {
                model.id = id;

                DataSet ds = TaskFollow.GetList($"id='{id}' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                TaskFollow.Update(model);

                DataRow dr = ds.Tables[0].Rows[0];


                //日志               
                var task = new BLL.Task();
                DataSet ds1 = task.GetList($"id = '{dr["task_id"]}'");

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = ds1.Tables[0].Rows[0]["task_title"].ToString(); ;

                string EventType = "任务跟进修改";
                string EventID = model.id;
                string Log_Content = null;

                Log_Content += Syslog.get_log_content(dr["follow_content"].ToString(), request["T_follow"], "跟进内容", dr["follow_content"].ToString(), request["T_follow"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                string task_id = PageValidate.InputText(request["taskid"], 50);
                string follow_status = request["statuid"];

                if (PageValidate.checkID(task_id))
                {
                    if (follow_status != "0")
                        follow_status = "1";
                    model.follow_id = emp_id;
                    model.task_id = task_id;
                    model.follow_status = int.Parse(follow_status);
                    model.follow_time = DateTime.Now;
                    model.id = Guid.NewGuid().ToString();
                    

                    TaskFollow.Add(model);

                    if (follow_status == "1")
                    {
                        Task task = new Task();
                        task.UpdateStatu(task_id, int.Parse(follow_status));
                    }
                }
            }
            return XhdResult.Success().ToString();
        }
        //serch
        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " follow_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;

            string serchtxt = $" 1=1 ";

            if (!PageValidate.checkID(request["taskid"])) return "{\"Rows\":[],\"Total\":0}";
            serchtxt += $" and task_id= '{PageValidate.InputText(request["taskid"], 50)}'";

            DataSet ds = TaskFollow.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return (dt);
        }
        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);
            DataSet ds = TaskFollow.GetList($"id = '{id}' ");
            return DataToJson.DataToJSON(ds);
        }
        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            bool canDel = false;

            if (uid != "admin")
            {
                var getauth = new GetAuthorityByUid();
                canDel = getauth.GetBtnAuthority(emp_id.ToString(), "B81E9DC4-7367-4F79-81AC-E6044CD5E556");
                if (!canDel) return XhdResult.Error("权限不够！").ToString();
            }
            DataSet ds = TaskFollow.GetList($"id = '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();



            bool isdel = TaskFollow.Delete(id);
            if (!isdel)
                return XhdResult.Error("删除失败，请检查参数！").ToString();

            string EventType = "删除任务跟进";
            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["task_title"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();

        }
    }
}
