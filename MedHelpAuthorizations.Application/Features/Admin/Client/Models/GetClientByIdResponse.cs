using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Base;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Admin.Client.Models
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
        public int[] ClientAuthTypes { get; set; } = Array.Empty<int>();
        public int[] ClientApplicationFeatures { get; set; } = Array.Empty<int>();
        public List<EmployeeClientViewModel> EmployeeClients { get; set; } = new List<EmployeeClientViewModel>();
        public List<ClientInsuranceDto> ClientInsurances { get; set; } = new List<ClientInsuranceDto>();
        public List<ClientLocationDto> ClientLocations { get; set; } = new List<ClientLocationDto>();
        public List<ClientApiIntegrationKeyDto> ClientApiIntegrationKeys { get; set; } = new List<ClientApiIntegrationKeyDto>();
        public SourceSystemEnum? SourceSystemId { get; set; }
		public SpecialtyEnum[] SpecialityIds { get; set; } = Array.Empty<SpecialtyEnum>();
        public int? TaxId { get; set; }
        public bool IsActive { get; set; }
    }
}
