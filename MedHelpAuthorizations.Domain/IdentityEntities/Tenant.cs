using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.IdentityEntities
{
    public class Tenant : AuditableEntity<int>
    {
        public string Identifier { get; set; }
        public string TenantName { get; set; }
        public string DatabaseName { get; set; }
        public string AdminEmail { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidUpto { get; set; }
        public string Issuer { get; set; }

        [ForeignKey("ServerId")]
        public Server DatabaseServer { get; set; }

        public bool IsProductionTenant { get; set; } = true; //EN-545

        public ICollection<TenantUser> TenantUsers { get; set; }

    }
}
