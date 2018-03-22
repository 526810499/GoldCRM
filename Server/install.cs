
using System;
using System.Reflection;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using XHD.Common;
using XHD.Common.DEncrypt;
using System.Configuration;

namespace XHD.Server
{
    public class install
    {
        public HttpContext Context;
        public HttpRequest request;

        public install()
        {
        }

        public install(HttpContext context)
        {
            Context = context;
            request = context.Request;
        }

        public string initcheck()
        {
            //conn.config check
            string filename = @"\conn.config";
            int check_config = CheckConfig(filename);

            //floder check
            int check_floder = CheckFolder("install");
            int check_datafloder = CheckFolder("App_Data");
            int check_configed = configed();


            return (string.Format("{0},{1},{2},{3}", check_config, check_floder, check_datafloder, check_configed));
        }

        public string checkconnect()
        {
            string servername = PageValidate.InputText(request["t_name"], 255);
            string uid = PageValidate.InputText(request["t_uid"], 255);
            string pwd = PageValidate.InputText(request["t_pwd"], 255);
            string dbname = PageValidate.InputText(request["t_db_name"], 255);

            string connstr1 = string.Format("server={0};uid={1};pwd={2}", servername, uid, pwd);
            string connstr2 = string.Format("server={0};uid={1};pwd={2};database={3}", servername, uid, pwd, dbname);

            int check_configed = configed();
            if (check_configed == 1)
            {
                return ("false:configed");
            }
            try
            {
                var myconn1 = new SqlConnection(connstr1);
                myconn1.Open();
                try
                {
                    var myconn2 = new SqlConnection(connstr2);
                    myconn2.Open();

                    return ("warn:dbname");
                }
                catch
                {
                    int check_datafile = CheckFile(Context.Server.MapPath(@"\App_Data\\" + dbname + ".mdf"));
                    if (check_datafile == 0)
                        return ("success");
                    return ("false:dbfile");
                }
            }
            catch
            {
                return ("false:connect");
            }
        }

        public void startconfig()
        {
            string servername = PageValidate.InputText(request["t_name"], 255);
            string uid = PageValidate.InputText(request["t_uid"], 255);
            string pwd = PageValidate.InputText(request["t_pwd"], 255);
            string dbname = PageValidate.InputText(request["t_db_name"], 255);
            string Encrypt = PageValidate.InputText(request["t_Encrypt_val"], 255);

            string connstr1 = string.Format("server={0};uid={1};pwd={2}", servername, uid, pwd);
            string connstr2 = string.Format("server={0};uid={1};pwd={2};database={3}", servername, uid, pwd, dbname);

            string sqlencrypt = "false";
            string regconnstr = connstr2;
            if (Encrypt == "0")
            {
                sqlencrypt = "true";
                regconnstr = DESEncrypt.Encrypt(connstr2);
            }

            //创建数据库测试
            var myconn1 = new SqlConnection(connstr1);
            string physicsPath = Context.Server.MapPath(@"\App_Data\\");
            string sql = "";

            sql += "if DB_ID('" + dbname + "') is null ";
            sql += " CREATE DATABASE [" + dbname + "] ON  PRIMARY ";
            sql += "( NAME = N'" + dbname + "', FILENAME = N'" + physicsPath + dbname +".mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )  ";
            sql += " LOG ON        ";
            sql += "( NAME = N'" + dbname + "_log', FILENAME = N'" + physicsPath + dbname + ".ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)  ";
            sql += "  COLLATE Chinese_PRC_CI_AS";

            var cmd = new SqlCommand(sql, myconn1);
            myconn1.Open();
            int ex = cmd.ExecuteNonQuery();
            myconn1.Close();

            //context.Response.Write(ex);

            //执行sql
            var sb = new StringBuilder();
            using (var objReader = new StreamReader(Context.Server.MapPath(@"\install\install.sql"), Encoding.UTF8))
            {
                sb.Append(objReader.ReadToEnd());
                objReader.Close();
            }
            string commandText = sb.ToString();
            string splitter = "\r\nGO\r\n";

            int startPos = 0;
            do
            {
                int lastPos = commandText.IndexOf(splitter, startPos);
                int len = (lastPos > startPos ? lastPos : commandText.Length) - startPos;
                string query = commandText.Substring(startPos, len);

                if (query.Trim().Length > 0)
                {
                    query = "USE [" + dbname + "] " + query;
                    var cmdtable = new SqlCommand(query, myconn1);
                    myconn1.Open();
                    cmdtable.ExecuteNonQuery();
                    myconn1.Close();
                }

                if (lastPos == -1)
                    break;
                startPos = lastPos + splitter.Length;
            } while (startPos < commandText.Length);


            //保存连接字符串
            saveconfig(sqlencrypt, regconnstr);
            //DBUtility.DbHelperSQL.connectionString = connstr2;
        }

        //检查更新配置文件
        public int CheckConfig(string webconfigfile)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                File.SetLastWriteTime(context.Server.MapPath(webconfigfile), DateTime.Now);

                return 1;
            }
            catch(Exception ex)
            {
                XHD.Controller.IO.LogManager.Add(ex.ToString());
                return 0;
            }
        }

        public static int CheckFolder(string foldername)
        {
            HttpContext context = HttpContext.Current;
            string physicsPath = context.Server.MapPath(@"\" + foldername);
            try
            {
                using (
                    var fs = new FileStream(physicsPath + "\\a.txt", FileMode.Create, FileAccess.ReadWrite,
                        FileShare.ReadWrite))
                {
                    fs.Close();
                }
                if (File.Exists(physicsPath + "\\a.txt"))
                {
                    File.Delete(physicsPath + "\\a.txt");
                    return 1;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public static int CheckFile(string filename)
        {
            bool ex = File.Exists(filename);
            if (ex)
                return 1;
            return 0;
        }

        public static void saveconfig(string sqlencrypt, string regconnstr)
        {
            //保存连接字符串
            var webconfigDoc = new XmlDocument();
            string filePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\conn.config";
            string xPath = "/appSettings/add[@key='?']";
            webconfigDoc.Load(filePath);
            XmlNode regkey1 = webconfigDoc.SelectSingleNode(xPath.Replace("?", "ConStringEncrypt"));
            XmlNode regkey2 = webconfigDoc.SelectSingleNode(xPath.Replace("?", "ConnectionString"));
            XmlNode regkey3 = webconfigDoc.SelectSingleNode(xPath.Replace("?", "CompleteConfig"));
            string RegKeyString1 = regkey1.Attributes["value"].InnerText;
            string RegKeyString2 = regkey2.Attributes["value"].InnerText;
            string RegKeyString3 = regkey3.Attributes["value"].InnerText;

            regkey1.Attributes["value"].InnerText = sqlencrypt;
            regkey2.Attributes["value"].InnerText = regconnstr;
            regkey3.Attributes["value"].InnerText = "true";
            webconfigDoc.Save(filePath);
        }

        public int configed()
        {
            //string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            try
            {
                var webconfigDoc = new XmlDocument();
                string filePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\conn.config";
                string xPath = "/appSettings/add[@key='?']";
                webconfigDoc.Load(filePath);
                XmlNode regkey1 = webconfigDoc.SelectSingleNode(xPath.Replace("?", "CompleteConfig"));
                string RegKeyString1 = regkey1.Attributes["value"].InnerText;

                if (RegKeyString1 == "true")
                    return 1;
                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}