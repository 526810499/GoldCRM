using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using XHD.BLL;
using XHD.Common;
using XHD.Controller;
using System.Text;

namespace XHD.Server
{
    public class m_params
    {
        public static BLL.Sys_Param param = new BLL.Sys_Param();
        public static BLL.Sys_Param_Provinces provinces = new BLL.Sys_Param_Provinces();
        public static BLL.Sys_Param_City city = new BLL.Sys_Param_City();
        public static BLL.hr_employee emp = new BLL.hr_employee();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public m_params()
        {
        }

        public m_params(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetEmpWithToken(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        //combo
        public string combo()
        {
            string id = PageValidate.InputText(request["type"], 50);
            if (!PageValidate.checkID(id, false)) return "{}";

            DataSet ds = param.GetList(0, $" params_type = '{id}' ", "params_order,params_name");

            var str = new StringBuilder();

            str.Append("[");
            //str.Append("{id:0,text:'无'},");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append($"{{ \"value\":\"{ ds.Tables[0].Rows[i]["id"]  }\",\"text\":\"{ ds.Tables[0].Rows[i]["params_name"] }\"}},");
            }
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");

            return str.ToString();
        }

        public string getcity()
        {
            DataSet dsp = provinces.GetAllList();
            DataSet dsc = city.GetAllList();
            DataTable dt = dsc.Tables[0];

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            DataRow drp = null;
            for (int i = 0; i < dsp.Tables[0].Rows.Count; i++)
            {
                drp = dsp.Tables[0].Rows[i];
                sb.Append($"{{\"value\":\"{drp["id"]}\",\"text\":\"{drp["Provinces"]}\"");
                DataRow[] drc = dt.Select($"Provinces_id='{drp["id"]}'");

                if (drc.Length > 0)
                {
                    sb.Append(",\"children\":[");
                    for (int j = 0; j < drc.Length; j++)
                    {
                        sb.Append($"{{\"value\":\"{drc[j]["id"]}\",\"text\":\"{drc[j]["City"]}\"}},");
                    }
                    sb.Replace(",", "", sb.Length - 1, 1);
                    sb.Append("]");
                }
                sb.Append("},");
            }
            sb.Replace(",", "", sb.Length - 1, 1);
            sb.Append("]");

            return sb.ToString();
        }

        public string getemp()
        {
            string serchtxt = $"uid != 'admin'  ";

            DataSet ds = emp.GetList(serchtxt);

            var str = new StringBuilder();

            str.Append("[");
            //str.Append("{id:0,text:'无'},");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append($"{{ \"value\":\"{ ds.Tables[0].Rows[i]["id"]  }\",\"text\":\"{ ds.Tables[0].Rows[i]["name"] }\"}},");
            }
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");

            return str.ToString();
        }

    }
}
