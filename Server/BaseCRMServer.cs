using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class BaseCRMServer
    {
        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;

        public BaseCRMServer() { }

        public BaseCRMServer(HttpContext context)
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
        /// 盘点是否管理员
        /// </summary>
        /// <returns></returns>
        protected bool CheckIsAdmin()
        {

            if (uid.ToLower() != "admin")
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取数据库操作语句条件，加入创建者ID
        /// </summary>
        /// <param name="where"></param>
        /// <param name="adminReturnEmpty">如果是管理员是否也要加入创建者id</param>
        /// <returns></returns>
        protected string GetSQLCreateIDWhere(string where, bool adminReturnEmpty)
        {
            if (CheckIsAdmin() && adminReturnEmpty)
            {
                return where;
            }
            else {
                if (string.IsNullOrWhiteSpace(where))
                {
                    where = $" create_id='{ emp_id }'";
                }
                else {
                    where += $" and create_id='{ emp_id }'";
                }
            }

            return where;
        }
    }
}
