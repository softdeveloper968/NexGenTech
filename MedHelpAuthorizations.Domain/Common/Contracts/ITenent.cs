namespace MedHelpAuthorizations.Domain.Common.Contracts
{
    public interface IDataPipeEntity
    {
        public int ClientId { get; set; }
        public string TenantIdentifier { get; set; }
    }
}
