namespace Eclipse.Core;

internal static class TextMark
{

    public static string Apply(string text, string mark)
    {
        if (text.EndsWith(mark, System.StringComparison.Ordinal))
        {
            return text;
        }

        return text + mark;
    }

    public static string Remove(string text, string mark)
    {
        if (!text.EndsWith(mark, System.StringComparison.Ordinal))
        {
            return text;
        }

        return text.Substring(0, text.Length - mark.Length);
    }

    public static string ApplyPrefix(string text, string prefix)
    {
        if (text.StartsWith(prefix, System.StringComparison.Ordinal))
        {
            return text;
        }

        return prefix + text;
    }

    public static string RemovePrefix(string text, string prefix)
    {
        if (!text.StartsWith(prefix, System.StringComparison.Ordinal))
        {
            return text;
        }

        return text.Substring(prefix.Length);
    }
}
