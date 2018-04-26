

using System.Data;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;

namespace XHD.Server
{
    public class hr_department : BaseCRMServer
    {
        private static BLL.hr_department dep = new BLL.hr_department();
        private static Model.hr_department model = new Model.hr_department();

 
        public hr_department()
        {
        }

        public hr_department(HttpContext context) : base(context) { }

        public string deptree()
        {
            DataSet ds = dep.GetList(0, $"", "dep_order");
            var str = new StringBuilder();
            str.Append("[");
            str.Append("{\"id\":\"root\",\"text\":\"无\",\"d_icon\":\"61.png\",\"d_type\":\"\"},");
            str.Append(GetTreeString("root", ds.Tables[0], null));
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");
            return str.ToString();
        }

        public string treegrid()
        {
            DataSet ds = dep.GetList(0, $"", "dep_order");
            string dt = "{\"Rows\":[" + GetTasks. GetTasksString("root", ds.Tables[0]) + "]}";

            return dt;
        }

        public string tree()
        {
            string serchtxt = $" 1=1 ";

            string authtxt = PageValidate.InputText(request["auth"], 50);
            //if (PageValidate.IsNumber(authtxt))
            //{
            //    var dataauth = new GetDataAuth();
            //    DataAuth auth = dataauth.getAuth(emp_id);

            //    switch (auth.authtype)
            //    {
            //        case 1:
            //        case 2:
            //            string did = employee.dep_id.ToString();
            //            if (string.IsNullOrEmpty(did))
            //                did = "0";
            //            authtxt = did;
            //            break;
            //        case 4:
            //            authtxt = "0";
            //            break;
            //        case 3:
            //            DataSet dsdep = dep.GetList($"compnay_id = '{company_id}'");
            //            string deptask = GetTasks.GetDepTask(employee.dep_id, dsdep.Tables[0]);
            //            string intext = $"{employee.dep_id},{deptask}";
            //            authtxt = intext.TrimEnd(',');
            //            break;
            //    }
            //}
            //context.Response.Write(authtxt);
            DataSet ds = dep.GetList(0, serchtxt, " dep_order");
            var str = new StringBuilder();
            str.Append("[");
            if (request["qxz"].CInt(0, false) == 1)
            {
                str.Append("{\"id\":\"\",\"text\":\"==所有==\",\"d_icon\":\"\"},");
            }
            str.Append(GetTreeString("root", ds.Tables[0]));
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");
            return str.ToString();
        }

        //Form JSON
        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";

            id = PageValidate.InputText(id, 50);
            DataSet ds = dep.GetList($"  id = '{id}'");

            return DataToJson.DataToJSON(ds);
        }

