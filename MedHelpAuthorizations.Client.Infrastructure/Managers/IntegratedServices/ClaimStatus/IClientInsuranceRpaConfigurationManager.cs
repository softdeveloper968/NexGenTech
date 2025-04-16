using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Create;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Update;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByRpaInsurance;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByUserrnameAndUrl;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetFailed;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ClaimStatus
{
    public interface IClientInsuranceRpaConfigurationManager : IManager
    {
        Task<IResult<List<GetAllClientInsuranceRpaConfigurationsResponse>>> GetAllAsync();

        Task<IResult<GetClientInsuranceRpaConfigurationByIdResponse>> GetByIdAsync(int id);

        Task<IResult<int>> CreateAsync(CreateClientInsuranceRpaConfigurationCommand command);

        Task<IResult<int>> UpdateAsync(UpdateClientInsuranceRpaConfigurationCommand command);

        Task<IResult<int>> UpdateFailureReportedAsync(UpdateClientInsuranceRpaConfigurationFailureCommand command);

        Task<IResult<int>> UpdateExpiryWarningAsync(UpdateRpaConfigurationExpiryWarningCommand command);

        Task<IResult<int>> UpdateCurrentClaimCount(UpdateRpaConfigurationDailyClaimCountCommand request);

        Task<IResult<int>> DeleteAsync(int id);
        
        Task<IResult<GetClientInsuranceRpaConfigurationByCriteriaResponse>> GetSingleByCriteria(GetSingleClientInsuranceRpaConfigurationByCriteriaQuery query);

        Task<IResult<List<GetClientInsuranceRpaConfigurationByCriteriaResponse>>> GetAllByRpaInsuranceIdAsync(GetClientInsuranceRpaConfigurationByRpaInsuranceQuery query);
        Task<IResult<List<GetClientInsuranceRpaConfigurationsByUsernameAndUrlResponse>>> GetClientInsuranceRpaConfigurationsByUsernameAndUrlAsync(GetClientInsuranceRpaConfigurationsByUsernameAndUrlQuery query);

		/// <summary>
		/// Retrieves a paginated list of client insurance RPA configurations based on the specified request.
		/// </summary>
		/// <param name="request">The request containing pagination parameters.</param>
		/// <returns>A paginated result containing the requested client insurance RPA configurations.</returns>
		Task<PaginatedResult<GetAllClientInsuranceRpaConfigurationsResponse>> GetClientInsuranceRpaConfigurationsPagedAsync(GetAllPagedClientRpaInsurangeConfigRequest request); //AA-23    

		/// <summary>
		/// GetErrorOrFailedClientInsuranceRpaConfig for logged in user
		/// </summary>
		/// <returns></returns>
		Task<IResult<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>> GetErrorOrFailedClientInsuranceRpaConfigAsync(); //AA-250
	}
}
