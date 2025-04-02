using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications
{
	public class ClientFeeScheduleEntrySpecification : HeroSpecification<ClientFeeScheduleEntry>
	{
		/// <summary>
		/// Represents a specification used to filter ClientFeeScheduleEntry entities based on procedure code, client insurance, and date of service.
		/// </summary>
		/// <param name="procedureCode">The procedure code to match.</param>
		/// <param name="clientInsuranceId">The ID of the client insurance.</param>
		/// <param name="dateOfServiceFrom">The date of service to compare against.</param>
		public ClientFeeScheduleEntrySpecification(string procedureCode, int clientInsuranceId, DateTime dateOfServiceFrom)  //AA-231
		{
			IncludeStrings.Add("ClientFeeSchedule.ClientInsuranceFeeSchedules");
			Criteria = cfs => cfs.ClientCptCode.Code == procedureCode &&
							  cfs.ClientFeeSchedule.ClientInsuranceFeeSchedules.Any(x => x.ClientInsurance.Id == clientInsuranceId) &&
							  cfs.ClientFeeSchedule.StartDate != null && cfs.ClientFeeSchedule.StartDate.Date <= dateOfServiceFrom.Date && (cfs.ClientFeeSchedule.EndDate == null || cfs.ClientFeeSchedule.EndDate != null && cfs.ClientFeeSchedule.EndDate.Value.Date >= dateOfServiceFrom.Date);
		}
	}
}


