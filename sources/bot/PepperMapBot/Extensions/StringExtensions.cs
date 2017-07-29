using System;
using System.Globalization;
using System.Text;

public static class StringExtensions
{
    /// <summary>
    /// Compares two specified System.String objects unsing a Comparable algorithm:
    /// only letters and digits are signifiant ("A'&é" == "aE").
    /// Returns an integer that indicates the relationship of the two strings to
    /// each other in the sort order.
    /// </summary>
    /// <param name="source">The first string to compare</param>
    /// <param name="text">The second string to compare</param>
    /// <returns>0 if the two strings are comparable.</returns>
    public static bool IsComparableTo(this string source, string text)
    {
        StringBuilder string1 = new StringBuilder();
        StringBuilder string2 = new StringBuilder();

        // Remove extra characters            
        foreach (char c in RemoveAbreviations(source))
        {
            if (char.IsLetterOrDigit(c))
                string1.Append(c);
        }

        foreach (char c in RemoveAbreviations(text))
        {
            if (char.IsLetterOrDigit(c))
                string2.Append(c);
        }

        return String.Compare(string1.ToString(), string2.ToString(), CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0;

    }

    /// <summary>
    /// Remplace les noms par leux abrégés.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private static string RemoveAbreviations(string text)
    {
        return text.ToLower().Replace("saint", "st");
    }
}
