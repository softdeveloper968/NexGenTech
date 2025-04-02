using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface ICptCodeRepository : IRepositoryAsync<CptCode, int>
    {
        Task<CptCode> GetByCptCode(string code); //EN-186
    }
}
