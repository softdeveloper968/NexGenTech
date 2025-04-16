using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientDocuments.Queries
{
    public class GetAllPagedClientDocumentsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Comments { get; set; }
        public string FileName { get; set; }
        public string URL { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string CreatedBy { get; set; }
        public int ClientId { get; set; }
        public string UserId { get; set; }
    }
}
