using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Host;

internal sealed class ButtonBarryOptions : AbstractOptionGroup
{
    public override string GroupName => "Eclipse.Options.ButtonBarry";

    public override MenuCategory ParentMenu => MenuCategory.Modifiers;

    public override uint GroupPriority => 2;

    [ModdedNumberOption("Eclipse.Option.ButtonBarry.Cooldown", 2.5f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float Cooldown { get; set; } = 30f;

    [ModdedNumberOption("Eclipse.Option.ButtonBarry.MaxUses", 1f, 3f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxUses { get; set; } = 1f;

    [ModdedToggleOption("Eclipse.Option.ButtonBarry.IgnoreSabotage")]
    public bool IgnoreSabotage { get; set; } = true;

    [ModdedToggleOption("Eclipse.Option.ButtonBarry.FirstRound")]
    public bool FirstRound { get; set; }
}
