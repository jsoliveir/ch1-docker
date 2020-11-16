using Api.Core.Mail.Models;
using System.Net.Mail;

namespace Api.Core.Mail.Observers
{
    public interface IEmailReceiverObservable
    {
        void OnEmailReceived(Email email);
    }
}
