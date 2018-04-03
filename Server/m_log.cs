
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
    public class m_log : BaseCRMServer
    {
        public static BLL.Sys_log log = new BLL.Sys_log();
        public static Model.Sys_log model = new Model.Sys_log();

 

        public m_log()
        {
        }

        public m_log(HttpContext context) : base(context) { }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " EventDate";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total = "0";

            DataSet ds = null;

            string serchtext = $" 1=1 ";

            if (!string.IsNullOrEmpty(request["EventID"]))
                serchtext += $" and EventID = '{PageValidate.InputText(request["EventID"],50)}'";           

            ds = log.GetList(PageSize, PageIndex, serchtext, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return dt;
        }

        public string logtype()
        {
            DataSet ds = log.GetLogtype();

            var str = new StringBuilder();

            str.Append("[");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append("{value:'" + ds.Tables[0].Rows[i]["EventType"] + "',text:'" +ds.Tables[0].Rows[i]["EventType"] + "'},");
            }
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");

            return str.ToString();
        }
    }
}