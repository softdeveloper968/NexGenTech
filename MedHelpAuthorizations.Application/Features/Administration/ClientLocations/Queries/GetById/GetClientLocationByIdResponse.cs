using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.Base;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetById
{
    public class GetClientLocationByIdResponse : GetClientLocationsBaseResponse
    {
        public int? Id { get; set; }

    }
}