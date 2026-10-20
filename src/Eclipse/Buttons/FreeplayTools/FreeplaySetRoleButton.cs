
using Eclipse.Assets;
using Eclipse.Game;
using Eclipse.Freeplay;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.FreeplayTools;

internal sealed class FreeplaySetRoleButton : EclipseActionButton
{

    public override string Name => "Eclipse.Freeplay.SetRole";

    public override float Cooldown => 0.001f;

    public override float InitialCooldown => 0.001f;

    public override ButtonLocation Location => ButtonLocation.BottomLeft;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.FreeplayIcon("FreeplaySetRole", MiraAssets.CustomTeamFile);

    public override bool Enabled(RoleBehaviour? role)
    {
        return FreeplayState.IsActive;
    }

    protected override void OnClick()
    {
        var local = PlayerControl.LocalPlayer;
        local.NetTransform.Halt();

        if (Minigame.Instance)
        {
            return;
        }

        var playerMenu = CustomPlayerMenu.Create();
        playerMenu.Begin(
            player => player.Data != null && !player.Data.IsDead && !player.Data.Disconnected,
            chosen =>
            {
                playerMenu.ForceClose();
                if (chosen == null)
                {
                    return;
                }

                RolePickerMenu.Open(role =>
                {
                    RoleChanger.ChangeRole(chosen, role.Role);
                });
            });
    }
}
