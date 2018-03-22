using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Product
    {
        public static BLL.Product product = new BLL.Product();
        public static Model.Product model = new Model.Product();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public Product()
        {
        }

        public Product(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        public string save()
        {
            model.category_id = PageValidate.InputText(request["T_product_category_val"], 50);
            model.product_name = PageValidate.InputText(request["T_product_name"], 255);
            model.specifications = PageValidate.InputText(request["T_specifications"], 255);
            model.unit = PageValidate.InputText(request["T_product_unit"], 255);
            model.remarks = PageValidate.InputText(request["T_remarks"], 255);
            model.price = decimal.Parse(request["T_price"]);
            model.agio = decimal.Parse(request["T_agio"]);
            model.cost = decimal.Parse(request["T_cost"]);

            string pid = PageValidate.InputText(request["pid"], 50);
            if (PageValidate.checkID(pid))
            {
                model.id = pid;
                DataSet ds = product.GetList($" id= '{pid}' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];
                product.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.product_name;
                string EventType = "产品修改";
                string EventID = model.id;

                string Log_Content = null;

                if (dr["category_name"].ToString() != request["T_product_category"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "产品类别", dr["category_name"],
                        request["T_product_category"]);

                if (dr["product_name"].ToString() != request["T_product_name"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "产品名字", dr["product_name"],
                        request["T_product_name"]);

                if (dr["specifications"].ToString() != request["T_specifications"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "产品规格", dr["specifications"],
                        request["T_specifications"]);

                if (dr["unit"].ToString() != request["T_product_unit"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "单位", dr["unit"], request["T_product_unit"]);

                if (dr["remarks"].ToString() != request["T_Remark"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "备注", dr["remarks"], request["T_Remark"]);

                if (decimal.Parse(dr["price"].ToString()) != decimal.Parse(request["T_price"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "零售价", dr["price"], request["T_price"]);

                if (decimal.Parse(dr["cost"].ToString()) != decimal.Parse(request["T_cost"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "成本价", dr["cost"], request["T_cost"]);

                if (decimal.Parse(dr["agio"].ToString()) != decimal.Parse(request["T_agio"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "折扣价", dr["agio"], request["T_agio"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                model.id = Guid.NewGuid().ToString();
                
                product.Add(model);
            }

            return XhdResult.Success().ToString();
        }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " create_time";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";
            if (!string.IsNullOrEmpty(request["categoryid"]))
                serchtxt += $" and category_id='{PageValidate.InputText(request["categoryid"], 50)}'";

            if (!string.IsNullOrEmpty(request["stext"]))
                serchtxt += $" and product_name like N'%{ PageValidate.InputText(request["stext"], 255) }%'";

            //权限
            DataSet ds = product.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return (dt);
        }

        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);
            DataSet ds = product.GetList($" id= '{id}' ");
            return DataToJson.DataToJSON(ds);

        }


        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();
            id = PageValidate.InputText(id, 50);
            DataSet ds = product.GetList($" id= '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            var ccod = new BLL.Sale_order_details();
            if (ccod.GetList($"product_id = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此产品下含有订单，不允许删除！").ToString();

            

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "FDE2E0EE-D2B1-40BD-928F-AB28F58D770D");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = product.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();

            //日志
            string EventType = "产品删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["product_name"].ToString();



            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();

        }
    }
}
