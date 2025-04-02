using MedHelpAuthorizations.Application.Features.ResponsibleParties.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.ResponsibleParties.GetByCriteria;
using MedHelpAuthorizations.Application.Features.ResponsibleParties.GetById;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.ResponsibleParty
{
    public interface IResponsiblePartyManager: IManager
    {
        Task<PaginatedResult<GetByCritieriaResponsiblePartyResponse>> GetByAccountNumerAsync(string accNumber);
        Task<PaginatedResult<GetByCritieriaResponsiblePartyResponse>> GetByCriteriaAsync(GetByCritieriaResponsiblePartyQuery request);
        Task<PaginatedResult<GetByCritieriaResponsiblePartyResponse>> GetByExternalIdAsync(string externalId);
        Task<IResult<GetResponsiblePartyByIdResponse>> GetByIdAsync(int id);
        Task<IResult<int>> SaveAsync(AddEditResponsiblePartyCommand request);
    }
}