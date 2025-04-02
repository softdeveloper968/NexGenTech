namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocationSpeciality.Queries.GetClientLocationSpecilalityQuery.cs
{
    public class GetClientLocationSpecilalityResponse
    {
        public int Id { get; set; }
        public int SpecialityId { get; set; }
        public int ClientLocationId { get; set; }
        public int ClientId { get; set; }
    }
}
