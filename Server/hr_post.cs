
using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class hr_post : BaseCRMServer
    {
        public static BLL.hr_post post = new BLL.hr_post();
        public static Model.hr_post model = new Model.hr_post();
        private static BLL.hr_employee emp = new BLL.hr_employee();

 

        public hr_post()
        {
        }

        public hr_post(HttpContext context) : base(context) { }
        public string grid()
        {
            string depid = PageValidate.InputText(request["depid"], 50);
            string emps = PageValidate.InputText(request["empstatus"], 50);
            int empstuats = 0;
            if (PageValidate.IsNumber(emps))
                empstuats = int.Parse(emps);

            string serchtxt = $" 1=1 ";

            switch (empstuats)
            {
                case 0:
                    serchtxt += " ";
                    break;
                case 1:
                    serchtxt += " and (emp_id is null or emp_id = '')";
                    break;
                case 2:
                    serchtxt += " and (emp_id is not null and emp_id <> '') ";
                    break;
            }

            if (PageValidate.checkID(depid))
                serchtxt += $" and dep_id= '{depid}'";

            DataSet ds = post.GetList(0, serchtxt, " position_order");
            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }


        public string save()
        {
            model.dep_id = PageValidate.InputText(request["T_depname_val"], 50);
            model.post_name = PageValidate.InputText(request["T_postname"], 250);
            model.position_id = PageValidate.InputText(request["T_position_val"], 50);
            model.emp_id = PageValidate.InputText(request["T_emp_val"], 50);
            model.note = PageValidate.InputText(request["T_descript"], int.MaxValue);

            var modelemp = new Model.hr_employee();
            //更新员工岗位
            modelemp.dep_id = model.dep_id;

            modelemp.position_id = model.position_id;
            modelemp.id = model.emp_id;

            string postid = PageValidate.InputText(request["postid"], 50);
            if (PageValidate.checkID(postid))
            {
                model.id = postid;
                DataSet ds = post.GetList($"id='{postid}' ");
                DataRow dr = null;
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                dr = ds.Tables[0].Rows[0];

                //判断默认岗位
                if (model.emp_id == "")
                {
                    model.default_post = 0;
                }
                else
                {
                    DataSet ds1 = post.GetList(string.Format("default_post=1 and emp_id='{0}' and id!='{1}' ", model.emp_id, postid));
                    if (ds1.Tables[0].Rows.Count > 0)
                        model.default_post = 0; //此员工有默认岗位  
                    else
                    {
                        model.default_post = 1; //设置此岗位为此员工默认岗位 

                        //更新员工岗位
                        modelemp.post_id = model.id;
                        emp.UpdatePost(modelemp);
                    }
                }

                post.Update(model);

                //日志

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.post_name;
                string EventType = "岗位修改";
                string EventID = model.id;
                string Log_Content = null;

                if (dr["post_name"].ToString() != request["T_postname"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "岗位名称", dr["post_name"], request["T_postname"]);

                if (dr["position_name"].ToString() != request["T_position"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "岗位级别", dr["position_name"], request["T_position"]);

                if (dr["emp_name"].ToString() != request["T_emp"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "岗位员工", dr["emp_name"], request["T_emp"]);

                if (dr["note"].ToString() != request["T_descript"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "描述", dr["note"], request["T_descript"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);


            }
            else
            {
                
                postid = Guid.NewGuid().ToString().ToUpper();
                model.id = postid;
                model.create_id = emp_id;
                model.create_time = DateTime.Now;

                post.Add(model).ToString();


                //判断默认岗位
                if (model.emp_id == "")
                {
                    model.default_post = 0;
                }
                else
                {
                    DataSet ds1 = post.GetList(string.Format("default_post=1 and emp_id='{0}' and id!='{1}' ", model.emp_id, postid));
                    if (ds1.Tables[0].Rows.Count > 0)
                        model.default_post = 0; //此员工有默认岗位  
                    else
                    {
                        model.default_post = 1; //设置此岗位为此员工默认岗位 

                        //更新员工岗位
                        modelemp.post_id = postid;
                        emp.UpdatePost(modelemp);
                    }
                }
                post.UpdatePostEmp(model);
            }
            return XhdResult.Success().ToString();
        }

        //Form JSON
        public string form(string id)
        {
            id = PageValidate.InputText(id, 50);
            string dt;

            if (!PageValidate.checkID(id)) return "{}";

            DataSet ds = post.GetList($"id='{id}' ");

            dt = DataToJson.DataToJSON(ds);


            return dt;
        }

        //del
        public string del(string id)
        {
            id = PageValidate.InputText(id, 50);
            string EventType = "岗位删除";
            DataSet ds = post.GetList($" id = '{id}' ");
            if (ds.Tables[0].Rows.Count == 0)
                return XhdResult.Error("系统错误，参数不正确！").ToString();

            if (ds.Tables[0].Rows[0]["emp_id"].ToString().Length > 0)
                return XhdResult.Error("含有员工信息不能删除").ToString();

            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                bool candel = getauth.GetBtnAuthority(emp_id.ToString(), "02674D56-664F-4F76-98EA-E15D14F45612");
                if (!candel)
                    return XhdResult.Error("权限不够！").ToString();
            }

            bool isdel = post.Delete(id);
            if (isdel)
            {
                
                    string UserID = emp_id;
                    string UserName = emp_name;
                    string IPStreet = request.UserHostAddress;
                    string EventID = id;
                    string EventTitle = ds.Tables[0].Rows[0]["post_name"].ToString();

                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

                return XhdResult.Success().ToString();
            }
            return XhdResult.Error().ToString();
        }

        //get post by empid
        public string getpostbyempid(string id)
        {
            id = PageValidate.InputText(id, 50);
            var hp = new BLL.hr_post();
            DataSet ds = hp.GetList($" emp_id='{id}'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
                return dt;
            }
            return null;
        }

        //serch
        public string serch(string serchtxt)
        {
            var hp = new BLL.hr_post();
            serchtxt = PageValidate.InputText(serchtxt, 255);
            DataSet ds = hp.GetList($" post_name like N'%{ serchtxt }%' ");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
                return dt;
            }
            return ("{Rows:[],Total:0}");
        }

        //post_emp
        public void postemp()
        {
            string json = request["PostData"].ToLower();
            var js = new JavaScriptSerializer();

            PostData[] postdata;
            postdata = js.Deserialize<PostData[]>(json);

            var hp = new BLL.hr_post();

            string empid = PageValidate.InputText(request["empid"], 50);

            var modelemp = new Model.hr_employee();
            model.emp_id = empid;
            modelemp.id = empid;

            for (int i = 0; i < postdata.Length; i++)
            {
                model.id = postdata[i].Post_id;
                model.default_post = postdata[i].Default_post;

                if (postdata[i].Default_post == 1)
                {
                    modelemp.dep_id = postdata[i].Dep_id;
                    modelemp.position_id = postdata[i].Position_id;
                    modelemp.post_id = postdata[i].Post_id;
                    //context.Response.Write(postdata[i].Depname + "@");
                    emp.UpdatePost(modelemp);
                }

                hp.UpdatePostEmp(model);
            }
        }

        //combo
        public string combo(string id)
        {
            id = PageValidate.InputText(id, 50);
            DataSet ds = post.GetList($" dep_id='{id}' ");

            var str = new StringBuilder();

            str.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append("{id:" + ds.Tables[0].Rows[i]["id"] + ",text:'" + ds.Tables[0].Rows[i]["post_name"] + "'},");
            }
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");

            return str.ToString();
        }

        public class PostData
        {
            public string Post_id { get; set; }
            public string Post_name { get; set; }
            public string Emp_id { get; set; }
            public string Emp_name { get; set; }
            public int? Default_post { get; set; }
            public string Dep_id { get; set; }
            public string Depname { get; set; }
            public string Position_id { get; set; }
            public string Position_name { get; set; }
        }
    }
}