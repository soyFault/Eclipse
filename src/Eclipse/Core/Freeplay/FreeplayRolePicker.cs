namespace Eclipse.Core;

internal static class FreeplayRolePicker
{
    public static bool IsSelectable(bool isDeadRole, bool hiddenFromFreeplay)
    {
        return !isDeadRole && !hiddenFromFreeplay;
    }

    public static string SortKey(string? roleName)
    {
        return (roleName ?? string.Empty).Trim().ToLowerInvariant();
    }
}
