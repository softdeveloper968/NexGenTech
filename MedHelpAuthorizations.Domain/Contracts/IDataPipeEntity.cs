using System;

namespace MedHelpAuthorizations.Domain.Contracts
{
    public interface IDataPipeEnity
    {
        public int ClientId { get; set; }
        public string TenantIdentifier { get; set; }
        public DateTime? TransformedOn { get; set; }
    }
}
