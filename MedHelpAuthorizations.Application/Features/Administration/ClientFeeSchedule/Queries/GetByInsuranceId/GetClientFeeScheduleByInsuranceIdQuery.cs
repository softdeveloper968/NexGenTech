using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.AddEdit;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Queries.GetByInsuranceId
{
    public class GetClientFeeScheduleByInsuranceIdQuery : IRequest<Result<List<ClientFeeScheduleDto>>>
    {
        public int ClientInsuranceId { get; set; }
        public int ClientId { get; set; }
    }
    public class GetClientFeeScheduleByInsuranceIdQueryHandler : IRequestHandler<GetClientFeeScheduleByInsuranceIdQuery, Result<List<ClientFeeScheduleDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientFeeScheduleByInsuranceIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ClientFeeScheduleDto>>> Handle(GetClientFeeScheduleByInsuranceIdQuery query, CancellationToken cancellationToken)
        {
            var feeSchedules = await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>()
               .Entities
			   .Include(x => x.ClientFeeScheduleProviderLevels)
			   .Include(x => x.ClientFeeScheduleSpecialties)
			   .Where(z => z.ClientId == query.ClientId)
               .ToListAsync();

            var data = feeSchedules
                              .Where(z => z.ClientInsuranceFeeSchedules.Select(x => x.ClientInsurance).ToList().Select(x => x.Id).Contains(query.ClientInsuranceId))
                              .ToList();

            var feeScheduleData = _mapper.Map<List<ClientFeeScheduleDto>>(data);
            return Result<List<ClientFeeScheduleDto>>.Success(feeScheduleData);
        }
    }
}

