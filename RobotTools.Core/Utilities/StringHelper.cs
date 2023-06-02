using System.Text.RegularExpressions;

namespace RobotTools.Core.Utilities;

public static class StringHelper
{
    /// <summary>
    /// Extend the string constructor with a string.Format like syntax.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string FormatWith(this string s, params object[] args)
    {
        return string.Format(s, args);
    }

    public static string Match(this string s,string pattern)
    {
        var regex = Regex.Match(s, pattern, RegexOptions.IgnoreCase);
            if (regex.Success)
                return regex.Groups[1].Value;
        
        return null;
    }
}
