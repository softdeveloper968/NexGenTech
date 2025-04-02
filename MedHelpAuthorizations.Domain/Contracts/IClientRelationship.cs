namespace MedHelpAuthorizations.Domain.Contracts
{
    public interface IClientRelationship
    {
        public int ClientId { get; set; }
        public Entities.Client Client { get; set; }
    }
}
