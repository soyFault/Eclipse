using Eclipse.Core;
using Eclipse.Roles.Crewmate.Killing;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Roles.Crewmate;

internal sealed class SheriffOptions : AbstractRoleOptionGroup<SheriffRole>
{

    public override string GroupName => "Eclipse.Role.Sheriff";

    [ModdedNumberOption("Eclipse.Option.Sheriff.KillCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 25f;

    [ModdedToggleOption("Eclipse.Option.Sheriff.FirstRoundUse")]
    public bool FirstRoundUse { get; set; }

    [ModdedToggleOption("Eclipse.Option.Sheriff.CanSelfReport")]
    public bool CanSelfReport { get; set; }

    [ModdedToggleOption("Eclipse.Option.Sheriff.CommsDisables")]
    public bool CommsDisables { get; set; }

    [ModdedToggleOption("Eclipse.Option.Sheriff.ShootNeutralBenign")]
    public bool ShootNeutralBenign { get; set; }

    [ModdedToggleOption("Eclipse.Option.Sheriff.ShootNeutralEvil")]
    public bool ShootNeutralEvil { get; set; } = true;

    [ModdedToggleOption("Eclipse.Option.Sheriff.ShootNeutralKilling")]
    public bool ShootNeutralKilling { get; set; } = true;

    [ModdedToggleOption("Eclipse.Option.Sheriff.ShootNeutralOutlier")]
    public bool ShootNeutralOutlier { get; set; } = true;

    [ModdedEnumOption(
        "Eclipse.Option.Sheriff.MisfireKills",
        typeof(MisfireKills),
        ["Eclipse.Option.Sheriff.MisfireKills.Sheriff", "Eclipse.Option.Sheriff.MisfireKills.Target", "Eclipse.Option.Sheriff.MisfireKills.Both", "Eclipse.Option.Sheriff.MisfireKills.Nobody"])]
    public MisfireKills Misfire { get; set; } = MisfireKills.Sheriff;

    public SheriffShotSettings ToShotSettings()
    {
        return new SheriffShotSettings(ShootNeutralBenign, ShootNeutralEvil, ShootNeutralKilling, ShootNeutralOutlier, Misfire);
    }
}
