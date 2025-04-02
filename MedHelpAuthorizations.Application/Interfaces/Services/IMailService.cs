using MedHelpAuthorizations.Application.Requests.Mail;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);

        Task SendAsync(MailRequestWithAttachment request, bool addAitMonitorEmails = false);
        Task SendEmail(string subject, string to, string body, bool hasAttachment, string failedRpaConfigurations, string failedBatches);

        Task SendFailedRpaConfigEmail(string subject, string to, string body, bool hasAttachment,
            string failedRpaConfigurations, string failedBatches);
    }
}