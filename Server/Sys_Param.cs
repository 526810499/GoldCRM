
using System.Data;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;

namespace XHD.Server
{
    public class Sys_Param : BaseCRMServer
    {
        public static BLL.Sys_Param param = new BLL.Sys_Param();
        public static Model.Sys_Param model = new Model.Sys_Param();
 

        public Sys_Param()
        {
        }

        public Sys_Param(HttpContext context) : base(context) { }
        public string GetApp()
        {
            var cpst = new BLL.Sys_Param_Type();
            DataSet ds = cpst.GetList(0, "", "params_order");

            var str = new StringBuilder();
            str.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append($"{{\"id\":\"{ ds.Tables[0].Rows[i]["id"]  }\",\"text\":\" { ds.Tables[0].Rows[i]["params_name"] } \"}},");
            }
            if (str.Length > 0)
                str.Replace(",", "", str.Length - 1, 1);

            str.Append("]");
            return str.ToString();
        }

        public string GetParams(string id)
        {
            id = PageValidate.InputText(id, 50);
            string dt;
            if (PageValidate.checkID(id, false))
            {
                DataSet ds = param.GetList(0, $" params_type = '{id}' ", "params_order,params_name");
                dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            }
            else
            {
                dt = "{}";
            }
            return dt;
        }

        //combo
        public string combo()
        {
            string id = PageValidate.InputText(request["type"], 50);
            if (!PageValidate.checkID(id, false)) return "{}";

            DataSet ds = param.GetList(0, $" params_type = '{id}' ", "params_order,params_name");

            var str = new StringBuilder();

            str.Append("[");
            //str.Append("{id:0,text:'无'},");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append($"{{\"id\":\"{ ds.Tables[0].Rows[i]["id"]  }\",\"text\":\" { ds.Tables[0].Rows[i]["params_name"] } \"}},");
            }
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");

            return str.ToString();
        }

        //Form JSON
        public string form()
        {
            string id = PageValidate.InputText(request["paramid"], 50);

            if (!PageValidate.checkID(id)) return "{}";

            DataSet ds = param.GetList($"id = '{id}' ");

            return DataToJson.DataToJSON(ds);
        }

        //save
        public string save()
        {
            model.params_name = PageValidate.InputText(request["T_param_name"], 255);
            model.params_order = int.Parse(request["T_param_order"]);
            model.KeyID = request["T_KeyID"].CLong(0, false);
            model.params_type = PageValidate.InputText(request["parentid"], 50);
            string id = PageValidate.InputText(request["paramid"], 50);

            if (PageValidate.checkID(id))
            {

                bool r = param.CheckKeyIDIsTrue(model);
                if (r)
                {
                    return XhdResult.Error("标识Key已存在请更换，更新失败！").ToString();
                }
                model.id = id;

                DataSet ds = param.GetList($"id = '{id}' ");
                if (ds.Tables[0].Rows.Count < 1)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                param.Update(model);

                //日志
                var params_type = new BLL.Sys_Param_Type();
                DataSet ds1 = params_type.GetList($"id = '{PageValidate.InputText(dr["params_type"].ToString(), 50)}'");

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = ds1.Tables[0].Rows[0]["params_name"].ToString();

                string EventType = "参数修改";
                string EventID = model.id;
                string Log_Content = null;

                Log_Content += Syslog.get_log_content(dr["params_name"].ToString(), request["T_param_name"], "参数名", dr["params_name"].ToString(), request["T_param_name"]);
                Log_Content += Syslog.get_log_content(dr["params_order"].ToString(), request["T_param_order"], "排序", dr["params_order"].ToString(), request["T_param_order"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                id = Guid.NewGuid().ToString().ToUpper();

                model.id = id;
                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                bool r = param.CheckKeyIDIsTrue(model);
                if (r)
                {
                    return XhdResult.Error("标识Key已存在请更换，更新失败！").ToString();
                }
                model.params_type = PageValidate.InputText(request["parentid"], 50);
                param.Add(model);
            }

            return XhdResult.Success().ToString();
        }

        //del
        public string del(string paramid)
        {
            string id = PageValidate.InputText(paramid, 50);

            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            DataSet ds = param.GetList($"id = '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "B7265173-AB71-44BF-827F-C3B66F81DD51");
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }


            var parentid = PageValidate.InputText(request["parentid"], 50);

            var customer = new BLL.CRM_Customer();
            var follow = new BLL.CRM_follow();
            var order = new BLL.Sale_order();
            //var receive = new BLL.C_CRM_receive();
            var task = new BLL.Task();

            DataSet dstemp = null;

            switch (parentid)
            {
                case "cus_industry":
                    dstemp = customer.GetList($"cus_industry_id = '{id}'");
                    break;
                case "cus_type":
                    dstemp = customer.GetList($"cus_type_id = '{id}'");
                    break;
                case "cus_level":
                    dstemp = customer.GetList($"cus_level_id = '{id}'");
                    break;
                case "cus_source":
                    dstemp = customer.GetList($"cus_source_id = '{id}'");
                    break;
                case "follow_type":
                    dstemp = follow.GetList($"follow_type_id = '{id}'");
                    break;
                //case "pay_type":
                //    dstemp = order.GetList($"pay_type_id = '{id}'" );
                //    if (dstemp.Tables[0].Rows.Count == 0)
                //        dstemp = receive.GetList($"pay_type_id = '{id}'" );
                //    break;
                case "order_status":
                    dstemp = order.GetList($"Order_status_id = '{id}'");
                    break;

                case "follow_aim":
                    dstemp = follow.GetList($"follow_aim_id = '{id}'");
                    break;
                case "task_type":
                    dstemp = task.GetList($"task_type_id = '{id}'");
                    break;
            }

            if (dstemp != null && dstemp.Tables[0].Rows.Count > 0)
                return XhdResult.Error("此参数下含有数据，不允许删除！").ToString();

            bool isdel = param.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();

            //日志
            var params_type = new BLL.Sys_Param_Type();
            DataSet ds1 = params_type.GetList($"id = '{PageValidate.InputText(ds.Tables[0].Rows[0]["params_type"].ToString(), 50)}'");
            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = $"【{ds1.Tables[0].Rows[0]["params_name"].ToString()}】" + ds.Tables[0].Rows[0]["params_name"].ToString();
            string EventType = "参数删除";

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();
        }

        public string validate()
        {
            string param_name = PageValidate.InputText(request["T_param_name"], 50);
            string parentid = PageValidate.InputText(request["parentid"], 50);
            string cid = PageValidate.InputText(request["T_cid"], 50);
            if (!PageValidate.checkID(cid))
                cid = "root";

            DataSet ds = param.GetList(string.Format(" params_name='{0}' and params_type='{1}' and id!='{2}'  ", param_name, parentid, cid));
            //context.Response.Write(" Count:" + ds.Tables[0].Rows.Count);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ("false");
            }
            return ("true");
        }
    }
}
