using Eclipse.Core;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace Eclipse.Options.Host;

internal sealed partial class VanillaTweakOptions : AbstractOptionGroup
{

public override string GroupName => "Eclipse.Options.VanillaTweaks";

    public override uint GroupPriority => 2;

    [ModdedEnumOption(
        "Eclipse.Option.Tweak.HideNames",
        typeof(NameSight),
        ["Eclipse.Option.Tweak.HideNames.Basic", "Eclipse.Option.Tweak.HideNames.RoomBased", "Eclipse.Option.Tweak.HideNames.Harsh"])]
    public NameSight HideNamesOutOfSight { get; set; } = NameSight.Basic;
}
