
using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Task
    {
        public static BLL.Task task = new BLL.Task();
        public static Model.Task model = new Model.Task();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public Task()
        {

        }

        public Task(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);
            DataSet ds = task.GetList($"id = '{id}' ");
            return DataToJson.DataToJSON(ds);
        }

        public string save()
        {
            model.task_title = PageValidate.InputText(request["T_task_name"], 255);

            if (PageValidate.checkID(request["T_task_type_val"]))
                model.task_type_id = PageValidate.InputText(request["T_task_type_val"], 50);

            if (PageValidate.checkID(request["T_customer_val"]))
                model.customer_id = PageValidate.InputText(request["T_customer_val"], 50);

            if (PageValidate.checkID(request["T_executive_val"]))
                model.executive_id = PageValidate.InputText(request["T_executive_val"], 50);

            if (PageValidate.IsNumber(request["T_status_val"]))
                model.task_status_id = int.Parse(request["T_status_val"]);

            if (PageValidate.IsNumber(request["T_priority_val"]))
                model.Priority_id = int.Parse(request["T_priority_val"]);

            if (PageValidate.IsDateTime(request["T_executive_time"]))
                model.executive_time = DateTime.Parse(request["T_executive_time"]);

            model.task_content = PageValidate.InputText(request["T_content"], int.MaxValue);


            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                DataSet ds = task.GetList($"id='{id}' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                model.id = id;

                task.Update(model);

                //日志               

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.task_title;

                string EventType = "任务修改";
                string EventID = model.id;
                string Log_Content = null;

                if (dr["task_title"].ToString() != request["T_task_name"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "任务名称", dr["task_title"], request["T_task_name"]);

                if (dr["task_type"].ToString() != request["T_task_type"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "任务类别", dr["task_type"], request["T_task_type"]);

                if (dr["customer"].ToString() != request["T_customer"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "关联客户", dr["customer"], request["T_customer"]);

                if (dr["executive"].ToString() != request["T_executive"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "执行人", dr["executive"], request["T_executive"]);

                if (dr["task_status_id"].ToString() != request["T_status_val"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "任务状态", dr["task_status_id"], request["T_status_val"]);

                if (dr["Priority_id"].ToString() != request["T_priority_val"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "优先级", dr["Priority_id"], request["T_priority_val"]);

                if (dr["executive_time"].ToString() != DateTime.Parse(request["T_executive_time"]).ToString())
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "执行时间", dr["executive_time"], request["T_executive_time"]);

                if (dr["task_content"].ToString() != request["T_content"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "任务描述", "任务内容改变", null);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                
                model.id = Guid.NewGuid().ToString();
                model.assign_id = emp_id;
                model.create_id = emp_id;
                model.create_time = DateTime.Now;

                task.Add(model);
            }
            return XhdResult.Success().ToString();
        }

        public void stopTask(string id)
        {
            UpdateStatu(id, 2);
        }

        public void UpdateStatu(string id, int status)
        {
            model.id = id;
            model.task_status_id = status;

            task.UpdateStatu(model);
        }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " create_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";

            if (PageValidate.checkID(request["executive_employee_val"]))
                serchtxt += $" and executive_id = '{PageValidate.InputText(request["executive_employee_val"], 50)}'";

            if (PageValidate.IsNumber(request["assign_employee_val"]))
                serchtxt += string.Format(" and assign_id = {0}", int.Parse(request["assign_employee_val"]));

            if (PageValidate.IsNumber(request["task_status_val"]))
                serchtxt += string.Format(" and task_status_id = {0}", int.Parse(request["task_status_val"]));

            if (!string.IsNullOrEmpty(request["taskname"]))
                serchtxt += string.Format(" and task_title like N'%{0}%'", PageValidate.InputText(request["taskname"], 255));

            if (PageValidate.IsDateTime(request["startdate"]))
                serchtxt += string.Format(" and executive_time >= '{0}' ", PageValidate.InputText(request["startdate"], 255));

            if (PageValidate.IsDateTime(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += string.Format(" and executive_time <= '{0}' ", enddate);
            }

            if (PageValidate.IsDateTime(request["startcreate"]))
                serchtxt += string.Format(" and create_time >= '{0}' ", PageValidate.InputText(request["startcreate"], 255));

            if (PageValidate.IsDateTime(request["endcreate"]))
            {
                DateTime enddate = DateTime.Parse(request["endcreate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += string.Format(" and create_time <= '{0}' ", enddate);
            }
            //权限     
            string requesttype = request["type"];
            if (requesttype == "assign")
                serchtxt += string.Format(" and assign_id='{0}'", emp_id);
            else if (requesttype == "executive")
                serchtxt += string.Format(" and executive_id='{0}'", emp_id);

            //return (serchtxt);

            DataSet ds = task.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;

        }

        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            bool canDel = false;

            if (uid != "admin")
            {
                var getauth = new GetAuthorityByUid();

                //权限共享，双重检测
                canDel = getauth.GetBtnAuthority(emp_id.ToString(), "9833C0E4-6010-4C35-8B0F-F938988D7727");
                if (!canDel) canDel = getauth.GetBtnAuthority(emp_id.ToString(), "ACDB8A22-7330-4F0B-9E1B-5700CAC234DE");
                if (!canDel) return XhdResult.Error("权限不够！").ToString();
            }
            DataSet ds = task.GetList($"id = '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            bool isdel = task.Delete(id);

            if (!isdel)
                return XhdResult.Error("删除失败，请检查参数！").ToString();

            string EventType = "删除任务";
            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["task_title"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            var follow = new BLL.Task_follow();
            follow.DeleteWhere($"task_id= '{id}'");
            return XhdResult.Success().ToString();


        }

        public string TaskRemind()
        {
            DataSet ds = task.GetList(7, $"executive_id= '{emp_id}'", "executive_time desc");
            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }
    }
}
