using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Admin.Client.Models;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.Client.Queries
{
    public class GetAllClientsWithTenants : IRequest<PaginatedResult<GetAllClientsWithTenantResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public int TenantId { get; set; }
        public bool IsActiveOnly { get; set; }
        public GetAllClientsWithTenants(int pageNumber, int pageSize, string searchString, int tenantId, bool isActiveOnly)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            TenantId = tenantId;
            IsActiveOnly = isActiveOnly;
        }
    }

    public class GetAllClientsWithTenantsHandler : IRequestHandler<GetAllClientsWithTenants, PaginatedResult<GetAllClientsWithTenantResponse>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly ITenantManagementService _tenantManagementService;
        private readonly IMapper _mapper;
        public GetAllClientsWithTenantsHandler(ITenantRepositoryFactory tenantRepositoryFactory, ITenantManagementService tenantManagementService, IMapper mapper)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _tenantManagementService = tenantManagementService;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<GetAllClientsWithTenantResponse>> Handle(GetAllClientsWithTenants request, CancellationToken cancellationToken)
        {
            try
            {
                var tenant = (await _tenantManagementService.GetAllAsync()).First(x => x.Id == request.TenantId);

                var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(request.TenantId);
                var clientInsuranceRepo = _tenantRepositoryFactory.GetClientInsuranceRpaConfigurationRepository(tenant.Identifier);

                Expression<Func<Domain.Entities.Client, GetAllClientsWithTenantResponse>> expression = e => new GetAllClientsWithTenantResponse
                {
                    TenantId = tenant.Id,
                    TenantName = tenant.TenantName,
                    ClientId = e.Id,
                    Name = e.Name,
                    ClientCode = e.ClientCode,
                    PhoneNumber = e.PhoneNumber,
                    FaxNumber = e.FaxNumber,
                    SourceSystemId = e.SourceSystemId, //EN-64
                    TaxId = e.TaxId, //EN-538,
                    IsActive = e.IsActive, //EN-657
                };

                // Define the specification for filtering clients
                var clientFilterSpec = new ClientFilterSpecification(request.SearchString);

                // Construct the query
                var query = unitOfWork.Repository<Domain.Entities.Client>().Entities
                                       .Specify(clientFilterSpec)
                                       .Select(expression)
                                       .Where(x => x.TenantId == request.TenantId && (
                                           string.IsNullOrEmpty(request.SearchString) ||
                                           (
                                               x.ClientCode.Contains(request.SearchString) ||
                                               x.Name.Contains(request.SearchString) ||
                                               x.PhoneNumber.ToString().Contains(request.SearchString) ||
                                               x.FaxNumber.ToString().Contains(request.SearchString)
                                           )));

                // Filter by active clients if required
                if (request.IsActiveOnly)
                {
                    query = query.Where(x => x.IsActive);
                }

                // Apply pagination directly in the query
                var paginatedData = await query.Skip((request.PageNumber - 1) * request.PageSize)
                                               .Take(request.PageSize)
                                               .ToListAsync(cancellationToken);

                // Calculate the total count for pagination metadata
                var totalCount = await query.CountAsync(cancellationToken);

                foreach (var client in paginatedData)
                {
                    int failedCount = await clientInsuranceRepo.GetFailedClientInsuranceRpaConfigurationsCountAsync();
                    client.TenantFailedConfigurationCount = (await clientInsuranceRepo.GetExpiryWarningOrFailedClientInsuranceRpaConfigByClientIdAsync(client.ClientId)).Count();
                }

                return PaginatedResult<GetAllClientsWithTenantResponse>.Success(paginatedData, totalCount, request.PageNumber, request.PageSize);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

