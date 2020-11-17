using Api.Core.Mail.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Core.Mail.Controllers.v1
{
    public interface IMail
    {
        Task<IActionResult> Send(Email email);
    }
}