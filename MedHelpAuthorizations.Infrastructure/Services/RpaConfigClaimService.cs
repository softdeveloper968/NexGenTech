using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.EntityFrameworkCore;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Features.RpaConfigClaims;
using MedHelpAuthorizations.Infrastructure.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class RpaConfigClaimService : IRpaConfigClaimService
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly ITenantManagementService _tenantManagementService;
        private readonly ICurrentUserService _currentUserService;
        private IUnitOfWork<int> _unitOfWork;

        private string _tenantIdentifier => _currentUserService.TenantIdentifier;

        public RpaConfigClaimService(ITenantManagementService tenantManagementService,
                                     ITenantRepositoryFactory tenantRepositoryFactory,
                                     IUnitOfWork<int> unitOfWork,
                                     ICurrentUserService currentUserService)
        {
            _tenantManagementService = tenantManagementService;
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }


        /// <summary>
        /// Retrieves a list of RPA Configurations and maps them to RpaConfigClaimsDto.
        /// </summary>
        /// <returns>A list of RPA Configurations with empty Claims list for each item.</returns>
        public async Task<List<RpaConfigClaimsDto>> GetRpaConfigClaimsListAsync()
        {

            try
            {
                // Fetch RPA configurations for the given tenant
                var rpaConfigs = await GetRpaConfigurationsAsync();

                // If no configurations are found, return an empty list
                if (rpaConfigs == null || !rpaConfigs.Any())
                {
                    return new List<RpaConfigClaimsDto>();
                }

                // Initialize a list to hold the mapped RpaConfigClaimsDto
                var rpaConfigClaimsList = new List<RpaConfigClaimsDto>();

                // Map each RPA config to RpaConfigClaimsDto and add to the list
                foreach (var rpaConfig in rpaConfigs)
                {
                    var rpaConfigClaimsDto = new RpaConfigClaimsDto
                    {
                        ClientId = rpaConfig.ClientInsurance?.ClientId ?? 0,
                        RPAConfigId = rpaConfig?.ClientRpaCredentialConfigurationId ?? 0,
                        RPAInsuranceId = rpaConfig?.ClientInsuranceId ?? 0,
                        AuthTypeId = rpaConfig?.AuthTypeId ?? 0,
                        AuthTypeName = rpaConfig?.AuthType?.Name ?? string.Empty,
                        LocationId = rpaConfig?.ClientLocationId ?? 0,
                        LocationName = rpaConfig?.ClientLocation?.Name ?? string.Empty,
                        UserName = rpaConfig?.ClientRpaCredentialConfiguration?.Username ?? string.Empty,
                        Password = rpaConfig?.ClientRpaCredentialConfiguration?.Password ?? string.Empty,
                        AlternateUserName = rpaConfig?.AlternateClientRpaCredentialConfiguration?.Username ?? string.Empty,
                        AlternatePassword = rpaConfig?.AlternateClientRpaCredentialConfiguration?.Password ?? string.Empty,
                        URL = !string.IsNullOrEmpty(rpaConfig?.ClientInsurance?.RpaInsurance?.TargetUrl)
                    ? rpaConfig.ClientInsurance.RpaInsurance.TargetUrl
                    : rpaConfig?.ClientRpaCredentialConfiguration?.RpaInsuranceGroup?.DefaultTargetUrl ?? string.Empty,
                        Claims = new List<GetClaimStatusBatchClaimsByBatchIdResponse>() 
                    };

                    rpaConfigClaimsList.Add(rpaConfigClaimsDto);
                }

                return rpaConfigClaimsList;
            }
            catch (Exception ex)
            {
                // Log and throw the exception with a custom message for better context
                throw new ApplicationException("An error occurred while retrieving and processing RPA configurations.", ex);
            }
        }



        /// <summary>
        /// Provide a List of Claims For Given rpaConfigId
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="rpaConfigId"></param>
        /// <param name="tenantIdentifier"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<RpaConfigClaimsDto> ProcessClientClaimsAsync(CancellationToken cancellationToken, int rpaConfigId, string tenantIdentifier = "")
        {
            if (string.IsNullOrEmpty(tenantIdentifier))
            {
                tenantIdentifier = _tenantIdentifier;
            }

            var rpaConfigInfo = await GetRpaConfigurationByIdAsync(rpaConfigId);
            if (rpaConfigInfo == null)
            {
                return null; // Return null if the RPA config info is not found
            }

            try
            {
                // Process the claims for the single RPA configuration
                RpaConfigClaimsDto result = await ProcessRpaConfigClaimsAsync(tenantIdentifier, rpaConfigInfo, cancellationToken);

                return result; // Return the result if it's valid
            }
            catch (Exception ex)
            {
                // Log the exception or handle as needed
                throw new ApplicationException("An error occurred while processing the RPA config claims.", ex);
            }
        }

        /// <summary>
        /// Retrieves RPA configurations for a specific client.
        /// </summary>
        /// <param name="client">Client entity.</param>
        /// <param name="unitOfWork">Unit of work for database operations.</param>
        /// <returns>List of RPA configurations.</returns>
        public async Task<List<ClientInsuranceRpaConfiguration>> GetRpaConfigurationsAsync()
        {
            try
            {

                return await _unitOfWork.Repository<ClientInsuranceRpaConfiguration>()
                                       .Entities
                                       .Include(c => c.ClientInsurance)
                                       .Include(c => c.ClientLocation)
                                       .Include(c => c.ClientInsurance.RpaInsurance)
                                       .ThenInclude(d => d.RpaInsuranceGroup)
                                       .Include(c => c.ClientRpaCredentialConfiguration)
                                       .Include(c => c.AlternateClientRpaCredentialConfiguration)
                                       .ThenInclude(d => d.RpaInsuranceGroup)
                                       .Where(c => c.TransactionTypeId == TransactionTypeEnum.ClaimStatus
                                           && !c.IsDeleted)
                                       .OrderByDescending(c => c.AuthTypeId.HasValue)
                                       .ThenByDescending(c => c.ClientLocationId.HasValue)
                                       .ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ClientInsuranceRpaConfiguration> GetRpaConfigurationByIdAsync(int rpaConfigId)
        {
            var result = await _unitOfWork.Repository<ClientInsuranceRpaConfiguration>()
                                   .Entities
                                   .Include(c => c.ClientInsurance)
                                   .Include(c => c.ClientLocation)
                                   .Include(c => c.ClientInsurance.Client)
                                   .Include(c => c.ClientInsurance.RpaInsurance)
                                   .ThenInclude(d => d.RpaInsuranceGroup)
                                   .Include(c => c.ClientRpaCredentialConfiguration)
                                   .Include(c => c.AlternateClientRpaCredentialConfiguration)
                                   .ThenInclude(d => d.RpaInsuranceGroup)
                                   .Where(c => c.Id == rpaConfigId
                                       && c.TransactionTypeId == TransactionTypeEnum.ClaimStatus
                                       && !c.IsDeleted).FirstOrDefaultAsync();

            if (result == null)
            {
                Console.WriteLine($"No configuration found for ID: {rpaConfigId}");
            }
            return result;
        }

        /// <summary>
        /// Processes claims for a specific RPA configuration.
        /// </summary>
        /// <param name="tenantIdentifier">Identifier of the tenant.</param>
        /// <param name="client">Client entity.</param>
        /// <param name="rpaConfig">RPA configuration entity.</param>
        /// <param name="unitOfWork">Unit of work for database operations.</param>
        /// <param name="returnQuantityCap">Maximum number of claims to process.</param>
        /// <param name="cancellationToken">Cancellation token to handle operation cancellation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<RpaConfigClaimsDto> ProcessRpaConfigClaimsAsync(string tenantIdentifier, ClientInsuranceRpaConfiguration rpaConfig, CancellationToken cancellationToken)
        {
            var claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenantIdentifier);
            var claimStatusBatchClaims = claimStatusBatchClaimsRepository.ClaimStatusBatchClaims;

            // Filter and retrieve claims based on specifications
            var result = await GetFilteredClaimStatusBatchClaimsAsync(claimStatusBatchClaims, rpaConfig);

            // Create an object to store processed claims and RPA configuration details
            var rpaConfigClaims = new RpaConfigClaimsDto
            {
                RPAConfigId = rpaConfig.Id,
                UserName = rpaConfig?.ClientRpaCredentialConfiguration?.Username ?? string.Empty,
                Password = rpaConfig?.ClientRpaCredentialConfiguration?.Password ?? string.Empty,
                URL = !string.IsNullOrEmpty(rpaConfig.ClientInsurance.RpaInsurance.TargetUrl)
                    ? rpaConfig.ClientInsurance.RpaInsurance.TargetUrl
                    : rpaConfig.ClientRpaCredentialConfiguration.RpaInsuranceGroup.DefaultTargetUrl,
                Claims = result,
            };
            return rpaConfigClaims;

            // Process the rpaConfigurationClaimStatusBatchClaims further as needed
        }

        /// <summary>
        /// Retrieves and filters claim status batch claims based on the specifications.
        /// </summary>
        /// <param name="claimStatusBatchClaims">Queryable of claim status batch claims.</param>
        /// <param name="rpaConfig">RPA configuration entity.</param>
        /// <param name="returnQuantityCap">Maximum number of claims to retrieve.</param>
        /// <returns>List of filtered claim status batch claims responses.</returns>
        public async Task<List<GetClaimStatusBatchClaimsByBatchIdResponse>> GetFilteredClaimStatusBatchClaimsAsync(IQueryable<ClaimStatusBatchClaim> claimStatusBatchClaims, ClientInsuranceRpaConfiguration rpaConfig)
        {
            try
            {
                return await claimStatusBatchClaims
                .AsNoTracking()
                .Include(c => c.ClientInsurance)
                .Include(c => c.ClaimStatusTransaction)
                    .ThenInclude(t => t.ClaimStatusTransactionLineItemStatusChangẹ)
                .Include(c => c.ClaimStatusTransaction)
                    .ThenInclude(cs => cs.ClaimLineItemStatus)
                .Include(c => c.ClientLocation)
                .Include(c => c.Patient)
                    .ThenInclude(p => p.Person)
                .Specify(new ApprovedClaimLineItemStatusWaitPeriodSpecification())
                .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                .Specify(new ClaimStatusClaimBilledOnQualificationFilterSpecification())
                .Specify(new ClaimStatusMaxDaysPipelineQualificationFilterSpecification())
                .Specify(new ClaimStatusDaysBetweenAttemptsQualificationFilterSpecification())
                .Specify(new ClaimStatusOmitDeniedWrongPayerFilterSpecification())
                .Specify(new ClaimStatusOmitDeniedPolicyNumberFilterSpecification())
                .Specify(new ClaimStatusBatchClaimRPAConfigurationSpecification(rpaConfig.ClientInsurance.RpaInsuranceId ?? 0, rpaConfig.AuthTypeId ?? 0, rpaConfig.ClientLocationId ?? 0))
                .OrderBy(c => c.ClaimStatusTransactionId.HasValue)
                .ThenBy(c => c.ClaimStatusTransaction != null && c.ClaimStatusTransaction.ClaimLineItemStatus != null
                    ? c.ClaimStatusTransaction.ClaimLineItemStatus.Rank : 0)
                .Select(claimStatusBatchClaim => GetResult(claimStatusBatchClaim))
                .ToListAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public static GetClaimStatusBatchClaimsByBatchIdResponse GetResult(ClaimStatusBatchClaim claimStatusBatchClaim)
        {
            var result = new GetClaimStatusBatchClaimsByBatchIdResponse
            {
                Id = claimStatusBatchClaim.Id,
                CurrentLineItemStatusId = claimStatusBatchClaim?.ClaimStatusTransaction?.ClaimLineItemStatusId,
                CurrentExceptionReason = claimStatusBatchClaim?.ClaimStatusTransaction?.ExceptionReason,
                LastStatusChangedOn = claimStatusBatchClaim?.ClaimStatusTransaction?.ClaimStatusTransactionLineItemStatusChangẹ?.LastModifiedOn,
                ClaimStatusBatchId = claimStatusBatchClaim.ClaimStatusBatchId,
                ClaimStatusTransactionId = claimStatusBatchClaim?.ClaimStatusTransactionId,
                ClaimNumber = claimStatusBatchClaim?.ClaimNumber,
                PayerClaimNumber = claimStatusBatchClaim?.ClaimStatusTransaction?.ClaimNumber,
                PatientLastName = claimStatusBatchClaim?.Patient?.Person?.LastName ?? string.Empty,
                PatientFirstName = claimStatusBatchClaim?.Patient?.Person?.FirstName ?? string.Empty,
                DateOfBirth = claimStatusBatchClaim?.DateOfBirth,
                RenderingNpi = claimStatusBatchClaim?.ClientProvider?.Npi ?? string.Empty,
                GroupNpi = claimStatusBatchClaim?.GroupNpi,
                PolicyNumber = claimStatusBatchClaim?.PolicyNumber,
                PolicyNumberUpdatedOn = claimStatusBatchClaim?.PolicyNumberUpdatedOn,
                EligibilityPolicyNumber = claimStatusBatchClaim?.ClaimStatusTransaction?.EligibilityPolicyNumber ?? string.Empty,
                DateOfServiceFrom = claimStatusBatchClaim?.DateOfServiceFrom,
                DateOfServiceTo = claimStatusBatchClaim?.DateOfServiceTo,
                ProcedureCode = claimStatusBatchClaim?.ProcedureCode,
                Modifiers = claimStatusBatchClaim?.Modifiers,
                ClaimBilledOn = claimStatusBatchClaim?.ClaimBilledOn,
                BilledAmount = claimStatusBatchClaim?.BilledAmount,
                Quantity = claimStatusBatchClaim.Quantity,
                IsDeleted = claimStatusBatchClaim.IsDeleted,
                ClientLocationNpi = claimStatusBatchClaim?.ClientLocation?.Npi ?? string.Empty,
                ClientLocationInsuranceIdentifierString = claimStatusBatchClaim?.ClientLocation?.GetLocationIdentifierStringForInsuranceId(claimStatusBatchClaim.ClientLocation, claimStatusBatchClaim.ClientInsuranceId) ?? string.Empty,
                PayerIdentifier = claimStatusBatchClaim?.ClientInsurance?.PayerIdentifier ?? string.Empty,
                TaxId = claimStatusBatchClaim?.Client?.TaxId.ToString().PadLeft(9, '0') ?? string.Empty,
            };
            return result;
        }

    }
}
