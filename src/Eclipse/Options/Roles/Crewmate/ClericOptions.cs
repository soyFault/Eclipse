using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Roles.Crewmate;

internal sealed class ClericOptions : AbstractRoleOptionGroup<ClericRole>
{
    public override string GroupName => "Eclipse.Role.Cleric";

    [ModdedNumberOption("Eclipse.Option.Cleric.BarrierCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float BarrierCooldown { get; set; } = 25f;

    [ModdedNumberOption("Eclipse.Option.Cleric.BarrierDuration", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float BarrierDuration { get; set; } = 25f;

    [ModdedNumberOption("Eclipse.Option.Cleric.CleanseCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float CleanseCooldown { get; set; } = 25f;

    [ModdedToggleOption("Eclipse.Option.Cleric.ProtectedSeesBarrier")]
    public bool ShowBarrier { get; set; }

    [ModdedToggleOption("Eclipse.Option.Cleric.AttackNotification")]
    public bool AttackNotification { get; set; } = true;
}
