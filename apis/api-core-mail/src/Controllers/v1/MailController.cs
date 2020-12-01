using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Core.Mail.Models;
using Api.Core.Mail.Observers;
using Api.Core.Mail.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Core.Mail.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class MailController : Controller, IMail
    {
        private readonly IEmailService _emailService;
        public MailController(IEmailService emailService)
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
