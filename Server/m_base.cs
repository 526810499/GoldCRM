using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using XHD.BLL;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class m_base
    {
        public static BLL.Sys_Menu menu = new BLL.Sys_Menu();
        public static BLL.Sys_info info = new BLL.Sys_info();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public m_base()
        {
        }

        public m_base(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetEmpWithToken(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);            
        }

        public string getMenu()
        {
            DataSet ds = menu.GetList("isMobile = 1");

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);

            return dt;
        }

        public string getUserInfo()
        {
            var hr_emp = new BLL.hr_employee();
            DataSet ds = hr_emp.GetList(string.Format("id = '{0}'", emp_id));

            string dt = DataToJson.DataToJSON(ds);

            return (dt);
        }

        public string getVersion()
        {
            DataSet ds = info.GetList($"Sys_key = 'mob_version'");

            return XhdResult.Success(ds.Tables[0].Rows[0]["Sys_value"].ToString()).ToString();
        }
    }
}
