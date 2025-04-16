using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetByClientFeeSchedule
{
	public class GetByClientFeeScheduleEntryQuery : IRequest<Result<List<GetAllFeeScheduleEntryResponse>>>
	{
		public int Id { get; set; }
	}

	public class GetByClientFeeScheduleEntryQueryHandler : IRequestHandler<GetByClientFeeScheduleEntryQuery, Result<List<GetAllFeeScheduleEntryResponse>>>
	{
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IMapper _mapper;

		public GetByClientFeeScheduleEntryQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<List<GetAllFeeScheduleEntryResponse>>> Handle(GetByClientFeeScheduleEntryQuery query, CancellationToken cancellationToken)
		{
			try
			{
				Expression<Func<Domain.Entities.ClientFeeScheduleEntry, GetAllFeeScheduleEntryResponse>> expression = e => _mapper.Map<GetAllFeeScheduleEntryResponse>(e);


				var data = await _unitOfWork.Repository<ClientFeeScheduleEntry>().Entities
					.Include(x => x.ClientCptCode)
					.Where(x => x.ClientFeeScheduleId == query.Id)
				   .Select(expression)
				   .ToListAsync();

				return await Result<List<GetAllFeeScheduleEntryResponse>>.SuccessAsync(data);
			}
			catch (Exception ex)
			{
				throw;
			}

		}
	}
}
