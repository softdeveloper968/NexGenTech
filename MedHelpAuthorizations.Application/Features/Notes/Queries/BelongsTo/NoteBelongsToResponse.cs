using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Notes.Queries.BelongsTo
{
  public class NoteBelongsToResponse
  {
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int AuthorizationId { get; set; }

  }
}
