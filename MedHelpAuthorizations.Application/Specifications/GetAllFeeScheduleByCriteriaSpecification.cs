using MedHelpAuthorizations.Application.Specifications.Base;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class GetAllFeeScheduleByCriteriaSpecification : HeroSpecification<Domain.Entities.ClientFeeSchedule>
    {
		/// <summary>
		/// Specifies the criteria for retrieving fee schedules based on client, client insurance, and date of service. //EN-155
		/// </summary>
		/// <param name="clientId">The ID of the client.</param>
		/// <param name="clientInsuranceId">The ID of the client insurance.</param>
		/// <param name="dateOfService">The date of service for which to retrieve fee schedules.</param>
		public GetAllFeeScheduleByCriteriaSpecification(int clientId, int clientInsuranceId, DateTime dateOfService)
        {
            AddInclude(x => x.ClientInsuranceFeeSchedules);

            Criteria = p => true;

            Criteria = p =>
            p.ClientId == clientId &&
            p.ClientInsuranceFeeSchedules.Any(c => c.ClientInsuranceId == clientInsuranceId) &&
            (
                (dateOfService >= p.StartDate) &&
                (p.EndDate == null || dateOfService < p.EndDate.Value)
            );
        }
    }
}
