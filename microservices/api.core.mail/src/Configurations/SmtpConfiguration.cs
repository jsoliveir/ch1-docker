
using Microsoft.Extensions.Logging;

namespace Api.Core.Mail.Configurations
{
    public class SmtpConfiguration
    { 
         public string Server { get; set; }

         public int Port { get; set; }
    }

}
