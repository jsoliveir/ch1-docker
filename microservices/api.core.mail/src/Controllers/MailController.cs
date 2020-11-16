using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Core.Mail.Models;
using Api.Core.Mail.Observers;
using Api.Core.Mail.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Core.Mail.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class MailController : Controller
    {
        private readonly IMailService _emailService;
        public MailController(IMailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody]Email email)
        {
            await _emailService.SendMail(email);
            return Ok("mail sent");
        }
    }
}
