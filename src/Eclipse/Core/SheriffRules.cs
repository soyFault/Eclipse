using Eclipse.Api.Factions;

namespace Eclipse.Core;

internal enum MisfireKills
{
    Sheriff,

    Target,

    Both,

    Nobody,
}
internal readonly record struct SheriffShotSettings(
    bool NeutralBenignValid,
    bool NeutralEvilValid,
    bool NeutralKillingValid,
    bool NeutralOutlierValid,
    MisfireKills Misfire);

internal readonly record struct ShotOutcome(bool IsValidShot, bool TargetDies, bool SheriffDies)
{

    public bool IsMisfire => !IsValidShot;
}

internal static class SheriffRules
{
    public static ShotOutcome Decide(Faction targetFaction, string? targetAlignmentId, SheriffShotSettings settings)
    {
        return IsValidTarget(targetFaction, targetAlignmentId, settings)
            ? new ShotOutcome(IsValidShot: true, TargetDies: true, SheriffDies: false)
            : new ShotOutcome(
                IsValidShot: false,
                TargetDies: settings.Misfire is MisfireKills.Target or MisfireKills.Both,
                SheriffDies: settings.Misfire is MisfireKills.Sheriff or MisfireKills.Both);
    }

    private static bool IsValidTarget(Faction faction, string? alignmentId, SheriffShotSettings settings)
    {
        switch (faction)
        {
            case Faction.Impostor:
                return true;
            case Faction.Crewmate:
                return false;
            default:
                return alignmentId switch
                {
                    AlignmentIds.NeutralBenign => settings.NeutralBenignValid,
                    AlignmentIds.NeutralEvil => settings.NeutralEvilValid,
                    AlignmentIds.NeutralKilling => settings.NeutralKillingValid,
                    AlignmentIds.NeutralOutlier => settings.NeutralOutlierValid,
                    _ => true,
                };
        }
    }
}
