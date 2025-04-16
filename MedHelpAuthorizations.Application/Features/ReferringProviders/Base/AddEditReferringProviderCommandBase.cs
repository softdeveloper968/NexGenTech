using AutoMapper;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.ReferringProviders.Commands.AddEdit.Base
{
    public class AddEditReferringProviderCommandBase
    {
        public int ProviderId { get; set; }
        public int PersonId { get; set; }
        public string FaxNumber { get; set; }
        public SpecialtyEnum? SpecialtyId { get; set; } = null;
        public string License { get; set; }
        public string Npi { get; set; }
        public string TaxId { get; set; }
        public string Upin { get; set; }
        public string TaxonomyCode { get; set; }
        public string Credentials { get; set; }
    }
}
