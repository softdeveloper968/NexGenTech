using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Update;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetRpaConfigurationsWithLocation;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Commands;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Queries.GetAll;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientRpaCredentialConfigurationManager : IManager
    {
        
        /// <summary>
        /// Saves a new client RPA credential configuration.
        /// </summary>
        /// <param name="request">The command containing the data to create the new configuration.</param>
        /// <returns>An operation result indicating success and the ID of the created configuration.</returns>
        Task<IResult<int>> SaveAsync(CreateClientRpaCredentialConfigCommand request); //AA-23

        /// <summary>
        /// Update a new client RPA credential configuration.
        /// </summary>
        /// <param name="request">The command containing the data to Update the new configuration.</param>
        /// <returns>An operation result indicating success and the ID of the Update configuration.</returns>
        Task<IResult<int>> UpdateAsync(UpdateClientRpaCredentialConfigCommand request); //AA-268

        /// <summary>
        /// Retrieves a list of all client RPA credential configurations.
        /// </summary>
        /// <returns>A list of all client RPA credential configurations.</returns>
        Task<IResult<List<GetAllClientRpaCredentialConfigurationsResponse>>> GetAllRpaCredentialConfigurations(); //AA-23

		/// <summary>
		/// Reset alerts
		/// </summary>
		/// <returns></returns>
		Task<IResult<int>> ResetAlert(ResetClientRpaCredentialConfigCommand request); //AA-280

        Task<IResult<int>> UpdateIsCredentialInUseAsync(UpdateIsCredentialInUseCommand request);

        Task<IResult<List<GetRpaConfigurationsWithLocationResponse>>> GetRpaConfigurationsWithLocationAsync(); //EN-409

	}
}
