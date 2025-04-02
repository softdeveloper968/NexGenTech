using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllByClientId;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Queries.GetAll
{
    public class GetAllClientRpaCredentialConfigurationsQuery : IRequest<Result<List<GetAllClientRpaCredentialConfigurationsResponse>>>
    {
        public class GetAllClientRpaCredentialConfigurationsQueryHandler : IRequestHandler<GetAllClientRpaCredentialConfigurationsQuery, Result<List<GetAllClientRpaCredentialConfigurationsResponse>>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly ICurrentUserService _currentUserService;
            private int _clientId => _currentUserService.ClientId;

            public GetAllClientRpaCredentialConfigurationsQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
            {
                _unitOfWork = unitOfWork;
                _currentUserService = currentUserService;
            }

            public async Task<Result<List<GetAllClientRpaCredentialConfigurationsResponse>>> Handle(GetAllClientRpaCredentialConfigurationsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    Expression<Func<ClientRpaCredentialConfiguration, GetAllClientRpaCredentialConfigurationsResponse>> expression = e => new GetAllClientRpaCredentialConfigurationsResponse
                    {
                        Id = e.Id,
                        RpaInsuranceGroupId = e.RpaInsuranceGroupId,
                        Username = e.Username,
                        Password = e.Password,
                        FailureMessage = e.FailureMessage,
                        ReportFailureToEmail = e.ReportFailureToEmail,
                        ExpiryWarningReported = e.ExpiryWarningReported,
                        IsCredentialInUse = e.IsCredentialInUse,
                        UseOffHoursOnly = e.UseOffHoursOnly,
                        OtpForwardFromEmail = e.OtpForwardFromEmail,
                        RpaGroupName = e.RpaInsuranceGroup != null ? e.RpaInsuranceGroup.Name : string.Empty,
                        DefaultTargetUrl = e.RpaInsuranceGroup != null ? e.RpaInsuranceGroup.DefaultTargetUrl : string.Empty,
                        FailureReported = e.FailureReported
                    };

                    var data = await _unitOfWork.Repository<ClientRpaCredentialConfiguration>().Entities
                        .Include(c => c.RpaInsuranceGroup)
                        .Select(expression)
                        .ToListAsync(cancellationToken: cancellationToken);

                    return await Result<List<GetAllClientRpaCredentialConfigurationsResponse>>.SuccessAsync(data);
                }
                catch (Exception ex)
                {
                    return await Result<List<GetAllClientRpaCredentialConfigurationsResponse>>.FailAsync(
                        $"Getting RPA Credential Configuration failed" + ex.InnerException != null
                            ? ex.InnerException.Message
                            : ex.Message);
                }
            }
        }
    }
}
