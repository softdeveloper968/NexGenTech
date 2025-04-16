using MedHelpAuthorizations.Application.Features.Administration.Client_ApplicationFeatures;
using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Base;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetById
{
    public class GetClientByIdResponse
    {
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClientCode { get; set; }
        public long? PhoneNumber { get; set; }
        public long? FaxNumber { get; set; }
        public int? ClientKpiId { get; set; }
        public ClientKpiDto ClientKpi { get; set; }
        public List<ClientAuthTypeDto> ClientAuthTypes { get; set; }
        public List<ClientApplicationFeatureDto> ClientApplicationFeatures { get; set; }
        public List<EmployeeClientViewModel> EmployeeClients { get; set; }
        public List<ClientInsuranceDto> ClientInsurances { get; set; }
        public List<ClientLocationDto> ClientLocations { get; set; }
        public List<ClientApiIntegrationKeyDto> ClientApiIntegrationKeys { get; set; }
        public SourceSystemEnum? SourceSystemId { get; set; }
        public List<ClientSpecialityDto> ClientSpecialties { get; set; }
        public List<ClientHoliday> ClientHolidays { get; set; } = new();
        public List<ClientDayOfOperation> ClientDaysOfOperation { get; set; } = new();
        public int? TaxId { get; set; }
        public int? NpiNumber { get; set; }
        public int? AddressId { get; set; }
        public int? ClientQuestionnaireId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? InitialAnalysisEndOn { get; set; }
        public int? AutoLogMinutes { get; set; }
    }
}