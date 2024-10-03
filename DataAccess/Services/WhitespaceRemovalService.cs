﻿using System.Text.RegularExpressions;

namespace ogybot.DataAccess.Services;

public static partial class WhitespaceRemovalService
{
    public static string RemoveExcessWhitespaces(string originalString)
    {
        return RemoveWhitespacesRegex()
            .Replace(originalString, " ")
            .Trim();
    }

    [GeneratedRegex(@"[\sÁÀ]+")]
    private static partial Regex RemoveWhitespacesRegex();
}