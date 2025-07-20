using Humanizer;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Renamer.Extensions;

internal static class StringExtensions
{
    internal static string ToPascalCasePreservingSeparators(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var parts = Regex.Split(input, @"([_-])");

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] != "_" && parts[i] != "-")
            {
                parts[i] = parts[i].Dehumanize();
                // Capitalize first letter for PascalCase
                if (parts[i].Length > 0)
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
            }
        }

        return string.Concat(parts);
    }
}
