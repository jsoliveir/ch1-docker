using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Client.Subscriptions.Controllers.v1;
using Api.Core.Subscriptions.Models;
using Api.Core.Subscriptions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Core.Subscriptions.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubscriptionController : Controller, ISubscriptions
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly ICoreSubscriptionsService _coreSubscriptionsService;

        public SubscriptionController(
            ICoreSubscriptionsService coreSubscriptionsService,
            ILogger<SubscriptionController> logger)
        {
            _logger = logger;
            _coreSubscriptionsService = coreSubscriptionsService;
        }

        [HttpPost]
        public async Task<ActionResult<Subscription>> Create(SubscriptionViewModel subscription)
        {
            try
            {
                subscription =  await _coreSubscriptionsService.Create(subscription);
                return Ok(subscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _coreSubscriptionsService.Delete(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> Get([FromRoute] int id)
        {
            var subscription = await _coreSubscriptionsService.Get(id);
            return Ok(subscription);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Subscription>> Patch([FromRoute] int id,[FromQuery] bool? consent)
        {
            var subscription = await _coreSubscriptionsService.Get(id);

            subscription.Consent = 
                consent ?? subscription.Consent;

            await _coreSubscriptionsService.Update(id,subscription);
            return Ok(subscription);
        }

        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<Subscription>>> List([FromQuery] int max=10)
        {
            var subscriptions = await _coreSubscriptionsService.List(max);
            return Ok(subscriptions);
        }
    }
}
