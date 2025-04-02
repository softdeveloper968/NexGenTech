using System.ComponentModel.DataAnnotations;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// Each State of the USA
    /// </summary>
    /// 
    public class State : AuditableEntity<StateEnum>
    {
        public State() { }
        public State(StateEnum id, string code, string name) 
        {
            Id = id;
            Code = code;
            Name = name;
        }

        [StringLength(16)]
        public string Name { get; set; }

        [StringLength(3)]
        public string Code { get; set; }
    }
}
