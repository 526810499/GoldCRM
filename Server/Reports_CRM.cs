

using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Reports_CRM : BaseCRMServer
    {
 

        public Reports_CRM()
        {
        }

        public Reports_CRM(HttpContext context) : base(context) { }
        public string CRM_Reports_year()
        {
            var ccc = new BLL.CRM_Customer();

            string stype_val = PageValidate.InputText(request["stype_val"], 255);
            string syear = PageValidate.InputText(request["syear"], 50);

            if (!PageValidate.IsNumber(syear)) return "{}";

            DataSet ds = ccc.Reports_year(stype_val, int.Parse( syear), $"1=1");

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }

        public string Follow_Reports_year()
        {
            var follow = new BLL.CRM_follow();

            string items = "Follow_Type";

            string syear = PageValidate.InputText(request["syear"], 50);
            if (!PageValidate.IsNumber(syear)) return "{}";

            DataSet ds = follow.Reports_year(items,int.Parse(syear), $"1=1");

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }

        public string Funnel()
        {
            string whereStr = "";

            string stype_val = PageValidate.InputText(request["stype_val"], 255);
            string syear = PageValidate.InputText(request["syear"], 50);

            if (!PageValidate.IsNumber(syear)) return "{}";

            stype_val = stype_val.TrimEnd(';');
            string[] ids = stype_val.Split(';');

            string typeid = "";

            for (int i = 0; i < ids.Length; i++)
            {
                typeid += $"'{ids[i]}',";
            }
            typeid = typeid.TrimEnd(',');

            if (!string.IsNullOrEmpty(stype_val) && stype_val != "null")
                whereStr =$" a.id in ({typeid})";

            //context.Response.Write(whereStr);

            var ccc = new BLL.CRM_Customer();
            DataSet ds = ccc.Funnel(whereStr, syear.ToString());

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);

            return dt;
        }
    }
}