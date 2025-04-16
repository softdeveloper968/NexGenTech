using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Features.Administration.Client_ApplicationFeatures
{
    public class ClientApplicationFeatureDto : IRequest<Result<int>>, IClientRelationship
    {
        public int ClientId { get; set; }
        public ApplicationFeatureEnum ApplicationFeatureId { get; set; }
        public Domain.Entities.Client Client { get ; set; }
    }
}
