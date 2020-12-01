using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.Core.Subscriptions.Models
{
    public class Subscription : SubscriptionViewModel
    {
        public int Id { get; set; }
    }
}
