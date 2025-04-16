namespace MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Queries.GetAllPaged
{
    public class GetAllClientUserNotificationResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
        public string FileUrl { get; set; }
        public bool IsDownload { get; set; } 
        public string FileStatus { get; set; }
        public  DateTime CreatedOn { get; set; }
        public string ErrorMessage { get; set; }
    }
}
