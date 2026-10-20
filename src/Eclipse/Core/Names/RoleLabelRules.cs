namespace Eclipse.Core;

internal enum RoleNameStyle
{
    Top = 0,

    TopSmall = 1,

    Bottom = 2,

    BottomSmall = 3,
}

internal static class RoleLabelRules
{

    public static bool Shows(bool targetIsViewer, bool viewerIsDead, bool bothImpostors, bool roleIsRevealed)
    {
        return targetIsViewer || viewerIsDead || bothImpostors || roleIsRevealed;
    }

    public static bool IsOnTop(RoleNameStyle style)
    {
        return style is RoleNameStyle.Top or RoleNameStyle.TopSmall;
    }

    public static bool IsSmall(RoleNameStyle style)
    {
        return style is RoleNameStyle.TopSmall or RoleNameStyle.BottomSmall;
    }

    public static string FormatLine(string roleName, string colorHex, string iconTag, RoleNameStyle style)
    {
        var size = IsSmall(style) ? 80 : 100;
        return $"<size={size}%>{iconTag}<color=#{colorHex}>{roleName}</color></size>";
    }

    public static (string Prefix, string SuffixTail) Place(string? line, RoleNameStyle style)
    {
        if (string.IsNullOrEmpty(line))
        {
            return (string.Empty, string.Empty);
        }

        return IsOnTop(style) ? (line + "\n", string.Empty) : (string.Empty, "\n" + line);
    }
}
