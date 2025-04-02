using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    [CustomReportTypeEntityHeaderAttribute(entityName: CustomReportHelper._Person, CustomTypeCode.Empty, false)]
    public class Person : AuditableEntity<int>, IClientRelationship
    {
        public string LastCommaFirstName => $"{LastName}, {FirstName}";
        public string FirstInitialLastName => $"{FirstName?.Substring(0, 1)}. {LastName}";
        public string FirstMiddleLastName => $"{FirstName} {MiddleName} {LastName}";
        [CustomReportTypeColumnsHeaderForMainEntity(CustomReportHelper._Person, CustomTypeCode.String, CustomReportHelper.PersonFirstName)]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [CustomReportTypeColumnsHeaderForMainEntity(CustomReportHelper._Person, CustomTypeCode.String, CustomReportHelper.PersonLastName)]
        public string LastName { get; set; }
        public string SocialSecurityNumber { get; set; } //AA-218
        public long? HomePhoneNumber { get; set; } = null;
        public long? MobilePhoneNumber { get; set; } = null;
        public long? OfficePhoneNumber { get; set; } = null;
        public long? FaxNumber { get; set; } = null;
        public string Email { get; set; }
        public int? AddressId { get; set; }
        public GenderIdentityEnum? GenderIdentityId { get; set; }
        [CustomReportTypeColumnsHeaderForMainEntity(CustomReportHelper._Person, CustomTypeCode.DateTime, CustomReportHelper.PersonDateOfBirth, hasCustomDateRange: true)]
        public DateTime? DateOfBirth { get; set; }
        public int ClientId { get; set; } = 1;

        [NotMapped]
        public int Age
        {
            get
            {
                if (DateOfBirth.HasValue)
                {
                    var year = DateTime.UtcNow.Year - DateOfBirth.Value.Year;
                    if (DateTime.UtcNow.Month < DateOfBirth.Value.Month || DateTime.UtcNow.Month == DateOfBirth.Value.Month && DateTime.UtcNow.Day < DateOfBirth.Value.Day)
                        year--;

                    return year;
                }

                return 0;
            }
        }

        #region Navigation Objects

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }


        [ForeignKey("GenderIdentityId")]
        public virtual GenderIdentity GenderIdentity { get; set; }

        #endregion
    }
}
