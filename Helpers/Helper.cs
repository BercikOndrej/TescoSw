using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TescoSwTask.Helpers
{
    public static class Helper
    {
        public static string FormatNumber(double number) 
        {
            var customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ",";
            customCulture.NumberFormat.NumberGroupSeparator = " ";
            return number.ToString("#,0.00", customCulture) + " ,- Kč";
        }

        public static bool IsSaleAtWeekend(DateTime date) => date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
    }
}
