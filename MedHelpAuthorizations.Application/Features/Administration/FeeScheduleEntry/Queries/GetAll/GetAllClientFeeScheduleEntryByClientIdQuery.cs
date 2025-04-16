using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetAll
{
    public class GetAllClientFeeScheduleEntryByClientIdQuery : IRequest<Result<List<GetAllFeeScheduleEntryResponse>>>
    {
		public int ClientId { get; set; }
	}
    public class GetAllClientFeeScheduleEntryByClientIdQueryHandler : IRequestHandler<GetAllClientFeeScheduleEntryByClientIdQuery, Result<List<GetAllFeeScheduleEntryResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;
		private int _clientId => _currentUserService.ClientId;

		public GetAllClientFeeScheduleEntryByClientIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetAllFeeScheduleEntryResponse>>> Handle(GetAllClientFeeScheduleEntryByClientIdQuery request, CancellationToken cancellationToken)
        {
			Expression<Func<ClientFeeScheduleEntry, GetAllFeeScheduleEntryResponse>> expression = e => _mapper.Map<GetAllFeeScheduleEntryResponse>(e);
			if (request.ClientId == 0)
			{
				request.ClientId = _clientId;
			}
			var clientCriteriaSpec = new GenericByClientIdSpecification<ClientFeeScheduleEntry>(request.ClientId);

			var data = await _unitOfWork.Repository<ClientFeeScheduleEntry>().Entities
				.Include(x => x.ClientCptCode) //AA-323
			   .Specify(clientCriteriaSpec)
			   .Select(expression)
			   .ToListAsync();

			return await Result<List<GetAllFeeScheduleEntryResponse>>.SuccessAsync(data);
        }
    }
}
