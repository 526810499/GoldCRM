using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;
using System.IO;

namespace XHD.Server
{
    public class CRM_Contact
    {
        private static BLL.CRM_Contact contact = new BLL.CRM_Contact();
        private static Model.CRM_Contact model = new Model.CRM_Contact();

        private HttpContext Context;
        private string emp_id;
        private string emp_name;
        private Model.hr_employee employee;
        private HttpRequest request;
        private string uid;


        public CRM_Contact()
        {
        }

        public CRM_Contact(HttpContext context)
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
            model.customer_id = PageValidate.InputText(request["T_company_val"], 50);
            if (PageValidate.checkID(request["customer_id"]))
                model.customer_id = PageValidate.InputText(request["customer_id"], 50);

            model.C_name = PageValidate.InputText(request["T_contact"], 250);

            if (PageValidate.IsNumber(request["T_sex_val"]))
                model.C_sex = int.Parse(request["T_sex_val"]);
            else
                model.C_sex = null;

            if (PageValidate.IsDateTime(request["T_birthday"]))
                model.C_birthday = DateTime.Parse(request["T_birthday"]);

            model.C_department = PageValidate.InputText(request["T_dep"], 250);
            model.C_position = PageValidate.InputText(request["T_position"], 250);

            model.C_tel = PageValidate.InputText(request["T_tel"], 250);
            model.C_mob = PageValidate.InputText(request["T_mobil"], 250);
            model.C_fax = PageValidate.InputText(request["T_fax"], 250);
            model.C_email = PageValidate.InputText(request["T_email"], 250);
            model.C_QQ = PageValidate.InputText(request["T_qq"], 250);
            model.C_add = PageValidate.InputText(request["T_add"], 250);

            model.C_hobby = PageValidate.InputText(request["T_hobby"], 250);
            model.C_remarks = PageValidate.InputText(request["T_remarks"], int.MaxValue);

