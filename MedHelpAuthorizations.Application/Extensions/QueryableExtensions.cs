using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using MedHelpAuthorizations.Domain.Common.Contracts;
using Hangfire.MemoryStorage.Dto;
using System.Linq.Expressions;

namespace MedHelpAuthorizations.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class
        {
            if (source == null) throw new ApiException();
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = await source.CountAsync();
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
        }

        public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class, IEntity
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(query,
                    (current, include) => current.Include(include));
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));
            return secondaryResult.Where(spec.Criteria);
        }

        public static IQueryable<T> OrderByMappings<T>(this IQueryable<T> query, string sortBy, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy) || !IsValidSortOrder(sortOrder))
            {
                return query; // No sorting specified or invalid sort order
            }

            if (!SortDefinitions.SortDefinitions.SortFieldMappings.TryGetValue(sortBy, out var mappedField))
            {
                return query; // Invalid sort field specified
            }

            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, mappedField);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";
            var types = new Type[] { query.ElementType, property.Type };
            var methodCallExpression = Expression.Call(typeof(Queryable), methodName, types, query.Expression, lambda);

            return query.Provider.CreateQuery<T>(methodCallExpression);
        }

        private static bool IsValidSortOrder(string sortOrder)
        {
            return string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(sortOrder, "DESC", StringComparison.OrdinalIgnoreCase);
        }
    }
}