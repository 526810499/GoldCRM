

using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System.Collections.Generic;
using XHD.SMS;

namespace XHD.Server
{
    public class SMS_Helper : BaseCRMServer
    {
        public static BLL.Sys_info info = new BLL.Sys_info();
        public static BLL.CRM_Contact contact = new BLL.CRM_Contact();
        public static Model.CRM_Contact model = new Model.CRM_Contact();
        public static SMSHelper sms = new SMSHelper();

 
        public string SerialNo;
        public string key;


        public SMS_Helper()
        {
        }

        public SMS_Helper(HttpContext context)
       : base(context)
        {

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
