using System;
using System.Collections.Generic;

#nullable disable

namespace MedHelpAuthorizations.Domain.Models
{
    public class ClientUser
    {
        public long ClientUserId { get; set; }
        public long ClientId { get; set; }
        public long UserId { get; set; }

        public virtual Client Client { get; set; }
        public virtual User User { get; set; }
    }
}
