

using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System.Collections.Generic;
using XHD.SMS;

namespace XHD.Server
{
    public class SMS_Helper
    {
        public static BLL.Sys_info info = new BLL.Sys_info();
        public static BLL.CRM_Contact contact = new BLL.CRM_Contact();
        public static Model.CRM_Contact model = new Model.CRM_Contact();
        public static SMSHelper sms = new SMSHelper();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;

        public string SerialNo;
        public string key;


        public SMS_Helper()
        {
        }

        public SMS_Helper(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);

            DataSet ds = info.GetAllList();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dic.Add(ds.Tables[0].Rows[i]["sys_key"].ToString(), ds.Tables[0].Rows[i]["sys_value"].ToString());
            }
            SerialNo = dic["sms_no"];
            string enkey = dic["sms_key"];
            key = Common.DEncrypt.DESEncrypt.Decrypt(enkey);
        }

        public double getBalance()
        {
            return sms.getBalance(SerialNo, key);
        }
    }
}
