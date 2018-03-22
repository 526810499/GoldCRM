
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class toolbar
    {
        public static BLL.Sys_Button btn = new BLL.Sys_Button();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public toolbar()
        {
        }

        public toolbar(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        public string GetSys()
        {
            string mid = PageValidate.InputText(request["mid"],50);

            string serchtxt = "";
           if (PageValidate.checkID(mid, false))
                serchtxt = $"Menu_id='{mid}' ";
            else
                return "{}";

            //return serchtxt;

            bool BtnAble = false;

            if (uid == "admin")
            {
                BtnAble = true;
            }

            DataSet ds = btn.GetList(0, serchtxt , "Btn_order");
            var getauth = new GetAuthorityByUid();
            string toolbarscript = "{\"Items\":[";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                toolbarscript += "{";
                toolbarscript += "\"type\": \"button\",";
                toolbarscript += "\"id\": \"" + ds.Tables[0].Rows[i]["Btn_id"] + "\",";
                toolbarscript += "\"text\": \"" + ds.Tables[0].Rows[i]["Btn_name"] + "\",";
                toolbarscript += "\"icon\": \"" + ds.Tables[0].Rows[i]["Btn_icon"] + "\",";
                toolbarscript += "\"menu\": \"" + ds.Tables[0].Rows[i]["Menu_id"] + "\",";
                if (BtnAble)
                {
                    toolbarscript += "\"disable\": true,";
                }
                else
                {
                    toolbarscript += "\"disable\": " +getauth.GetBtnAuthority(emp_id.ToString(),ds.Tables[0].Rows[i]["Btn_id"].ToString()).ToString().ToLower() + ",";
                }
                toolbarscript += "\"click\":";
                toolbarscript += ds.Tables[0].Rows[i]["Btn_handler"].ToString().Replace("()","");
                toolbarscript += "},";
            }
            toolbarscript = toolbarscript.TrimEnd(',');
            toolbarscript += "]}";

            return (toolbarscript);
        }
    }
}