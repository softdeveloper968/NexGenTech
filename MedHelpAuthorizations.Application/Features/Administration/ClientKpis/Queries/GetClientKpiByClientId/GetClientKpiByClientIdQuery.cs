using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Queries.GetClientKpiByClientId
{
    public class GetClientKpiByClientIdQuery : IRequest<Result<ClientKpiDto>>
	{
		public int ClientId { get; set; }
	}

	public class GetClientKpiByClientIdQueryHandler : IRequestHandler<GetClientKpiByClientIdQuery, Result<ClientKpiDto>>
	{
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        private int _clientId => _currentUserService.ClientId;
        public GetClientKpiByClientIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}

		public async Task<Result<ClientKpiDto>> Handle(GetClientKpiByClientIdQuery request, CancellationToken cancellationToken)
		{

			if (request.ClientId == 0) 
			{ 
				request.ClientId = _clientId;
			}

			Expression<Func<ClientKpi, ClientKpiDto>> expression = e => _mapper.Map<ClientKpiDto>(e);
			var clientKpiCriteriaSpec = new ClientKpiByClientIdSpecification(request.ClientId);

			var data = await _unitOfWork.Repository<ClientKpi>().Entities
				.Include("Client.EmployeeClients")
			   .Specify(clientKpiCriteriaSpec)
			   .Select(expression).FirstOrDefaultAsync();

			return await Result<ClientKpiDto>.SuccessAsync(data);
		}
	}
}
