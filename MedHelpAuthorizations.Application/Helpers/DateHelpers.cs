using System;

namespace MedHelpAuthorizations.Application.Helpers
{
    public static class DateHelpers
    {
        public static DateTime? ConvertStringToNullableDate(string dateString)
        {

            DateTime parsedDate;

            var success = DateTime.TryParse(dateString, out parsedDate);
            if (success)
                return parsedDate;
            else
                return null;
        }

        public static string GetReformattedDateString(string dateString)
        {

            DateTime parsedDate;

            var success = DateTime.TryParse(dateString, out parsedDate);
            if (success)
                return parsedDate.ToString("MMddyyyy");
            else
                return null;
        }
    }
}
