using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.UpdateClientFeeSchedule
{
    public class UpdateClientFeeScheduleCommand : ClientInsuranceFeeScheduleDto, IRequest<Result<int>>
    {
    }
    public class UpdateClientFeeScheduleCommandHandler : IRequestHandler<UpdateClientFeeScheduleCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<UpdateClientFeeScheduleCommand> _localizer;

        public UpdateClientFeeScheduleCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<UpdateClientFeeScheduleCommand> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(UpdateClientFeeScheduleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var feeSchedules = await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().GetAllAsync();

                
                if (feeSchedules != null)
                {
					// Filter fee schedules based on the provided client insurance ID
					var clientSchedules = feeSchedules?.Where(x => x.ClientInsuranceId == command.ClientInsuranceId).ToList();

                    if (clientSchedules != null)
                    {
                        // Find the first active fee schedule for the specified client insurance
                        //var activeOne = clientSchedules.FirstOrDefault(x => x.IsActive);

                        //if (activeOne != null)
                        //{
                        //    // Deactivate the currently active fee schedule
                        //    activeOne.IsActive = false;
                        //    await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().UpdateAsync(activeOne);
                        //    await _unitOfWork.Commit(cancellationToken);
                        //}

                        // Retrieve the fee schedule to update based on the provided ID
                        var scheduleToUpdate = await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().GetByIdAsync(command.Id);

                        if (scheduleToUpdate != null)
                        {
                            // Update the retrieved fee schedule with the provided IsActive value
                            scheduleToUpdate.IsActive = command.IsActive;
                            await _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().UpdateAsync(scheduleToUpdate);
                            await _unitOfWork.Commit(cancellationToken);
                        }

                        return await Result<int>.SuccessAsync(_localizer["Successfully updated"]);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }

            return await Result<int>.FailAsync(_localizer["Update failed"]);
        }

    }

}
