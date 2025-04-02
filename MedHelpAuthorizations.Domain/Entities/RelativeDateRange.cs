using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class RelativeDateRange : AuditableEntity<RelativeDateRangeEnum>
    {
        [StringLength(32)]
        public string Code { get; set; }

        [StringLength(64)]
        public string Description { get; set; }

        public static Tuple<DateTime, DateTime> GetRelativeDateRange(DateTime startDate, DateTime endDate, RelativeDateRangeEnum relativeDateRangeId)
        {
            switch(relativeDateRangeId)
            {
                case RelativeDateRangeEnum.FirstOfMonthToUtcNow: 
                    return Tuple.Create(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1), DateTime.UtcNow);
                case RelativeDateRangeEnum.FirstOfYearToUtcNow: 
                    return Tuple.Create(new DateTime(DateTime.UtcNow.Year, 1, 1), DateTime.UtcNow);
                case RelativeDateRangeEnum.DateMinToUtcNow: 
                    return Tuple.Create(DateTime.MinValue, DateTime.UtcNow);
                default:
                    //FirstOfMonthToUtcNow
                    return Tuple.Create(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1), DateTime.UtcNow);
            }
        }
    }
}
