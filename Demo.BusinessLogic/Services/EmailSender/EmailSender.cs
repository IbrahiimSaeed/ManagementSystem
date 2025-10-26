using Demo.DataAccess.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        public void SendEmail(Email email)
        {
            //Email ==> protocol ==> SMTP
            //Send Email ==> Noon@gmail.com , Noon@yahoo.com ,Noon@outlook.com
            var client = new SmtpClient("smtp.gmail.com", 587); //SSL , TLS ==> port [Enable]
            client.EnableSsl = true;
            //Sender , Receiver
            //username[Email Sender] , password[Sender]
            client.Credentials = new NetworkCredential("hemasaeedklaus22@gmail.com", "ligkifksrwccqidg");
            client.Send("hemasaeedklaus22@gmail.com", email.To, email.Subject, email.Body);

        }
    }
}
