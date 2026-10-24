using System.Collections.Generic;
using System.Text;

namespace Eclipse.Core;

internal static class IntroText
{
    public static string Compose(string blurb, string? alignment, string? modifierLine)
    {
        var builder = new StringBuilder(blurb);
        AppendLine(builder, alignment);
        AppendLine(builder, modifierLine);
        return builder.ToString();
    }

    public static string ModifierLine(string singular, string plural, IReadOnlyList<string> names)
    {
        return names.Count == 0 ? string.Empty : (names.Count == 1 ? singular : plural) + ": " + string.Join(", ", names);
    }

    private static void AppendLine(StringBuilder builder, string? line)
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            builder.Append("\n<size=70%><alpha=#BB>").Append(line).Append("<alpha=#FF></size>");
        }
    }
}
