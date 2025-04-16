using MedHelpAuthorizations.Application.Features.Documents.Queries.Base;
using System;

namespace MedHelpAuthorizations.Application.Features.Documents.Queries.GetByCriteria
{
    public class GetByCriteriaDocumentsResponse : GetDocumentResponse
    {
        public int AuthorizationId { get; set; }
        public DateTime? AuthorizationCreatedOn { get; set; }
        public int PatientId { get; set; }
    }
}