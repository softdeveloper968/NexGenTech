//using MedHelpAuthorizations.Domain.Entities.Enums;
//using System;

//namespace MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByPatientId
//{
//    public class GetPagedAuthorizationByPatientIdResponse
//    {
//        public int Id { get; set; }
//        public int AuthTypeId { get; set; }
//        public int PatientId { get; set; }
//        public string PatientName { get; set; }
//        public DateTime CreatedOn { get; set; }
//        public DateTime? PatientDateOfBirth { get; set; }
//        public DateTime? CompleteDate { get; set; }
//        public string Completeby { get; set; }
//        public string AuthNumber { get; set; }
//        public int? Units { get; set; }
//        public DateTime? StartDate { get; set; }
//        public DateTime? EndDate { get; set; }
//        public DateTime? DischargedOn { get; set; }
//        public string DischargedBy { get; set; }
//        public bool HasDocuments { get; set; } = false;
//        public bool NeedsDocuments { get; set; } = false;
//        public int ClientId { get; set; }
//        public AuthorizationStatusEnum AuthorizationStatusId { get; set; }
//    }    
//}