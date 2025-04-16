using MedHelpAuthorizations.Application.Models.Chat;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Responses.Identity
{
    public class ChatUserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureDataUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        //public virtual ICollection<ChatHistory> ChatHistoryFromUsers { get; set; }
        //public virtual ICollection<ChatHistory> ChatHistoryToUsers { get; set; }
    }
}