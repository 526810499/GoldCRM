using System;
using System.Web.Security;
using System.Data;
using XHD.Common;

namespace XHD.View
{
    public class Global : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            
        }

        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
        }       

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
            Exception objErr = Server.GetLastError().GetBaseException();

            SoftLog.LogStr("Application_Error"+objErr.ToString(), "Application_Error");

            BLL.Sys_log_Err ssle = new BLL.Sys_log_Err();
            Model.Sys_log_Err model = new Model.Sys_log_Err();

            model.id = Guid.NewGuid().ToString().ToUpper();
            model.Err_typeid = 1;
            model.Err_type = "【前台】";
            model.Err_time = DateTime.Now;
            model.Err_url = PageValidate.InputText(Request.Url.ToString(), 500);
            model.Err_message = PageValidate.InputText(objErr.Message, int.MaxValue);
            model.Err_source = PageValidate.InputText(objErr.Source, 500);
            model.Err_trace = PageValidate.InputText(objErr.StackTrace, int.MaxValue);
            model.Err_ip = Request.UserHostAddress;

            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];



            //检验Cookie是否已经存在 
            if (null == cookie)
            {
                model.Err_emp_id = "-1";
                model.Err_emp_name = "未登录";
            }
            else
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                string CoockiesID = ticket.UserData;
                if (PageValidate.checkID(CoockiesID))
                {
                    BLL.hr_employee emp = new BLL.hr_employee();
                    string emp_id = PageValidate.InputText(CoockiesID,50);
                    DataSet dsemp = emp.GetListWithCompany($"id='{emp_id}'" );

                    if (dsemp.Tables[0].Rows.Count == 0)
                    {
                        model.Err_emp_id = "-1";
                        model.Err_emp_name = "疑似攻击？";
                    }
                    else
                    {

                        string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
                        string company_id = dsemp.Tables[0].Rows[0]["company_id"].ToString();

                        model.Err_emp_id = emp_id;
                        model.Err_emp_name = empname;
                    }
                }
                else
                {
                    model.Err_emp_id = "-1";
                    model.Err_emp_name = "异常登录";
                }
            }

            ssle.Add(model);

            //Server.ClearError();


        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码
        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }
       

    }
}
