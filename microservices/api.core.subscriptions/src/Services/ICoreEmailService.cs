using Api.Core.Subscriptions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Subscriptions.Services
{
    public interface ICoreEmailService
    {
        Task Send(Email mail);
    }
}
