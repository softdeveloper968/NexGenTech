using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Notes.Queries.GetById
{
    public class GetNotesByIdResponse
    {
        public int Id { get; set; }
        public int? AuthorizationId { get; set; }
        public string NoteUserId { get; set; }
        public DateTime? NoteTs { get; set; }
        public int ClientId { get; set; }
        public string NoteContent { get; set; }
    }
}
