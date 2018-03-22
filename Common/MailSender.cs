using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Xml;
using OpenSmtp.Mail;
using Attachment = System.Net.Mail.Attachment;
using MailMessage = System.Net.Mail.MailMessage;
using Mailpriority = System.Net.Mail.MailPriority;

namespace XHD.Common
{
    public class MailSender
    {
        private static readonly string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
        private static readonly string userName = ConfigurationManager.AppSettings["UserName"];
        private static readonly string pwd = ConfigurationManager.AppSettings["Pwd"];
        private static readonly int smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
        private static readonly string authorName = ConfigurationManager.AppSettings["AuthorName"];
        private static readonly string to = ConfigurationManager.AppSettings["To"];

        public static void Send(string server, string sender, string recipient, string subject, string body,bool isBodyHtml, Encoding encoding, bool isAuthentication, params string[] files)
        {
            var smtpClient = new SmtpClient(server);
            var message = new MailMessage(sender, recipient);

            message.Priority = Mailpriority.High;
            message.IsBodyHtml = isBodyHtml; //html format

            message.SubjectEncoding = encoding; //encoding
            message.BodyEncoding = encoding; //encoding

            message.Subject = subject; //title
            message.Body = body; //content

            message.Attachments.Clear();


            if (files != null && files.Length != 0)
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    var attach = new Attachment(files[i]);
                    message.Attachments.Add(attach);
                }
            }

            if (isAuthentication)
            {
                smtpClient.Credentials = new NetworkCredential(SmtpConfig.Create().SmtpSetting.User,SmtpConfig.Create().SmtpSetting.Password);
            }
            smtpClient.Send(message);
        }

        public static void Send(string recipient, string subject, string body)
        {
            Send(SmtpConfig.Create().SmtpSetting.Server, SmtpConfig.Create().SmtpSetting.Sender, recipient, subject,body, true, Encoding.Default, true, null);
        }

        public static void Send(string Recipient, string Sender, string Subject, string Body)
        {
            Send(SmtpConfig.Create().SmtpSetting.Server, Sender, Recipient, Subject, Body, true, Encoding.UTF8, true,null);
        }


        public void Send(string subject, string body)
        {
            List<string> toList = StringPlus.GetSubStringList(StringPlus.ToDBC(to), ',');
            var smtp = new Smtp(smtpServer, userName, pwd, smtpPort);
            foreach (string s in toList)
            {
                var msg = new OpenSmtp.Mail.MailMessage();
                msg.From = new EmailAddress(userName, authorName);

                msg.AddRecipient(s, AddressType.To);

                //设置邮件正文,并指定格式为 html 格式
                msg.HtmlBody = body;
                //设置邮件标题
                msg.Subject = subject;
                //指定邮件正文的编码
                msg.Charset = "gb2312";
                //发送邮件
                smtp.SendMail(msg);
            }
        }
    }

    public class SmtpSetting
    {
        public string Server { get; set; }

        public bool Authentication { get; set; }

        public string User { get; set; }

        public string Sender { get; set; }

        public string Password { get; set; }
    }

    public class SmtpConfig
    {
        private static SmtpConfig _smtpConfig;

        private SmtpConfig()
        {
        }

        private string ConfigFile
        {
            get
            {
                string configPath = ConfigurationManager.AppSettings["SmtpConfigPath"];
                if (string.IsNullOrEmpty(configPath) || configPath.Trim().Length == 0)
                {
                    configPath = HttpContext.Current.Request.MapPath("/Config/SmtpSetting.config");
                }
                else
                {
                    if (!Path.IsPathRooted(configPath))
                        configPath = HttpContext.Current.Request.MapPath(Path.Combine(configPath, "SmtpSetting.config"));
                    else
                        configPath = Path.Combine(configPath, "SmtpSetting.config");
                }
                return configPath;
            }
        }

        public SmtpSetting SmtpSetting
        {
            get
            {
                var doc = new XmlDocument();
                doc.Load(ConfigFile);
                var smtpSetting = new SmtpSetting();
                smtpSetting.Server = doc.DocumentElement.SelectSingleNode("Server").InnerText;
                smtpSetting.Authentication = Convert.ToBoolean(doc.DocumentElement.SelectSingleNode("Authentication").InnerText);
                smtpSetting.User = doc.DocumentElement.SelectSingleNode("User").InnerText;
                smtpSetting.Password = doc.DocumentElement.SelectSingleNode("Password").InnerText;
                smtpSetting.Sender = doc.DocumentElement.SelectSingleNode("Sender").InnerText;

                return smtpSetting;
            }
        }

        public static SmtpConfig Create()
        {
            if (_smtpConfig == null)
            {
                _smtpConfig = new SmtpConfig();
            }
            return _smtpConfig;
        }
    }
}