

using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class upload : BaseCRMServer
    {
 

        public upload()
        {
        }

        public upload(HttpContext context) : base(context) { }

        public string upfiles(string ftype)
        {
            string success = "";
            HttpPostedFile uploadFile = request.Files[0];
            string fileName = uploadFile.FileName;
            //string sExt = fileName.Substring(fileName.LastIndexOf(".")).ToLower();

            switch (ftype)
            {
                case "tmp_headimg":success = headimg(uploadFile);
                    break;
                case "cus_import":success = cus_import(uploadFile);
                    break;
                case "contact_import": success = contact_import(uploadFile);
                    break;
                case "contract": success = contract(uploadFile);
                    break;
                case "logo":success = logo(uploadFile);
                    break;
                default:
                    success = "";
                    break;
            }

            return success;
        }

        public string headimg(HttpPostedFile uploadFile)
        {
            Random rnd = new Random();
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + rnd.Next(10000, 99999)+".png";
            uploadFile.SaveAs(Context.Server.MapPath(@"~/images/upload/temp/" + filename));
            return (filename);
        }

        public string cus_import(HttpPostedFile uploadFile)
        {
            string fileName = uploadFile.FileName;

            string nowfileName = "Customer.xls";

            uploadFile.SaveAs(Context.Server.MapPath(@"~/file/customer/" + nowfileName));

            return (nowfileName);
        }
        public string contact_import(HttpPostedFile uploadFile)
        {
            string fileName = uploadFile.FileName;

            string nowfileName = "contact.xls" ;

            uploadFile.SaveAs(Context.Server.MapPath(@"~/file/contact/" + nowfileName));

            return (nowfileName);
        }
        public string contract(HttpPostedFile uploadFile)
        {

            string filename = uploadFile.FileName;
            string sExt = filename.Substring(filename.LastIndexOf(".")).ToLower();
            DateTime now = DateTime.Now;
            string nowfileName = now.ToString("yyyyMMddHHmmss") + Assistant.GetRandomNum(6) + sExt;

            uploadFile.SaveAs(HttpContext.Current.Server.MapPath(@"~/file/contract/" + nowfileName));
            return nowfileName;
        }

        public string logo(HttpPostedFile uploadFile)
        {

            string filename = uploadFile.FileName;
            string sExt = filename.Substring(filename.LastIndexOf(".")).ToLower();
            DateTime now = DateTime.Now;
            string nowfileName = now.ToString("yyyyMMddHHmmss") + Assistant.GetRandomNum(6) + sExt;

            uploadFile.SaveAs(HttpContext.Current.Server.MapPath(@"~/images/logo/" + nowfileName));

            //sysinfo
            BLL.Sys_info info = new BLL.Sys_info();
            Model.Sys_info model = new Model.Sys_info();

            model.sys_value = "images/logo/" + nowfileName;
            model.sys_key = "sys_logo";

            info.Update(model);
            //info.logo(nowfileName);

            return nowfileName;
        }

        public string upheadimg()
        {
            int x1 = int.Parse(request["x1"]);
            int y1 = int.Parse(request["y1"]);
            int w = int.Parse(request["w"]);
            int h = int.Parse(request["h"]);

            Random rnd = new Random();
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + rnd.Next(10000, 99999)+ ".png";
            string fullname = $"file/header/{ filename }";

            string filepath = Context.Server.MapPath($"~/file/header/");
            string fullpath = Context.Server.MapPath($"~/file/header/{ filename }");

            if (!Directory.Exists(Path.GetDirectoryName(filepath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            }

            string fileName = request["txtFileName"];

            var page = new Page();

            string oldpath = page.Server.MapPath(@"~/images/upload/temp/" + fileName);

            Image originalImg = Image.FromFile(oldpath);

            ZoomImage.SaveCutPic(oldpath, fullpath, 0, 0, w, h, x1, y1,Convert.ToInt32(300 * originalImg.Width / originalImg.Height), 300);

            originalImg.Dispose();

            File.Delete(oldpath);

            string json = $"{{filename:'{filename}',filepath:'{fullname}'}}";
            return json;
        }


    }
}