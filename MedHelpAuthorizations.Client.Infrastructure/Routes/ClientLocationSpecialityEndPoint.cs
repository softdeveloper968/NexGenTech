
namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ClientLocationSpecialityEndPoint
    {
        public static string GetClientLocationSpeciality(int locationId)
        {
            return $"api/v1/tenant/ClientLocationSpeciality/GetClientLocationSpeciality?locationId={locationId}";
        }
    }
}
