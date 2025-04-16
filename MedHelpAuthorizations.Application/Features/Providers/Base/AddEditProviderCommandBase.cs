
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Providers.Commands.AddEdit.Base
{
    public class AddEditProviderCommandBase 
    {
        public int ProviderId { get; set; }
        public int PersonId { get; set; }
        public string FaxNumber { get; set; }
        public SpecialtyEnum? SpecialtyId { get; set; } = null;
		public ProviderLevelEnum? ProviderLevelId { get; set; } = null;
		public string License { get; set; }
        public string Npi { get; set; }
        public string TaxId { get; set; }
        public string Upin { get; set; }
        public string TaxonomyCode { get; set; }
        public string Credentials { get; set; }
        public int? SupervisingProviderId { get; set; }
    }
}
