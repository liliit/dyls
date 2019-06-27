
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using DYLS.IDal;



namespace DYLS.Common.Utils
{
    /// <summary>
    /// EmailHelper
    /// </summary>
    public class EmailHelper
    {
        /// <summary>
        /// Send
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="to"></param>
        public static void Send(string title, string content, string to)
        {
            var from = ConfigHelper.Get(ConfigHelper.MailFrom);
            var fromMail = ConfigHelper.Get(ConfigHelper.MailFromMail);
            var smtpAddress = ConfigHelper.Get(ConfigHelper.MailSmtp);
            var account = ConfigHelper.Get(ConfigHelper.MailSmtpAccount);
            var pwd = ConfigHelper.Get(ConfigHelper.MailSmtpPwd);

            var mail = new MailMessage(fromMail, to);
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Subject = title;
            mail.IsBodyHtml = true; //是否允许内容为 HTML 格式
            mail.BodyEncoding = Encoding.UTF8;
            mail.Body = content;
            mail.From = new MailAddress(fromMail, from, Encoding.UTF8);

            var smtp = new SmtpClient(smtpAddress);
            smtp.Credentials = new NetworkCredential(account, pwd);

            var success = false;
            try
            {
                smtp.Send(mail);
                success = true;
            }
            catch
            {
                // ignored
            }

            mail.Dispose();
            smtp.Dispose();
        }
    }
}
