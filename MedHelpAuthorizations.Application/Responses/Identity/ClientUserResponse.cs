using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Responses.Identity
{
    public class ClientUserResponse 
    {
        public int ClientUserId { get; set; }
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; }

    }
}
