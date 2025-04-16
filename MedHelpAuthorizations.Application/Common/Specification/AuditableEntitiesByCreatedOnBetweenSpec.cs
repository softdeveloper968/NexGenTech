using Ardalis.Specification;
using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Application.Common.Specification;

public class AuditableEntitiesByCreatedOnBetweenSpec<T> : Specification<T>
    where T : AuditableEntity<T>
{
    public AuditableEntitiesByCreatedOnBetweenSpec(DateTime from, DateTime until) =>
        Query.Where(e => e.CreatedOn >= from && e.CreatedOn <= until);
}