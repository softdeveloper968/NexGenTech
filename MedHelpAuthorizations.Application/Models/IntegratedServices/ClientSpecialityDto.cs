using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Models.IntegratedServices
{
    public class ClientSpecialityDto : IRequest<Result<int>>, IClientRelationship
    {
        public int ClientId { get; set; }
        public SpecialtyEnum SpecialtyId { get; set; }
        public Domain.Entities.Client Client { get ; set ; }
    }
}
