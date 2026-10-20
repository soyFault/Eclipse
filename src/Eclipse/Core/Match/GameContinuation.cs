namespace Eclipse.Core;

internal static class GameContinuation
{

    public static bool IsDeathEndHeld(int impostorsAlive, int othersAlive, int haltersAlive)
    {
        return IsDeathEndHeld(impostorsAlive, othersAlive, haltersAlive, 0);
    }

    public static bool IsDeathEndHeld(int impostorsAlive, int othersAlive, int haltersAlive, int neutralKillersAlive)
    {
        return IsDeathEndHeld(impostorsAlive, othersAlive, haltersAlive, neutralKillersAlive, false);
    }

    public static bool IsDeathEndHeld(int impostorsAlive, int othersAlive, int haltersAlive, int neutralKillersAlive, bool traitorPending)
    {
        return neutralKillersAlive > 0 || (impostorsAlive == 0 && traitorPending) || (impostorsAlive > 0 && haltersAlive > 0 && impostorsAlive >= othersAlive);
    }
}
