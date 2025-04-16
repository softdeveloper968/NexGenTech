using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Commands.AddEdit
{
    public partial class AddEditFeeScheduleEntryCommand : AddEditFeeScheduleEntryViewModel, IRequest<Result<int>>
    {

    }

    public class AddEditFeeScheduleEntryCommandHandler : IRequestHandler<AddEditFeeScheduleEntryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditFeeScheduleEntryCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
        private readonly IClientFeeScheduleRepository _clientFeeScheduleRepository;
        private readonly IClientFeeScheduleService _clientFeeScheduleService;
        private int _clientId => _currentUserService.ClientId;

        public AddEditFeeScheduleEntryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditFeeScheduleEntryCommandHandler> localizer, ICurrentUserService currentUserService, IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IClientFeeScheduleRepository clientFeeScheduleRepository, IClientFeeScheduleService clientFeeScheduleService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
            _clientFeeScheduleRepository = clientFeeScheduleRepository;
            _clientFeeScheduleService = clientFeeScheduleService;
        }

        public async Task<Result<int>> Handle(AddEditFeeScheduleEntryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientId = _clientId;
                if (command.Id == 0)
                {
                    // Find the first matching entry in the ClientFeeSchedule based on the clientCptCodeId .
                    var specification = new ClientFeeScheduleEntryByClientFeeScheduleIdSpecification(command.ClientFeeScheduleId);
                    var matchingEntry = await _unitOfWork.Repository<ClientFeeScheduleEntry>().Entities
                                                        .Specify(specification)
                                                        .FirstOrDefaultAsync(x => x.ClientCptCodeId == command.ClientCptCodeId);

                    // If a matching entry is found, delete it.
                    if (matchingEntry != null)
                    {
                        return await Result<int>.FailAsync(_localizer[$"Cannot create a new Fee schedule entry. An entry already exists for code: {command.ClientCptCode.Code}."]);
                    }


                    var feeScheduleEntry = _mapper.Map<ClientFeeScheduleEntry>(command);
                    feeScheduleEntry.ClientCptCode = null;
                    await _unitOfWork.Repository<ClientFeeScheduleEntry>().AddAsync(feeScheduleEntry);
                    await _unitOfWork.Commit(cancellationToken);

                    //EN-214
                    var feeSchedule = await _clientFeeScheduleRepository.GetByIdAsync(command.ClientFeeScheduleId);
                    if (feeSchedule != null)
                    {

                        if (command.IsReimbursable)
                        {
                            // Need to filter on ProveiderLevel ONLY if the Feeschedule has at least 1 FeeScheduleProviderLevels and same for FeeScheduleSpecialties
                            Expression<Func<ClaimStatusBatchClaim, bool>> Criteria = x => true;
                            Criteria = Criteria.And(c => c.ClientInsuranceId != null && feeSchedule.ClientInsuranceFeeSchedules.Select(x => x.ClientInsuranceId).ToList().Contains(c.ClientInsuranceId ?? 0) && c.DateOfServiceFrom >= feeSchedule.StartDate
                                                    && (feeSchedule.EndDate == null || c.DateOfServiceFrom <= feeSchedule.EndDate) && c.ClientCptCodeId == command.ClientCptCodeId);

                            if (feeSchedule.ClientFeeScheduleProviderLevels != null && feeSchedule.ClientFeeScheduleProviderLevels.Any())
                            {
                                Criteria = Criteria.And(c => c.ClientProvider != null && feeSchedule.ClientFeeScheduleProviderLevels.Select(x => x.ProviderLevelId).ToList().Contains(c.ClientProvider.ProviderLevelId ?? 0));
                            }

                            if (feeSchedule.ClientFeeScheduleSpecialties != null && feeSchedule.ClientFeeScheduleSpecialties.Any())
                            {
                                Criteria = Criteria.And(c => c.ClientProvider != null && feeSchedule.ClientFeeScheduleSpecialties.Select(x => x.SpecialtyId).ToList().Contains(c.ClientProvider.SpecialtyId));
                            }

                            _unitOfWork.Repository<ClaimStatusBatchClaim>().ExecuteUpdate(Criteria,
                               ec =>
                               {
                                   ec.ClientFeeScheduleEntryId = feeScheduleEntry.Id;
                               }
                           );
                        }
                        else
                        {
                            var claimStatusBatchClaimData = await _claimStatusBatchClaimsRepository.GetClaimsByCriteriaAsync(feeSchedule.ClientInsuranceFeeSchedules.Select(x => x.ClientInsuranceId).ToList(), command.ClientCptCodeId, feeSchedule.StartDate, feeSchedule.EndDate, feeSchedule.ClientFeeScheduleSpecialties.Select(x => x.SpecialtyId).ToList(), feeSchedule.ClientFeeScheduleProviderLevels.Select(x => x.ProviderLevelId).ToList());

                            if (claimStatusBatchClaimData != null && claimStatusBatchClaimData.Any())
                            {
                                foreach (var claim in claimStatusBatchClaimData)
                                {
                                    await _clientFeeScheduleService.ProcessFeeScheduleMatchedClaim(claim, null, feeScheduleEntry.Id);
                                }
                            }
                        }

                    }

                    return await Result<int>.SuccessAsync(feeScheduleEntry.Id, _localizer["Fee Schedule Entry Saved"]);
                }
                else
                {
                    var feeScheduleEntry = await _unitOfWork.Repository<ClientFeeScheduleEntry>().GetByIdAsync(command.Id);

                    if (feeScheduleEntry == null)
                    {
                        return await Result<int>.FailAsync(_localizer["Fee Schedule Entry Not Found!"]);
                    }

                    // Retrieve the fee schedule related to the entry
                    var feeSchedule = await _clientFeeScheduleRepository.GetByIdAsync(feeScheduleEntry.ClientFeeScheduleId);
                    if (feeSchedule == null)
                    {
                        return await Result<int>.FailAsync(_localizer["Fee Schedule Not Found!"]);
                    }

                    // Capture the original value of IsReimbursable before updating
                    bool originalIsReimbursable = feeScheduleEntry.IsReimbursable;

                    // Map the updated values from the command
                    _mapper.Map(command, feeScheduleEntry);

                    // Update the FeeScheduleEntry
                    await _unitOfWork.Repository<ClientFeeScheduleEntry>().UpdateAsync(feeScheduleEntry);
                    await _unitOfWork.Commit(cancellationToken);

                    // Define the common logic to get claims by criteria
                    var claimStatusBatchClaimData = await _claimStatusBatchClaimsRepository.GetClaimsByCriteriaAsync(
                        feeSchedule.ClientInsuranceFeeSchedules.Select(x => x.ClientInsuranceId).ToList(),
                        feeScheduleEntry.ClientCptCodeId,
                        feeSchedule.StartDate,
                        feeSchedule.EndDate,
                        feeSchedule.ClientFeeScheduleSpecialties.Select(x => x.SpecialtyId).ToList(),
                        feeSchedule.ClientFeeScheduleProviderLevels.Select(x => x.ProviderLevelId).ToList()
                    );

                    if (claimStatusBatchClaimData != null && claimStatusBatchClaimData.Any())
                    {
                        // If IsReimbursable changed from true to false, process claims
                        if (originalIsReimbursable && !feeScheduleEntry.IsReimbursable)
                        {
                            foreach (var claim in claimStatusBatchClaimData)
                            {
                                await _clientFeeScheduleService.ProcessFeeScheduleMatchedClaim(claim, null, feeScheduleEntry.Id);
                            }
                        }
                        // If IsReimbursable changed from false to true, update Contractual claims to Unknown
                        else if (!originalIsReimbursable && feeScheduleEntry.IsReimbursable)
                        {
                            foreach (var claim in claimStatusBatchClaimData)
                            {
                                await _clientFeeScheduleService.UpdateOrCreateClaimStatusTransactionAsync(claim, feeScheduleEntry.Id);
                            }
                        }
                    }

                    return await Result<int>.SuccessAsync(feeScheduleEntry.Id, _localizer["Fee Schedule Entry Updated"]);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}