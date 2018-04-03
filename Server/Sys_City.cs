

using System;
using System.Data;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Sys_City : BaseCRMServer
    {
        public static BLL.Sys_Param_City city = new BLL.Sys_Param_City();
        public static Model.Sys_Param_City model = new Model.Sys_Param_City();
 
        public Sys_City()
        {
        }

        public Sys_City(HttpContext context) : base(context) { }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " City_order ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = "";

            string id = PageValidate.InputText(request["provincesid"], 50);
            if (PageValidate.checkID(id))
                serchtxt += $"  Provinces_id = '{id}'";
            else
                serchtxt += "1=2";
            //权限          

            //context.Response.Write(serchtxt);
            DataSet ds = city.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        //save
        public string save()
        {
            model.City = PageValidate.InputText(request["T_City"], 255);
            model.City_order = int.Parse(request["T_order"]);
            model.Provinces_id = PageValidate.InputText(request["T_Parent_val"], 255);

            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                model.id = (id);

                DataSet ds = city.GetList($"id = '{id}'");

                if (ds.Tables[0].Rows.Count < 1)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                city.Update(model);

                //日志
                var provinces = new BLL.Sys_Param_Provinces();
                DataSet ds1 = provinces.GetList($"id = '{model.Provinces_id}'");

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.City;

                string EventType = "城市修改";
                string EventID = model.id;
                string Log_Content = null;

                Log_Content += Syslog.get_log_content(dr["City"].ToString(), request["T_City"], "城市", dr["City"].ToString(), request["T_City"]);
                Log_Content += Syslog.get_log_content(dr["City_order"].ToString(), request["T_order"], "排序", dr["City_order"].ToString(), request["T_order"]);
                Log_Content += Syslog.get_log_content(dr["Provinces_id"].ToString(), request["T_Parent_val"], "省份", ds1.Tables[0].Rows[0]["Provinces"].ToString(), request["T_Parent"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                model.id = Guid.NewGuid().ToString();
                city.Add(model);
            }

            return XhdResult.Success().ToString();
        }

        //Form JSON
        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);

            DataSet ds = city.GetList($"id='{id}' ");

            return DataToJson.DataToJSON(ds);

        }

        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();
            id = PageValidate.InputText(id, 50);

            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "A879344D-72E3-47DE-9E78-CEBAAABDE7AD");
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }

            DataSet ds = city.GetList($"id = '{id}'");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("参数不正确，删除失败！").ToString();

            if (ds.Tables[0].Rows[0]["City_type"].ToString() == "sys")
                return XhdResult.Error("系统参数，不允许删除！").ToString();


            var customer = new BLL.CRM_Customer();
            if (customer.GetList($"City_id = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此参数下含有客户数据，不允许删除！").ToString();

            bool isdel = city.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();

            //日志     
            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["City"].ToString();
            string EventType = "城市删除";

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();
        }

        public string combo()
        {
            string id = PageValidate.InputText(request["provincesid"], 50);
            if (!PageValidate.checkID(id)) return "{}";

            string serchtxt = $"Provinces_id='{id}' ";
            DataSet ds = city.GetList(0, serchtxt, "City_order");

            var str = new StringBuilder();

            str.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append("{\"id\":\"" + ds.Tables[0].Rows[i]["id"] + "\",\"text\":\"" + ds.Tables[0].Rows[i]["City"] + "\"},");
            }
            if (str.Length > 0)
                str.Replace(",", "", str.Length - 1, 1);

            str.Append("]");

            return str.ToString();
        }

    }
}