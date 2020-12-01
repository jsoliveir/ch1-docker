using Api.Core.Subscriptions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Client.Subscriptions.Controllers.v1
{
    public interface ISubscriptions
    {
        Task<ActionResult> Delete(int id);
        Task<ActionResult<Subscription>> Create(SubscriptionViewModel subscription);
        Task<ActionResult<Subscription>> Get([FromRoute] int id);
        Task<ActionResult<IEnumerable<Subscription>>> List([FromQuery] int max = 10);
    }
}