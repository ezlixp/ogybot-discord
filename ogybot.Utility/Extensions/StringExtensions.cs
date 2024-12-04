using System.Diagnostics.CodeAnalysis;

namespace ogybot.Utility.Extensions;

public static class StringExtensions
{
    /// <summary>
    ///     Checks if the specified string is null, empty or white-spaces only.
    /// </summary>
    /// <param name="str">The string to be checked</param>
    /// <returns></returns>
    public static bool IsNullOrWhitespace([NotNullWhen(false)] this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
}