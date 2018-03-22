

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using XHD.BLL;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Sys_base
    {
        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;


        public Sys_base()
        {
        }

        public Sys_base(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);

        }

        public string GetAllMenus()
        {
            //if (!SysReg.Check())
            //    return "{}";

            var app = new BLL.Sys_App();
            DataSet ds = app.GetList(0, " ", "App_order");

            var str = new StringBuilder();
            str.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string children = GetSysApp(ds.Tables[0].Rows[i]["id"].ToString());

                if (children.Length > 2)
                {
                    str.Append("{\"id\":\"" + ds.Tables[0].Rows[i]["id"] + "\",\"text\":\"" + ds.Tables[0].Rows[i]["App_name"] + "\",\"App_icon\":\"" + ds.Tables[0].Rows[i]["App_icon"] + "\",\"App_css\":\"" + ds.Tables[0].Rows[i]["App_handler"] + "\",\"children\":");
                    str.Append(children);
                    str.Append("},");
                }
            }
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");
            return str.ToString();
        }

        public string GetSysApp(string appid)
        {
            appid = PageValidate.InputText(appid, 50);

            if (!PageValidate.checkID(appid, false))
                return "{}";

            var menu = new BLL.Sys_Menu();
            DataSet ds = null;

            if (uid == "admin")
            {
                ds = menu.GetList(0, $"App_id = '{appid}'", "Menu_order");
            }
            else
            {
                var getauth = new GetAuthorityByUid();
                string menus = getauth.GetAuthority(emp_id.ToString(), "Menus");
                if (menus.Length > 0)
                    ds = menu.GetList(0, $"App_id='{ appid }' and Menu_id in ({ menus})", "Menu_order");
            }
            string dt = "";
            if (ds != null)
                dt = "[" + GetTasksString("root", ds.Tables[0]) + "]";

            return (dt);
        }

        public string GetMenu(string appid)
        {
            if (string.IsNullOrEmpty(appid)) return "";
            string txt = "{\"Rows\":";
            txt += GetSysApp(appid);
            txt += "}";
            return txt;
        }

        public string getUserTree()
        {
            var sol = new Sys_online();
            var model = new Model.Sys_online();

            model.UserName = PageValidate.InputText(emp_name, 250);
            model.UserID = emp_id;
            model.LastLogTime = DateTime.Now;

            DataSet ds1 = sol.GetList($" UserID = '{emp_id}'");

            //添加当前用户信息
            if (ds1.Tables[0].Rows.Count > 0)
            {
                sol.Update(emp_id);
            }
            else
            {
                sol.Add(model);
            }

            //删除超时用户
            sol.Delete(" LastLogTime<DATEADD(MI,-2,getdate())");

            var dep = new BLL.hr_department();
            var hp = new BLL.hr_post();

            DataSet ds = dep.GetList(0, $"1=1", "dep_order");
            var str = new StringBuilder();
            str.Append("[");
            str.Append(GetTreeString("root", ds.Tables[0], 1));
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");
            return str.ToString();
        }

        public string GetUserInfo()
        {
            var hr_emp = new BLL.hr_employee();
            DataSet ds = hr_emp.GetList(string.Format("id = '{0}'", emp_id));

            string dt = DataToJson.DataToJSON(ds);

            return (dt);
        }

        public string GetOnline()
        {
            var sol = new Sys_online();
            var model = new Model.Sys_online();

            model.UserName = emp_name;
            model.UserID = emp_id;
            model.LastLogTime = DateTime.Now;

            DataSet ds1 = sol.GetList($" UserID='{emp_id}'");

            //添加当前用户信息
            if (ds1.Tables[0].Rows.Count > 0)
            {
                sol.Update(emp_id);
            }
            else
            {
                sol.Add(model);
            }
            //}

            //删除超时用户
            sol.Delete(" LastLogTime<DATEADD(MI,-2,getdate())");

            return (GetGridJSON.DataTableToJSON(sol.GetAllList().Tables[0]));
        }

        public string GetIcons()
        {
            try
            {
                //var icontype = request["icontype"];

                string rootPath = Context.Server.MapPath("~/images/icon/");
                var objtojson = new ObjectListToJSON();
                //List<FileInfo> lp = GetAllFilesInDirectory(rootPath);
                //string a = objtojson.toJSON(lp);
                //return (a);
                return GetAllFilesNameInDirectory(rootPath);
            }
            catch (Exception err)
            {
                return ("系统错误:" + err.Message);
            }
        }


        private List<FileInfo> GetAllFilesInDirectory(string strDirectory)
        {
            var listFiles = new List<FileInfo>();
            var directory = new DirectoryInfo(strDirectory);
            DirectoryInfo[] directoryArray = directory.GetDirectories();
            FileInfo[] fileInfoArray = directory.GetFiles();
            if (fileInfoArray.Length > 0) listFiles.AddRange(fileInfoArray);
            foreach (DirectoryInfo _directoryInfo in directoryArray)
            {
                var directoryA = new DirectoryInfo(_directoryInfo.FullName);
                DirectoryInfo[] directoryArrayA = directoryA.GetDirectories();
                FileInfo[] fileInfoArrayA = directoryA.GetFiles();
                if (fileInfoArrayA.Length > 0) listFiles.AddRange(fileInfoArrayA);
                GetAllFilesInDirectory(_directoryInfo.FullName); //递归遍历  
            }
            return listFiles;


        }

        private string GetAllFilesNameInDirectory(string strDirectory)
        {
            var directory = new DirectoryInfo(strDirectory);
            FileInfo[] fileInfoArray = directory.GetFiles();

            string FileNameJson = "[";

            foreach (FileInfo fileinfo in fileInfoArray)
            {
                FileNameJson += $"{{ \"filename\":\"{ fileinfo.Name }\"}},";
            }
            FileNameJson = FileNameJson.TrimEnd(',');
            FileNameJson += "]";

            return FileNameJson;
        }



        private static string GetTasksString(string Id, DataTable table)
        {
            DataRow[] rows = table.Select($"parentid='{Id}'");

            if (rows.Length == 0) return string.Empty;
            ;
            var str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                str.Append("{");
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    if (i != 0) str.Append(",");
                    str.Append("\"");
                    str.Append(row.Table.Columns[i].ColumnName);
                    str.Append("\":\"");
                    str.Append(row[i]);
                    str.Append("\"");
                }
                if (GetTasksString((string)row["Menu_id"], table).Length > 0)
                {
                    str.Append(",\"children\":[");
                    str.Append(GetTasksString((string)row["Menu_id"], table));
                    str.Append("]},");
                }
                else
                {
                    str.Append("},");
                }
            }
            return str[str.Length - 1] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
        }

        private static string GetTreeString(string Id, DataTable table, int todo)
        {
            var hp = new BLL.hr_post();
            var sol = new Sys_online();
            DataRow[] rows = table.Select(string.Format("parentid='{0}'", Id));

            if (rows.Length == 0) return string.Empty;

            var str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                str.Append("{\"id\":\"" + (string)row["id"] + "\",\"text\":\"" + (string)row["dep_name"] + "\",\"d_icon\":\"" + (string)row["dep_icon"] + "\"");

                if (GetTreeString((string)row["id"], table, 0).Length > 0)
                {
                    str.Append(",\"children\":[");
                    if (todo == 1)
                    {
                        DataSet dsp = hp.GetList($"dep_id='{(string)row["id"]}'");
                        if (dsp.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsp.Tables[0].Rows.Count; j++)
                            {
                                if (PageValidate.checkID(dsp.Tables[0].Rows[j]["emp_id"].ToString()))
                                {
                                    DataSet dso = sol.GetList($"UserID='{dsp.Tables[0].Rows[j]["emp_id"]}'");
                                    string posticon = "93.png";
                                    if (dso.Tables[0].Rows.Count > 0)
                                        posticon = "37.png"; //95

                                    str.Append("{\"id\":\"" + dsp.Tables[0].Rows[j]["id"] + "\",\"text\":\"" + dsp.Tables[0].Rows[j]["emp_name"] + "\",\"d_icon\":\"" + posticon + "\"}");
                                    str.Append(",");
                                }
                            }
                        }
                    }
                    str.Append(GetTreeString((string)row["id"], table, 1));
                    str.Append("]},");
                }
                else
                {
                    if (todo == 1)
                    {
                        DataSet dsp = hp.GetList($"dep_id='{(string)row["id"]}'");
                        if (dsp.Tables[0].Rows.Count > 0)
                        {
                            var str1 = new StringBuilder();
                            for (int j = 0; j < dsp.Tables[0].Rows.Count; j++)
                            {
                                if (PageValidate.checkID(dsp.Tables[0].Rows[j]["emp_id"].ToString()))
                                {
                                    DataSet dso = sol.GetList($"UserID='{  dsp.Tables[0].Rows[j]["emp_id"]}'");
                                    string posticon = "93.png";
                                    if (dso.Tables[0].Rows.Count > 0)
                                        posticon = "37.png"; //95

                                    str1.Append("{\"id\":\"" + dsp.Tables[0].Rows[j]["id"] + "\",\"text\":\"" + dsp.Tables[0].Rows[j]["emp_name"] + "\",\"d_icon\":\"" + posticon + "\"},");
                                    //if (j < dsp.Tables[0].Rows.Count - 1)
                                    //    str.Append(",");
                                }
                            }
                            if (str1.Length > 0)
                            {
                                str.Append(",\"children\":[");
                                str.Append(str1);
                                if (str[str.Length - 1] == ',')
                                    str.Remove(str.Length - 1, 1);
                                str.Append("]");
                            }
                        }
                    }
                    str.Append("},");
                }
            }
            return str[str.Length - 1] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
        }


    }
    public class xhdfiles_c
    {
        public string filename { get; set; }
    }
}