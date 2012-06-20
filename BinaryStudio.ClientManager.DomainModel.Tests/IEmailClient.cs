using System.Collections.Generic;
using System.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel.Tests
{
    public interface IEmailClient
    {
        IEnumerable<MailMessage> GetMessages();
    }
}