using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Business.Helpers
{
    public static class Utilities
    {
        public static bool IsValidTimeFormat(string input)
        {
            TimeSpan dummyOutput;

            return TimeSpan.TryParse(input, out dummyOutput);
        }
        public static DateTimeOffset ConvertStringToDatetime(string input)
        {
            if (string.IsNullOrEmpty(input))
                return DateTimeOffset.Now;
            //if (input.Split(' ').Length > 0)
            input = input.Trim();
            DateTimeOffset dummyOutput;
            if (DateTimeOffset.TryParseExact(input, "dd/MM/yyyy",
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dummyOutput)
               ||
               (DateTimeOffset.TryParseExact(input, "d/M/yyyy",
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dummyOutput))
               ||
               (DateTimeOffset.TryParseExact(input, "dd/M/yyyy",
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dummyOutput))
               ||
               (DateTimeOffset.TryParseExact(input, "d/MM/yyyy",
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dummyOutput))
               ||
               DateTimeOffset.TryParseExact(input, "dd-MM-yyyy",
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dummyOutput)
               ||
               (DateTimeOffset.TryParseExact(input, "d-M-yyyy",
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dummyOutput))
               ||
               (DateTimeOffset.TryParseExact(input, "dd-M-yyyy",
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dummyOutput))
               ||
               (DateTimeOffset.TryParseExact(input, "d-MM-yyyy",
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dummyOutput)))
            {
                return dummyOutput;
            }
            else
            {
                return DateTimeOffset.Now;
            }
        }
        public static bool IsValidDatetimeFormat(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            //if (input.Split(' ').Length > 0)
            //    input = input.Split(' ')[0];
            input = input.Trim();
            DateTimeOffset dummyOutput;
            if (DateTimeOffset.TryParseExact(input, "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dummyOutput)
                ||
                (DateTimeOffset.TryParseExact(input, "d/M/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dummyOutput))
                ||
                (DateTimeOffset.TryParseExact(input, "dd/M/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dummyOutput))
                ||
                (DateTimeOffset.TryParseExact(input, "d/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dummyOutput))
                ||
                DateTimeOffset.TryParseExact(input, "dd-MM-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dummyOutput)
                ||
                (DateTimeOffset.TryParseExact(input, "d-M-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dummyOutput))
                ||
                (DateTimeOffset.TryParseExact(input, "dd-M-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dummyOutput))
                ||
                (DateTimeOffset.TryParseExact(input, "d-MM-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dummyOutput)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
