
using System;
using System.Data;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Sys_Provinces : BaseCRMServer
    {
        public static BLL.Sys_Param_Provinces provinces = new BLL.Sys_Param_Provinces();
        public static Model.Sys_Param_Provinces model = new Model.Sys_Param_Provinces();

 

        public Sys_Provinces()
        {
        }

        public Sys_Provinces(HttpContext context) : base(context) { }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " Provinces_order ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $"";
            //权限          

            //context.Response.Write(serchtxt);
            DataSet ds = provinces.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        public string tree()
        {
            string serchtxt = $"";
            DataSet ds = provinces.GetList(0, serchtxt, "Provinces_order");
            var str = new StringBuilder();
            str.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append("{\"id\":\"" + ds.Tables[0].Rows[i]["id"] + "\",\"text\":\"" + ds.Tables[0].Rows[i]["Provinces"] + "\"},");
            }
            if (str.Length > 0)
                str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");
            return str.ToString();
        }

        //save
        public string save()
        {
            model.Provinces = PageValidate.InputText(request["T_City"], 255);
            model.Provinces_order = int.Parse(request["T_order"]);

            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                model.id = (id);

                DataSet ds = provinces.GetList($"id='{id}'");
                if (ds.Tables[0].Rows.Count < 1)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                provinces.Update(model);

                //日志

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.Provinces;

                string EventType = "省份修改";
                string EventID = model.id;
                string Log_Content = null;

                Log_Content += Syslog.get_log_content(dr["Provinces"].ToString(), request["T_City"], "省份", dr["Provinces"].ToString(), request["T_City"]);
                Log_Content += Syslog.get_log_content(dr["Provinces_order"].ToString(), request["T_order"], "排序", dr["Provinces_order"].ToString(), request["T_order"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                model.id = Guid.NewGuid().ToString();
                provinces.Add(model);
            }

            return XhdResult.Success().ToString();
        }

        //Form JSON
        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";

            DataSet ds = provinces.GetList($"id='{id}'");

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
                bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "D2A01338-27CF-48BC-A1E0-86232D0BB774");
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }
            //DataSet ds = provinces.GetList($" id= '{id}'" );
            DataSet ds = provinces.GetList($"id='{id}'");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("参数不正确，删除失败！").ToString();

            var city = new BLL.Sys_Param_City();
            if (city.GetList($"Provinces_id = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此参数下含有城市数据，不允许删除！").ToString();

            var customer = new BLL.CRM_Customer();
            if (customer.GetList($"Provinces_id = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此参数下含有客户数据，不允许删除！").ToString();

            bool isdel = provinces.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();

            //日志     
            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["Provinces"].ToString();
            string EventType = "省份删除";

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();
        }

        public string combo()
        {
            string serchtxt = $"";
            DataSet ds = provinces.GetList(0, serchtxt, "Provinces_order");

            var str = new StringBuilder();

            str.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append("{\"id\":\"" + ds.Tables[0].Rows[i]["id"] + "\",\"text\":\"" + ds.Tables[0].Rows[i]["Provinces"] + "\"},");
            }
            if (str.Length > 0)
                str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");

            return str.ToString();
        }


    }
}