
namespace MedHelpAuthorizations.Application.Features.IntegratedServices.FlowLogEntries.Queries.Base
{
    public class GetFlowLogEntryQueryBaseResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int FlowId { get; set; }

        public string StepName { get; set; }

        public bool IsSuccessful { get; set; } = false;

        public string Message { get; set; }
    }
}
