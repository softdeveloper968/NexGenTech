using MedHelpAuthorizations.Application.Features.InsuranceCards.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetByCardholderId;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetById;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetByPatientId;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.InsuranceCards
{
    public interface IInsuranceCardsManager : IManager
    {
        Task<IResult<List<GetInsuranceCardsByPatientIdResponse>>> GetInsuranceCardsByPatientIdAsync(int patientId);
        Task<IResult<List<GetInsuranceCardsByCardholderIdResponse>>> GetInsuranceCardsByCardholderIdAsync(int cardholderId);
        Task<PaginatedResult<GetAllPagedInsuranceCardsResponse>> GetAllPagedAsync(GetAllPagedInsuranceCardsQuery request);
        Task<PaginatedResult<GetInsuranceCardsByCriteriaResponse>> GetByCriteriaAsync(GetInsuranceCardsByCriteriaPagedQuery request);
        Task<IResult<GetInsuranceCardByIdResponse>> GetByIdAsync(int id);
        Task<IResult<int>> SaveAsync(AddEditInsuranceCardCommand request);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
