namespace Eclipse.Core;

internal static class FirstRoundRule
{
    public static bool IsLocked(bool allowedInFirstRound, bool isFirstRound, bool isFreeplay)
    {
        return !allowedInFirstRound && isFirstRound && !isFreeplay;
    }
}
