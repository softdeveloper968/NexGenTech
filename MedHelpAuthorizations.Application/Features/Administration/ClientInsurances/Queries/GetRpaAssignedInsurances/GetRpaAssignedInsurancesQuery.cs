using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetRpaAssignedInsurances
{
    public class GetRpaAssignedInsurancesQuery : IRequest<Result<List<GetRpaAssignedInsurancesResponse>>>
    {
        //public int PageNumber { get; set; }
        //public int PageSize { get; set; }
        public int ClientId { get; set; }

        public GetRpaAssignedInsurancesQuery()
        {
            //PageNumber = pageNumber;
            //PageSize = pageSize;
        }
    }

    public class GetRpaAssignedInsurancesQueryHandler : IRequestHandler<GetRpaAssignedInsurancesQuery, Result<List<GetRpaAssignedInsurancesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetRpaAssignedInsurancesQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

    public async Task<Result<List<GetRpaAssignedInsurancesResponse>>> Handle(GetRpaAssignedInsurancesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<ClientInsurance, GetRpaAssignedInsurancesResponse>> expression = e => new GetRpaAssignedInsurancesResponse
                {
                    Id = e.Id,
                    LookupName = e.LookupName,
                    Name = e.Name,
                    ClientId = e.ClientId,
                    PayerIdentifier = e.PayerIdentifier,
                    RpaInsuranceId = (int)e.RpaInsuranceId,
                    RpaInsuranceCode = e.RpaInsurance != null ? e.RpaInsurance.Code : string.Empty,
                    ExternalId = e.ExternalId,
                    PhoneNumber = e.PhoneNumber,
                    FaxNumber = e.FaxNumber
                };

                request.ClientId = _clientId;

                var data = await _unitOfWork.Repository<ClientInsurance>().Entities
                    .Specify(new GenericByClientIdSpecification<ClientInsurance>(request.ClientId))
                    .Specify(new ClientInsurancesRpaAssignedSpecification())
                    .Select(expression)
                    .ToListAsync(cancellationToken: cancellationToken);

                return await Result<List<GetRpaAssignedInsurancesResponse>>.SuccessAsync(data);
            }
            catch (Exception ex)
            {
                return await Result<List<GetRpaAssignedInsurancesResponse>>.FailAsync(
                    "Getting RPA Assigned ClientInsurances failed" + (ex.InnerException != null
                        ? ex.InnerException.Message
                        : ex.Message));
            }
        }
    }
}