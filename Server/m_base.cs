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
    public class m_base : BaseCRMServer
    {
        public static BLL.Sys_Menu menu = new BLL.Sys_Menu();
        public static BLL.Sys_info info = new BLL.Sys_info();

 

        public m_base()
        {
        }

        public m_base(HttpContext context) : base(context) { }

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
