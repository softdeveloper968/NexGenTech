using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.UnMappedClientFeeSchedule.Command.Delete
{
	public class DeleteClientFeeScheduleCptCommand : IRequest<Result<int>>
	{
		public int Id { get; set; }
	}

	public class DeleteClientFeeScheduleCptCommandHandler : IRequestHandler<DeleteClientFeeScheduleCptCommand, Result<int>>
	{
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IStringLocalizer<DeleteClientFeeScheduleCptCommandHandler> _localizer;

		public DeleteClientFeeScheduleCptCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientFeeScheduleCptCommandHandler> localizer)
		{
			_unitOfWork = unitOfWork;
			_localizer = localizer;
		}

		public async Task<Result<int>> Handle(DeleteClientFeeScheduleCptCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var data = await _unitOfWork.Repository<UnmappedFeeScheduleCpt>().GetByIdAsync(command.Id);
				if (data != null)
				{
					await _unitOfWork.Repository<UnmappedFeeScheduleCpt>().DeleteAsync(data);
					await _unitOfWork.Commit(cancellationToken);
					
				}
				return Result<int>.Fail(_localizer["ClientFeeScheduleEntry Not Found"]);
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}

		}
	}
}
