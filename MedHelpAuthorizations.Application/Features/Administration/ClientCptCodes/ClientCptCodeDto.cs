using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes
{
    public class ClientCptCodeDto
    {
        public int Id { get; set; }
        public decimal? ScheduledFee { get; set; } = 0.00m;
        public int? CptCodeGroupId { get; set; }
        public int ClientId { get; set; }
        public string LookupName { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Code { get; set; }
        public string CodeVersion { get; set; }
        public TypeOfServiceEnum? TypeOfServiceId { get; set; }
    }
}
