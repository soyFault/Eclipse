using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Networking;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Killing;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Crewmate;


internal sealed class OfficerLoadButton : EclipseActionButton
{

    public override string Name => "Eclipse.Button.OfficerLoad";

    public override float Cooldown => Mathf.Clamp(OptionGroupSingleton<OfficerOptions>.Instance.LoadCooldown, 5f, 120f);

    public override float InitialCooldown => 10f;

    public override int MaxUses => (int)OptionGroupSingleton<OfficerOptions>.Instance.MaxBulletsTotal;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("OfficerLoad", MiraAssets.CrewmateFile);

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.SecondaryAbility;

    public override Color TextOutlineColor => EclipseColors.Officer;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is OfficerRole;
    }

    protected override bool IsRoundLocked => MatchRounds.IsFirstRoundLocked(OptionGroupSingleton<OfficerOptions>.Instance.FirstRoundShooting);

    public override bool CanUse()
    {
        var options = OptionGroupSingleton<OfficerOptions>.Instance;
        return base.CanUse()
               && PlayerControl.LocalPlayer.Data.Role is OfficerRole officer
               && !IsRoundLocked
               && OfficerRules.CanLoad(officer.GunConfiscated, officer.LoadedBullets, (int)options.MaxBulletsAtOnce, officer.BulletsLeft);
    }

    protected override void OnClick()
    {
        if (PlayerControl.LocalPlayer.Data.Role is not OfficerRole officer)
        {
            return;
        }

        officer.LastLoadTime = Time.time;
        OfficerRpcs.RpcOfficerSync(PlayerControl.LocalPlayer, officer.RoundsBeforeReset, officer.BulletsLeft - 1, officer.LoadedBullets + 1);
    }
}
