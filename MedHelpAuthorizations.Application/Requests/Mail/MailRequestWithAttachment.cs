namespace MedHelpAuthorizations.Application.Requests.Mail
{
    public class MailRequestWithAttachment : MailRequest
    {
        public string Base64Content { get; set; }
        public string FileName { get; set; }
    }
}