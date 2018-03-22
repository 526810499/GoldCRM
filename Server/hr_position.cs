
using System;
using System.Data;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class hr_position
    {
        public static BLL.hr_position position = new BLL.hr_position();
        public static Model.hr_position model = new Model.hr_position();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public hr_position()
        {
        }

        public hr_position(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }

        public string grid()
        {
            string serchtext = $" 1=1 ";
            DataSet ds = position.GetList(0, serchtext, "[position_order]");
            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }

        //save
        public string save()
        {
            model.position_name = PageValidate.InputText(request["T_position"], 255);
            model.position_order = int.Parse(request["T_order"]);
            model.position_level = PageValidate.InputText(request["T_level"], 50);

            string id = PageValidate.InputText(request["id"], 250);

            if (PageValidate.checkID(id))
            {
                model.id = id;
                DataSet ds = position.GetList($"id='{id}' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];
                position.Update(model);


                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.position_name;
                string EventType = "职位修改";
                string EventID = model.id;
                string Log_Content = null;

                if (dr["position_name"].ToString() != request["T_position"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "职务名称", dr["position_name"], request["T_position"]);

                if (dr["position_level"].ToString() != request["T_level"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "职务级别", dr["position_level"], request["T_level"]);

                if (dr["position_order"].ToString() != request["T_order"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "行号", dr["position_order"], request["T_order"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);

                return XhdResult.Success().ToString();
            }
            else
            {
                
                model.id = Guid.NewGuid().ToString().ToUpper();
                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                position.Add(model);

                return XhdResult.Success().ToString();
            }
        }

        //Form JSON
        public string form(string id)
        {
            string dt;

            if (!PageValidate.checkID(id)) return "{}";

            DataSet ds = position.GetList($"id='{id}' ");
            dt = DataToJson.DataToJSON(ds);

            return dt;
        }

        public string del(string id)
        {
            id = PageValidate.InputText(id, 50);
            string EventType = "职务删除";

            DataSet ds = position.GetList($"id='{id}' ");
            if (ds.Tables[0].Rows.Count == 0)
                return XhdResult.Error("系统错误，参数不正确！").ToString();

            var emp = new BLL.hr_employee();
            if (emp.GetList($"position_id ='{id}' ").Tables[0].Rows.Count > 0)
                return XhdResult.Error("含有员工信息不能删除！").ToString();

            var post = new BLL.hr_post();
            if (post.GetList($"position_id ='{id}' ").Tables[0].Rows.Count > 0)
                return XhdResult.Error("含有岗位信息不能删除！").ToString();

            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "9E686C8B-A2CC-4456-988B-88205728E487");
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }

            bool isdel = position.Delete(id);
            if (!isdel)
                return XhdResult.Error("删除失败，请检查参数！").ToString();

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["position_name"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();
        }


        public string combo()
        {
            string serchtext = $" 1=1 ";
            DataSet ds = position.GetList(0, serchtext, "position_level");
            var str = new StringBuilder();
            str.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append($"{{\"id\":\"{ ds.Tables[0].Rows[i]["id"] }\",\"text\":\"{ ds.Tables[0].Rows[i]["position_name"] }\"}},");
            }
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");
            return str.ToString();
        }

        public string getlevel(string id)
        {
            id = PageValidate.InputText(id, 50);
            var hz = new BLL.hr_position();
            DataSet ds = hz.GetList($"id='{id}'");

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["position_level"].ToString();
            }
            return ("-1");
        }
    }
}