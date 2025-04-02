using AutoMapper;
using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Commands.AddEdit
{
    public class AddEditCopyClientFeeScheduleEntryCommand : IRequest<Result<int>>
	{
		public int ClientFeeScheduleId { get; set; }
		public int CopyClientFeeScheduleId { get; set; }
		public int ClientId { get; set; }
	}
	public class AddEditCopyClientFeeScheduleEntryCommandHandler : IRequestHandler<AddEditCopyClientFeeScheduleEntryCommand, Result<int>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		private readonly IStringLocalizer<AddEditCopyClientFeeScheduleEntryCommand> _localizer;
		private readonly IManuallyRunJobService _manuallyRunJobService;
        private readonly ITenantInfo _tenantInfo;
        private int _clientId => _currentUserService.ClientId;

		public AddEditCopyClientFeeScheduleEntryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditCopyClientFeeScheduleEntryCommand> localizer, ICurrentUserService currentUserService, IManuallyRunJobService manuallyRunJobService, ITenantInfo tenantInfo)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_localizer = localizer;
			_currentUserService = currentUserService;
			_manuallyRunJobService = manuallyRunJobService;
			_tenantInfo = tenantInfo;

        }
		public async Task<Result<int>> Handle(AddEditCopyClientFeeScheduleEntryCommand request, CancellationToken cancellationToken)
		{
			request.ClientId = _clientId;


			Expression<Func<ClientFeeScheduleEntry, bool>> filterExpression = x =>
				   x.ClientFeeScheduleId == request.ClientFeeScheduleId &&
				   x.ClientId == request.ClientId;

			// Execute the delete operation with the specified filter expression
			_unitOfWork.Repository<ClientFeeScheduleEntry>().ExecuteDelete(filterExpression);
			await _unitOfWork.Commit(cancellationToken);

			var clientFeeScheduleEntryData = await _unitOfWork.Repository<ClientFeeScheduleEntry>().Entities.Specify(new ClientFeeScheduleEntryByClientFeeScheduleIdSpecification(request.CopyClientFeeScheduleId)).ToListAsync();
			var ClientFeeScheduleData = await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().GetByIdAsync(request.ClientFeeScheduleId);

			var clientFeeScheduleEntries = clientFeeScheduleEntryData.Select(item => new ClientFeeScheduleEntry()
			{
				Fee = item.Fee,
				AllowedAmount = item.AllowedAmount,
				IsReimbursable = item.IsReimbursable,
				ClientCptCodeId = item.ClientCptCodeId,
				ClientFeeScheduleId = request.ClientFeeScheduleId,
				ClientId = request.ClientId
			}).ToList();
			_unitOfWork.Repository<ClientFeeScheduleEntry>().AddRange(clientFeeScheduleEntries);
			await _unitOfWork.Commit(cancellationToken);
			
			if(ClientFeeScheduleData != null)
			{
				ClientFeeScheduleData.ImportStatus = Domain.Entities.Enums.ImportStatusEnum.NotApplicable;
				await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().UpdateAsync(ClientFeeScheduleData);
				await _unitOfWork.Commit(cancellationToken);
			}

            BackgroundJob.Enqueue(() => _manuallyRunJobService.ProcessFeeScheduleEntriesForSingleTenant(_tenantInfo.Identifier, request.ClientFeeScheduleId)); //EN-232

            return await Result<int>.SuccessAsync(request.ClientFeeScheduleId, _localizer["Client Fee Schedule Entry Saved"]);
		}
	}
}
