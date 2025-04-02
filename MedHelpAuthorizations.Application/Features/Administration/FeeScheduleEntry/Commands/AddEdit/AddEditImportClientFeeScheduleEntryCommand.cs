using AutoMapper;
using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Commands.AddEdit
{
    public class AddEditImportClientFeeScheduleEntryCommand : ImportClientFeeScheduleEntryDto, IRequest<Result<int>>
    {
    }

    public class AddEditImportClientFeeScheduleEntryCommandHandler : IRequestHandler<AddEditImportClientFeeScheduleEntryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<AddEditImportClientFeeScheduleEntryCommand> _localizer;
        private readonly IInputDataService _inputDataService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IManuallyRunJobService _manuallyRunJobService;
        private readonly ITenantInfo _tenantInfo;
        private int _clientId => _currentUserService.ClientId;


        public AddEditImportClientFeeScheduleEntryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IMediator mediator, IStringLocalizer<AddEditImportClientFeeScheduleEntryCommand> localizer,
            IInputDataService inputDataService, ICurrentUserService currentUserService, IManuallyRunJobService manuallyRunJobService, ITenantInfo tenantInfo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
            _localizer = localizer;
            _inputDataService = inputDataService;
            _currentUserService = currentUserService;
            _manuallyRunJobService = manuallyRunJobService;
            _tenantInfo = tenantInfo;
        }

        public async Task<Result<int>> Handle(AddEditImportClientFeeScheduleEntryCommand command, CancellationToken cancellationToken)
        {
            command.ClientId = _clientId;
            if (command.UploadRequest != null)
            {
                var uploadRequest = command.UploadRequest;

                var ClientFeeScheduleEntryCreated = new List<ClientFeeScheduleEntryCsvViewModel>();
                // Deserialize client fee schedule entry data from the uploaded document
                var ClientFeeScheduleEntryCollection = await _inputDataService.DeserializeClientFeeScheduleEntryDataAsync(uploadRequest?.Data, uploadRequest?.Extension);
                // Remove duplicates from the collection based on the CptCode property
                var data = ClientFeeScheduleEntryCollection.DistinctBy(x => x.CptCode);
                if (data != null)
                {
                    var clientCptCodeData = await _unitOfWork.Repository<ClientCptCode>().Entities.Specify(new GenericByClientIdSpecification<ClientCptCode>(command.ClientId)).ToListAsync();

                    var specification = new ClientFeeScheduleEntryByClientFeeScheduleIdSpecification(command.ClientFeeScheduleId);

                    // Now, you can use this specification to filter entities based on the clientFeeScheduleId.
                    var clientFeeScheduleEntryData = await _unitOfWork.Repository<ClientFeeScheduleEntry>().Entities
                                                        .Specify(specification)
                                                        .ToListAsync();


                    var processingTasks = new List<Task>();

                    processingTasks.Add(Task.Run(async () =>
                    {
                        foreach (var item in data)
                        {
                            try
                            {
                                if (!clientCptCodeData.Any(data => data.Code == item.CptCode))
                                {
                                    // if (string.IsNullOrWhiteSpace(item.Description))
                                    {
                                        // look up in CptCode reference table
                                    }
                                    var newClientCptCode = new ClientCptCode
                                    {
                                        Code = item.CptCode,
                                        Description = item.Description,
                                        LookupName = item.Description,
                                        ShortDescription = item.Description,
                                        ScheduledFee = item.Fee,
                                        ClientId = (int)command.ClientId
                                    };

                                    await _unitOfWork.Repository<ClientCptCode>().AddAsync(newClientCptCode);
                                    await _unitOfWork.Commit(cancellationToken);

                                    // Create a new fee schedule entry with the new client CPT code
                                    var clientFeeScheduleEntry = new ClientFeeScheduleEntry()
                                    {
                                        Fee = item.Fee ?? 0.0m,
                                        AllowedAmount = item.AllowedAmount ?? 0.0m,
                                        IsReimbursable = item.AllowedAmount != null && item.AllowedAmount != 0 ? true : false,
                                        ClientCptCodeId = newClientCptCode.Id,
                                        ClientFeeScheduleId = command.ClientFeeScheduleId,
                                        ClientId = (int)command.ClientId
                                    };
                                    await _unitOfWork.Repository<ClientFeeScheduleEntry>().AddAsync(clientFeeScheduleEntry);
                                    await _unitOfWork.Commit(cancellationToken);
                                }
                                else
                                {
                                    //Get matchedCptCode
                                    var matchedCptCode = clientCptCodeData.FirstOrDefault(data => data.Code == item.CptCode);


                                    if (clientFeeScheduleEntryData != null)
                                    {
                                        var matchingEntry = clientFeeScheduleEntryData.FirstOrDefault(entry => entry.ClientCptCodeId == matchedCptCode.Id && entry.ClientFeeScheduleId == command.ClientFeeScheduleId);

                                        if (matchingEntry != null)
                                        {
                                            // Update an existing fee schedule entry
                                            var clientFeeScheduleEntry = new ClientFeeScheduleEntry()
                                            {
                                                Fee = item.Fee ?? 0.0m,
                                                AllowedAmount = item.AllowedAmount ?? 0.0m,
                                                ClientCptCodeId = matchedCptCode.Id,
                                                ClientFeeScheduleId = command.ClientFeeScheduleId,
                                                ClientId = (int)command.ClientId,
                                                IsReimbursable = item.AllowedAmount != null && item.AllowedAmount != 0 ? true : false,
                                                Id = matchingEntry.Id,
                                            };

                                            await _unitOfWork.Repository<ClientFeeScheduleEntry>().UpdateAsync(clientFeeScheduleEntry);
                                        }
                                        else
                                        {
                                            var clientFeeScheduleEntry = new ClientFeeScheduleEntry()
                                            {
                                                Fee = item.Fee ?? 0.0m,
                                                AllowedAmount = item.AllowedAmount ?? 0.0m,
                                                IsReimbursable = item.AllowedAmount != null && item.AllowedAmount != 0 ? true : false,
												ClientCptCodeId = matchedCptCode.Id,
                                                ClientFeeScheduleId = command.ClientFeeScheduleId,
                                                ClientId = (int)command.ClientId
                                            };
                                            await _unitOfWork.Repository<ClientFeeScheduleEntry>().AddAsync(clientFeeScheduleEntry);
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                var clientFeeScheduleData = _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().GetByIdAsync(command.ClientFeeScheduleId);
                                if (clientFeeScheduleData != null)
                                {
                                    var clientFeeSchedule = new Domain.Entities.ClientFeeSchedule
                                    {
                                        Id = command.ClientFeeScheduleId,
                                        StartDate = clientFeeScheduleData.Result.StartDate,
                                        EndDate = clientFeeScheduleData.Result.EndDate,
                                        ImportStatus = Domain.Entities.Enums.ImportStatusEnum.Error,
                                        Name = clientFeeScheduleData.Result.Name,
                                        ClientId = clientFeeScheduleData.Result.ClientId
                                    };

                                    await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().UpdateAsync(clientFeeSchedule);
                                    await _unitOfWork.Commit(cancellationToken);
                                }
                                return;
                            }
                        }
                    }));
                    await Task.WhenAll(processingTasks);
                    await _unitOfWork.Commit(cancellationToken);

                    var clientFeeScheduleData = await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().GetByIdAsync(command.ClientFeeScheduleId);
                    if (clientFeeScheduleData != null)
                    {
                        var clientFeeSchedule = new Domain.Entities.ClientFeeSchedule
                        {
                            Id = command.ClientFeeScheduleId,
                            StartDate = clientFeeScheduleData.StartDate,
                            EndDate = clientFeeScheduleData.EndDate,
                            ImportStatus = Domain.Entities.Enums.ImportStatusEnum.Completed,
                            Name = clientFeeScheduleData.Name,
                            ClientId = clientFeeScheduleData.ClientId
                        };
                        await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().UpdateAsync(clientFeeSchedule);
                        await _unitOfWork.Commit(cancellationToken);
                    }

                }

                BackgroundJob.Enqueue(() => _manuallyRunJobService.ProcessFeeScheduleEntriesForSingleTenant(_tenantInfo.Identifier, command.ClientFeeScheduleId)); //EN-232
            }

            return await Result<int>.SuccessAsync(command.ClientFeeScheduleId, _localizer["Client Fee Schedule Entry Saved"]);
        }
    }
}
