using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Responses.Identity
{
    public class RoleResponse
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
    public class RoleNamesResponse
    {
        public string NormalizedName { get; set; }
        public string Name { get; set; }
    }
}