using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetById;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetByCriteria
{
	public class GetCptCodeByCriteriaQuery : IRequest<Result<GetClientCptCodeByIdResponse>>
	{
		public string ProcedureCode { get; set; }
		public GetCptCodeByCriteriaQuery() { }
	}

	public class GetCptCodeByCriteriaQueryHandler : IRequestHandler<GetCptCodeByCriteriaQuery, Result<GetClientCptCodeByIdResponse>>
	{
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;
		private readonly IClientCptCodeRepository _clientCptCodeRepository;
		private int _clientId => _currentUserService.ClientId;

		public GetCptCodeByCriteriaQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IClientCptCodeRepository clientCptCodeRepository)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentUserService = currentUserService;
			_clientCptCodeRepository = clientCptCodeRepository;
		}

		public async Task<Result<GetClientCptCodeByIdResponse>> Handle(GetCptCodeByCriteriaQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var data = await _clientCptCodeRepository.GetByClientId(_clientId, query.ProcedureCode);
				var mapper = _mapper.Map<GetClientCptCodeByIdResponse>(data);

				if(mapper != null)
				{
					return Result<GetClientCptCodeByIdResponse>.Success(mapper);
				}

				return Result<GetClientCptCodeByIdResponse>.Fail("Not Found");
			}
			catch (Exception ex)
			{
				// Handle any exceptions here
				return Result<GetClientCptCodeByIdResponse>.Fail(ex.Message);
			}
		}

	}
}
