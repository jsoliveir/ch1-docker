using Api.Core.Subscriptions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Subscriptions.Services
{
    public interface ISubscriptionsDbService
    {
        void DeleteSubscription(Subscription subscription);

        Subscription GetSubscriptionById(int id);

        IEnumerable<Subscription> GetSubscriptions(int max = 1);
        Subscription CreateSubscription(Subscription subscription);

        Subscription UpdateSubscription(Subscription subscription);
    }
}
