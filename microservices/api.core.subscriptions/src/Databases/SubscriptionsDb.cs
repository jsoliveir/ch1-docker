using Api.Core.Subscriptions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Subscriptions.Databases
{
    public class SubscriptionsDb : DbContext
    {
        public SubscriptionsDb(DbContextOptions<SubscriptionsDb> options)
            : base(options) { }

        public DbSet<Subscription> Subscriptions { get; set; }
    }
}
