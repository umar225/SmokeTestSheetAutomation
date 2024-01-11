using Coursewise.AWS.Communication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.AWS.Communication
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
        Task<bool> SendRawEmail(Email email);
    }
}
