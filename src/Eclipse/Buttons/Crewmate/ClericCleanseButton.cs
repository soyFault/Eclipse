using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Networking;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Crewmate;

internal sealed class ClericCleanseButton : EclipsePlayerButton<ClericRole>
{
    public override string Name => "Eclipse.Button.ClericCleanse";

    public override float Cooldown => Mathf.Clamp(OptionGroupSingleton<ClericOptions>.Instance.CleanseCooldown, 5f, 120f);

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("ClericCleanseButton", MiraAssets.CrewmateFile);

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.PrimaryAbility;

    public override Color TextOutlineColor => EclipseColors.Cleric;

    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }

        ClericRpcs.RpcClericCleanse(PlayerControl.LocalPlayer, Target);
        CustomButtonSingleton<ClericBarrierButton>.Instance.ResetCooldownAndOrEffect();
    }
}
