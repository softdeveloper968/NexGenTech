using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.AddEdit;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Queries.GetByCriteria
{
    public class GetClientFeeScheduleByCriteriaQuery : IRequest<Result<List<ClientFeeScheduleDto>>>
    {
        public int ClientId { get; set; }
        public int ClientInsuranceId { get; set; }
        public DateTime DateOfService { get; set; }
        
        public GetClientFeeScheduleByCriteriaQuery(int clientInsuranceId, DateTime dateOfService)
        {
            ClientInsuranceId = clientInsuranceId;
			DateOfService = dateOfService;
        }
    }

    public class GetClientFeeScheduleByCriteriaQueryHandler : IRequestHandler<GetClientFeeScheduleByCriteriaQuery, Result<List<ClientFeeScheduleDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetClientFeeScheduleByCriteriaQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<ClientFeeScheduleDto>>> Handle(GetClientFeeScheduleByCriteriaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                request.ClientId = _clientId;

                var specification = new GetAllFeeScheduleByCriteriaSpecification(request.ClientId, request.ClientInsuranceId, request.DateOfService);

                var entities = await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>()
                    .Entities
			        .Include(x => x.ClientFeeScheduleProviderLevels)
			        .Include(x => x.ClientFeeScheduleSpecialties)
					.Include(x => x.ClientInsuranceFeeSchedules)
                    .ThenInclude(c => c.ClientInsurance)
                    .Specify(specification)
                    .ToListAsync();

                if(entities != null)
                {
					var data = _mapper.Map<List<ClientFeeScheduleDto>>(entities);

					if (data != null)
					{
						return await Result<List<ClientFeeScheduleDto>>.SuccessAsync(data);
					}
				}
                
				return await Result<List<ClientFeeScheduleDto>>.FailAsync("Not Found");

			}
            catch (Exception ex)
            {
				return Result<List<ClientFeeScheduleDto>>.Fail(ex.Message);
			}
        }
    }

}
