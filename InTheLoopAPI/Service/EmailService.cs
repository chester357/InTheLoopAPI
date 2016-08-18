using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace InTheLoopAPI.Service
{
    public class EmailService
    {
        public void SendEmail(String sendTo, String token)
        {
            var fromAddress = new MailAddress("loopstirhelp@gmail.com", "Loopstir Help");
            var toAddress = new MailAddress(sendTo, "Loopstir User");
            const string fromPassword = "fybknnchsmdvxfbd";
            const string subject = "Reset Password";
            const string body = "Html Will Go HERE";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
            };

            var mes = new MailMessage(fromAddress, toAddress);
            mes.IsBodyHtml = true;
            mes.Subject = subject;
            mes.Body = "\nYour password reset token is " + token + " \n\nThanks, Loopstir\n";
            mes.Body = 
            @"
                <html>
                    <body>
                        <h4>Hello Loopstir,</h4>
                        <h4>Your password reset token is: " + token + @"</h4>
                        <h4>If you were not expecting this email please contact Loopstir at, www.loopstir.com</h4>
                        <h4>Thanks!</h4>
                    </body>
                </html>
            ";

            smtp.Send(mes);
        }

        public SupportEmail SendSupportEmail(SupportEmail email)
        {
            DatabaseContext context = new DatabaseContext();

            var result = context.SupportEmails.Add(email);

            context.SaveChanges();

            return result;
        }
    }
}