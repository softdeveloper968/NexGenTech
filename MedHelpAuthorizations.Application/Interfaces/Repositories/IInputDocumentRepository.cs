using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IInputDocumentRepository : IRepositoryAsync<InputDocument, int>
    {
        IQueryable<InputDocument> InputDocuments { get; }

        Task <InputDocument> GetByFileNameAndClientIdAsync(string fileName);
    }
}
