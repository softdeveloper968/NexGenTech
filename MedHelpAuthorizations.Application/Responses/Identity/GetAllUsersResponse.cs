using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Responses.Identity
{
    public class GetAllUsersResponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
    }
}