
using System;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using XHD.Common;
using XHD.Controller;
using System.IO;
using System.Text;

namespace XHD.Server
{
    public class hr_employee : BaseCRMServer
    {
        public static BLL.hr_employee emp = new BLL.hr_employee();
        public static Model.hr_employee model = new Model.hr_employee();



        public hr_employee()
        {
        }

        public hr_employee(HttpContext context) : base(context) { }

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

            if (request["config"].CInt(0, false) != 1)
            {
                serchtxt = $"uid != 'admin'  ";
            }

            string did = PageValidate.InputText(request["did"], 50);
            if (PageValidate.checkID(did))
                serchtxt += $" and dep_id= '{did}'";

            //string authtxt = request["auth"];

            //if (PageValidate.IsNumber(authtxt))
            //{
            //    GetDataAuth dataauth = new GetDataAuth();
            //    DataAuth auth = dataauth.getAuth(int.Parse(authtxt), "sys_view", emp_id);

            //    //return auth.authtype.ToString();

            //    switch (auth.authtype)
            //    {
            //        case 0: serchtxt += " and 1=2"; break;
            //        case 1:
            //        case 2:
            //        case 3: serchtxt += $" and id in ({auth.authtext}) "; break;
            //    }
            //}

            if (!string.IsNullOrEmpty(request["stext"]))
            {
                if (request["stext"] != "输入姓名搜索")
                    serchtxt += " and name like N'%" + PageValidate.InputText(request["stext"], 255) + "%'";
            }

            //return serchtxt;
            //权限
            DataSet ds = emp.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return dt;
        }

        //表格json
        public string getRole(string empid)
        {
            string dt;
            if (!PageValidate.checkID(empid)) return "{}";

            DataSet ds = emp.GetRole(empid);
            dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);

