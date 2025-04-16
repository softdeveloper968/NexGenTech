using AutoMapper;
using MedHelpAuthorizations.Application.Features.Admin.Client.Models;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Base;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.Client.Queries
{
	public class GetClientByIdQuery : IRequest<Result<GetClientByIdResponse>>
    {
        public int TenantId { get; set; }
        public int Id { get; set; }
        public GetClientByIdQuery(int tenantId, int clientId)
        {
            TenantId = tenantId;
            Id = clientId;
        }
    }

    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Result<GetClientByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly ITenantManagementService _tenantManagementService;

        public GetClientByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ITenantRepositoryFactory tenantRepositoryFactory, ITenantManagementService tenantManagementService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _tenantManagementService = tenantManagementService;
        }

        public async Task<Result<GetClientByIdResponse>> Handle(GetClientByIdQuery query, CancellationToken cancellationToken)
        {
            var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(query.TenantId);
            try
            {
                Expression<Func<Domain.Entities.Client, GetClientByIdResponse>> expression = e => new GetClientByIdResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    ClientCode = e.ClientCode,
                    PhoneNumber = e.PhoneNumber,
                    FaxNumber = e.FaxNumber,
                    ClientAuthTypes = e.ClientAuthTypes.Select(x => x.AuthTypeId).ToArray(),
                    ClientApplicationFeatures = e.ClientApplicationFeatures.Select(x => (int)x.ApplicationFeatureId).ToArray(),
                    ClientApiIntegrationKeys = _mapper.Map<List<ClientApiIntegrationKeyDto>>(e.ClientApiIntegrationKeys.ToList()),
                    EmployeeClients = _mapper.Map<List<EmployeeClientViewModel>>(e.EmployeeClients.ToList()),
                    ClientInsurances = _mapper.Map<List<ClientInsuranceDto>>(e.ClientInsurances.ToList()),
                    ClientLocations = _mapper.Map<List<ClientLocationDto>>(e.ClientLocations.ToList()),
                    ClientKpiId = e.ClientKpiId,
                    ClientKpi = _mapper.Map<ClientKpiDto>(e.ClientKpi),
                    SourceSystemId = e.SourceSystemId, //EN-64
					SpecialityIds = e.ClientSpecialties.Select(x => x.SpecialtyId).ToArray(),
                    TaxId = e.TaxId,
                    IsActive = e.IsActive,
				};

                var client = await unitOfWork.Repository<Domain.Entities.Client>()
                                    .Entities
                                    .Select(expression)
                                    .FirstOrDefaultAsync(x => x.Id == query.Id);

                var mappedClient = _mapper.Map<GetClientByIdResponse>(client);
                mappedClient.TenantId = query.TenantId;
                mappedClient.TenantName = await _tenantManagementService.GetTenantNameById(query.TenantId);

                return await Result<GetClientByIdResponse>.SuccessAsync(mappedClient);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
