using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetById
{
    public class GetAuthorizationByIdResponse
    {        
        public int Id { get; set; }
        public int AuthTypeId { get; set; }
        public string AuthTypeName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string AccountNumber { get; set; }
        public DateTime? PatientDateOfBirth { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Completeby { get; set; }
        public string AuthNumber { get; set; }
        public int? Units { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DischargedOn { get; set; }
        public string DischargedBy { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ClientId { get; set; }
        public ICollection<AuthorizationClientCptCode> AuthorizationClientCptCodes { get; set; } = new HashSet<AuthorizationClientCptCode>();
        public AuthorizationStatusEnum AuthorizationStatusId { get; set; }
        public IList<Document> Documents { get; set; } = new List<Document>();
        public IList<string> NeededDocumentTypes { get; set; } = new List<string>();
        public bool HasDocuments => Documents.Any();
        public bool NeedsDocuments => NeededDocumentTypes.Any();
    }
}