namespace Eclipse.Core;

internal static class KillCooldownRules
{
    public static bool Pauses(bool tickInMinigame, bool minigameOpen)
    {
        return !tickInMinigame && minigameOpen;
    }
}
