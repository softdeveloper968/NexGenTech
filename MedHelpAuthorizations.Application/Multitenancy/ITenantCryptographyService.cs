using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Multitenancy
{
    public interface ITenantCryptographyService
    {
        public string Encrypt(string tenantId, int clientId);
        public Tuple<string, int> Decrypt(string encryptedClientTenantId);
    }
}
