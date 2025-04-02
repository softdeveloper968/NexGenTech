
namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base
{
    public class EmployeeClientViewModel
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeClientInsurancesString {get; set; }

        public string EmployeeClientLocationsString { get; set; }

        public string EmployeeClientAlphaSplitsString { get; set; }

        public string AssignedEmployeeRolesString { get; set; }

        public int AssignedAverageDailyClaimCount { get; set; }

        public decimal ExpectedMonthlyCashCollections { get; set; }
        public string UserId { get; set; } //AA-206 TODO : update mapping
        public string EmployeeUserId { get; set; }
        public bool ReceiveReport { get; set; }
    }
}
