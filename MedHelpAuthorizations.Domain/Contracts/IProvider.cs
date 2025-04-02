using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Contracts
{
    public interface IProvider : IPersonLinkedEntity
    {
        SpecialtyEnum SpecialtyId { get; set; }

        [StringLength(20)]
        string Credentials { get; set; }


        [StringLength(10)]
        string Npi { get; set; }


        [StringLength(6)]
        string Upin { get; set; }


        [StringLength(9)]
        string TaxId { get; set; }


        [StringLength(10)]
        string TaxonomyCode { get; set; }


        [StringLength(25)]
        string License { get; set; }


        [StringLength(25)]
        string ExternalId { get; set; }
    }
}
