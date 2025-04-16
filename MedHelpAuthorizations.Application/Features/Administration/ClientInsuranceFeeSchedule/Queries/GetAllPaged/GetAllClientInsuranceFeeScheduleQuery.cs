using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule.Queries.GetAllPaged
{
	public class GetAllClientInsuranceFeeScheduleQuery : IRequest<PaginatedResult<GetAllClientInsuranceFeeScheduleResponse>>
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int ClientInsuranceId { get; set; }

		public GetAllClientInsuranceFeeScheduleQuery(int pageNumber, int pageSize, int clientInsuranceId)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			ClientInsuranceId = clientInsuranceId;
		}
	}

	public class GetAllClientInsuranceFeeScheduleQueryHandler : IRequestHandler<GetAllClientInsuranceFeeScheduleQuery, PaginatedResult<GetAllClientInsuranceFeeScheduleResponse>>
	{
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IMapper _mapper;

		public GetAllClientInsuranceFeeScheduleQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<PaginatedResult<GetAllClientInsuranceFeeScheduleResponse>> Handle(GetAllClientInsuranceFeeScheduleQuery request, CancellationToken cancellationToken)
		{
			try
			{
				Expression<Func<Domain.Entities.ClientInsuranceFeeSchedule, GetAllClientInsuranceFeeScheduleResponse>> expression = e => _mapper.Map<GetAllClientInsuranceFeeScheduleResponse>(e);

				var data = await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().Entities
													   .Include(x => x.ClientInsurance)
													   .Include(x => x.ClientFeeSchedule)
														.ThenInclude(y => y.ClientFeeScheduleProviderLevels)
													   .Include(x => x.ClientFeeSchedule)
														.ThenInclude(y => y.ClientFeeScheduleSpecialties)
													   .Include(y => y.ClientFeeSchedule)
														.ThenInclude(y => y.ClientInsuranceFeeSchedules)
													   .ThenInclude(x => x.ClientInsurance)
													   .Where(e => e.ClientInsuranceId == request.ClientInsuranceId) // Filter by ClientInsuranceId //AA-259 done in 261
													   .Select(expression)
													   .ToPaginatedListAsync(request.PageNumber, request.PageSize);

				return data;

			}
			catch (Exception)
			{
				throw;
			}

		}
	}
}
