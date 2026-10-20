namespace Eclipse.Core;

internal static class MysticRules
{
    public static bool IsAlerted(bool inMeeting, bool localIsMystic, bool localIsDead, bool victimIsLocal)
    {
        return !inMeeting && localIsMystic && !localIsDead && !victimIsLocal;
    }
}