        //save
        public string save()
        {
            model.dep_name = PageValidate.InputText(request["T_depname"], 255);
            model.parentid = PageValidate.InputText(request["T_parent_val"], 50);
            model.dep_type = PageValidate.InputText(request["T_deptype"], 50);
            model.dep_order = int.Parse(request["T_sort"]);
            model.dep_chief = PageValidate.InputText(request["T_leader"], 255);
            model.dep_tel = PageValidate.InputText(request["T_tel"], 255);
            model.dep_email = PageValidate.InputText(request["T_email"], 255);
            model.dep_fax = PageValidate.InputText(request["T_fax"], 255);
            model.dep_add = PageValidate.InputText(request["T_add"], 255);
            model.dep_descript = PageValidate.InputText(request["T_descript"], 255);

            if (model.dep_type == "部门")
                model.dep_icon = "88.png";
            else
                model.dep_icon = "61.png";

            string id = PageValidate.InputText(request["id"], 50);

            if (PageValidate.checkID(id))
            {
                model.id = id;
                DataSet ds = dep.GetList($"id='{ id }' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];
                dep.Update(model);

                //日志

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.dep_name;
                string EventType = "组织架构修改";
                string EventID = model.id;
                string Log_Content = null;

                if (dr["dep_name"].ToString() != request["T_depname"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "机构名称", dr["dep_name"], request["T_depname"]);

                if (dr["parentid"].ToString() != request["T_parent_val"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "上级机构", dr["parentid"], request["T_parent_val"]);

                if (dr["dep_type"].ToString() != request["T_deptype"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "机构类型", dr["dep_type"], request["T_deptype"]);

                if (dr["dep_order"].ToString() != request["T_sort"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "部门排序", dr["dep_order"], request["T_sort"]);

                if (dr["dep_chief"].ToString() != request["T_leader"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "负责人", dr["dep_chief"], request["T_leader"]);

                if (dr["dep_tel"].ToString() != request["T_tel"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "电话", dr["dep_tel"], request["T_tel"]);

                if (dr["dep_email"].ToString() != request["T_email"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "邮箱", dr["dep_email"], request["T_email"]);

                if (dr["dep_fax"].ToString() != request["T_fax"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "传真", dr["dep_fax"], request["T_fax"]);

                if (dr["dep_add"].ToString() != request["T_add"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "地址", dr["dep_add"], request["T_add"]);

                if (dr["dep_descript"].ToString() != request["T_descript"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "描述", dr["dep_descript"], request["T_descript"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);

                return XhdResult.Success("更新成功！").ToString();
            }
            else
            {
                
                model.id = Guid.NewGuid().ToString().ToUpper();
                model.create_id = emp_id;
                model.create_time = DateTime.Now;

                dep.Add(model);

                return XhdResult.Success().ToString();
            }
        }

        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return "false";

            id = PageValidate.InputText(id, 50);

            string EventType = "组织架构删除";

            DataSet ds = dep.GetList($" id ='{id}' ");
            if (ds.Tables[0].Rows.Count == 0)
                return XhdResult.Error("系统错误，参数不正确！").ToString();

            DataSet ds1 = dep.GetList($"parentid = '{id}'");
            if (ds1.Tables[0].Rows.Count > 0)
                return XhdResult.Error("此组织下含有下级组织，不能删除！").ToString();

            var post = new BLL.hr_post();
            if (post.GetList($"dep_id = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("含有岗位信息不能删除！").ToString();

            var emp = new BLL.hr_employee();
            if (emp.GetList($"dep_id = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("含有员工信息不能删除！").ToString();

            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "4E0A9A25-5608-440C-8D14-5261F6CA436D");
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }

            bool isdel = dep.Delete(id);
            if (!isdel)
                return XhdResult.Error("删除失败，请检查参数！").ToString();

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["dep_name"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();
        }

        private static string GetTreeString(string Id, DataTable table, string authtxt)
        {
            DataRow[] rows = table.Select(string.Format("parentid='{0}'", Id));

            if (rows.Length == 0) return string.Empty;

            var str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                string d_icon =  (string)row["dep_icon"];

                if (!string.IsNullOrEmpty(authtxt) && authtxt.IndexOf(row["id"].ToString()) == -1 && authtxt != "0")
                    d_icon = "50.png";

                str.Append($"{{\"id\":\"{ (string)row["id"] }\",\"text\":\"{ row["dep_name"] }\",\"d_icon\":\"{ d_icon }\",\"d_type\":\"{row["dep_type"]}\"");

                if (GetTreeString((string)row["id"], table, authtxt).Length > 0)
                {
                    str.Append(",\"children\":[");
                    str.Append(GetTreeString((string)row["id"], table, authtxt));
                    str.Append("]},");
                }
                else
                {
                    str.Append("},");
                }
            }
            return str[str.Length - 1] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
        }

        private static string GetTreeString(string Id, DataTable table)
        {
            DataRow[] rows = table.Select(string.Format("parentid='{0}'", Id));

            if (rows.Length == 0) return string.Empty;

            var str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                string d_icon =(string)row["dep_icon"];

                str.Append($"{{\"id\":\"{ (string)row["id"] }\",\"text\":\"{ row["dep_name"] }\",\"d_icon\":\"{ d_icon }\"");

                if (GetTreeString((string)row["id"], table).Length > 0)
                {
                    str.Append(",\"children\":[");
                    str.Append(GetTreeString((string)row["id"], table));
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