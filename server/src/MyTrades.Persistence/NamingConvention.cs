using System.Text.RegularExpressions;

namespace MyTrades.Persistence;

public static class NamingConvention
{
    public static string ToSnakeCase(this string name)
    {
        return Regex.Replace(name, "(?<!^)([A-Z])", "_$1").ToLower();
    }
}