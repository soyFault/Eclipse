using Eclipse.Core;
using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Roles.Crewmate;

internal sealed class SnitchOptions : AbstractRoleOptionGroup<SnitchRole>
{
    public override string GroupName => "Eclipse.Role.Snitch";

    [ModdedEnumOption(
        "Eclipse.Option.Snitch.RevealsImpostors",
        typeof(SnitchReveal),
        ["Eclipse.Option.Snitch.RevealsImpostors.Self", "Eclipse.Option.Snitch.RevealsImpostors.Everyone"])]
    public SnitchReveal RevealsImpostors { get; set; } = SnitchReveal.Self;

    [ModdedToggleOption("Eclipse.Option.Snitch.SeesImpostorsInMeetings")]
    public bool SeesImpostorsInMeetings { get; set; } = true;

    [ModdedNumberOption("Eclipse.Option.Snitch.TasksRemainingWhenRevealed", 1f, 3f, 1f, MiraNumberSuffixes.None, "0")]
    public float TasksRemainingWhenRevealed { get; set; } = 1f;
}
