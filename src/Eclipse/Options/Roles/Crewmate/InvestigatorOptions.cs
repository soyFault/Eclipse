using Eclipse.Core;
using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Roles.Crewmate;

internal sealed class InvestigatorOptions : AbstractRoleOptionGroup<InvestigatorRole>
{
    public override string GroupName => "Eclipse.Role.Investigator";

    [ModdedEnumOption(
        "Eclipse.Option.Investigator.FootprintMode",
        typeof(FootprintMode),
        ["Eclipse.Option.Investigator.FootprintMode.Distance", "Eclipse.Option.Investigator.FootprintMode.Time"])]
    public FootprintMode Mode { get; set; } = FootprintMode.Distance;

    public ModdedNumberOption IntervalDistance { get; } = new("Eclipse.Option.Investigator.IntervalDistance", 0.5f, 0.25f, 3f, 0.5f, MiraNumberSuffixes.None)
    {
        Visible = () => OptionGroupSingleton<InvestigatorOptions>.Instance.Mode == FootprintMode.Distance,
    };

    public ModdedNumberOption IntervalTime { get; } = new("Eclipse.Option.Investigator.IntervalTime", 1f, 0.5f, 6f, 0.5f, MiraNumberSuffixes.Seconds)
    {
        Visible = () => OptionGroupSingleton<InvestigatorOptions>.Instance.Mode == FootprintMode.Time,
    };

    [ModdedNumberOption("Eclipse.Option.Investigator.FootprintSize", 1f, 10f, 1f, MiraNumberSuffixes.Multiplier)]
    public float FootprintSize { get; set; } = 4f;

    [ModdedNumberOption("Eclipse.Option.Investigator.FootprintDuration", 1f, 15f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float FootprintDuration { get; set; } = 10f;

    [ModdedToggleOption("Eclipse.Option.Investigator.Anonymous")]
    public bool ShowAnonymousFootprints { get; set; }

    [ModdedToggleOption("Eclipse.Option.Investigator.NearVents")]
    public bool ShowFootprintVent { get; set; }

    public float Interval => Mode == FootprintMode.Distance ? IntervalDistance.Value : IntervalTime.Value;
}
