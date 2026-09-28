using System.Text.RegularExpressions;

namespace Domain.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string OnlyNumbers(this string input)
        {
            return Regex.Replace(input, @"\D", "");
        }
    }
}
