using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.Client_ApplicationFeatures;
using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Base;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetAllPaged
{
    public class GetAllClientsQuery : IRequest<PaginatedResult<GetAllPagedClientsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public GetAllClientsQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GGetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, PaginatedResult<GetAllPagedClientsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GGetAllClientsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllPagedClientsResponse>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<Domain.Entities.Client, GetAllPagedClientsResponse>> expression = e => new GetAllPagedClientsResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    ClientCode = e.ClientCode,
                    PhoneNumber = e.PhoneNumber,
                    FaxNumber = e.FaxNumber,
                    ClientAuthTypes = _mapper.Map<List<ClientAuthTypeDto>>(e.ClientAuthTypes.ToList()),
                    ClientApplicationFeatures = _mapper.Map<List<ClientApplicationFeatureDto>>(e.ClientApplicationFeatures.ToList()),
					ClientKpi = _mapper.Map<ClientKpiDto>(e.ClientKpi),
					EmployeeClients = _mapper.Map<List<EmployeeClientViewModel>>(e.EmployeeClients.ToList()),
                    SourceSystemId=e.SourceSystemId,
                    TaxId = e.TaxId,
                    AutoLogMinutes = e.AutoLogMinutes,
                    //ClientDaysOfOperation = e.ClientDaysOfOperation.ToList(),
                    //ClientHolidays = e.ClientHolidays.ToList()
                    
				};
                var clientFilterSpec = new ClientFilterSpecification(request.SearchString);

                var data = await _unitOfWork.Repository<Domain.Entities.Client>().Entities
                                   .Specify(clientFilterSpec)
                                   .Select(expression)
                                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}