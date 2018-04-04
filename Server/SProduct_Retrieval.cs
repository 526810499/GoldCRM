using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{

    /// <summary>
    /// 订购补货
    /// </summary>
    public class SProduct_Retrieval : BaseCRMServer
    {
        public static BLL.BProduct_Retrieval bll = new BLL.BProduct_Retrieval();
        public static Model.Product_Retrieval model = new Model.Product_Retrieval();

    

        public SProduct_Retrieval()
        {
        }

        public SProduct_Retrieval(HttpContext context) : base(context) {

            allDataBtnid = "6CD949FC-208E-427C-8383-AACDA68C1853";
            depDataBtnid = "5F551588-B7CC-48F4-90C2-108D63BFB040";
        }

        public string save()
        {
            model.category_id = (request["T_category_val"]).CString("");
            model.remark = PageValidate.InputText(request["T_remark"], 250);
            model.weight = request["T_weight"].CDecimal(0, false);
            model.number = request["T_number"].CInt(0, false);
            model.createdep_id = PageValidate.InputText(request["T_dep_id_val"], 250);
            string id = PageValidate.InputText(request["id"], 250);

            if (!string.IsNullOrWhiteSpace(id))
            {
                model.id = id;

                DataSet ds = bll.GetList($" id= '{id}' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow row = ds.Tables[0].Rows[0];

                bll.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = row["category_id"].CString("") + "_" + row["weight"].CString("") + "_" + row["number"].CString("");
                string EventType = "订购补货修改";
                string EventID = model.id.CString("");
                string Log_Content = null;

                if (row["category_id"].ToString() != request["T_category_val"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "category_id", row["category_id"].ToString(), request["T_category_val"]);

                if (row["weight"].ToString() != request["T_weight"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "weight", row["weight"].ToString(), request["T_weight"]);

                if (row["number"].ToString() != request["T_numbert"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "number", row["number"].ToString(), request["T_numbert"]);

                if (row["dep_id"].ToString() != request["T_dep_id_val"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "T_dep_id_val", row["dep_id"].ToString(), request["T_dep_id_val"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);

            }

            else
            {
                model.id = "DG-" + DateTime.Now.ToString("yy-MM-dd-") + DateTime.Now.GetHashCode().ToString().Replace("-", "");
                model.status = 1;
                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                bll.Add(model);

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

            string serchtxt = $" 1=1 ";
            if (!string.IsNullOrEmpty(request["category_id"]))
                serchtxt += " and category_id='" + PageValidate.InputText(request["category_id"], 50) + "'";
            if (!string.IsNullOrEmpty(request["id"]))
                serchtxt += " and id='" + PageValidate.InputText(request["id"], 50) + "'";

            serchtxt = GetSQLCreateIDWhere(serchtxt,true);

            string Total = "";
            DataSet ds = bll.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return (dt);

        }


        public string form(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return "{}";

            DataSet ds = bll.GetList($"id='{id}' ");
            return DataToJson.DataToJSON(ds);

        }

        //del
        public string del(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return "false";

            DataSet ds = bll.GetList($" id = '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "9E7DE3B4-8EC4-4846-A573-351B85BB357C");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = bll.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误！").ToString();

            DataRow row = ds.Tables[0].Rows[0];

            //日志
            string EventType = "订购补货删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id.CString("");
            string EventTitle = row["category_id"].CString("") + "_" + row["weight"].CString("") + "_" + row["number"].CString("");

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();

        }

    }
}
