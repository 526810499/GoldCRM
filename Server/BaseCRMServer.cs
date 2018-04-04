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
        public string dep_id;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        protected string allDataBtnid;
        protected string depDataBtnid;

        public BaseCRMServer() { }

        public BaseCRMServer(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);
            dep_id = employee.dep_id;
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
        /// 检查是否拥有这个权限
        /// </summary>
        /// <param name="btnID"></param>
        /// <returns></returns>
        protected bool CheckBtnAuthority(string btnID)
        {
            if (string.IsNullOrWhiteSpace(btnID)) { return false; }
            return new GetAuthorityByUid().GetBtnAuthority(emp_id, btnID);
        }

        /// <summary>
        /// 盘点是否能查看全部数据
        /// </summary>
        /// <returns></returns>
        protected bool CheckCanQueryAllData()
        {
            return CheckBtnAuthority(allDataBtnid);

        }

        /// <summary>
        /// 判断是否能查看部门数据
        /// </summary>
        /// <returns></returns>
        protected bool CheckCanQueryDepData()
        {
            return CheckBtnAuthority(depDataBtnid);
        }

        /// <summary>
        /// 获取数据库操作语句条件，加入创建者ID
        /// </summary>
        /// <param name="where"></param>
        /// <param name="adminReturnEmpty">如果是管理员是否也要加入创建者id</param>
        /// <param name="right">权限，拥有权限时也可以查看全部</param>
        /// <returns></returns>
        protected string GetSQLCreateIDWhere(string where, bool adminReturnEmpty)
        {
            if (CheckIsAdmin() && adminReturnEmpty)
            {
                return where;
            }

            bool r = false;
            //是否能查看所有数据
            r = CheckCanQueryAllData();
            if (r)
            {
                return where;
            }
            //是否能查看部门数据
            r = CheckCanQueryDepData();
            if (r)
            {
                if (!string.IsNullOrWhiteSpace(where))
                {
                    where += $" and ";
                }
                where += $"  createdep_id='{ dep_id }'";

                return where;
            }
            //以上条件都不行，只能查看自己创建的数据
            if (!string.IsNullOrWhiteSpace(where))
            {
                where += $" and ";
            }
            where += $" and create_id='{ emp_id }'";

            return where;
        }

        /// <summary>
        /// 获取查询数据授权用户ID
        /// </summary>
        /// <param name="ColumnID"></param>
        /// <returns></returns>
        protected string DataAuthUserID(string ColumnID)
        {
            GetDataAuth dataauth = new GetDataAuth();
            DataAuth auth = dataauth.getAuth(emp_id);

            switch (auth.authtype)
            {
                case 0: return " 1=2 ";
                case 1:
                case 2:
                case 3:
                case 4:
                    return $" and {ColumnID} in ({auth.authtext})";
                case 5: return "";
            }

            return auth.authtype + ":" + auth.authtext;
        }


    }
}
