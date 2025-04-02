using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetById;
using System;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAllPaged;

namespace MedHelpAuthorizations.Application.Features.Patients.Queries.GetPatientsByCriteria
{
    public class GetPatientsByCriteriaResponse : GetPatientByIdResponse
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public GetAllPagedAuthTypesResponse AuthType { get; set; }
    }
}