
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class toolbar : BaseCRMServer
    {
        public static BLL.Sys_Button btn = new BLL.Sys_Button();
 

        public toolbar()
        {
        }

        public toolbar(HttpContext context) : base(context) { }

        public string GetSys()
        {
            string mid = PageValidate.InputText(request["mid"],50);

            string serchtxt = "  ";
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
            serchtxt += " and isHide=0 ";
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