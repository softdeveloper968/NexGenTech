using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetById;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.CheckedMatchCptCode
{
	public class CheckedMatchCptCodeQuery : IRequest<Result<GetClientCptCodeByIdResponse>>
	{
		public int Id { get; set; }
	}
	public class CheckedMatchCptCodeQueryHandler : IRequestHandler<CheckedMatchCptCodeQuery, Result<GetClientCptCodeByIdResponse>>
	{
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public CheckedMatchCptCodeQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}


		public async Task<Result<GetClientCptCodeByIdResponse>> Handle(CheckedMatchCptCodeQuery request, CancellationToken cancellationToken)
		{
			var clientCptCode = await _unitOfWork.Repository<ClientCptCode>().GetByIdAsync(request.Id);

			if (clientCptCode != null)
			{
				var unmappedFeeScheduleCpt = await _unitOfWork.Repository<UnmappedFeeScheduleCpt>().Entities.Include(x => x.ClientCptCode).ToListAsync();
				if(unmappedFeeScheduleCpt != null)
				{
                    var matchedData = unmappedFeeScheduleCpt.Where(x => x.ClientCptCode.Code == clientCptCode.Code && x.ClientId == clientCptCode.ClientId);

                    if (matchedData != null && matchedData.Any())
                    {
                        var resultData = _mapper.Map<GetClientCptCodeByIdResponse>(clientCptCode);
                        return await Result<GetClientCptCodeByIdResponse>.SuccessAsync(resultData);
                    }
                }
                return await Result<GetClientCptCodeByIdResponse>.FailAsync("No matching data found");

            }
			else
			{
				return await Result<GetClientCptCodeByIdResponse>.FailAsync("ClientCptCode not found");
			}
		}

	}
}
