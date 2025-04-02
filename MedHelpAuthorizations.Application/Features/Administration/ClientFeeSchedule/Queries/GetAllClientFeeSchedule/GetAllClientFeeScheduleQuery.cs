using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.AddEdit;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Queries.GetAllClientFeeSchedule
{
    public class GetAllClientFeeScheduleQuery : IRequest<Result<List<ClientFeeScheduleDto>>>
    {
        public int ClientId { get; set; }
    }

    public class GetAllClientFeeScheduleQueryHandler : IRequestHandler<GetAllClientFeeScheduleQuery, Result<List<ClientFeeScheduleDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		private int _clientId => _currentUserService.ClientId;

		public GetAllClientFeeScheduleQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

        public async Task<Result<List<ClientFeeScheduleDto>>> Handle(GetAllClientFeeScheduleQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.Entities.ClientFeeSchedule, ClientFeeScheduleDto>> expression = e => new ClientFeeScheduleDto
            {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
            };
			request.ClientId = _clientId;
			var data = await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().Entities
			    .Include(x => x.ClientFeeScheduleProviderLevels)
			    .Include(x => x.ClientFeeScheduleSpecialties)
			    .Include(x => x.ClientInsuranceFeeSchedules)
                    .ThenInclude(y => y.ClientInsurance)
				.Specify(new GenericByClientIdSpecification<Domain.Entities.ClientFeeSchedule>(request.ClientId))
				.Select(expression)
                .ToListAsync();
            return Result<List<ClientFeeScheduleDto>>.Success(data);
        }
    }
}
