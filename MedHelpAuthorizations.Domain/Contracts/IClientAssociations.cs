namespace MedHelpAuthorizations.Domain.Contracts
{
    public interface IClientAssociations
    {
        public int ClientLocationId { get; set; }
        public int ClientProviderId { get; set; }
        public int ClientInsuranceId { get; set; }
        public int CptCodeId { get; set; }
    }
}