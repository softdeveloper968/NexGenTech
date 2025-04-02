using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Notes.Queries.GetById
{
     public class GetClaimStatusWorkstationNotesResponse
    {
        public int Id { get; set; }
        public int? ClaimStatusTransactionId { get; set; }
        public DateTime? NoteTs { get; set; }
        public int? ClientId { get; set; }
        public string NoteContent { get; set; }
        public string UserName { get; set; }
    }
}
