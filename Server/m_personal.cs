using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace XHD.Server
{
    public class m_personal
    {
        public static BLL.hr_employee employee = new BLL.hr_employee();
        public static Model.hr_employee model = new Model.hr_employee();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee _model;
        public HttpRequest request;
        public string uid;
        

        public m_personal()
        {
        }

        public m_personal(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            _model = userinfo.GetEmpWithToken(context);

            emp_id = _model.id;
            emp_name = PageValidate.InputText(_model.name, 50);
            uid = PageValidate.InputText(_model.uid, 50);
        }

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
