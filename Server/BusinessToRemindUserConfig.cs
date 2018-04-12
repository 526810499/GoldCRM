using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;
namespace XHD.Server
{



    /// <summary>
    /// 用户业务提醒配置
    /// </summary>
    public class BusinessToRemindUserConfig : BaseCRMServer
    {
        private static BLL.BBusinessToRemindUserConfig bll = new BLL.BBusinessToRemindUserConfig();
        private static Model.BusinessToRemindUserConfig model = new Model.BusinessToRemindUserConfig();




        public BusinessToRemindUserConfig()
        {
        }

        public BusinessToRemindUserConfig(HttpContext context) : base(context) { }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " id ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";



            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";

            if (PageValidate.checkID(request["userid"]))
                serchtxt += $" and bc.userid='{PageValidate.InputText(request["userid"], 50)}'";
            if (PageValidate.checkID(request["username"]))
                serchtxt += $" and hc.name like %'{HttpUtility.UrlDecode(PageValidate.InputText(request["username"], 50))}'%";

            DataSet ds = bll.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        public string form(long id)
        {
            DataSet ds = bll.GetList($"bc.id = {id} ");
            if (ds == null || ds.Tables[0].Rows.Count <= 0) { return "{}"; }
            return DataToJson.DataToJSON(ds);
        }

        public string save()
        {
            model.userid = PageValidate.InputText(request["T_userid_val"], 250);
            model.remindType = request["T_remindType_val"].CInt(0, false);
            model.rcount = request["T_rcount"].CInt(0, false);
            model.remark = PageValidate.InputText(request["T_Remark"], int.MaxValue);

            long id = request["id"].CLong(0, false);
            if (id > 0)
            {
                DataSet ds = bll.GetList($"bc.id = {id} ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                model.id = id;

                bll.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.remark;

                string EventType = "用户业务提醒配置";
                string EventID = model.id.CString("");
                string Log_Content = null;

                Log_Content += Syslog.get_log_content(dr["userid"].ToString(), request["T_userid_val"], "userid", dr["userid"].ToString(), request["T_userid_val"]);

                Log_Content += Syslog.get_log_content(dr["remark"].ToString(), request["T_Remark"], "remark", dr["remark"].ToString(), request["T_Remark"]);
                Log_Content += Syslog.get_log_content(dr["remindType"].ToString(), request["T_remindType_val"], "remindType", dr["remindType"].ToString(), request["T_remindType_val"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                model.create_id = emp_id;
                model.create_time = DateTime.Now;

                bll.Add(model);
            }

            return XhdResult.Success().ToString();
        }

        public string del(long id)
        {
            if (id <= 0) return XhdResult.Error("参数错误！").ToString();
            DataSet ds = bll.GetList($"bc.id = {id} ");

            if (ds.Tables[0].Rows.Count == 0)
                return XhdResult.Error("参数不正确，更新失败！").ToString();

            DataRow dr = ds.Tables[0].Rows[0];

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "A6D3033C-D095-43E6-BC0B-136ABF8ECF30");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = bll.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误！").ToString();

            //日志
            string EventType = "应收款删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id.CString("");
            string EventTitle = dr["remark"].CString("") + "__" + dr["userid"].CString("") + "__" + dr["remindType"].CString("");

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();
        }


        /// <summary>
        /// 删除提醒
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string DeleteRemind(int type)
        {

            bll.DeletBrithDayRemind(emp_id);

            return XhdResult.Success().ToString();
        }

        /// <summary>
        /// 获取今日生日提醒
        /// </summary>
        /// <returns></returns>
        public string GetTodayRemind()
        {

            string result = "";
            try
            {
                DataSet ds = bll.BrithDayRemind(emp_id);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    DataTable table = ds.Tables[0];

                    foreach (DataRow row in table.Rows)
                    {
                        string cusername = row["cusername"].CString("");

                        result += "<br/><span   style='height:20px;margin-top:5px'><a herf='javascript:void(0)' style='color:#3366FF' onclick=\"window.top.f_addTab('crm_customer','客户管理','crm/Customer/customer.aspx?sday=" + DateTime.Now.Day + "&smonth=" + DateTime.Now.Month + "')\">" + cusername + "</a></span>";

                    }

                    result = "<b>今日生日用户:</b>" + result;
                }
            }
            catch (Exception error)
            {

            }
 
            return XhdResult.Success(result).ToString();

        }

    }
}
