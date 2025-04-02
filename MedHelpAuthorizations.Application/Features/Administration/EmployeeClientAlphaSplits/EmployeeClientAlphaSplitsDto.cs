using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClientAlphaSplits
{
    public class EmployeeClientAlphaSplitDto : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public AlphaSplitEnum AlphaSplitId { get; set; }

        public int? EmployeeClientId { get; set; }

        [StringLength(2)]
        public string CustomBeginAlpha { get; set; }

        [StringLength(2)]
        public string CustomEndAlpha { get; set; }

        public AlphaSplitDto AlphaSplit { get; set; }
    }
}
