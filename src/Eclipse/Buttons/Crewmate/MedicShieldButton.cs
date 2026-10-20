using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Networking;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Crewmate;


internal sealed class MedicShieldButton : EclipsePlayerButton<MedicRole>
{
    public override string Name => "Eclipse.Button.MedicShield";

    public override float Cooldown => 0.001f;

    public override float InitialCooldown => 10f;

    public override int MaxUses => OptionGroupSingleton<MedicOptions>.Instance.ChangeTarget
        ? (int)OptionGroupSingleton<MedicOptions>.Instance.ShieldUses.Value
        : 1;

    public override bool ZeroIsInfinite { get; set; } = true;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("MedicShield", MiraAssets.CrewmateFile);

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.SecondaryAbility;

    public override Color TextOutlineColor => EclipseColors.Medic;

    public override bool CanUse()
    {
        var local = PlayerControl.LocalPlayer;
        var canMove = local.Data.Role is MedicRole { Shielded: null } or MedicRole { CanChangeTarget: true };
        return base.CanUse() && canMove;
    }

    public override bool IsTargetValid(PlayerControl? target)
    {
        return base.IsTargetValid(target) && PlayerControl.LocalPlayer.Data.Role is MedicRole role && target != role.Shielded;
    }

    protected override void OnClick()
    {
        var local = PlayerControl.LocalPlayer;
        if (Target == null || local.Data.Role is not MedicRole role)
        {
            return;
        }

        MedicRpcs.RpcMedicShield(local, Target);
        role.CanChangeTarget = false;
        ResetTarget();
    }
}
