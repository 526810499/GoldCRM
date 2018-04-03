

using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace XHD.Server
{
    public class Sys_data_authority : BaseCRMServer
    {
        public static BLL.Sys_data_authority auth = new BLL.Sys_data_authority();
        public static Model.Sys_data_authority model = new Model.Sys_data_authority();
 
        public Sys_data_authority()
        {
        }

        public Sys_data_authority(HttpContext context) : base(context) { }

        public string get(string Role_id)
        {
            Role_id = PageValidate.InputText(Role_id, 50);
            DataSet ds = auth.GetList($"Role_id = '{Role_id}'");

            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }

        public void save(string id, string depids)
        {
            id = PageValidate.InputText(id, 50);
            model.Role_id = id;
            
            model.create_id = emp_id;
            model.create_time = DateTime.Now;

            auth.Delete($"Role_id='{id}' ");

            depids = depids.TrimEnd(',');

            string[] ids = depids.Split(',');

            for (int i = 0; i < ids.Length; i++)
            {
                model.dep_id = PageValidate.InputText(ids[i].ToString(), 50);
                auth.Add(model);
            }

            //日志
            var role = new BLL.Sys_role();
            DataSet ds = role.GetList($"id = '{id}'");


            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["RoleName"].ToString();
            string EventType = "数据权限修改";

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);
        }


    }
}