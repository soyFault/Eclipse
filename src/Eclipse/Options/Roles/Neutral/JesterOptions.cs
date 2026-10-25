using Eclipse.Core;
using Eclipse.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;

namespace Eclipse.Options.Roles.Neutral;

internal sealed class JesterOptions : AbstractRoleOptionGroup<JesterRole>
{
    public override string GroupName => "Eclipse.Role.Jester";

    [ModdedToggleOption("Eclipse.Option.Jester.CanButton")]
    public bool CanButton { get; set; } = true;

    [ModdedEnumOption(
        "Eclipse.Option.Jester.AfterWin",
        typeof(JestWin),
        ["Eclipse.Option.Jester.AfterWin.EndsGame", "Eclipse.Option.Jester.AfterWin.Haunts", "Eclipse.Option.Jester.AfterWin.Nothing"])]
    public JestWin AfterWin { get; set; } = JestWin.EndsGame;

    public ModdedToggleOption AnnounceWin { get; } = new("Eclipse.Option.Jester.AnnounceWin", true)
    {
        Visible = () => OptionGroupSingleton<JesterOptions>.Instance.AfterWin != JestWin.EndsGame,
    };
}
