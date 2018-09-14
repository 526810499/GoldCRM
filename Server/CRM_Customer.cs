using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;
using System.IO;

namespace XHD.Server
{
    public class CRM_Customer : BaseCRMServer
    {
        private static BLL.CRM_Customer customer = new BLL.CRM_Customer();
        private static Model.CRM_Customer model = new Model.CRM_Customer();

        public CRM_Customer()
        {

        }

        public CRM_Customer(HttpContext context) : base(context)
        {
            ///开单销售选择客户
            if (context.Request["saleselect"].CInt(0, false) == 1)
            {
                allDataBtnid = "4C2A57BB-94A5-401A-82AA-24DE1F5DE4DB";
                depDataBtnid = "1F5A29CE-CE24-4A96-9B98-D72D5AF9B924";
            }
            else {
                allDataBtnid = "ED2E90BC-2618-49B9-B93A-6C4468E27738";
                depDataBtnid = "62735B9F-00B7-439C-93E3-5104D65DF5BE";
            }
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
                sortorder = " desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";

            string serchtype = request["isdel"];
            if (serchtype == "1")
                serchtxt += " and isDelete=1 ";
            else
                serchtxt += " and isDelete=0 ";

            if (!string.IsNullOrEmpty(request["company"]) && request["company"].CString("") != "输入关键词智能搜索客户")
                serchtxt += $" and cus_name like N'%{PageValidate.InputText(request["company"], 255)}%'";

            if (!string.IsNullOrEmpty(request["address"]))
                serchtxt += $" and address like N'%{PageValidate.InputText(request["address"], 255)}%'";

            if (!string.IsNullOrEmpty(request["tel"]))
                serchtxt += $" and cus_tel like N'%{PageValidate.InputText(request["tel"], 255)}%'";

            string keyword = PageValidate.InputText(request["keyword"], 500);
            if (!string.IsNullOrEmpty(keyword) && keyword != "输入关键词搜索地址、描述、备注")
                serchtxt += string.Format(" and ( address like N'%{0}%' or DesCripe like N'%{0}%' or Remarks like N'%{0}%' ) ", keyword);

            if (PageValidate.checkID(request["industry_val"]))
                serchtxt += $" and cus_industry_id = '{PageValidate.InputText(request["industry_val"], 50)}'";

            if (PageValidate.checkID(request["customertype_val"]))
                serchtxt += $" and cus_type_id = '{PageValidate.InputText(request["customertype_val"], 50)}' ";

            if (PageValidate.checkID(request["customerlevel_val"]))
                serchtxt += $" and cus_Level_id = '{PageValidate.InputText(request["customerlevel_val"], 50)}' ";

            if (PageValidate.checkID(request["cus_sourse_val"]))
                serchtxt += $" and cus_Source_id = '{PageValidate.InputText(request["cus_sourse_val"], 50)}' ";

            if (PageValidate.checkID(request["T_Provinces_val"]))
                serchtxt += $" and Provinces_id = '{PageValidate.InputText(request["T_Provinces_val"], 50)}'";

            if (PageValidate.checkID(request["T_City_val"]))
                serchtxt += $" and City_id = '{PageValidate.InputText(request["T_City_val"], 50)}'";


            if (PageValidate.checkID(request["employee_val"]))
                serchtxt += $" and emp_id = '{PageValidate.InputText(request["employee_val"], 50)}'";
            else if (PageValidate.checkID(request["department_val"]))
                serchtxt += $" and emp_id in (select id from hr_employee where dep_id = '{PageValidate.InputText(request["department_val"], 50)}' )";

            if (!string.IsNullOrEmpty(request["startdate"]))
                serchtxt += $" and create_time >= '{PageValidate.InputText(request["startdate"], 255)}'";

            if (!string.IsNullOrEmpty(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and create_time <= '{enddate}'";
            }

            if (!string.IsNullOrEmpty(request["startdate_del"]))
                serchtxt += $" and Delete_time >= '{PageValidate.InputText(request["startdate_del"], 255)}'";

            if (!string.IsNullOrEmpty(request["enddate_del"]))
            {
                DateTime enddatedel = DateTime.Parse(request["enddate_del"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and Delete_time <= '{enddatedel}'";
            }

            if (!string.IsNullOrEmpty(request["startfollow"]))
                serchtxt += $" and lastfollow >= '{PageValidate.InputText(request["startfollow"], 255)}'";

            if (!string.IsNullOrEmpty(request["endfollow"]))
            {
                DateTime enddate = DateTime.Parse(request["endfollow"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and lastfollow <= '{enddate}'";
            }

            if (request["type"] == "map")
                serchtxt += " and xy is not null";
            else if (request["type"] == "repeat")
            {
                serchtxt += " and cus_name in (select cus_name from (select cus_name from CRM_Customer where isDelete=0) abc group by cus_name having count(1)>=2  )";
                sorttext = " cus_name,id desc";
            }

            if (!string.IsNullOrEmpty(request["smstype"]))
            {
                if (PageValidate.checkID(request["smsid"]))
                {
                    serchtxt += $" and id in (select customer_id from CRM_sms_details where smsid = '{PageValidate.InputText(request["smsid"], 50)}')";
                }
                else
                {
                    serchtxt += " and 1=2";
                }
            }

            if (!string.IsNullOrEmpty(request["cusername"]) && request["search"].CInt(0, false) != 1)
            {
                serchtxt += $" and cus_name='{PageValidate.InputText(request["cusername"], 50)}'";
            }


            if (!string.IsNullOrEmpty(request["sday"]))
            {
                serchtxt += $" and birthdays={request["sday"].CInt(0, false)}";
            }

            if (!string.IsNullOrEmpty(request["smonth"]))
            {
                serchtxt += $" and birthmonths={request["smonth"].CInt(0, false)}";
            }


            if (!string.IsNullOrEmpty(request["stext"]) && request["search"].CInt(0, false) == 1)
            {
                if (request["stext"] != "输入姓名搜索")
                    serchtxt += " and cus_name like N'%" + PageValidate.InputText(request["stext"], 255) + "%'";
            }

            serchtxt += GetCusWhere();

            //权限
            DataSet ds = customer.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return dt;
        }

        private string GetCusWhere()
        {
            string serchtxt = "";

            //是否有所有权限
            bool isAllPrv = CheckBtnAuthority(allDataBtnid);
            //
            if (!isAllPrv && uid != "admin")
            {
                bool isDeepPrv = CheckBtnAuthority(depDataBtnid);
                if (isDeepPrv)
                {
                    serchtxt += $" and ( (isPrivate=0 and emp_id='{emp_id}') or (isPrivate=1) or (isPrivate=0 and emp_depid='{dep_id}') )";
                }
                else {
                    serchtxt += $" and ( (isPrivate=0 and emp_id='{emp_id}') or (isPrivate=1) )";
                }
            }
            return serchtxt;
        }

        public string save()
        {
            try
            {
                model.Serialnumber = "CUS-" + DateTime.Now.ToString("yyyy-MM-dd-") + DateTime.Now.GetHashCode().ToString().Replace("-", "");
                model.cus_name = PageValidate.InputText(request["T_customer"], 250);
                model.cus_add = PageValidate.InputText(request["T_address"], 250);
                model.cus_tel = PageValidate.InputText(request["T_tel"], 250);
                model.cus_fax = PageValidate.InputText(request["T_fax"], 250);
                model.cus_website = PageValidate.InputText(request["T_Website"], 250);
                model.cus_industry_id = PageValidate.InputText(request["T_industry_val"].CString(""), 50);
                model.Provinces_id = PageValidate.InputText(request["T_Provinces_val"], 50);
                model.City_id = PageValidate.InputText(request["T_City_val"], 50);
                model.cus_type_id = PageValidate.InputText(request["T_type_val"], 50);
                model.cus_level_id = PageValidate.InputText(request["T_level_val"], 50);
                model.cus_source_id = PageValidate.InputText(request["T_source_val"], 50);
                model.DesCripe = PageValidate.InputText(request["T_descript"], int.MaxValue);
                model.Remarks = PageValidate.InputText(request["T_remarks"], int.MaxValue);
                model.emp_id = PageValidate.InputText(request["T_employee_val"], 50);
                model.isPrivate = int.Parse(request["T_private_val"]);
                model.xy = PageValidate.InputText(request["T_xy"], 50);
                model.cus_extend = PageValidate.InputText(request["extendjson"], int.MaxValue);
                model.birthday = request["T_birthday"].CDateTime();
                model.integral = request["T_integral"].CInt(0, false);
                model.createdep_id = dep_id;
                string id = PageValidate.InputText(request["id"], 50);
                if (model.cus_industry_id == "undefined") { model.cus_industry_id = ""; }
                if (model.cus_type_id == "undefined") { model.cus_type_id = ""; }
                if (model.cus_level_id == "undefined") { model.cus_level_id = ""; }
                if (model.cus_source_id == "undefined") { model.cus_source_id = ""; }
                if (PageValidate.checkID(id))
                {
                    model.id = id;

                    DataSet ds = customer.GetList($"id = '{id}' ");

                    if (ds.Tables[0].Rows.Count == 0)
                        return XhdResult.Error("参数不正确，更新失败！").ToString();

                    DataRow dr = ds.Tables[0].Rows[0];
                    string creatuid = dr["emp_id"].CString("");
                    string createdep_id = dr["emp_depid"].CString("");
                    int isOldisPrivate = dr["isPrivate"].CInt(1, false);

                    //是私客需要判断是否当前用户有权限修改
                    if (isOldisPrivate == 0)
                    {                    //判断公客权限
                        if (emp_id != creatuid && uid != "admin" && !CheckBtnAuthority(allDataBtnid) && !(createdep_id == dep_id && CheckBtnAuthority(depDataBtnid)))
                        {
                            return XhdResult.Error("您不具修改该客户的权限！").ToString();
                        }
                    }
                    else if (model.isPrivate == 0)
                    {//原来是公客，现在要改成私客，需判断是否有权限
                        if (emp_id != creatuid && uid != "admin" && !CheckBtnAuthority(allDataBtnid) && !(createdep_id == dep_id && CheckBtnAuthority(depDataBtnid)))
                        {
                            return XhdResult.Error("您不具修改该客户的权限！").ToString();
                        }
                    }
                    //原来创建的不是当前要关联的这个用户需要去获取下部门
                    if (creatuid != model.emp_id)
                    {
                        model.emp_depid = new BLL.hr_employee().GetModel(model.emp_id).dep_id;
                    }
                    else { model.emp_depid = createdep_id; }

                    bool isupdate = customer.Update(model);

                    if (!isupdate) return XhdResult.Error("更新失败！").ToString();

                    string logcontent = "";
                    logcontent += Syslog.get_log_content(dr["cus_name"].CString(""), request["T_customer"], "客户名", dr["cus_name"].CString(""), request["T_customer"]);
                    logcontent += Syslog.get_log_content(dr["cus_add"].CString(""), request["T_address"], "地址", dr["cus_add"].CString(""), request["T_address"]);
                    logcontent += Syslog.get_log_content(dr["cus_tel"].CString(""), request["T_tel"], "电话", dr["cus_tel"].CString(""), request["T_tel"]);
                    logcontent += Syslog.get_log_content(dr["cus_fax"].CString(""), request["T_fax"], "传真", dr["cus_fax"].CString(""), request["T_fax"]);
                    logcontent += Syslog.get_log_content(dr["cus_website"].CString(""), request["T_Website"], "网址", dr["cus_website"].CString(""), request["T_Website"]);
                    logcontent += Syslog.get_log_content(dr["cus_industry_id"].CString(""), request["T_industry_val"], "行业", dr["cus_industry"].CString(""), request["T_industry"]);
                    logcontent += Syslog.get_log_content(dr["Provinces_id"].CString(""), request["T_Provinces_val"], "省份", dr["Provinces"].CString(""), request["T_Provinces"]);
                    logcontent += Syslog.get_log_content(dr["City_id"].CString(""), request["T_City_val"], "城市", dr["City"].CString(""), request["T_City"]);
                    logcontent += Syslog.get_log_content(dr["cus_type_id"].CString(""), request["T_type_val"], "客户类别", dr["cus_type"].CString(""), request["T_type"]);
                    logcontent += Syslog.get_log_content(dr["cus_level_id"].CString(""), request["T_level_val"], "客户级别", dr["cus_level"].CString(""), request["T_level"]);
                    logcontent += Syslog.get_log_content(dr["cus_source_id"].CString(""), request["T_source_val"], "客户来源", dr["cus_source"].CString(""), request["T_source"]);
                    logcontent += Syslog.get_log_content(dr["DesCripe"].CString(""), request["T_descript"], "客户描述", dr["DesCripe"].CString(""), request["T_descript"]);
                    logcontent += Syslog.get_log_content(dr["Remarks"].CString(""), request["T_remarks"], "备注", dr["Remarks"].CString(""), request["T_remarks"]);
                    logcontent += Syslog.get_log_content(dr["emp_id"].CString(""), request["T_employee_val"], "归属", dr["employee"].CString(""), request["T_employee"]);
                    logcontent += Syslog.get_log_content(dr["emp_depid"].CString(""), createdep_id, "emp_depid", dr["emp_depid"].CString(""), createdep_id);
                    logcontent += Syslog.get_log_content(dr["isPrivate"].CString(""), request["T_private_val"], "公私", dr["isPrivate"].CString("") == "0" ? "私客" : "公客", request["T_private"]);
                    logcontent += Syslog.get_log_content(dr["xy"].CString(""), request["T_xy"], "坐标", dr["xy"].CString(""), request["T_xy"]);

                    if (!string.IsNullOrEmpty(logcontent))
                    {
                        //日志

                        string UserID = emp_id;
                        string UserName = emp_name;
                        string IPStreet = request.UserHostAddress;
                        string EventTitle = model.cus_name;
                        string EventType = "客户修改";
                        string EventID = model.id;

                        Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, logcontent);
                    }

                    return XhdResult.Success().ToString();
                }
                else
                {
                    id = Guid.NewGuid().ToString();
                    model.id = id;
                    model.create_id = emp_id;
                    model.create_time = DateTime.Now;
                    model.emp_depid = new BLL.hr_employee().GetModel(model.emp_id).dep_id;
                    model.isDelete = 0;

                    bool isadd = customer.Add(model);

                    if (!isadd) return XhdResult.Error("添加失败！").ToString();

                    //BLL.CRM_Contact contact = new BLL.CRM_Contact();
                    //Model.CRM_Contact modelcontact = new Model.CRM_Contact();

                    //modelcontact.id = Guid.NewGuid().ToString();
                    //modelcontact.customer_id = id;
                    //modelcontact.company_id = company_id;

                    //modelcontact.C_name = PageValidate.InputText(request["T_contact_name"], 250);
                    //modelcontact.C_sex = PageValidate.InputText(request["T_sex"], 50);
                    //modelcontact.C_department = PageValidate.InputText(request["T_contact_dep"], 250);
                    //modelcontact.C_position = PageValidate.InputText(request["T_contact_position"], 250);
                    //modelcontact.C_email = PageValidate.InputText(request["T_email"], 250);
                    //modelcontact.C_QQ = PageValidate.InputText(request["T_qq"], 250);
                    //modelcontact.C_tel = PageValidate.InputText(request["T_contact_tel"], 250);
                    //modelcontact.C_mob = PageValidate.InputText(request["T_mobil"], 250);
                    //modelcontact.C_hobby = PageValidate.InputText(request["T_hobby"], 250);
                    //modelcontact.C_remarks = PageValidate.InputText(request["T_contact_remarks"], int.MaxValue);
                    //modelcontact.create_id = emp_id;
                    //modelcontact.create_time = DateTime.Now;

                    //contact.Add(modelcontact);

                    return XhdResult.Success().ToString();
                }
            }
            catch (Exception error)
            {
                SoftLog.LogStr(error.ToString(), "customer");
                return XhdResult.Error("系统错误，请重试！").ToString();
            }
        }

        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);
            DataSet ds = customer.GetList($"id = '{id}'  {GetCusWhere()} ");
            string dt = DataToJson.DataToJSON(ds);

            return dt;
        }
        //预删除
        public string AdvanceDelete(string id)
        {
            id = PageValidate.InputText(id, 50);
            DataSet ds = customer.GetList($"id='{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，找不到数据！").ToString();

            var contact = new BLL.CRM_Contact();
            var order = new BLL.Sale_order();
            var follow = new BLL.CRM_follow();

            int contactcount = 0, followcount = 0, ordercount = 0;
            contactcount = contact.GetList($"customer_id = '{id}'").Tables[0].Rows.Count;
            followcount = follow.GetList($" Customer_id= '{id}'").Tables[0].Rows.Count;
            ordercount = order.GetList($" Customer_id= '{id}'").Tables[0].Rows.Count;

            string returntxt = "此客户已放入回收站。 ";
            if (contactcount > 0 || followcount > 0 || ordercount > 0)
            {
                returntxt += "【注意】此客户下含有 ";
                if (contactcount > 0) returntxt += $"【{contactcount}】联系人 ";
                if (followcount > 0) returntxt += $"【{followcount}】跟进 ";
                if (ordercount > 0) returntxt += $"【{ordercount}】订单 ";
                returntxt += "，如需彻底删除此客户，需将这些数据删除。 ";
            }

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "bcdedd22-d9c5-4c74-b042-f0a75a4a35be");
                if (!candel)
                    return XhdResult.Error("无权限！").ToString();

                //dataauth
                var dataauth = new GetDataAuth();
                DataAuth auth = dataauth.getAuth(emp_id);
                string authid = ds.Tables[0].Rows[0]["emp_id"].ToString();
                switch (auth.authtype)
                {
                    case 0: candel = false; break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        if (authid.IndexOf(auth.authtext) == -1) candel = false; break;
                }
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }


            bool isdel = customer.AdvanceDelete(id, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (!isdel) return XhdResult.Error("系统错误，请检查参数！").ToString();

            //日志
            string EventType = "客户预删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["cus_name"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success(returntxt).ToString();
        }

        //regain            
        public string regain(string idlist)
        {
            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "D2769CAF-8BC2-46D4-9758-7EE5EC4626C6");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }
            idlist = PageValidate.InputText(request["idlist"], int.MaxValue);
            idlist = idlist.TrimEnd(',');
            string[] arr = idlist.Split(',');
            var ids = "";
            for (int i = 0; i < arr.Length; i++)
                ids += $"'{arr[i]}',";

