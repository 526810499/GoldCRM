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
    /// 以旧换新
    /// </summary>
    public class SProduct_OldChangeNew : BaseCRMServer
    {
        public static BLL.BProduct_OldChangeNew bll = new BLL.BProduct_OldChangeNew();
        public static Model.Product_OldChangeNew model = new Model.Product_OldChangeNew();

 

        public SProduct_OldChangeNew()
        {
        }

        public SProduct_OldChangeNew(HttpContext context) : base(context) { }

        public string save()
        {
            model.remark = PageValidate.InputText(request["T_remark"], 250);
            model.oldWeight = request["T_oldWeight"].CDecimal(0, false);
            model.oldTotalPrice = request["T_oldTotalPrice"].CDecimal(0, false);
            model.oldCharge = request["T_oldCharge"].CDecimal(0, false);
            model.newWeight = request["T_newWeight"].CDecimal(0, false);
            model.newTotalPrice = request["T_newTotalPrice"].CDecimal(0, false);
            model.costsTotalPrice = request["T_costsTotalPrice"].CDecimal(0, false);
            model.oldWeight = request["T_newWeight"].CDecimal(0, false);
            model.discount = request["T_discount"].CDecimal(0, false);
            model.difTotalPrice = request["T_difTotalPrice"].CDecimal(0, false);
            model.dep_id = PageValidate.InputText(request["T_dep_id_val"], 250);
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
                string EventTitle = row["oldWeight"].CString("") + "_" + row["oldTotalPrice"].CString("") + "_" + row["oldCharge"].CString("") + row["newWeight"].CString("") + "_" + row["newTotalPrice"].CString("") + "_" + row["costsTotalPrice"].CString("");
                string EventType = "以旧换新修改";
                string EventID = model.id.CString("");
                string Log_Content = null;

                if (row["oldWeight"].ToString() != request["T_oldWeight"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "oldWeight", row["oldWeight"].ToString(), request["T_oldWeight"]);

                if (row["oldTotalPrice"].ToString() != request["T_oldTotalPrice"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "oldTotalPrice", row["oldTotalPrice"].ToString(), request["T_oldTotalPrice"]);

                if (row["oldCharge"].ToString() != request["T_oldCharge"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "oldCharge", row["oldCharge"].ToString(), request["T_oldCharge"]);

                if (row["newWeight"].ToString() != request["T_newWeight"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "newWeight", row["newWeight"].ToString(), request["T_newWeight"]);


                if (row["newTotalPrice"].ToString() != request["T_newTotalPrice"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "newTotalPrice", row["newTotalPrice"].ToString(), request["T_newTotalPrice"]);

                if (row["costsTotalPrice"].ToString() != request["T_costsTotalPrice"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "costsTotalPrice", row["costsTotalPrice"].ToString(), request["T_costsTotalPrice"]);
 
                if (row["discount"].ToString() != request["T_discount"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "discount", row["discount"].ToString(), request["T_discount"]);


                if (row["difTotalPrice"].ToString() != request["T_difTotalPrice"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "difTotalPrice", row["difTotalPrice"].ToString(), request["T_difTotalPrice"]);

                if (row["remark"].ToString() != request["T_remark"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "remark", row["remark"].ToString(), request["T_remark"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);

            }

            else
            {
                model.id = "HX-" + DateTime.Now.ToString("yy-MM-dd-") + DateTime.Now.GetHashCode().ToString().Replace("-", "");
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
            if (!string.IsNullOrEmpty(request["id"]))
                serchtxt += " and id='" + PageValidate.InputText(request["id"], 50) + "'";

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
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "F2F9B0E1-BF37-4FD6-A3B8-701316E339B1");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = bll.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误！").ToString();

            DataRow row = ds.Tables[0].Rows[0];

            //日志
            string EventType = "以旧换新删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id.CString("");
            string EventTitle = row["oldWeight"].CString("") + "_" + row["oldTotalPrice"].CString("") + "_" + row["oldCharge"].CString("")+ row["newWeight"].CString("") + "_" + row["newTotalPrice"].CString("") + "_" + row["costsTotalPrice"].CString("");

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();

        }

    }
}
