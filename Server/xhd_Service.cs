
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using XHD.BLL;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class xhd_Service
    {
        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;

        public xhd_Service()
        {
        }

        public xhd_Service(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
        }

        /// <summary>
        /// 建议
        /// </summary>
        /// <returns></returns>
        public int suggest()
        {
            return 1;
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <returns></returns>
        public string getVersion()
        {
            return "";
        }

    }
}
