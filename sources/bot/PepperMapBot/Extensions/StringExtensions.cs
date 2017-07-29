using System;
using System.Globalization;
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

        return newText.ToLower().Contains(newSource.ToLower());
    }

    private static string KeepOnlyLettersOrDigits(string text)
    {
        var result = new StringBuilder();
        foreach (char c in RemoveAbreviations(text))
        {
            if (char.IsLetterOrDigit(c))
                result.Append(c);
        }
        return result.ToString();
    }

    private static string RemoveAbreviations(string text)
    {
        return text.ToLower().Replace("saint", "st");
    }
}
