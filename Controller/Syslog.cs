using System;
using XHD.BLL;
using XHD.Common;

namespace XHD.Controller
{
    public static class Syslog
    {  
        public static void Add_log(string uid, string uname, string ip, string EventTitle, string EventType, string EventID, string Log_Content)
        {
            var log = new BLL.Sys_log();
            var modellog = new Model.Sys_log();

            modellog.id = Guid.NewGuid().ToString().ToUpper();
            modellog.EventDate = DateTime.Now;
            modellog.UserID = uid;
            modellog.IPStreet = PageValidate.InputText(ip, 255);

            modellog.EventTitle = PageValidate.InputText(EventTitle, 255);

            modellog.EventType = PageValidate.InputText(EventType, 255);
            modellog.EventID = EventID.ToString();
            modellog.Log_Content = PageValidate.InputText(Log_Content, int.MaxValue);

            log.Add(modellog);
        }       

        public static string get_log_content(string pri_str,string request_str,string item_name,string cur_str,string cur_request)
        {
            pri_str = PageValidate.InputText(pri_str, int.MaxValue);
            request_str = PageValidate.InputText(request_str, int.MaxValue);
            item_name = PageValidate.InputText(item_name, int.MaxValue);
            cur_str = PageValidate.InputText(cur_str, int.MaxValue);
            cur_request = PageValidate.InputText(cur_request, int.MaxValue);
            string Log_Content = "";
            if (pri_str != request_str)
                Log_Content += string.Format("【{0}】{1} → {2} \n",item_name, cur_str, cur_request);

            return Log_Content;
        }
    }
}