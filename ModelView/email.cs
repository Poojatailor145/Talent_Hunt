using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace TalentHunt.ModelView
{
    public class email
    {
        public email(string receiver, string subject, string message)
        {
            var senderEmail = new MailAddress("worlds.talenthunt@gmail.com", "Talent Hunt");
            var receiverEmail = new MailAddress(receiver, "receiver");
            var password = "talent.hunt";

            //string FilePath = "E:\\TalentHunt\\EmailTemplates\\index.html";
            //StreamReader str = new StreamReader(FilePath);
            //String MailText = str.ReadToEnd();
            //str.Close();

            var sub = subject;
            var body = message;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };

            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body,
                IsBodyHtml = true
            })
            {
                //mess.IsBodyHtml = true;
                smtp.Send(mess);
            }
        }
    }
}