using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

public static class StringExtensions
{
    public static bool IsComparableTo(this string source, string text)
    {
        var newSource = KeepOnlyLettersOrDigits(source);
        var newText = KeepOnlyLettersOrDigits(text);

        return String.Compare(newSource, newText, CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0;
    }

    public static bool IsIncludeIn(this string source, string text)
    {
        var newSource = KeepOnlyLettersOrDigits(source);
        var newText = KeepOnlyLettersOrDigits(text);

        return newText.Contains(newSource);
    }

    public static string IsSentenceContains(this string sentense, IEnumerable<string> values)
    {
        var newSentense = KeepOnlyLettersOrDigits(sentense);
        var valuesOrdered = values.ToDictionary(i => i, j => KeepOnlyLettersOrDigits(j))
                                  .OrderByDescending(i => i.Value.Length);

        foreach (var newtext in valuesOrdered)
        {
            if (newSentense.Contains(newtext.Value)) return newtext.Key;
        }
        return string.Empty;
    }

    private static string KeepOnlyLettersOrDigits(string text)
    {
        var result = new StringBuilder();
        foreach (char c in RemoveAbreviations(text))
        {
            if (char.IsLetterOrDigit(c))
                result.Append(c);
        }
        return result.ToString().ToLower();
    }

    private static string RemoveAbreviations(string text)
    {
        return text.ToLower().Replace("saint", "st");
    }
}