            string contact_id = PageValidate.InputText(request["contact_id"], 50);
            if (PageValidate.checkID(contact_id))
            {
                DataSet ds = contact.GetList($"id = '{contact_id}' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                model.id = contact_id;

                contact.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.C_name;

                string EventType = "联系人修改";
                string EventID = model.id;
                string Log_Content = null;

                if (dr["C_name"].ToString() != request["T_contact"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人", dr["C_name"], request["T_contact"]);

                if (dr["C_sex"].ToString() != request["T_sex"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人性别", dr["C_sex"], request["T_sex"]);

                if (dr["C_birthday"].ToString() != request["T_birthday"])
                {
                    if (PageValidate.IsDateTime(dr["C_birthday"].ToString()))
                    {
                        if (DateTime.Parse(dr["C_birthday"].ToString()).ToString("yyyy-MM-dd") != request["T_birthday"])
                        {
                            Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人生日", dr["C_birthday"], request["T_birthday"]);
                        }
                    }
                    else
                    {
                        Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人生日", dr["C_birthday"], request["T_birthday"]);
                    }
                }

                if (dr["C_department"].ToString() != request["T_dep"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人部门", dr["C_department"], request["T_dep"]);

                if (dr["C_position"].ToString() != request["T_position"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人职位", dr["C_position"], request["T_position"]);

                if (dr["C_tel"].ToString() != request["T_tel"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人电话", dr["C_tel"], request["T_tel"]);

                if (dr["C_mob"].ToString() != request["T_mobil"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人手机", dr["C_mob"], request["T_mobil"]);

                if (dr["C_fax"].ToString() != request["T_fax"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人传真", dr["C_fax"], request["T_fax"]);

                if (dr["C_email"].ToString() != request["T_email"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人邮箱", dr["C_email"], request["T_email"]);

                if (dr["C_QQ"].ToString() != request["T_qq"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人QQ", dr["C_QQ"], request["T_qq"]);

                if (dr["C_add"].ToString() != request["T_add"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人地址", dr["C_add"], request["T_add"]);

                if (dr["C_hobby"].ToString() != request["T_hobby"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "联系人爱好", dr["C_hobby"], request["T_hobby"]);

                if (dr["C_remarks"].ToString() != request["T_remarks"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "备注", dr["C_remarks"], request["T_remarks"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);

                return XhdResult.Success().ToString();
            }
            else
            {
                model.id = Guid.NewGuid().ToString();

                model.create_id = emp_id;
                model.create_time = DateTime.Now;

                contact.Add(model);

                return XhdResult.Success().ToString();
            }
        }

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

            if (!string.IsNullOrEmpty(request["contact"]))
                serchtxt += $" and C_name like N'%{ PageValidate.InputText(request["contact"], 255) }%'";

            if (!string.IsNullOrEmpty(request["tel"]))
                serchtxt += $" and C_mob like N'%{ PageValidate.InputText(request["tel"], 255) }%'";

            if (!string.IsNullOrEmpty(request["qq"]))
                serchtxt += $" and C_QQ like N'%{ PageValidate.InputText(request["qq"], 255) }%'";

            if (!string.IsNullOrEmpty(request["startdate"]))
                serchtxt += $" and create_time >= '{ PageValidate.InputText(request["startdate"], 255) }'";

            if (!string.IsNullOrEmpty(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and create_time  <= '{ enddate }'";
            }

            if (!string.IsNullOrEmpty(request["startdate_del"]))
                serchtxt += $" and Delete_time >= '{ PageValidate.InputText(request["startdate_del"], 255) }'";

            if (!string.IsNullOrEmpty(request["enddate_del"]))
            {
                DateTime enddate1 = DateTime.Parse(request["enddate_del"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and Delete_time  <= '{ enddate1 }'";
            }
            if (!string.IsNullOrEmpty(request["smstype"]))
            {
                if (PageValidate.checkID(request["smsid"]))
                {
                    serchtxt += $" and id in (select contact_id from SMS_details where sms_id = '{PageValidate.InputText(request["smsid"], 50)}')";
                }
                else
                {
                    serchtxt += " and 1=2";
                }
            }
            //权限           

            serchtxt += $" and customer_id in (select id from CRM_Customer where 1=1 { Auth() })";

            //context.Response.Write(serchtxt);
            DataSet ds = contact.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        public string form(string id)
        {
            string dt;
            if (!PageValidate.checkID(id)) return "{}";

            DataSet ds = contact.GetList($"id = '{id}'  and customer_id in (select id from CRM_Customer where 1=1 { Auth() })");
            dt = DataToJson.DataToJSON(ds);

            return dt;
        }

        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            id = PageValidate.InputText(request["id"], 50);
            DataSet ds = contact.GetList($"id = '{id}' ");

            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            var follow = new BLL.CRM_follow();
            if (follow.GetList($"contact_id = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此联系人下含有跟进信息，不能删除！").ToString();

            //SMS check

            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "62BD8016-4684-4977-BC77-AB10F51432C4");
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }



            bool isdel = contact.Delete(id);

            if (!isdel)
                return XhdResult.Error("删除失败，请检查参数！").ToString();

            //日志
            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["C_name"].ToString();
            string EventType = "联系人删除";

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "电话" + ds.Tables[0].Rows[0]["C_tel"]);

            return XhdResult.Success().ToString();

        }

        public string import()
        {
            Random rnd = new Random();
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + rnd.Next(10000, 99999);

            string filepath = Context.Server.MapPath($"~/file/contact/");
            string fullpath = Context.Server.MapPath($"~/file/contact/{ filename }.xls");

            if (!Directory.Exists(Path.GetDirectoryName(filepath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            }

            string configpath = Context.Server.MapPath(@"~/file/template/contact.xml");

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
            string EventTitle = string.Format("【{0}】联系人导入", emp_name);

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, "联系人导入", EventID, i);

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
                    return $" and ( emp_id in ({auth.authtext}) or isPrivate =1)";
                case 5: return "";
            }

            return auth.authtype + ":" + auth.authtext;
        }
    }
}
