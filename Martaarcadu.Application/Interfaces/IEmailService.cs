using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.Interfaces
{
    public interface IEmailService
    {
        //Send OTP via Email for Password Reset or Email Verification
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
