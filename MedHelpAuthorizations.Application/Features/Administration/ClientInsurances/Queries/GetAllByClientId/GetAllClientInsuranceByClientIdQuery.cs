using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllByClientId
{
    public class GetAllClientInsuranceByClientIdQuery : IRequest<Result<List<GetAllClientInsurancesByClientIdResponse>>>
    {
        //public int PageNumber { get; set; }
        //public int PageSize { get; set; }
        public int ClientId { get; set; }

        public GetAllClientInsuranceByClientIdQuery()
        {
            //PageNumber = pageNumber;
            //PageSize = pageSize;
        }
    }

    public class GetClientInsurancesQueryHandler : IRequestHandler<GetAllClientInsuranceByClientIdQuery, Result<List<GetAllClientInsurancesByClientIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetClientInsurancesQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetAllClientInsurancesByClientIdResponse>>> Handle(GetAllClientInsuranceByClientIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<ClientInsurance, GetAllClientInsurancesByClientIdResponse>> expression = e => new GetAllClientInsurancesByClientIdResponse
                {
                    Id = e.Id,
                    LookupName = e.LookupName,
                    Name = e.Name,
                    ClientId = e.ClientId,
                    PayerIdentifier = e.PayerIdentifier,
                    RpaInsuranceCode = e.RpaInsurance != null ? e.RpaInsurance.Code : string.Empty,
                    ExternalId = e.ExternalId,
                    PhoneNumber = e.PhoneNumber,
                    FaxNumber = e.FaxNumber,
                    RpaInsuranceGroupId = e.RpaInsurance != null ? e.RpaInsurance.RpaInsuranceGroupId != null ? e.RpaInsurance.RpaInsuranceGroupId : null : null,
                    AutoCalcPenalty = e.AutoCalcPenalty,
                };

                if(request.ClientId == 0)
                {
                    request.ClientId = _clientId;
                }
               

                var data = await _unitOfWork.Repository<ClientInsurance>().Entities
                    .Specify(new GenericByClientIdSpecification<ClientInsurance>(request.ClientId))
                    .Select(expression)
                    .ToListAsync(cancellationToken: cancellationToken);

                return await Result<List<GetAllClientInsurancesByClientIdResponse>>.SuccessAsync(data);
            }
            catch (Exception ex)
            {
                return await Result<List<GetAllClientInsurancesByClientIdResponse>>.FailAsync(
                    $"Getting ClientInsurances for ClientId: {_clientId} failed" + ex.InnerException != null
                        ? ex.InnerException.Message
                        : ex.Message);
            }
        }
    }
}
