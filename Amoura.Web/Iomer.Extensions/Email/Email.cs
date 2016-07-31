using System;
using System.Web;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Amoura.Web.Extensions;

namespace Amoura.Extensions.Email
{
    public static class Email
    {
        public static EmailStatus SendEmail(string toEmail, string fromEmail, string subject, string emailMessage, string emailTemplate, ListDictionary replacements, string emailBcc = "")
        {
            var emailSent = new EmailStatus { EmailSent = false, ErrorMessage = "" };

            try
            {
                var md = new MailDefinition
                {
                    From = fromEmail,
                    IsBodyHtml = true,
                    Subject = subject
                };

                string body;
                var filePath = HttpContext.Current.Request.PhysicalApplicationPath;
                using (var sr = new StreamReader(filePath + @emailTemplate))
                {
                    body = sr.ReadToEnd();
                }

                body = body.Replace("&lt;&lt;EmailMessage&gt;&gt;", emailMessage);
                //rich text content may have links to media items. relative links won't work in email so try to prepend domain
                body = body.Replace("href=\"/media/\"", string.Format("href=\"{0}/media/\"", WebExtensions.GetDomain()));

                var msg = md.CreateMailMessage(toEmail, replacements, body, new System.Web.UI.Control());

                if (!string.IsNullOrEmpty(emailBcc))
                {
                    var bccSplit = emailBcc.Split(';').Where(i => i.Contains("@")).ToList();
                    if (bccSplit.Any())
                    {
                        foreach (var bccEmail in bccSplit)
                        {
                            msg.Bcc.Add(new MailAddress(bccEmail));
                        }
                    }
                }

                var smtp = new SmtpClient();
                smtp.Send(msg);
                emailSent.EmailSent = true;
                emailSent.ErrorMessage = "";
            }
            catch (Exception ex)
            {
                emailSent.EmailSent = false;
                emailSent.ErrorMessage = ex.Message;
            }

            return emailSent;
        }

        public static EmailStatus SendEmail(MailMessage msg)
        {
            var emailSent = new EmailStatus { EmailSent = false, ErrorMessage = "" };

            try
            {
                var smtp = new SmtpClient();
                smtp.Send(msg);
                emailSent.EmailSent = true;
                emailSent.ErrorMessage = "";
            }
            catch (Exception ex)
            {
                emailSent.EmailSent = false;
                emailSent.ErrorMessage = ex.Message;
            }

            return emailSent;
        }

        public static MailMessage GetMailMessage(string toEmail, string fromEmail, string subject, string emailTemplate, ListDictionary replacements)
        {
            string body;
            var filePath = HttpContext.Current.Request.PhysicalApplicationPath;
            using (var sr = new StreamReader(filePath + @emailTemplate))
            {
                body = sr.ReadToEnd();
            }

            var md = new MailDefinition
            {
                From = fromEmail,
                IsBodyHtml = true,
                Subject = subject
            };
            var msg = md.CreateMailMessage(toEmail, replacements, body, new System.Web.UI.Control());
            return msg;
        }

        public struct EmailStatus
        {
            public bool EmailSent { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}
