
using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Tool_batch
    {
        public static BLL.Tool_batch batch = new BLL.Tool_batch();
        public static Model.Tool_batch model = new Model.Tool_batch();

        public HttpContext Context;
        public string emp_id;
        public string emp_name;
        public Model.hr_employee employee;
        public HttpRequest request;
        public string uid;
        

        public Tool_batch()
        {
        }

        public Tool_batch(HttpContext context)
        {
            Context = context;
            request = context.Request;

            var userinfo = new User_info();
            employee = userinfo.GetCurrentEmpInfo(context);

            emp_id = employee.id;
            emp_name = PageValidate.InputText(employee.name, 50);
            uid = PageValidate.InputText(employee.uid, 50);
            
        }


        public void save()
        {
            model.batch_type = PageValidate.InputText(request["type"], 50);
            model.b_count = 0;
            model.o_emp_id = PageValidate.InputText(request["T_employee1_val"], 50);
            model.c_emp_id = PageValidate.InputText(request["T_employee2_val"], 50);

            model.id = Guid.NewGuid().ToString();
            model.create_id = emp_id;
            model.create_time = DateTime.Now;
            

            string batch_filter = "";

            string serchtxt = " isDelete=0 ";

            if (!string.IsNullOrEmpty(request["T_employee1_val"]))
                serchtxt += string.Format(" and emp_id='{0}'", PageValidate.InputText(request["T_employee1_val"], 50));

            if (PageValidate.checkID(request["industry_val"]))
            {
                serchtxt += $" and industry_id = '{PageValidate.InputText(request["industry_val"], 50)}'";
                batch_filter += $"【行业】{PageValidate.InputText(request["industry"], 50)}\n";
            }

            if (PageValidate.checkID(request["customertype_val"]))
            {
                serchtxt += $" and type_id = '{PageValidate.InputText(request["customertype_val"], 50)}' ";
                batch_filter += $"【客户类别】{PageValidate.InputText(request["customertype"], 50)}\n";
            }

            if (PageValidate.checkID(request["customerlevel_val"]))
            {
                serchtxt += $" and Level_id = '{PageValidate.InputText(request["customerlevel_val"], 50)}' ";
                batch_filter += $"【客户级别】{PageValidate.InputText(request["customerlevel"], 50)}\n";
            }

            if (PageValidate.checkID(request["cus_sourse_val"]))
            {
                serchtxt += $" and Source_id = '{PageValidate.InputText(request["cus_sourse_val"], 50)}' ";
                batch_filter += $"【客户来源】{PageValidate.InputText(request["cus_sourse"], 50)}\n";
            }

            if (!string.IsNullOrEmpty(request["startdate"]))
            {
                serchtxt += $" and create_time >= '{PageValidate.InputText(request["startdate"], 255)}'";
                batch_filter += $"【创建时间】>{PageValidate.InputText(request["startdate"], 50)}\n";
            }

            if (!string.IsNullOrEmpty(request["enddate"]))
            {
                DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and create_time <= '{enddate}'";
                batch_filter += $"【创建时间】<{enddate}\n";
            }

            if (!string.IsNullOrEmpty(request["startfollow"]))
            {
                serchtxt += $" and lastfollow >= '{PageValidate.InputText(request["startfollow"], 255)}'";
                batch_filter += $"【跟进时间】{PageValidate.InputText(request["startfollow"], 50)}\n";
            }

            if (!string.IsNullOrEmpty(request["endfollow"]))
            {
                DateTime enddate = DateTime.Parse(request["endfollow"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += $" and lastfollow <= '{enddate}'";
                batch_filter += $"【跟进时间】{enddate}\n";
            }


            var customer = new BLL.CRM_Customer();
            var model_cus = new Model.CRM_Customer();

            if (string.IsNullOrEmpty(batch_filter)) batch_filter = "全部";

            model.batch_filter = PageValidate.InputText( batch_filter,int.MaxValue);
            model.b_count = customer.GetList(string.Format("emp_id='{0}' and {1}", model.o_emp_id, serchtxt)).Tables[0].Rows.Count;

            model_cus.emp_id = model.c_emp_id;
            model_cus.create_id = model.o_emp_id;

            customer.Update_batch(model_cus, serchtxt);

            batch.Add(model);
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
            string serchtxt = "1=1";

            //context.Response.Write(serchtxt);

            DataSet ds = batch.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return dt;
        }
    }
}
