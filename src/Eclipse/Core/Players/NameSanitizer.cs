using System.Text;

namespace Eclipse.Core;

internal static class NameSanitizer
{

    public static string Clean(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name ?? string.Empty;
        }

        var dirty = false;
        foreach (var c in name)
        {
            if (NeedsChange(c))
            {
                dirty = true;
                break;
            }
        }

        if (!dirty)
        {
            return name;
        }

        var builder = new StringBuilder(name.Length);
        foreach (var c in name)
        {
            if (c == '<')
            {
                builder.Append('[');
            }
            else if (c == '>')
            {
                builder.Append(']');
            }
            else if (!IsInvisible(c))
            {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }

    private static bool NeedsChange(char c)
    {
        return c is '<' or '>' || IsInvisible(c);
    }

    private static bool IsInvisible(char c)
    {
        return c < ' '
            || c == '\u007F'
            || (c >= '​' && c <= '‏')
            || (c >= '‪' && c <= '‮')
            || (c >= '⁠' && c <= '⁩')
            || c == '﻿';
    }
}
