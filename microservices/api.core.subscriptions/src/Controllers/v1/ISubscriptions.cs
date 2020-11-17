using Api.Core.Subscriptions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Core.Subscriptions.Controllers.v1
{
    public interface ISubscriptions
    {
        ActionResult<Subscription> Create(Subscription subscription);
        ActionResult<Subscription> Get(int id);
        ActionResult<IEnumerable<Subscription>> List(int max = 10);
        ActionResult Delete(int id);
        ActionResult<Subscription> Put(int id, Subscription subscription);
    }
}