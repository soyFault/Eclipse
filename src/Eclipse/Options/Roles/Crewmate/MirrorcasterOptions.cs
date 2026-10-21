using Eclipse.Core;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Roles.Crewmate;

internal sealed class MirrorcasterOptions : AbstractRoleOptionGroup<MirrorcasterRole>
{
    public override string GroupName => "Eclipse.Role.Mirrorcaster";

    [ModdedEnumOption(
        "Eclipse.Option.Mirrorcaster.WhoGetsNotification",
        typeof(MirrorWhoIsTold),
        ["Eclipse.Option.Mirrorcaster.NotifEnum.Mirrorcaster", "Eclipse.Option.Mirrorcaster.NotifEnum.MirrorcasterAndKiller"])]
    public MirrorWhoIsTold WhoGetsNotification { get; set; } = MirrorWhoIsTold.MirrorcasterAndKiller;

    [ModdedNumberOption("Eclipse.Option.Mirrorcaster.MirrorCooldown", 0f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float MirrorCooldown { get; set; }

    [ModdedNumberOption("Eclipse.Option.Mirrorcaster.MirrorDuration", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float MirrorDuration { get; set; } = 30f;

    [ModdedNumberOption("Eclipse.Option.Mirrorcaster.UnleashCooldown", 0f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float UnleashCooldown { get; set; } = 15f;

    [ModdedEnumOption(
        "Eclipse.Option.Mirrorcaster.AttackInformationGiven",
        typeof(MirrorAttackInfo),
        ["Eclipse.Option.Mirrorcaster.InfoEnum.Role", "Eclipse.Option.Mirrorcaster.InfoEnum.Subalignment", "Eclipse.Option.Mirrorcaster.InfoEnum.Faction", "Eclipse.Option.Mirrorcaster.InfoEnum.Nothing"])]
    public MirrorAttackInfo AttackInformationGiven { get; set; } = MirrorAttackInfo.Subalignment;

    [ModdedToggleOption("Eclipse.Option.Mirrorcaster.AccumulateMultipleUnleashes")]
    public bool MultiUnleash { get; set; }

    [ModdedNumberOption("Eclipse.Option.Mirrorcaster.MaxMirrors", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxMirrors { get; set; } = 5f;
}
