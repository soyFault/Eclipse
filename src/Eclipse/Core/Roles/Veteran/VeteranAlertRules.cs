using System;

namespace Eclipse.Core;

internal readonly record struct AlertResponse(bool CancelsInteraction, bool KillsInteractor, bool CountsAsAttack)
{
    public static AlertResponse Nothing => new(false, false, false);
}

internal static class VeteranAlertRules
{
    public static AlertResponse Decide(
        bool targetIsAlerting,
        bool interactorIsTarget,
        bool inMeetingOrExile,
        bool interactorIsAlerting,
        bool canBeKilledOnAlert,
        bool ignoresDefense,
        bool isIndirect,
        bool isAttack)
    {
        if (inMeetingOrExile || !targetIsAlerting || interactorIsTarget)
        {
            return AlertResponse.Nothing;
        }

        var cancels = !canBeKilledOnAlert && !ignoresDefense;

        // Two Veterans on alert interacting with each other: stop it, but nobody dies (it would never end).
        var kills = !interactorIsAlerting && !isIndirect;

        return new AlertResponse(cancels, kills, isAttack);
    }

    public static bool PreventsGameEnd(int alertsLeft)
    {
        return alertsLeft > 0;
    }

    public static bool ShowsShield(bool everyoneSees, bool viewerIsVeteran, bool viewerIsDead)
    {
        return everyoneSees || viewerIsVeteran || viewerIsDead;
    }

    // 1 while there is more than fadeSeconds left, then a straight line down to 0 at the very end of the alert.
    public static float FadeAlpha(float secondsLeft, float fadeSeconds)
    {
        if (fadeSeconds <= 0f)
        {
            return secondsLeft > 0f ? 1f : 0f;
        }

        return Math.Clamp(secondsLeft / fadeSeconds, 0f, 1f);
    }
}
