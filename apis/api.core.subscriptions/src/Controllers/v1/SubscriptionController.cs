using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Core.Subscriptions.Controllers;
using Api.Core.Subscriptions.Models;
using Api.Core.Subscriptions.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Core.Subscriptions.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubscriptionController : Controller, ISubscriptions
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly ICoreEmailService _mailService;
        private readonly ISubscriptionsDbService _subscriptionsService;

        public SubscriptionController(
            ISubscriptionsDbService subscriptionsService,
            ILogger<SubscriptionController> logger,
            ICoreEmailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
            _subscriptionsService = subscriptionsService;
        }

        [HttpPost]
        public ActionResult<Subscription> Create(Subscription subscription)
        {
            try
            {
                subscription = _subscriptionsService.CreateSubscription(subscription);
                _mailService.Send(new Email()
                {
                    Subject = "Woohoo! You've created a subscription",
                    Body = $"Tank you {subscription.FirstName} for subscribing the newsletter [{subscription.NewsletterId}]",
                    From = "no-reply@iban.com",
                    To = subscription.Email,
                });

                return Ok(subscription);
            }
            catch (Exception)
            {
                _logger.LogError("Error while attempting to create a subscription");
            }

            return Forbid("Error while attempting to create a subscription");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            try
            {
                var subscription = _subscriptionsService.GetSubscriptionById(id);

                if (subscription?.Id == null)
                    return NotFound();

                _mailService.Send(new Email()
                {
                    Subject = "Hope to see you again :'(",
                    Body = $"We're sad to see you go :'(",
                    From = "no-reply@iban.com",
                    To = subscription.Email,
                });

                _subscriptionsService.DeleteSubscription(subscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }

            return Ok("OK");
        }

        [HttpPut("{id}")]
        public ActionResult<Subscription> Put([FromRoute] int id, [FromBody] Subscription subscription)
        {
            var origin = _subscriptionsService.GetSubscriptionById(id);

            if (origin?.Id == null)
                return NotFound();

            subscription.Id = origin.Id;
            origin = _subscriptionsService.UpdateSubscription(subscription);
           
            return Ok(origin);
        }

        [HttpGet("{id}")]
        public ActionResult<Subscription> Get([FromRoute] int id)
        {

            var subscription = _subscriptionsService.GetSubscriptionById(id);

            if (subscription?.Id == null)
                return NotFound();

            return Ok(subscription);
        }

        [HttpGet("List")]
        public ActionResult<IEnumerable<Subscription>> List([FromQuery] int max = 10)
        {
            var subscriptions = _subscriptionsService.GetSubscriptions(max);
            return Ok(subscriptions);
        }
    }
}
