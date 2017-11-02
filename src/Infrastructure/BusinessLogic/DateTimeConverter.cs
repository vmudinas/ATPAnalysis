using System;
using System.Globalization;

namespace Infrastructure.BusinessLogic
{
    public class DateTimeConverter
    {
        public static DateTime MomentUtcToDateTime(string dateTime)
        {
            return DateTime.Parse(
                dateTime.Replace("\"", ""), CultureInfo.InvariantCulture);
        }
    }
}