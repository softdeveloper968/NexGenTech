using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class DbOperation : AuditableEntity<DbOperationEnum>
    {
        public DbOperation() { }
        public DbOperation(DbOperationEnum id, string name) 
        {
            Id = id;
            Name = name;
        }

        [StringLength(6)]
        public string Name { get; set; }
    }
}
