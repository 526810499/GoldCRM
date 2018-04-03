
using System;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class public_news : BaseCRMServer
    {
        public static BLL.public_news news = new BLL.public_news();
        public static Model.public_news model = new Model.public_news();

 

        public public_news()
        {
        }

        public public_news(HttpContext context) : base(context) { }
        public string save()
        {
            model.news_title = PageValidate.InputText(request["T_title"], 255);
            model.news_content = PageValidate.InputText(request["T_content"], int.MaxValue);

            string nid = PageValidate.InputText(request["nid"], 50);
            if (PageValidate.checkID(nid))
            {
                DataSet ds = news.GetList($"id = '{nid}' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                model.id = nid;

                news.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.news_title;
                string EventType = "新闻修改";
                string EventID = model.id;
                string Log_Content = null;

                if (dr["news_title"].ToString() != request["T_title"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "新闻标题", dr["news_title"], request["T_title"]);

                if (dr["news_content"].ToString() != request["T_content"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "新闻内容", "原内容被修改", "原内容被修改");

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                model.create_time = DateTime.Now;
                model.create_id = emp_id;
                model.id = Guid.NewGuid().ToString();
                
                news.Add(model);
            }
            return XhdResult.Success().ToString();
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
                sortorder = "desc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total, serchtxt = $" 1=1 ";

            if (!string.IsNullOrEmpty(request["sstart"]))
                serchtxt += " and create_time >= '" + PageValidate.InputText(request["sstart"], 50) + "'";

            if (!string.IsNullOrEmpty(request["sdend"]))
            {
                DateTime enddate = DateTime.Parse(request["sdend"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                serchtxt += " and create_time  <= '" + enddate + "'";
            }

            if (!string.IsNullOrEmpty(request["stext"]))
            {
                if (request["stext"] != "输入关键词搜索")
                    serchtxt += " and news_title like N'%" + PageValidate.InputText(request["stext"], 500) + "%'";
            }

            DataSet ds = news.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

            return (GetGridJSON.DataTableToJSON1(ds.Tables[0], Total));
        }

        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);
            DataSet ds = news.GetList($"id = '{id}' ");
            string dt = DataToJson.DataToJSON(ds);

            return dt;
        }

        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();

            bool canDel = false;
            if (uid != "admin")
            {
                var getauth = new GetAuthorityByUid();
                canDel = getauth.GetBtnAuthority(emp_id.ToString(), "5CE532AF-69BE-4FB0-B32F-52F17A6FFD80");
                if (!canDel)
                    return XhdResult.Error("权限不够！").ToString();
            }

            id = PageValidate.InputText(id, 50);
            DataSet ds = news.GetList($"id='{id}' ");

            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            bool isdel = news.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误！").ToString();

            //日志
            string EventType = "新闻删除";
            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["news_title"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();
        }

        public string newsremind()
        {
            DataSet ds = news.GetList(7, $" 1=1 ", " create_time desc");
            string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
            return dt;
        }
    }
}