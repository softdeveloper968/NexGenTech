using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.AddEdit;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Queries
{
    public class GetAllPagedClientFeeScheduleQuery : IRequest<PaginatedResult<ClientFeeScheduleDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //  public string SearchString { get; set; }
        public int ClientId { get; set; }

        public GetAllPagedClientFeeScheduleQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllPagedClientFeeScheduleQueryHandler : IRequestHandler<GetAllPagedClientFeeScheduleQuery, PaginatedResult<ClientFeeScheduleDto>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetAllPagedClientFeeScheduleQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<ClientFeeScheduleDto>> Handle(GetAllPagedClientFeeScheduleQuery request, CancellationToken cancellationToken)
        {
            try
            {
				request.ClientId = _clientId;

				Expression<Func<Domain.Entities.ClientFeeSchedule, ClientFeeScheduleDto>> expression = e => _mapper.Map<ClientFeeScheduleDto>(e);

                var data = await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>()
                                                                                    .Entities													                               
														                            .Include(x => x.ClientFeeScheduleProviderLevels)
													                                .Include(x => x.ClientFeeScheduleSpecialties)
																					.Include(x => x.ClientInsuranceFeeSchedules)
                                                                                        .ThenInclude(c => c.ClientInsurance)
                                                                                    .Specify(new GenericByClientIdSpecification<Domain.Entities.ClientFeeSchedule>(request.ClientId))
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
