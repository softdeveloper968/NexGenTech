using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClientAlphaSplits
{
    public class AlphaSplitDto 
    {
        AlphaSplitEnum Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string BeginAlpha { get; set; }

        public string EndAlpha { get; set; }
    }
}
