using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule.Queries.GetByClientFeeScheduleId
{
    public class GetByClientFeeScheduleIdQuery : IRequest<Result<List<GetAllClientInsuranceFeeScheduleResponse>>>
    {
        public int ClientFeeScheduleId { get; set; }

        public GetByClientFeeScheduleIdQuery()
        {
        }
    }

    public class GetByClientFeeScheduleIdQueryHandler : IRequestHandler<GetByClientFeeScheduleIdQuery, Result<List<GetAllClientInsuranceFeeScheduleResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClientInsuranceFeeScheduleRepository _clientInsuranceFeeScheduleRepository;

        public GetByClientFeeScheduleIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IClientInsuranceFeeScheduleRepository clientInsuranceFeeScheduleRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _clientInsuranceFeeScheduleRepository = clientInsuranceFeeScheduleRepository;
        }

        public async Task<Result<List<GetAllClientInsuranceFeeScheduleResponse>>> Handle(GetByClientFeeScheduleIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>()
                                                         .Entities.Include(x => x.ClientInsurance)
                                                         .Where(y => y.ClientInsuranceId == request.ClientFeeScheduleId)
                                                         .ToListAsync();

            var mappedData = _mapper.Map<List<GetAllClientInsuranceFeeScheduleResponse>>(data);
            return await Result<List<GetAllClientInsuranceFeeScheduleResponse>>.SuccessAsync(mappedData);

        }
    }
}


