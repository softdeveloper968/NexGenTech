using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MedHelpAuthorizations.Domain.Common.Contracts;
using Microsoft.EntityFrameworkCore.Internal;

namespace MedHelpAuthorizations.Application.Specifications.Base
{
    public abstract class HeroSpecification<T> : ISpecification<T> where T : class, IEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        public List<string> IncludeStrings { get; } = new();
		public Expression<Func<T, object>> Predicate { get; set; }

		protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

	}
}