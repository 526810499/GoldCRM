
using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using XHD.BLL;
using XHD.Common;
using XHD.Common.DEncrypt;
using XHD.Controller;

namespace XHD.Server
{
    public class login : IRequiresSessionState
    {
        public static BLL.hr_employee emp = new BLL.hr_employee();
        public HttpContext Context;
        public HttpRequest request;

        public login()
        {
        }

        public login(HttpContext context)
        {
            Context = context;
            request = context.Request;
        }

        public string check()
        {
            string username = PageValidate.InputText(request["username"], 255);
            //string password = FormsAuthentication.HashPasswordForStoringInConfigFile(request["password"], "MD5");
            string password = PageValidate.InputText(request["password"], 255);
            string validate = PageValidate.InputText(request["validate"], 255);

            HttpCookie cookie_validate = request.Cookies["XHD_SAAS_Valicode"];
            if (null == cookie_validate) return XhdResult.Error("验证码不存在").ToString();
            FormsAuthenticationTicket ticket_valicode = FormsAuthentication.Decrypt(cookie_validate.Value);
            string valicode = ticket_valicode.UserData;

            if (ticket_valicode.Expired) return XhdResult.Error("验证码已过期").ToString();

            if (!string.IsNullOrEmpty(validate) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (validate.ToLower() != valicode.ToLower())
                    return XhdResult.Error("验证码错误！").ToString(); //验证码错误                      

                DataSet ds = emp.GetList($" uid = '{ username }' and pwd = '{ password } ' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("用户名或密码错误！").ToString();  //用户名或密码错误

                if (ds.Tables[0].Rows[0]["canlogin"].ToString() != "1")
                    return XhdResult.Error("账户已限制登录！").ToString();  //不允许登录

                DataRow rows = ds.Tables[0].Rows[0];

                string userid = rows["ID"].ToString();
                var ticket = new FormsAuthenticationTicket(
                    1,
                    username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(20),
                    true,
                    userid,
                    "/"
                    );
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                cookie.HttpOnly = true;
                cookie.Secure = FormsAuthentication.RequireSSL;
                cookie.Domain = FormsAuthentication.CookieDomain;
                cookie.Path = FormsAuthentication.FormsCookiePath;
                cookie.Expires = DateTime.Now.AddMinutes(20);

                Context.Response.Cookies.Add(cookie);
 
                CookieHelper.Add("udepid", rows["dep_id"].CString(""));
                CookieHelper.Add("udepname", HttpUtility.UrlEncode(rows["dep_name"].CString("")));
                CookieHelper.Add("uid", HttpUtility.UrlEncode(DEncrypt.Encrypt(userid)));
                CookieHelper.Add("urname", HttpUtility.UrlEncode(rows["name"].CString("")));
                //日志
                var log = new BLL.Sys_log();
                var modellog = new Model.Sys_log();
                modellog.EventType = "系统登录";
                modellog.id = Guid.NewGuid().ToString().ToUpper();
                modellog.EventDate = DateTime.Now;
                modellog.UserID = userid;
                modellog.IPStreet = request.UserHostAddress;

                log.Add(modellog);

                //online
                var sol = new Sys_online();
                var model = new Model.Sys_online();

                model.UserName = ds.Tables[0].Rows[0]["name"].ToString();
                model.UserID = PageValidate.InputText(ds.Tables[0].Rows[0]["id"].ToString(), 50);
                model.LastLogTime = DateTime.Now;

                DataSet ds1 = sol.GetList($" UserID='{userid}'");

                //添加当前用户信息
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    sol.Update(userid);
                }
                else
                {
                    sol.Add(model);
                }
                //删除超时用户
                sol.Delete(" LastLogTime<DATEADD(MI,-1,getdate())");

                //验证完毕，允许登录
                return XhdResult.Success().ToString();

            }
            return XhdResult.Error("系统数据错误！").ToString();  //系统数据错误
        }

        public void logout()
        {
            HttpCookie cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (null != cookie)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                string CoockiesID = ticket.UserData;

                FormsAuthentication.SignOut();
                //Context.Response.Write("true");

                //online
                var sol = new Sys_online();
                try
                {
                    if (PageValidate.checkID(CoockiesID))
                    {
                        sol.Delete($" UserID='{PageValidate.InputText(CoockiesID, 50)}'");
                    }
                }
                catch
                {
                }
            }
        }

        public string checkpwd()
        {
            HttpCookie cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            string CoockiesID = ticket.UserData;

            var emp = new BLL.hr_employee();

            int emp_id = int.Parse(CoockiesID);
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(request["password"], "MD5");


            DataSet ds = emp.GetList(string.Format("id='{0}' and pwd='{1}'", emp_id, password));

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ("{sucess:sucess}");
            }
            return ("{sucess:false}");
        }
    }
}