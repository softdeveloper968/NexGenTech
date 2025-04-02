using MedHelpAuthorizations.Application.Specifications.Base;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientFilterSpecification : HeroSpecification<Domain.Entities.Client>
    {
        public ClientFilterSpecification(string searchString)
        {
            Includes.Add(x => x.ClientAuthTypes);
            Includes.Add(x => x.ClientApplicationFeatures);
            if (!string.IsNullOrEmpty(searchString))
            {
               Criteria = p => p.Name != null && (p.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.Name != null;
            }
        }
    }
}