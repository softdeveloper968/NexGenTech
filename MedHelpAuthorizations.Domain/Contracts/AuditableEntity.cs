using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Shared.Helpers;
using System;

namespace MedHelpAuthorizations.Domain.Contracts
{
    [CustomReportTypeEntityHeader(entityName: CustomReportHelper._AuditableEntity, typeCode: CustomTypeCode.Empty, hasSubEntityExist: false)]
    public abstract class AuditableEntity<TId> : IAuditableEntity<TId>
    {
       
        public TId Id { get; set; }
        public string CreatedBy { get; set; }
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: "", typeCode: CustomTypeCode.DateRangeType, propertyName: CustomReportHelper.CreatedOn, hasRelativeDateRange: true)]
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}