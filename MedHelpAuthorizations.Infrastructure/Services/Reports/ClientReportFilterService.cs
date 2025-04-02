using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Specifications;

namespace MedHelpAuthorizations.Infrastructure.Services.Reports
{
    public class ClientReportFilterService : IClientReportFilterService
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IStringLocalizer<ClientReportFilterService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private IUnitOfWork<int> _unitOfWork;
        private readonly IUserService _userService;

        private readonly ISystemDefaultReportFilterRepository _systemDefaultReportFilterRepository;
        private int _clientId => _currentUserService.ClientId;
        private string _userId => _currentUserService.UserId;

        public ClientReportFilterService(IMapper mapper, IStringLocalizer<ClientReportFilterService> localizer, ICurrentUserService currentUserService, IUnitOfWork<int> unitOfWork, ITenantRepositoryFactory tenantRepositoryFactory, ISystemDefaultReportFilterRepository systemDefaultReportRepository, IUserService userService)
        {
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _systemDefaultReportFilterRepository = systemDefaultReportRepository;
        }

        public async Task<List<GetClientReportFilterResponse>> GetClientReportFilterDetailsByClientId(int clientId)
        {
            return await _unitOfWork.Repository<ClientUserReportFilter>().Entities
                   .Include(x => x.Report)
                   .Where(x => x.ClientId == clientId && x.UserId == _userId)
                   .OrderBy(r => r.FilterName)
                   .Select(z => new GetClientReportFilterResponse
                   {
                       Id = z.Id,
                       ClientId = z.ClientId,
                       ReportId = z.ReportId,
                       Report = _mapper.Map<GetAllReportsResponse>(z.Report),
                       FilterConfiguration = z.FilterConfiguration,
                       FilterName = z.FilterName,
                       HasDefaultFilter = z.HasDefaultFilter,
                       RunSavedDefaultFilter = z.RunSavedDefaultFilter
                   })
                   .ToListAsync();
        }
        public async Task<List<GetClientReportFilterResponse>> GetClientReportFiltersByReportId(int reportId)
        {
            return await _unitOfWork.Repository<ClientUserReportFilter>().Entities
                   .Include(x => x.Report)
                   .Where(x => x.ClientId == _clientId && x.UserId == _userId && x.ReportId == (ReportsEnum)reportId)
                   .OrderBy(r => r.FilterName)
                   .Select(z => new GetClientReportFilterResponse
                   {
                       Id = z.Id,
                       ClientId = z.ClientId,
                       ReportId = z.ReportId,
                       Report = _mapper.Map<GetAllReportsResponse>(z.Report),
                       FilterConfiguration = z.FilterConfiguration,
                       FilterName = z.FilterName,
                       HasDefaultFilter = z.HasDefaultFilter,
                       RunSavedDefaultFilter = z.RunSavedDefaultFilter
                   })
                   .ToListAsync();
        }
        /// <summary>
        /// Only used for custom report For getting Shared Reports as well Ids
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<ClientCustomReportFilterDetails> GetCustomReportClientReportFiltersByReportId(int reportId)
        {
            try
            {

                var result = await _unitOfWork.Repository<ClientUserReportFilter>().Entities
                       .Include(x => x.Report)
                       .Specify(new GenericByClientIdSpecification<ClientUserReportFilter>(_clientId))
                       .Specify(new ClientUserReportFiltersByUserIdSpecification(_userId))
                       .Where(x => x.ReportId == (ReportsEnum)reportId)
                       .OrderBy(r => r.FilterName)
                       .Select(clientUserReportFilter => new GetClientCustomReportFilterResponse()
                       {
                           Id = clientUserReportFilter.Id,
                           ClientId = clientUserReportFilter.ClientId,
                           ReportId = clientUserReportFilter.ReportId,
                           Report = _mapper.Map<GetAllReportsResponse>(clientUserReportFilter.Report),
                           FilterConfiguration = clientUserReportFilter.FilterConfiguration,
                           FilterName = clientUserReportFilter.FilterName,
                           HasDefaultFilter = clientUserReportFilter.HasDefaultFilter,
                           RunSavedDefaultFilter = clientUserReportFilter.RunSavedDefaultFilter,
                           EmployeeClientReportFilter = (clientUserReportFilter.EmployeeClientUserReportFilters != null) ? clientUserReportFilter.EmployeeClientUserReportFilters.Where(x => x.ClientUserReportFilterId == clientUserReportFilter.Id).Select(z => _mapper.Map<EmployeeClientReportFilterDTO>(z)).ToList() : null,
                           CreatedByUserId = (clientUserReportFilter.EmployeeClientUserReportFilters != null && clientUserReportFilter.EmployeeClientUserReportFilters.Any(x => x.ClientUserReportFilterId == clientUserReportFilter.Id && x.EmployeeClient.Employee.UserId == _userId)) ? _userId : clientUserReportFilter.UserId,

                       })
                       .ToListAsync();

                string[] userIds = result.Select(clientUserReportFilter => clientUserReportFilter.CreatedByUserId).Distinct().ToArray();
                string[] employeeClientReportFilterIds = result.SelectMany(s => s.EmployeeClientReportFilter.Select(z => z.EmployeeClient.Employee.UserId)).Distinct().ToArray();

                if (employeeClientReportFilterIds is not null && employeeClientReportFilterIds.Any())
                {
                    userIds = userIds.Union(employeeClientReportFilterIds).ToArray();
                }
                Dictionary<string, string> userNamesDict = await _userService.GetNamesAsync(userIds).ConfigureAwait(false);

                ClientCustomReportFilterDetails clientCustomReportDetails = new ClientCustomReportFilterDetails()
                {
                    CustomReportFilterResponses = result,
                    UserNamesDict = userNamesDict
                };
                ///Update user full name.

                return clientCustomReportDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<List<GetClientReportFilterResponse>> GetClientReportFiltersByReportIdAndFilterName(int reportId, string filterName)
        {
            return await _unitOfWork.Repository<ClientUserReportFilter>().Entities
                   .Include(x => x.Report)
                   .Where(x => x.ClientId == _clientId && x.UserId == _userId && x.ReportId == (ReportsEnum)reportId && x.FilterName == filterName)
                   .OrderBy(r => r.FilterName)
                   .Select(z => new GetClientReportFilterResponse
                   {
                       Id = z.Id,
                       ClientId = z.ClientId,
                       ReportId = z.ReportId,
                       Report = _mapper.Map<GetAllReportsResponse>(z.Report),
                       FilterConfiguration = z.FilterConfiguration,
                       FilterName = z.FilterName,
                       HasDefaultFilter = z.HasDefaultFilter,
                       RunSavedDefaultFilter = z.RunSavedDefaultFilter
                   })
                   .ToListAsync();
        }
        public async Task<ClientUserReportFilter> GetClientReportFilters(int reportFilterId, int clientId, ReportsEnum reportId)
        {
            return await _unitOfWork.Repository<ClientUserReportFilter>().Entities
                   .Include(x => x.Report)
                   .FirstOrDefaultAsync(x => x.Id == reportFilterId && x.UserId == _userId && x.ClientId == clientId && x.ReportId == (ReportsEnum)reportId);
        }

        public async Task<bool> AddSystemDefaultReportFiltersForEmployeeClient(int employeeClientId, string tenantIdentifier = null)
        {
            ///EN-109
            // Get unitof work for tenantidentifier if identifier is not null or Whitespace. 
            if (!string.IsNullOrWhiteSpace(tenantIdentifier))
            {
                _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);
            }
            try
            {

                //get employeeClient and their ClientEmployeeRoles
                EmployeeClient employeeClientWithRoles = await _unitOfWork.Repository<EmployeeClient>().Entities
                                                        .Include(x => x.Employee)
                                                        .Include(x => x.AssignedClientEmployeeRoles)
                                                        .FirstOrDefaultAsync(z => z.Id == employeeClientId);

                //select all the SystemDefaultReportFilter that reference the same employee roles that is EmployeeClient.AssignedClientEmployeeRoles is assigned to.
                List<SystemDefaultReportFilter> systemDefaultReportFilters = await _unitOfWork.Repository<SystemDefaultReportFilter>().Entities
                                                                            .Include(s => s.SystemDefaultReportFilterEmployeeRoles)
                                                                            .Where(x => x.SystemDefaultReportFilterEmployeeRoles
                                                                            .Any(y => employeeClientWithRoles.AssignedClientEmployeeRoles
                                                                            .Select(z => z.EmployeeRoleId).Contains(y.EmployeeRoleId)))
                                                                            .ToListAsync();

                //Add a method in clientReportFilterService, that creates clientReportFilters for employeeClient.
                if (systemDefaultReportFilters.Any())
                {
                    // Create or update ClientUserReports for this EmployeeClient based-off of the systemDefaultReportFilters definitions that reference the EmployteeRoles that  employteeClient has. 
                    string userId = employeeClientWithRoles.Employee.UserId;
                    foreach (var systemDefaultReportFilter in systemDefaultReportFilters)
                    {
                        ///Get clientReportFilter detail by ReportEnum, SystemDefaultReportFilterId
                        var clientReportFilter = await _unitOfWork.Repository<ClientUserReportFilter>().Entities
                                                                .FirstOrDefaultAsync(z => z.ReportId == systemDefaultReportFilter.ReportId
                                                                    && z.SystemDefaultReportFilterId.HasValue
                                                                    && z.ClientId == employeeClientWithRoles.ClientId && z.UserId == userId
                                                                    && z.SystemDefaultReportFilterId.Value == systemDefaultReportFilter.Id) ?? null;
                        // Update report definition from DefaultSystemReportFilter  if the employeeCLient already has a copy.
                        if (clientReportFilter == null)
                        {
                            ClientUserReportFilter report = new()
                            {
                                SystemDefaultReportFilterId = systemDefaultReportFilter.Id,
                                IsDeleted = false,
                                ClientId = employeeClientWithRoles.ClientId,
                                FilterName = systemDefaultReportFilter.FilterName,
                                FilterConfiguration = systemDefaultReportFilter.FilterConfiguration,
                                ReportId = systemDefaultReportFilter.ReportId,
                                UserId = userId,///EmployeeClient userId
                            };

                            await _unitOfWork.Repository<ClientUserReportFilter>().AddAsync(report);
                            await _unitOfWork.Commit(new CancellationToken());
                        }
                        else
                        {
                            clientReportFilter.FilterConfiguration = systemDefaultReportFilter.FilterConfiguration;
                            await _unitOfWork.Repository<ClientUserReportFilter>().UpdateAsync(clientReportFilter);
                            await _unitOfWork.Commit(new CancellationToken());
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }

        }

        /// <summary>
        /// Soft Delete Saved ClientUserReport By Id.
        /// </summary>
        /// <param name="clientUserReportId"> ClientUSerReport Id</param>
        /// <param name="tenantIdentifier">TenantName <see cref="| If Any"/></param>
        /// <returns>Success: ClientUserReportId and Message | Fail:Error Message</returns>
        public async Task<Result<int>> DeleteById(int clientUserReportId, string tenantIdentifier = null)
        {
            /// EN-112.
            // Get unitof work for tenantidentifier if identifier is not null or Whitespace. 
            if (!string.IsNullOrWhiteSpace(tenantIdentifier))
            {
                _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);
            }
            ///Get ClientUserReport from tenant db.
            ClientUserReportFilter clientReportFilter = await _unitOfWork.Repository<ClientUserReportFilter>().Entities.FirstOrDefaultAsync(z => z.Id == clientUserReportId && z.UserId == _userId && z.ClientId == _clientId);
            ///If client user reprt not found return.
            if (clientReportFilter == null)
            {
                return await Result<int>.FailAsync(_localizer[$"Client Report {clientUserReportId} Not Found!"]);
            }
            ///If Client user report already deleted then return.
            if (clientReportFilter.IsDeleted)
            {
                return await Result<int>.FailAsync(_localizer[$"Client Report {clientUserReportId} already Deleted!"]);
            }
            try
            {
                ///ToDo: Need to handle If Report has systemDefaultReportId.

                ///Soft Delete Client user report filter.
                clientReportFilter.IsDeleted = true;
                await _unitOfWork.Repository<ClientUserReportFilter>().UpdateAsync(clientReportFilter);
                await _unitOfWork.Commit(new CancellationToken());
                return await Result<int>.SuccessAsync(clientUserReportId, _localizer[$"Client Report {clientUserReportId} Not Found!"]);
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(_localizer[ex.Message]);
            }
        }
    }
}
