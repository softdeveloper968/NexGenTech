using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetById;
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

namespace MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetBySearch
{
	public class GetClientFeeScheduleBySearchQuery : IRequest<Result<List<GetClientCptCodeByIdResponse>>>
	{
		public string SearchString { get; set; }
	}

	public class GetClientFeeScheduleBySearchQueryHandler : IRequestHandler<GetClientFeeScheduleBySearchQuery, Result<List<GetClientCptCodeByIdResponse>>>
	{
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;
		private int _clientId => _currentUserService.ClientId;

		public GetClientFeeScheduleBySearchQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}

		public async Task<Result<List<GetClientCptCodeByIdResponse>>> Handle(GetClientFeeScheduleBySearchQuery request, CancellationToken cancellationToken)
		{
			Expression<Func<ClientCptCode, GetClientCptCodeByIdResponse>> expression = e => _mapper.Map<GetClientCptCodeByIdResponse>(e);

			var cptCodeCriteriaSpec = new ClientCptCodeBySearchSpecification(request.SearchString, _clientId);

			var data = await _unitOfWork.Repository<ClientCptCode>().Entities
			   .Specify(cptCodeCriteriaSpec)
			   .Select(expression).ToListAsync();

			return new Result<List<GetClientCptCodeByIdResponse>> { Data = data };
		}
	}
}
