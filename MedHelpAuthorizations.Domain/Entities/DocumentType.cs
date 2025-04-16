using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;


namespace MedHelpAuthorizations.Domain.Entities
{
    public class DocumentType : AuditableEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDefault { get; set; } = false;

        public virtual ICollection<Client> Clients { get; set; }

    }
}