            ids = ids.TrimEnd(',');

            DataSet ds = customer.GetList($"id in ({ids }) ");

            //日志   
            string EventType = "恢复删除客户";
            string UserID = emp_id;
            string UserName = emp_name;

            string IPStreet = request.UserHostAddress;

            int success = 0, failure = 0; //计数
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                bool isregain = customer.AdvanceDelete(arr[i], 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (isregain)
                {
                    string EventID = PageValidate.InputText(ds.Tables[0].Rows[i]["id"].ToString(), 50);
                    string EventTitle = ds.Tables[0].Rows[i]["cus_name"].ToString();
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);
                    success++;
                }
                else
                {
                    failure++;
                }
            }
            return XhdResult.Success(string.Format("{0}恢复成功,{1}失败", success, failure)).ToString();
        }

        public string del(string idlist)
        {
            bool candel = false;

            if (uid != "admin")
            {
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "08783D72-5D43-42F8-9FBD-6C637FC3376F");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }


            idlist = PageValidate.InputText(request["idlist"], int.MaxValue);

            string EventType = "彻底删除客户";
            idlist = idlist.TrimEnd(',');
            string[] arr = idlist.Split(',');
            var ids = "";
            for (int i = 0; i < arr.Length; i++)
                ids += $"'{arr[i]}',";

            ids = ids.TrimEnd(',');

            DataSet ds = customer.GetList($"id in ({ids.Trim()}) ");

            var contact = new BLL.CRM_Contact();
            var order = new BLL.Sale_order();
            var follow = new BLL.CRM_follow();

            int contactcount = 0, followcount = 0, ordercount = 0, success = 0, failure = 0;

            //日志    
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (uid != "admin")
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    string creatuid = dr["create_id"].CString("");
                    string createdep_id = dr["createdep_id"].CString("");
                    //判断公客权限
                    if (emp_id != creatuid && uid != "admin" && !CheckBtnAuthority(allDataBtnid) && !(createdep_id == dep_id && CheckBtnAuthority(depDataBtnid)))
                    {
                        return XhdResult.Error("您不具修改该客户的权限！").ToString();
                    }
                }

                string cid = ds.Tables[0].Rows[i]["id"].ToString();
                cid = PageValidate.InputText(cid, 50);

                contactcount = contact.GetList($"customer_id = '{cid}'").Tables[0].Rows.Count;
                followcount = follow.GetList($" Customer_id='{cid}'").Tables[0].Rows.Count;
                ordercount = order.GetList($" Customer_id='{cid}'").Tables[0].Rows.Count;

                //context.Response.Write( string.Format("{0}联系人, {2}跟进, {3}订单，{1}合同 ", contactcount, contractcount, followcount, ordercount)+":"+(contactcount > 0 || contractcount > 0 || followcount > 0 || ordercount > 0)+" ");

                if (contactcount > 0 || followcount > 0 || ordercount > 0)
                {
                    failure++;
                }
                else
                {
                    bool isdel = customer.Delete(cid);
                    if (isdel)
                    {
                        success++;
                        string UserID = emp_id;
                        string UserName = emp_name;
                        string IPStreet = request.UserHostAddress;
                        string EventID = cid;
                        string EventTitle = ds.Tables[0].Rows[i]["cus_name"].ToString();

                        Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);
                    }
                    else
                    {
                        failure++;
                    }
                }
            }
            return XhdResult.Success(string.Format("{0}条数据成功删除，{1}条失败。", success, failure)).ToString();


        }
        public int c_count()
        {
            string serchtxt = " isDelete=0 ";

            if (!string.IsNullOrEmpty(request["T_employee1_val"]))
                serchtxt += string.Format(" and emp_id= '{0}'", PageValidate.InputText(request["T_employee1_val"], 50));

            if (PageValidate.checkID(request["industry_val"]))
                serchtxt += $" and industry_id = '{PageValidate.InputText(request["industry_val"], 50)}'";

            if (PageValidate.checkID(request["T_customertype_val"]))
                serchtxt += $" and cus_type_id = '{PageValidate.InputText(request["T_customertype_val"], 50)}' ";

            if (PageValidate.checkID(request["T_customerlevel_val"]))
                serchtxt += $" and cus_Level_id = '{PageValidate.InputText(request["T_customerlevel_val"], 50)}' ";

            if (PageValidate.checkID(request["T_cus_sourse_val"]))
                serchtxt += $" and cus_Source_id = '{PageValidate.InputText(request["T_cus_sourse_val"], 50)}' ";

            if (!string.IsNullOrEmpty(request["startdate"]))
                serchtxt += $" and create_time >= '{PageValidate.InputText(request["startdate"], 255)}'";

            if (!string.IsNullOrEmpty(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and create_time <= '{enddate}'";
            }

            if (!string.IsNullOrEmpty(request["startfollow"]))
                serchtxt += $" and lastfollow >= '{PageValidate.InputText(request["startfollow"], 255)}'";

            if (!string.IsNullOrEmpty(request["endfollow"]))
            {
                DateTime enddate = DateTime.Parse(request["endfollow"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and lastfollow <= '{enddate}'";
            }

            DataSet ds = customer.GetList(serchtxt);

            return (ds.Tables[0].Rows.Count);

        }

        //validate website
        public string validate()
        {
            string type = PageValidate.InputText(request["type"], 50);
            string customerid = PageValidate.InputText(request["T_cid"], 50);
            string validval = "";
            string valifile = "";
            switch (type)
            {
                case "cus": validval = PageValidate.InputText(request["T_customer"], 50); valifile = "cus_name"; break;
                case "tel": validval = PageValidate.InputText(request["T_tel"], 50); valifile = "cus_tel"; break;
            }

            DataSet ds = customer.GetList($"{ valifile } = N'{validval}' and id!= '{customerid}' ");
            //context.Response.Write(" Count:" + ds.Tables[0].Rows.Count);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ("false");
            }
            return ("true");
        }
        public string getMapList()
        {
            string serchtxt = $" isDelete=0 and ( xy is not null and xy <>'' ) ";
            //权限
            serchtxt += Auth();
            DataSet ds = customer.GetMapList(serchtxt);
            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }

        public string Compared()
        {
            string year1 = PageValidate.InputText(request["year1"], 50);
            string year2 = PageValidate.InputText(request["year2"], 50);
            string month1 = PageValidate.InputText(request["month1"], 50);
            string month2 = PageValidate.InputText(request["month2"], 50);

            DataSet ds = customer.Compared(year1, month1, year2, month2);

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return (dt);
        }

        public string Compared_type()
        {
            string year1 = PageValidate.InputText(request["year1"], 50);
            string year2 = PageValidate.InputText(request["year2"], 50);
            string month1 = PageValidate.InputText(request["month1"], 50);
            string month2 = PageValidate.InputText(request["month2"], 50);

            DataSet ds = customer.Compared_type(year1, month1, year2, month2);

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return (dt);
        }

        public string Compared_level()
        {
            string year1 = PageValidate.InputText(request["year1"], 50);
            string year2 = PageValidate.InputText(request["year2"], 50);
            string month1 = PageValidate.InputText(request["month1"], 50);
            string month2 = PageValidate.InputText(request["month2"], 50);

            DataSet ds = customer.Compared_level(year1, month1, year2, month2);

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return (dt);
        }

        public string Compared_source()
        {
            string year1 = PageValidate.InputText(request["year1"], 50);
            string year2 = PageValidate.InputText(request["year2"], 50);
            string month1 = PageValidate.InputText(request["month1"], 50);
            string month2 = PageValidate.InputText(request["month2"], 50);

            DataSet ds = customer.Compared_source(year1, month1, year2, month2);

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return (dt);
        }

        public string Compared_empcusadd()
        {
            string idlist = PageValidate.InputText(request["idlist"], int.MaxValue);
            string year1 = PageValidate.InputText(request["year1"], 50);
            string year2 = PageValidate.InputText(request["year2"], 50);
            string month1 = PageValidate.InputText(request["month1"], 50);
            string month2 = PageValidate.InputText(request["month2"], 50);

            if (idlist.Length < 1)
                idlist = "0";

            string[] pid = idlist.Split(';');
            string pidlist = "";
            for (int i = 0; i < pid.Length; i++)
            {
                pidlist += $"'{pid[i]}',";
            }
            pidlist = pidlist.TrimEnd(',');

            //var post = new BLL.hr_post();
            //DataSet dspost = post.GetList("id in(" + pidlist + ")");

            //string emplist = "(";

            //for (int i = 0; i < dspost.Tables[0].Rows.Count; i++)
            //{
            //    emplist += $"'{dspost.Tables[0].Rows[i]["emp_id"]}',";
            //}
            //emplist = emplist.TrimEnd(',');
            //emplist += ")";

            //return emplist;

            DataSet ds = customer.Compared_empcusadd(year1, month1, year2, month2, $" (select emp_id from hr_post where id in ({pidlist}) ) ");

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return (dt);
        }

        public string emp_customer()
        {
            string idlist = PageValidate.InputText(request["idlist"], int.MaxValue);
            string syear = request["syear"];

            if (idlist.Length < 1)
                idlist = "0";

            string[] pid = idlist.Split(';');
            string pidlist = "";
            for (int i = 0; i < pid.Length; i++)
            {
                pidlist += $"'{pid[i]}',";
            }
            pidlist = pidlist.TrimEnd(',');



            //context.Response.Write(emplist);

            DataSet ds = customer.report_empcus(int.Parse(syear), $" (select emp_id from hr_post where id in ({pidlist}) ) ");

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return (dt);
        }
        public string import()
        {
            Random rnd = new Random();
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + rnd.Next(10000, 99999);

            string filepath = Context.Server.MapPath($"~/file/customer/");
            string fullpath = Context.Server.MapPath($"~/file/customer/{ filename }.xls");

            if (!Directory.Exists(Path.GetDirectoryName(filepath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            }

            string configpath = Context.Server.MapPath(@"~/file/template/Customer.xml");

            HttpPostedFile uploadFile = request.Files[0];

            uploadFile.SaveAs(fullpath);

            var excel = new Excel.Excel_new(fullpath, configpath, emp_id);
            string i = excel.Import();

            File.Delete(fullpath);

            //日志
            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = emp_id;
            string EventTitle = string.Format("【{0}】客户导入", emp_name);

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, "客户导入", EventID, i);

            return i;
        }
        private string Auth()
        {
            GetDataAuth dataauth = new GetDataAuth();
            DataAuth auth = dataauth.getAuth(emp_id);

            switch (auth.authtype)
            {
                case 0: return " and isPrivate = 1 ";
                case 1:
                case 2:
                case 3:
                case 4:
                    return $" and emp_id in ({auth.authtext}) ";
                case 5: return "";
            }

            return auth.authtype + ":" + auth.authtext;
        }
    }
}
