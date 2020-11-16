using Api.Core.Mail.Models;
using System.Net.Mail;

namespace Api.Core.Mail.Observers
{
    public interface IEmailReceiverObserver
    {
        void Attach(IEmailReceiverObservable observable);

        void Dettach(IEmailReceiverObservable observable);

        void Notify(Email email);
    }
}
