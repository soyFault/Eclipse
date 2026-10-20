using Eclipse.Core;
using Eclipse.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Roles.Crewmate;


internal sealed class MedicOptions : AbstractRoleOptionGroup<MedicRole>
{
    public override string GroupName => "Eclipse.Role.Medic";

    [ModdedToggleOption("Eclipse.Option.Medic.ShieldedKnows")]
    public bool ShieldedKnows { get; set; } = true;

    [ModdedEnumOption(
        "Eclipse.Option.Medic.WhoGetsNotification",
        typeof(ShieldNotification),
        ["Eclipse.Option.Medic.Audience.Medic", "Eclipse.Option.Medic.Audience.Shielded", "Eclipse.Option.Medic.Audience.ShieldedAndMedic", "Eclipse.Option.Medic.Audience.Nobody", "Eclipse.Option.Medic.Audience.Everyone"])]
    public ShieldNotification WhoGetsNotification { get; set; } = ShieldNotification.Medic;

    [ModdedToggleOption("Eclipse.Option.Medic.ChangeTarget")]
    public bool ChangeTarget { get; set; } = true;

    public ModdedNumberOption ShieldUses { get; } = new("Eclipse.Option.Medic.ShieldUses", 3f, 0f, 15f, 1f, "∞", "#", MiraNumberSuffixes.None, "0", false)
    {
        Visible = () => OptionGroupSingleton<MedicOptions>.Instance.ChangeTarget,
    };

    [ModdedToggleOption("Eclipse.Option.Medic.ShieldBreaks")]
    public bool ShieldBreaks { get; set; }

    [ModdedToggleOption("Eclipse.Option.Medic.ShowReports")]
    public bool ShowReports { get; set; } = true;

    public ModdedNumberOption ReportNameDuration { get; } = new("Eclipse.Option.Medic.ReportNameDuration", 0f, 0f, 60f, 2.5f, MiraNumberSuffixes.Seconds)
    {
        Visible = () => OptionGroupSingleton<MedicOptions>.Instance.ShowReports,
    };

    public ModdedNumberOption ReportColorDuration { get; } = new("Eclipse.Option.Medic.ReportColorDuration", 15f, 0f, 60f, 2.5f, MiraNumberSuffixes.Seconds, "0.#", true)
    {
        Visible = () => OptionGroupSingleton<MedicOptions>.Instance.ShowReports,
    };
}
