
using System;
using System.Data;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Product_supplier : BaseCRMServer
    {
        public static BLL.Product_supplier category = new BLL.Product_supplier();
        public static Model.Product_supplier model = new Model.Product_supplier();

 

        public Product_supplier()
        {
        }

        public Product_supplier(HttpContext context) : base(context) { }

        public string save()
        {
            model.parentid = (request["T_category_parent_val"]).CInt(0, false);
            model.product_supplier = PageValidate.InputText(request["T_product_warehouse"], 250);
            model.contact = PageValidate.InputText(request["T_contact"], 250);
            model.contactphone = PageValidate.InputText(request["T_contactphone"], 250);
            model.address = PageValidate.InputText(request["T_Adress"], 250);
            model.remark = PageValidate.InputText(request["T_Remark"], 250);
            int id = request["id"].CInt(0, false);
            int pid = (request["T_category_parent_val"]).CInt(0, false);
            if (id > 0)
            {
                model.id = id;

                DataSet ds = category.GetList($" id={id} ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                if (id == pid)
                    return XhdResult.Error("上级不能是自己，更新失败！").ToString();

                category.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.product_supplier;
                string EventType = "供应商修改";
                string EventID = model.id.CString("");
                string Log_Content = null;

                if (dr["product_supplier"].ToString() != request["T_product_warehouse"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "供应商名称", dr["product_supplier"].ToString(), request["T_product_warehouse"]);

                if (dr["product_icon"].ToString() != request["T_category_icon"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "图标", dr["product_icon"].ToString(), request["T_category_icon"]);

                if (dr["parentid"].ToString() != request["T_category_parent_val"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "上级", dr["parentid"].ToString(), request["T_category_parent_val"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);



            }

            else
            {

                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                category.Add(model);

            }
            return XhdResult.Success().ToString();
        }

        public string grid()
        {
            string sorttext = " create_time desc ";

            string serchtxt = $" 1=1 ";
            if (!string.IsNullOrEmpty(request["company"]))
                serchtxt += " and product_supplier like N'%" + PageValidate.InputText(request["company"], 50) + "%'";

            DataSet ds = category.GetList(0, serchtxt, sorttext);
            return "{\"Rows\":[" + GetTasks.GetTasksString("0", ds.Tables[0]) + "]}";

        }

        public string tree()
        {
            DataSet ds = category.GetList($"1=1");
            var str = new StringBuilder();
            str.Append("[");
            if (request["qxz"].CInt(0, false) == 1)
            {
                str.Append("{\"id\":\"\",\"text\":\"请选择\",\"d_icon\":\"\"},");
            }
            str.Append(GetTreeString("0", ds.Tables[0]));
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");
            return str.ToString();
        }

        public string combo()
        {
            DataSet ds = category.GetList($"1=1");
            var str = new StringBuilder();
            str.Append("[");
            str.Append("{\"id\":\"0\",\"text\":\"无\",\"d_icon\":\"\"},");
            str.Append(GetTreeString("0", ds.Tables[0]));
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");
            return str.ToString();
        }

        public string form(int id)
        {
            if (id <= 0) return "{}";

            DataSet ds = category.GetList($"id={id} ");
            return DataToJson.DataToJSON(ds);

        }

        //del
        public string del(int id)
        {
            if (id <= 0) return "false";

            DataSet ds = category.GetList($" id = {id} ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            var product = new BLL.Product();
            if (product.GetList($" SupplierID = {id}").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此供应商下含有产品，不允许删除！").ToString();

            if (category.GetList($"parentid = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此供应商下含有下级，不允许删除！").ToString();

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "B40C35DA-FDB2-4C2B-ABFC-EA97B37E30BD");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = category.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误！").ToString();

            //日志
            string EventType = "供应商删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id.CString("");
            string EventTitle = ds.Tables[0].Rows[0]["product_supplier"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();

        }


        private static string GetTreeString(string Id, DataTable table)
        {
            DataRow[] rows = table.Select($"parentid='{Id}'");

            if (rows.Length == 0) return string.Empty;
            ;
            var str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                str.Append("{\"id\":\"" + row["id"].CString("") + "\",\"text\":\"" + row["product_supplier"].CString("") + "\",\"d_icon\":\"../../" +  row["product_icon"].CString("") + "\"");

                if (GetTreeString(row["id"].CString(""), table).Length > 0)
                {
                    str.Append(",\"children\":[");
                    str.Append(GetTreeString(row["id"].CString(""), table));
                    str.Append("]},");
                }
                else
                {
                    str.Append("},");
                }
            }
            return str[str.Length - 1] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
        }
    }
}
