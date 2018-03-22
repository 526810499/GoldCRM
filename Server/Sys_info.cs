

using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using XHD.SMS;

namespace XHD.Server
{
    public class Sys_info
    {
        public static BLL.Sys_info info = new BLL.Sys_info();
        public static Model.Sys_info model = new Model.Sys_info();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;


        public Sys_info()
        {
        }

        public Sys_info(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
        }

        public string grid()
        {
            DataSet ds = info.GetAllList();
            return (GetGridJSON.DataTableToJSON(ds.Tables[0]));
        }

        public string up()
        {
            model.sys_value = PageValidate.InputText(request["name"], int.MaxValue);
            model.sys_key = "sys_name";

            info.Update(model);

            return XhdResult.Success().ToString();
        }

        public void logo(string filename)
        {            
            model.sys_value = "images/logo/" + filename;
            model.sys_key = "sys_logo";

            info.Update(model);
        }

        public string regSMS()
        {
            string SerialNo = PageValidate.InputText(request["T_SerialNo"], 50);
            model.sys_value = SerialNo;
            model.sys_key = "sms_no";
            info.Update(model);

            string key = PageValidate.InputText(request["T_key"], 50);
            model.sys_value = Common.DEncrypt.DESEncrypt.Encrypt(key); 
            model.sys_key = "sms_key";
            info.Update(model);

            SMSHelper sms = new SMSHelper();
            int v = sms.registEx(SerialNo, key, key);

            if (v == 0)
            {
                model.sys_value = "1";
                model.sys_key = "sms_done";
                info.Update(model);

                return XhdResult.Success().ToString();
            }

            var sms_result = SMSHelper.sms_result(v);
            return XhdResult.Error(sms_result).ToString();
        }
    }
}