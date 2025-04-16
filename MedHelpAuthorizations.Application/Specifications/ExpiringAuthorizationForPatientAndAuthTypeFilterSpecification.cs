using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Enums;
using System;
using System.Linq;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ExpiringAuthorizationConcurrencyFilterSpecification : HeroSpecification<Authorization>
    {
        private DateTime _relativeDate;

        public ExpiringAuthorizationConcurrencyFilterSpecification(int patientId, int authTypeId, int clientId, DateTime? relativeDate = null)
        {
            _relativeDate = relativeDate ?? DateTime.UtcNow;

            Includes.Add(a => a.AuthType);
            Includes.Add(a => a.Documents);

            Criteria = p => true;

            if (clientId == 0)
                throw new ArgumentException("clientId must be greater than 0", "clientId");

            Criteria = Criteria.And(p => p.ClientId == clientId);

            //Filter out auths where concurrent auth has been obtained. 
            Criteria = Criteria.And(p => !p.InitialAuthorizations.Any());

            Criteria = Criteria.And(p => p.PatientId == patientId
                && (p.AuthTypeId == authTypeId)
                && (p.CompleteDate == null) 
                && (p.StartDate != null)
                //&& ((p.EndDate.HasValue ? (p.EndDate - DateTime.UtcNow).Value.TotalDays < 30 : false)) 
                && (p.EndDate != null && p.EndDate < _relativeDate.AddDays(30))
                && (p.DischargedOn == null || p.DischargedOn > _relativeDate));            
        }
    }
}
