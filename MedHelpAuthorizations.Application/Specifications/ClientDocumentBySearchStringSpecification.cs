using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientDocumentBySearchStringSpecification : HeroSpecification<ClientDocument>
    {
        public ClientDocumentBySearchStringSpecification(string searchString, int clientId) //EN-791
        {
            Criteria = p => p.ClientId == clientId;

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = Criteria.And(p => (p.Title != null && p.Title.Contains(searchString)) || (p.Comments != null && p.Comments.Contains(searchString)) || (p.FileName != null && p.FileName.Contains(searchString)));
            }
        }
    }
}
