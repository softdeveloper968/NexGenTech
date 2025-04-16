using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetAllPaged
{
	public class GetAllFeeScheduleEntryQuery : IRequest<PaginatedResult<GetAllFeeScheduleEntryResponse>>
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int ClientFeeScheduleId { get; set; }
		public string SearchString { get; set; } = string.Empty;
		public string SortLabel { get; set; } = string.Empty; // Sorting field name
		public string SortDirection { get; set; } = "asc";    // Sorting direction ("asc" or "desc")

		public GetAllFeeScheduleEntryQuery(int pageNumber, int pageSize, int clientFeeScheduleId, string searchString, string sortLabel, string sortDirection)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			ClientFeeScheduleId = clientFeeScheduleId;
			SearchString = searchString;
			SortLabel = sortLabel;
			SortDirection = sortDirection;
		}
	}

	public class GetAllFeeScheduleEntryQueryHandler : IRequestHandler<GetAllFeeScheduleEntryQuery, PaginatedResult<GetAllFeeScheduleEntryResponse>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		private int _clientId => _currentUserService.ClientId;

		public GetAllFeeScheduleEntryQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<PaginatedResult<GetAllFeeScheduleEntryResponse>> Handle(GetAllFeeScheduleEntryQuery request, CancellationToken cancellationToken)
		{
			Expression<Func<ClientFeeScheduleEntry, GetAllFeeScheduleEntryResponse>> expression = e => new GetAllFeeScheduleEntryResponse
			{
				Id = e.Id,
				ClientId = e.ClientId,
				ClientCptCodeId = e.ClientCptCodeId,
				ClientFeeScheduleId = e.ClientFeeScheduleId,
				AllowedAmount = e.AllowedAmount ?? 0m,
				Fee = e.Fee,
				IsReimbursable = e.IsReimbursable,
				ClientCptCode = _mapper.Map<ClientCptCodeDto>(e.ClientCptCode),
				ReimbursablePercentage = e.ReimbursablePercentage * 100 ?? 0m,
			};

			var query = _unitOfWork.Repository<ClientFeeScheduleEntry>()
						.Entities
						.Include(x => x.ClientCptCode)
						.Specify(new ClientCptCodesByClientFeeScheduleIdSpecification(request.SearchString, request.ClientFeeScheduleId))
						.Select(e => new
						{
							e.Id,
							e.ClientId,
							e.ClientCptCodeId,
							e.ClientFeeScheduleId,
							e.AllowedAmount,
							e.Fee,
							e.IsReimbursable,
							e.ClientCptCode.Code,  // Project ClientCptCode.Code directly
							e.ReimbursablePercentage
						});

			// Apply sorting based on SortLabel and SortDirection
			switch (request.SortLabel)
			{
				case "idField":
					query = request.SortDirection == "asc" ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id);
					break;
				case "procedure":
					query = request.SortDirection == "asc"
						? query.OrderBy(p => p.Code)  // Use direct field for sorting
						: query.OrderByDescending(p => p.Code);
					break;
				case "fee":
					query = request.SortDirection == "asc" ? query.OrderBy(p => p.Fee) : query.OrderByDescending(p => p.Fee);
					break;
				case "allowedAmount":
					query = request.SortDirection == "asc" ? query.OrderBy(p => p.AllowedAmount) : query.OrderByDescending(p => p.AllowedAmount);
					break;
				case "reimbursablePercentage":
					query = request.SortDirection == "asc" ? query.OrderBy(p => p.ReimbursablePercentage) : query.OrderByDescending(p => p.ReimbursablePercentage);
					break;
				case "isReimbursable":
					query = request.SortDirection == "asc" ? query.OrderBy(p => p.IsReimbursable) : query.OrderByDescending(p => p.IsReimbursable);
					break;
				default:
					query = query.OrderBy(p => p.Id); // Default sorting by Id
					break;
			}

			// Perform pagination
			var paginatedData = await query
				.Skip((request.PageNumber - 1) * request.PageSize)
				.Take(request.PageSize)
				.ToListAsync(cancellationToken);

			// Apply mapping after data is retrieved
			var mappedData = paginatedData.Select(e => new GetAllFeeScheduleEntryResponse
			{
				Id = e.Id,
				ClientId = e.ClientId,
				ClientCptCodeId = e.ClientCptCodeId,
				ClientFeeScheduleId = e.ClientFeeScheduleId,
				AllowedAmount = e.AllowedAmount ?? 0m,
				Fee = e.Fee,
				IsReimbursable = e.IsReimbursable,
				ClientCptCode = new ClientCptCodeDto { Code = e.Code , Id = e.ClientCptCodeId},  // Map manually
				ReimbursablePercentage = e.ReimbursablePercentage * 100 ?? 0m,
			}).ToList();



			return new PaginatedResult<GetAllFeeScheduleEntryResponse>(mappedData)
			{
				TotalCount = query.Count(),
				CurrentPage = request.PageNumber,
				PageSize = request.PageSize
			};

		}
	}
}
