using Api.Core.Subscriptions.Databases;
using Api.Core.Subscriptions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Subscriptions.Services
{
    public class SubscriptionDbService : ISubscriptionsDbService
    {
        private readonly DbContextOptions<SubscriptionsDb> _context;

        public SubscriptionDbService(DbContextOptions<SubscriptionsDb> context)
        {
            _context = context;
        }

        public IEnumerable<Subscription> GetSubscriptions(int max = 1)
        {
            using (var database = new SubscriptionsDb(_context))
            {
                return database.Subscriptions.Take(max).ToList();
            }
        }

        public Subscription GetSubscriptionById(int id)
        {
            using (var database = new SubscriptionsDb(_context))
            {
                return database.Subscriptions.FirstOrDefault(s => s.Id == id);
            }
        }
        public Subscription CreateSubscription(Subscription subscription)
        {
            using (var database = new SubscriptionsDb(_context))
            {
                var created = database.Subscriptions.Add(subscription);
                database.SaveChanges();
                return created.Entity;
            }
        }
        public void DeleteSubscription(Subscription subscription)
        {
            using (var database = new SubscriptionsDb(_context))
            {
                database.Subscriptions.Remove(subscription);
                database.SaveChanges();
            }
        }

        public Subscription UpdateSubscription(Subscription subscription)
        {
            using (var database = new SubscriptionsDb(_context))
            {
                if (GetSubscriptionById(subscription.Id) == null)
                    throw new KeyNotFoundException(nameof(subscription.Id));

                var updated = database.Subscriptions.Update(subscription);
                database.SaveChanges();
                return updated.Entity;
            }
        }
    }
}
