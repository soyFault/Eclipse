using Eclipse.Api.Factions;

namespace Eclipse.Core;

internal enum OfficerShot
{
    Clean = 0,
    Misfire = 1,
}

internal enum OfficerHint
{
    KillBased = 0,
    MajorityInnocent = 1,
    CrewmatesInnocent = 2,
}

internal readonly record struct OfficerShotSettings(bool OnlyActiveKillers, bool CrewKillingAreInnocent, bool NonKillingNeutralsAreInnocent);

internal static class OfficerRules
{

    public const float FreshSeconds = 10f;

    public static OfficerShot Decide(Faction faction, string? alignmentId, bool targetHasKilled, OfficerShotSettings settings)
    {
        if (settings.OnlyActiveKillers)
        {
            var innocentCrew = faction == Faction.Crewmate && settings.CrewKillingAreInnocent;
            return innocentCrew || !targetHasKilled ? OfficerShot.Misfire : OfficerShot.Clean;
        }

        var safeNeutral = settings.NonKillingNeutralsAreInnocent
                          && alignmentId is AlignmentIds.NeutralBenign or AlignmentIds.NeutralEvil or AlignmentIds.NeutralOutlier;
        return safeNeutral || faction == Faction.Crewmate ? OfficerShot.Misfire : OfficerShot.Clean;
    }

    public static OfficerHint TabHint(bool onlyActiveKillers, bool nonKillingNeutralsAreInnocent)
    {
        if (onlyActiveKillers)
        {
            return OfficerHint.KillBased;
        }

        return nonKillingNeutralsAreInnocent ? OfficerHint.MajorityInnocent : OfficerHint.CrewmatesInnocent;
    }

    public static bool IsFresh(float secondsSinceLoad)
    {
        return secondsSinceLoad < FreshSeconds;
    }

    public static (int Rounds, int Loaded) AfterMeetingStart(int roundsBeforeReset, bool hasFreshBullet)
    {
        var rounds = roundsBeforeReset - 1;
        return rounds > 0 ? (rounds, 0) : (0, hasFreshBullet ? 1 : 0);
    }

    public static bool CanLoad(bool gunConfiscated, int loaded, int maxAtOnce, int bulletsLeft)
    {
        return !gunConfiscated && loaded < maxAtOnce && bulletsLeft > 0;
    }

    public static bool CanShoot(bool gunConfiscated, int loaded)
    {
        return !gunConfiscated && loaded > 0;
    }

    public static bool PreventsGameEnd(int bulletsLeft, int loaded)
    {
        return bulletsLeft > 0 || loaded > 0;
    }
}
