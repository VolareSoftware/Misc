using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class StringHelpers
    {
        public static IEnumerable<int> ToInts(this string commaDelimitedString)
        {
            if (!string.IsNullOrEmpty(commaDelimitedString))
            {
                return commaDelimitedString.Split(',').Select(int.Parse);
            }
            else
            {
                return Enumerable.Empty<int>();
            }
        }
    }
}