using Eclipse.Assets;
using Eclipse.Freeplay;
using Eclipse.Game;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.FreeplayTools;

internal sealed class FreeplaySetModifierButton : EclipseActionButton
{
    public override string Name => "Eclipse.Freeplay.SetModifier";

    public override float Cooldown => 0.001f;

    public override float InitialCooldown => 0.001f;

    public override ButtonLocation Location => ButtonLocation.BottomLeft;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.FreeplayIcon("FreeplaySetModifier", MiraAssets.CustomTeamFile);

    public override bool Enabled(RoleBehaviour? role)
    {
        return FreeplayState.IsActive;
    }

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.NetTransform.Halt();
        if (Minigame.Instance)
        {
            return;
        }

        var menu = CustomPlayerMenu.Create();
        menu.Begin(
            player => player.Data != null && !player.Data.IsDead && !player.Data.Disconnected,
            chosen =>
            {
                menu.ForceClose();
                if (chosen == null)
                {
                    return;
                }

                ModifierPickerMenu.Open(modifier =>
                {
                    if (FreeplayModifiers.TypeIdOf(modifier) is { } typeId)
                    {
                        FreeplayActions.ToggleModifier(chosen, typeId);
                    }
                });
            });
    }
}
