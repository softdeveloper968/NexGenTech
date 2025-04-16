using System.Linq;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientUserReportFiltersByUserIdSpecification : HeroSpecification<ClientUserReportFilter>
    {
        public ClientUserReportFiltersByUserIdSpecification(string userId)
        {
            IncludeStrings.Add("EmployeeClientUserReportFilters.EmployeeClient.Employee");

            //Includes.Add(a => a.EmployeeClientUserReportFilters);
            Criteria =x => x.UserId == userId;
            Criteria = Criteria.Or(x => x.EmployeeClientUserReportFilters.Any(e => e.EmployeeClient != null && e.EmployeeClient.Employee.UserId == userId));
        }

    }
}
