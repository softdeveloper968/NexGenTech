using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetById
{
	public class GetFeeScheduleEntryByIdQuery : IRequest<Result<GetAllFeeScheduleEntryResponse>>
    {
        public int Id { get; set; }
    }

    public class GetFeeScheduleEntryByIdQueryHandler : IRequestHandler<GetFeeScheduleEntryByIdQuery, Result<GetAllFeeScheduleEntryResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IFeeScheduleEntryRepository _feeScheduleEntryRepository;
		private readonly IMapper _mapper;

        public GetFeeScheduleEntryByIdQueryHandler(IUnitOfWork<int> unitOfWork, IFeeScheduleEntryRepository feeScheduleEntryRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_feeScheduleEntryRepository = feeScheduleEntryRepository;
			_mapper = mapper;
        }

        public async Task<Result<GetAllFeeScheduleEntryResponse>> Handle(GetFeeScheduleEntryByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<Domain.Entities.ClientFeeScheduleEntry, GetAllFeeScheduleEntryResponse>> expression = e => new GetAllFeeScheduleEntryResponse
                {
                    Id = e.Id,
                    ClientId = e.ClientId,
                    ClientCptCodeId = e.ClientCptCodeId,
                    ClientFeeScheduleId = e.ClientFeeScheduleId,
                    AllowedAmount = (decimal)e.AllowedAmount, //EN-70
					Fee = e.Fee,
                    IsReimbursable = e.IsReimbursable,
                    ReimbursablePercentage = (decimal)e.ReimbursablePercentage //EN-70
                };
                var feeScheduleEntry = await _feeScheduleEntryRepository.GetById(query.Id);

                var mappedClient = _mapper.Map<GetAllFeeScheduleEntryResponse>(feeScheduleEntry);
                return await Result<GetAllFeeScheduleEntryResponse>.SuccessAsync(mappedClient);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
