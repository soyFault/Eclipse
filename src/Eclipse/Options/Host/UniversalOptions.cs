using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Host;

internal sealed class UniversalOptions : AbstractOptionGroup
{
    public override string GroupName => "Eclipse.Options.Universal";

    public override MenuCategory ParentMenu => MenuCategory.Modifiers;

    public override uint GroupPriority => 1;

    [ModdedNumberOption("Eclipse.Option.Universal.DrunkAmount", 0f, 5f, 1f, MiraNumberSuffixes.None, "0")]
    public float DrunkAmount { get; set; }

    [ModdedNumberOption("Eclipse.Option.Universal.DrunkChance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float DrunkChance { get; set; } = 50f;

    [ModdedNumberOption("Eclipse.Option.Universal.FlashAmount", 0f, 5f, 1f, MiraNumberSuffixes.None, "0")]
    public float FlashAmount { get; set; }

    [ModdedNumberOption("Eclipse.Option.Universal.FlashChance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float FlashChance { get; set; } = 50f;

    [ModdedNumberOption("Eclipse.Option.Universal.FlashSpeed", 1.05f, 2.5f, 0.05f, MiraNumberSuffixes.Multiplier,
        "0.00")]
    public float FlashSpeed { get; set; } = 1.75f;

    [ModdedNumberOption("Eclipse.Option.Universal.GiantAmount", 0f, 5f, 1f, MiraNumberSuffixes.None, "0")]
    public float GiantAmount { get; set; }

    [ModdedNumberOption("Eclipse.Option.Universal.GiantChance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float GiantChance { get; set; } = 50f;

    [ModdedNumberOption("Eclipse.Option.Universal.GiantSpeed", 0.25f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float GiantSpeed { get; set; } = 0.75f;

    [ModdedNumberOption("Eclipse.Option.Universal.MiniAmount", 0f, 5f, 1f, MiraNumberSuffixes.None, "0")]
    public float MiniAmount { get; set; }

    [ModdedNumberOption("Eclipse.Option.Universal.MiniChance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float MiniChance { get; set; } = 50f;

    [ModdedNumberOption("Eclipse.Option.Universal.MiniSpeed", 1.05f, 2.5f, 0.05f, MiraNumberSuffixes.Multiplier,
        "0.00")]
    public float MiniSpeed { get; set; } = 1.35f;

    [ModdedNumberOption("Eclipse.Option.Universal.SixthSenseAmount", 0f, 5f, 1f, MiraNumberSuffixes.None, "0")]
    public float SixthSenseAmount { get; set; }

    [ModdedNumberOption("Eclipse.Option.Universal.SixthSenseChance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float SixthSenseChance { get; set; } = 50f;

    [ModdedNumberOption("Eclipse.Option.Universal.SleuthAmount", 0f, 5f, 1f, MiraNumberSuffixes.None, "0")]
    public float SleuthAmount { get; set; }

    [ModdedNumberOption("Eclipse.Option.Universal.SleuthChance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float SleuthChance { get; set; } = 50f;

    [ModdedNumberOption("Eclipse.Option.Universal.TiebreakerChance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float TiebreakerChance { get; set; } = 50f;

    [ModdedNumberOption("Eclipse.Option.Universal.ButtonBarryChance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float ButtonBarryChance { get; set; } = 50f;

}
