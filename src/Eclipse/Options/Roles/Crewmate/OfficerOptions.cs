
using Eclipse.Core;
using Eclipse.Roles.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Roles.Crewmate;

internal sealed class OfficerOptions : AbstractRoleOptionGroup<OfficerRole>
{
    /// <summary>Gets the group header (a locale key; Mira translates it).</summary>
    public override string GroupName => "Eclipse.Role.Officer";

    /// <summary>Gets or sets the time between shots.</summary>
    [ModdedNumberOption("Eclipse.Option.Officer.ShootCooldown", 2.5f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ShootCooldown { get; set; } = 5f;

    /// <summary>Gets or sets the time between loading bullets.</summary>
    [ModdedNumberOption("Eclipse.Option.Officer.LoadCooldown", 2.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float LoadCooldown { get; set; } = 30f;

    /// <summary>Gets or sets a value indicating whether the Officer can report the bodies they leave (when off, those bodies cannot be reported from the Officer's own game).</summary>
    [ModdedToggleOption("Eclipse.Option.Officer.CanSelfReport")]
    public bool CanSelfReport { get; set; }

    /// <summary>Gets or sets a value indicating whether the Officer can load and shoot in the first round.</summary>
    [ModdedToggleOption("Eclipse.Option.Officer.FirstRound")]
    public bool FirstRoundShooting { get; set; }

    /// <summary>Gets or sets a value indicating whether only players who have already killed are valid targets.</summary>
    [ModdedToggleOption("Eclipse.Option.Officer.OnlyActiveKillers")]
    public bool CanOnlyShootActiveKillers { get; set; } = true;

    /// <summary>Gets a value indicating whether Crewmate Killing roles count as innocent (only when only killers can be shot).</summary>
    public ModdedToggleOption CrewKillingAreInnocentOption { get; } = new("Eclipse.Option.Officer.CrewKillingInnocent", false)
    {
        Visible = () => OptionGroupSingleton<OfficerOptions>.Instance.CanOnlyShootActiveKillers,
    };

    /// <summary>Gets a value indicating whether non-killing Neutrals count as innocent (only when anybody guilty-looking can be shot).</summary>
    public ModdedToggleOption NonKillingNeutralsAreInnocentOption { get; } = new("Eclipse.Option.Officer.NeutralsInnocent", false)
    {
        Visible = () => !OptionGroupSingleton<OfficerOptions>.Instance.CanOnlyShootActiveKillers,
    };

    /// <summary>Gets or sets how many bullets can be loaded in total.</summary>
    [ModdedNumberOption("Eclipse.Option.Officer.MaxBulletsTotal", 3f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxBulletsTotal { get; set; } = 9f;

    /// <summary>Gets or sets how many bullets can be loaded at once.</summary>
    [ModdedNumberOption("Eclipse.Option.Officer.MaxBulletsAtOnce", 2f, 9f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxBulletsAtOnce { get; set; } = 3f;

    /// <summary>Gets or sets how many rounds the gun stays confiscated after a misfire.</summary>
    [ModdedNumberOption("Eclipse.Option.Officer.RoundsPunished", 1f, 5f, 1f, MiraNumberSuffixes.None, "0")]
    public float RoundsPunished { get; set; } = 1f;

    /// <summary>Gets a value indicating whether Crewmate Killing roles count as innocent.</summary>
    public bool CrewKillingAreInnocent => CrewKillingAreInnocentOption.Value;

    /// <summary>Gets a value indicating whether non-killing Neutrals count as innocent.</summary>
    public bool NonKillingNeutralsAreInnocent => NonKillingNeutralsAreInnocentOption.Value;

    /// <summary>
    /// Packs the shot-related settings for the pure rules in <see cref="OfficerRules"/>.
    /// </summary>
    /// <returns>The current shot settings.</returns>
    public OfficerShotSettings ToShotSettings()
    {
        return new OfficerShotSettings(CanOnlyShootActiveKillers, CrewKillingAreInnocent, NonKillingNeutralsAreInnocent);
    }
}
