using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Networking;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Crewmate;

internal sealed class ClericBarrierButton : EclipsePlayerButton<ClericRole>
{
    public override string Name => "Eclipse.Button.ClericBarrier";

    public override float Cooldown => Mathf.Clamp(OptionGroupSingleton<ClericOptions>.Instance.BarrierCooldown, 5f, 120f);

    public override float EffectDuration => OptionGroupSingleton<ClericOptions>.Instance.BarrierDuration;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("ClericBarrierButton", MiraAssets.CrewmateFile);

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.SecondaryAbility;

    public override Color TextOutlineColor => EclipseColors.Cleric;

    public override bool IsTargetValid(PlayerControl? target)
    {
        return base.IsTargetValid(target) && !Protections.Ledger.Has(ProtectionKind.Barrier, target!.PlayerId, Protections.Now);
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }

        ClericRpcs.RpcClericBarrier(PlayerControl.LocalPlayer, Target);
        CustomButtonSingleton<ClericCleanseButton>.Instance.ResetCooldownAndOrEffect();
    }
}
