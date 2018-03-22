
using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Sys_role_emp
    {
        public static BLL.Sys_role_emp rm = new BLL.Sys_role_emp();
        public static Model.Sys_role_emp model = new Model.Sys_role_emp();

        private static BLL.hr_employee emp = new BLL.hr_employee();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public Sys_role_emp()
        {
        }

        public Sys_role_emp(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        public void add()
        {
            string rid = PageValidate.InputText(request["role_id"], 50);
            string empids = PageValidate.InputText(request["empids"], int.MaxValue);
            //rm.Delete(string.Format("RoleID={0} and empID in ({1})", int.Parse(rid), empids));
            empids = empids.TrimEnd(',');

            string[] emplist = empids.Split(',');
            model.RoleID =rid;
            

            for (int i = 0; i < emplist.Length; i++)
            {
                model.empID = PageValidate.InputText( emplist[i],50);
                rm.Add(model);
            }

            var log = new BLL.Sys_log();
            var modellog = new Model.Sys_log();

            modellog.id = Guid.NewGuid().ToString();
            modellog.EventDate = DateTime.Now;
            modellog.UserID = emp_id;
            modellog.IPStreet = request.UserHostAddress;

            modellog.EventType = "权限人员调整";
            modellog.EventID = rid;
            log.Add(modellog);
        }

        public void remove()
        {
            string rid = PageValidate.InputText(request["role_id"], 50);
            string empids = PageValidate.InputText(request["empids"], int.MaxValue);
            empids = empids.TrimEnd(',');

            string[] emplist = empids.Split(',');
            string ids = "";
            for (int i = 0; i < emplist.Length; i++)
            {
                ids += $"'{emplist[i]}',";
            }
            ids = ids.TrimEnd(',');

            rm.Delete(string.Format("RoleID='{0}' and empID in ({1})", rid, ids));

            var log = new BLL.Sys_log();
            var modellog = new Model.Sys_log();
            
            modellog.id = Guid.NewGuid().ToString();
            modellog.EventDate = DateTime.Now;
            modellog.UserID = emp_id;
            modellog.IPStreet = request.UserHostAddress;

            modellog.EventType = "权限人员调整";
            modellog.EventID = rid;
            log.Add(modellog);
        }

        public string emplist()
        {
            string rid = PageValidate.InputText(request["role_id"], 50);

            string sql = $"id not in (select empID from Sys_role_emp where RoleID='{rid}') and uid !='admin'  ";
            if (!string.IsNullOrEmpty(request["stext"]))
            {
                sql += " and name like N'%" + PageValidate.InputText(request["stext"], 255) + "%'";
            }

            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " create_time";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;

            DataSet dsrm = emp.GetList(PageSize, PageIndex, sql, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(dsrm.Tables[0], Total);
            return dt;
        }

        public string get()
        {
            string rid = PageValidate.InputText(request["role_id"], 50);
            if (PageValidate.checkID(rid))
            {
                string sql = (string.Format("id in (select empID from Sys_role_emp where RoleID='{0}')", rid));
                if (!string.IsNullOrEmpty(request["stext"]))
                {
                    sql += " and name like N'%" + PageValidate.InputText(request["stext"], 255) + "%'";
                }
                int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                string sortname = request["sortname"];
                string sortorder = request["sortorder"];

                if (string.IsNullOrEmpty(sortname))
                    sortname = " create_time";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = " desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total;

                DataSet ds = emp.GetList(PageSize, PageIndex, sql, sorttext, out Total);

                string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

                return dt;
            }
            return ("test" + rid);
        }
    }
}