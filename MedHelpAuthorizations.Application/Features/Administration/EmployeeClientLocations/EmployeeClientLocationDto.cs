namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClientLocations
{
    public class EmployeeClientLocationDto
    {
        public int? Id { get; set; }
        public int EmployeeClientId { get; set; }
        public int ClientLocationId { get; set; }
        public string ClientLocationName { get; set; }
        public int? EligibilityClientLocationId { get; set; }

        //public ClientLocationDto ClientLocation { get; set; }
    }
}
