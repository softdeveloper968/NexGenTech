namespace MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Queries.GetAllPagedData
{
    public class GetAllPagedClientNotesResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public int ClientId { get; set; }
        public string UserId { get; set; }
    }
}
