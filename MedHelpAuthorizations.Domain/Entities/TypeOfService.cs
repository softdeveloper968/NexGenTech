using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    ///  Types of Service  
    /// </summary>
    public class TypeOfService : AuditableEntity<TypeOfServiceEnum>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
