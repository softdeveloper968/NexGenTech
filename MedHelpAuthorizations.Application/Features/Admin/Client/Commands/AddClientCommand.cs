using AutoMapper;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.Client.Commands
{
    public class AddEditAdminClientCommand : IRequest<Result<int>>
    {
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public long? PhoneNumber { get; set; }
        public long? FaxNumber { get; set; }
        public List<int> AuthTypes { get; set; }
        public List<int> Features { get; set; }
        public SourceSystemEnum? SourceSystemId { get; set; }
        public List<SpecialtyEnum> SpecialitIds { get; set; }
        public int? TaxId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class AddEditAdminClientCommandHandler : IRequestHandler<AddEditAdminClientCommand, Result<int>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AddEditAdminClientCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory, IMapper mapper)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditAdminClientCommand request, CancellationToken cancellationToken)
        {
            if (request.TenantId == 0)
            {
                throw new Exception("Invalid Tenant Id");
            }

            try
            {
                var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(request.TenantId);
                var clientEntity = unitOfWork.Repository<Domain.Entities.Client>();

                if (request.ClientId > 0)
                {
                    var client = await clientEntity.Entities
                                .Include(c => c.ClientApplicationFeatures)
                                .Include(c => c.ClientAuthTypes)
                                .Include(c => c.ClientInsurances)
                                .Include(c => c.ClientLocations)
                                .Include(c => c.ClientKpi)
                                .Include(c => c.ClientSpecialties) //EN-201
                        .Where(x => x.Id == request.ClientId)
                        .FirstOrDefaultAsync();

                    client.Name = request.ClientName;
                    client.ClientCode = request.ClientCode;
                    client.PhoneNumber = request.PhoneNumber;
                    client.FaxNumber = request.FaxNumber;
                    client.SourceSystemId = request.SourceSystemId;
                    client.TaxId = request.TaxId;
                    client.IsActive = request.IsActive;

                    var authTypesToRemove = client.ClientAuthTypes.Where(item => !request.AuthTypes.Any(item2 => item2 == item.AuthTypeId)).ToList();
                    var authTypesToAdd = request.AuthTypes
                                            .Where(item => !client.ClientAuthTypes.Any(item2 => item2.AuthTypeId == item))
                                            .Select(x => new Domain.Entities.ClientAuthType() { AuthTypeId = x, Client = client }).ToList();

                    unitOfWork.Repository<ClientAuthType>().RemoveRange(authTypesToRemove);
                    unitOfWork.Repository<ClientAuthType>().AddRange(authTypesToAdd);

                    //EN-201
                    if (client.ClientSpecialties.Any())
                    {
                        var clientSpecialityToRemove = client.ClientSpecialties.Where(item => !request.SpecialitIds.Any(item2 => item2 == item.SpecialtyId)).ToList();
                        unitOfWork.Repository<ClientSpecialty>().RemoveRange(clientSpecialityToRemove);
                    }

                    //EN-201
                    if (request.SpecialitIds != null && request.SpecialitIds.Any())
                    {
                        var clientSpecialityToAdd = request.SpecialitIds
                            .Where(item => !client.ClientSpecialties.Any(item2 => item2.SpecialtyId == item))
                            .Select(x => new ClientSpecialty() { SpecialtyId = (SpecialtyEnum)x, Client = client }).ToList();


                        unitOfWork.Repository<ClientSpecialty>().AddRange(clientSpecialityToAdd);
                    }

                    var appFeaturesToRemove = client.ClientApplicationFeatures.Where(item => !request.Features.Any(item2 => item2 == (int)item.ApplicationFeatureId)).ToList();
                    var appFeaturesToAdd = request.Features
                        .Where(item => !client.ClientApplicationFeatures.Any(item2 => (int)item2.ApplicationFeatureId == item))
                        .Select(x => new ClientApplicationFeature() { ApplicationFeatureId = (ApplicationFeatureEnum)x, Client = client }).ToList();

                    unitOfWork.Repository<ClientApplicationFeature>().RemoveRange(appFeaturesToRemove);
                    unitOfWork.Repository<ClientApplicationFeature>().AddRange(appFeaturesToAdd);

                    await clientEntity.UpdateAsync(client);

                    await unitOfWork.Commit(cancellationToken);

                    return Result<int>.Success(request.ClientId);
                }
                else
                {
                    Domain.Entities.Client clientData = new Domain.Entities.Client()
                    {
                        ClientCode = request.ClientCode,
                        PhoneNumber = request.PhoneNumber,
                        FaxNumber = request.FaxNumber,
                        Name = request.ClientName,
                        SourceSystemId = request.SourceSystemId, //EN-64
                        TaxId = request.TaxId,
                        IsActive = request.IsActive
                    };

                    clientData.ClientAuthTypes = request.AuthTypes != null
                                                      ? request.AuthTypes.Select(x => new Domain.Entities.ClientAuthType() { AuthTypeId = x }).ToList()
                                                      : new List<Domain.Entities.ClientAuthType>();

                    clientData.ClientApplicationFeatures = request.Features != null
                                                        ? request.Features.Select(x => new ClientApplicationFeature() { ApplicationFeatureId = (ApplicationFeatureEnum)x }).ToList()
                                                        : new List<ClientApplicationFeature>();

                    clientData.ClientSpecialties = request.SpecialitIds != null
                                                    ? request.SpecialitIds.Select(x => new ClientSpecialty() { SpecialtyId = (SpecialtyEnum)x }).ToList()
                                                    : new List<ClientSpecialty>();


                    await clientEntity.AddAsync(clientData);

                    await unitOfWork.Commit(cancellationToken);

                    return Result<int>.Success(clientData.Id);
                }

            }
            catch (Exception ex)
            {
                var mode = request.ClientId == 0 ? "create" : "update";
                return Result<int>.Fail($"Failed to {mode} client");
            }
        }
    }
}
