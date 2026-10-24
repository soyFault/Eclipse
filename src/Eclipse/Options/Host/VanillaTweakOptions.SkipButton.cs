using Eclipse.Core;
using MiraAPI.GameOptions.Attributes;

namespace Eclipse.Options.Host;

internal sealed partial class VanillaTweakOptions
{
    [ModdedEnumOption(
        "Eclipse.Option.Tweak.SkipButton",
        typeof(SkipState),
        ["Eclipse.Option.Tweak.SkipButton.Never", "Eclipse.Option.Tweak.SkipButton.Emergency", "Eclipse.Option.Tweak.SkipButton.Always"])]
    public SkipState SkipButtonDisable { get; set; } = SkipState.No;
}
