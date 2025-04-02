namespace MedHelpAuthorizations.Domain.Contracts
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
