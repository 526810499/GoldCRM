

using System.Data;
using XHD.BLL;
using XHD.Common;

namespace XHD.Controller
{
    public class GetDataAuth
    {
        private static BLL.Sys_role role = new Sys_role();
        private static BLL.hr_employee emp = new hr_employee();
        private static BLL.Sys_role_emp roleemp = new Sys_role_emp();
        private static BLL.hr_department  dep = new hr_department();
        private static BLL.Sys_data_authority dataauth = new Sys_data_authority();

        public GetDataAuth()
        {
        }

        /// <summary>
        /// 根据用户ID获取公客修改权限
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool getPrivateCusEdit(string empid)
        {
            int temp = 0;
            string RoleIDs = GetRoleidByUID(empid);

            DataSet ds = role.GetList($" id in {RoleIDs}");

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (int.Parse(ds.Tables[0].Rows[i]["PublicAuth"].ToString()) > temp)
                        temp = int.Parse(ds.Tables[0].Rows[i]["PublicAuth"].ToString());
                }                
            }

            if (temp == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 根据用户ID获取数据权限
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public DataAuth getAuth(string empid)
        {
            return getAuth(empid, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empid"></param>
        /// <param name="authtype"></param>
        /// <returns></returns>
        public DataAuth getAuth(string empid,int authtype)
        {
            DataAuth da = new DataAuth();
            
            da.emp_id = empid;

            if (authtype == 999)//特殊权限标记
            {
                da.authtype = 5;
                return da;
            }
            
            DataSet ds1 = emp.GetList(string.Format("id = '{0}'", empid));
            if (ds1.Tables[0].Rows[0]["uid"].ToString() == "admin") //管理员不受权限控制
            {
                da.authtype = 5;
                return da;
            }

            int temp = 0;
            string RoleIDs = GetRoleidByUID(empid);

            if (!string.IsNullOrEmpty(RoleIDs))
            {
                //var sda = new Sys_data_authority();
                //DataSet ds = sda.GetList($" option_id= {optionid} and Role_id in {RoleIDs}");
                DataSet ds = role.GetList($" id in {RoleIDs}");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (int.Parse(ds.Tables[0].Rows[i]["DataAuth"].ToString()) > temp)
                            temp = int.Parse(ds.Tables[0].Rows[i]["DataAuth"].ToString());
                    }
                    //return temp.ToString();
                }
            }

            da.authtype = temp;

            switch (temp)
            {
                case 0: break;
                case 1: da.authtext = $"'{empid}'"; break;
                case 2: da.authtext = get_dep_emp_ids(ds1.Tables[0].Rows[0]["dep_id"].ToString()); break;
                case 3: da.authtext = get_depall_emp_ids(ds1.Tables[0].Rows[0]["dep_id"].ToString()); break;
                case 4: da.authtext = get_depAp_emp_ids(RoleIDs); break;
            }
            return da;
        }

        /// <summary>
        /// 根据部门ID获取用户ID集合
        /// </summary>
        /// <param name="dep_id"></param>
        /// <returns></returns>
        private string get_dep_emp_ids(string dep_id)
        {
            if (!PageValidate.checkID(dep_id)) return "";

            var emp = new hr_employee();
            DataSet ds = emp.GetList($"dep_id ='{dep_id}'");

            if (ds.Tables[0].Rows.Count < 1) return "";

            string emplist = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                emplist += $"'{ ds.Tables[0].Rows[i]["id"] }',";
            }
            emplist = emplist.TrimEnd(',');

            return emplist;
        }

        /// <summary>
        /// 根据部门ID获取用户ID集合
        /// </summary>
        /// <param name="dep_id"></param>
        /// <returns></returns>
        private string get_depall_emp_ids(string dep_id)
        {
            if (!PageValidate.checkID(dep_id)) return "";
            
            DataSet ds = dep.GetAllList();

            string deplist = GetTasks.GetDepTask(dep_id, ds.Tables[0]);
            deplist += $"'{dep_id}'";

            DataSet ds1 = emp.GetList($"dep_id in ({deplist})");

            if (ds1.Tables[0].Rows.Count < 1) return "";

            string emplist = "";
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                emplist += $"'{ ds1.Tables[0].Rows[i]["id"] }',";
            }
            emplist = emplist.TrimEnd(',');

            return emplist;
        }

        /// <summary>
        /// 根据权限ID获取跨部用户ID集合
        /// </summary>
        /// <param name="role_id"></param>
        /// <returns></returns>
        private string get_depAp_emp_ids(string role_id)
        {
            DataSet ds = emp.GetList($" dep_id in (select id from hr_department where id in (select dep_id from Sys_data_authority where Role_id in {role_id} )) ");
            if (ds.Tables[0].Rows.Count < 1) return "";

            string emplist = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                emplist += $"'{ ds.Tables[0].Rows[i]["id"] }',";
            }
            emplist = emplist.TrimEnd(',');

            return emplist;
        }

        /// <summary>
        /// 根据用户ID获取角色集合
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        private string GetRoleidByUID(string uid)
        {
            if (PageValidate.checkID(uid))
            {                
                DataSet ds = roleemp.GetList($"empID = '{uid}'");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string RoleIDs = "(";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        RoleIDs += $"'{ds.Tables[0].Rows[i]["RoleID"].ToString()}',";
                    }
                    RoleIDs = RoleIDs.TrimEnd(',');
                    RoleIDs += ")";
                    return RoleIDs;
                }
                else
                {
                    return "";
                }
            }
            return "";
        }


    }

    public class DataAuth
    {       
        public string emp_id { get; set; }
        public int authtype { get; set; }
        public string authtext { get; set; }
    }
}