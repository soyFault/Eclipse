using System;
using System.Collections.Generic;

namespace Eclipse.Core;

internal readonly record struct CommandLine(bool IsCommand, string Name, string Argument);

internal static class ChatCommandParser
{
    public static CommandLine Parse(string? raw)
    {
        var text = raw?.Trim() ?? string.Empty;
        if (text.Length < 2 || text[0] != '/')
        {
            return new CommandLine(false, string.Empty, string.Empty);
        }

        text = text[1..].TrimStart();
        if (text.Length == 0)
        {
            return new CommandLine(false, string.Empty, string.Empty);
        }

        var space = text.IndexOfAny([' ', '\t', '\n']);
        return space < 0
            ? new CommandLine(true, text.ToLowerInvariant(), string.Empty)
            : new CommandLine(true, text[..space].ToLowerInvariant(), text[(space + 1)..].Trim());
    }

    public static bool Matches(string typed, IEnumerable<string> names)
    {
        foreach (var name in names)
        {
            if (name.Trim().Equals(typed, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public static List<string> Split(string? names)
    {
        var result = new List<string>();
        foreach (var piece in (names ?? string.Empty).Split(':'))
        {
            var trimmed = piece.Trim();
            if (trimmed.Length > 0)
            {
                result.Add(trimmed);
            }
        }

        return result;
    }

    public static int FindByName(IReadOnlyList<string> names, string query)
    {
        var wanted = query.Trim();
        if (wanted.Length == 0)
        {
            return -1;
        }

        for (var i = 0; i < names.Count; i++)
        {
            if (names[i].Equals(wanted, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        var found = -1;
        for (var i = 0; i < names.Count; i++)
        {
            if (names[i].StartsWith(wanted, StringComparison.OrdinalIgnoreCase))
            {
                if (found >= 0)
                {
                    return -1;
                }

                found = i;
            }
        }

        return found;
    }

    public static bool IsValidNewName(string name, IEnumerable<string> namesInUse)
    {
        var wanted = name.Trim();
        if (wanted.Length < 1 || wanted.Length > 12)
        {
            return false;
        }

        foreach (var used in namesInUse)
        {
            if (used.Trim().Equals(wanted, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }
}
