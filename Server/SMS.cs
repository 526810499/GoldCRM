using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;
using System.IO;
using System.Collections.Generic;
using XHD.SMS;

namespace XHD.Server
{
    public class SMS
    {
        public static BLL.Sys_info info = new BLL.Sys_info();

        public static BLL.SMS sms = new BLL.SMS();
        public static Model.SMS model = new Model.SMS();

        public static BLL.SMS_details sms_details = new BLL.SMS_details();
        public static Model.SMS_details sdmodel = new Model.SMS_details();

        public static SMSHelper smshelper = new SMSHelper();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;


        public string SerialNo;
        public string key;
        public string sms_done;
        public string company;

        public SMS()
        {
        }

        public SMS(HttpContext context)
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
            sms_done = dic["sms_done"];
            company = dic["sys_name"];
        }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " create_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";

            if (PageValidate.checkID(request["customerid"]))
                serchtxt += $" and customer_id='{PageValidate.InputText(request["customerid"], 50)}'";

            //权限   

            //context.Response.Write(serchtxt);
            DataSet ds = sms.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        public string form(string id)
        {
            string dt;
            if (!PageValidate.checkID(id)) return "{}";

            DataSet ds = sms.GetList($"id = '{id}'  ");
            dt = DataToJson.DataToJSON(ds);

            return dt;
        }

        public string save()
        {
            model.sms_title = PageValidate.InputText(request["T_title"], 255);
            model.sms_content = $"【{company}】{PageValidate.InputText(request["T_content"], int.MaxValue)}" ;

            string mobiles = PageValidate.InputText(request["mobiles"], int.MaxValue);
            mobiles = mobiles.Trim().TrimEnd(',');
            model.sms_mobiles = mobiles;

            model.id = Guid.NewGuid().ToString();

            model.create_id = emp_id;
            model.create_time = DateTime.Now;
            model.isSend = 0;

            sms.Add(model);



            string ids = PageValidate.InputText(request["ids"], int.MaxValue);
            ids = ids.Trim().TrimEnd(',');
            string[] arr = ids.Split(',');
            string[] arr1 = mobiles.Split(',');
            sdmodel.sms_id = model.id;

            for (int i = 0; i < arr.Length; i++)
            {
                sdmodel.contact_id = arr[i].ToString();
                sdmodel.mobiles = arr1[i].ToString();
                sms_details.Add(sdmodel);
            }

            return XhdResult.Success().ToString();
        }

        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            id = PageValidate.InputText(request["id"], 50);
            DataSet ds = sms.GetList($"id = '{id}' ");

            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            if (ds.Tables[0].Rows[0]["isSend"].ToString() == "1")
                return XhdResult.Error("此短信已发送，不能再删除！").ToString();

            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "2CD08163-7750-4AFB-9538-516DAA07E596");
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }

            bool isdel = sms.Delete(id);

            if (!isdel)
                return XhdResult.Error("删除失败，请检查参数！").ToString();
            else
            {
                sms_details.Delete($"sms_id = '{id}'");
            }

            //日志
            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["sms_title"].ToString();
            string EventType = "短信删除";

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, ds.Tables[0].Rows[0]["sms_title"].ToString());

            return XhdResult.Success().ToString();
        }

        public string send()
        {
            model.check_id = emp_id;
            model.isSend = 1;
            model.sendtime = DateTime.Now;
            string id = PageValidate.InputText(request["id"], 50);
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            model.id = id;

            DataSet ds = sms.GetList($"id = '{id}'");
            DataRow dr = ds.Tables[0].Rows[0];

            string content = dr["sms_content"].ToString();
            string mobile = dr["sms_mobiles"].ToString();
            string[] mobiles = mobile.Split(',');

            var guid = new Guid(id);
            byte[] buffer = guid.ToByteArray();
            long smsid = BitConverter.ToInt64(buffer, 0);

            var smscontent = $"{content}";

            int result = smshelper.sendSMS(SerialNo, key, mobiles, smscontent, smsid);
            if (result == 0)//成功
            {
                //更新审核
                sms.check(model);

                return XhdResult.Success("发送成功！").ToString();
            }
            else
            {
                return XhdResult.Error(SMSHelper.sms_result(result)).ToString();
            }
            //return SMSHelper.sms_result(result);

        }


    }
}
