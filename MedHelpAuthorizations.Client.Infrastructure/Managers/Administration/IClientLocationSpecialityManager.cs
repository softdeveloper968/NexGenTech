using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetClientLocationServiceTypes;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocationSpeciality.Queries.GetClientLocationSpecilalityQuery.cs;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientLocationSpecialityManager : IManager
    {
        Task<IResult<List<GetClientLocationSpecilalityResponse>>> GetClientLocationSpeciality(int locationId);
    }
}
