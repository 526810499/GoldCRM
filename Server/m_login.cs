using System;
using System.Data;
using System.Web;
using System.Web.Security;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class m_login
    {
        public static BLL.hr_employee emp = new BLL.hr_employee();

        public HttpRequest request;
        public HttpContext Context;
        public string emp_id;
        public string emp_name;        

        public m_login()
        {
        }

        public m_login(HttpContext context)
        {
            Context = context;
            request = context.Request;
        }

        public string check()
        {
            string username = PageValidate.InputText(request["username"], 255);
            //string password = FormsAuthentication.HashPasswordForStoringInConfigFile(request["password"], "MD5");
            string password = PageValidate.InputText(request["password"], 255);

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) )
            {
                password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");

                DataSet ds = emp.GetList($" uid='{ username }' and pwd='{ password } ' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("用户名或密码错误！").ToString();  //用户名或密码错误

                if (ds.Tables[0].Rows[0]["canlogin"].ToString() != "1")
                    return XhdResult.Error("账户已限制登录！").ToString();  //不允许登录

                string userid = ds.Tables[0].Rows[0]["id"].ToString();
                var ticket = new FormsAuthenticationTicket(
                    1,
                    username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(20),
                    true,
                    userid,
                    "/"
                    );
                var token = FormsAuthentication.Encrypt(ticket);

                //日志
                var log = new BLL.Sys_log();
                var modellog = new Model.Sys_log();
                modellog.EventType = "[APP]登录";
                modellog.id = Guid.NewGuid().ToString().ToUpper();
                modellog.EventDate = DateTime.Now;
                modellog.UserID = userid;
                modellog.IPStreet = request.UserHostAddress;

                log.Add(modellog);
              
                return XhdResult.Success(token).ToString();
            }
               
            return XhdResult.Error("输入的数据不正确！").ToString();
        }
    }
}
