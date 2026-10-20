using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Roles.Crewmate;

internal sealed class MysticOptions : AbstractRoleOptionGroup<MysticRole>
{
    public override string GroupName => "Eclipse.Role.Mystic";

    [ModdedNumberOption("Eclipse.Option.Mystic.ArrowDuration", 0f, 1f, 0.05f, MiraNumberSuffixes.Seconds, "0.00")]
    public float ArrowDuration { get; set; } = 0.1f;
}
