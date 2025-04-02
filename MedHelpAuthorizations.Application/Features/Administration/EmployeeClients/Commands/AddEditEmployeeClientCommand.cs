using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientAlphaSplits;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientInsurances;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientLocations;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeRoles;
using MedHelpAuthorizations.Application.Features.Administration.Employees;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Commands
{
    public class AddEditEmployeeClientCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int? EmployeeId { get; set; }
        public EmployeeDto Employee { get; set; }
        public int AssignedAverageDailyClaimCount { get; set; }
        public decimal ExpectedMonthlyCashCollections { get; set; }
        public int OverallAverageDailyClaimCount { get; set; }
        public List<ClientEmployeeRoleDto> AssignedClientEmployeeRoles { get; set; }
        public List<EmployeeClientAlphaSplitDto> EmployeeClientAlphaSplits { get; set; }
        public List<EmployeeClientLocationDto> EmployeeClientLocations { get; set; }
        public List<EmployeeClientInsuranceDto> EmployeeClientInsurances { get; set; }
        public bool ReceiveReport { get; set; }
    }

    public class AddEditEmployeeClientCommandHandler : IRequestHandler<AddEditEmployeeClientCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditClientCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IClientReportFilterService _clientReportFilterService;
        private int _clientId => _currentUserService.ClientId;


        public AddEditEmployeeClientCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditClientCommandHandler> localizer, ICurrentUserService currentUserService, IClientReportFilterService clientReportFilterService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _clientReportFilterService = clientReportFilterService;
        }

        public async Task<Result<int>> Handle(AddEditEmployeeClientCommand command, CancellationToken cancellationToken)
        {
            EmployeeClient dbEmployeeClient = null;

            try
            {
                if (command.Id == 0)
                {
                    dbEmployeeClient = _mapper.Map<EmployeeClient>(command);
                    dbEmployeeClient = await _unitOfWork.Repository<EmployeeClient>().AddAsync(dbEmployeeClient);

                    await _unitOfWork.Commit(cancellationToken);
                    ///When create a new emplyee client.Add any customreports that are relevant to their Role.
                    if (dbEmployeeClient != null && dbEmployeeClient.Id > 0)///EN-112
                    {
                        bool hasSystemDefaultReport = await _clientReportFilterService.AddSystemDefaultReportFiltersForEmployeeClient(employeeClientId: dbEmployeeClient.Id);
                    }
                    return await Result<int>.SuccessAsync(dbEmployeeClient.Id, _localizer["Employee Client Saved"]);
                }
                else
                {
                    dbEmployeeClient = await _unitOfWork.Repository<EmployeeClient>().Entities
                        .Include(x => x.Employee)
                        .Include("EmployeeClientInsurances.ClientInsurance")
                        .Include("EmployeeClientLocations.ClientLocation")
                        .Include("AssignedClientEmployeeRoles.EmployeeRole")
                        .Include("EmployeeClientAlphaSplits")
                        .FirstOrDefaultAsync(x => x.Id == command.Id);

                    if (dbEmployeeClient != null)
                    {
                        
                            var customAlphaSplit = command.EmployeeClientAlphaSplits.FirstOrDefault(x => x.AlphaSplitId == AlphaSplitEnum.CustomRange);

                            //If not one of the brand new employee clients we just added in previous AddRange
                            if (dbEmployeeClient.Id != 0)
                            {
                                //Assign Claim count to Employee Client
                                dbEmployeeClient.AssignedAverageDailyClaimCount = command.AssignedAverageDailyClaimCount;
                                dbEmployeeClient.ReceiveReport = command.ReceiveReport;
                                var employeeRolesToRemove = dbEmployeeClient.AssignedClientEmployeeRoles?.Where(item => !command.AssignedClientEmployeeRoles.Any(item2 => item2.EmployeeRoleId == item.EmployeeRoleId));
                                var alphaSplitsToRemove = dbEmployeeClient.EmployeeClientAlphaSplits?.Where(item => !command.EmployeeClientAlphaSplits.Any(item2 => item2.AlphaSplitId == item.AlphaSplitId)); //&& (item.AlphaSplitId == Enums.AlphaSplitEnum.CustomRange && (item.CustomBeginAlpha != item2.CustomBeginAlpha || item.CustomEndAlpha != item2.CustomEndAlpha)));
                                var empClientInsurancesToRemove = dbEmployeeClient.EmployeeClientInsurances?.Where(item => !command.EmployeeClientInsurances.Any(item2 => item2.ClientInsuranceId == item.ClientInsuranceId));
                                var empClientLocationsToRemove = dbEmployeeClient.EmployeeClientLocations?.Where(item => !command.EmployeeClientLocations.Any(item2 => item2.ClientLocationId == item.ClientLocationId));

                                var employeeRolesToAdd = command.AssignedClientEmployeeRoles?.Where(item => !dbEmployeeClient.AssignedClientEmployeeRoles.Any(item2 => item2.EmployeeRoleId == item.EmployeeRoleId));
                                var alphaSplitsToAdd = command.EmployeeClientAlphaSplits?.Where(item => !dbEmployeeClient.EmployeeClientAlphaSplits.Any(item2 => item2.AlphaSplitId == item.AlphaSplitId));
                                var empClientInsurancesToAdd = command.EmployeeClientInsurances?.Where(item => !dbEmployeeClient.EmployeeClientInsurances.Any(item2 => item2.ClientInsuranceId == item.ClientInsuranceId));
                                var empClientLocationsToAdd = command.EmployeeClientLocations?.Where(item => !dbEmployeeClient.EmployeeClientLocations.Any(item2 => item2.ClientLocationId == item.ClientLocationId));

                                //empClientInsurancesToAdd.ToList().ForEach(eci => eci.ClientInsurance = null);
                                //empClientLocationsToAdd.ToList().ForEach(eci => eci.ClientLocation = null);

                                _unitOfWork.Repository<ClientEmployeeRole>().RemoveRange(employeeRolesToRemove);
                                _unitOfWork.Repository<EmployeeClientAlphaSplit>().RemoveRange(alphaSplitsToRemove);
                                _unitOfWork.Repository<EmployeeClientInsurance>().RemoveRange(empClientInsurancesToRemove);
                                _unitOfWork.Repository<EmployeeClientLocation>().RemoveRange(empClientLocationsToRemove);

                                _unitOfWork.Repository<ClientEmployeeRole>().AddRange(_mapper.Map<List<ClientEmployeeRole>>(employeeRolesToAdd));
                                _unitOfWork.Repository<EmployeeClientAlphaSplit>().AddRange(_mapper.Map<List<EmployeeClientAlphaSplit>>(alphaSplitsToAdd));
                                _unitOfWork.Repository<EmployeeClientInsurance>().AddRange(_mapper.Map<List<EmployeeClientInsurance>>(empClientInsurancesToAdd));
                                _unitOfWork.Repository<EmployeeClientLocation>().AddRange(_mapper.Map<List<EmployeeClientLocation>>(empClientLocationsToAdd));

                                //Update any cutomAlphaSplit that is not new, but needs to be updated instead.
                                var existingCustomAlphaSplit = dbEmployeeClient.EmployeeClientAlphaSplits.FirstOrDefault(x => x.AlphaSplitId == AlphaSplitEnum.CustomRange);
                                if (existingCustomAlphaSplit != null && customAlphaSplit != null)
                                {
                                    existingCustomAlphaSplit.CustomBeginAlpha = customAlphaSplit?.CustomBeginAlpha;
                                    existingCustomAlphaSplit.CustomEndAlpha = customAlphaSplit?.CustomEndAlpha;
                                }
                            }
                        
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(dbEmployeeClient.Id, _localizer["Employee Client Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["Client Employee Not Found!"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
