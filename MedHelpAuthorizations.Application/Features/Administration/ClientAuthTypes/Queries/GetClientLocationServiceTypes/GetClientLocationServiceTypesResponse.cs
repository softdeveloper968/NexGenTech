using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetClientLocationServiceTypes
{
    public class GetClientLocationServiceTypesResponse
    {
        public int Id { get; set; }
        public int AuthTypeId { get; set; }
        public int ClientLocationId { get; set; }
        public int ClientId { get; set; }
        
    }
}
