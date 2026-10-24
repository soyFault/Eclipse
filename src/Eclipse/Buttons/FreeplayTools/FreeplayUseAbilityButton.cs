using Eclipse.Game;
using Eclipse.Assets;
using Eclipse.Freeplay;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Buttons.FreeplayTools;

internal sealed class FreeplayUseAbilityButton : CustomActionButton
{
    public override string Name => "Eclipse.Freeplay.UseAbility";

    public override float Cooldown => 0.001f;



    public override float InitialCooldown => 0.001f;

    public override ButtonLocation Location => ButtonLocation.BottomLeft;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.FreeplayIcon("FreeplayUseAbility", MiraAssets.CrewmateFile);

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
            user =>
            {
                menu.ForceClose();
                if (user != null)
                {
                    OnChosen(user);
                }
            });
    }

    private static void OnChosen(PlayerControl user)
    {
        AurialEvents.Tell(user);
    }
}
