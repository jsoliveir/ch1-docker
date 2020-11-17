using Api.Core.Mail.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Api.Core.Mail.Services
{
    public interface IEmailService
    {
        Task SendMail(Email email);
    }
}