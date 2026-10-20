using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Roles.Crewmate;

internal sealed class SonarOptions : AbstractRoleOptionGroup<SonarRole>
{
    public override string GroupName => "Eclipse.Role.Sonar";

    [ModdedNumberOption("Eclipse.Option.Sonar.TrackCooldown", 1f, 30f, 1f, MiraNumberSuffixes.Seconds)]
    public float TrackCooldown { get; set; } = 20f;

    [ModdedNumberOption("Eclipse.Option.Sonar.MaxTracks", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxTracks { get; set; } = 5f;

    [ModdedNumberOption("Eclipse.Option.Sonar.UpdateInterval", 0f, 15f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float UpdateInterval { get; set; } = 5f;

    [ModdedToggleOption("Eclipse.Option.Sonar.ResetOnNewRound")]
    public bool ResetOnNewRound { get; set; } = true;

    public ModdedToggleOption TaskUses { get; } = new("Eclipse.Option.Sonar.TaskUses", false)
    {
        Visible = () => !OptionGroupSingleton<SonarOptions>.Instance.ResetOnNewRound,
    };
}
