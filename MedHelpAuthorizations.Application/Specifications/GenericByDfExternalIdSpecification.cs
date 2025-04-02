using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class GenericByDfExternalIdSpecification<T> : HeroSpecification<T> 
        where T : class, IDataPipe<int>, IEntity
    {
        public GenericByDfExternalIdSpecification(string dfExternalId)
        {
            Criteria = p => p.DfExternalId == dfExternalId;
        }
    }
}
