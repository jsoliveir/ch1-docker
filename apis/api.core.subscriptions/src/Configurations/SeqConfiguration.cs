
using Microsoft.Extensions.Logging;

namespace Api.Core.Subscriptions.Configurations
{
    public class SeqConfiguration
    { 
         public string ServerUrl { get; set; }

         public LogLevel MinimumLevel { get; set; }
    }

}
