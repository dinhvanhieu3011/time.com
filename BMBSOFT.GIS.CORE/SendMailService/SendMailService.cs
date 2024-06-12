using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using BASE.CORE.Helper;
using BASE.CORE.Resource;
using Microsoft.Extensions.Hosting;

namespace BASE.CORE.SendMailService
{
    public interface ISendMailService
    {
        bool SendsToEmail(string[] sendTos, string ccTo, string subject, string body, ref string returnMessage);
        bool SendToEmail(string sendTo, string ccTo, string subject, string body, ref string returnMessage);
        bool IsValidEmail(string email);
        string ConvertBase64Mail(string emailContent);
    }
    public class SendMailService : ISendMailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IHostEnvironment _env;
        public SendMailService(EmailConfiguration emailConfiguration, IHostEnvironment env)
        {
            _emailConfig = emailConfiguration;
            _env = env;
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            const string pattern = "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

            return Regex.IsMatch(email, pattern);
        }

        public bool SendToEmail(string sendTo, string ccTo,
            string subject,
            string body,
            ref string returnMessage)
        {
            if (returnMessage == null) throw new ArgumentNullException(nameof(returnMessage));
            try
            {
                if (string.IsNullOrWhiteSpace(sendTo))
                {
                    returnMessage = StringMessage.ErrorMessages.EmailNotEmpty;
                    return false;
                }

                var result = IsValidEmail(sendTo);
                if (result == false)
                {
                    returnMessage = StringMessage.ErrorMessages.EmailNotValid;
                    return false;
                }
                if (string.IsNullOrWhiteSpace(subject))
                {
                    returnMessage = StringMessage.ErrorMessages.TitleNotEmpty;
                    return false;
                }
                //TODO: config send mail smtp

                var smtp = new SmtpClient();
                var msg = new MailMessage
                {
                    From = new MailAddress(_emailConfig.EmailFrom, ""),
                    Subject = subject
                };
                msg.To.Add(sendTo);
                msg.IsBodyHtml = true;
                msg.Body = body;

                if (!string.IsNullOrEmpty(ccTo))
                {
                    msg.CC.Add(ccTo);
                }
                //msg.IsBodyHtml = true;
                smtp.UseDefaultCredentials = false;
                smtp.Host = _emailConfig.EmailHost;
                smtp.Credentials = new NetworkCredential(_emailConfig.EmailUser, _emailConfig.EmailPass);
                smtp.Port = _emailConfig.EmailPost; 
                smtp.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                smtp.Send(msg);                
                return true;
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
                return false;
            }
        }

        public bool SendsToEmail(string[] sendTos, string ccTo, string subject, string body, ref string returnMessage)
        {
            var retVal = false;
            try
            {
                foreach (var sendTo in sendTos)
                {
                    var message = "";
                    retVal = SendToEmail(sendTo, ccTo, subject, body, ref message);
                }
            }
            catch (Exception)
            {
                //LoggerHelper.Logger.Error(ex);
                return false;
            }
            return retVal;
        }

        public string ConvertBase64Mail(string emailContent)
        {
            var imgFolder = Path.Combine(_env.ContentRootPath, @"Uploads\Image");
            
            if (!Directory.Exists(imgFolder))
            {
                Directory.CreateDirectory(imgFolder);
            }
            const string pattern = "<img.+?src=[\"'](.+?)[\"'].*?>";
            const string apiUrl = "https://uongbismartcity-api.c2tech.vn/";
            var imgTags = Regex.Matches(emailContent, pattern);
            char[] x = {' ', ','};
            foreach (Match img in imgTags)
            {
                var imgName = Guid.NewGuid() + ".jpg";
                var imgPath = Path.Combine(imgFolder, imgName);
                var splitList = img.Value.Split(x);
                var newImgTag = splitList[2].Replace("\"","");
                var imgBytes = Convert.FromBase64String(newImgTag);
                File.WriteAllBytes(imgPath,imgBytes);
                emailContent = emailContent.Replace(splitList[1]+','+splitList[2], "src=\"" + apiUrl+"Uploads/Image/" + imgName + "\"");
            }
            return emailContent;
        }

    }
}
