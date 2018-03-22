
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Sale_order_details
    {
        public static BLL.Sale_order_details cod = new BLL.Sale_order_details();
        public static Model.Sale_order_details model = new Model.Sale_order_details();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public Sale_order_details()
        {
        }

        public Sale_order_details(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        public string grid(string orderid)
        {
            if (!PageValidate.checkID(orderid)) return "{\"Rows\":[],\"Total\":0}";

            DataSet ds = cod.GetList($" order_id = '{orderid}'");
            return GetGridJSON.DataTableToJSON(ds.Tables[0]);
        }
    }
}