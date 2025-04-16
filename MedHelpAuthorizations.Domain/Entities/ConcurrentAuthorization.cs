using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

#nullable disable

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ConcurrentAuthorization : AuditableEntity<int>//, ITenant
    {
        public ConcurrentAuthorization()
        {
        }

        [ForeignKey(nameof(InitialAuthorization)), Column(Order = 0)]
        public int InitialAuthorizationId { get; set; }

        [ForeignKey(nameof(SucceededAuthorization)), Column(Order = 1)]
        public int SucceededAuthorizationId { get; set; }

        //[ForeignKey("InitialAuthorizationId")]
        public Authorization InitialAuthorization { get; set; }

        //[ForeignKey("SucceededAuthorizationId")]
        public Authorization SucceededAuthorization { get; set; }
       //public string TenantId { get; set; }

    }
}
