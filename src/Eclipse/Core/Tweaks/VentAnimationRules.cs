namespace Eclipse.Core;

internal static class VentAnimationRules
{
    public static bool Plays(bool hideOutOfSight, bool inSight)
    {
        return !hideOutOfSight || inSight;
    }
}
