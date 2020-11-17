using Api.Core.Subscriptions.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Subscriptions.Services
{
    public interface ICoreSubscriptionsService
    {
        Task<Subscription> Create(SubscriptionViewModel subscription);
        Task Delete(int id);
        Task<Subscription> Get(int id);
        Task<IEnumerable<Subscription>> List(int max = 10);
        Task<Subscription> Update(int id,Subscription subscription);
    }
}