            return dt;
        }

        //validate
        public string Exist()
        {
            string user_id = PageValidate.InputText(request["T_uid"], 50);
            string T_emp_id = PageValidate.InputText(request["emp_id"], 50);


            DataSet ds1 = emp.GetList($"uid='{user_id}' and id!='{T_emp_id}' ");

            return (ds1.Tables[0].Rows.Count > 0 ? "false" : "true");
        }

        //Form JSON
        public string form(string id)
        {
            string dt;
            if (PageValidate.checkID(id))
            {
                id = PageValidate.InputText(request["id"], 50);
            }
            else if (id == "epu")
            {
                id = emp_id;
            }
            else
            {
                return "{}";
            }

            DataSet ds = emp.GetList($"id='{id}' ");
            dt = DataToJson.DataToJSON(ds);
            return dt;
        }

        //save
        public string save()
        {
            model.uid = PageValidate.InputText(request["T_uid"], 255);
            model.email = PageValidate.InputText(request["T_email"], 255);
            model.name = PageValidate.InputText(request["T_name"], 255);
            model.birthday = PageValidate.InputText(request["T_birthday"], 255);
            model.sex = PageValidate.InputText(request["T_sex"], 255);
            model.idcard = PageValidate.InputText(request["T_idcard"], 255);
            model.tel = PageValidate.InputText(request["T_tel"], 255);
            model.status = PageValidate.InputText(request["T_status"], 255);
            model.EntryDate = PageValidate.InputText(request["T_entryDate"], 255);
            model.address = PageValidate.InputText(request["T_Adress"], 255);
            model.schools = PageValidate.InputText(request["T_school"], 255);
            model.education = PageValidate.InputText(request["T_edu"], 255);
            model.professional = PageValidate.InputText(request["T_professional"], 255);
            model.remarks = PageValidate.InputText(request["T_remarks"], 255);
            model.title = PageValidate.InputText(request["headurl"], 255);
            model.canlogin = int.Parse(request["canlogin"]);

            string empid;
            string id = PageValidate.InputText(request["id"], 50);
            if (PageValidate.checkID(id))
            {
                DataSet ds = emp.GetList($" id='{id}' ");

                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];
                model.id = id;
                empid = model.id;

                emp.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.name;
                string EventType = "员工修改";
                string EventID = model.id;
                string Log_Content = null;

                if (dr["email"].ToString() != request["T_email"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "邮箱", dr["email"], request["T_email"]);

                if (dr["name"].ToString() != request["T_name"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "员工姓名", dr["name"], request["T_name"]);

                if (dr["birthday"].ToString() != request["T_birthday"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "员工生日", dr["birthday"], request["T_birthday"]);

                if (dr["sex"].ToString() != request["T_sex"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "员工性别", dr["sex"], request["T_sex"]);

                if (dr["status"].ToString() != request["T_status"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "状态", dr["status"], request["T_status"]);

                if (dr["idcard"].ToString() != request["T_idcard"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "身份证", dr["idcard"], request["T_idcard"]);

                if (dr["tel"].ToString() != request["T_tel"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "手机", dr["tel"], request["T_tel"]);

                if (dr["EntryDate"].ToString() != request["T_entryDate"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "入职日期", dr["EntryDate"], request["T_entryDate"]);

                if (dr["address"].ToString() != request["T_Adress"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "地址", dr["address"], request["T_Adress"]);

                if (dr["schools"].ToString() != request["T_school"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "毕业学校", dr["schools"], request["T_school"]);

                if (dr["education"].ToString() != request["T_edu"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "学历", dr["education"], request["T_edu"]);

                if (dr["professional"].ToString() != request["T_professional"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "专业", dr["professional"],
                        request["T_professional"]);

                if (dr["remarks"].ToString() != request["T_remarks"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "备注", dr["remarks"], request["T_remarks"]);

                if (dr["canlogin"].ToString() != request["canlogin"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "能否登录", dr["canlogin"], request["canlogin"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);

            }
            else
            {
                int usercount = emp.GetList($"").Tables[0].Rows.Count;

                empid = Guid.NewGuid().ToString().ToUpper();
                model.id = empid;
                model.pwd = FormsAuthentication.HashPasswordForStoringInConfigFile("123456", "MD5");
                model.create_id = emp_id;
                model.create_time = DateTime.Now;

                emp.Add(model);
            }

            //post
            string json = request["PostData"].ToLower();
            var js = new JavaScriptSerializer();

            PostData[] postdata;
            postdata = js.Deserialize<PostData[]>(json);

            var hp = new BLL.hr_post();
            var modelpost = new Model.hr_post();

            //清除员工
            hp.UpdatePostEmpbyEid(empid);

            modelpost.emp_id = empid;
            model.id = empid;

            for (int i = 0; i < postdata.Length; i++)
            {
                modelpost.id = postdata[i].id;
                modelpost.default_post = postdata[i].default_post;

                //设置员工
                hp.UpdatePostEmp(modelpost);

                if (postdata[i].default_post == 1)
                {
                    model.dep_id = postdata[i].dep_id;
                    model.position_id = postdata[i].position_id;
                    model.post_id = postdata[i].id;

                    //更新默认岗位
                    emp.UpdatePost(model);
                }
            }

            return XhdResult.Success().ToString();
        }

        public void updateDefaultCity(string city)
        {
            model.default_city = PageValidate.InputText(city, 50);
            model.id = emp_id;
            emp.UpdateDefaultCity(model);
        }

        public string getDefaultCity()
        {
            return employee.default_city;
        }

        public string PersonalUpdate()
        {
            model.email = PageValidate.InputText(request["T_email"], 255);
            model.name = PageValidate.InputText(request["T_name"], 255);
            model.birthday = PageValidate.InputText(request["T_birthday"], 255);
            model.sex = PageValidate.InputText(request["T_sex"], 255);
            model.idcard = PageValidate.InputText(request["T_idcard"], 255);
            model.tel = PageValidate.InputText(request["T_tel"], 255);


            model.address = PageValidate.InputText(request["T_Adress"], 255);
            model.schools = PageValidate.InputText(request["T_school"], 255);
            model.education = PageValidate.InputText(request["T_edu"], 255);
            model.professional = PageValidate.InputText(request["T_professional"], 255);
            model.remarks = PageValidate.InputText(request["T_remarks"], 255);
            model.title = PageValidate.InputText(request["headurl"], 255);

            DataSet dsemp = emp.GetList($"id = '{emp_id}' ");
            DataRow dr = dsemp.Tables[0].Rows[0];
            model.id = emp_id;

            bool isup = emp.PersonalUpdate(model);

            if (isup)
            {
                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.name;
                string EventType = "个人信息修改";
                string EventID = emp_id;
                string Log_Content = null;

                if (dr["email"].ToString() != request["T_email"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "邮箱", dr["email"], request["T_email"]);

                if (dr["name"].ToString() != request["T_name"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "员工姓名", dr["name"], request["T_name"]);

                if (dr["birthday"].ToString() != request["T_birthday"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "员工生日", dr["birthday"], request["T_birthday"]);

                if (dr["sex"].ToString() != request["T_sex"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "员工性别", dr["sex"], request["T_sex"]);

                if (dr["idcard"].ToString() != request["T_idcard"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "身份证", dr["idcard"], request["T_idcard"]);

                if (dr["tel"].ToString() != request["T_tel"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "手机", dr["tel"], request["T_tel"]);

                if (dr["address"].ToString() != request["T_Adress"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "地址", dr["address"], request["T_Adress"]);

                if (dr["schools"].ToString() != request["T_school"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "毕业学校", dr["schools"], request["T_school"]);

                if (dr["education"].ToString() != request["T_edu"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "学历", dr["education"], request["T_edu"]);

                if (dr["professional"].ToString() != request["T_professional"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "专业", dr["professional"],
                        request["T_professional"]);

                Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);

                return XhdResult.Success().ToString();
            }
            else
                return XhdResult.Error().ToString();
        }

        //changepwd
        public string changepwd()
        {
            DataSet ds = emp.GetPWD(emp_id);

            string oldpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(request["T_oldpwd"], "MD5");
            string newpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(request["T_newpwd"], "MD5");

            if (ds.Tables[0].Rows[0]["pwd"].ToString() != oldpwd)
                return XhdResult.Error("请输入正确的原密码！").ToString();

            model.pwd = newpwd;
            model.id = (emp_id);
            emp.changepwd(model);
            return XhdResult.Success().ToString();
        }

        //allchangepwd
        public string allchangepwd()
        {
            string empid = PageValidate.InputText(request["empid"], 50);

            string newpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(request["T_newpwd"], "MD5");

            model.pwd = newpwd;
            model.id = empid;
            bool ischange = emp.changepwd(model);

            if (ischange)
            {
                string EventType = "管理员修改密码";
                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventID = empid;
                string EventTitle = "";

                Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

                return XhdResult.Success().ToString();
            }
            else
                return XhdResult.Error("修改失败，系统错误！").ToString();
        }

        //del
        public string del(string id)
        {
            id = PageValidate.InputText(id, 50);
            var hp = new BLL.hr_post();

            string EventType = "员工删除";

            DataSet ds = emp.GetList($" id='{id}' ");
            if (ds.Tables[0].Rows.Count == 0)
                return XhdResult.Error("系统错误，参数不正确！").ToString();

            var customer = new BLL.CRM_Customer();
            int cc = customer.GetList($"emp_id = '{id}'").Tables[0].Rows.Count;

            if (cc > 0)
                return XhdResult.Error("此员工下含有客户，不能删除！").ToString();

            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "1D4CA3FD-7297-43FE-86AF-C3C101926508");
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }

            bool isdel = false;
            isdel = emp.Delete(id);
            //update post
            hp.UpdatePostEmpbyEid(id);

            if (isdel)
            {
                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventID = id;
                string EventTitle = ds.Tables[0].Rows[0]["name"].ToString();

                Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

                return XhdResult.Success().ToString();
            }
            return XhdResult.Error().ToString();
        }
        //combo
        public string combo()
        {
            string serchtxt = " 1=1 ";

            string did = PageValidate.InputText(request["did"], 50);
            if (PageValidate.checkID(did))
                serchtxt += $" and dep_id= '{did}'";
            else
                return "";


            DataSet ds = emp.GetList(serchtxt);

            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append("{id:'" + ds.Tables[0].Rows[i]["id"].ToString() + "',text:'" + ds.Tables[0].Rows[i]["name"] + "'},");
            }
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");

            return str.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Base64Upload()
        {
            string base64txt = request["base64img"];
            string ext = "png";
            //处理文件头
            if (string.IsNullOrEmpty(base64txt))
                return null;

            int i = base64txt.IndexOf("image") + 6;
            int j = base64txt.IndexOf("base64");

            if (i > -1 && j - i - 1 > 0)
            {
                ext = base64txt.Substring(i, j - i - 1);
            }
            base64txt = base64txt.Substring(base64txt.IndexOf(',') + 1);
            string saveFile = request["saveFile"].CString("header");

            Random rnd = new Random();
            string nowfileName = DateTime.Now.ToString("yyyyMMddHHmmss") + rnd.Next(10000, 99999) + "." + ext;

            string filetype = PageValidate.InputText(request["Type"], 50);

            string path1 = @"/file/" + saveFile;
            string path = path1 + "/" + nowfileName;

            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(path1)))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path1));
            }


            //过滤特殊字符
            string dummyData = base64txt.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
            if (dummyData.Length % 4 > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }

            StringToFile(dummyData, HttpContext.Current.Server.MapPath(path));

            return XhdResult.Success(nowfileName).ToString();
        }

        /// <summary>  
        /// 把经过base64编码的字符串保存为文件  
        /// </summary>  
        /// <param name="base64String">经base64加码后的字符串 </param>  
        /// <param name="fileName">保存文件的路径和文件名 </param>  
        /// <returns>保存文件是否成功 </returns>  
        private static bool StringToFile(string base64String, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            if (!string.IsNullOrEmpty(base64String) && File.Exists(fileName))
            {
                bw.Write(Convert.FromBase64String(base64String));
            }
            bw.Close();
            fs.Close();
            return true;
        }

        public class PostData
        {
            public string dep_id { get; set; }
            public string id { get; set; }
            public int? default_post { get; set; }
            public string position_id { get; set; }
        }
    }
}