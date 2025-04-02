using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System.Linq;
using static MedHelpAuthorizations.Shared.Constants.Permission.Permissions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MedHelpAuthorizations.Application.Specifications
{
    /// <summary>
    /// Specification for filtering <see cref="ClientInsuranceAverageCollectionPercentage"/> entities based on the same year, quarter, and client insurance identifier.
    /// </summary>
    public class SameYearQuarterClientInsuranceSpecification : HeroSpecification<ClientInsuranceAverageCollectionPercentage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SameYearQuarterClientInsuranceSpecification"/> class.
        /// </summary>
        /// <param name="quarter">The quarter to filter on.</param>
        /// <param name="year">The year to filter on.</param>
        /// <param name="clientInsuranceId">The unique identifier of the client insurance to filter on.</param>
        public SameYearQuarterClientInsuranceSpecification(int quarter, string year, string clientInsuranceIds)
        {
            // Start with a base criteria that always evaluates to true
            Criteria = bc => true;

            // Add criteria for matching the specified quarter
            Criteria = Criteria.And(c => c.Quarter == quarter);

            // Add criteria for matching the specified year
            Criteria = Criteria.And(c => c.Year == year);

            // Add criteria for matching the specified client insurance identifier
            if (!string.IsNullOrEmpty(clientInsuranceIds))
            {
                Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertStringToList(clientInsuranceIds, false).Contains(c.ClientInsuranceId));
            }
        }
    }

}
