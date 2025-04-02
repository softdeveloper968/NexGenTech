namespace MedHelpAuthorizations.Application.Features.IntegratedServices.DataPipes.Base
{
    public class Stg_PatientDto 
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string HomePhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string Email { get; set; }
        public string AddressStreetLine1 { get; set; }
        public string AddressStreetLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? TransformedOn { get; set; }
        public int ClientId { get; set; }
        public string TenantIndentifier { get; set; }
    }
}
