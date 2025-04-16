using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class UpdateUserBasicInfoRequest
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public long? PhoneNumber { get; set; } = null;
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; } //EN-147
    }
}
