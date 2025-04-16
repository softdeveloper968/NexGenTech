using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class FlowLogEntry : AuditableEntity<int>
    {
        #region Navigational Property Init
        #endregion

        public FlowLogEntry(int clientId, int flowId, string stepName, bool isSuccessful, string message)
        {
            ClientId = clientId;
            FlowId = flowId;
            StepName = stepName;
            IsSuccessful = isSuccessful;
            Message = message;
        }

        public FlowLogEntry(int clientId, int flowId, string stepName, bool isSuccessful)
        {
            ClientId = clientId;
            FlowId = flowId;
            StepName = stepName;
            IsSuccessful = isSuccessful;
            Message = string.Empty;
        }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int FlowId { get; set; }

        [Required]
        public string StepName { get; set; }

        [Required]
        public bool IsSuccessful { get; set; } = false;

        public string Message { get; set; }


        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("FlowId")]
        public virtual Flow Flow { get; set; }

    }
}
