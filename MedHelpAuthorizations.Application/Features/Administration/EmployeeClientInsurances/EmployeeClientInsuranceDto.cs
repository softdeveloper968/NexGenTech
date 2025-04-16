using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClientInsurances
{
    public class EmployeeClientInsuranceDto 
    {
        public int? Id {  get; set; }

        public int EmployeeClientId { get; set; }

        public int ClientInsuranceId { get; set; }

        public string ClientInsuranceName { get; set; }

        //public ClientInsuranceDto ClientInsurance { get; set; }

    }
}
