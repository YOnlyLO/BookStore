using System.Text.RegularExpressions;

namespace BookStore.WebUiTest.Infrastructure;

public static partial class UiText
{
    /// <summary>
    /// Приводит текст элемента к виду для сравнения: Angular форматирует числа и цены
    /// по-русски с неразрывными пробелами («1 350,00 ₽»), а ячейки могут содержать переносы строк.
    /// </summary>
    public static string Normalize(string text)
    {
        return Whitespace().Replace(text, " ").Trim();
    }

    /// <summary>
    /// Строковый литерал XPath. В XPath 1.0 нет экранирования, поэтому строку
    /// с обоими видами кавычек приходится собирать через concat().
    /// </summary>
    public static string XPathLiteral(string value)
    {
        if (!value.Contains('\''))
        {
            return $"'{value}'";
        }

        if (!value.Contains('"'))
        {
            return $"\"{value}\"";
        }

        return $"concat('{value.Replace("'", "', \"'\", '")}')";
    }

    // \s в .NET включает неразрывный пробел (U+00A0) и узкий неразрывный пробел (U+202F).
    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();
}
