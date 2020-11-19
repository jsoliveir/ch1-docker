using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Api.Core.Subscriptions.Models;
using Api.Core.Subscriptions.Services;
using Microsoft.AspNetCore.Authorization;
using Api.Client.Subscriptions.Controllers.v1;

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

        /// <summary>
        ///Create a new subscription.
        /// When a subscription is created a mail is sent to the give destination email address
        ///</summary>
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

        /// <summary>
        ///Unsubscribe a email address using a subscription Id.
        ///</summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _coreSubscriptionsService.Delete(id);
            return Ok();
        }

        /// <summary>
        ///Retrieve a subscription already created
        ///</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> Get([FromRoute] int id)
        {
            var subscription = await _coreSubscriptionsService.Get(id);
            return Ok(subscription);
        }

        /// <summary>
        ///Change a subscription consent
        ///</summary>
        [HttpPatch("{id}")]
        public async Task<ActionResult<Subscription>> Patch([FromRoute] int id,[FromQuery] bool? consent)
        {
            var subscription = await _coreSubscriptionsService.Get(id);

            subscription.Consent = 
                consent ?? subscription.Consent;

            await _coreSubscriptionsService.Update(id,subscription);
            return Ok(subscription);
        }

         /// <summary>
        ///Get a list of subscriptions
        /// (demo purposes) A method like this should not be exposed
        ///</summary>
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<Subscription>>> List([FromQuery] int records=10)
        {
            var subscriptions = await _coreSubscriptionsService.List(records);
            return Ok(subscriptions);
        }
    }
}
