using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.UserAlerts.Queries.GetById
{
    public class GetUserAlertsByIdResponse
    {      
        public int Id { get; set; }
        public string UserId { get; set; }
        public AlertTypeEnum AlertType { get; set; }

        public string PreviewText { get; set; }
        public string ResourceType { get; set; }
        public string ResourceId { get; set; }
        public bool IsViewed { get; set; }
        public bool IsRemoved { get; set; }
    }
}
