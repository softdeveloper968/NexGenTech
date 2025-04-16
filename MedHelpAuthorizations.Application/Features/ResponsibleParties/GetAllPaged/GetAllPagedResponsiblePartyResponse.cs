using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ResponsibleParties.GetAllPaged
{
    public class GetAllPagedResponsiblePartyResponse
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int Age { get; set; }

        public long? MobilePhoneNumber { get; set; } = null;
        public long? OfficePhoneNumber { get; set; } = null;
        public long? FaxNumber { get; set; } = null;
        public string Email { get; set; }



        public GenderIdentity GenderIdentity { get; set; }


        public string AccountNumber { get; private set; }
        public string ExternalId { get; set; }
        public decimal? SocialSecurityNumber { get; set; }



    }
}
