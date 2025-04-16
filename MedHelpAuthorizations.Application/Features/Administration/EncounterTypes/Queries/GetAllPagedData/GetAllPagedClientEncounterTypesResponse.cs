namespace MedHelpAuthorizations.Application.Features.Administration.EncounterTypes.Queries.GetAllPagedData
{
    public class GetAllPagedClientEncounterTypesResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int clientId { get; set; }
    }
}
