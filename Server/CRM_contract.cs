using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;
using System.IO;

namespace XHD.Server
{
    public class CRM_contract : BaseCRMServer
    {
        private static BLL.CRM_contract contract = new BLL.CRM_contract();
        private static Model.CRM_contract model = new Model.CRM_contract();

        private static BLL.CRM_contract_atta atta = new BLL.CRM_contract_atta();

 

        public CRM_contract()
        {
        }

        public CRM_contract(HttpContext context) : base(context) { }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " create_time ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";

            if (PageValidate.checkID(request["customerid"]))
                serchtxt += $" and customer_id='{PageValidate.InputText(request["customerid"], 50)}'";

            if (!string.IsNullOrEmpty(request["company"]))
                serchtxt += $" and customer_id in (select id from CRM_Customer where cus_name like N'%{ PageValidate.InputText(request["company"], 255)}%')";

            if (!string.IsNullOrEmpty(request["contract"]))
                serchtxt += $" and Contract_name like N'%{ PageValidate.InputText(request["contract"], 255) }%'";

            if (PageValidate.checkID(request["employee_val"]))
                serchtxt += $" and Our_Contractor_id = '{PageValidate.InputText(request["employee_val"], 50)}'";
            else if (PageValidate.checkID(request["department_val"]))
                serchtxt += $" and Our_Contractor_id in (select id from hr_employee where dep_id = '{PageValidate.InputText(request["department_val"], 50)}' )";

            //开始时间
            if (PageValidate.IsDateTime(request["startdate1"]))
                serchtxt += $" and Start_date >= '{ PageValidate.InputText(request["startdate1"], 255) }'";

            if (PageValidate.IsDateTime(request["startdate2"]))
            {
                DateTime enddate = DateTime.Parse(request["startdate2"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and Start_date  <= '{ enddate }'";
            }

            //到期时间
            if (PageValidate.IsDateTime(request["enddate1"]))
                serchtxt += $" and End_date >= '{ PageValidate.InputText(request["enddate1"], 255) }'";

            if (PageValidate.IsDateTime(request["enddate2"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate2"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and End_date  <= '{ enddate }'";
            }

            //签订时间
            if (PageValidate.IsDateTime(request["signdate1"]))
                serchtxt += $" and Sign_date >= '{ PageValidate.InputText(request["signdate1"], 255) }'";

            if (PageValidate.IsDateTime(request["signdate2"]))
            {
                DateTime enddate = DateTime.Parse(request["signdate2"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and Sign_date  <= '{ enddate }'";
            }

            //context.Response.Write(serchtxt);
            DataSet ds = contract.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        public string form(string id)
        {
            string dt;
            if (!PageValidate.checkID(id)) return $"{{\"id\":\"{ Guid.NewGuid().ToString() }\"}}";

            DataSet ds = contract.GetList($"id = '{id}' ");

            dt = DataToJson.DataToJSON(ds);

            return dt;
        }

        public string save()
        {
            model.Customer_id = PageValidate.InputText(request["T_customer_val"], 50);
            if (PageValidate.checkID(request["customer_id"]))
                model.Customer_id = PageValidate.InputText(request["customer_id"], 50);

            model.Contract_name = PageValidate.InputText(request["T_name"], 250);
            model.Serialnumber = PageValidate.InputText(request["T_no"], 50);
            model.Contract_amount = decimal.Parse(request["T_amount"]);
            model.Pay_cycle = int.Parse(request["T_Pay_cycle"]);

            model.Start_date = DateTime.Parse(request["T_Start_date"]);
            model.End_date = DateTime.Parse(request["T_End_date"]);
            model.Sign_date = DateTime.Parse(request["T_Sign_date"]);

            model.Customer_Contractor = PageValidate.InputText(request["T_Customer_Contractor"], 250);
            model.Our_Contractor_id = PageValidate.InputText(request["T_Our_Contractor_val"], 250);

            model.Main_Content = PageValidate.InputText(request["T_Main_Content"], int.MaxValue);
            model.Remarks = PageValidate.InputText(request["T_Remarks"], int.MaxValue);

            string id = PageValidate.InputText(request["id"], 50);
            if (!PageValidate.checkID(id))
                return XhdResult.Error("参数不正确，请检查！").ToString();

            DataSet ds = contract.GetList($"id = '{id}' ");

            if (ds.Tables[0].Rows.Count > 0)                
            {
                DataRow dr = ds.Tables[0].Rows[0];

                model.id = id;

                contract.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.Contract_name;

                string EventType = "合同修改";
                string EventID = model.id;
                string Log_Content = null;
                

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);

                return XhdResult.Success().ToString();
            }
            else
            {
                model.id = id;

                model.creater_id = emp_id;
                model.create_time = DateTime.Now;

                contract.Add(model);

                return XhdResult.Success().ToString();
            }
        }

        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            id = PageValidate.InputText(request["id"], 50);
            DataSet ds = contract.GetList($"id = '{id}' ");

            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();           

            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "124BB10B-70A2-4564-A0A0-FFBC5025218D");
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }

            //删除附件
            XHDIO.DelDirectoryAndFile($"~/file/contract/{ds.Tables[0].Rows[0]["id"].ToString()}/");

            //删除附件数据库
            atta.DeleteWhere($"contract_id = '{id}'");

            //删除合同
            bool isdel = contract.Delete(id);

            if (!isdel)
                return XhdResult.Error("删除失败，请检查参数！").ToString();

            //日志
            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["Contract_name"].ToString();
            string EventType = "合同删除";

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "");

            return XhdResult.Success().ToString();

        }
    }
}
