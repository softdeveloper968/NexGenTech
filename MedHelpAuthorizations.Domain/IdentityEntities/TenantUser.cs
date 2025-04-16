using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.IdentityEntities
{
    public class TenantUser : AuditableEntity<int>
    {
        public int TenantId { get; set; }
        public string UserId { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsActive { get; set; }
        public TenantUser() { }
        public TenantUser(int tenantId, string userId)
        {
            TenantId = tenantId;
            UserId = userId;
        }
        [ForeignKey(nameof(TenantId))]
        public Tenant Tenant { get; set; }
    }
}
