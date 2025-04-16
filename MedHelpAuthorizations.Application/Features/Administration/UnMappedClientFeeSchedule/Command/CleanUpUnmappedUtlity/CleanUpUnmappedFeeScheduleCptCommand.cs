using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.UnMappedClientFeeSchedule.Command.CleanUpUnmappedUtlity
{
	public class CleanUpUnmappedFeeScheduleCptCommand : IRequest<Result<int>>
	{

	}

	public class CleanUpUnamappedFeeScheduleCptHandler : IRequestHandler<CleanUpUnmappedFeeScheduleCptCommand, Result<int>>
	{
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IStringLocalizer<CleanUpUnamappedFeeScheduleCptHandler> _localizer;
		private readonly IClaimStatusQueryService _claimStatusQueryService;
		private readonly ICurrentUserService _currentUserService;
		private int _clientId => _currentUserService.ClientId;

		public CleanUpUnamappedFeeScheduleCptHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<CleanUpUnamappedFeeScheduleCptHandler> localizer,
			IClaimStatusQueryService claimStatusQueryService,
			ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_localizer = localizer;
			_currentUserService = currentUserService;
			_claimStatusQueryService = claimStatusQueryService;
		}

		public async Task<Result<int>> Handle(CleanUpUnmappedFeeScheduleCptCommand command, CancellationToken cancellationToken)
		{
			try
			{//EN-156
				var data = await _unitOfWork.Repository<UnmappedFeeScheduleCpt>().Entities.Include(x => x.ClientCptCode).ToListAsync();
				var unmappedFeeScheduleCptData = data?.Where(x => x.ClientId == _clientId)?.ToList();

				if (unmappedFeeScheduleCptData != null && unmappedFeeScheduleCptData.Any())
				{
					foreach (var item in unmappedFeeScheduleCptData)
					{
						var feeSchedules = await _claimStatusQueryService.GetClientFeeScheduleEntry(item.ClientCptCode.Code, item.ClientInsuranceId, item.ReferencedDateOfServiceFrom);

						if (feeSchedules != null)
						{
							await _unitOfWork.Repository<UnmappedFeeScheduleCpt>().DeleteAsync(item);
						}
					}

					await _unitOfWork.Commit(cancellationToken);
				}

				return Result<int>.Success();
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}

		}
	}
}