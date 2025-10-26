using Demo.DataAccess.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Services.EmailSender
{
    public interface IEmailSender
    {
        void SendEmail(Email email);
    }
}
