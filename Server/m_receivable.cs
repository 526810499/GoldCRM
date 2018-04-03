using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;

namespace XHD.Server
{
    public class m_receivable : BaseCRMServer
    {
        public static BLL.Finance_Receivable receivable = new BLL.Finance_Receivable();
        public static Model.Finance_Receivable model = new Model.Finance_Receivable();

 

        public m_receivable()
        {
        }

        public m_receivable(HttpContext context) : base(context) { }

        public string list()
        {
            int PageIndex = int.Parse(request["pageindex"] == null ? "1" : request["pageindex"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "10" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " Finance_Receivable.create_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;

            string serchtxt = $"1=1  ";


            if (!string.IsNullOrEmpty(request["customer_id"]))
                serchtxt += $" and CRM_Customer.id ='{PageValidate.InputText(request["customer_id"], 50)}'";

            DataSet ds = receivable.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }




        public string form()
        {
            string id = PageValidate.InputText(request["id"], 50);

            if (!PageValidate.checkID(id))
                return "{}";

            DataSet ds = receivable.GetList($" Finance_Receivable.id = '{id}'  ");
            return DataToJson.DataToJSON(ds);
        }
    }
}
