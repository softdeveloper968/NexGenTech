using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums; 

namespace MedHelpAuthorizations.Domain.Entities
{
    public class Address : AuditableEntity<int>, IClientRelationship//, ITenant
    {

        [Required]
        public AddressTypeEnum AddressTypeId { get; set; } = AddressTypeEnum.Residential;

        public string AddressStreetLine1 { get; set; }
        public string AddressStreetLine2 { get; set; }

        public string City { get; set; }
        public StateEnum StateId { get; set; } = StateEnum.UNK;
        public string PostalCode { get; set; }
        public bool Normalized { get; set; } = false;
        public int DeliveryPointBarcode { get; set; } = 0;
        public int ClientId { get; set; } = 1;
        //public string TenantId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        
        #region Foreign Key relationships
        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        #endregion
    }
}
