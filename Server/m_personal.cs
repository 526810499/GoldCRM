using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace XHD.Server
{
    public class m_personal : BaseCRMServer
    {
        public static BLL.hr_employee employee = new BLL.hr_employee();
        public static Model.hr_employee model = new Model.hr_employee();

 

        public m_personal()
        {
        }

        public m_personal(HttpContext context) : base(context) { }

        //changepwd
        public string changepwd()
        {
            DataSet ds = employee.GetPWD(emp_id);

            string oldpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(request["oldpwd"], "MD5");
            string newpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(request["newpwd"], "MD5");

            if (ds.Tables[0].Rows[0]["pwd"].ToString() != oldpwd)
                return XhdResult.Error("请输入正确的原密码！").ToString();

            model.pwd = newpwd;
            model.id = (emp_id);
            employee.changepwd(model);
            return XhdResult.Success().ToString();
        }

        //Form JSON
        public string form()
        {
            DataSet ds = employee.GetList($"id='{emp_id}' ");
            string dt = DataToJson.DataToJSON(ds);
            return dt;
        }

    }
}
