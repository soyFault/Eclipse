using Eclipse.Roles.Crewmate.Killing;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Roles.Crewmate;

internal sealed class VeteranOptions : AbstractRoleOptionGroup<VeteranRole>
{
    public override string GroupName => "Eclipse.Role.Veteran";

    [ModdedNumberOption("Eclipse.Option.Veteran.AlertCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float AlertCooldown { get; set; } = 25f;

    [ModdedNumberOption("Eclipse.Option.Veteran.AlertDuration", 5f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float AlertDuration { get; set; } = 10f;

    [ModdedNumberOption("Eclipse.Option.Veteran.MaxAlerts", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxAlerts { get; set; } = 5f;

    [ModdedToggleOption("Eclipse.Option.Veteran.KilledOnAlert")]
    public bool KilledOnAlert { get; set; }

    public ModdedToggleOption KnowWhenAttacked { get; } = new("Eclipse.Option.Veteran.KnowWhenAttacked", true)
    {
        Visible = () => !OptionGroupSingleton<VeteranOptions>.Instance.KilledOnAlert,
    };

    [ModdedToggleOption("Eclipse.Option.Veteran.TaskUses")]
    public bool TaskUses { get; set; } = true;

    [ModdedToggleOption("Eclipse.Option.Veteran.EveryoneSeesShield")]
    public bool EveryoneSeesShield { get; set; } = true;
}
