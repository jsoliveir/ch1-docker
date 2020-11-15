using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Client.Subscriptions.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : Controller
    {
        public SubscriptionsController(ILogger<SubscriptionsController> logger)
        {
        }
    }
}
