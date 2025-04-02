using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.IdentityEntities.Enums
{
    public enum ServerType
    {
        Database = 1
    }

    public enum AuthenticationType
    {
        Windows = 1,
        Credentials = 2
    }
}
