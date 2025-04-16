
namespace MedHelpAuthorizations.Application.Features.IntegratedServices.RpaInsurances.Queries.GetAll
{
    public class GetAllRpaInsurancesResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCredentialInUse { get; set; }
        public string RpaInsuranceGroupName { get; set; }
    }
}
