using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.UserAlerts.Queries.GetByUserId
{
    public class GetUserAlertByUserIdResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AlertTypeEnum AlertType { get; set; }
        public string PreviewText { get; set; }
        public string ResourceType { get; set; }
        public string ResourceId { get; set; }
        public bool IsViewed { get; set; }
        public bool IsRemoved { get; set; }
        public string FileStatus { get; set; }
        public string ErrorMessage {  get; set; } = string.Empty;
        public bool IsDownload {  get; set; } 
        public int ClientId { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string TimeSpan { get; set; }
        public string Fileurl { get; set; }
    }
}
