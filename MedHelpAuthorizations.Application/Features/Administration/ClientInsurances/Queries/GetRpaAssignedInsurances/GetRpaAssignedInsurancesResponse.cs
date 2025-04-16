using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllPaged;
using System;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetRpaAssignedInsurances
{
    public class GetRpaAssignedInsurancesResponse : GetClientInsurancesBaseResponse, IEquatable<GetRpaAssignedInsurancesResponse>
    {
        public int RpaInsuranceId { get; set; }

        public bool Equals(GetRpaAssignedInsurancesResponse other)
        {
            return Id == other.Id 
                && ClientId == other.ClientId 
                && LookupName == other.LookupName 
                && Name == other.Name 
                && RpaInsuranceCode == other.RpaInsuranceCode 
                && RpaInsuranceId == other.RpaInsuranceId;
        }
    }
}
