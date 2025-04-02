using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule.Command.AddEdit
{
	public class AddEditClientInsuranceFeeScheduleCommand : IRequest<Result<int>>
	{
		public int Id { get; set; }
		public int ClientFeeScheduleId { get; set; }
		public int ClientInsuranceId { get; set; }
		public bool IsActive { get; set; }
	}

	public class AddEditClientInsuranceFeeScheduleCommandHandler : IRequestHandler<AddEditClientInsuranceFeeScheduleCommand, Result<int>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IStringLocalizer<AddEditClientInsuranceFeeScheduleCommand> _localizer;

		public AddEditClientInsuranceFeeScheduleCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditClientInsuranceFeeScheduleCommand> localizer)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_localizer = localizer;
		}

		public async Task<Result<int>> Handle(AddEditClientInsuranceFeeScheduleCommand command, CancellationToken cancellationToken)
		{
			try
			{

				if (command.Id == 0)
				{
					var feeSchedule = await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().Entities.Include(x => x.ClientFeeSchedule).ToListAsync();
					var filterdata = feeSchedule?.Where(x => x.ClientInsuranceId == command.ClientInsuranceId).OrderByDescending(x => x.ClientFeeSchedule.EndDate);
					var clientFee = await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().GetByIdAsync(command.ClientFeeScheduleId);

					if (filterdata.Any())
					{
						var insurancefilterEndDate = filterdata.Select(x => x.ClientFeeSchedule.EndDate).FirstOrDefault();
						var feeFilterEndDate = clientFee.EndDate;

						if (feeFilterEndDate > insurancefilterEndDate)
						{
							var clientInsuranceFeedata = new Domain.Entities.ClientInsuranceFeeSchedule
							{
								Id = filterdata.Select(x => x.Id).FirstOrDefault(),
								ClientFeeScheduleId = filterdata.Select(x => x.ClientFeeScheduleId).FirstOrDefault(),
								ClientInsuranceId = filterdata.Select(x => x.ClientInsuranceId).FirstOrDefault(),
								IsActive = false
							};
							await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().UpdateAsync(clientInsuranceFeedata);
							await _unitOfWork.Commit(cancellationToken);

							var clientInsuranceFeeSchedules = new Domain.Entities.ClientInsuranceFeeSchedule
							{
								ClientFeeScheduleId = command.ClientFeeScheduleId,
								ClientInsuranceId = command.ClientInsuranceId,
								IsActive = true
							};
							await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().AddAsync(clientInsuranceFeeSchedules);
							await _unitOfWork.Commit(cancellationToken);
						}
						else if (feeFilterEndDate < insurancefilterEndDate)
						{
							var clientInsuranceFeeSchedules = new Domain.Entities.ClientInsuranceFeeSchedule
							{
								ClientFeeScheduleId = command.ClientFeeScheduleId,
								ClientInsuranceId = command.ClientInsuranceId,
								IsActive = false
							};

							await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().AddAsync(clientInsuranceFeeSchedules);
							await _unitOfWork.Commit(cancellationToken);
						}
					}
					else
					{
						var clientInsuranceFeeSchedules = new Domain.Entities.ClientInsuranceFeeSchedule
						{
							ClientFeeScheduleId = command.ClientFeeScheduleId,
							ClientInsuranceId = command.ClientInsuranceId,
							IsActive = true
						};

						await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().AddAsync(clientInsuranceFeeSchedules);
						await _unitOfWork.Commit(cancellationToken);
					}
					return await Result<int>.SuccessAsync(command.Id, _localizer["Client Fee Schedule Saved"]);
				}

				else
				{
					var data = await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().GetByIdAsync(command.Id);
					if (data.Id != 0)
					{
						await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().DeleteAsync(data);
						await _unitOfWork.Commit(cancellationToken);
					}

					var feeSchedule = await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().Entities.Include(x => x.ClientFeeSchedule).ToListAsync();
					var filterdata = feeSchedule.Where(x => x.ClientInsuranceId == command.ClientInsuranceId).OrderByDescending(x => x.ClientFeeSchedule.EndDate);

					if (filterdata.Any())
					{
						filterdata.FirstOrDefault(x => x.IsActive = true);
						await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().UpdateAsync(filterdata.FirstOrDefault());
						await _unitOfWork.Commit(cancellationToken);
					}
					return await Result<int>.SuccessAsync(_localizer["Deleted"]);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
