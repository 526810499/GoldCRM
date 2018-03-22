
using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using XHD.Common;

namespace XHD.Controller
{
    public class XHDIO
    { 
        public static void DelDirectoryAndFile(string filepath)
        {
            //获取文件夹
            string path = HttpContext.Current.Server.MapPath(filepath);
            //获取文件夹中所有图片
            if (Directory.GetFileSystemEntries(path).Length > 0)
            {
                //遍历文件夹中所有文件
                foreach (string file in Directory.GetFiles(path))
                {
                    //文件己存在
                    if (File.Exists(file))
                    {
                        FileInfo fi = new FileInfo(file);
                        //判断当前文件属性是否是只读
                        if (fi.Attributes.ToString().IndexOf("ReadyOnly") >= 0)
                        {
                            fi.Attributes = FileAttributes.Normal;
                        }
                        //删除文件
                        File.Delete(file);
                    }
                }
                //删除文件夹
                Directory.Delete(path);

            }
        }

        public static void DelFile(string filepath)
        {
            string path = HttpContext.Current.Server.MapPath(filepath);

            FileInfo fi = new FileInfo(path);
            //判断当前文件属性是否是只读
            if (fi.Attributes.ToString().IndexOf("ReadyOnly") >= 0)
            {
                fi.Attributes = FileAttributes.Normal;
            }
            //删除文件
            File.Delete(path);
        }
    }
}
