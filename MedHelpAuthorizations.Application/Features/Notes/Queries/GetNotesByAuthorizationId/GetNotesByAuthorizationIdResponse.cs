using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId
{
    public class GetNotesByAuthorizationIdResponse
    {
        public int Id { get; set; }
        public int? AuthorizationId { get; set; }
        public string NoteUserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ClientId { get; set; }
        public string NoteContent { get; set; }
    }
}
