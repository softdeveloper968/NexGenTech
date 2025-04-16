using MedHelpAuthorizations.Domain.Entities.Enums;
using System;

namespace MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.Base
{
    public class GetInsuranceCardResponseBase
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public int PatientId { get; set; }
        public int InsuranceId { get; set; }
        public string InsuranceName { get; set; }
        public int PersonId { get; set; }
        public int CardholderId { get; set; }
        public string CardholderFirstName { get; set; }
        public string CardholderLastName { get; set; }
        public string CardholderMiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int ClientInsuranceId { get; set; }
        public string ClientInsuranceName { get; set; }
        public string GroupNumber { get; set; }
        public string MemberNumber { get; set; }
        public RelationShipTypeEnum? CardholderRelationshipToPatient { get; set; }
        public byte? InsuranceCoverageTypes { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int InsuranceCardOrder { get; set; }
        public int CoPayAmount { get; set; }
        public int? ScannedImageId { get; set; }
        public bool Verified { get; set; }
        public int ClientId { get; set; }
        //public int ClientEntityId { get; set; }
    }
}

