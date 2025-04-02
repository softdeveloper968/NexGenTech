using MedHelpAuthorizations.Application.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Models
{
    public class TenantAssociatedUserResponse
    {
        public string TenantName { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; }
        public long? PhoneNumber { get; set; } = null;
        public bool IsExistingEmployee { get; set; } = false;
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedByName { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
