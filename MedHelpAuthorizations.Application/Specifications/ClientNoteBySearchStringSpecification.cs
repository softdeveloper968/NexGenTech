using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientNoteBySearchStringSpecification : HeroSpecification<ClientNote>
    {
        public ClientNoteBySearchStringSpecification(string searchString, int clientId)
        {
            Criteria = p => p.ClientId == clientId;

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = Criteria.And(p => (p.Title != null && p.Title.Contains(searchString)) || (p.Note != null && p.Note.Contains(searchString)));
            }
        }
    }
}
