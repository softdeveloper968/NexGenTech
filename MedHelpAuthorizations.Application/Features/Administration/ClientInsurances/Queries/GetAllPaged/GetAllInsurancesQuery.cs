using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllPaged
{
    public class GetAllInsurancesQuery : IRequest<PaginatedResult<GetAllPagedInsurancesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int ClientId { get; set; }

        public GetAllInsurancesQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllInsurancesQueryHandler : IRequestHandler<GetAllInsurancesQuery, PaginatedResult<GetAllPagedInsurancesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public GetAllInsurancesQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllPagedInsurancesResponse>> Handle(GetAllInsurancesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ClientInsurance, GetAllPagedInsurancesResponse>> expression = e => new GetAllPagedInsurancesResponse
            {
                Id = e.Id,
                LookupName = e.LookupName,
                Name = e.Name,
                ClientId = e.ClientId,
                PayerIdentifier = e.PayerIdentifier,
                RpaInsuranceId = e.RpaInsuranceId,
                RpaInsuranceCode = e.RpaInsurance != null ? e.RpaInsurance.Code : string.Empty,
                ExternalId = e.ExternalId,
                PhoneNumber = e.PhoneNumber,
                FaxNumber = e.FaxNumber,
                //ClientFeeScheduleId = e.ClientFeeSchedule.Id,
                //ClientFeeSchedule = e.ClientFeeSchedule,
                ClientInsuranceFeeSchedules = e.ClientInsuranceFeeSchedules,
                RequireLocationInput = e.RequireLocationInput,
                RequirePayerIdentifier = (e.RpaInsurance != null && (e.RpaInsurance.ApiIntegration != null && e.RpaInsurance.ApiIntegration.RequirePayerIdentifier)),
                AutoCalcPenalty = e.AutoCalcPenalty
            };

            request.ClientId = _clientId;

            var data = await _unitOfWork.Repository<ClientInsurance>().Entities
                .Include(x => x.ClientInsuranceFeeSchedules)
                  .ThenInclude(y => y.ClientFeeSchedule)
                .Include(z => z.RpaInsurance)///EN-597
                    .ThenInclude(c => c.ApiIntegration)///EN-597
                .Specify(new GenericByClientIdSpecification<ClientInsurance>(request.ClientId))
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return data;
        }
    }
}