using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;

namespace XHD.Server
{
    public class m_order
    {
        public static BLL.Sale_order order = new BLL.Sale_order();
        public static Model.Sale_order model = new Model.Sale_order();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public m_order()
        {
        }

        public m_order(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetEmpWithToken(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        public string list()
        {
            int PageIndex = int.Parse(request["pageindex"] == null ? "1" : request["pageindex"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "10" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " Sale_order.create_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;

            string serchtxt = $"1=1  ";


            if (!string.IsNullOrEmpty(request["customer_id"]))
                serchtxt += $" and Sale_order.customer_id ='{PageValidate.InputText(request["customer_id"], 50)}'";



            //权限 
            serchtxt += DataAuth();

            DataSet ds = order.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

       


        public string form()
        {
            string id = PageValidate.InputText(request["id"], 50);

            if (!PageValidate.checkID(id))
                return "{}";

            DataSet ds = order.GetList($" Sale_order.id = '{id}' ");
            return DataToJson.DataToJSON(ds);
        }

        private string DataAuth()
        {
            GetDataAuth dataauth = new GetDataAuth();
            DataAuth auth = dataauth.getAuth(emp_id);

            switch (auth.authtype)
            {
                case 0: return " 1=2 ";
                case 1:
                case 2:
                case 3:
                case 4:
                    return $" and Sale_order.emp_id in ({auth.authtext})";
                case 5: return "";
            }

            return auth.authtype + ":" + auth.authtext;
        }
    }
}
