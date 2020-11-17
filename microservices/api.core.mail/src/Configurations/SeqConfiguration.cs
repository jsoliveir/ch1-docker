
using Microsoft.Extensions.Logging;

namespace Api.Core.Mail.Configurations
{
    public class SeqConfiguration
    { 
         public string ServerUrl { get; set; }

         public LogLevel MinimumLevel { get; set; }
    }

}
