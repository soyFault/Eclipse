using System;

namespace Eclipse.Core;

internal static class UniversalRules
{
    public const float BaseSpeed = 2.5f;

    public const float NormalScale = 0.7f;

    public const float GiantScale = 1f;

    public const float MiniScale = 0.49f;

    public static bool CanReceive(bool alreadyHasUniversal)
    {
        return !alreadyHasUniversal;
    }

    public static float SpeedFor(float multiplier)
    {
        return BaseSpeed * Math.Clamp(multiplier, 0.25f, 2.5f);
    }

    public static bool CanCallMeeting(int emergenciesLeft, bool sabotageActive, bool ignoreSabotage, bool dead)
    {
        return !dead && emergenciesLeft > 0 && (ignoreSabotage || !sabotageActive);
    }

    public static bool SleuthLearns(bool reporterIsSleuth, bool bodyKnown)
    {
        return reporterIsSleuth && bodyKnown;
    }
}
