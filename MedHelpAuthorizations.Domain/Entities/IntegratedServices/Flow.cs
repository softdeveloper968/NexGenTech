using System.ComponentModel.DataAnnotations;
using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class Flow : AuditableEntity<int>
    {
        #region Navigational Property Init
        #endregion

        public Flow(string flowName)
        {
            FlowName = flowName;
        }
        
        [Required]
        public string FlowName { get; set; }
    }
}
