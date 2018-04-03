using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System.Web.Script.Serialization;

namespace XHD.Server
{
    public class Personal_queckmenu : BaseCRMServer
    {
        public static BLL.Personal_queckmenu menu = new BLL.Personal_queckmenu();
        public static Model.Personal_queckmenu model = new Model.Personal_queckmenu();

 
        public Personal_queckmenu()
        {
        }

        public Personal_queckmenu(HttpContext context) : base(context) { }

        public string get()
        {
            DataSet ds = menu.GetList($" user_id = '{emp_id}'");
            return GetGridJSON.DataTableToJSON(ds.Tables[0]);
        }

        public void set()
        {
            string json = request["PostData"].ToLower();
            var js = new JavaScriptSerializer();

            PostData[] postdata;
            postdata = js.Deserialize<PostData[]>(json);

            model.user_id = emp_id;

            for (int i = 0; i < postdata.Length; i++)
            {
                model.menu_id = postdata[i].Menu_id;

                //if (postdata[i].__status == "add")
                //{
                //    menu.Add(model);
                //}
                //else 
                if (postdata[i].__status == "delete")
                {
                    menu.Delete(model);
                }
                else
                {
                    menu.Add(model);
                }
            }
        }

        public class PostData
        {            
            public string Menu_id { get; set; }
            public string __status { get; set; }
        }
    }
}
