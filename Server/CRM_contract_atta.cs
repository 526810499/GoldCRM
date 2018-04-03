using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;
using System;
using System.IO;
using System.Text;

namespace XHD.Server
{
    public class CRM_contract_atta : BaseCRMServer
    {
        private static BLL.CRM_contract_atta atta = new BLL.CRM_contract_atta();
        private static Model.CRM_contract_atta model = new Model.CRM_contract_atta();

 
        public CRM_contract_atta()
        {
        }

        public CRM_contract_atta(HttpContext context) : base(context) { }

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
            string serchtxt = $" ";

            if (PageValidate.checkID(request["contract_id"]))
                serchtxt += $" contract_id='{PageValidate.InputText(request["contract_id"], 50)}'";
            else
                serchtxt += "1=2";


            //context.Response.Write(serchtxt);
            DataSet ds = atta.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);

            return dt;
        }

        public string upload()
        {
            HttpPostedFile uploadFile = request.Files[0];
            string filename = uploadFile.FileName;
            string sExt = filename.Substring(filename.LastIndexOf(".")).ToLower();
            DateTime now = DateTime.Now;
            string nowfileName = now.ToString("yyyyMMddHHmmss") + Assistant.GetRandomNum(6) + sExt;

            if (!PageValidate.checkID(request["contract_id"]))
                return XhdResult.Error("没有正确的合同ID").ToString();

            var contract_id = PageValidate.InputText(request["contract_id"], 50);

            string ServerUrl = $"~/file/contract/{contract_id}/";
            var savePath = HttpContext.Current.Server.MapPath(ServerUrl);

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(savePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                }
            }
            catch (Exception ex)
            {
                Controller.IO.LogManager.Add(ex.ToString());
            }

            string path = $"~/file/contract/{contract_id}/" + nowfileName;

            uploadFile.SaveAs(HttpContext.Current.Server.MapPath(path));

            model.id = Guid.NewGuid().ToString();
            model.contract_id = contract_id;
            model.file_name = PageValidate.InputText(filename, 250);
            model.real_name = nowfileName;
            model.file_size = uploadFile.ContentLength;

            model.create_id = emp_id;
            model.create_time = DateTime.Now;

            atta.Add(model);

            return XhdResult.Success().ToString();
        }

        public string del()
        {
            string id = PageValidate.InputText(request["id"], 50);

            if (!PageValidate.checkID(id))
                return XhdResult.Error("参数错误！").ToString();

            DataSet ds = atta.GetList($"id = '{id}'");

            if (ds.Tables[0].Rows.Count == 0)
                return XhdResult.Error("找不到此数据，请检查！").ToString();

            DataRow dr = ds.Tables[0].Rows[0];

            string contract_id = dr["contract_id"].ToString();
            string real_name = dr["real_name"].ToString();

            string path = $"~/file/contract/{contract_id}/" + real_name;

            string realpath = HttpContext.Current.Server.MapPath(path);

            if (File.Exists(realpath))
            {
                FileInfo fi = new FileInfo(realpath);
                //判断当前文件属性是否是只读
                if (fi.Attributes.ToString().IndexOf("ReadyOnly") >= 0)
                {
                    fi.Attributes = FileAttributes.Normal;
                }
                //删除文件
                File.Delete(realpath);
            }

            atta.Delete(id);

            return XhdResult.Success().ToString();
        }
    }
}
