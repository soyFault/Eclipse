using Eclipse.Assets;
using Eclipse.Freeplay;
using MiraAPI.Hud;
using Eclipse.Game;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.FreeplayTools;

internal sealed class FreeplayAttackButton : EclipseActionButton
{
    public override string Name => "Eclipse.Freeplay.Attack";

    public override float Cooldown => 0.001f;

    public override float InitialCooldown => 0.001f;

    public override ButtonLocation Location => ButtonLocation.BottomLeft;


    public override LoadableAsset<Sprite> Sprite => EclipseAssets.FreeplayIcon("FreeplayAttack", MiraAssets.CrewmateFile);

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

        var attackerMenu = CustomPlayerMenu.Create();
        attackerMenu.Begin(
            player => IsAlive(player),
            attacker =>
            {
                attackerMenu.ForceClose();
                if (attacker == null)
                {
                    return;
                }

                var victimMenu = CustomPlayerMenu.Create();
                victimMenu.Begin(
                    player => IsAlive(player) && player.PlayerId != attacker.PlayerId,
                    victim =>
                    {
                        victimMenu.ForceClose();
                        if (victim != null)
                        {
                            FreeplayActions.Attack(attacker, victim);
                        }
                    });
            });
    }

    private static bool IsAlive(PlayerControl player)
    {
        return player.Data != null && !player.Data.IsDead && !player.Data.Disconnected;
    }
}
