using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.Core.Subscriptions.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender 
    {
       NA,
       Male,
       Female
    }
}
