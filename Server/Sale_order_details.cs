
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Sale_order_details : BaseCRMServer
    {
        public static BLL.Sale_order_details cod = new BLL.Sale_order_details();
        public static Model.Sale_order_details model = new Model.Sale_order_details();

 

        public Sale_order_details()
        {
        }

        public Sale_order_details(HttpContext context) : base(context) { }

        public string grid(string orderid)
        {
            if (!PageValidate.checkID(orderid)) return "{\"Rows\":[],\"Total\":0}";

            DataSet ds = cod.GetList($" order_id = '{orderid}'");
            return GetGridJSON.DataTableToJSON(ds.Tables[0]);
        }
    }
}