using System.Text.RegularExpressions;
using System.Web;

namespace MyProject.Extensions
{
    public static class HtmlStringExtensions
    {
        public static string RemoveWhitespace(this IHtmlString htmlString)
        {
            return Regex.Replace(htmlString.ToHtmlString(), @"\s", "");
        }
    }
}