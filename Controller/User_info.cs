using System.Web;
using System.Web.Security;
using XHD.Model;

namespace XHD.Controller
{
    public class User_info
    {
        public hr_employee GetCurrentEmpInfo(HttpContext context)
        {
            HttpCookie cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            string CoockiesID = ticket.UserData;

            var emp = new BLL.hr_employee();
            hr_employee empmodel = emp.GetModel(CoockiesID);
            return empmodel;
        }

        public hr_employee GetEmpWithToken(HttpContext context)
        {
            string token = context.Request["token"];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);
            string CoockiesID = ticket.UserData;

            var emp = new BLL.hr_employee();
            hr_employee empmodel = emp.GetModel(CoockiesID);
            return empmodel;
        }
    }
}