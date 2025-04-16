using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Models.Common;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Responses.Identity
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; }
        public long? PhoneNumber { get; set; } = null;
        public bool IsExistingEmployee { get; set; } = false; //AA-233
        public string ProfilePictureDataUrl { get; set; }
        public IEnumerable<string> TenantIdentifiers { get; set; } = new List<string>().AsEnumerable();
		public IEnumerable<string> EmployeeLinkedTenants { get; set; } = new List<string>().AsEnumerable();
	}

    public class UserMasterResponse 
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; } = true;
        public long? PhoneNumber { get; set; } = null;
        public string ProfilePictureDataUrl { get; set; }
        public IEnumerable<UserRoleModel> Roles { get; set; } = new List<UserRoleModel>();
        public IEnumerable<TenantResponse> Tenants { get; set; } = new List<TenantResponse>();
        public IEnumerable<BasicTenantClientResponse> TenantClients { get; set; } = new List<BasicTenantClientResponse>();
        public string Pin { get; set; }
    }

    public class UserInfoResponse : AuditableResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; }
        public long? PhoneNumber { get; set; } = null;

        public bool IsExistingEmployee { get; set; } = false; //AA-233
        public IEnumerable<string> TenantIdentifiers { get; set; } = new List<string>().AsEnumerable();
    }
}