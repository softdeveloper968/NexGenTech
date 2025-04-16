namespace MedHelpAuthorizations.Application.Interfaces.Common
{
    public interface IHubService
    {
        void SendAlert(string message, string receiverUserId, string senderUserId);
        Task SendToUser(string notificationType, string userId, string fileName, string fileUrl);

        Task SendErrorToUser(string notificationType, string userId, string fileName, string error, string errorMessage);

        Task SendAttemptedCountToClient(string notificationType, int documentId, int attemptedCount);
        Task SendActualCountToClient(string notificationType, int documentId, int actualCount);
        Task SendFileUploadPercentageToClient(string notificationType, int documentId, int actualCount);
        Task RefreshTable(string notificationType, int documentId);
    }
}